using Enterprise.Invoicing.Entities.Models;
using Enterprise.Invoicing.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Invoicing.Repositories
{
    public interface IManageRepository
    {
        #region 部门
        IQueryable<Department> GetDepartmentList();
        ReturnValue SaveDepartment(int id,bool valid, string depName,string phone,string leader,string remark);
        ReturnValue DeleteDepartment(int id);
        #endregion

        #region 员工
        IQueryable<EmployeeModel> GetEmployeeList();
        ReturnValue SaveEmployee(int id, int depId, string name, string mobile, string email, string duty, bool valid, string remark);
        ReturnValue DeleteEmployee(int id);
        bool ChangeEmployeeStatus(int id, bool status);
        #endregion

        #region 物料
        IQueryable<Material> GetMaterialList();
        ReturnValue SaveMaterial(string type, string fix, string no, bool valid, string name, string bigcate, string category, string model, string unit, string orderno, string remark, string fastcode, string pinyin, string tunumber, int xslength);
        ReturnValue DeleteMaterial(string no);
         List<KeyValue> GetFirstCate();
         List<KeyValue> GetSecondCate(string cate);
        #endregion

        #region 供应商和客户
        IQueryable<Supplier> GetSupplierList();
        ReturnValue SaveSupplier(int id, int type, string name, string person, string phone, string address, bool valid, string remark);
        ReturnValue DeleteSupplier(int id);
        #endregion

        string GetBillNo();
        string GetTCNo();
        bool ChangeBillStatus(string no, int status, int staff);


        #region 消息
        bool SaveSendMessage(MsgSend send);
        #endregion

    }
}
