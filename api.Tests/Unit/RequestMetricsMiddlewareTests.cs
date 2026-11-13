using System.Text;
using api.Utils.Metrics;
using Microsoft.AspNetCore.Http;

namespace api.Tests.Unit;

/// <summary>
/// Through the test host only one of the counting stream's four write overloads is ever hit, so a
/// regression in the others — under-counting bytes, or not forwarding them — would go unnoticed.
/// These drive the middleware directly with a <see cref="DefaultHttpContext"/>.
/// </summary>
public class RequestMetricsMiddlewareTests
{
    private static readonly byte[] Payload = Encoding.UTF8.GetBytes("hello world");

    private static (DefaultHttpContext Context, MemoryStream Body) NewContext(string? path = "/api/v1/City")
    {
        var body = new MemoryStream();
        var context = new DefaultHttpContext();
        context.Request.Method = "GET";
        if (path is not null)
        {
            context.Request.Path = path;
        }
        context.Response.Body = body;
        return (context, body);
    }

    private static async Task<(RequestSample Sample, MemoryStream Body, DefaultHttpContext Context)> Run(
        RequestDelegate next, string? path = "/api/v1/City")
    {
        var collector = new RequestMetricsCollector();
        var (context, body) = NewContext(path);

        await new RequestMetricsMiddleware(next, collector).InvokeAsync(context);

        return (Assert.Single(collector.GetRecent()), body, context);
    }

    public static TheoryData<string> WriteOverloads() =>
        ["array", "span", "arrayAsync", "memoryAsync"];

    [Theory]
    [MemberData(nameof(WriteOverloads))]
    public async Task EveryWriteOverload_IsCountedAndForwarded(string overload)
    {
        var (sample, body, _) = await Run(async ctx =>
        {
            var stream = ctx.Response.Body;
            switch (overload)
            {
                case "array": stream.Write(Payload, 0, Payload.Length); break;
                case "span": stream.Write(Payload.AsSpan()); break;
                case "arrayAsync": await stream.WriteAsync(Payload, 0, Payload.Length); break;
                case "memoryAsync": await stream.WriteAsync(Payload.AsMemory()); break;
            }
        });

        Assert.Equal(Payload.Length, sample.ResponseBytes);
        Assert.Equal(Payload, body.ToArray());
    }

    [Fact]
    public async Task MultipleWrites_AreSummed()
    {
        var (sample, _, _) = await Run(async ctx =>
        {
            await ctx.Response.Body.WriteAsync(Payload);
            ctx.Response.Body.Write(Payload, 0, 5);
        });

        Assert.Equal(Payload.Length + 5, sample.ResponseBytes);
    }

    [Fact]
    public async Task RecordsMethodPathAndStatus()
    {
        var (sample, _, _) = await Run(ctx =>
        {
            ctx.Response.StatusCode = 404;
            return Task.CompletedTask;
        });

        Assert.Equal("GET", sample.Method);
        Assert.Equal("/api/v1/City", sample.Path);
        Assert.Equal(404, sample.StatusCode);
        Assert.Equal(0, sample.ResponseBytes);
    }

    [Fact]
    public async Task EmptyPath_IsRecordedAsRoot()
    {
        var (sample, _, _) = await Run(_ => Task.CompletedTask, path: null);

        Assert.Equal("/", sample.Path);
    }

    [Fact]
    public async Task RestoresTheOriginalBodyStream()
    {
        var (_, body, context) = await Run(_ => Task.CompletedTask);

        Assert.Same(body, context.Response.Body);
    }

    [Fact]
    public async Task WhenTheNextDelegateThrows_StillRecordsAndRestoresTheBody()
    {
        var collector = new RequestMetricsCollector();
        var (context, body) = NewContext();
        var middleware = new RequestMetricsMiddleware(async ctx =>
        {
            await ctx.Response.Body.WriteAsync(Payload);
            throw new InvalidOperationException("boom");
        }, collector);

        await Assert.ThrowsAsync<InvalidOperationException>(() => middleware.InvokeAsync(context));

        Assert.Same(body, context.Response.Body);
        Assert.Equal(Payload.Length, Assert.Single(collector.GetRecent()).ResponseBytes);
    }

    [Fact]
    public async Task CountingStream_IsWriteOnlyAndForwardsFlush()
    {
        await Run(async ctx =>
        {
            var stream = ctx.Response.Body;

            Assert.False(stream.CanRead);
            Assert.False(stream.CanSeek);
            Assert.True(stream.CanWrite);
            Assert.Throws<NotSupportedException>(() => stream.Length);
            Assert.Throws<NotSupportedException>(() => stream.Position);
            Assert.Throws<NotSupportedException>(() => stream.Position = 0);
            Assert.Throws<NotSupportedException>(() => stream.Read(new byte[1], 0, 1));
            Assert.Throws<NotSupportedException>(() => stream.Seek(0, SeekOrigin.Begin));
            Assert.Throws<NotSupportedException>(() => stream.SetLength(0));

            stream.Flush();
            await stream.FlushAsync();
        });
    }
}
