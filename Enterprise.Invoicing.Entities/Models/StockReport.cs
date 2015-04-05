using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class StockReport
    {
        public int Id { get; set; }
        public string stockMonth { get; set; }
        public int depotId { get; set; }
        public string materialNo { get; set; }
        public decimal startAmount { get; set; }
        public decimal startCost { get; set; }
        public decimal inAmount { get; set; }
        public decimal inCost { get; set; }
        public decimal outAmount { get; set; }
        public decimal outCost { get; set; }
        public decimal endAmount { get; set; }
        public decimal endCost { get; set; }
        public System.DateTime createDate { get; set; }
    }
}
