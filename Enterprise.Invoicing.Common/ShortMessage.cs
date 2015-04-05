using FULLTUNNELCONTROLLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ecoBio.Wms.Common
{
    public class ShortMessage
    { 
        FullTunnelServiceControl ctr1 = new FullTunnelServiceControl();
         

            private void Content(object sender, EventArgs e)
            {
               

                //string strControl_type = "connect";
                string strName = "用户名";
                string strPwd = "密码";
                string strServerip = "218.94.58.243";//218.94.58.243
                string strServerport = "9005";
                
               // object hr = ctr1.StartAndManageConnect_User(ref strName, ref strPwd, ref strServerip, ref strServerport);
                object hr = ctr1.StartAndManageConnect_User(strName, strPwd, strServerip, strServerport);
                if (hr.ToString() == "0")
                {       
                    //链接成功
                }
            }

            public string SendMessage()
            {
                string result = "";
                string strControl_type = "send";
                string strName = "zhangxq";               //用户名
                string strPwd = "135246";                  //密码
                string strSenderr = "051383558078";
                string strServerip = "192.168.31.145";
                string strServerport = "9005";
                string strSendtype = "1";
                string strBackstr = "1";
                string strSourcecode = "02588888888";
                string strDestcode = "15312605215";        //发送的号码
                string strContent = "wljtest";
                string strComid = "1";
                string strNeedreceipt = "1";
                string strAccessNum = "0513C00014003";
                object seqID;

                FullTunnelServiceControl ctr1 = new FullTunnelServiceControl();
                object oOut = null;
                object hr = ctr1.SendBusinessPkg(strControl_type, strName, strPwd, strSourcecode,
                                                                     strDestcode, strServerip, strServerport, strContent,
                                                                     strSendtype, strComid, strNeedreceipt, out seqID);
                if (hr.ToString() == "1")
                {
                    result = "发送业务包成功" + "     此条短信的SequenceID是：" + seqID;
                }
                else
                {
                    result = "发送业务包失败" + hr.ToString();
                }

                return result;
            }

            private static string SendFromDbThread()
            {
                string strControl_type = "send";
                string strName = "用户名";               //用户名
                string strPwd = "密码";                  //密码
                string strServerip = "218.94.58.243";//218.94.58.243
                string strServerport = "9005";
                string strSendtype = "1";
                string strSourcecode = "12";        //必须得填 没有的话也必须填
                string strDestcode = "发送到的手机号";        //发送的号码
                string strContent = "短信内容";
                string strComid = "1";            //必须得填 没有的话也必须填
                string strNeedreceipt = "1";
                object seqID;

                FullTunnelServiceControl ctr1 = new FullTunnelServiceControl();
                object oOut = null;
                object hr = ctr1.SendBusinessPkg(strControl_type, strName, strPwd, strSourcecode,
                                                                     strDestcode, strServerip, strServerport, strContent,
                                                                     strSendtype, strComid, strNeedreceipt, out seqID);
                if (hr.ToString() == "1")
                {
                    //textBox1.Text = "发送业务包成功" + "     此条短信的SequenceID是：" + seqID;    
                    return seqID.ToString() + "发送业务包成功";
                }
                else
                {
                    //textBox1.Text = "发送业务包失败"+hr.ToString();
                    return "发送业务包fail";
                }
            }

           
        
    }
}
