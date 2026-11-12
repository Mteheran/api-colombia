using api.Utils.Metrics;

namespace api.Tests.Unit;

/// <summary>
/// The collector was only ever exercised sideways through the metrics endpoint, which never fills
/// the 100-slot ring buffer nor drains the hour. These pin the buffer wrap-around, newest-first
/// ordering and the drain/reset semantics the hourly flusher depends on.
/// </summary>
public class RequestMetricsCollectorTests
{
    private const int Capacity = 100;

    private static RequestSample Sample(string path, long bytes = 10) =>
        new(DateTime.UtcNow, "GET", path, 200, bytes);

    [Fact]
    public void NewCollector_IsEmpty()
    {
        var collector = new RequestMetricsCollector();

        Assert.Empty(collector.GetRecent());
        Assert.Equal(new HourAggregate(0, 0, 0, null), collector.PeekCurrentHour());
        Assert.Null(collector.DrainCurrentHour());
    }

    [Fact]
    public void GetRecent_ReturnsNewestFirst()
    {
        var collector = new RequestMetricsCollector();

        collector.Record(Sample("/a"));
        collector.Record(Sample("/b"));
        collector.Record(Sample("/c"));

        Assert.Equal(["/c", "/b", "/a"], collector.GetRecent().Select(s => s.Path));
    }

    [Fact]
    public void GetRecent_AfterTheBufferWraps_KeepsOnlyTheLatestHundred()
    {
        var collector = new RequestMetricsCollector();

        for (var i = 0; i < Capacity + 25; i++)
        {
            collector.Record(Sample($"/{i}"));
        }

        var recent = collector.GetRecent();

        Assert.Equal(Capacity, recent.Count);
        Assert.Equal($"/{Capacity + 24}", recent[0].Path);
        Assert.Equal("/25", recent[^1].Path);
    }

    [Fact]
    public void PeekCurrentHour_AggregatesCountBytesAndLargestPath()
    {
        var collector = new RequestMetricsCollector();

        collector.Record(Sample("/small", 5));
        collector.Record(Sample("/large", 500));
        collector.Record(Sample("/medium", 50));

        Assert.Equal(new HourAggregate(3, 555, 500, "/large"), collector.PeekCurrentHour());
    }

    [Fact]
    public void PeekCurrentHour_OnATie_KeepsTheFirstPathSeen()
    {
        var collector = new RequestMetricsCollector();

        collector.Record(Sample("/first", 100));
        collector.Record(Sample("/second", 100));

        Assert.Equal("/first", collector.PeekCurrentHour().LargestRequestPath);
    }

    [Fact]
    public void PeekCurrentHour_DoesNotReset()
    {
        var collector = new RequestMetricsCollector();
        collector.Record(Sample("/a"));

        collector.PeekCurrentHour();

        Assert.Equal(1, collector.PeekCurrentHour().RequestCount);
    }

    [Fact]
    public void DrainCurrentHour_ReturnsTheTotalsAndResetsThem()
    {
        var collector = new RequestMetricsCollector();
        collector.Record(Sample("/a", 7));
        collector.Record(Sample("/b", 3));

        var drained = collector.DrainCurrentHour();

        Assert.Equal(new HourAggregate(2, 10, 7, "/a"), drained);
        Assert.Equal(new HourAggregate(0, 0, 0, null), collector.PeekCurrentHour());
        Assert.Null(collector.DrainCurrentHour());
    }

    [Fact]
    public void DrainCurrentHour_LeavesTheRecentListIntact()
    {
        var collector = new RequestMetricsCollector();
        collector.Record(Sample("/a"));

        collector.DrainCurrentHour();

        Assert.Single(collector.GetRecent());
    }

    [Fact]
    public async Task Record_FromManyThreads_LosesNothing()
    {
        var collector = new RequestMetricsCollector();
        const int threads = 8, perThread = 500;

        await Task.WhenAll(Enumerable.Range(0, threads).Select(_ => Task.Run(() =>
        {
            for (var i = 0; i < perThread; i++)
            {
                collector.Record(Sample("/x", 1));
            }
        })));

        var hour = collector.PeekCurrentHour();
        Assert.Equal(threads * perThread, hour.RequestCount);
        Assert.Equal(threads * perThread, hour.TotalResponseBytes);
        Assert.Equal(Capacity, collector.GetRecent().Count);
    }
}
