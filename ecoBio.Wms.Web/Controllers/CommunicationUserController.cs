using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entities = ecoBio.Wms.Data.Entities;
using ecoBio.Wms.Common;
using ecoBio.Wms.ViewModel;
using ecoBio.Wms.Data.Entities.Models;
using ecoBio.Wms.Backstage.Repositories;
using System.IO;
using ecoBio.Wms.Backstage.Repositories.Backstage.CommunicationUser;
using jmail;
using Calabonga.Mvc.PagedListExt;
using ecoBio.Wms.FlexData;
using System.Configuration;
namespace ecoBio.Wms.Web.Controllers
{
    public class CommunicationUserController : BaseController
    {
        private ecoBio.Wms.Service.Management.EmailRecordService _emailRecordRepos = null;
        private ecoBio.Wms.Service.Management.UserService _userRepos = null;
        private ecoBio.Wms.Service.Management.EmployeeService _employeeservice = null;
        private ecoBio.Wms.Service.Management.CommunicationServer _communicationRepos = null;
        /// <summary>
        /// 实现控制反转
        /// </summary>
        /// <param name="moduleFunctionRepos"></param>
        /// 

        public CommunicationUserController(ecoBio.Wms.Backstage.Repositories.Backstage.CommunicationUser.IEmailRecordRepository EmailRecordRepos,
            ecoBio.Wms.Backstage.Repositories.IUserRepository userRepos,
           ecoBio.Wms.Backstage.Repositories.IEmployeeRepository employeeRepos,
            ecoBio.Wms.Backstage.Repositories.Backstage.CommunicationUser.ICommunicationRepository communicationRepos
             )
        {
            _emailRecordRepos = new Service.Management.EmailRecordService(EmailRecordRepos);
            _userRepos = new Service.Management.UserService(userRepos);
            _employeeservice = new Service.Management.EmployeeService(employeeRepos);
            _communicationRepos = new Service.Management.CommunicationServer(communicationRepos);
        }
        [LoginAllow]
        public ActionResult Index()
        {
            return View();

        }
        #region 邮件


        [LoginAllow]
        public ActionResult SendMail(Entities::Models.EmailRecord instance)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            data.one = new Entities::Models.EmailRecord();
            // string password= ecoBio.Wms.Web.Masterpage.CurrUser.emailpassword;
            //string key = DESHelper.GetKey();
            //string pwd = DESHelper.DESDecrypt(password, key);
            //  data.pwd = pwd;
            var use = _userRepos.GetUserAll().ToList();
            var em = _employeeservice.GetEmployee().ToList();
            List<SelectListItem> rec = use.Select(p => new SelectListItem { Text = p.UserChineseName, Value = p.UserEmail }).ToList();
            List<SelectListItem> emp = em.Select(p => new SelectListItem { Text = p.EmployeeChineseName, Value = p.EmployeeMail }).ToList();
            data.use = use;
            data.em = em;
            data.rec = rec;
            data.emp = emp;
            return View(data);
        }
        [ValidateInput(false)]
        [HttpPost]
        [LoginAllow]
        public ActionResult SaveMail(Entities::Models.EmailRecord instance, string hidmail)
        {
            try
            {

                //string Recipient = WebRequest.GetString("Recipient", true);
                //string Cc = WebRequest.GetString("Cc", true);
                //string Bcc = WebRequest.GetString("Bcc", true);
                string Recipient1 = WebRequest.GetString("Recipient1", true).Replace(",", ";");
                string Cc1 = WebRequest.GetString("Cc1", true);
                string Bcc1 = WebRequest.GetString("Bcc1", true);
                string att = WebRequest.GetString("AttachmentGuid1", true);

                //string con = WebRequest.GetString("hidmail", true);
                //Mail(Recipient1, "wms@ecobiotech.com.cn", "ecoBio@mail", Cc1, Bcc1, instance.Title, instance.EmailContent, att);
                WebAccountHelper.SendEmail(Recipient1, instance.Title, hidmail, att);
                //Mail(Recipient1, "844260219@qq.com", "zcc890213", Cc1, Bcc1, instance.Title, instance.EmailContent, att);
                instance.EmailGuid = Guid.NewGuid();
                instance.SendDate = System.DateTime.Now;
                _emailRecordRepos.AddService(instance);

            }
            catch
            {
            }
            return RedirectToAction("SendMail");

        }

        public ActionResult Recipient()
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            data.one = new Entities::Models.EmailRecord();
            var Recipient = _emailRecordRepos.GetEmailRecordAll();
            data.Recipient = Recipient;
            return View(data);
        }
        [LoginAllow]
        public ActionResult IndexMail(int? page, int? pagesize)
        {
            dynamic data = new System.Dynamic.ExpandoObject();

            var list = _emailRecordRepos.GetEmailRecordAll();

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 9;
            var vs = list.ToPagedList(_page, _pagesize);

            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;

            return PartialView(data);

        }
        [HttpPost]
        [LoginAllow]
        public ActionResult upload()
        {
            var uploadsFolder = ConfigurationManager.AppSettings["CustomerRes"] + "public/email";


            // var backpath = ConfigurationManager.AppSettings["VirtualRes"] + "public/email";

            var httpfile = Request.Files["myfile"];

            var fn = httpfile.FileName;
            //var identifier = fn.Substring(fn.LastIndexOf("\\"));
            // var iden = identifier.Split('\\');
            //var a = iden[1];
            var a = fn;
            var uploadsPath = Path.Combine(uploadsFolder, a);

            ReturnValue r;
            if (httpfile != null)
            {
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                //var exn = fn.Substring(fn.LastIndexOf("\\"));

                httpfile.SaveAs(uploadsPath + a);
                Guid pathGuid = Guid.NewGuid();

                r = new ReturnValue { status = "ok", value = a, value2 = uploadsPath + a };

                //r = _emailRecordRepos.SaveEmailRecord(instance.EmailGuid, fn,a,"邮件","");
            }
            else
            {
                r = new ReturnValue { status = "error", message = "未添加文档" };
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        private object Mail(string ToEmailAddress, string FromEmailAddress, string FromEmailPassword, string CCEmailAddress, string BccEmailAddress, string EmailSubject, string EmailBody, string EmailAttachmentPath)
        {
            try
            {
                jmail.Message Jmail = new jmail.Message();
                string[] from = FromEmailAddress.Split('@');
                if (CCEmailAddress == "")
                { }
                else
                {
                    string cc1 = CCEmailAddress.Substring(0, CCEmailAddress.Length - 1);
                    string[] cc = cc1.Split(',');

                    //添加抄送人邮箱地址
                    foreach (var item in cc)
                    {
                        Jmail.AddRecipientCC(item);
                    }
                }
                if (BccEmailAddress == "")
                { }
                else
                {
                    string Bcc1 = BccEmailAddress.Substring(0, BccEmailAddress.Length - 1);
                    string[] Bcc = Bcc1.Split(',');
                    //添加密送人邮箱地址
                    foreach (var item in Bcc)
                    {
                        Jmail.AddRecipientBCC(item);
                    }
                }
                if (EmailAttachmentPath == "," || EmailAttachmentPath == "")
                { }
                else
                {
                    //string p = EmailAttachmentPath.Substring(0, EmailAttachmentPath.Length - 1);
                    string[] path = EmailAttachmentPath.Split(',');
                    //邮件添加附件
                    foreach (var item in path)
                    {
                        Jmail.AddAttachment(@item);
                    }
                    Jmail.AddAttachment(EmailAttachmentPath);
                }
                string to1 = ToEmailAddress.Substring(0, ToEmailAddress.Length - 1);
                string[] to = to1.Split(',');

                //Silent属性：如果设置为true,JMail不会抛出例外错误. JMail. Send( () 会根据操作结果返回true或false
                Jmail.Silent = true;
                //Jamil创建日志，前提logging属性设置为true
                Jmail.Logging = true;
                Jmail.FromName = ecoBio.Wms.Web.Masterpage.CurrUser.alias;
                //字符集，缺省为"US-ASCII"
                Jmail.Charset = "GB2312";
                Jmail.ISOEncodeHeaders = true;
                //添加收件人邮箱地址
                foreach (var item in to)
                {
                    Jmail.AddRecipient(item);
                }
                // Jmail.AddRecipient(ToEmailAddress);


                //Jmail.AddRecipientCC(CCEmailAddress);

                //发件人邮箱地址
                Jmail.From = FromEmailAddress;
                //发件人邮件姓名                 
                Jmail.MailServerUserName = from[0];
                //发件人邮件密码
                Jmail.MailServerPassWord = FromEmailPassword;
                //设置邮件标题
                Jmail.Subject = EmailSubject;
                //邮件内容
                //Jmail.Body = EmailBody;                
                Jmail.HTMLBody = EmailBody;
                //邮件优先级为3为Normal
                Jmail.Priority = 3;

                Jmail.Send("smtp.qq.com");
                //Jmail.Send("smtp.ecobiotech.com.cn");
                Jmail.Close();
                return "";
            }
            catch (Exception ex)
            {
                Console.WriteLine("发送失败：" + ex);
                return "";
            }
        }

        #endregion

        #region 消息

        [LoginAllow]
        public ActionResult SendCommunication(Entities::Models.Communication instance)
        {
            dynamic data = new System.Dynamic.ExpandoObject();
            data.one = new Entities::Models.Communication();
            // string password= ecoBio.Wms.Web.Masterpage.CurrUser.emailpassword;
            //string key = DESHelper.GetKey();
            //string pwd = DESHelper.DESDecrypt(password, key);
            //  data.pwd = pwd;
            var use = _userRepos.GetUserAll().ToList();
            var em = _employeeservice.GetEmployee().ToList();
            List<SelectListItem> rec = use.Select(p => new SelectListItem { Text = p.UserChineseName, Value = p.UserGuid.ToString() }).ToList();
            List<SelectListItem> emp = em.Select(p => new SelectListItem { Text = p.EmployeeChineseName, Value = p.EmployeeGuid.ToString() }).ToList();
            data.use = use;
            data.em = em;
            data.rec = rec;
            data.emp = emp;
            return View(data);
        }
        [ValidateInput(false)]
        [HttpPost]
        [LoginAllow]
        public ActionResult SaveCommunication(Entities::Models.Communication instance, string hidmessage)
        {
            try
            {
                string Recipien1 = WebRequest.GetString("Recipien1", true);
                string Title1 = WebRequest.GetString("Title1", true);
                string cons = WebRequest.GetString("EmailContent1", true);
               // string CommunicationContent = WebRequest.GetString("CommunicationContent", true);
                var a = Recipien1.Split(',');

                var ReadStatus = "";
                var senguid = "";
                for (int i = 0; i < a.Length - 1; i++)
                // var x = item.Split('|');
                {
                    senguid += a[i].ToString().Split('|')[0].Trim() + ',';
                    ReadStatus += a[i].ToString().Split('|')[1] + ',';
                }
                instance.SendGuid = ecoBio.Wms.Web.Masterpage.CurrUser.guid;
                //instance.MsgContent = cons;
                instance.Recipient = senguid;
                instance.ReadStatus = ReadStatus;
                instance.MsgGuid = Guid.NewGuid();
                instance.SendDate = System.DateTime.Now;
                instance.Title = Title1;
                instance.MsgContent = hidmessage;
                _communicationRepos.AddService(instance);

            }
            catch
            {
            }
            return RedirectToAction("SendMail");

        }
        [LoginAllow]
        public ActionResult IndexCommunication(int? page, int? pagesize)
        {
            dynamic data = new System.Dynamic.ExpandoObject();

            var list = _communicationRepos.GetCommunicationAll();

            int _page = page.HasValue ? page.Value : 1;
            int _pagesize = pagesize.HasValue ? pagesize.Value : 9;
            var vs = list.ToPagedList(_page, _pagesize);

            data.list = vs;
            data.pageSize = _pagesize;
            data.pageIndex = _page;
            data.totalCount = vs.TotalCount;

            return PartialView(data);

        }
        #endregion


    }
}
