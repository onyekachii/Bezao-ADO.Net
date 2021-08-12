using System;

namespace BEZAOPayDAL.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Mode { get; set; }

        public decimal Amount {get; set; }

        public DateTime Time { get; set; }
    }
}