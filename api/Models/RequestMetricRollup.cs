namespace api.Models
{
    /// <summary>
    /// Hourly aggregate of API traffic, written once per hour by the metrics
    /// background service. Durable counterpart to the in-memory collector so
    /// monthly totals survive restarts and scale-out.
    /// </summary>
    public class RequestMetricRollup
    {
        public int Id { get; set; }

        /// <summary>Start of the UTC hour this rollup covers (minute/second = 0).</summary>
        public DateTime HourBucketUtc { get; set; }

        /// <summary>Number of requests recorded during the hour.</summary>
        public long RequestCount { get; set; }

        /// <summary>Sum of response bytes during the hour (for egress estimates).</summary>
        public long TotalResponseBytes { get; set; }

        /// <summary>Largest single response (in bytes) seen during the hour.</summary>
        public long MaxResponseBytes { get; set; }

        /// <summary>Path of the largest response seen during the hour.</summary>
        public string? LargestRequestPath { get; set; }
    }
}
