//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace Enterprise.Invoicing.Web.App_Start
//{
//    public class print
//    {
//    }
//}

using System;
using System.Data;
using System.Web;
using System.Text;
using System.Collections;
using System.Web.UI;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Collections.Generic;

namespace WebPrint
{
    /// <summary>
    /// 这是一个完全利用IE自身对象实现Web打印的方案，主体数据来源于DataGrid控件绑定的数据
    /// 同时，用户可以自定义标题、页眉、页脚、每页打印行数等一般报表的基本要素
    /// 作者：KG 
    /// 日期：2005.1.1
    /// 来源：www.domanage.com.
    /// </summary>
    public class clsPrint : System.Web.UI.Page
    {
        private string[] sShoulderLeft;
        private string[] sShoulderCenter;
        private string[] sShoulderRight;



        private string[] sFooterLeft;
        private string[] sFooterCenter;
        private string[] sFooterRight;



        private string sPageTitle = "";
        private string sSpanColumnList = "";

        private int iPageNumber = 30;

        private List<string> MyDataGrid;

        private static string pageUrlBase; //Page基本的URL 

        public clsPrint()
        {
            try
            {
                string urlSuffix = Context.Request.Url.Host;
                urlSuffix = urlSuffix + (Context.Request.Url.Port.ToString() == "" ? "" : ":" + Context.Request.Url.Port);
                urlSuffix = urlSuffix + (Context.Request.ApplicationPath.ToString() == "/" ? "" : Context.Request.ApplicationPath);
                pageUrlBase = @"http://" + urlSuffix;

            }
            catch
            {
                // for design time
            }
        }


        #region 每页显示的行数
        /// <summary>
        /// 每页显示的行数
        /// </summary>
        public int PageNumber
        {
            get
            {
                return iPageNumber;
            }
            set
            {
                iPageNumber = value;
            }
        }
        #endregion



        #region 页的标题
        /// <summary>
        /// 页的标题
        /// </summary>
        public string PageTitle
        {
            get
            {
                return sPageTitle;
            }
            set
            {
                sPageTitle = value;
            }
        }
        #endregion

        #region 网格控键要合并的列集合
        /// <summary>
        /// 网格控键要合并的列集合，格式如： 1,2,3,4用逗号分割
        /// </summary>
        public string SpanColumnList
        {
            get
            {
                return sSpanColumnList;
            }
            set
            {
                sSpanColumnList = value;
            }
        }
        #endregion



        #region 左页眉标题，数组类型[0][1] 显示顺序从上至下
        /// <summary>
        /// 页的标题
        /// </summary>
        public string[] ShoulderLeft
        {
            get
            {
                return sShoulderLeft;
            }
            set
            {
                sShoulderLeft = value;
            }
        }
        #endregion



        #region 中页眉标题，数组类型[0][1] 显示顺序从上至下
        /// <summary>
        /// 页的标题
        /// </summary>
        public string[] ShoulderCenter
        {
            get
            {
                return sShoulderCenter;
            }
            set
            {
                sShoulderCenter = value;
            }
        }
        #endregion



        #region 右页眉标题，数组类型[0][1] 显示顺序从上至下
        /// <summary>
        /// 页的标题
        /// </summary>
        public string[] ShoulderRight
        {
            get
            {
                return sShoulderRight;
            }
            set
            {
                sShoulderRight = value;
            }
        }
        #endregion



        #region 左页脚标题，数组类型[0][1] 显示顺序从上至下
        /// <summary>
        /// 页的标题
        /// </summary>
        public string[] FooterLeft
        {
            get
            {
                return sFooterLeft;
            }
            set
            {
                sFooterLeft = value;
            }
        }
        #endregion



        #region 中页脚标题，数组类型[0][1] 显示顺序从上至下
        /// <summary>
        /// 页的标题
        /// </summary>
        public string[] FooterCenter
        {
            get
            {
                return sFooterCenter;
            }
            set
            {
                sFooterCenter = value;
            }
        }
        #endregion



        #region 右页脚标题，数组类型[0][1] 显示顺序从上至下
        /// <summary>
        /// 页的标题
        /// </summary>
        public string[] FooterRight
        {
            get
            {
                return sFooterRight;
            }
            set
            {
                sFooterRight = value;
            }
        }
        #endregion



        #region 打印的网格控件名称
        /// <summary>
        /// 打印的网格控件名称
        /// </summary>
        public List<string> dgDataGrid
        {
            get
            {
                return MyDataGrid;
            }
            set
            {
                MyDataGrid = value;
            }
        }
        #endregion



        #region 修改模板文件的内容，构造打印数据，重写打印模板
        /// <summary>
        /// 修改模板文件的内容，构造打印数据，重写打印模板
        /// </summary>
        public void PrintView()
        {
            ClearView();

            StringBuilder htmltext = new StringBuilder();
            try
            {
                using (StreamReader sr = new StreamReader(Server.MapPath(Context.Request.ApplicationPath) + "\\printWindow.htm"))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        htmltext.Append(line);
                    }
                    sr.Close();
                }
            }
            catch
            {
                Response.Write("<Script>alert('读取文件错误')</Script>");
            }



            //----------替换htm里的标记为你想加的内容 



            htmltext.Replace("Model", PintPage());



            //----------生成htm文件------------------―― 
            try
            {
                using (StreamWriter sw = new StreamWriter(Server.MapPath(Context.Request.ApplicationPath) + "\\printWindow.htm", false, System.Text.Encoding.GetEncoding("GB2312")))
                {
                    sw.WriteLine(htmltext);
                    sw.Flush();
                    sw.Close();
                    //Response.Write("<script languge='javascript'>alert('111');window.open('"+pageUrlBase+"/printWindow.htm','_blank');</script>"); 
                }
            }
            catch
            {
                Response.Write("The file could not be wirte:");
            }
            //Response.Write("<script languge='javascript'>alert('111');window.open('"+pageUrlBase+"/printWindow.htm','_blank');</script>");
            //Page.RegisterStartupScript("show111", "<script languge='javascript'>alert('"+pageUrlBase+"');window.open('"+pageUrlBase+"/printWindow.htm','_blank');</script>");
        }
        #endregion



        #region 打印主函数，构造要打印的页面的所有打印项目（标题，列标题，网格数据，shoulder,footer）
        /// <summary>
        /// 打印主函数
        /// </summary>
        /// <returns></returns>
        private string PintPage()
        {
            int iTableIndex = 1;
            string[] ColumnList;



            string sTable = "<object ID='WebBrowser' WIDTH=0 HEIGHT=0 CLASSID='CLSID:8856F961-340A-11D0-A96B-00C04FD705A2'></object>\n\n";



            //添加页的标题
            sTable = sTable + AddPageTitle();



            //添加页眉
            sTable = sTable + AddShoulder();



            //添加网格数据第一页
            sTable = sTable + "<table id=\"table1\" class=\"print-body\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\">";
            //添加该页的列标题
            sTable = sTable + AddTableTitle();



            //初始化行号
            int iStartItemIndex = 0;
            int iItemIndex = iStartItemIndex;
            //主体数据
            int i = 0;
            DataTable dt = new DataTable();
            foreach (var item in this.MyDataGrid)
            {
                string[] list = item.Split('|');
                DataRow r = dt.NewRow();
                for (int k = 0; k < list.Length; k++)
                {
                    r[k] = item;
                }
                dt.Rows.Add(r);
            }



            foreach (DataRow row in dt.Rows)
            {
                if (i > 0 && i % iPageNumber == 0)
                {
                    //上一页结束
                    sTable = sTable + "</table>";
                    if (sSpanColumnList != "")
                    {
                        ColumnList = sSpanColumnList.Split(',');
                        for (int ii = 0; ii < ColumnList.Length; ii++)
                        {
                            sTable = sTable + "<script languge='javascript'>TableRowSpan(\"table" + iTableIndex.ToString() + "\"," + ColumnList[ii].ToString() + ");</script>\n\n";
                        }
                    }



                    sTable = sTable + AddFooter();
                    //换页
                    sTable = sTable + AddPageBreak();
                    //添加页的标题
                    sTable = sTable + AddPageTitle();
                    //添加页眉
                    sTable = sTable + AddShoulder();
                    iTableIndex++;
                    //创建新一轮的表格
                    sTable = sTable + "<table id=\"table" + iTableIndex.ToString() + "\" class=\"print-body\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\">";
                    //添加该页的列标题
                    sTable = sTable + AddTableTitle();
                }
                //将记录添加到表格的一行中
                sTable = sTable + AddItemToTable(row);
                //行号加一
                iItemIndex++;
                i++;
            }



            sTable = sTable + "</table>\n\n";
            if (sSpanColumnList != "")
            {
                ColumnList = sSpanColumnList.Split(',');
                for (int ii = 0; ii < ColumnList.Length; ii++)
                {
                    sTable = sTable + "<script languge='javascript'>TableRowSpan(\"table" + iTableIndex.ToString() + "\"," + ColumnList[ii].ToString() + ");</script>\n\n";
                    //sTable = sTable + "<script languge='javascript'>TableRowSpan(\"table"+iTableIndex.ToString()+"\",0);</script>\n\n";
                }
            }
            //添加页脚
            sTable = sTable + AddFooter();



            sTable = sTable + "<script languge='javascript'>WebBrowser.ExecWB(7,1); window.opener=null;window.close();</script>";



            return sTable;
        }
        #endregion



        #region 增加一行数据到表格中，生成要打印网格数据的行数据
        /// <summary>
        /// 增加一行数据到表格中
        /// </summary>
        /// <param name="row">值</param>
        private string AddItemToTable(DataRow row)
        {
            string sItem = "<tr>\n";



            for (int i = 0; i < MyDataGrid.Count; i++)
            {

               
            }

            sItem = sItem + "</tr>\n";
            return sItem;
        }
        #endregion

        #region 添加打印网格数据的列标题
        /// <summary>
        /// 添加打印网格数据的列标题
        /// </summary>
        private string AddTableTitle()
        {
            string sTableTitle = "<tr>\n";



            for (int i = 0; i < MyDataGrid.Count; i++)
            {
                //if (MyDataGrid.Columns == true)
                //{
                //    sTableTitle = sTableTitle + "<td class=\"print-coltitle\" width=\"" + MyDataGrid.Columns.HeaderStyle.Width.Value.ToString() + "\"";
                //    sTableTitle = sTableTitle + " height=\"" + MyDataGrid.Columns.HeaderStyle.Height.Value.ToString() + "\"";
                //    sTableTitle = sTableTitle + " > " + MyDataGrid.Columns.HeaderText.ToString() + "</td>";
                //}
            }
            sTableTitle = sTableTitle + "</tr>\n";



            return sTableTitle;
        }
        #endregion



        #region 添加页眉
        /// <summary>
        /// 添加打印网格数据的列标题
        /// </summary>
        private string AddShoulder()
        {
            string sShoulder = "";



            int iLeftLength = (sShoulderLeft == null ? 0 : sShoulderLeft.Length);
            int iCenterLength = (sShoulderCenter == null ? 0 : sShoulderCenter.Length);
            int iRightLength = (sShoulderRight == null ? 0 : sShoulderRight.Length);



            if (sShoulderLeft == null && sShoulderCenter == null && sShoulderRight == null)
            {
                //
            }
            else
            {
                sShoulder = sShoulder + "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\">\n";
                sShoulder = sShoulder + "<tr>\n";
                sShoulder = sShoulder + "<td valign=\"bottom\">\n";
                sShoulder = sShoulder + "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\">\n";
                for (int i = 0; i < iLeftLength; i++)
                {
                    sShoulder = sShoulder + "<tr>\n";
                    sShoulder = sShoulder + "<td >" + (i < iLeftLength ? sShoulderLeft.ToString() : " ") + "</td>\n";
                    sShoulder = sShoulder + "</tr>\n";
                }
                sShoulder = sShoulder + "</table>\n";
                sShoulder = sShoulder + "</td>\n";
                if (sShoulderCenter != null)
                {
                    sShoulder = sShoulder + "<td valign=\"bottom\" align=\"left\">\n";
                    sShoulder = sShoulder + "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\">\n";
                    for (int i = 0; i < iCenterLength; i++)
                    {
                        sShoulder = sShoulder + "<tr>\n";
                        sShoulder = sShoulder + "<td >" + (i < iCenterLength ? sShoulderCenter.ToString() : " ") + "</td>\n";
                        sShoulder = sShoulder + "</tr>\n";
                    }
                    sShoulder = sShoulder + "</table>\n";
                    sShoulder = sShoulder + "</td>\n";
                }
                sShoulder = sShoulder + "<td valign=\"bottom\" align=\"right\">\n";
                sShoulder = sShoulder + "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" >\n";
                for (int i = 0; i < iRightLength; i++)
                {
                    sShoulder = sShoulder + "<tr>\n";
                    sShoulder = sShoulder + "<td >" + (i < iRightLength ? sShoulderRight.ToString() : " ") + "</td>\n";
                    sShoulder = sShoulder + "</tr>\n";
                }
                sShoulder = sShoulder + "</table>\n";
                sShoulder = sShoulder + "</td>\n";
                sShoulder = sShoulder + "</tr>\n";
                sShoulder = sShoulder + "</table>\n";
            }



            return sShoulder;
        }
        #endregion



        #region 添加页脚
        /// <summary>
        /// 添加打印网格数据的列标题
        /// </summary>
        private string AddFooter()
        {
            string sFooter = "";
            int iLeftLength = (sFooterLeft == null ? 0 : sFooterLeft.Length);
            int iCenterLength = (sFooterCenter == null ? 0 : sFooterCenter.Length);
            int iRightLength = (sFooterRight == null ? 0 : sFooterRight.Length);



            if (sFooterLeft == null && sFooterCenter == null && sFooterRight == null)
            {
                //
            }
            else
            {
                sFooter = sFooter + "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\">\n";
                sFooter = sFooter + "<tr>\n";
                sFooter = sFooter + "<td valign=\"top\">\n";
                sFooter = sFooter + "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\">\n";
                for (int i = 0; i < iLeftLength; i++)
                {
                    sFooter = sFooter + "<tr>\n";
                    sFooter = sFooter + "<td >" + (i < iLeftLength ? sFooterLeft.ToString() : " ") + "</td>\n";
                    sFooter = sFooter + "</tr>\n";
                }
                sFooter = sFooter + "</table>\n";
                sFooter = sFooter + "</td>\n";
                if (sFooterCenter != null)
                {
                    sFooter = sFooter + "<td valign=\"top\" align=\"left\">\n";
                    sFooter = sFooter + "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\">\n";
                    for (int i = 0; i < iCenterLength; i++)
                    {
                        sFooter = sFooter + "<tr>\n";
                        sFooter = sFooter + "<td >" + (i < iCenterLength ? sFooterCenter.ToString() : " ") + "</td>\n";
                        sFooter = sFooter + "</tr>\n";
                    }
                    sFooter = sFooter + "</table>\n";
                    sFooter = sFooter + "</td>\n";
                }
                sFooter = sFooter + "<td valign=\"top\" align=\"right\">\n";
                sFooter = sFooter + "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" >\n";
                for (int i = 0; i < iRightLength; i++)
                {
                    sFooter = sFooter + "<tr>\n";
                    sFooter = sFooter + "<td >" + (i < iRightLength ? sFooterRight.ToString() : " ") + "</td>\n";
                    sFooter = sFooter + "</tr>\n";
                }
                sFooter = sFooter + "</table>\n";
                sFooter = sFooter + "</td>\n";
                sFooter = sFooter + "</tr>\n";
                sFooter = sFooter + "</table>\n";
            }
            return sFooter;
        }
        #endregion



        #region 添加页的标题
        /// <summary>
        /// 功能：添加页的标题
        /// </summary>
        private string AddPageTitle()
        {
            string sTitle = "";



            sTitle = sTitle + "\n<table border=\"0\" height=\"40\" width=\"100%\"><tr><td align=\"center\" class=\"print-title\" >" + sPageTitle + "</td></tr></table>\n";



            return sTitle;
        }
        #endregion



        #region 添加页的换页符，打印换页
        /// <summary>
        /// 功能：添加页的换页符
        /// </summary>
        private string AddPageBreak()
        {
            string sPageBreak = "";



            sPageBreak = sPageBreak + "\n<p style='page-break-before:always'>\n";



            return sPageBreak;
        }
        #endregion

        #region 重写模板文件，恢复打印模板样式
        /// <summary>
        /// 恢复打印模板
        /// </summary>
        public void ClearView()
        {
            StringBuilder htmltext = new StringBuilder();
            try
            {
                if (!File.Exists(Server.MapPath(Context.Request.ApplicationPath) + "\\printWindow.htm"))
                {
                    FileStream fs = File.Create(Server.MapPath(Context.Request.ApplicationPath) + "\\printWindow.htm");
                    fs.Close();
                }



                using (StreamWriter sw = new StreamWriter(Server.MapPath(Context.Request.ApplicationPath) + "\\printWindow.htm"))
                {
                    string sTableTitle = "";
                    sTableTitle = sTableTitle + "<html>\n";
                    sTableTitle = sTableTitle + "<head>\n";
                    sTableTitle = sTableTitle + "<title></title>\n";
                    sTableTitle = sTableTitle + "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=GB2312\">\n";

                    #region 模板页的样式
                    sTableTitle = sTableTitle + "<style type=\"text/css\">\n";
                    sTableTitle = sTableTitle + "<!--\n";
                    sTableTitle = sTableTitle + "body,table,tr,td,p{font-size:9pt;font-family:宋体;line-height:1.4}\n";
                    sTableTitle = sTableTitle + ".print-coltitle\n";
                    sTableTitle = sTableTitle + "{\n";
                    sTableTitle = sTableTitle + " border-right: black 1px solid;\n";
                    sTableTitle = sTableTitle + " border-top: black 0px solid;\n";
                    sTableTitle = sTableTitle + " border-left: black 0px solid;\n";
                    sTableTitle = sTableTitle + " border-bottom: black 2px solid;\n";
                    sTableTitle = sTableTitle + " white-space: normal;\n";
                    sTableTitle = sTableTitle + " background-color: #eeeeee;\n";
                    sTableTitle = sTableTitle + "}\n";



                    sTableTitle = sTableTitle + ".print-row\n";
                    sTableTitle = sTableTitle + "{\n";
                    sTableTitle = sTableTitle + " border-right: black 1px solid;\n";
                    sTableTitle = sTableTitle + " border-top: black 0px solid;\n";
                    sTableTitle = sTableTitle + " border-left: black 0px solid;\n";
                    sTableTitle = sTableTitle + " border-bottom: black 1px solid;\n";
                    sTableTitle = sTableTitle + " white-space:normal;\n";
                    sTableTitle = sTableTitle + " background-color: white;\n";
                    sTableTitle = sTableTitle + "}\n";



                    sTableTitle = sTableTitle + ".print-body\n";
                    sTableTitle = sTableTitle + "{\n";
                    sTableTitle = sTableTitle + " border-right: black 1px solid;\n";
                    sTableTitle = sTableTitle + " border-top: black 2px solid;\n";
                    sTableTitle = sTableTitle + " border-left: black 2px solid;\n";
                    sTableTitle = sTableTitle + " border-bottom: black 2px solid;\n";
                    sTableTitle = sTableTitle + " background-color: white;\n";
                    sTableTitle = sTableTitle + " white-space: normal;\n";
                    sTableTitle = sTableTitle + "}\n";
                    sTableTitle = sTableTitle + ".print-title{font-size:12pt;font-family:宋体;line-height:1.4}\n";
                    sTableTitle = sTableTitle + "-->\n";
                    sTableTitle = sTableTitle + "</style>\n";
                    #endregion



                    sTableTitle = sTableTitle + "<script language=\"javascript\">function maximizeWin() { if (window.screen) { var aw = screen.availWidth; var ah = screen.availHeight;window.moveTo(-4, -4); window.resizeTo(aw+9, ah+4); } } function TableAllRowSpan(TableName,ColumnIndex){var objTableName = TableName;var strTmp = \"\" ; var SpanCount = 1 ;var SpanStart = 1;var SpanOffSet = 0 ;var RowIndex = 0 ;for ( RowIndex = 1 ; RowIndex < objTableName.rows.length ; RowIndex++ ){ if (strTmp == objTableName.rows(RowIndex).cells(ColumnIndex).outerText) {SpanCount++;}else{strTmp = objTableName.rows(RowIndex).cells(ColumnIndex).outerText ;if ( RowIndex != 1 ){objTableName.rows(SpanStart).cells(ColumnIndex).rowSpan = SpanCount; for ( SpanOffSet = 1 ; SpanOffSet < SpanCount ; SpanOffSet++){objTableName.rows(SpanStart + SpanOffSet).cells(ColumnIndex).style.display = \"none\";}}SpanStart = RowIndex ; SpanCount = 1;}}if ( RowIndex != 1 ){objTableName.rows(SpanStart).cells(ColumnIndex).rowSpan = SpanCount;for ( SpanOffSet = 1 ; SpanOffSet < SpanCount ; SpanOffSet++ ){objTableName.rows(SpanStart + SpanOffSet).cells(ColumnIndex).style.display = \"none\";}}}";
                    sTableTitle = sTableTitle + "function TableSomeRowSpan (TableName, ColumnIndex ,bRow, eRow){ var objTableName = TableName;var strTmp = \"\" ; var SpanCount = 1 ;var SpanStart = 1 ;var SpanOffSet = 0 ;var RowIndex = 0 ;for ( RowIndex = bRow ; RowIndex < eRow ; RowIndex++ ){ if (strTmp == objTableName.rows(RowIndex).cells(ColumnIndex).outerText) {SpanCount++;} else{strTmp = objTableName.rows(RowIndex).cells(ColumnIndex).outerText ;if ( RowIndex != bRow ){objTableName.rows(SpanStart).cells(ColumnIndex).rowSpan = SpanCount;for ( SpanOffSet = 1 ; SpanOffSet < SpanCount ; SpanOffSet++){objTableName.rows(SpanStart + SpanOffSet).cells(ColumnIndex).style.display = \"none\";}}SpanStart = RowIndex ; SpanCount = 1;}}if ( RowIndex != bRow ){objTableName.rows(SpanStart).cells(ColumnIndex).rowSpan = SpanCount;for ( SpanOffSet = 1 ; SpanOffSet < SpanCount ; SpanOffSet++ ){objTableName.rows(SpanStart + SpanOffSet).cells(ColumnIndex).style.display = \"none\";}}}function TableRowSpan(TableName,ColumnIndex){var objTableName = document.all(TableName);var SpanCount = 1 ;var SpanStart = 1 ;var SpanOffSet = 0 ; var RowIndex = 0 ;var BColumnIndex =0;if(ColumnIndex==0){TableAllRowSpan(objTableName,ColumnIndex);} else{BColumnIndex = ColumnIndex - 1 ;for ( RowIndex = 1 ; RowIndex < objTableName.rows.length ; RowIndex++ ){SpanOffSet = objTableName.rows(RowIndex).cells(BColumnIndex).rowSpan;if(SpanOffSet > 1){SpanStart = RowIndex; SpanCount = SpanStart + SpanOffSet; TableSomeRowSpan (objTableName, ColumnIndex ,SpanStart, SpanCount); RowIndex = RowIndex + SpanOffSet-1;}}}} maximizeWin();</script>\n";

                    sTableTitle = sTableTitle + "</head>\n";
                    sTableTitle = sTableTitle + "<body>\n";
                    sTableTitle = sTableTitle + "Model\n";
                    sTableTitle = sTableTitle + "</body>\n";
                    sTableTitle = sTableTitle + "</html>\n";
                    sw.WriteLine(sTableTitle);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch
            {
                Response.Write("<Script>alert('读取文件错误')</Script>");
            }
        }
        #endregion
    }
}


