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
    public class AccountService
    {
        private IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public LoginUser GetLoginModel(string userid)
        {
            return _accountRepository.GetLoginModel(userid);
        }

        public List<Enterprise.Invoicing.Entities.Models.Menu> GetMenuByRole(int rolesn)
        {
            return _accountRepository.GetMenuByRole(rolesn);
        }
        public List<Function> GetFunctionByRole(int rolesn)
        {
            return _accountRepository.GetFunctionByRole(rolesn);
        }
        public string GetMyFunctionString(int rolesn)
        {
            var list = _accountRepository.GetFunctionByRole(rolesn);
            string result = "";
            foreach (var item in list)
            {
                result += item.functionNo + ";";
            }
            return result;
        }
        public LoginUser GetLoginUserByModel(Employee employee)
        {
            LoginUser model = new LoginUser();
            model.email = employee.email;
            model.login_time = DateTime.Now;
            model.mobile = employee.mobile;
            model.name = employee.staffName;

            //model.role_name = employee.Role != null ? employee.Role.roleName : "";
            model.role_sn = employee.roleSn.HasValue ? employee.roleSn.Value : 0;// != null ? employee.Role.roleSn : 0;

            //model.dep_name = employee.Department != null ? employee.Department.depName : "";
            model.depId = employee.depId;// != null ? employee.Department.depId : 0;

            return model;

        }

        #region 权限判断
        public bool CheckAjaxRight(int role, string controller, string action)
        {
            return _accountRepository.CheckAjaxRight(role, controller, action);
        }
        #endregion

        public ReturnValue UpdatePwd(int staff, string oldpwd, string newpwd)
        {
            return _accountRepository.UpdatePwd( staff, oldpwd,  newpwd);
        }
    }
}
