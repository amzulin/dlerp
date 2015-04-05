using Enterprise.Invoicing.Common;
using Enterprise.Invoicing.Repositories;
using Enterprise.Invoicing.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Calabonga.Mvc.PagedListExt;
using Enterprise.Invoicing.ViewModel;
using Enterprise.Invoicing.Entities.Models;
using Enterprise.Invoicing.Entities;

namespace Enterprise.Invoicing.Web.Controllers
{
    public class bomController : BaseController
    {
        private ManageService manageService;
        private BomService bomService;
        private PurchaseService purchaseService;
        private StockOutService stockoutService;
        private StockInService stockinService;
        public bomController(IManageRepository _manageRepository, IBomRepository _bomrepository, IPurchaseRepository _bomRepository, IStockInRepository _stockinrepository, IStockOutRepository _stockoutrepository)
        {
            manageService = new ManageService(_manageRepository);
            bomService = new BomService(_bomrepository);
            stockoutService = new StockOutService(_stockoutrepository);
            stockinService = new StockInService(_stockinrepository);
            purchaseService = new PurchaseService(_bomRepository);
        }

        #region Bom管理

        public ActionResult bomlist(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var key = WebRequest.GetString("key", true);
            var valid = WebRequest.GetString("valid", true);
            data.key = key;
            data.valid = valid;
            var list = bomService.GetBomMaterial(key, valid).Where(p => p.parent_Id.HasValue == false && p.isChild == false);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = "&key=" + key + "&valid=" + valid;
            return View(data);
        }
        [AjaxAction(ForAction = "bomlist", ForController = "bom")]
        public ActionResult bomone()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            int id = WebRequest.GetInt("id", 0);
            string type = WebRequest.GetString("type");
            if (type == "export")
            {
                #region 导出
                var one = bomService.GetBomMaterial().FirstOrDefault(p => p.bomId == id);
                //  List<V_BomMaterial> f = new List<V_BomMaterial>();
                // f.Add(one);
                var list = bomService.GetChildBomMaterial(id, 0);
                var virtusls = ServiceDB.Instance.QueryModelList<BomVirtual>("select * from BomVirtual where bomId=" + id);
                // f.AddRange(list);
                string[] head = new string[11] { "序号", "层级", "属性", "物料编码", "物料名称", "物料规格", "物料图号", "基本用量", "单位", "损耗率", "备注" };

                List<string> datas = new List<string>();
                #region 构造
                var first = list.Where(p => p.parent_Id == id).ToList();
                var index = 1;
                for (int i = 0; i < first.Count; i++)
                {
                    var p = first[i];
                    var row = index.ToString() + "|" + (i + 1).ToString() + "|" + p.materialCate + "|" + p.materialNo + "|" + p.materialName + "|" + p.materialModel + "|" + p.tunumber + "|" + Math.Round(p.amount, p.xslength) + "|" + p.unit + "|" + (p.loss > 0 ? p.loss + "%" : "") + "|" + p.remark;
                    datas.Add(row); index++;
                    var second = list.Where(x => x.parent_Id == p.bomId).ToList();
                    for (int s = 0; s < second.Count; s++)
                    {
                        var so = second[s];
                        var rs = index.ToString() + "|" + (i + 1).ToString() + "." + (s + 1).ToString() + "|" + so.materialCate + "|" + so.materialNo + "|" + so.materialName + "|" + so.materialModel + "|" + so.tunumber + "|" + Math.Round(so.amount, so.xslength) + "|" + so.unit + "|" + (so.loss > 0 ? so.loss + "%" : "") + "|" + so.remark;
                        datas.Add(rs); index++;
                        var third = list.Where(x => x.parent_Id == so.bomId).ToList();
                        for (int t = 0; t < third.Count; t++)
                        {
                            var to = third[t];
                            var rt = index.ToString() + "|" + (i + 1).ToString() + "." + (s + 1).ToString() + "." + (t + 1).ToString() + "|" + to.materialCate + "|" + to.materialNo + "|" + to.materialName + "|" + to.materialModel + "|" + to.tunumber + "|" + Math.Round(to.amount, to.xslength) + "|" + to.unit + "|" + (to.loss > 0 ? to.loss + "%" : "") + "|" + to.remark;
                            datas.Add(rt); index++;
                            var four = list.Where(x => x.parent_Id == to.bomId).ToList();
                            for (int f = 0; f < four.Count; f++)
                            {
                                var fo = four[f];
                                var rf = index.ToString() + "|" + (i + 1).ToString() + "." + (s + 1).ToString() + "." + (t + 1).ToString() + "." + (f + 1).ToString() + "|" + fo.materialCate + "|" + fo.materialNo + "|" + fo.materialName + "|" + fo.materialModel + "|" + fo.tunumber + "|" + Math.Round(fo.amount, fo.xslength) + "|" + fo.unit + "|" + (fo.loss > 0 ? fo.loss + "%" : "") + "|" + fo.remark;
                                datas.Add(rf); index++;

                                var five = list.Where(x => x.parent_Id == fo.bomId).ToList();
                                for (int v = 0; v < five.Count; v++)
                                {
                                    var vo = five[f];
                                    var rv = index.ToString() + "|" + (i + 1).ToString() + "." + (s + 1).ToString() + "." + (t + 1).ToString() + "." + (f + 1).ToString() + "." + (v + 1).ToString() + "|" + vo.materialCate + "|" + vo.materialNo + "|" + vo.materialName + "|" + vo.materialModel + "|" + vo.tunumber + "|" + Math.Round(vo.amount, vo.xslength) + "|" + vo.unit + "|" + (vo.loss > 0 ? vo.loss + "%" : "") + "|" + vo.remark;
                                    datas.Add(rv); index++;


                                }

                            }

                        }
                    }
                }
                #endregion


                //for (int i = 0; i < f.Count; i++)
                //{
                //    var p = f[i];
                //    var row = (i + 1).ToString() + "|" + p.materialCate + "|" + p.materialNo + "|" + p.materialName + "|" + p.materialModel + "|" + p.tunumber + "|" + Math.Round(p.amount, p.xslength) + "|" + p.unit + "|" + p.remark;
                //    datas.Add(row);
                //}
                for (int i = 0; i < virtusls.Count; i++)
                {
                    var p = virtusls[i];
                    var row = index.ToString() + "|";
                    row += "|";
                    row += "其他科目|";
                    row += "|";
                    row += p.virtualName + "|";
                    row += "单价:|";
                    row += Math.Round(p.vPrice, 2) + "|";
                    row += Math.Round(p.vAmount, 2) + "|";
                    row += "||";
                    row += p.remark;
                    datas.Add(row); index++;
                }
                var root = "||" + one.materialCate + "|" + one.materialNo + "|" + one.materialName + "|" + one.materialModel + "|" + one.tunumber + "||" + one.unit + "||" + one.remark;

                datas.Insert(0, root);
                var msg = FileHelper.ExportEasy(head, datas);
                return Content(msg);
                #endregion
            }
            else
            {
                V_BomMaterial model = new V_BomMaterial();
                var ddlcate = bomService.GetBomNodeCate().Select(x => new SelectListItem { Text = x.text, Value = x.text }).ToList();
                string bomjson = "";
                if (id > 0 && type == "edit")
                {
                    model = bomService.GetBomMaterial().FirstOrDefault(p => p.bomId == id);
                    var obj = bomService.GetOneBom(id, 0);
                    bomjson = JsonHelper.ToJson(obj);
                }
                else type = "add";
                data.id = id;
                data.type = type;
                data.model = model;
                data.bomjson = bomjson;
                data.ddlcate = ddlcate;
                return View(data);
            }
        }
        [AjaxAction(ForAction = "bomlist", ForController = "bom")]
        public ActionResult bomview()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            int id = WebRequest.GetInt("id", 0);
            string bomjson = "";
            var obj = bomService.GetOneBom(id, 0);
            bomjson = JsonHelper.ToJson(obj);

            data.bomjson = bomjson;
            return View(data);
        }

        [AjaxAction(ForAction = "bomlist", ForController = "bom")]
        public ActionResult bomnode()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            int id = WebRequest.GetInt("id", 0);
            string type = WebRequest.GetString("type");
            V_BomMaterial model = new V_BomMaterial();
            var ddlcate = bomService.GetBomNodeCate().Select(x => new SelectListItem { Text = x.text, Value = x.text }).ToList();
            if (type == "edit")
            {
                model = bomService.GetBomMaterial().FirstOrDefault(p => p.bomId == id);
                var mc = ddlcate.FirstOrDefault(p => p.Value == model.materialCate);
                if (mc != null) mc.Selected = true;
            }
            data.id = id;
            data.type = type;
            data.model = model;
            data.ddlcate = ddlcate;
            return View(data);
        }


        [AjaxAction(ForAction = "bomlist", ForController = "bom")]
        public ActionResult bomvirtual()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            int id = WebRequest.GetInt("id", 0);
            int forbom = WebRequest.GetInt("forbom", 0);
            string type = WebRequest.GetString("type");
            BomVirtual model = new BomVirtual();
            if (type == "edit")
            {
                model = ServiceDB.Instance.QueryOneModel<BomVirtual>("select * from BomVirtual where virtualId=" + id);
            }
            data.id = id;
            data.type = type;
            data.model = model;
            data.forbom = forbom;
            return View(data);
        }

        [HttpPost]
        [AjaxAction(ForAction = "bomlist", ForController = "bom")]
        public ActionResult savebom()
        {
            int id = WebRequest.GetInt("id", 0);
            int? parent_id = WebRequest.GetInt("parent", 0);
            float amount = WebRequest.GetFloat("amount", 0.0f);
            string type = WebRequest.GetString("type");
            string version = WebRequest.GetString("version");
            if (type == "add")
            {
                parent_id = id;
                id = 0;
            }
            string material = WebRequest.GetString("material");
            string cate = WebRequest.GetString("cate");
            string remark = WebRequest.GetString("remark");
            if (parent_id == 0) parent_id = null;
            var r = bomService.SaveBom(type, id, parent_id, material, cate, amount, remark, version);
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AjaxAction(ForAction = "bomcostview", ForController = "cost")]
        public ActionResult savevirtual()
        {
            //id: id, type: type,amount:amount, price: price, remark: remark,km:km
            int id = WebRequest.GetInt("id", 0);
            int forbom = WebRequest.GetInt("forbom", 0);
            float amount = WebRequest.GetFloat("amount", 0.0f);
            float price = WebRequest.GetFloat("price", 0.0f);
            string km = WebRequest.GetString("km");
            string type = WebRequest.GetString("type");
            int bom = 0;
            int loss = 0;
            if (type == "add")
            {
                bom = id;
                id = 0;
            }
            string remark = WebRequest.GetString("remark");
            var r = bomService.SaveVirtual(type, id, bom, forbom, km, amount, price, remark);
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AjaxAction(ForAction = "bomlist", ForController = "bom")]
        public ActionResult deletebom()
        {
            int id = WebRequest.GetInt("id", 0);
            var r = bomService.DeleteBom(id);
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AjaxAction(ForAction = "bomlist", ForController = "bom")]
        public ActionResult deletebomselect()
        {
            string forids = WebRequest.GetString("forids");
            string[] ids = forids.Split(',');
            int count = 0;
            foreach (string item in ids)
            {
                if (item == "") continue;
                var bom = ServiceDB.Instance.QueryOneModel<BomMain>("select * from bommain where bomid=" + item);
                if (bom != null && bom.bomId > 0)
                {
                    var del = bomService.DeleteBom(bom.bomId);
                    if (del.status) count++;
                }
            }
            ReturnValue r = new ReturnValue();
            r.status = count > 0;
            r.message = r.status ? "" : "BOM节点删除失败，BOM已引用，禁止删除";
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AjaxAction(ForAction = "bomlist", ForController = "bom")]
        public ActionResult bomvalid()
        {
            int id = WebRequest.GetInt("id", 0);
            int valid = WebRequest.GetInt("valid", 0);
            string sql = "";
            if (valid == 0) sql = "update BomMain set status=" + valid + ",endDate='" + DateTime.Now.ToString() + "' where bomId=" + id;
            else sql = "update BomMain set status=" + valid + ",startDate='" + DateTime.Now.ToString() + "',endDate=null where bomId=" + id;
            var row = ServiceDB.Instance.ExecuteSqlCommand(sql);
            ReturnValue rv = new ReturnValue();
            rv.status = row == 1;
            return Json(rv, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxAction(ForAction = "bomlist", ForController = "bom")]
        public ActionResult deletevirtual()
        {
            int id = WebRequest.GetInt("id", 0);
            int forbom = WebRequest.GetInt("forbom", 0);
            var r = bomService.DeleteVirtual(id, forbom);
            return Json(r, JsonRequestBehavior.AllowGet);
        }


        public ActionResult bommaterial(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var key = WebRequest.GetString("key", true);
            var valid = WebRequest.GetString("valid", true);
            data.key = key;
            data.valid = valid;
            var list = bomService.GetBomMaterialTwo(key);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = "&key=" + key + "&valid=" + valid;
            return View(data);
        }
        #endregion


        #region 订单列表
        public ActionResult orderlist(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();

            return View(data);
        }
        [AjaxAction(ForAction = "orderlist", ForController = "bom")]
        public ActionResult orderlistpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null) where = "";
            if (orderby == null || orderby == "") orderby = " createdate desc ";
            var list = bomService.BomOrderList(where, orderby);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "orderlist", ForController = "bom")]
        public ActionResult orderlistview()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var index = WebRequest.GetString("index", true);
            var status = WebRequest.GetInt("os", 0);
            var over = WebRequest.GetInt("over", 0);
            List<V_BomOrderDetailModel> r = new List<V_BomOrderDetailModel>();
            if (!string.IsNullOrEmpty(no)) r = bomService.BomOrderDetailList(no).ToList();
            var list = r;
            data.no = no;
            data.index = index;
            data.list = list;
            data.status = status;
            data.over = over;
            return PartialView(data);
        }
        [AjaxAction(ForAction = "inlist", ForController = "stockin")]
        public ActionResult bomdelete()
        {
            string no = WebRequest.GetString("no", true);
            bool result = bomService.DeleteBomOrder(no);
            return Content(result ? "ok" : "error");
        }
        #endregion

        #region BOM订单

        public ActionResult orderone()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var type = WebRequest.GetString("type", true);
            var model = new V_BomOrderModel();
            var message = "";
            if (type == "" || no == "")
            {
                #region 创建申请单
                type = "add";
                no = bomService.GetBomOrderNo();
                #endregion
            }
            else if (type == "edit")
            {
                model = bomService.BomOrderList(no).FirstOrDefault();
            }
            // var mc = stockinService.GetMaterialCategory().Select(x => new SelectListItem { Text = x.text, Value = x.value }).ToList();
            //    var depots = stockinService.QueryDepot(1).Select(x => new SelectListItem { Text = x.depotName, Value = x.depotId.ToString() }).ToList();
            //  data.mc = mc;
            var ddl = purchaseService.QuerySupplier(1, 1).ToList();
            data.ddl = ddl; data.no = no;
            // data.depots = depots;
            data.no = no;
            data.one = model;
            data.message = message;
            data.type = type;
            return View(data);
        }
        [HttpPost]
        [AjaxAction(ForAction = "orderone", ForController = "bom")]
        public ActionResult saveorderonetemp(V_BomOrderDetailModel model)
        {
            List<V_BomOrderDetailModel> temp = (List<V_BomOrderDetailModel>)SessionHelper.GetSession("BD1" + model.bomOrderNo + Masterpage.CurrUser.staffid);
            if (temp == null || temp.Count == 0)
            {
                temp = new List<V_BomOrderDetailModel>();
                temp.Add(model);
            }
            else
            {
                bool add = false;
                V_BomOrderDetailModel ht = new V_BomOrderDetailModel();
                if (model.detailSn > 0)
                {
                    ht = temp.FirstOrDefault(p => p.detailSn == model.detailSn);
                    ht.type = "edit";
                }
                else
                {
                    ht = temp.FirstOrDefault(p => p.bomId == model.bomId);
                    if (ht == null)
                    {
                        ht = new V_BomOrderDetailModel();
                        add = true;
                        ht.type = "add";
                    }
                }
                ht.bomId = model.bomId;
                ht.OrderDetailRemark = model.OrderDetailRemark;
                ht.Amount = model.Amount;
                ht.Price = model.Price;
                ht.sendDate = model.sendDate;
                ht.bomOrderNo = model.bomOrderNo;
                ht.materialNo = model.materialNo;
                ht.materialModel = model.materialModel;
                ht.materialName = model.materialName;
                if (add) temp.Add(ht);

            }
            SessionHelper.SetSession("BD1" + model.bomOrderNo + Masterpage.CurrUser.staffid, temp);
            return Json(temp.Count, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxAction(ForAction = "orderone", ForController = "bom")]
        public ActionResult deleteorderone()
        {
            string in_no = WebRequest.GetString("no", true);
            int g = WebRequest.GetInt("detail", 0);
            int bid = WebRequest.GetInt("bid", 0);
            bool result = false;
            List<V_BomOrderDetailModel> temp = (List<V_BomOrderDetailModel>)SessionHelper.GetSession("BD1" + in_no + Masterpage.CurrUser.staffid);
            if (temp != null && temp.Count > 0)
            {
                var h = temp.FirstOrDefault(p => p.bomId == bid);

                if (h != null)
                {
                    if (g != 0) h.type = "delete";
                    else temp.Remove(h);
                    result = true; SessionHelper.SetSession("BD1" + in_no + Masterpage.CurrUser.staffid, temp);
                }
            }
            return Content(result ? "ok" : "error");
        }

        [HttpPost]
        [AjaxAction(ForAction = "orderone", ForController = "bom")]
        public ActionResult saveorderone(BomOrder model, string type)
        {
            ReturnValue r = new ReturnValue() { status = false };
            List<V_BomOrderDetailModel> temp = (List<V_BomOrderDetailModel>)SessionHelper.GetSession("BD1" + model.bomOrderNo + Masterpage.CurrUser.staffid);
            if (temp == null || temp.Count < 1)
            {
                r.message = "不存在BOM订单明细";
            }
            else if (!model.supplierId.HasValue || model.supplierId.Value == 0)
            {
                r.message = "未选择客户";
            }
            else
            {
                if (type == "add")
                {
                    model.createDate = DateTime.Now;
                    model.depId = Masterpage.CurrUser.depId;
                    model.staffId = Masterpage.CurrUser.staffid;
                    model.status = 0;
                    model.isover = 0;
                    model.valid = true;
                    string backno = bomService.AddBomOrder(model);
                    if (backno == "")
                    {
                        r.message = "BOM订单生成失败";
                    }
                    else
                    {
                        r = bomService.SaveBomOrderDetail(backno, temp, model.deportStaff, model.remark);
                    }
                }
                else if (type == "edit")
                {
                    r = bomService.SaveBomOrderDetail(model.bomOrderNo, temp, model.deportStaff, model.remark);

                }
            }
            SessionHelper.Del("BD1" + model.bomOrderNo + Masterpage.CurrUser.staffid);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [AjaxAction(ForAction = "orderone", ForController = "bom")]
        public ActionResult orderonedetail(string no)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = bomService.BomOrderDetailList(no).ToList();
            List<V_BomOrderDetailModel> temp = (List<V_BomOrderDetailModel>)SessionHelper.GetSession("BD1" + no + Masterpage.CurrUser.staffid);
            List<V_BomOrderDetailModel> newlist = new List<V_BomOrderDetailModel>();
            if (temp != null && temp.Count > 0)
            {
                newlist.AddRange(temp);
            }
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    var h = newlist.FirstOrDefault(p => p.detailSn == item.detailSn);
                    if (h != null) continue;
                    else
                        newlist.AddRange(list.Select(p => new V_BomOrderDetailModel
                        {
                            detailSn = p.detailSn,
                            materialNo = p.materialNo,
                            materialModel = p.materialModel,
                            materialName = p.materialName,
                            remark = p.remark,
                            type = p.type,
                            OrderDetailRemark = p.OrderDetailRemark,
                            Amount = p.Amount,
                            Price = p.Price,
                            sendDate = p.sendDate,
                            bomOrderNo = p.bomOrderNo,
                            bomId = p.bomId,
                            tunumber = p.tunumber


                        }).ToList());
                }
            }
            //  var mc = stockinService.GetMaterialCategory();
            //    data.mc = mc;
            data.list = newlist;
            data.no = no;
            SessionHelper.SetSession("BD1" + no + Masterpage.CurrUser.staffid, newlist);
            return PartialView(data);
        }


        #endregion

        #region BOM 转化
        [HttpPost]
        [AjaxAction(ForAction = "orderlist", ForController = "bom")]
        public ActionResult deleteorderdetail(string no, int sn)
        {

            bool result = bomService.DeleteBomOrderDetail(no, sn);
            return Content(result ? "ok" : "error");
        }

        [HttpPost]
        [AjaxAction(ForAction = "orderlist", ForController = "bom")]
        public ActionResult bomoptcreate(string type, string no, int sn)
        {

            // var r = bomService.BomOrderDetailCreate(no, sn, Masterpage.CurrUser.name);
            ReturnValue r = new ReturnValue();
            switch (type)
            {
                case "requrie":
                    r = bomService.CreatePurcharseRequire(no, sn, Masterpage.CurrUser.staffid, Masterpage.CurrUser.depId);
                    break;
                case "delegate":
                    r = bomService.CreateDelegate(no, sn, Masterpage.CurrUser.staffid, Masterpage.CurrUser.depId);
                    break;
                case "produce":
                    r = bomService.CreateProduce(no, sn, Masterpage.CurrUser.staffid, Masterpage.CurrUser.depId);
                    break;
                default:
                    r.status = false;
                    r.message = "操作类别有误";
                    break;
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AjaxAction(ForAction = "orderlist", ForController = "bom")]
        public ActionResult bomoptdelete(string type, string no, int sn)
        {
            ReturnValue r = new ReturnValue();
            switch (type)
            {
                case "requrie":
                    r = bomService.DeletePurcharseRequire(no, sn, Masterpage.CurrUser.staffid, Masterpage.CurrUser.depId);
                    break;
                case "delegate":
                    r = bomService.DeleteDelegate(no, sn, Masterpage.CurrUser.staffid, Masterpage.CurrUser.depId);
                    break;
                case "produce":
                    r = bomService.DeleteProduce(no, sn, Masterpage.CurrUser.staffid, Masterpage.CurrUser.depId);
                    break;
                default:
                    r.status = false;
                    r.message = "操作类别有误";
                    break;
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [AjaxAction(ForAction = "orderlist", ForController = "bom")]
        public ActionResult bomdetail(string no, int sn, string type)
        {

            dynamic data = new System.Dynamic.ExpandoObject();
            var one = bomService.BomOrderDetailOne(sn);
            var list = bomService.GetBomOrderBomDetailList(sn).ToList();
            // var virtusl = bomService.GetBomOrderVirtualDetailList(sn).ToList();
            data.one = one;
            data.list = list;
            // data.virtusl = virtusl;
            data.no = no;
            data.sn = sn;
            data.type = type;
            return View(data);
        }
        [AjaxAction(ForAction = "orderlist", ForController = "bom")]
        public ActionResult savebomdetail(int sn, string no, string ids, string counts, string dates, string remarks, string st, string sns, string amounts, string prices, string bzs)
        {
            ReturnValue r = new ReturnValue { };
            try
            {
                #region 生成需求单
                string backno = "";
                if (st == "delegate")
                {
                    var one = bomService.BomOrderDetailOne(sn);

                    backno = purchaseService.AddRequire("", Masterpage.CurrUser.staffid, Masterpage.CurrUser.depId, 0, one.remark);
                    if (backno == "")
                    {
                        r.status = false;
                        r.message = "生成采购需求单失败！";
                        return Json(r, JsonRequestBehavior.AllowGet);
                    }
                }
                #endregion
                List<int> id = ids.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                List<double> count = counts.Split(',').Select(x => Convert.ToDouble(x)).ToList();
                List<string> date = dates.Split(',').ToList();
                List<string> remark = remarks.Split(',').ToList();
                r = bomService.SaveBomOrderDetail(no, sn, id, count, date, remark, backno);
                #region 改科目
                //string[] snsl = sns.Split(',');
                //string[] amountl = amounts.Split(',');
                //string[] pricel = prices.Split(',');
                //string[] bzl = bzs.Split(',');
                //if (sns.Length > 0)
                //{
                //    for (int i = 0; i < sns.Length; i++)
                //    {
                //        ServiceDB.Instance.ExecuteSqlCommand("update BomOrderVirtualList set sAmount=" + amountl[i] + ",sPrice=" + pricel[i] + ",remark='" + bzl[i] + "' where virtualSn=" + snsl[i]);
                //    }
                //}
                #endregion
            }
            catch
            {
                r.status = false;
                r.message = "修改失败，参数有误";
            }

            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region BOM领料出库

        public ActionResult bomout()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var type = WebRequest.GetString("type", true);
            var model = new StockModel();
            var bom = new V_BomOrderDetailModel();
            var message = "";
            if (type == "" || no == "")
            {
                #region 创建申请单
                type = "add";
                no = stockoutService.GetStockOutNo();
                #endregion
            }
            else if (type == "edit")
            {
                model = stockoutService.StockOutList(no).FirstOrDefault();

                bom = bomService.BomOrderDetailOne(model.bomdetailsn.Value);
            }
            var ddl = purchaseService.QuerySupplier(1, 1).ToList();
            data.ddl = ddl;
            var depots = stockinService.QueryDepot(1).Select(x => new SelectListItem { Text = x.depotName, Value = x.depotId.ToString() }).ToList();
            data.depots = depots;
            data.no = no;
            data.one = model;
            data.bom = bom;
            data.message = message;
            data.type = type;
            return View(data);
        }
        [HttpPost]
        [AjaxAction(ForAction = "bomout", ForController = "bom")]
        public ActionResult savebomouttemp()
        {
            int detail = WebRequest.GetFormInt("detail", 0);
            int int_depot = WebRequest.GetFormInt("depot", 0);
            string no = WebRequest.GetString("no", true);
            int sn = WebRequest.GetFormInt("sn", 0);
            int bomid = WebRequest.GetFormInt("bomid", 0);
            int int_count = WebRequest.GetFormInt("count", 0);
            string remark = WebRequest.GetString("remark", true);
            List<V_BomMaterial> temp = (List<V_BomMaterial>)SessionHelper.GetSession("WO1" + no + sn + Masterpage.CurrUser.staffid);
            var bom = bomService.GetBomMaterial().FirstOrDefault(p => p.bomId == bomid);
            if (temp == null || temp.Count == 0)
            {
                temp = new List<V_BomMaterial>();
                temp.Add(new V_BomMaterial
                {
                    outdetailsn = 0,
                    materialModel = bom.materialModel,
                    materialName = bom.materialName,
                    materialNo = bom.materialNo,
                    outamount = int_count,
                    outno = no,
                    bomId = bomid,
                    deoptid = int_depot,
                    remark = remark,
                    materialCate = bom.materialCate,
                    type = "add"
                });
            }
            else
            {
                bool add = false;
                V_BomMaterial ht = temp.FirstOrDefault(p => p.bomId == bomid);

                if (ht == null)
                {
                    ht = new V_BomMaterial();
                    add = true;
                    ht.materialModel = bom.materialModel;
                    ht.materialName = bom.materialName;
                    ht.materialNo = bom.materialNo;
                    ht.materialCate = bom.materialCate;
                    ht.type = "add";
                    ht.outdetailsn = 0;
                }
                if (ht.outdetailsn == 0) ht.type = "add";
                else ht.type = "edit";
                ht.outamount = int_count;
                ht.deoptid = int_depot;
                ht.outno = no;
                ht.bomId = bomid;
                ht.remark = remark;
                if (add) temp.Add(ht);

            }
            SessionHelper.SetSession("WO1" + no + sn + Masterpage.CurrUser.staffid, temp);
            return Json(temp.Count, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxAction(ForAction = "bomout", ForController = "bom")]
        public ActionResult deletebomout()
        {
            string in_no = WebRequest.GetString("no", true);
            int g = WebRequest.GetFormInt("detail", 0);
            int sn = WebRequest.GetFormInt("sn", 0);
            int bomid = WebRequest.GetFormInt("bomid", 0);
            bool result = false;
            List<V_BomMaterial> temp = (List<V_BomMaterial>)SessionHelper.GetSession("WO1" + in_no + sn + Masterpage.CurrUser.staffid);
            if (temp != null && temp.Count > 0)
            {
                var h = temp.FirstOrDefault(p => p.bomId == bomid);

                if (h != null)
                {
                    if (g != 0) h.type = "delete";
                    else temp.Remove(h);
                    result = true; SessionHelper.SetSession("WO1" + in_no + sn + Masterpage.CurrUser.staffid, temp);
                }
            }
            return Content(result ? "ok" : "error");
        }

        [HttpPost]
        [AjaxAction(ForAction = "bomout", ForController = "bom")]
        public ActionResult savebomout()
        {
            string no = WebRequest.GetString("no", true);
            string type = WebRequest.GetString("type", true);
            string remark = WebRequest.GetString("remark", true);
            string deport = WebRequest.GetString("deport", true);
            int supplier = WebRequest.GetInt("supplier", 0);
            int sn = WebRequest.GetInt("sn", 0);
            float amount = WebRequest.GetFloat("amount", 0);
            ReturnValue r = new ReturnValue() { status = false };
            List<V_BomMaterial> temp = (List<V_BomMaterial>)SessionHelper.GetSession("WO1" + no + sn + Masterpage.CurrUser.staffid);
            if (temp == null || temp.Count < 1)
            {
                r.message = "不存在领料出库明细";
            }
            else if (supplier == 0 || sn == 0)
            {
                r.message = "未选择客户或客户订单产品";
            }
            else
            {
                if (type == "add")
                {
                    string backno = bomService.AddStockOut(Masterpage.CurrUser.staffid, Masterpage.CurrUser.depId, supplier, sn, amount, 3, remark, deport);
                    if (backno == "")
                    {
                        r.message = "领料出库单生成失败";
                    }
                    else
                    {
                        r = bomService.SaveStockOutDetail(backno, temp, amount, remark, deport);
                    }
                }
                else if (type == "edit")
                {
                    r = bomService.SaveStockOutDetail(no, temp, amount, remark, deport);

                }
            }
            SessionHelper.Del("WO1" + no + sn + Masterpage.CurrUser.staffid);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [AjaxAction(ForAction = "bomout", ForController = "bom")]
        public ActionResult bomoutdetail(string no, int sn, int amount)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var depots = stockinService.QueryDepot(1).Select(x => new SelectListItem { Text = x.depotName, Value = x.depotId.ToString() }).ToList();
            data.depots = depots;
            var firstdepots = 0;
            if (depots != null && depots.Count > 0) firstdepots = Convert.ToInt32(depots[0].Value);
            List<V_BomMaterial> newlist = new List<V_BomMaterial>();
            if (sn > 0)
            {
                var list = bomService.GetBomoutDetail(no);
                List<V_BomMaterial> temp = (List<V_BomMaterial>)SessionHelper.GetSession("WO1" + no + sn + Masterpage.CurrUser.staffid);
                if (temp != null && temp.Count > 0)
                {
                    newlist.AddRange(temp);
                }
                if (list != null && list.Count > 0)
                {
                    #region 数据库已有

                    foreach (var item in list)
                    {
                        var h = newlist.FirstOrDefault(p => p.outdetailsn == item.outdetailsn);
                        if (h == null)
                            newlist.AddRange(list.Select(p => new V_BomMaterial
                            {
                                bomId = p.bomId,
                                materialCate = p.materialCate,
                                deoptid = p.deoptid,
                                materialModel = p.materialModel,
                                materialName = p.materialName,
                                materialNo = p.materialNo,
                                outamount = p.outamount,
                                outno = p.outno,
                                outdetailsn = p.outdetailsn,
                                tunumber = p.tunumber,
                                remark = p.remark,
                                unit = p.unit,
                                type = ""
                            }).ToList());
                    }
                    #endregion
                }
                else
                {
                    #region 初始添加
                    if (newlist.Count < 1)
                    {
                        var one = bomService.BomOrderDetailOne(sn);
                        var bomlist = bomService.GetChildBomMaterial(one.bomId, amount);
                        foreach (var p in bomlist)
                        {
                            var h = newlist.FirstOrDefault(x => x.bomId == p.bomId);
                            if (h == null)
                                newlist.Add(new V_BomMaterial
                                {
                                    bomId = p.bomId,
                                    materialCate = p.materialCate,
                                    materialModel = p.materialModel,
                                    materialName = p.materialName,
                                    materialNo = p.materialNo,
                                    outamount = p.outamount,
                                    deoptid = firstdepots,
                                    outno = no,
                                    outdetailsn = 0,
                                    tunumber = p.tunumber,
                                    remark = p.remark,
                                    unit = p.unit,
                                    type = "add"
                                });
                        }
                    }
                    #endregion
                }
            }
            data.list = newlist;
            data.no = no;
            data.sn = sn;
            SessionHelper.SetSession("WO1" + no + sn + Masterpage.CurrUser.staffid, newlist);
            return PartialView(data);
        }

        [AjaxAction(ForAction = "bomout", ForController = "bom")]
        public ActionResult getsupplierorder(int supplierid)
        {
            var list = bomService.GetBomOrderDropList(supplierid);
            var orders1 = list.Select(x => new SelectListItem { Text = x.bomOrderNO, Value = x.bomOrderNO }).ToList();
            List<SelectListItem> orders = new List<SelectListItem>();
            List<string> had = new List<string>();
            foreach (var item in orders1)
            {
                if (!had.Contains(item.Value))
                {
                    had.Add(item.Value);
                    orders.Add(item);
                }
            }
            var details = list.Select(x => new { no = x.bomOrderNO, sn = x.detailSn, cate = x.materialCate, mno = x.materialNo, mname = x.materialName, nmodel = x.materialModel, mtu = x.materialTu, amount = x.orderAmount, outamount = x.outAmount });
            var data = new { orders = orders, details = details };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 委外单

        public ActionResult delegatelist()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            return View();
        }
        [AjaxAction(ForAction = "delegatelist", ForController = "bom")]
        public ActionResult delegatelistpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null) where = "";
            if (orderby == null || orderby == "") orderby = " createdate desc ";
            //select * from V_PurchaseRequireMode where delegateNo<>'' " + where + " order by " + orderby
            var list = ServiceDB.Instance.QueryModelList<V_DelegateOrderModel>("select * from V_DelegateOrderModel where delegateNo<>'' " + where + " order by " + orderby);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;

            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "delegatelist", ForController = "bom")]
        public ActionResult delegatedetailview()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var list = ServiceDB.Instance.QueryModelList<V_DelegateOrderDetail>("select * from V_DelegateOrderDetail where delegateNo='" + no + "'");

            //var list = bomService.RequireDetailList(no);
            data.no = no;
            data.list = list;
            return PartialView(data);
        }


        [AjaxAction(ForAction = "delegatelist", ForController = "bom")]
        public ActionResult delegateone()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var type = WebRequest.GetString("type", true);
            var model = new V_DelegateOrderModel();
            List<ReturnValue> ddl = new List<ReturnValue>();
            var message = "";
            if (type == "" || no == "")
            {
                #region 创建申请单
                type = "add";
                no = bomService.GetDelegateNo();

                #endregion
            }
            else if (type == "edit")
            {
                model = ServiceDB.Instance.QueryOneModel<V_DelegateOrderModel>("select * from V_DelegateOrderModel where delegateNo='" + no + "'");
                ddl = ServiceDB.Instance.QueryModelList<ReturnValue>("select m.materialNo as value,m.materialName as value2,m.materialModel as value3,m.unit as message,convert(bit,1) as status from bommain as bm inner join Material as m on bm.materialNo=m.materialNo where bm.parent_Id=" + model.bomId).ToList();

            }
            data.no = no;
            data.one = model;
            data.message = message;
            data.ddl = ddl;
            data.type = type;
            return View(data);
        }



        [HttpPost]
        [AjaxAction(ForAction = "delegatelist", ForController = "bom")]
        public ActionResult savedelegatedetailtemp()
        {
            //type: htype, no: no, detail: hdetail, m: hmaterial, name: hname, model: hmodel, count: ctxt
            string m = WebRequest.GetString("m", true);
            string count = WebRequest.GetString("count", true);
            string no = WebRequest.GetString("no", true);
            int int_count = Convert.ToInt32(count);

            string name = WebRequest.GetString("name", true);
            string model = WebRequest.GetString("model", true);
            string mu = WebRequest.GetString("mu", true);

            string remark = WebRequest.GetString("remark", true);
            List<V_DelegateOrderDetail> temp = (List<V_DelegateOrderDetail>)SessionHelper.GetSession("WX" + no + Masterpage.CurrUser.staffid);
            if (temp == null || temp.Count == 0)
            {
                temp = new List<V_DelegateOrderDetail>();
                temp.Add(new V_DelegateOrderDetail
                {
                    detailSn = 0,
                    materialModel = model,
                    unit = mu,
                    materialName = name,
                    materialNo = m,
                    detailAmount = int_count,
                    delegateNo = no,
                    remark = remark,
                    supplierName = "add"
                });
            }
            else
            {
                var ht = temp.FirstOrDefault(p => p.materialNo == m);
                if (ht != null)
                {
                    if (ht.detailSn != 0) ht.supplierName = "edit";
                    ht.detailAmount = int_count;
                    ht.remark = remark;
                }
                else
                {
                    temp.Add(new V_DelegateOrderDetail
                    {
                        detailSn = 0,
                        materialModel = model,
                        materialName = name,
                        materialNo = m,
                        detailAmount = int_count,
                        delegateNo = no,
                        remark = remark,
                        unit = mu,
                        supplierName = "add"
                    });
                }
            }
            SessionHelper.SetSession("WX" + no + Masterpage.CurrUser.staffid, temp);
            return Json(temp.Count, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxAction(ForAction = "delegatelist", ForController = "bom")]
        public ActionResult deletedelegatedetail()
        {
            string in_no = WebRequest.GetString("no", true);
            string detail = WebRequest.GetString("detail", true);
            string m = WebRequest.GetString("m", true);
            int g = Convert.ToInt32(detail);
            bool result = false;
            List<V_DelegateOrderDetail> temp = (List<V_DelegateOrderDetail>)SessionHelper.GetSession("WX" + in_no + Masterpage.CurrUser.staffid);
            if (temp != null && temp.Count > 0)
            {
                var h = temp.FirstOrDefault(p => p.materialNo == m);

                if (h != null)
                {
                    if (g != 0) h.supplierName = "delete";
                    else temp.Remove(h);
                    result = true; SessionHelper.SetSession("WX" + in_no + Masterpage.CurrUser.staffid, temp);
                }
            }
            return Content(result ? "ok" : "error");
        }

        [HttpPost]
        [AjaxAction(ForAction = "delegatelist", ForController = "bom")]
        public ActionResult savedelegatedetail()
        {
            //no,type,remark,material,bomorder,depot,bomordersn,amount,bomid,,,,,
            string no = WebRequest.GetString("no", true);
            string type = WebRequest.GetString("type", true);
            string remark = WebRequest.GetString("remark", true);
            string material = WebRequest.GetString("material", true);
            string bomorder = WebRequest.GetString("bomorder", true);
            string depot = WebRequest.GetString("depot", true);
            string bomordersn = WebRequest.GetString("bomordersn", true);
            string amount = WebRequest.GetString("amount", true);
            string bomid = WebRequest.GetString("bomid", true);
            string date = WebRequest.GetString("date", true);
            ReturnValue r = new ReturnValue() { status = false };
            List<V_DelegateOrderDetail> temp = (List<V_DelegateOrderDetail>)SessionHelper.GetSession("WX" + no + Masterpage.CurrUser.staffid);
            if (temp == null || temp.Count < 1)
            {
                r.message = "不存在申请明细";
            }
            else
            {
                bool saveedit = false;
                if (type == "add")
                {
                    string backno = bomService.GetDelegateNo();
                    var insert = "INSERT INTO DelegateOrder VALUES (" + backno + ",null,'" + material + "','" + bomorder + "'," + bomordersn + "," + bomid + "," + Masterpage.CurrUser.staffid + "," + amount + ",0,0,0,1,1,0,0,'','" + depot + "',getdate(),'" + date + "','" + remark + "')";
                    var row = ServiceDB.Instance.ExecuteSqlCommand(insert);
                    if (backno == "" || row == 0)
                    {
                        r.message = "申请单生成失败";
                    }
                    else
                    {
                        no = backno;
                        saveedit = true;
                    }
                }
                else if (type == "edit")
                {
                    saveedit = true;
                }

                #region 保存明细
                if (saveedit)
                {
                    try
                    {
                        var model = ServiceDB.Instance.QueryOneModel<DelegateOrder>("select * from DelegateOrder where delegateno='" + no + "' and status=0");
                        if (model == null) r = new ReturnValue { status = false, message = "不存在委外单" };
                        else if (temp == null || temp.Count < 1) r = new ReturnValue { status = false, message = "不存在委外单明细" };
                        else
                        {
                            foreach (var item in temp)
                            {
                                if (item.supplierName == "") continue;

                                if (item.supplierName == "delete" && item.detailSn.HasValue)
                                {
                                    ServiceDB.Instance.ExecuteSqlCommand("delete from DelegateOrderDetail where detailSn=" + item.detailSn.Value);
                                }
                                else if (item.supplierName == "edit")
                                {

                                    var d = ServiceDB.Instance.QueryOneModel<DelegateOrderDetail>("select * from DelegateOrderDetail where detailSn=" + item.detailSn.Value);

                                    if (d != null)
                                    {
                                        ServiceDB.Instance.ExecuteSqlCommand("update DelegateOrderDetail set materialNo='" + item.materialNo + "',amount=" + item.detailAmount + ",remark='" + item.detailRemark + "' where detailSn=" + item.detailSn.Value);
                                    }
                                }
                                else if (item.supplierName == "add")
                                {
                                    string sql = "INSERT INTO DelegateOrderDetail VALUES           ('" + model.delegateNo + "'           ,'" + model.materialNo + "'           ," + item.detailAmount + " ,0,0,0,'" + item.detailRemark + "')";
                                    ServiceDB.Instance.ExecuteSqlCommand(sql);
                                }
                            }
                            #region 申请单

                            if (type == "edit")
                            {
                                ServiceDB.Instance.ExecuteSqlCommand("update DelegateOrder set staffId=" + Masterpage.CurrUser.staffid + ",amount=" + amount + ",backDate='" + date + "',deportStaff='" + depot + "',remark='" + remark + "' where delegateNo='" + no + "'");
                            }
                            #endregion
                            r.status = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        r = new ReturnValue { status = false, message = "保存失败：" + ex.Message };
                    }
                }
                else
                {
                    r.message = "不存在明细";
                }
                #endregion
            }
            SessionHelper.Del("WX" + no + Masterpage.CurrUser.staffid);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [AjaxAction(ForAction = "delegatelist", ForController = "bom")]
        public ActionResult delegatedetail(string no)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = ServiceDB.Instance.QueryModelList<V_DelegateOrderDetail>("select * from V_DelegateOrderDetail where delegateNo='" + no + "'");
            List<V_DelegateOrderDetail> temp = (List<V_DelegateOrderDetail>)SessionHelper.GetSession("WX" + no + Masterpage.CurrUser.staffid);
            List<V_DelegateOrderDetail> newlist = new List<V_DelegateOrderDetail>();
            if (temp != null && temp.Count > 0)
            {
                newlist.AddRange(temp);
            }
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    var h = newlist.FirstOrDefault(p => p.materialNo == item.materialNo);
                    if (h != null) continue;
                    else
                        newlist.AddRange(list.Select(p => new V_DelegateOrderDetail
                        {
                            detailSn = p.detailSn,
                            materialNo = p.materialNo,
                            delegateNo = p.delegateNo,
                            detailAmount = p.detailAmount,
                            materialModel = p.materialModel,
                            materialName = p.materialName,
                            tunumber = p.tunumber,
                            remark = p.remark,
                            createDate = p.createDate,
                            unit = p.unit,
                            supplierName = ""
                        }).ToList());
                }
            }
            data.list = newlist;
            data.no = no;
            SessionHelper.SetSession("WX" + no + Masterpage.CurrUser.staffid, newlist);
            return PartialView("delegatedetail", data);
        }

        [AjaxAction(ForAction = "delegatelist", ForController = "bom")]
        public ActionResult delegatedelete()
        {
            string no = WebRequest.GetString("no", true);
            int result = ServiceDB.Instance.ExecuteSqlCommand("delete from DelegateOrder where delegateno='" + no + "'");
            return Content(result == 1 ? "ok" : "error");
        }


        #endregion

        #region 领料单

        public ActionResult producelist()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            return View();
        }
        [AjaxAction(ForAction = "producelist", ForController = "bom")]
        public ActionResult producelistpart(int? page, int? pagesize, string where, string orderby)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            if (where == null) where = "";
            if (orderby == null || orderby == "") orderby = " createdate desc ";
            //select * from V_PurchaseRequireMode where produceNo<>'' " + where + " order by " + orderby
            var list = ServiceDB.Instance.QueryModelList<V_ProductionModel>("select * from V_ProductionModel where produceNo<>'' " + where + " order by " + orderby);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;

            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }

        [AjaxAction(ForAction = "producelist", ForController = "bom")]
        public ActionResult producedetailview()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var list = ServiceDB.Instance.QueryModelList<V_ProductionDetailModel>("select * from V_ProductionDetailModel where produceNo='" + no + "'");

            //var list = bomService.RequireDetailList(no);
            data.no = no;
            data.list = list;
            return PartialView(data);
        }


        [AjaxAction(ForAction = "producelist", ForController = "bom")]
        public ActionResult produceone()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var no = WebRequest.GetString("no", true);
            var type = WebRequest.GetString("type", true);
            var model = new V_ProductionModel();
            List<ReturnValue> ddl = new List<ReturnValue>();
            var message = "";
            if (type == "" || no == "")
            {
                #region 创建申请单
                type = "add";
                no = bomService.GetProductNo();

                #endregion
            }
            else if (type == "edit")
            {
                model = ServiceDB.Instance.QueryOneModel<V_ProductionModel>("select * from V_ProductionModel where produceNo='" + no + "'");

                var orderdetal = ServiceDB.Instance.QueryOneModel<BomOrderDetail>("select * from BomOrderDetail where detailSn=" + model.orderDetailSn);
                ddl = ServiceDB.Instance.QueryModelList<ReturnValue>("select m.materialNo as value,m.materialName as value2,m.materialModel as value3,m.unit as message,convert(bit,1) as status from bommain as bm inner join Material as m on bm.materialNo=m.materialNo where bm.parent_Id=" + orderdetal.bomId).ToList();

            }
            data.no = no;
            data.one = model;
            data.message = message;
            data.ddl = ddl;
            data.type = type;
            return View(data);
        }



        [HttpPost]
        [AjaxAction(ForAction = "producelist", ForController = "bom")]
        public ActionResult saveproducedetailtemp()
        {
            //type: htype, no: no, detail: hdetail, m: hmaterial, name: hname, model: hmodel, count: ctxt
            string m = WebRequest.GetString("m", true);
            string count = WebRequest.GetString("count", true);
            string no = WebRequest.GetString("no", true);
            int int_count = Convert.ToInt32(count);

            string name = WebRequest.GetString("name", true);
            string model = WebRequest.GetString("model", true);
            string mu = WebRequest.GetString("mu", true);

            string remark = WebRequest.GetString("remark", true);
            List<V_ProductionDetailModel> temp = (List<V_ProductionDetailModel>)SessionHelper.GetSession("GM" + no + Masterpage.CurrUser.staffid);
            if (temp == null || temp.Count == 0)
            {
                temp = new List<V_ProductionDetailModel>();
                temp.Add(new V_ProductionDetailModel
                {
                    detailSn = 0,
                    materialModel = model,
                    unit = mu,
                    materialName = name,
                    materialNo = m,
                    amount = int_count,
                    produceNo = no,
                    remark = remark,
                    edittype = "add"
                });
            }
            else
            {
                var ht = temp.FirstOrDefault(p => p.materialNo == m);
                if (ht != null)
                {
                    if (ht.detailSn != 0) ht.edittype = "edit";
                    ht.amount = int_count;
                    ht.remark = remark;
                }
                else
                {
                    temp.Add(new V_ProductionDetailModel
                    {
                        detailSn = 0,
                        materialModel = model,
                        materialName = name,
                        materialNo = m,
                        amount = int_count,
                        produceNo = no,
                        remark = remark,
                        unit = mu,
                        edittype = "add"
                    });
                }
            }
            SessionHelper.SetSession("GM" + no + Masterpage.CurrUser.staffid, temp);
            return Json(temp.Count, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxAction(ForAction = "producelist", ForController = "bom")]
        public ActionResult deleteproducedetail()
        {
            string in_no = WebRequest.GetString("no", true);
            string detail = WebRequest.GetString("detail", true);
            string m = WebRequest.GetString("m", true);
            int g = Convert.ToInt32(detail);
            bool result = false;
            List<V_ProductionDetailModel> temp = (List<V_ProductionDetailModel>)SessionHelper.GetSession("GM" + in_no + Masterpage.CurrUser.staffid);
            if (temp != null && temp.Count > 0)
            {
                var h = temp.FirstOrDefault(p => p.materialNo == m);

                if (h != null)
                {
                    if (g != 0) h.edittype = "delete";
                    else temp.Remove(h);
                    result = true; SessionHelper.SetSession("GM" + in_no + Masterpage.CurrUser.staffid, temp);
                }
            }
            return Content(result ? "ok" : "error");
        }

        [HttpPost]
        [AjaxAction(ForAction = "producelist", ForController = "bom")]
        public ActionResult saveproducedetail()
        {
            //no,type,remark,material,bomorder,depot,bomordersn,amount,bomid,,,,,
            string no = WebRequest.GetString("no", true);
            string type = WebRequest.GetString("type", true);
            string remark = WebRequest.GetString("remark", true);
            string material = WebRequest.GetString("material", true);
            string bomorder = WebRequest.GetString("bomorder", true);
            string depot = WebRequest.GetString("depot", true);
            string bomordersn = WebRequest.GetString("bomordersn", true);
            string amount = WebRequest.GetString("amount", true);
            string bomid = WebRequest.GetString("bomid", true);
            string date = WebRequest.GetString("date", true);
            ReturnValue r = new ReturnValue() { status = false };
            List<V_ProductionDetailModel> temp = (List<V_ProductionDetailModel>)SessionHelper.GetSession("GM" + no + Masterpage.CurrUser.staffid);
            if (temp == null || temp.Count < 1)
            {
                r.message = "不存在申请明细";
            }
            else
            {
                bool saveedit = false;
                if (type == "add")
                {
                    string backno = bomService.GetDelegateNo();
                    var insert = "INSERT INTO Production VALUES (" + backno + ",null,'" + material + "','" + bomorder + "'," + bomordersn + "," + bomid + "," + Masterpage.CurrUser.staffid + "," + amount + ",0,0,0,1,1,0,0,'','" + depot + "',getdate(),'" + date + "','" + remark + "')";
                    var row = ServiceDB.Instance.ExecuteSqlCommand(insert);
                    if (backno == "" || row == 0)
                    {
                        r.message = "申请单生成失败";
                    }
                    else
                    {
                        no = backno;
                        saveedit = true;
                    }
                }
                else if (type == "edit")
                {
                    saveedit = true;
                }

                #region 保存明细
                if (saveedit)
                {
                    try
                    {
                        var model = ServiceDB.Instance.QueryOneModel<Production>("select * from Production where produceno='" + no + "' and status=0");
                        if (model == null) r = new ReturnValue { status = false, message = "不存在委外单" };
                        else if (temp == null || temp.Count < 1) r = new ReturnValue { status = false, message = "不存在委外单明细" };
                        else
                        {
                            foreach (var item in temp)
                            {
                                if (item.edittype == "") continue;

                                if (item.edittype == "delete" && item.detailSn.HasValue)
                                {
                                    ServiceDB.Instance.ExecuteSqlCommand("delete from ProduceDetail where detailSn=" + item.detailSn.Value);
                                }
                                else if (item.edittype == "edit")
                                {

                                    var d = ServiceDB.Instance.QueryOneModel<ProduceDetail>("select * from ProduceDetail where detailSn=" + item.detailSn.Value);

                                    if (d != null)
                                    {
                                        ServiceDB.Instance.ExecuteSqlCommand("update ProduceDetail set materialNo='" + item.materialNo + "',amount=" + item.amount + ",remark='" + item.remark + "' where detailSn=" + item.detailSn.Value);
                                    }
                                }
                                else if (item.edittype == "add")
                                {
                                    string sql = "INSERT INTO ProduceDetail VALUES           ('" + model.produceNo + "'           ,'" + model.materialNo + "'           ," + item.amount + " ,0,0,0,'" + item.remark + "')";
                                    ServiceDB.Instance.ExecuteSqlCommand(sql);
                                }
                            }
                            #region 申请单

                            if (type == "edit")
                            {
                                ServiceDB.Instance.ExecuteSqlCommand("update Production set staffId=" + Masterpage.CurrUser.staffid + ",amount=" + amount + ",backDate='" + date + "',deportStaff='" + depot + "',remark='" + remark + "' where produceNo='" + no + "'");
                            }
                            #endregion
                            r.status = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        r = new ReturnValue { status = false, message = "保存失败：" + ex.Message };
                    }
                }
                else
                {
                    r.message = "不存在明细";
                }
                #endregion
            }
            SessionHelper.Del("GM" + no + Masterpage.CurrUser.staffid);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [AjaxAction(ForAction = "producelist", ForController = "bom")]
        public ActionResult producedetail(string no)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var list = ServiceDB.Instance.QueryModelList<V_ProductionDetailModel>("select * from V_ProductionDetailModel where produceNo='" + no + "'");
            List<V_ProductionDetailModel> temp = (List<V_ProductionDetailModel>)SessionHelper.GetSession("GM" + no + Masterpage.CurrUser.staffid);
            List<V_ProductionDetailModel> newlist = new List<V_ProductionDetailModel>();
            if (temp != null && temp.Count > 0)
            {
                newlist.AddRange(temp);
            }
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    var h = newlist.FirstOrDefault(p => p.materialNo == item.materialNo);
                    if (h != null) continue;
                    else
                        newlist.AddRange(list.Select(p => new V_ProductionDetailModel
                        {
                            detailSn = p.detailSn,
                            materialNo = p.materialNo,
                            produceNo = p.produceNo,
                            amount = p.amount,
                            materialModel = p.materialModel,
                            materialName = p.materialName,
                            tunumber = p.tunumber,
                            remark = p.remark,
                            createDate = p.createDate,
                            unit = p.unit,
                            edittype = ""
                        }).ToList());
                }
            }
            data.list = newlist;
            data.no = no;
            SessionHelper.SetSession("GM" + no + Masterpage.CurrUser.staffid, newlist);
            return PartialView("producedetail", data);
        }

        [AjaxAction(ForAction = "producelist", ForController = "bom")]
        public ActionResult producedelete()
        {
            string no = WebRequest.GetString("no", true);
            int result1 = ServiceDB.Instance.ExecuteSqlCommand("delete from ProduceDetail where produceno='" + no + "'");
            int result = ServiceDB.Instance.ExecuteSqlCommand("delete from Production where produceno='" + no + "'");
            return Content(result == 1 ? "ok" : "error");
        }


        #endregion

        #region 子bom
        public ActionResult childbomlist(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var key = WebRequest.GetString("key", true);
            var valid = WebRequest.GetString("valid", true);
            data.key = key;
            data.valid = valid;
            var list = bomService.GetBomMaterial(key, valid).Where(p => p.parent_Id.HasValue == false && p.isChild == true);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = "&key=" + key + "&valid=" + valid;
            return View(data);
        }
        [HttpPost]
        [AjaxAction(ForAction = "childbomlist", ForController = "bom")]
        public ActionResult savechildbom()
        {
            int id = WebRequest.GetInt("id", 0);
            string name = WebRequest.GetString("name");
            string remark = WebRequest.GetString("remark");
            string model = WebRequest.GetString("model");
            string version = WebRequest.GetString("version");
            string code = WebRequest.GetString("code");
            string sql = "insert into BomMain (materialCate,otherProject,loss,rootCost,status,version,remark,isChild,bomName,startDate)values('" + code + "','" + model + "',0,0,1,'" + version + "','" + remark + "',1,'" + name + "',getdate()) ";
            var row = ServiceDB.Instance.ExecuteSqlCommand(sql);
            ReturnValue rv = new ReturnValue();
            rv.status = row == 1;
            rv.message = row == 1 ? "添加成功" : "添加失败";

            return Json(rv, JsonRequestBehavior.AllowGet);
        }
        [AjaxAction(ForAction = "childbomlist", ForController = "bom")]
        public ActionResult childbomone()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            int id = WebRequest.GetInt("id", 0);
            string type = WebRequest.GetString("type");
            if (type == "export")
            {
                #region 导出
                var one = bomService.GetBomMaterial().FirstOrDefault(p => p.bomId == id);
                //  List<V_BomMaterial> f = new List<V_BomMaterial>();
                // f.Add(one);
                var list = bomService.GetChildBomMaterial(id, 0);
                var virtusls = ServiceDB.Instance.QueryModelList<BomVirtual>("select * from BomVirtual where bomId=" + id);
                // f.AddRange(list);
                string[] head = new string[11] { "序号", "层级", "属性", "物料编码", "物料名称", "物料规格", "物料图号", "基本用量", "单位", "损耗率", "备注" };

                List<string> datas = new List<string>();
                #region 构造
                var first = list.Where(p => p.parent_Id == id).ToList();
                var index = 1;
                for (int i = 0; i < first.Count; i++)
                {
                    var p = first[i];
                    var row = index.ToString() + "|" + (i + 1).ToString() + "|" + p.materialCate + "|" + p.materialNo + "|" + p.materialName + "|" + p.materialModel + "|" + p.tunumber + "|" + Math.Round(p.amount, p.xslength) + "|" + p.unit + "|" + (p.loss > 0 ? p.loss + "%" : "") + "|" + p.remark;
                    datas.Add(row); index++;
                    var second = list.Where(x => x.parent_Id == p.bomId).ToList();
                    for (int s = 0; s < second.Count; s++)
                    {
                        var so = second[s];
                        var rs = index.ToString() + "|" + (i + 1).ToString() + "." + (s + 1).ToString() + "|" + so.materialCate + "|" + so.materialNo + "|" + so.materialName + "|" + so.materialModel + "|" + so.tunumber + "|" + Math.Round(so.amount, so.xslength) + "|" + so.unit + "|" + (so.loss > 0 ? so.loss + "%" : "") + "|" + so.remark;
                        datas.Add(rs); index++;
                        var third = list.Where(x => x.parent_Id == so.bomId).ToList();
                        for (int t = 0; t < third.Count; t++)
                        {
                            var to = third[t];
                            var rt = index.ToString() + "|" + (i + 1).ToString() + "." + (s + 1).ToString() + "." + (t + 1).ToString() + "|" + to.materialCate + "|" + to.materialNo + "|" + to.materialName + "|" + to.materialModel + "|" + to.tunumber + "|" + Math.Round(to.amount, to.xslength) + "|" + to.unit + "|" + (to.loss > 0 ? to.loss + "%" : "") + "|" + to.remark;
                            datas.Add(rt); index++;
                            var four = list.Where(x => x.parent_Id == to.bomId).ToList();
                            for (int f = 0; f < four.Count; f++)
                            {
                                var fo = four[f];
                                var rf = index.ToString() + "|" + (i + 1).ToString() + "." + (s + 1).ToString() + "." + (t + 1).ToString() + "." + (f + 1).ToString() + "|" + fo.materialCate + "|" + fo.materialNo + "|" + fo.materialName + "|" + fo.materialModel + "|" + fo.tunumber + "|" + Math.Round(fo.amount, fo.xslength) + "|" + fo.unit + "|" + (fo.loss > 0 ? fo.loss + "%" : "") + "|" + fo.remark;
                                datas.Add(rf); index++;

                                var five = list.Where(x => x.parent_Id == fo.bomId).ToList();
                                for (int v = 0; v < five.Count; v++)
                                {
                                    var vo = five[f];
                                    var rv = index.ToString() + "|" + (i + 1).ToString() + "." + (s + 1).ToString() + "." + (t + 1).ToString() + "." + (f + 1).ToString() + "." + (v + 1).ToString() + "|" + vo.materialCate + "|" + vo.materialNo + "|" + vo.materialName + "|" + vo.materialModel + "|" + vo.tunumber + "|" + Math.Round(vo.amount, vo.xslength) + "|" + vo.unit + "|" + (vo.loss > 0 ? vo.loss + "%" : "") + "|" + vo.remark;
                                    datas.Add(rv); index++;


                                }

                            }

                        }
                    }
                }
                #endregion


                //for (int i = 0; i < f.Count; i++)
                //{
                //    var p = f[i];
                //    var row = (i + 1).ToString() + "|" + p.materialCate + "|" + p.materialNo + "|" + p.materialName + "|" + p.materialModel + "|" + p.tunumber + "|" + Math.Round(p.amount, p.xslength) + "|" + p.unit + "|" + p.remark;
                //    datas.Add(row);
                //}
                for (int i = 0; i < virtusls.Count; i++)
                {
                    var p = virtusls[i];
                    var row = index.ToString() + "|";
                    row += "|";
                    row += "其他科目|";
                    row += "|";
                    row += p.virtualName + "|";
                    row += "单价:|";
                    row += Math.Round(p.vPrice, 2) + "|";
                    row += Math.Round(p.vAmount, 2) + "|";
                    row += "||";
                    row += p.remark;
                    datas.Add(row); index++;
                }
                var root = "||" + one.materialCate + "|" + one.materialNo + "|" + one.materialName + "|" + one.materialModel + "|" + one.tunumber + "||" + one.unit + "||" + one.remark;

                datas.Insert(0, root);
                var msg = FileHelper.ExportEasy(head, datas);
                return Content(msg);
                #endregion
            }
            else
            {
                V_BomMaterial model = new V_BomMaterial();
                var ddlcate = bomService.GetBomNodeCate().Select(x => new SelectListItem { Text = x.text, Value = x.text }).ToList();
                string bomjson = "";
                if (id > 0 && type == "edit")
                {
                    model = bomService.GetBomMaterial().FirstOrDefault(p => p.bomId == id);
                    var obj = bomService.GetOneBom(id, 0);
                    bomjson = JsonHelper.ToJson(obj);
                }
                else type = "add";
                data.id = id;
                data.type = type;
                data.model = model;
                data.bomjson = bomjson;
                data.ddlcate = ddlcate;
                return View(data);
            }
        }
        [AjaxAction(ForAction = "childbomlist", ForController = "bom")]
        public ActionResult childbomnode()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            int id = WebRequest.GetInt("id", 0);
            string type = WebRequest.GetString("type");
            V_BomMaterial model = new V_BomMaterial();
            var ddlcate = bomService.GetBomNodeCate().Select(x => new SelectListItem { Text = x.text, Value = x.text }).ToList();
            if (type == "edit")
            {
                model = bomService.GetBomMaterial().FirstOrDefault(p => p.bomId == id);
                var mc = ddlcate.FirstOrDefault(p => p.Value == model.materialCate);
                if (mc != null) mc.Selected = true;
            }
            data.id = id;
            data.type = type;
            data.model = model;
            data.ddlcate = ddlcate;
            return View(data);
        }
        /// <summary>
        /// bom添加子BOM
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AjaxAction(ForAction = "childbomlist", ForController = "bom")]
        public ActionResult savechildbomnode()
        {
            int id = WebRequest.GetInt("id", 0);
            string name = WebRequest.GetString("name");
            string remark = WebRequest.GetString("remark");
            string version = WebRequest.GetString("version");
            int? parent_id = WebRequest.GetInt("parent", 0);
            int child = WebRequest.GetInt("child", 0);
            string type = WebRequest.GetString("type");
            if (type == "add")
            {
                parent_id = id;
                id = 0;
            }

            if (parent_id == 0) parent_id = null;
            var r = bomService.SaveChildBom(type, id, parent_id, child, remark);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [AjaxAction(ForAction = "childbomlist", ForController = "bom")]
        public ActionResult childbomtree(string type, int id)
        {
            if (type == "export")
            {
                #region 导出
                var one = bomService.GetBomMaterial().FirstOrDefault(p => p.bomId == id);
                var list = bomService.GetChildBomMaterial(id, 0);
                var virtusls = ServiceDB.Instance.QueryModelList<BomVirtual>("select * from BomVirtual where bomId=" + id);
                string[] head = new string[11] { "序号", "层级", "属性", "物料编码", "物料名称", "物料规格", "物料图号", "基本用量", "单位", "损耗率", "备注" };

                List<string> datas = new List<string>();
                #region 构造
                var first = list.Where(p => p.parent_Id == id).ToList();
                var index = 1;
                for (int i = 0; i < first.Count; i++)
                {
                    var p = first[i];
                    var row = index.ToString() + "|" + (i + 1).ToString() + "|" + p.materialCate + "|" + p.materialNo + "|" + p.materialName + "|" + p.materialModel + "|" + p.tunumber + "|" + Math.Round(p.amount, p.xslength) + "|" + p.unit + "|" + (p.loss > 0 ? p.loss + "%" : "") + "|" + p.remark;
                    datas.Add(row); index++;
                    var second = list.Where(x => x.parent_Id == p.bomId).ToList();
                    for (int s = 0; s < second.Count; s++)
                    {
                        var so = second[s];
                        var rs = index.ToString() + "|" + (i + 1).ToString() + "." + (s + 1).ToString() + "|" + so.materialCate + "|" + so.materialNo + "|" + so.materialName + "|" + so.materialModel + "|" + so.tunumber + "|" + Math.Round(so.amount, so.xslength) + "|" + so.unit + "|" + (so.loss > 0 ? so.loss + "%" : "") + "|" + so.remark;
                        datas.Add(rs); index++;
                        var third = list.Where(x => x.parent_Id == so.bomId).ToList();
                        for (int t = 0; t < third.Count; t++)
                        {
                            var to = third[t];
                            var rt = index.ToString() + "|" + (i + 1).ToString() + "." + (s + 1).ToString() + "." + (t + 1).ToString() + "|" + to.materialCate + "|" + to.materialNo + "|" + to.materialName + "|" + to.materialModel + "|" + to.tunumber + "|" + Math.Round(to.amount, to.xslength) + "|" + to.unit + "|" + (to.loss > 0 ? to.loss + "%" : "") + "|" + to.remark;
                            datas.Add(rt); index++;
                            var four = list.Where(x => x.parent_Id == to.bomId).ToList();
                            for (int f = 0; f < four.Count; f++)
                            {
                                var fo = four[f];
                                var rf = index.ToString() + "|" + (i + 1).ToString() + "." + (s + 1).ToString() + "." + (t + 1).ToString() + "." + (f + 1).ToString() + "|" + fo.materialCate + "|" + fo.materialNo + "|" + fo.materialName + "|" + fo.materialModel + "|" + fo.tunumber + "|" + Math.Round(fo.amount, fo.xslength) + "|" + fo.unit + "|" + (fo.loss > 0 ? fo.loss + "%" : "") + "|" + fo.remark;
                                datas.Add(rf); index++;

                                var five = list.Where(x => x.parent_Id == fo.bomId).ToList();
                                for (int v = 0; v < five.Count; v++)
                                {
                                    var vo = five[f];
                                    var rv = index.ToString() + "|" + (i + 1).ToString() + "." + (s + 1).ToString() + "." + (t + 1).ToString() + "." + (f + 1).ToString() + "." + (v + 1).ToString() + "|" + vo.materialCate + "|" + vo.materialNo + "|" + vo.materialName + "|" + vo.materialModel + "|" + vo.tunumber + "|" + Math.Round(vo.amount, vo.xslength) + "|" + vo.unit + "|" + (vo.loss > 0 ? vo.loss + "%" : "") + "|" + vo.remark;
                                    datas.Add(rv); index++;


                                }

                            }

                        }
                    }
                }
                #endregion
                for (int i = 0; i < virtusls.Count; i++)
                {
                    var p = virtusls[i];
                    var row = index.ToString() + "|";
                    row += "|";
                    row += "其他科目|";
                    row += "|";
                    row += p.virtualName + "|";
                    row += "单价:|";
                    row += Math.Round(p.vPrice, 2) + "|";
                    row += Math.Round(p.vAmount, 2) + "|";
                    row += "||";
                    row += p.remark;
                    datas.Add(row); index++;
                }
                var root = "||" + one.materialCate + "|" + one.materialNo + "|" + one.materialName + "|" + one.materialModel + "|" + one.tunumber + "||" + one.unit + "||" + one.remark;

                datas.Insert(0, root);
                var msg = FileHelper.ExportEasy(head, datas);
                return Content(msg);
                #endregion
            }
            else
            {
                if (type == "edit" && !Masterpage.CheckRight("childbom_update")) return RedirectToAction("login", "account");
                if (type == "view" && !Masterpage.CheckRight("childbom_view")) return RedirectToAction("login", "account");
                dynamic data = new System.Dynamic.ExpandoObject();
                string bomjson = "";
                var one = bomService.GetBomMaterial().FirstOrDefault(p => p.bomId == id);// ServiceDB.Instance.QueryOneModel<V_BomCostModel>(" select * from V_BomCostModel  where bomid=" + id);
                var child = bomService.GetBomMaterial().Where(p => p.parent_Id == id).ToList();// ServiceDB.Instance.QueryModelList<V_BomCostModel>(" select * from V_BomCostModel  where parent_Id=" + id).ToList();


                var list = bomService.GetBomTreeGrid(child, 1);

                var result = new { canclick = 1, isroot = 1, version = one.version, ischild = one.isChild, cate = one.materialCate, km = 0, id = one.bomId, no = one.materialNo, name = one.isChild ? one.bomName : one.materialName, model = one.otherProject, amount = one.isChild ? "" : Math.Round(one.amount, one.xslength).ToString(), useunit = "", unit = one.unit, unit2 = "", remark = one.remark, index = 0, children = list };

                List<object> ol = new List<object>();
                ol.Add(result);
                bomjson = JsonHelper.ToJson(ol);
                data.one = one;
                data.bomjson = bomjson;
                data.type = type;
                return View(data);
            }
        }        
        
        /// <summary>
        /// 子bom复制
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AjaxAction(ForAction = "childbomlist", ForController = "bom")]
        public ActionResult savechildbomcopy()
        {
            int from = WebRequest.GetInt("from", 0);
            int id = WebRequest.GetInt("id", 0);
            string name = WebRequest.GetString("name");
            string remark = WebRequest.GetString("remark");
            string version = WebRequest.GetString("version");
            string code = WebRequest.GetString("code");
            var had = ServiceDB.Instance.QueryOneModel<BomMain>("select * from BomMain where bomname='" + name + "' and version='" + version + "'");
            if (had != null)
            {
                return Json(new ReturnValue { status = false, message = name + version + "的子BOM已存在，不可重复添加" }, JsonRequestBehavior.AllowGet);
            }
            var guid = Guid.NewGuid();
            string sql = "insert into BomMain (materialCate,loss,rootCost,status,version,remark,isChild,bomName,startDate,bomguid)values('" + code + "',0,0,1,'" + version + "','" + remark + "',1,'" + name + "',getdate(),'" + guid + "') ";
            var row = ServiceDB.Instance.ExecuteSqlCommand(sql);
            if (row == 1)
            {
                var me = ServiceDB.Instance.QueryOneModel<BomMain>("select * from BomMain where bomguid='" + guid + "'");
                var child = ServiceDB.Instance.QueryModelList<BomMain>("select * from BomMain where parent_Id=" + from).ToList();
                if (child != null && child.Count>0)
                {
                    foreach (var item in child)
                    {
                        bomService.SaveChildBomNode(me.bomId, item.bomId);
                    }
                }

            }
            ReturnValue rv = new ReturnValue();
            rv.status = row == 1;
            rv.message = row == 1 ? "复制成功" : "复制失败";

            return Json(rv, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// bom复制
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AjaxAction(ForAction = "bomlist", ForController = "bom")]
        public ActionResult savebomcopy()
        {
            int from = WebRequest.GetInt("from", 0);
            int id = WebRequest.GetInt("id", 0);
            string remark = WebRequest.GetString("remark");
            string material = WebRequest.GetString("material");
            string newmaterial = WebRequest.GetString("newmaterial");
            string version = WebRequest.GetString("version");
            var had = ServiceDB.Instance.QueryOneModel<BomMain>("select * from BomMain where materialNo='" + newmaterial + "' and version='" + version + "' and parent_Id is null");
            if (had != null)
            {
                return Json(new ReturnValue { status = false, message = had.materialNo+version + "该版本号的BOM已存在，不可重复添加" }, JsonRequestBehavior.AllowGet);
            }
            var guid = Guid.NewGuid();
            string sql = "insert into BomMain (materialNo,loss,rootCost,status,version,remark,isChild,startDate,bomguid)values('" + newmaterial + "',0,0,1,'" + version + "','" + remark + "',0,getdate(),'" + guid + "') ";
            var row = ServiceDB.Instance.ExecuteSqlCommand(sql);
            if (row == 1)
            {
                var me = ServiceDB.Instance.QueryOneModel<BomMain>("select * from BomMain where bomguid='" + guid + "'");
                var child = ServiceDB.Instance.QueryModelList<BomMain>("select * from BomMain where parent_Id=" + from).ToList();
                if (child != null && child.Count>0)
                {
                    foreach (var item in child)
                    {
                        bomService.SaveChildBomNode(me.bomId, item.bomId);
                    }
                }

            }
            ReturnValue rv = new ReturnValue();
            rv.status = row == 1;
            rv.message = row == 1 ? "复制成功" : "复制失败";

            return Json(rv, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
