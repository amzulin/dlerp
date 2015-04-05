using Enterprise.Invoicing.Entities.Models;
using Enterprise.Invoicing.Repositories;
using Enterprise.Invoicing.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Invoicing.Service
{
    public class SystemService
    {
        private ISystemRepository _systemRepository;
        public SystemService(ISystemRepository systemRepository)
        {
            _systemRepository = systemRepository;
        }

        #region 用户
        public List<EmployeeModel> GetEmployeeList(string key)
        {
            var list = _systemRepository.GetEmployeeList();
            if (key != "")
            {
                return list.Where(p => p.depName.Contains(key) || p.staffName.Contains(key) || p.remark.Contains(key) || p.email.Contains(key) || p.duty.Contains(key)).ToList();
            }
            return list.ToList();
        }
        public ReturnValue SetUser(int id, bool isuer, string userid, string pwd, int role, bool valid, string remark, int utype)
        {
            return _systemRepository.SetUser(id, isuer, userid, pwd, role, valid, remark,utype);
        }
        #endregion

        #region 字典
        public List<Dictionary> GetDictionaryList(string key)
        {
            var list = _systemRepository.GetDictionaryList();
            if (key != "")
            {
                return list.Where(p => p.dictionaryKey.Contains(key) || p.dictionaryValue.Contains(key) || p.remark.Contains(key) || p.dictionaryLable.Contains(key)).ToList();
            }
            return list.ToList();
        }
        public ReturnValue SaveDictionary(string type, string key, string value, string lable, string remark)
        {
            return _systemRepository.SaveDictionary(type, key, value, lable, remark);
        }
        public ReturnValue DeleteDictionary(string key)
        {
            return _systemRepository.DeleteDictionary(key);
        }
        #endregion

        #region 角色
        public List<Role> GetRoleList(string key)
        {
            var list = _systemRepository.GetRoleList();
            if (key != "")
            {
                return list.Where(p => p.roleName.Contains(key) || p.remark.Contains(key)).ToList();
            }
            return list.ToList();
        }
        public ReturnValue SaveRole(int id, string name, string remark,bool price)
        {
            return _systemRepository.SaveRole(id, name, remark,price);
        }
        public ReturnValue DeleteRole(int id)
        {
            return _systemRepository.DeleteRole(id);
        }

        #region 角色菜单权限
        public List<Menu> GetAllMenu()
        {
            return _systemRepository.GetAllMenu().ToList();
        }
        public List<Menu> GetHadMenu(int role)
        {
            return _systemRepository.GetHadMenu(role).ToList();
        }
        public bool SaveRoleMenu(int role, string[] add, string[] delete)
        {
            return _systemRepository.SaveRoleMenu(role, add, delete);
        }
        #endregion

        #region 角色功能权限
        public List<Function> GetAllFunction()
        {
            return _systemRepository.GetAllFunction().ToList();
        }
        public List<Function> GetHadFunction(int role)
        {
            return _systemRepository.GetHadFunction(role).ToList();
        }
        public bool SaveRoleFunction(int role, string[] add, string[] delete)
        {
            return _systemRepository.SaveRoleFunction(role, add, delete);
        }
        #endregion

        #endregion

    }
}
