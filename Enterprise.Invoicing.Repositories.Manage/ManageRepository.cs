using Enterprise.Invoicing.Entities.Models;
using Enterprise.Invoicing.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Invoicing.Repositories
{
    public class ManageRepository : BasicRepository<Department>, IManageRepository
    {
        public ManageRepository(): base(new InvoicingContext()){ }
        public ManageRepository(InvoicingContext context) : base(context) { }


        #region 部门
        public IQueryable<Department> GetDepartmentList()
        {
            return from d in context.Departments orderby d.depId ascending select d;
        }
        public ReturnValue SaveDepartment(int id, bool valid, string depName, string phone, string leader, string remark)
        {
            using (var c =new InvoicingContext())
            {
                try
                {
                    var model = c.Departments.FirstOrDefault(p => p.depId == id);
                    var hadname = c.Departments.FirstOrDefault(p => p.depName == depName);
                    if (model == null)
                    {
                        #region 添加
                        if (hadname != null) return new ReturnValue { status = false, message = "名称已存在，不能重复添加" };
                        model = new Department();
                        model.depName = depName;
                        model.leader = leader;
                        model.phone = phone;
                        model.remark = remark;
                        model.valid = valid;
                        c.Departments.Add(model);
                        #endregion
                    }
                    else
                    {
                        #region 修改
                        if (hadname != null && hadname.depId != id) return new ReturnValue { status = false, message = "名称已存在，不能重复添加" };
                        model.depName = depName;
                        model.leader = leader;
                        model.phone = phone;
                        model.valid = valid;
                        model.remark = remark;
                        c.Entry(model).State = EntityState.Modified;
                        #endregion
                    }
                    c.SaveChanges(); return new ReturnValue { status = true };
                }
                catch (Exception ex)
                {
                    return new ReturnValue { status = false, message = "操作失败：" + ex.Message };
                }
            }
        }
        public ReturnValue DeleteDepartment(int id)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.Departments.FirstOrDefault(p => p.depId == id);
                    c.Departments.Remove(model);
                    c.SaveChanges(); return new ReturnValue { status = true };
                }
                catch (Exception ex)
                {
                    return new ReturnValue { status = false, message = "操作失败：" + ex.Message };
                }
            }
        }
        #endregion

        #region 员工
        public IQueryable<EmployeeModel> GetEmployeeList()
        {
            var list = from e in context.Employees
                       join d in context.Departments on e.depId equals d.depId
                       join r in context.Roles on e.roleSn equals r.roleSn into g
                       from x in g.DefaultIfEmpty()
                       orderby e.status ascending
                       select new EmployeeModel
                       {
                           depId = d.depId,
                           depName = d.depName,
                           duty = e.duty,
                           email = e.email,
                           isUser = e.isUser,
                           mobile = e.mobile,
                           remark = e.remark,
                           roleName = x == null ? "" : x.roleName,
                           roleSn = x == null ? 0 : x.roleSn,
                           showPrice = x == null ? false : x.showPrice,
                           staffId = e.staffId,
                           staffName = e.staffName,
                           status = e.status,
                           userId = e.userId,
                           userPwd = e.userPwd,
                           rigthType = e.rigthType
                       };
            return list;
        }
        public ReturnValue SaveEmployee(int id, int depId, string name, string mobile, string email, string duty, bool valid, string remark)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.Employees.FirstOrDefault(p => p.staffId == id);
                    var hadname = c.Employees.FirstOrDefault(p => p.staffName == name);
                    if (model == null)
                    {
                        #region 添加
                        if (hadname != null) return new ReturnValue { status = false, message = "名称已存在，不能重复添加" };
                        model = new Employee();
                        model.depId = depId;
                        model.duty = duty;
                        model.email = email;
                        model.isUser = false;
                        model.mobile = mobile;
                        model.remark = remark;
                        model.staffName = name;
                        model.status = valid;
                        c.Employees.Add(model);
                        #endregion
                    }
                    else
                    {
                        #region 修改
                        if (hadname != null && hadname.staffId != id) return new ReturnValue { status = false, message = "名称已存在，不能重复添加" };
                        model.depId = depId;
                        model.duty = duty;
                        model.email = email;
                        model.remark = remark; 
                        model.mobile = mobile;
                        model.remark = remark;
                        model.staffName = name; 
                        model.status = valid;
                        c.Entry(model).State = EntityState.Modified;
                        #endregion
                    }
                    c.SaveChanges(); return new ReturnValue { status = true };
                }
                catch (Exception ex)
                {
                    return new ReturnValue { status = false, message = "操作失败：" + ex.Message };
                }
            }
        }
        public ReturnValue DeleteEmployee(int id)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.Employees.FirstOrDefault(p => p.staffId == id);
                    c.Employees.Remove(model);
                    c.SaveChanges(); return new ReturnValue { status = true };
                }
                catch (Exception ex)
                {
                    return new ReturnValue { status = false, message = "操作失败：" + ex.Message };
                }
            }
        }
        public bool ChangeEmployeeStatus(int id, bool status)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.Employees.FirstOrDefault(p => p.staffId == id);
                    model.status = status;
                    c.SaveChanges(); return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        #endregion

        #region 物料
        public IQueryable<Material> GetMaterialList()
        {
            return from m in context.Materials orderby m.category ascending select m;
        }
        public ReturnValue SaveMaterial(string type, string fix, string no, bool valid, string name, string bigcate, string category, string model, string unit, string orderno, string remark, string fastcode, string pinyin, string tunumber, int xslength)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var m = c.Materials.FirstOrDefault(p => p.materialNo == no);
                    var mn = c.Materials.FirstOrDefault(p => p.materialName == name && p.materialModel == model);
                    if (tunumber == null) tunumber = "";
                    if (type == "add")
                    {
                        #region 添加 
                        var ht = c.Materials.FirstOrDefault(p => p.tunumber == tunumber&& p.tunumber!="");
                        if (ht != null) return new ReturnValue { status = false, message = "物料已存在，不能重复添加!" };
                        if (m != null || mn != null) return new ReturnValue { status = false, message = "物料已存在，不能重复添加" };
                        m = new Material();
                        m.category = category;
                        m.bigcate = bigcate;
                        m.materialModel = model;
                        m.materialName = name;
                        m.materialNo = GetNewNo(fix);// no;
                        m.orderNo = orderno;
                        m.remark = remark;
                        m.unit = unit;
                        m.valid = valid;
                        m.pinyin = pinyin;
                        m.fastcode = fastcode;
                        m.tunumber = tunumber; m.xslength = xslength;
                        c.Materials.Add(m);
                        #endregion
                    }
                    else if (type == "edit")
                    {
                        #region 修改
                        if (mn != null && m.materialNo != mn.materialNo) return new ReturnValue { status = false, message = "物料已存在，不能重复添加" };
                        var ht = c.Materials.FirstOrDefault(p => p.tunumber == tunumber && p.tunumber != "" && m.materialNo != p.materialNo);
                        if (ht != null) return new ReturnValue { status = false, message = "物料已存在，不能重复添加!" };
                        m.category = category;
                        m.bigcate = bigcate;
                        m.materialModel = model;
                        m.materialName = name;
                        m.orderNo = orderno;
                        m.remark = remark;
                        m.unit = unit; m.valid = valid; m.xslength = xslength;
                        m.pinyin = pinyin;
                        m.fastcode = fastcode;
                        m.tunumber = tunumber;
                        c.Entry(m).State = EntityState.Modified;
                        #endregion
                    }
                    else return new ReturnValue { status = false, message = "操作类别错误" };
                    c.SaveChanges(); return new ReturnValue { status = true };
                }
                catch (Exception ex)
                {
                    return new ReturnValue { status = false, message = "操作失败：" + ex.Message };
                }
            }
        }
        public ReturnValue DeleteMaterial(string no)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.Materials.FirstOrDefault(p => p.materialNo == no);
                    c.Materials.Remove(model);
                    c.SaveChanges(); return new ReturnValue { status = true };
                }
                catch (Exception ex)
                {
                    return new ReturnValue { status = false, message = "操作失败：" + ex.Message };
                }
            }
        }
        public List<KeyValue> GetFirstCate()
        {
            List<KeyValue> r = new List<KeyValue>();
            var data = context.Dictionaries.FirstOrDefault(p => p.dictionaryKey == "MaterialCategory");
            if (data != null)
            {
                string[] v = data.dictionaryValue.Split('|');
                foreach (var item in v)
                {
                     string[] t = item.Split(':');
                     r.Add(new KeyValue { text = t[0], value = t[0] });
                }
            }
            return r;
        }
        public List<KeyValue> GetSecondCate(string cate)
        {
            List<KeyValue> r = new List<KeyValue>();
            var data = context.Dictionaries.FirstOrDefault(p => p.dictionaryKey == "MaterialCategory");
            if (data != null)
            {
                string[] v = data.dictionaryValue.Split('|');
                foreach (var item in v)
                {
                    string[] t = item.Split(':');
                    if (t[0] == cate)
                    {
                        string[] c = t[1].Split(',');
                        foreach (var ci in c)
                        {

                            r.Add(new KeyValue { text = ci, value = ci });
                        }
                        break;
                    }
                }
            }
            return r;
        }

        public string GetNewNo(string fix)
        {
            var had = (from m in context.Materials where m.materialNo.StartsWith(fix) orderby m.materialNo descending select m).FirstOrDefault();  //context.Materials.FirstOrDefault(p=>p.materialNo.StartsWith(fix)
            if (had == null) return fix + "0001";
            else
            {
                var last = Convert.ToInt32(had.materialNo.Replace(fix, "")) + 1;
                if (last < 10) return fix + "000" + last.ToString();
                else if (last < 100 && last > 9) return fix + "00" + last.ToString();
                else if (last < 1000 && last > 99) return fix + "0" + last.ToString();
                else return fix + last.ToString();
            }
        }
        #endregion

        #region 供应商和客户
        public IQueryable<Supplier> GetSupplierList()
        {
            return from s in context.Suppliers orderby s.type ascending select s;
        }
        public ReturnValue SaveSupplier(int id, int type, string name, string person, string phone, string address, bool valid, string remark)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.Suppliers.FirstOrDefault(p => p.supplierId == id);
                    var hadname = c.Suppliers.FirstOrDefault(p => p.type == type && p.supplierName == name);
                    if (model == null)
                    {
                        #region 添加
                        if (hadname != null) return new ReturnValue { status = false, message = "名称已存在，不能重复添加" };
                        model = new Supplier();
                        model.address = address;
                        model.phone = phone;
                        model.remark = remark;
                        model.supplierName = name;
                        model.type = type;
                        model.person = person;
                        model.valid = valid;
                        c.Suppliers.Add(model);
                        #endregion
                    }
                    else
                    {
                        #region 修改
                        if (hadname != null && hadname.supplierId != id) return new ReturnValue { status = false, message = "同种物料已存在，不能重复添加" };
                        model.address = address;
                        model.phone = phone;
                        model.remark = remark;
                        model.supplierName = name;
                        model.person = person;
                        model.valid = valid;
                        c.Entry(model).State = EntityState.Modified;
                        #endregion
                    }
                    c.SaveChanges(); return new ReturnValue { status = true };
                }
                catch (Exception ex)
                {
                    return new ReturnValue { status = false, message = "操作失败：" + ex.Message };
                }
            }
        }
        public ReturnValue DeleteSupplier(int id)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.Suppliers.FirstOrDefault(p => p.supplierId == id);
                    c.Suppliers.Remove(model);
                    c.SaveChanges(); return new ReturnValue { status = true };
                }
                catch (Exception ex)
                {
                    return new ReturnValue { status = false, message = "操作失败：" + ex.Message };
                }
            }
        }
        #endregion

        #region 报销单
        public string GetBillNo()
        {
            var last = (from r in context.BillCosts where r.billType==0 orderby r.createDate descending select r).FirstOrDefault();
            if (last != null)
            {
                if (last.createDate.Month != DateTime.Now.Month)
                {
                    return "BX" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
                }
                else
                {
                    var l = last.billNo.Split('-');
                    var n = (Convert.ToInt32(l[1]) + 1).ToString();
                    if (n.Length == 1) n = "000" + n;
                    else if (n.Length == 2) n = "00" + n;
                    else if (n.Length == 3) n = "0" + n;
                    return "BX" + DateTime.Now.ToString("yyyyMMdd") + "-" + n;
                }
            }
            else
                return "BX" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
        }

        public bool ChangeBillStatus(string no, int status, int staff)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.BillCosts.FirstOrDefault(p => p.billNo == no);
                    if (status == -1)
                    {
                        model.status = 4;
                        model.checkRes = false;
                    }
                    else
                    {
                        if (model.status == 0)
                        {
                            model.status = 1;
                            model.checkRes = true;
                        }
                        else if (model.status == 1)
                        {
                            model.status = 0;
                            model.checkRes = false;

                        }
                        else if (model.status == 4)
                        {
                            model.status = 0;

                            model.checkRes = false;
                        }

                    }
                    model.staffCheck = staff;
                    c.Entry(model).State = EntityState.Modified;
                    c.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool ChangeSettleStatus(string no, int status, int staff)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.Settlements.FirstOrDefault(p => p.settleNo == no);
                    var person = c.Employees.FirstOrDefault(p => p.staffId == staff);
                    if (status == -1)
                    {
                        model.status = 4;
                    }
                    else
                    {
                        if (model.status == 0)
                        {
                            model.status = 1;
                        }
                        else if (model.status == 1)
                        {
                            model.status = 0;

                        }
                        else if (model.status == 4)
                        {
                            model.status = 0;
                        }

                    }
                    model.checkStaff = person != null ? person.staffName : "";
                    model.checkDate = DateTime.Now;
                    c.Entry(model).State = EntityState.Modified;
                    c.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }    
        #endregion

        #region 特采单
        public string GetTCNo()
        {
            var last = (from r in context.BillCosts where r.billType == 1 orderby r.createDate descending select r).FirstOrDefault();
            if (last != null)
            {
                if (last.createDate.Month != DateTime.Now.Month)
                {
                    return "TC" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
                }
                else
                {
                    var l = last.billNo.Split('-');
                    var n = (Convert.ToInt32(l[1]) + 1).ToString();
                    if (n.Length == 1) n = "000" + n;
                    else if (n.Length == 2) n = "00" + n;
                    else if (n.Length == 3) n = "0" + n;
                    return "TC" + DateTime.Now.ToString("yyyyMMdd") + "-" + n;
                }
            }
            else
                return "TC" + DateTime.Now.ToString("yyyyMMdd") + "-0001";
        }
        #endregion


        #region 消息
        public bool SaveSendMessage(MsgSend send)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    c.MsgSends.Add(send);
                    c.SaveChanges(); return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        #endregion
    }
}
