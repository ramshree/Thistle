using Cassandra;
using System;

namespace ConnectWithCassandra.Models
{
    [Serializable]
    public class receipt
    {
        public string businessUnit { get; set; }

        public Guid receiptId { get; set; }

        public string user { get; set; }

        public string domain { get; set; }

        public string receiptType { get; set; }

        public TimeUuid  dateCreated { get; set; }

        public TimeUuid? dateUpdate { get; set; }
    }
}
