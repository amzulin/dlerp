using Enterprise.Invoicing.Entities.Models;
using Enterprise.Invoicing.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Invoicing.Repositories
{
    public interface IAccountRepository
    {
        LoginUser GetLoginModel(string userid);

        List<Menu> GetMenuByRole(int rolesn);


        List<Function> GetFunctionByRole(int rolesn);

        bool CheckAjaxRight(int role, string controller, string action);
        ReturnValue UpdatePwd(int staff,string oldpwd, string newpwd);
    }
}
