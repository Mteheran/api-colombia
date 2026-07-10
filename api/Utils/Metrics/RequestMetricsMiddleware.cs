namespace api.Utils.Metrics
{
    /// <summary>
    /// Captures per-request analytics (path, status, response bytes) into the
    /// in-memory <see cref="RequestMetricsCollector"/>. Wraps the response body
    /// in a lightweight counting stream so the recorded size reflects the actual
    /// bytes sent (compressed, when compression applies). No I/O on the hot path.
    /// </summary>
    public class RequestMetricsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RequestMetricsCollector _collector;

        public RequestMetricsMiddleware(RequestDelegate next, RequestMetricsCollector collector)
        {
            _next = next;
            _collector = collector;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var originalBody = context.Response.Body;
            var counting = new CountingStream(originalBody);
            context.Response.Body = counting;

            try
            {
                await _next(context);
            }
            finally
            {
                context.Response.Body = originalBody;

                _collector.Record(new RequestSample(
                    TimestampUtc: DateTime.UtcNow,
                    Method: context.Request.Method,
                    Path: context.Request.Path.HasValue ? context.Request.Path.Value! : "/",
                    StatusCode: context.Response.StatusCode,
                    ResponseBytes: counting.BytesWritten));
            }
        }

        /// <summary>Pass-through stream that counts bytes written to the inner stream.</summary>
        private sealed class CountingStream : Stream
        {
            private readonly Stream _inner;

            public CountingStream(Stream inner) => _inner = inner;

            public long BytesWritten { get; private set; }

            public override bool CanRead => false;
            public override bool CanSeek => false;
            public override bool CanWrite => true;
            public override long Length => throw new NotSupportedException();
            public override long Position
            {
                get => throw new NotSupportedException();
                set => throw new NotSupportedException();
            }

            public override void Flush() => _inner.Flush();
            public override Task FlushAsync(CancellationToken cancellationToken) => _inner.FlushAsync(cancellationToken);

            public override void Write(byte[] buffer, int offset, int count)
            {
                BytesWritten += count;
                _inner.Write(buffer, offset, count);
            }

            public override void Write(ReadOnlySpan<byte> buffer)
            {
                BytesWritten += buffer.Length;
                _inner.Write(buffer);
            }

            public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            {
                BytesWritten += count;
                return _inner.WriteAsync(buffer, offset, count, cancellationToken);
            }

            public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
            {
                BytesWritten += buffer.Length;
                return _inner.WriteAsync(buffer, cancellationToken);
            }

            public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();
            public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
            public override void SetLength(long value) => throw new NotSupportedException();
        }
    }
}
