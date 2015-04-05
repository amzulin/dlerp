using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Enterprise.Invoicing.Entities.Models.Mapping;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class InvoicingContext : DbContext
    {
        static InvoicingContext()
        {
            Database.SetInitializer<InvoicingContext>(null);
        }

        public InvoicingContext()
            : base("Name=InvoicingContext")
        {
        }

        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<BillCost> BillCosts { get; set; }
        public DbSet<BillCostDetail> BillCostDetails { get; set; }
        public DbSet<BomCost> BomCosts { get; set; }
        public DbSet<BomMain> BomMains { get; set; }
        public DbSet<BomOrder> BomOrders { get; set; }
        public DbSet<BomOrderDetail> BomOrderDetails { get; set; }
        public DbSet<BomOrderDetailList> BomOrderDetailLists { get; set; }
        public DbSet<BomOrderVirtualList> BomOrderVirtualLists { get; set; }
        public DbSet<BomOut> BomOuts { get; set; }
        public DbSet<BomOutDetail> BomOutDetails { get; set; }
        public DbSet<BomVirtual> BomVirtuals { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<DelegateBack> DelegateBacks { get; set; }
        public DbSet<DelegateBackDetail> DelegateBackDetails { get; set; }
        public DbSet<DelegateOrder> DelegateOrders { get; set; }
        public DbSet<DelegateOrderDetail> DelegateOrderDetails { get; set; }
        public DbSet<DelegateSend> DelegateSends { get; set; }
        public DbSet<DelegateSendDetail> DelegateSendDetails { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Depot> Depots { get; set; }
        public DbSet<DepotDetail> DepotDetails { get; set; }
        public DbSet<Dictionary> Dictionaries { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<FunctionRight> FunctionRights { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<MaterialPrice> MaterialPrices { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuRight> MenuRights { get; set; }
        public DbSet<MsgRece> MsgReces { get; set; }
        public DbSet<MsgSend> MsgSends { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<ProduceDetail> ProduceDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductGive> ProductGives { get; set; }
        public DbSet<ProductGiveDetail> ProductGiveDetails { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<Production> Productions { get; set; }
        public DbSet<ProductPull> ProductPulls { get; set; }
        public DbSet<ProductPullDetail> ProductPullDetails { get; set; }
        public DbSet<ProductSemi> ProductSemis { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseDetail> PurchaseDetails { get; set; }
        public DbSet<PurchaseRequire> PurchaseRequires { get; set; }
        public DbSet<PurchaseRequireDetail> PurchaseRequireDetails { get; set; }
        public DbSet<PurchaseReturn> PurchaseReturns { get; set; }
        public DbSet<PurchaseReturnDetail> PurchaseReturnDetails { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<StockExchange> StockExchanges { get; set; }
        public DbSet<StockIn> StockIns { get; set; }
        public DbSet<StockInDetail> StockInDetails { get; set; }
        public DbSet<StockOut> StockOuts { get; set; }
        public DbSet<StockOutDetail> StockOutDetails { get; set; }
        public DbSet<StockReport> StockReports { get; set; }
        public DbSet<StockReturn> StockReturns { get; set; }
        public DbSet<StockReturnDetail> StockReturnDetails { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<MsgReceModel> MsgReceModels { get; set; }
        public DbSet<MsgSendModel> MsgSendModels { get; set; }
        public DbSet<V_BillCost> V_BillCost { get; set; }
        public DbSet<V_BillCostDetail> V_BillCostDetail { get; set; }
        public DbSet<V_BomCostModel> V_BomCostModel { get; set; }
        public DbSet<V_BomMaerialTwo> V_BomMaerialTwo { get; set; }
        public DbSet<V_BomMaterialView> V_BomMaterialView { get; set; }
        public DbSet<V_BomOrderDetailListModel> V_BomOrderDetailListModel { get; set; }
        public DbSet<V_BomOrderDetailModel> V_BomOrderDetailModel { get; set; }
        public DbSet<V_BomOrderModel> V_BomOrderModel { get; set; }
        public DbSet<V_BomOrderVirtualDetail> V_BomOrderVirtualDetail { get; set; }
        public DbSet<V_BomOutDetailModel> V_BomOutDetailModel { get; set; }
        public DbSet<V_BomOutModel> V_BomOutModel { get; set; }
        public DbSet<V_BomProduct> V_BomProduct { get; set; }
        public DbSet<V_DelegateBackDetail> V_DelegateBackDetail { get; set; }
        public DbSet<V_DelegateBackModel> V_DelegateBackModel { get; set; }
        public DbSet<V_DelegateOrderDetail> V_DelegateOrderDetail { get; set; }
        public DbSet<V_DelegateOrderModel> V_DelegateOrderModel { get; set; }
        public DbSet<V_DelegateSendDetailModel> V_DelegateSendDetailModel { get; set; }
        public DbSet<V_DelegateSendModel> V_DelegateSendModel { get; set; }
        public DbSet<V_MaterialCost> V_MaterialCost { get; set; }
        public DbSet<V_MaterialPriceModel> V_MaterialPriceModel { get; set; }
        public DbSet<V_NeedPay> V_NeedPay { get; set; }
        public DbSet<V_Product> V_Product { get; set; }
        public DbSet<V_ProductGiveDetail> V_ProductGiveDetail { get; set; }
        public DbSet<V_ProductGiveModel> V_ProductGiveModel { get; set; }
        public DbSet<V_ProductionDetailModel> V_ProductionDetailModel { get; set; }
        public DbSet<V_ProductionModel> V_ProductionModel { get; set; }
        public DbSet<V_ProductPullDetail> V_ProductPullDetail { get; set; }
        public DbSet<V_ProductPullModel> V_ProductPullModel { get; set; }
        public DbSet<V_ProductSemi> V_ProductSemi { get; set; }
        public DbSet<V_PurchaseDetailModel> V_PurchaseDetailModel { get; set; }
        public DbSet<V_PurchaseModel> V_PurchaseModel { get; set; }
        public DbSet<V_PurchaseRequireMode> V_PurchaseRequireMode { get; set; }
        public DbSet<V_ReceiptDetail> V_ReceiptDetail { get; set; }
        public DbSet<V_ReportDepot> V_ReportDepot { get; set; }
        public DbSet<V_ReturnInModel> V_ReturnInModel { get; set; }
        public DbSet<V_ReturnOutModel> V_ReturnOutModel { get; set; }
        public DbSet<V_StockChangeModel> V_StockChangeModel { get; set; }
        public DbSet<V_StockInModel> V_StockInModel { get; set; }
        public DbSet<V_StockInPurchase> V_StockInPurchase { get; set; }
        public DbSet<V_StockOutModel> V_StockOutModel { get; set; }
        public DbSet<V_StockOutPurchase> V_StockOutPurchase { get; set; }
        public DbSet<V_StockReport> V_StockReport { get; set; }
        public DbSet<V_StockReportStatistics> V_StockReportStatistics { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AttachmentMap());
            modelBuilder.Configurations.Add(new BillCostMap());
            modelBuilder.Configurations.Add(new BillCostDetailMap());
            modelBuilder.Configurations.Add(new BomCostMap());
            modelBuilder.Configurations.Add(new BomMainMap());
            modelBuilder.Configurations.Add(new BomOrderMap());
            modelBuilder.Configurations.Add(new BomOrderDetailMap());
            modelBuilder.Configurations.Add(new BomOrderDetailListMap());
            modelBuilder.Configurations.Add(new BomOrderVirtualListMap());
            modelBuilder.Configurations.Add(new BomOutMap());
            modelBuilder.Configurations.Add(new BomOutDetailMap());
            modelBuilder.Configurations.Add(new BomVirtualMap());
            modelBuilder.Configurations.Add(new CompanyMap());
            modelBuilder.Configurations.Add(new DelegateBackMap());
            modelBuilder.Configurations.Add(new DelegateBackDetailMap());
            modelBuilder.Configurations.Add(new DelegateOrderMap());
            modelBuilder.Configurations.Add(new DelegateOrderDetailMap());
            modelBuilder.Configurations.Add(new DelegateSendMap());
            modelBuilder.Configurations.Add(new DelegateSendDetailMap());
            modelBuilder.Configurations.Add(new DepartmentMap());
            modelBuilder.Configurations.Add(new DepotMap());
            modelBuilder.Configurations.Add(new DepotDetailMap());
            modelBuilder.Configurations.Add(new DictionaryMap());
            modelBuilder.Configurations.Add(new EmployeeMap());
            modelBuilder.Configurations.Add(new FunctionMap());
            modelBuilder.Configurations.Add(new FunctionRightMap());
            modelBuilder.Configurations.Add(new JobMap());
            modelBuilder.Configurations.Add(new LogMap());
            modelBuilder.Configurations.Add(new MaterialMap());
            modelBuilder.Configurations.Add(new MaterialPriceMap());
            modelBuilder.Configurations.Add(new MenuMap());
            modelBuilder.Configurations.Add(new MenuRightMap());
            modelBuilder.Configurations.Add(new MsgReceMap());
            modelBuilder.Configurations.Add(new MsgSendMap());
            modelBuilder.Configurations.Add(new NewsMap());
            modelBuilder.Configurations.Add(new ProduceDetailMap());
            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Configurations.Add(new ProductCategoryMap());
            modelBuilder.Configurations.Add(new ProductGiveMap());
            modelBuilder.Configurations.Add(new ProductGiveDetailMap());
            modelBuilder.Configurations.Add(new ProductGroupMap());
            modelBuilder.Configurations.Add(new ProductionMap());
            modelBuilder.Configurations.Add(new ProductPullMap());
            modelBuilder.Configurations.Add(new ProductPullDetailMap());
            modelBuilder.Configurations.Add(new ProductSemiMap());
            modelBuilder.Configurations.Add(new PurchaseMap());
            modelBuilder.Configurations.Add(new PurchaseDetailMap());
            modelBuilder.Configurations.Add(new PurchaseRequireMap());
            modelBuilder.Configurations.Add(new PurchaseRequireDetailMap());
            modelBuilder.Configurations.Add(new PurchaseReturnMap());
            modelBuilder.Configurations.Add(new PurchaseReturnDetailMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new StockExchangeMap());
            modelBuilder.Configurations.Add(new StockInMap());
            modelBuilder.Configurations.Add(new StockInDetailMap());
            modelBuilder.Configurations.Add(new StockOutMap());
            modelBuilder.Configurations.Add(new StockOutDetailMap());
            modelBuilder.Configurations.Add(new StockReportMap());
            modelBuilder.Configurations.Add(new StockReturnMap());
            modelBuilder.Configurations.Add(new StockReturnDetailMap());
            modelBuilder.Configurations.Add(new SupplierMap());
            modelBuilder.Configurations.Add(new MsgReceModelMap());
            modelBuilder.Configurations.Add(new MsgSendModelMap());
            modelBuilder.Configurations.Add(new V_BillCostMap());
            modelBuilder.Configurations.Add(new V_BillCostDetailMap());
            modelBuilder.Configurations.Add(new V_BomCostModelMap());
            modelBuilder.Configurations.Add(new V_BomMaerialTwoMap());
            modelBuilder.Configurations.Add(new V_BomMaterialViewMap());
            modelBuilder.Configurations.Add(new V_BomOrderDetailListModelMap());
            modelBuilder.Configurations.Add(new V_BomOrderDetailModelMap());
            modelBuilder.Configurations.Add(new V_BomOrderModelMap());
            modelBuilder.Configurations.Add(new V_BomOrderVirtualDetailMap());
            modelBuilder.Configurations.Add(new V_BomOutDetailModelMap());
            modelBuilder.Configurations.Add(new V_BomOutModelMap());
            modelBuilder.Configurations.Add(new V_BomProductMap());
            modelBuilder.Configurations.Add(new V_DelegateBackDetailMap());
            modelBuilder.Configurations.Add(new V_DelegateBackModelMap());
            modelBuilder.Configurations.Add(new V_DelegateOrderDetailMap());
            modelBuilder.Configurations.Add(new V_DelegateOrderModelMap());
            modelBuilder.Configurations.Add(new V_DelegateSendDetailModelMap());
            modelBuilder.Configurations.Add(new V_DelegateSendModelMap());
            modelBuilder.Configurations.Add(new V_MaterialCostMap());
            modelBuilder.Configurations.Add(new V_MaterialPriceModelMap());
            modelBuilder.Configurations.Add(new V_NeedPayMap());
            modelBuilder.Configurations.Add(new V_ProductMap());
            modelBuilder.Configurations.Add(new V_ProductGiveDetailMap());
            modelBuilder.Configurations.Add(new V_ProductGiveModelMap());
            modelBuilder.Configurations.Add(new V_ProductionDetailModelMap());
            modelBuilder.Configurations.Add(new V_ProductionModelMap());
            modelBuilder.Configurations.Add(new V_ProductPullDetailMap());
            modelBuilder.Configurations.Add(new V_ProductPullModelMap());
            modelBuilder.Configurations.Add(new V_ProductSemiMap());
            modelBuilder.Configurations.Add(new V_PurchaseDetailModelMap());
            modelBuilder.Configurations.Add(new V_PurchaseModelMap());
            modelBuilder.Configurations.Add(new V_PurchaseRequireModeMap());
            modelBuilder.Configurations.Add(new V_ReceiptDetailMap());
            modelBuilder.Configurations.Add(new V_ReportDepotMap());
            modelBuilder.Configurations.Add(new V_ReturnInModelMap());
            modelBuilder.Configurations.Add(new V_ReturnOutModelMap());
            modelBuilder.Configurations.Add(new V_StockChangeModelMap());
            modelBuilder.Configurations.Add(new V_StockInModelMap());
            modelBuilder.Configurations.Add(new V_StockInPurchaseMap());
            modelBuilder.Configurations.Add(new V_StockOutModelMap());
            modelBuilder.Configurations.Add(new V_StockOutPurchaseMap());
            modelBuilder.Configurations.Add(new V_StockReportMap());
            modelBuilder.Configurations.Add(new V_StockReportStatisticsMap());
        }
    }
}
