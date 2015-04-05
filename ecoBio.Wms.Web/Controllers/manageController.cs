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
    public class manageController : BaseController
    {
        private ManageService manageService; private StockInService stockinService; 
        public manageController(IManageRepository _manageRepository,IStockInRepository _stockinrepository)
        {
            manageService = new ManageService(_manageRepository); stockinService = new StockInService(_stockinrepository);
        }



        #region 部门管理

        public ActionResult department(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var key = WebRequest.GetString("key", true);
            data.key = key;
            var list = manageService.GetDepartmentList(key);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = "&key=" + key;
            return View(data);
        }
        [AjaxAction(ForAction = "department", ForController = "manage")]
        public ActionResult depone()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            int id = WebRequest.GetInt("id", 0);
            string type = WebRequest.GetString("type");
            Department model = new Department();
            if (id > 0 && type == "edit")
            {
                model = manageService.GetDepartmentList("").FirstOrDefault(p => p.depId == id);
            }
            else type = "add";
            data.id = id;
            data.type = type;
            data.model = model;
            return View(data);
        }
        [HttpPost]
        [AjaxAction(ForAction = "department", ForController = "manage")]
        public ActionResult savedepment()
        {
            //int id, string depName, string phone, string leader, string remark
            int id = WebRequest.GetInt("id", 0);
            int rad = WebRequest.GetInt("rad", 0);
            string depname = WebRequest.GetString("depname");
            string phone = WebRequest.GetString("phone");
            string leader = WebRequest.GetString("leader");
            string remark = WebRequest.GetString("remark");
            bool valid = rad != 0 ? true : false;
            var r = manageService.SaveDepartment(id,valid, depname, phone, leader, remark);
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AjaxAction(ForAction = "department", ForController = "manage")]
        public ActionResult deletedep()
        {
            int id = WebRequest.GetInt("id", 0);
            var r = manageService.DeleteDepartment(id);
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 员工管理

        public ActionResult employee(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var key = WebRequest.GetString("key", true);
            data.key = key;
            var list = manageService.GetEmployeeList(key);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = "&key=" + key;
            return View(data);
        }
        [AjaxAction(ForAction = "employee", ForController = "manage")]
        public ActionResult employeeone()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            int id = WebRequest.GetInt("id", 0);
            string type = WebRequest.GetString("type");
            EmployeeModel model = new EmployeeModel();
            if (id > 0 && type == "edit")
            {
                model = manageService.GetEmployeeList("").FirstOrDefault(p => p.staffId == id);
            }
            else type = "add";
            var deplist = manageService.GetDepartmentList("").
                Where(p => p.valid == true).
                Select(p => new SelectListItem { Text = p.depName, Value = p.depId.ToString() }).ToList();
            data.deplist = deplist;
            data.id = id;
            data.type = type;
            data.model = model;
            return View(data);
        }
        [HttpPost]
        [AjaxAction(ForAction = "employee", ForController = "manage")]
        public ActionResult saveemployee()
        {
            //id,depId, string name, string mobile, string email, string duty, string remark
            int id = WebRequest.GetInt("id", 0);
            int rad = WebRequest.GetInt("rad", 0);
            int depId = WebRequest.GetInt("depId",0);
            string name = WebRequest.GetString("name");
            string mobile = WebRequest.GetString("mobile");
            string email = WebRequest.GetString("email");
            string duty = WebRequest.GetString("duty");
            string remark = WebRequest.GetString("remark");
            bool valid = rad != 0 ? true : false;
            var r = manageService.SaveEmployee(id, depId, name, mobile, email, duty,valid, remark);
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AjaxAction(ForAction = "employee", ForController = "manage")]
        public ActionResult deleteemployee()
        {
            int id = WebRequest.GetInt("id", 0);
            var r = manageService.DeleteEmployee(id);
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 物料管理

        public ActionResult material(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var key = WebRequest.GetString("key", false);
            data.key = key;
            var list = manageService.GetMaterialList(key);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = "&key=" + key;
            var mc = stockinService.GetMaterialCategory().Select(x => new SelectListItem { Text = x.text, Value = x.value }).ToList(); 
            data.mc = mc;
            return View(data);
        }
        [AjaxAction(ForAction = "material", ForController = "manage")]
        public ActionResult materialone()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            string id = WebRequest.GetString("id");
            string type = WebRequest.GetString("type");
            Material model = new Material();
            if (id !="" && type == "edit")
            {
                model = manageService.GetMaterialList("").FirstOrDefault(p => p.materialNo == id);
            }
            else type = "add";

            var units = stockinService.GetMaterialUnit().Select(x => new SelectListItem { Text = x.text, Value = x.value }).ToList();
            var units2 = stockinService.GetMaterialUnit().Select(x => new SelectListItem { Text = x.text, Value = x.value }).ToList(); 
            data.units = units;
            data.units2 = units2;
            var nofix = stockinService.GetMaterialNoFix().Select(x => new SelectListItem { Text = x.text, Value = x.value }).ToList();
            data.nofix = nofix;
            data.id = id;
            data.type = type;
            data.model = model;
            return View(data);
        }
        [HttpPost]
        [AjaxAction(ForAction = "material", ForController = "manage")]
        public ActionResult savematerial()
        {
            //id,depId, string name, string mobile, string email, string duty, string remark
            string type = WebRequest.GetString("type");
            string no = WebRequest.GetString("no");
            int rad = WebRequest.GetInt("rad", 0);
            int xslength = WebRequest.GetInt("xslength", 0);
            string mc = WebRequest.GetString("mc");
            string fix = WebRequest.GetString("fix");
            string bmc = WebRequest.GetString("bmc");
            string name = WebRequest.GetString("name");
            string model = WebRequest.GetString("model");
            string unit = WebRequest.GetString("unit");
            string remark = WebRequest.GetString("remark");
            string pinyin = WebRequest.GetString("pinyin");
            string fastcode = WebRequest.GetString("fastcode");
            string tunumber = WebRequest.GetString("tunumber");
            string image = WebRequest.GetString("image");

            double xs = WebRequest.GetFloat("xs", 1);
            string unit2 = WebRequest.GetString("unit2");

            bool valid = rad != 0 ? true : false;
            var r = manageService.SaveMaterial(type, fix, no, valid, name, bmc, mc, model, unit, "", remark, fastcode, pinyin, tunumber, xslength);
            if (r.status) ServiceDB.Instance.ExecuteSqlCommand("update material set unit2='" + unit2 + "',image='" + image + "',ratio=" + xs + " where materialno='" + no + "'");
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AjaxAction(ForAction = "material", ForController = "manage")]
        public ActionResult deletematerial()
        {
            string id = WebRequest.GetString("id");
            var r = manageService.DeleteMaterial(id);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [AjaxAction(ForAction = "material", ForController = "manage")]
        public ActionResult materialexcel(string where)
        {
            var key = WebRequest.GetString("key", true);
            var list = manageService.GetMaterialList(key);
            string[] head = new string[9] { "序号", "物料大类", "物料类别", "物料编码", "物料名称", "物料规格", "物料图号", "单位",  "备注" };
            List<string> data = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                var p = list[i];
                var row = (i + 1).ToString() + "|";
                row += p.bigcate + "|";
                row += p.category + "|";
                row += p.materialNo + "|";
                row += p.materialName + "|";
                row += p.materialModel + "|";
                row += p.tunumber + "|";
                row += p.unit + "|";
                row += p.remark;
                data.Add(row);
            }

            var msg = FileHelper.ExportEasy(head, data);

            return Content(msg);
        }
        #endregion

        #region 供应商、客户管理

        public ActionResult supplier(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var key = WebRequest.GetString("key", true);
            data.key = key;
            var list = manageService.GetSupplierList(key).Where(p => p.type == 0);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = "&key=" + key;
            return View(data);
        }
        #region 价格管理
        [AjaxAction(ForAction = "supplier", ForController = "manage")]
        public ActionResult supplierprice(int? id)
        {
            if (!Masterpage.CheckRight("manage_supplier_price")||!id.HasValue)
            {
                return Content("非法操作！");
            }
            dynamic data = new System.Dynamic.ExpandoObject();
            var page = WebRequest.GetString("page", true);
            var name = WebRequest.GetString("name", true);
            var model = WebRequest.GetString("model", true);
            data.id = id;
            data.page = page;
            data.name = name;
            data.model = model;
            Supplier one = ServiceDB.Instance.QueryOneModel<Supplier>("select * from supplier where type=0 and supplierid=" + id.Value);
             if (one == null) return Content("供应商不存在！");
            string where = "";
            if (!string.IsNullOrEmpty(name)) where += " and (materialno like '%" + name + "%' or materialname like '%" + name + "%' or tunumber like '%" + name + "%'  or pinyin like '%" + name + "%' )";
            if (!string.IsNullOrEmpty(model)) where += " and materialmodel like '%" + model + "%'";
            var list = ServiceDB.Instance.QueryModelList<V_MaterialPriceModel>("select * from V_MaterialPriceModel where supplierid=" + id.Value + where + " order by materialname asc ");
            data.canadd = Masterpage.CheckRight("manage_supplier_priceadd") ? 1 : 0; 
            data.canedit = Masterpage.CheckRight("manage_supplier_priceupdate") ? 1 : 0;
            data.list = list;
            data.suppler = one.supplierName;
            return View(data);
        }
        //savesupplierprice", { id: id, material: hmaterial,price:p,date:d,remark
        [AjaxAction(ForAction = "supplier", ForController = "manage")]
        public ActionResult savesupplierprice(int id, int? priceid,string material,double price,string date,string remark)
        {
            if (!priceid.HasValue || (priceid.Value == 0 && !Masterpage.CheckRight("manage_supplier_priceadd")) || (priceid.Value >= 0 && !Masterpage.CheckRight("manage_supplier_priceupdate"))) return Json(new ReturnValue { status = false, message = "非法权限" }, JsonRequestBehavior.AllowGet);
            ReturnValue r = new ReturnValue { status = false };
            var row = 0;
            try
            {
                if (priceid.Value == 0) row = ServiceDB.Instance.ExecuteSqlCommand("insert into MaterialPrice values(" + id + ",'" + material + "'," + Masterpage.CurrUser.staffid + "," + price + ",'" + date + "',null,getdate(),0,'" + remark + "')");
                else row = ServiceDB.Instance.ExecuteSqlCommand("update MaterialPrice set materialNo='" + material + "',price=" + price + ",startDate='" + date + "',remark='" + remark + "' where priceid=" + priceid.Value);
                r.status = row == 1; r.message = "操作失败！";
            }
            catch (Exception ex) { r.status = false; r.message = (priceid.Value == 0 ? "添加" : "修改") + "失败！" + ex.Message; }

            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [AjaxAction(ForAction = "supplier", ForController = "manage")]
        public ActionResult deletesupplierprice(int? priceid)
        {
            try
            {
                if (!priceid.HasValue || !Masterpage.CheckRight("manage_supplier_pricedelete")) return Json(new ReturnValue { status = false, message = "非法权限" }, JsonRequestBehavior.AllowGet);
                var row = ServiceDB.Instance.ExecuteSqlCommand("delete MaterialPrice where priceid=" + priceid.Value);
                ReturnValue r = new ReturnValue() { status = (row == 1), message = "删除" + (row == 1 ? "成功" : "失败") };
                return Json(r, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) { return Json(new ReturnValue { status = false, message = "删除失败" + ex.Message }, JsonRequestBehavior.AllowGet); }
        }
        [AjaxAction(ForAction = "supplier", ForController = "manage")]
        public ActionResult checksupplierprice(int? priceid,int to)
        {
            try
            {
                if (!priceid.HasValue || !Masterpage.CheckRight("manage_supplier_pricecheck")) return Json(new ReturnValue { status = false, message = "非法权限" }, JsonRequestBehavior.AllowGet);
                var row = ServiceDB.Instance.ExecuteSqlCommand("update MaterialPrice set status=" + to + ",endDate="+(to==2?"'"+DateTime.Now.ToString("yyyy-MM-dd")+"'":"NULL")+" where priceid=" + priceid.Value);
                ReturnValue r = new ReturnValue() { status = (row == 1), message = "删除" + (row == 1 ? "成功" : "失败") };
                return Json(r, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) { return Json(new ReturnValue { status = false, message = "删除失败" + ex.Message }, JsonRequestBehavior.AllowGet); }
        }

        public ActionResult materialprice(int? page) {
            dynamic data = new System.Dynamic.ExpandoObject();
            var sup = WebRequest.GetString("sup", true);
            var mat = WebRequest.GetString("mat", true);
            var no = WebRequest.GetString("no", true);
            var type = WebRequest.GetString("type", true);
            data.mat = mat;
            data.sup = sup;
            data.no = no;
            data.type = type;
            string where = "";
            if (!string.IsNullOrEmpty(sup)) where += " and supplierName like '%" + sup + "%' ";
            if (!string.IsNullOrEmpty(type)) where += " and type=" + type + " ";
            if (!string.IsNullOrEmpty(mat)) where += " and (materialmodel like '%" + mat + "%' or materialname like '%" + mat + "%' or tunumber like '%" + mat + "%'  or pinyin like '%" + mat + "%' )";
            if (!string.IsNullOrEmpty(no)) where += " and materialNo like '%" + no + "%'";
            var list = ServiceDB.Instance.QueryModelList<V_MaterialPriceModel>("select * from V_MaterialPriceModel where supplierid>0 " + where + " order by supplierName asc,materialname asc ");

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = "&mat=" + mat + "&sup=" + sup + "&no=" + no + "&type=" + type;
            return View(data);
        }
        #endregion



        [AjaxAction(ForAction = "supplier", ForController = "manage")]
        public ActionResult supplierone()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            int id = WebRequest.GetInt("id", 0);
            string type = WebRequest.GetString("type");
            Supplier model = new Supplier();
            if (id > 0 && type == "edit")
            {
                model = manageService.GetSupplierList("").FirstOrDefault(p => p.supplierId == id);
            }
            else type = "add";
            data.id = id;
            data.type = type;
            data.model = model;
            return View(data);
        }

        public ActionResult customer(int? page)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            var key = WebRequest.GetString("key", true);
            data.key = key;
            var list = manageService.GetSupplierList(key).Where(p=>p.type==1);

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = 17;
            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            data.otherParam = "&key=" + key;
            return View(data);
        }

        #region 价格管理2
        [AjaxAction(ForAction = "customer", ForController = "manage")]
        public ActionResult customerprice(int? id)
        {
            if (!Masterpage.CheckRight("manage_supplier_price") || !id.HasValue)
            {
                return Content("非法操作！");
            }
            dynamic data = new System.Dynamic.ExpandoObject();
            var page = WebRequest.GetString("page", true);
            var name = WebRequest.GetString("name", true);
            var model = WebRequest.GetString("model", true);
            data.id = id;
            data.page = page;
            data.name = name;
            data.model = model;
            Supplier one = ServiceDB.Instance.QueryOneModel<Supplier>("select * from supplier where type=1 and supplierid=" + id.Value);
            if (one == null) return Content("客户不存在！");
            string where = "";
            if (!string.IsNullOrEmpty(name)) where += " and (materialno like '%" + name + "%' or materialname like '%" + name + "%' or tunumber like '%" + name + "%'  or pinyin like '%" + name + "%' )";
            if (!string.IsNullOrEmpty(model)) where += " and materialmodel like '%" + model + "%'";
            var list = ServiceDB.Instance.QueryModelList<V_MaterialPriceModel>("select * from V_MaterialPriceModel where supplierid=" + id.Value + where + " order by materialname asc ");
            data.canadd = Masterpage.CheckRight("manage_supplier_priceadd") ? 1 : 0;
            data.canedit = Masterpage.CheckRight("manage_supplier_priceupdate") ? 1 : 0;
            data.list = list;
            data.suppler = one.supplierName;
            return View(data);
        }
        //savesupplierprice", { id: id, material: hmaterial,price:p,date:d,remark
        [AjaxAction(ForAction = "customer", ForController = "manage")]
        public ActionResult savecustomerprice(int id, int? priceid, string material, double price, string date, string remark)
        {
            if (!priceid.HasValue || (priceid.Value == 0 && !Masterpage.CheckRight("manage_supplier_priceadd")) || (priceid.Value >= 0 && !Masterpage.CheckRight("manage_supplier_priceupdate"))) return Json(new ReturnValue { status = false, message = "非法权限" }, JsonRequestBehavior.AllowGet);
            ReturnValue r = new ReturnValue { status = false };
            var row = 0;
            try
            {
                if (priceid.Value == 0) row = ServiceDB.Instance.ExecuteSqlCommand("insert into MaterialPrice values(" + id + ",'" + material + "'," + Masterpage.CurrUser.staffid + "," + price + ",'" + date + "',null,getdate(),0,'" + remark + "')");
                else row = ServiceDB.Instance.ExecuteSqlCommand("update MaterialPrice set materialNo='" + material + "',price=" + price + ",startDate='" + date + "',remark='" + remark + "' where priceid=" + priceid.Value);
                r.status = row == 1; r.message = "操作失败！";
            }
            catch (Exception ex) { r.status = false; r.message = (priceid.Value == 0 ? "添加" : "修改") + "失败！" + ex.Message; }

            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [AjaxAction(ForAction = "customer", ForController = "manage")]
        public ActionResult deletecustomerprice(int? priceid)
        {
            try
            {
                if (!priceid.HasValue || !Masterpage.CheckRight("manage_supplier_pricedelete")) return Json(new ReturnValue { status = false, message = "非法权限" }, JsonRequestBehavior.AllowGet);
                var row = ServiceDB.Instance.ExecuteSqlCommand("delete MaterialPrice where priceid=" + priceid.Value);
                ReturnValue r = new ReturnValue() { status = (row == 1), message = "删除" + (row == 1 ? "成功" : "失败") };
                return Json(r, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) { return Json(new ReturnValue { status = false, message = "删除失败" + ex.Message }, JsonRequestBehavior.AllowGet); }
        }
        [AjaxAction(ForAction = "customer", ForController = "manage")]
        public ActionResult checkcustomerprice(int? priceid, int to)
        {
            try
            {
                if (!priceid.HasValue || !Masterpage.CheckRight("manage_supplier_pricecheck")) return Json(new ReturnValue { status = false, message = "非法权限" }, JsonRequestBehavior.AllowGet);
                var row = ServiceDB.Instance.ExecuteSqlCommand("update MaterialPrice set status=" + to + ",endDate=" + (to == 2 ? "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'" : "NULL") + " where priceid=" + priceid.Value);
                ReturnValue r = new ReturnValue() { status = (row == 1), message = "删除" + (row == 1 ? "成功" : "失败") };
                return Json(r, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) { return Json(new ReturnValue { status = false, message = "删除失败" + ex.Message }, JsonRequestBehavior.AllowGet); }
        }
        #endregion

        [AjaxAction(ForAction = "customer", ForController = "manage")]
        public ActionResult customerone()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            int id = WebRequest.GetInt("id", 0);
            string type = WebRequest.GetString("type");
            Supplier model = new Supplier();
            if (id > 0 && type == "edit")
            {
                model = manageService.GetSupplierList("").FirstOrDefault(p => p.supplierId == id);
            }
            else type = "add";
            data.id = id;
            data.type = type;
            data.model = model;
            return View(data);
        }

        [HttpPost]
        [AjaxAction(ForAction = "supplier", ForController = "manage")]
        public ActionResult savesupplier()
        {
            //int id, string depName, string phone, string leader, string remark
            int id = WebRequest.GetInt("id", 0);
            int rad = WebRequest.GetInt("rad", 0);
            int stype = WebRequest.GetInt("stype", 0);
            int fax = WebRequest.GetInt("fax", 0);
            string name = WebRequest.GetString("name");
            string phone = WebRequest.GetString("phone");
            string address = WebRequest.GetString("address");
            string person = WebRequest.GetString("person");
            string remark = WebRequest.GetString("remark");
            string no = WebRequest.GetString("no");
            bool valid = rad != 0 ? true : false;
            var r = manageService.SaveSupplier(id, stype, name, person, phone, address, valid, remark);
            if (r.status) ServiceDB.Instance.ExecuteSqlCommand("update supplier set supplierno='" + no + "',fax=" + fax + " where type=" + stype + " and suppliername='" + name + "'");
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AjaxAction(ForAction = "supplier", ForController = "manage")]
        public ActionResult deletesupplier()
        {
            int id = WebRequest.GetInt("id", 0);
            var r = manageService.DeleteSupplier(id);
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
