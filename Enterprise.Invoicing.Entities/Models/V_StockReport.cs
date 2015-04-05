using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_StockReport
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
        public string depotName { get; set; }
        public string materialName { get; set; }
        public string materialModel { get; set; }
        public string category { get; set; }
        public string unit { get; set; }
        public string unit2 { get; set; }
        public string fastcode { get; set; }
        public string pinyin { get; set; }
        public string tunumber { get; set; }
        public int xslength { get; set; }
    }
}
