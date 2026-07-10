namespace api.Utils.Metrics
{
    /// <summary>A single recorded request, kept in the in-memory ring buffer.</summary>
    public record RequestSample(
        DateTime TimestampUtc,
        string Method,
        string Path,
        int StatusCode,
        long ResponseBytes);

    /// <summary>Aggregate of the current hour, drained by the background flusher.</summary>
    public record HourAggregate(
        long RequestCount,
        long TotalResponseBytes,
        long MaxResponseBytes,
        string? LargestRequestPath);

    /// <summary>
    /// Thread-safe, in-memory request analytics. Updated on the hot path (O(1),
    /// no I/O) by <see cref="RequestMetricsMiddleware"/> and drained hourly by
    /// <see cref="RequestMetricsFlusher"/>. Registered as a singleton.
    /// </summary>
    public class RequestMetricsCollector
    {
        private const int RecentCapacity = 100;

        private readonly object _gate = new();
        private readonly RequestSample[] _recent = new RequestSample[RecentCapacity];
        private int _recentCount;
        private int _recentHead; // index of the next slot to write

        // Current-hour running totals.
        private long _hourCount;
        private long _hourTotalBytes;
        private long _hourMaxBytes;
        private string? _hourMaxPath;

        public void Record(RequestSample sample)
        {
            lock (_gate)
            {
                _recent[_recentHead] = sample;
                _recentHead = (_recentHead + 1) % RecentCapacity;
                if (_recentCount < RecentCapacity)
                {
                    _recentCount++;
                }

                _hourCount++;
                _hourTotalBytes += sample.ResponseBytes;
                if (sample.ResponseBytes > _hourMaxBytes)
                {
                    _hourMaxBytes = sample.ResponseBytes;
                    _hourMaxPath = sample.Path;
                }
            }
        }

        /// <summary>Most recent requests, newest first.</summary>
        public IReadOnlyList<RequestSample> GetRecent()
        {
            lock (_gate)
            {
                var result = new List<RequestSample>(_recentCount);
                for (int i = 0; i < _recentCount; i++)
                {
                    // Walk backwards from the most recently written slot.
                    int idx = (_recentHead - 1 - i + RecentCapacity) % RecentCapacity;
                    result.Add(_recent[idx]);
                }
                return result;
            }
        }

        /// <summary>Snapshot of the current (not-yet-flushed) hour.</summary>
        public HourAggregate PeekCurrentHour()
        {
            lock (_gate)
            {
                return new HourAggregate(_hourCount, _hourTotalBytes, _hourMaxBytes, _hourMaxPath);
            }
        }

        /// <summary>
        /// Atomically read and reset the current-hour totals. Returns null when
        /// there is nothing to persist (no requests since the last drain).
        /// </summary>
        public HourAggregate? DrainCurrentHour()
        {
            lock (_gate)
            {
                if (_hourCount == 0)
                {
                    return null;
                }

                var aggregate = new HourAggregate(_hourCount, _hourTotalBytes, _hourMaxBytes, _hourMaxPath);
                _hourCount = 0;
                _hourTotalBytes = 0;
                _hourMaxBytes = 0;
                _hourMaxPath = null;
                return aggregate;
            }
        }
    }
}
