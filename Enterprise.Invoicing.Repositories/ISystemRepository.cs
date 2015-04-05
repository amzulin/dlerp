using Enterprise.Invoicing.Entities.Models;
using Enterprise.Invoicing.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Invoicing.Repositories
{
    public interface ISystemRepository
    {
        #region 用户
        IQueryable<EmployeeModel> GetEmployeeList();
        ReturnValue SetUser(int id, bool isuer, string userid, string pwd, int role, bool valid, string remark, int utype);
        #endregion

        #region 字典
        IQueryable<Dictionary> GetDictionaryList();
        ReturnValue SaveDictionary(string type, string key, string value, string lable, string remark);
        ReturnValue DeleteDictionary(string key);
        #endregion

        #region 角色
        IQueryable<Role> GetRoleList();
        ReturnValue SaveRole(int id, string name, string remark,bool price);
        ReturnValue DeleteRole(int id);

        IQueryable<Menu> GetAllMenu();
        IQueryable<Menu> GetHadMenu(int role);
        bool SaveRoleMenu(int role, string[] add, string[] delete);
        IQueryable<Function> GetAllFunction();
        IQueryable<Function> GetHadFunction(int role);
        bool SaveRoleFunction(int role, string[] add, string[] delete);
        #endregion
    }
}
