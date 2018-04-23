using Cassandra;
using System;
using System.Collections.Generic;

namespace ConnectWithCassandra.Models
{
    [Serializable]
    public class itemHistory
    {
        public string businessUnit { get; set; }

        public string location { get; set; }

        public string upc { get; set; }

        public string identifierValue { get; set; }

        public string identifierType { get; set; }

        public int availableCount { get; set; }

        public Guid serialNumber { get; set; }

        public Guid histItemId { get; set; }

        public string status { get; set; }

        public Dictionary<string, string> itemAttributes { get; set; }

        public Guid receiptId { get; set; }

        public TimeUuid dateCreated { get; set; }

        public TimeUuid? dateUpdated { get; set; }
    }
}
