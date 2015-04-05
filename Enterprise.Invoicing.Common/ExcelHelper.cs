using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.Web;

namespace Enterprise.Invoicing.Common
{
    /// <summary>
    /// 读取excel文档
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// 读取2003及以前版本.xls
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static DataTable Read2003ToTable(string path)
        {
            HSSFWorkbook hssfworkbook;
            path = HttpContext.Current.Server.MapPath(path);

            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }

            HSSFSheet sheet = (HSSFSheet)hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

           
       /* 
         * ②：将文档保存到指定路径 
         */  
        string destFileName = @"D:\test.xls"; 
            //HSSFWorkbook hssfworkbook2 = writeToExcel();  
        MemoryStream msfile = new MemoryStream();  
        hssfworkbook.Write(msfile);  
        System.IO.File.WriteAllBytes(destFileName, msfile.ToArray());  



            DataTable dt = new DataTable(); for (int j = 0; j < 5; j++) { dt.Columns.Add(Convert.ToChar(((int)'A') + j).ToString()); }
            bool firstr = true; while (rows.MoveNext())
            {
                HSSFRow row = (HSSFRow)rows.Current;
                #region 第一行，初始化dt
                if (firstr)
                {
                    for (int j = 0; j < row.LastCellNum; j++) { dt.Columns.Add("column" + j); }
                    firstr = false;
                }
                #endregion
                for (int i = 0; i < row.LastCellNum; i++)
                {
                    HSSFCell cell = (HSSFCell)row.GetCell(i);
                    DataRow dr = dt.NewRow();
                    if (cell == null) { dr[i] = null; }
                    else { dr[i] = cell.ToString(); }
                    dt.Rows.Add(dr);                    
                }
            }
            return dt;
        }

        /// <summary>
        /// 读取2007以上版本.xlsx
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static DataTable Read2007ToTable(string path)
        {
            XSSFWorkbook hssfworkbook;
            path = HttpContext.Current.Server.MapPath(path);

            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new XSSFWorkbook(file);
            }

            XSSFSheet sheet = (XSSFSheet)hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();


            DataTable dt = new DataTable();
            bool firstr = true;
            while (rows.MoveNext())
            {
                XSSFRow row = (XSSFRow)rows.Current;
                #region 第一行，初始化dt
                if (firstr)
                {
                    for (int j = 0; j < row.LastCellNum; j++) { dt.Columns.Add("column" + j); }
                    firstr = false;
                }
                #endregion
                for (int i = 0; i < row.LastCellNum; i++)
                {
                    XSSFCell cell = (XSSFCell)row.GetCell(i);
                    DataRow dr = dt.NewRow();
                    if (cell == null) { dr[i] = null; }
                    else { dr[i] = cell.ToString(); }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        /// <summary>
        /// 读取2007以上版本.xlsx
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string Read2007ToString(string path)
        {
            XSSFWorkbook hssfworkbook;
            path = HttpContext.Current.Server.MapPath(path);

            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new XSSFWorkbook(file);
            }

            XSSFSheet sheet = (XSSFSheet)hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();



            StringBuilder sb = new StringBuilder();
            int irow = 0;
            sb.Append("<table>");
            while (rows.MoveNext())
            {
                XSSFRow row = (XSSFRow)rows.Current;
                irow++;
                sb.Append("<tr>");
                for (int i = 0; i < row.LastCellNum; i++)
                {
                    XSSFCell cell = (XSSFCell)row.GetCell(i);
                    string dr = "";
                    if (cell == null) { dr = ""; }
                    else { dr = cell.ToString(); }
                    sb.Append("<td>" + dr + "</td>");//("+irow+","+i+")"+
                }
                sb.Append("</tr>");
            }
            /* 
            * ②：将文档保存到指定路径 
                */
            string destFileName = @"D:\test.xlsx";
            //HSSFWorkbook hssfworkbook2 = writeToExcel();  
            MemoryStream msfile = new MemoryStream();
            hssfworkbook.Write(msfile);
            System.IO.File.WriteAllBytes(destFileName, msfile.ToArray());
            sb.Append("</table>");
            return sb.ToString();
        }


        /// <summary>
        /// 读取2007以上版本.xlsx
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string Read2003ToString(string path)
        {
            HSSFWorkbook hssfworkbook;
            path = HttpContext.Current.Server.MapPath(path);

            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }

            HSSFSheet sheet = (HSSFSheet)hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            StringBuilder sb = new StringBuilder();
            int irow = 0;
            sb.Append("<table>");
            while (rows.MoveNext())
            {
                HSSFRow row = (HSSFRow)rows.Current;
                irow++;
                sb.Append("<tr>");
                for (int i = 0; i < row.LastCellNum; i++)
                {
                    HSSFCell cell = (HSSFCell)row.GetCell(i);
                    string dr = "";
                    if (cell == null) { dr = ""; }
                    else { dr = cell.ToString(); }
                    sb.Append("<td>" + dr + "</td>");//("+irow+","+i+")"+
                }
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }

        #region 保存

        /// <summary>
        /// 读取2007以上版本.xlsx
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string Read2007ToString(string path,string name)
        {
            XSSFWorkbook hssfworkbook;
            var fpath = HttpContext.Current.Server.MapPath(path + name);

            using (FileStream file = new FileStream(fpath, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new XSSFWorkbook(file);
            }

            XSSFSheet sheet = (XSSFSheet)hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();



            StringBuilder sb = new StringBuilder();
            int irow = 0;
            sb.Append("<table>");
            while (rows.MoveNext())
            {
                XSSFRow row = (XSSFRow)rows.Current;
                irow++;
                sb.Append("<tr>");
                for (int i = 0; i < row.LastCellNum; i++)
                {
                    XSSFCell cell = (XSSFCell)row.GetCell(i);
                    string dr = "";
                    if (cell == null) { dr = ""; }
                    else {
                        dr = cell.ToString();
                        if (dr == "$ClientName$") cell.SetCellValue("这是写名称");
                        if (dr == "$Content$") cell.SetCellValue("这是写联系人");
                    }
                    sb.Append("<td>" + dr + "</td>");//("+irow+","+i+")"+
                    
                }
                sb.Append("</tr>");
            }
            /* 
            * ②：将文档保存到指定路径 
            */
            var destFileName = HttpContext.Current.Server.MapPath(path + "new_" + name);
            //HSSFWorkbook hssfworkbook2 = writeToExcel();  
            MemoryStream msfile = new MemoryStream();
            hssfworkbook.Write(msfile);
            System.IO.File.WriteAllBytes(destFileName, msfile.ToArray());
            sb.Append("</table>");
            return sb.ToString();
        }
        /// <summary>
        /// 读取2007以上版本.xlsx
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string Read2003ToString(string path, string name)
        {
            HSSFWorkbook hssfworkbook;
            var fpath = HttpContext.Current.Server.MapPath(path + name);

            using (FileStream file = new FileStream(fpath, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }

            HSSFSheet sheet = (HSSFSheet)hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            StringBuilder sb = new StringBuilder();
            int irow = 0;
            sb.Append("<table>");
            while (rows.MoveNext())
            {
                HSSFRow row = (HSSFRow)rows.Current;
                irow++;
                sb.Append("<tr>");
                for (int i = 0; i < row.LastCellNum; i++)
                {
                    HSSFCell cell = (HSSFCell)row.GetCell(i);
                    string dr = "";
                    if (cell == null)
                    {
                        dr = "";
                    }
                    else
                    {
                        dr = cell.ToString();
                        if (dr == "$ClientName$") cell.SetCellValue("这是写名称");
                        if (dr == "$Content$") cell.SetCellValue("这是写联系人");
                    }
                    sb.Append("<td>" + dr + "</td>");//("+irow+","+i+")"+
                }
                sb.Append("</tr>");
            }   /* 
            * ②：将文档保存到指定路径 
            */
            var destFileName = HttpContext.Current.Server.MapPath(path + "new_" + name);
            //HSSFWorkbook hssfworkbook2 = writeToExcel();  
            MemoryStream msfile = new MemoryStream();
            hssfworkbook.Write(msfile);
            System.IO.File.WriteAllBytes(destFileName, msfile.ToArray());
            sb.Append("</table>");
            return sb.ToString();
        }
        #endregion
    }
}
