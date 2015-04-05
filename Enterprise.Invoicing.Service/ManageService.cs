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
    public class ManageService
    {
        private IManageRepository _manageRepository;
        public ManageService(IManageRepository manageRepository)
        {
            _manageRepository = manageRepository;
        }

        #region 部门
        public List<Department> GetDepartmentList(string key)
        {
            var list = _manageRepository.GetDepartmentList();
            if (key != "")
            {
                return list.Where(p => p.depName.Contains(key) || p.leader.Contains(key) || p.remark.Contains(key) || p.phone.Contains(key)).ToList();
            }
            return list.ToList();
        }
        public ReturnValue SaveDepartment(int id, bool valid, string depName, string phone, string leader, string remark)
        {
            return _manageRepository.SaveDepartment(id, valid, depName, phone, leader, remark);
        }
        public ReturnValue DeleteDepartment(int id)
        {
            return _manageRepository.DeleteDepartment(id);
        }
        #endregion

        #region 员工
        public List<EmployeeModel> GetEmployeeList(string key)
        {
            var list = _manageRepository.GetEmployeeList();
            if (key != "")
            {
                return list.Where(p => p.depName.Contains(key) || p.staffName.Contains(key) || p.remark.Contains(key) || p.email.Contains(key) || p.duty.Contains(key)).ToList();
            }
            return list.ToList();
        }
        public ReturnValue SaveEmployee(int id, int depId, string name, string mobile, string email, string duty, bool valid, string remark)
        {
            return _manageRepository.SaveEmployee(id, depId, name, mobile, email, duty, valid, remark);
        }
        public ReturnValue DeleteEmployee(int id)
        {
            return _manageRepository.DeleteEmployee(id);
        }
        public bool ChangeEmployeeStatus(int id, bool status)
        {
            return _manageRepository.ChangeEmployeeStatus(id, status);
        }
        #endregion

        #region 物料
        public List<Material> GetMaterialList(string key)
        {
            var list = _manageRepository.GetMaterialList();
            if (key != "")
            {
                return list.Where(p => p.materialNo.Contains(key) || p.tunumber.Contains(key) || p.pinyin.Contains(key) || p.fastcode.Contains(key) || p.materialName.Contains(key) || p.remark.Contains(key) || p.materialModel.Contains(key) || p.unit.Contains(key)).ToList();
            }
            return list.ToList();
        }
        public ReturnValue SaveMaterial(string type, string fix, string no, bool valid, string name, string bigcate, string category, string model, string unit, string orderno, string remark, string fastcode, string pinyin, string tunumber, int xslength)
        {
            return _manageRepository.SaveMaterial(type, fix, no, valid, name, bigcate, category, model, unit, orderno, remark, fastcode, pinyin, tunumber, xslength);
        }
        public ReturnValue DeleteMaterial(string no)
        {
            return _manageRepository.DeleteMaterial(no);
        }

        public List<KeyValue> GetFirstCate()
        {
            return _manageRepository.GetFirstCate();
        }
        public List<KeyValue> GetSecondCate(string cate)
        {
            return _manageRepository.GetSecondCate(cate);
        }
        #endregion

        #region 供应商和客户
        public List<Supplier> GetSupplierList(string key)
        {
            var list = _manageRepository.GetSupplierList();
            if (key != "")
            {
                return list.Where(p => p.supplierName.Contains(key) || p.phone.Contains(key) ||p.supplierNo.Contains(key) || p.remark.Contains(key) || p.address.Contains(key) || p.person.Contains(key)).ToList();
            }
            return list.ToList();
        }
        public ReturnValue SaveSupplier(int id, int type, string name, string person, string phone, string address, bool valid, string remark)
        {
            return _manageRepository.SaveSupplier(id, type, name, person, phone, address, valid, remark);
        }
        public ReturnValue DeleteSupplier(int id)
        {
            return _manageRepository.DeleteSupplier(id);
        }
        #endregion


        public string GetBillNo() { return _manageRepository.GetBillNo(); }
        public string GetTCNo() { return _manageRepository.GetTCNo(); }
        public bool ChangeBillStatus(string no, int status, int staff)
        {
            return _manageRepository.ChangeBillStatus(no, status, staff);
        }


        #region 消息
        public bool SaveSendMessage(MsgSend send)
        {
            return _manageRepository.SaveSendMessage(send);
        }
        #endregion
    }
}
