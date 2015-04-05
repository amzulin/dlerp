using Enterprise.Invoicing.Entities;
using Enterprise.Invoicing.Entities.Models;
using Enterprise.Invoicing.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Calabonga.Mvc.PagedListExt;
using Enterprise.Invoicing.Service;
using Enterprise.Invoicing.Repositories;

namespace Enterprise.Invoicing.Web.Controllers
{
    public class messageController : BaseController
    {
        private ManageService manageService; 
        public messageController(IManageRepository _manageRepository)
        {
            manageService = new ManageService(_manageRepository); 
        }

        [LoginAllow]
        public ActionResult rece()
        {
            return View();
        }
        [LoginAllow]
        public ActionResult recepart(int? page, int? pagesize, string key, string cate)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            string where = " recestaffid=" + Masterpage.CurrUser.staffid + " and isDelete=0 ";
            if (key != "")
            {
                where += " and (title like '%" + key + "%' or msgcontent  like '%" + key + "%' or sendstaffname  like '%" + key + "%') ";
            }
            if (cate != "")
            {
                where += " and (msgcate like '%" + key + "%') ";
            }
            var list = ServiceDB.Instance.QueryModelList<MsgReceModel>("select * from MsgReceModel where " + where + " ORDER BY isRead DESC, createDate DESC");

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;

            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }
        [HttpPost]
        [LoginAllow]
        public ActionResult deleterece(int id)
        {
            var row = ServiceDB.Instance.ExecuteSqlCommand("update MsgRece set isDelete=1 where receId=" + id);
            ReturnValue r = new ReturnValue();
            r.status = row == 1;
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [LoginAllow]
        public ActionResult showmsg(int id)
        {
            var msg = ServiceDB.Instance.QueryOneModel<MsgReceModel>("select * from MsgReceModel where receId=" + id + " and recestaffid=" + Masterpage.CurrUser.staffid);

            ReturnValue r = new ReturnValue();
            if (msg != null)
            {
                if (!msg.isRead) ServiceDB.Instance.QueryOneModel<MsgReceModel>("update msgrece set isread=1,readdate=getdate() where receId=" + id);
                r.status = true;
                r.message = msg.msgcontent;
                r.value = msg.title;
            }
            else { r.status = false; }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        #region
        [LoginAllow]
        public ActionResult send()
        {
            return View();
        }
        [LoginAllow]
        public ActionResult sendpart(int? page, int? pagesize, string key, string cate)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            string where = " staffid=" + Masterpage.CurrUser.staffid + " and isDelete=0 ";
            if (key != "")
            {
                where += " and (title like '%" + key + "%' or msgcontent  like '%" + key + "%' or receNames  like '%" + key + "%') ";
            }
            if (cate != "")
            {
                where += " and (msgcate like '%" + key + "%') ";
            }
            var list = ServiceDB.Instance.QueryModelList<MsgSendModel>("select * from MsgSendModel where " + where + " ORDER BY createDate DESC");

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 17;

            var vs = list.ToPagedList(_page, _pagesize);
            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;
            return PartialView(data);
        }
        [HttpPost]
        [LoginAllow]
        public ActionResult deletesend(Guid id)
        {
            var row = ServiceDB.Instance.ExecuteSqlCommand("update MsgSend set isDelete=1 where msgId='" + id + "'");
            ReturnValue r = new ReturnValue();
            r.status = row == 1;
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [LoginAllow]
        public ActionResult showmsg2(Guid id)
        {
            var msg = ServiceDB.Instance.QueryOneModel<MsgSendModel>("select * from MsgSendModel where msgId='" + id + "' and staffid=" + Masterpage.CurrUser.staffid);

            ReturnValue r = new ReturnValue();
            if (msg != null)
            {
                r.status = true;
                r.message = msg.msgcontent;
                r.value = msg.title;
                r.value2 = msg.receNames;
            }
            else { r.status = false; }
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [LoginAllow]
        public ActionResult sendone()
        {

            dynamic data = new System.Dynamic.ExpandoObject();
            var person = ServiceDB.Instance.QueryModelList<Employee>("select * from Employee where isuser=1 and status=1 and staffid<>" + Masterpage.CurrUser.staffid);
            data.person = person;
            return View(data);
        }
        [HttpPost]
        [LoginAllow]
        public ActionResult savemsg(string title, string cate, string forids, string fornames, string remark,string attrname,string attrguid)
        {
            Guid msg = Guid.NewGuid();
            MsgSend send = new MsgSend();
            send.createDate = DateTime.Now;
            send.isDelete = false;
            send.msgcate = cate;
            send.hadAttr = false;
            send.msgcontent = remark;
            send.msgId = msg;
            send.receIds = forids;
            send.receNames = fornames;
            send.staffId = Masterpage.CurrUser.staffid;
            send.title = title;
            if (attrname != null && attrname != "" && attrguid != null && attrguid != "")
            {
                send.hadAttr = true;
                ServiceDB.Instance.ExecuteSqlCommand("insert into Attachment values(newid(),'" + msg + "','" + attrguid + "','" + attrname + "','',getdate())");
            }

            bool exc = manageService.SaveSendMessage(send);
            int rececount = 0;
            if (exc)
            {
                string[] rece = forids.Split(',');
                foreach (var item in rece)
                {
                    if (item != "")
                    {
                        var row = ServiceDB.Instance.ExecuteSqlCommand("insert into MsgRece values('" + msg + "'," + item + ",0,null,0,null)");
                        rececount += row;
                    }
                }
            }
            ReturnValue r = new ReturnValue();
            r.status = rececount > 0;
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
