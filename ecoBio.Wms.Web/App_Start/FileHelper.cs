
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using NPOI;
using NPOI.HPSF;
using NPOI.HSSF;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.POIFS;
using NPOI.Util;
using System.Data;
using Enterprise.Invoicing.ViewModel;  

namespace Enterprise.Invoicing.Web
{
    public class FileHelper
    {

        public static string ExportEasy(string[] heads, List<string> data)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet();

            //填充表头   
            HSSFRow dataRow = (HSSFRow)sheet.CreateRow(0);
            for (int i = 0; i < heads.Length; i++)
            {
                dataRow.CreateCell(i).SetCellValue(heads[i]);
            }


            //填充内容   
            for (int i = 0; i < data.Count; i++)
            {
                dataRow = (HSSFRow)sheet.CreateRow(i + 1);
                string[] str = data[i].Split('|');
                for (int j = 0; j < heads.Length; j++)
                {
                    dataRow.CreateCell(j).SetCellValue(str[j]);
                }
            }


            //保存   
            using (MemoryStream ms = new MemoryStream())
            {
                var name = @"C:\tempfile\" + Guid.NewGuid().ToString() + ".xls";
                try
                {
                    using (FileStream fs2 = new FileStream(name, FileMode.Create, FileAccess.Write))
                    {
                        workbook.Write(fs2);
                    }

                    #region 下载
                    //以字符流的形式下载文件
                    FileStream fs = new FileStream(name, FileMode.Open);
                    byte[] bytes = new byte[(int)fs.Length];
                    fs.Read(bytes, 0, bytes.Length);
                    fs.Close();
                    HttpContext.Current.Response.ContentType = "application/octet-stream";
                    //通知浏览器下载文件而不是打开
                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;  filename=" + HttpUtility.UrlEncode("report.xls", System.Text.Encoding.UTF8));
                    HttpContext.Current.Response.BinaryWrite(bytes);
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.End();
                    #endregion
                    return "下载成功";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                finally
                {
                    File.Delete(name);
                }

            }
        }
    }
}