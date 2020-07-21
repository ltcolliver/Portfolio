using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Transfer
    {
        public int ID { get; set; } // May need to be nullable?

        public string Type { get; set; } = "Send";
        public string Status { get; set; } = "Approved";
        public decimal Amount { get; set; }
        public int FromUserID { get; set; }
        public int ToUserID { get; set; }
    }

    public class TransferDetails
    {
        public int ID { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
    }

    public class TransferHistory
    {
        public int TransferID { get; set; }
        public string Username { get; set; }
        public string Direction { get; set; } // ("From" "To")
        public decimal Amount { get; set; }
    }
}
