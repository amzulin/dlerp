using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Enterprise.Invoicing.Common
{
    /// <summary>  
    /// 发送邮件的类  
    /// </summary>  
    public class MailHelper
    {
        private MailMessage mailMessage = new MailMessage();
        private SmtpClient smtpClient;
        private string password;//发件人密码  
        /**/
        /// <summary>  
        /// 设置收件人、发件人信息与邮件内容、标题  
        /// </summary>  
        /// <param name="To">收件人地址</param>  
        /// <param name="From">发件人地址</param>  
        /// <param name="Body">邮件正文</param>  
        /// <param name="Title">邮件的主题</param>  
        /// <param name="Password">发件人密码</param>  
        public void Mail(string To, string From, string Body, string Title, string Password)
        {
            mailMessage.To.Add(To);
            mailMessage.From = new System.Net.Mail.MailAddress(From);
            mailMessage.Subject = Title;
            mailMessage.Body = Body;
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mailMessage.Priority = System.Net.Mail.MailPriority.Normal;
            this.password = Password;
        }
        /**/
        /// <summary>  
        /// 添加附件  
        /// </summary>  
        public void Attachments(string Path)
        {
            string[] path = Path.Split(',');
            Attachment data;
            ContentDisposition disposition;
            for (int i = 0; i < path.Length; i++)
            {
                //string p = HttpContext.Current.Server.MapPath();
                string p = path[i];
                data = new Attachment(p, MediaTypeNames.Application.Octet);//实例化 附件  
                disposition = data.ContentDisposition;
                disposition.CreationDate = System.IO.File.GetCreationTime(path[i]);//获取 附件的创建日期  
                disposition.ModificationDate = System.IO.File.GetLastWriteTime(path[i]);// 获取附件的修改日期  
                disposition.ReadDate = System.IO.File.GetLastAccessTime(path[i]);//获取附 件的读取日期  
                mailMessage.Attachments.Add(data);//添加到附件中
            }
        }
        /**/
        /// <summary>  
        /// 异步发送邮件  
        /// </summary>  
        /// <param name="CompletedMethod"></param>  
        public void SendAsync(SendCompletedEventHandler CompletedMethod)
        {
            if (mailMessage != null)
            {
                smtpClient = new SmtpClient();
                smtpClient.Credentials = new System.Net.NetworkCredential(mailMessage.From.Address, password);//设置发件人身份的票据  
                smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtpClient.Host = "smtp." + mailMessage.From.Host;
                smtpClient.SendCompleted += new SendCompletedEventHandler(CompletedMethod);//注册异步发送邮件完成时的事件  
                smtpClient.SendAsync(mailMessage, mailMessage.Body);
            }
        }
        /**/
        /// <summary>  
        /// 发送邮件  
        /// </summary>  
        public void Send()
        {
            if (mailMessage != null)
            {
                smtpClient = new SmtpClient();
                smtpClient.Credentials = new System.Net.NetworkCredential(mailMessage.From.Address, password);//设置发件人身份的票据  
                smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtpClient.Host = "smtp." + mailMessage.From.Host;
                smtpClient.Send(mailMessage);
            }
        }
        /// <summary>
        /// 根据索引删除附件
        /// </summary>
        /// <param name="Id"></param>
        public void DeleteAttachments(int Id)
        {
            mailMessage.Attachments.RemoveAt(Id);
        }



        /// <summary> 
        /// 发送电子邮件 
        /// </summary> 
        /// <param name="MessageFrom">发件人邮箱地址</param> 
        /// <param name="MessageTo">收件人邮箱地址</param> 
        /// <param name="MessageSubject">邮件主题</param> 
        /// <param name="MessageBody">邮件内容</param> 
        /// <returns></returns> 
        public static bool Send( string MessageTo, string MessageSubject, string MessageBody)
        {
            MailMessage message = new MailMessage();
            MailAddress MessageFrom = new MailAddress("ecobio@163.com");
            message.From = MessageFrom;
            message.To.Add(MessageTo); //收件人邮箱地址可以是多个以实现群发 
            message.Subject = MessageSubject;
            message.Body = MessageBody;
            message.IsBodyHtml = false; //是否为html格式 
            message.Priority = MailPriority.High; //发送邮件的优先等级 

            SmtpClient sc = new SmtpClient();
            sc.Host = "smtp.163.com"; //指定发送邮件的服务器地址或IP 
            sc.Port = 25; //指定发送邮件端口 

            //测试账号：zwtest2012@163.com/zwtest20122012 ,随便注册的一个163账号要是自己的可以填自己的
            sc.Credentials = new System.Net.NetworkCredential("ecobio@163.com", "szecobio"); //指定登录服务器的用户名和密码(发件人的邮箱登陆密码)

            try
            {
                sc.Send(message); //发送邮件 
            }
            catch (Exception ex)
            {
                //throw(ex);

                //ExcelDBA.WriteLog("----------------------------");//日志小分割
                //ExcelDBA.WriteLog("发送邮件到[" + MessageTo.ToString() + "]失败!");
                //ExcelDBA.WriteLog("错误信息:" + ex.Message.ToString());
                return false;
            }
            return true;

        }
    }

    public class SendEmail {

        SmtpClient SmtpClient = null;   //设置SMTP协议
        MailAddress MailAddress_from = null; //设置发信人地址  当然还需要密码
        MailAddress MailAddress_to = null;  //设置收信人地址  不需要密码
        MailMessage MailMessage_Mai = null;
        FileStream FileStream_my = null; //附件文件流

        #region 设置Ｓmtp服务器信息
        /// <summary>
        /// 设置Ｓmtp服务器信息
        /// </summary>
        /// <param name="ServerName">SMTP服务名</param>
        /// <param name="Port">端口号</param>
        private void setSmtpClient(string ServerHost, int Port)
        {
            SmtpClient = new SmtpClient();
            SmtpClient.Host = ServerHost;//指定SMTP服务名  例如QQ邮箱为 smtp.qq.com 新浪cn邮箱为 smtp.sina.cn等
            SmtpClient.Port = Port; //指定端口号
            SmtpClient.Timeout = 60;  //超时时间

        }
        #endregion

        #region 验证发件人信息
        /// <summary>
        /// 验证发件人信息
        /// </summary>
        /// <param name="MailAddress">发件邮箱地址</param>
        /// <param name="MailPwd">邮箱密码</param>
        private void setAddressform(string MailAddress, string MailPwd)
        {
            //创建服务器认证
            NetworkCredential NetworkCredential_my = new NetworkCredential(MailAddress, MailPwd);
            //实例化发件人地址
            MailAddress_from = new System.Net.Mail.MailAddress(MailAddress, MailAddress);
            //指定发件人信息  包括邮箱地址和邮箱密码
            SmtpClient.Credentials = new System.Net.NetworkCredential(MailAddress_from.Address, MailPwd);
            ;
        }
        #endregion

        #region 检测附件大小
        private bool Attachment_MaiInit(string path)
        {

            try
            {
                FileStream_my = new FileStream(path, FileMode.Open);
                string name = FileStream_my.Name;
                int size = (int)(FileStream_my.Length / 1024 / 1024);
                FileStream_my.Close();
                //控制文件大小不大于10Ｍ
                if (size > 10)
                {

                    //MessageBox.Show("文件长度不能大于10M！你选择的文件大小为" + size.ToString() + "M", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                return true;
            }
            catch (IOException E)
            {
                //MessageBox.Show(E.Message);
                return false;
            }

        }
        #endregion

        public  void Send(string fromemail,string frompwd,string to,string body)
        {
            try
            {
                setSmtpClient("smtp.163.com", 25);
                //验证发件邮箱地址和密码
                setAddressform(fromemail, frompwd);
            }
            catch (Exception Ex)
            {
                //MessageBox.Show("邮件发送失败,请确定发件邮箱地址和密码的正确性！" + "\n" + "技术信息:\n" + Ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MailMessage_Mai = new MailMessage();
            //清空历史发送信息 以防发送时收件人收到的错误信息(收件人列表会不断重复)
            if(MailAddress_to!=null) MailMessage_Mai.To.Clear();
            //添加收件人邮箱地址
            string[] tos = to.Split(';');
            foreach (string row in tos)
            {
                MailAddress_to = new MailAddress(row);
                MailMessage_Mai.To.Add(MailAddress_to);
            }
            //MessageBox.Show("收件人：" + dataGridViewX1.Rows.Count.ToString() + "  个");

            //发件人邮箱
            MailMessage_Mai.From = MailAddress_from;
            MailMessage_Mai.IsBodyHtml = true;
            //邮件主题
            MailMessage_Mai.Subject = "Subject";
            MailMessage_Mai.SubjectEncoding = System.Text.Encoding.UTF8;
            //邮件正文
            MailMessage_Mai.Body = body;
            //MailMessage_Mai.Body = "<h1>Body</h1><br><h3>Body2</h3><hr><span style='color:red;'>cc<span>";
            // MailMessage_Mai.Body = "<html><head><title>title</title></head><body>这是内容<hr><br><img src=\"http://avatar.csdn.net/D/F/9/1_marquess.jpg\" /></body></html>";
            MailMessage_Mai.BodyEncoding = System.Text.Encoding.UTF8;
            //清空历史附件  以防附件重复发送
            MailMessage_Mai.Attachments.Clear();
            //添加附件

           //MailMessage_Mai.Attachments.Add(new Attachment("d:\\test.xlsx", MediaTypeNames.Application.Octet));
            //注册邮件发送完毕后的处理事件
            SmtpClient.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
            //开始发送邮件
            SmtpClient.Send(MailMessage_Mai);
            //SmtpClient.SendAsync(MailMessage_Mai, "000000000");
        }

        #region 发送邮件后所处理的函数
        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    //MessageBox.Show("发送已取消！");
                }
                if (e.Error != null)
                {

                    //MessageBox.Show("邮件发送失败！" + "\n" + "技术信息:\n" + e.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                else
                {
                    //MessageBox.Show("邮件成功发出!", "恭喜!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception Ex)
            {
                //MessageBox.Show("邮件发送失败！" + "\n" + "技术信息:\n" + Ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        #endregion
    
    }

}
