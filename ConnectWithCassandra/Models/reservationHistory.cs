using Cassandra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectWithCassandra.Models
{
    public class reservationHistory
    {
        public string businessUnit { get; set; }

        public string reserverId { get; set; }

        public Guid histReservationId { get; set; }

        public string location { get; set; }

        public string upc { get; set; }

        public Guid reservationId { get; set; }
        
        public string reservationStatus { get; set; }

        public Guid serialNumber { get; set; }

        public string identifierValue { get; set; }

        public string identifierType { get; set; }

        public string itemStatus { get; set; }

        public Dictionary<string, string> itemAttributes { get; set; }

        public Guid receiptId { get; set; }

        public TimeUuid dateCreated { get; set; }

        public TimeUuid? dateUpdated { get; set; }
    }
}
