using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Invoicing.ViewModel
{
    [Serializable]
    public class NeedPayModel
    {
        public int supplierId { get; set; }
        public string supplierName { get; set; }

       public double amount { get; set; }
        public double cost { get; set; }

        public System.DateTime dateStart { get; set; }
        public System.DateTime dateEnd { get; set; }
    }
    [Serializable]
    public class NeedPayDetailModel
    {
        public string purchaseNo { get; set; }
        public string stockInNo { get; set; }

        public double amount { get; set; }
        public double cost { get; set; }
        public double allcost { get; set; }

        public DateTime inDate { get; set; }
        //public System.DateTime inDate { get; set; }
        public string remark { get; set; }
    }

    [Serializable]
    public class V_StockInPurchase
    {
        public string stockInNo { get; set; }
        public int supplierId { get; set; }
        public DateTime createDate { get; set; }
        public string materialNo { get; set; }
        public string purchaseNo { get; set; }
        public int purchaseDetailSn { get; set; }
        public string supplierName { get; set; }
        public string category { get; set; }
        public string materialModel { get; set; }
        public string materialName { get; set; }
        public string tunumber { get; set; }
        public string unit { get; set; }
        public string pinyin { get; set; }
        public string fastcode { get; set; }

        public string person { get; set; }

        public double poPrice { get; set; }
        public double poAmount { get; set; }
        public double inAmount { get; set; }
        public double returnAmount { get; set; }
        public double poCost { get; set; }
        public double inCost { get; set; }
        public double returnCost { get; set; }

    }

}
