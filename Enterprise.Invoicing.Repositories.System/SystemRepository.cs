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
    public class SystemRepository : BasicRepository<StockOut>, ISystemRepository
    {
        public SystemRepository() : base(new InvoicingContext()) { }
        public SystemRepository(InvoicingContext context) : base(context) { }

        #region 用户
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
        public ReturnValue SetUser(int id, bool isuer, string userid, string pwd, int role, bool valid, string remark, int utype)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.Employees.FirstOrDefault(p => p.staffId == id);
                    if (model == null) return new ReturnValue { status = false, message = "员工不存在" };
                    model.isUser = isuer;
                    model.userId = userid;
                    model.status = valid;
                    model.remark = remark; model.rigthType = utype;
                    if (pwd != "") model.userPwd = pwd;
                    if (role != 0) model.roleSn = role;
                    c.Entry(model).State = EntityState.Modified;
                    c.SaveChanges();
                    return new ReturnValue { status = true };
                }
                catch (Exception ex)
                {
                    return new ReturnValue { status = false, message = "操作失败：" + ex.Message };
                }
            }
        }
        #endregion

        #region 字典
        public IQueryable<Dictionary> GetDictionaryList()
        {
            return from r in context.Dictionaries orderby r.dictionaryKey ascending select r;
        }
        public ReturnValue SaveDictionary(string type, string key, string value, string lable, string remark)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var m = c.Dictionaries.FirstOrDefault(p => p.dictionaryKey == key);
                    if (type == "add")
                    {
                        #region 添加
                        if (m != null) return new ReturnValue { status = false, message = "字典已存在，不能重复添加" };
                        m = new Dictionary();
                        m.dictionaryKey = key;
                        m.dictionaryLable = lable;
                        m.dictionaryValue = value;
                        m.remark = remark;
                        c.Dictionaries.Add(m);
                        #endregion
                    }
                    else if (type == "edit")
                    {
                        #region 修改
                        if (m == null) return new ReturnValue { status = false, message = "字典不存在" };
                        m.dictionaryLable = lable;
                        m.dictionaryValue = value;
                        m.remark = remark;
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
        public ReturnValue DeleteDictionary(string key)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.Dictionaries.FirstOrDefault(p => p.dictionaryKey == key);
                    c.Dictionaries.Remove(model);
                    c.SaveChanges(); return new ReturnValue { status = true };
                }
                catch (Exception ex)
                {
                    return new ReturnValue { status = false, message = "操作失败：" + ex.Message };
                }
            }
        }
        #endregion

        #region 角色
        public IQueryable<Role> GetRoleList()
        {
            return from r in context.Roles orderby r.roleName ascending select r;
        }
        public ReturnValue SaveRole(int id, string name, string remark, bool price)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.Roles.FirstOrDefault(p => p.roleSn == id);
                    var hadname = c.Roles.FirstOrDefault(p => p.roleName == name);
                    if (model == null)
                    {
                        #region 添加
                        if (hadname != null) return new ReturnValue { status = false, message = "角色已存在，不能重复添加" };
                        model = new Role();
                        model.remark = remark;
                        model.roleName = name; model.showPrice = price;
                        c.Roles.Add(model);
                        #endregion
                    }
                    else
                    {
                        #region 修改
                        if (hadname != null && hadname.roleSn != id) return new ReturnValue { status = false, message = "角色已存在，不能重复添加" };
                        model.remark = remark;
                        model.roleName = name; model.showPrice = price;
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
        public ReturnValue DeleteRole(int id)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    var model = c.Roles.FirstOrDefault(p => p.roleSn == id);
                    c.Roles.Remove(model);
                    c.SaveChanges(); return new ReturnValue { status = true };
                }
                catch (Exception ex)
                {
                    return new ReturnValue { status = false, message = "操作失败：" + ex.Message };
                }
            }
        }

        #region 角色菜单权限

        public IQueryable<Menu> GetAllMenu()
        {
            return from m in context.Menus orderby m.menuNo ascending select m;
        }
        public IQueryable<Menu> GetHadMenu(int role)
        {
            var list = from r in context.MenuRights
                       join m in context.Menus on r.menuNo equals m.menuNo
                       where r.roleSn == role
                       orderby r.rightSn ascending
                       select m;
            return list;
        }
        public bool SaveRoleMenu(int role, string[] add, string[] delete)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    #region 添加
                    foreach (var item in add)
                    {
                        if (item == "") continue;
                        var ha = c.MenuRights.FirstOrDefault(p => p.roleSn == role && p.menuNo == item);
                        if (ha == null)
                        {
                            MenuRight r = new MenuRight();
                            r.menuNo = item;
                            r.roleSn = role;
                            c.MenuRights.Add(r);
                        }
                    }
                    c.SaveChanges();
                    #endregion
                    #region 删除
                    foreach (var item in delete)
                    {
                        var ha = c.MenuRights.FirstOrDefault(p => p.roleSn == role && p.menuNo == item);
                        if (ha != null)
                        {
                            c.MenuRights.Remove(ha);
                        }
                    }
                    c.SaveChanges();
                    #endregion
                    return true;
                }
                catch
                { return false; }
            }
        }
        #endregion

        #region 角色功能权限

        public IQueryable<Function> GetAllFunction()
        {
            return from m in context.Functions orderby m.functionNo ascending select m;
        }
        public IQueryable<Function> GetHadFunction(int role)
        {
            var list = from r in context.FunctionRights
                       join m in context.Functions on r.functionNo equals m.functionNo
                       where r.roleSn == role
                       orderby r.rightSn ascending
                       select m;
            return list;
        }
        public bool SaveRoleFunction(int role, string[] add, string[] delete)
        {
            using (var c = new InvoicingContext())
            {
                try
                {
                    #region 添加
                    foreach (var item in add)
                    {
                        if (item == "") continue;
                        var ha = c.FunctionRights.FirstOrDefault(p => p.roleSn == role && p.functionNo == item);
                        if (ha == null)
                        {
                            FunctionRight r = new FunctionRight();
                            r.functionNo = item;
                            r.roleSn = role;
                            c.FunctionRights.Add(r);
                        }
                    }
                    c.SaveChanges();
                    #endregion
                    #region 删除
                    foreach (var item in delete)
                    {
                        var ha = c.FunctionRights.FirstOrDefault(p => p.roleSn == role && p.functionNo == item);
                        if (ha != null)
                        {
                            c.FunctionRights.Remove(ha);
                        }
                    }
                    c.SaveChanges();
                    #endregion
                    return true;
                }
                catch
                { return false; }
            }
        }
        #endregion
        #endregion
    }

    

}
