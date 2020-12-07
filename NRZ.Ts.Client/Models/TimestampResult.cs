using Disig.TimeStampClient;
using System;

namespace NRZ.Ts.Client.Models
{
    public class TimestampResult
    {
        public long Id { get; set; }

        public byte[] FileBytes { get; set; }

        /// <summary>
        /// Serialized timestamp respose <see cref="TimeStampResponse"/>
        /// </summary>
        public byte[] Tsr { get; set; }

        /// <summary>
        /// Serialized timestamp request <see cref="Request"/>
        /// </summary>
        public byte[] Tsq { get; set; }

        public string FileName { get; set; }

        /// <summary>
        /// Local (Server) time of timestamping
        /// </summary>
        public DateTime TimeLocal { get; set; }

        /// <summary>
        /// UTC time of timestamping
        /// </summary>
        public DateTime TimeUTC { get; set; }

        /// <summary>
        /// Timestamp serial number
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// Timestamp policy
        /// </summary>
        public string Policy { get; set; }

        /// <summary>
        /// Timestamp authority name
        /// </summary>
        public string TSAName { get; set; }

        public long Nonce { get; set; }

        public int Seconds { get; set; }

        public int Millis { get; set; }

        public int Micros { get; set; }

        public TimeStampToken TimeStampResponse { get; set; }
        public Request TimeStampRequest { get; set; }
    }
}
