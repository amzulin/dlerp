using Enterprise.Invoicing.Entities.Models;
using Enterprise.Invoicing.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Enterprise.Invoicing.Repositories
{
    public class AccountRepository : BasicRepository<Employee>, IAccountRepository
    { 
        public AccountRepository()            : base(new InvoicingContext())        { }

        public AccountRepository(InvoicingContext context) : base(context) { }


       
        public LoginUser GetLoginModel(string userid)
        {
            //context
            //using (var c = new InvoicingContext())
           // {
                var model = (from e in context.Employees
                             join r in context.Roles on e.roleSn equals r.roleSn
                             join d in context.Departments on e.depId equals d.depId
                             where e.userId==userid && e.isUser==true
                             select new LoginUser
                             {
                                 dep_name = d.depName,
                                 mobile = e.mobile,
                                 email = e.email,
                                 name = e.staffName, rigthType=e.rigthType,  showPrice=r.showPrice,
                                 role_name = r.roleName,
                                 role_sn = r.roleSn,
                                 staffid = e.staffId,
                                 userid = e.userId,
                                 userpwd = e.userPwd,depId=d.depId
                             }
                                ).FirstOrDefault();
                return model;
           // }
        }

        public List<Enterprise.Invoicing.Entities.Models.Menu> GetMenuByRole(int rolesn)
        {
            using (var c = new InvoicingContext())
            {
                var list = from m in c.Menus
                           join r in c.MenuRights on m.menuNo equals r.menuNo
                           where r.roleSn == rolesn
                           orderby m.menuNo ascending
                           select m;
                return list.ToList();
            }
        }
        public List<Function> GetFunctionByRole(int rolesn)
        {
            using (var c=new InvoicingContext())
            {
                var list = from f in c.Functions
                           join r in c.FunctionRights on f.functionNo equals r.functionNo
                           where r.roleSn == rolesn
                           orderby r.rightSn ascending
                           select f;
                return list.ToList();
            }
        }
        /// <summary>
        /// 判断菜单和ajax权限
        /// </summary>
        /// <param name="role"></param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool CheckAjaxRight(int role, string controller, string actions)
        {
            if (actions.Contains(","))
            {
                string[] actionlist = actions.Split(',');
                foreach (var item in actionlist)
                {
                    if (item != "")
                    {
                        var h = HasAjaxRight(role, controller, item);
                        if (h)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            else
            {
                return HasAjaxRight(role, controller, actions);
            }
        }

        public ReturnValue UpdatePwd(int staff, string oldpwd, string newpwd)
        {
            using (var c = new InvoicingContext())
            {
                var model = c.Employees.FirstOrDefault(p => p.staffId == staff);
                if (model == null) return new ReturnValue { status = false, message = "用户不存在" };
                if (model.isUser == false) return new ReturnValue { status = false, message = "该员工为非系统用户" };
                if (!model.status) return new ReturnValue { status = false, message = "该用户已被锁定" };
                if (model.userPwd.ToLower() != oldpwd.ToLower()) return new ReturnValue { status = false, message = "旧密码输入错误" };
                model.userPwd = newpwd;
                c.Entry(model).State = EntityState.Modified;
                c.SaveChanges();
                return new ReturnValue { status = true, message = "" };
            }
        }

        #region 私有方法
        private bool HasAjaxRight(int sn, string c, string a)
        {
            var h = (from m in context.Menus
                     join r in context.MenuRights on m.menuNo equals r.menuNo
                     where m.menuUrl.ToLower().Contains(c.ToLower())
                     && m.menuUrl.ToLower().Contains(a.ToLower()) && r.roleSn == sn
                     select m.menuNo).FirstOrDefault();
            if (h != null && h != "")
            {
                return true;
            }
            else return false;
        }

        #endregion
    }
}
