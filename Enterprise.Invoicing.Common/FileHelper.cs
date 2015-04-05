
using Enterprise.Invoicing.ViewModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Enterprise.Invoicing.Common
{
    public class FileHelper
    {
        public static List<PdfFileModel> GetPdfFileList(string path)
        {
            List<PdfFileModel> l = new List<PdfFileModel>();
            //string directory = HttpContext.Current.Server.MapPath(path);
            if (Directory.Exists(path))
            {
                DirectoryInfo di = new DirectoryInfo(path);

                FileInfo[] FI = di.GetFiles("*.pdf");//只查.zip文件
                if (FI.Length > 0)
                {
                    foreach (FileInfo tmpFI in FI)
                    {
                        PdfFileModel one = new PdfFileModel();
                        one.create = tmpFI.CreationTime;
                        one.name = tmpFI.Name;
                        one.path = ".." + path + "/" + tmpFI.Name;
                        one.fordate = one.create.Date.AddMonths(-1).ToString("yyyyMM");
                        l.Add(one);
                    }
                }
            }
            return l;
        }
        public static List<PdfFileModel> GetServicePdfFileList(string path, string virtuarl)
        {
            List<PdfFileModel> l = new List<PdfFileModel>();
            string directory = path;// HttpContext.Current.Server.MapPath(path);
            if (Directory.Exists(directory))
            {
                DirectoryInfo di = new DirectoryInfo(directory);

                FileInfo[] FI = di.GetFiles("*.pdf");//只查.zip文件
                if (FI.Length > 0)
                {
                    foreach (FileInfo tmpFI in FI)
                    {
                        PdfFileModel one = new PdfFileModel();
                        one.create = tmpFI.CreationTime;
                        one.name = tmpFI.Name;
                        try
                        {
                            one.guid = Guid.Parse(one.name.Substring(0, one.name.Length - 4));
                        }
                        catch
                        {
                            one.guid = Guid.Empty;
                        }
                        one.path = virtuarl + tmpFI.Name;
                        one.fordate = one.create.Date.AddMonths(-1).ToString("yyyyMM");
                        l.Add(one);
                    }
                }
            }
            return l;
        }
        /// <summary>
        /// 月报列表
        /// </summary>
        /// <param name="path"></param>
        /// <param name="virtuarl"></param>
        /// <returns></returns>
        public static List<PdfFileModel> GetServiceMonthList(string path, string virtuarl)
        {
            List<PdfFileModel> l = new List<PdfFileModel>();
            string directory = path;
            if (Directory.Exists(directory))
            {
                DirectoryInfo di = new DirectoryInfo(directory);

                FileInfo[] FI = di.GetFiles();
                if (FI.Length > 0)
                {
                    foreach (FileInfo tmpFI in FI)
                    {
                        PdfFileModel one = new PdfFileModel();
                        if (!tmpFI.Name.EndsWith(".txt")) continue;
                        one.create = tmpFI.CreationTime;
                        one.name = tmpFI.Name;
                        one.month = one.name.Substring(0, one.name.Length - 4);
                        one.reporttype = 3;
                        one.haspdf = false;
                        var h = FI.FirstOrDefault(p => p.Name.StartsWith(one.month) && p.Name.EndsWith(".pdf"));
                        if (h != null)//存在pdf
                        {
                            one.haspdf = true;
                        }                      
                        one.path = virtuarl + tmpFI.Name;
                        one.fordate = one.month.Substring(0, 4) + "年" + one.month.Substring(4) + "月";
                        l.Add(one);
                    }
                }
            }
            return l;
        }
        /// <summary>
        /// 周报列表
        /// </summary>
        /// <param name="path"></param>
        /// <param name="virtuarl"></param>
        /// <returns></returns>
        public static List<PdfFileModel> GetServiceFileList(string path,string virtuarl)
        {
            List<PdfFileModel> l = new List<PdfFileModel>();
            string directory = path;
            if (Directory.Exists(directory))
            {
                DirectoryInfo di = new DirectoryInfo(directory);

                FileInfo[] FI = di.GetFiles();
                if (FI.Length > 0)
                {
                    foreach (FileInfo tmpFI in FI)
                    {
                        PdfFileModel one = new PdfFileModel();
                        one.create = tmpFI.CreationTime;
                        one.name = tmpFI.Name;
                        one.week = Convert.ToInt32(one.name.Substring(0, one.name.Length - 4));// Utils.GetWeekOfYear(one.create);
                        one.reporttype = 2;                        
                        one.path = virtuarl + tmpFI.Name;
                        one.fordate = one.create.Date.ToString("yyyyMMdd");
                        l.Add(one);
                    }
                }
            }
            return l;
        }

        /// <summary>
        /// 日报列表
        /// </summary>
        /// <param name="path"></param>
        /// <param name="virtuarl"></param>
        /// <returns></returns>
        public static List<PdfFileModel> GetServiceDayList(string path, string virtuarl)
        {
            List<PdfFileModel> l = new List<PdfFileModel>();
            string directory = path;
            if (Directory.Exists(directory))
            {
                DirectoryInfo di = new DirectoryInfo(directory);

                FileInfo[] FI = di.GetFiles();
                if (FI.Length > 0)
                {
                    foreach (FileInfo tmpFI in FI)
                    {
                        PdfFileModel one = new PdfFileModel();
                        one.create = tmpFI.CreationTime;
                        one.name = tmpFI.Name;
                        one.day = one.name.Substring(0, one.name.Length - 4);
                        one.reporttype = 1;
                        one.path = virtuarl + tmpFI.Name;
                        one.fordate = one.create.Date.ToString("yyyy-MM-dd");
                        l.Add(one);
                    }
                }
            }
            return l;
        }

        public static List<InventoryPdfFile> GetFileList(string path, string suffix)
        {
            List<InventoryPdfFile> l = new List<InventoryPdfFile>();
            string directory = HttpContext.Current.Server.MapPath(path);
            if (Directory.Exists(directory))
            {
                DirectoryInfo di = new DirectoryInfo(directory);

                FileInfo[] FI = di.GetFiles("*." + suffix);//只查.zip文件
                if (FI.Length > 0)
                {
                    foreach (FileInfo tmpFI in FI)
                    {
                        InventoryPdfFile one = new InventoryPdfFile();
                        one.create = tmpFI.CreationTime;
                        one.name = tmpFI.Name;
                        one.path = ".."+path + "/" + tmpFI.Name;
                        one.fordate = one.create.Date.AddMonths(-1).ToString("yyyyMM");
                        l.Add(one);
                    }
                }
            }
            return l;
        }
        /// <summary>
        /// 文件是否存在
        /// path：文件在服务器的物理路径:c:\res
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFileExist(string path)
        {
            //return File.Exists(HttpContext.Current.Server.MapPath(path));
            return File.Exists(path);
        }
        /// <summary>
        /// 判断文件目录是否存在
        /// 不存在则创建
        /// </summary>
        /// <param name="directory"></param>
        public static void CheckDirectory(string directory)
        {
            //if (Directory.Exists(HttpContext.Current.Server.MapPath(directory)) == false)//如果不存在就创建directory文件夹
            //{
            //    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(directory));
            //} 
            if (Directory.Exists(directory) == false)//如果不存在就创建directory文件夹
            {
                Directory.CreateDirectory(directory);
            }
        }
        /// <summary>
        /// 文件下载
        /// path：文件在网站根目录的路径
        /// name：下载时显示的文件名称
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        public static void DownLoadFile(string path,string name)
        {
            HttpContext.Current.Response.BufferOutput = false;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/x-msdownload";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + name);
            HttpContext.Current.Response.ContentType = "application/octstream";
            HttpContext.Current.Response.CacheControl = "Private";
            //HttpContext.Current.Server.MapPath(path)
            System.IO.Stream stm = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
            HttpContext.Current.Response.AppendHeader("Content-length", stm.Length.ToString());

            System.IO.BinaryReader br = new System.IO.BinaryReader(stm);

            byte[] bytes;

            for (Int64 x = 0; x < (br.BaseStream.Length / 4096 + 1); x++)
            {
                bytes = br.ReadBytes(4096);
                HttpContext.Current.Response.BinaryWrite(bytes);
                System.Threading.Thread.Sleep(5);  //休息一下,防止耗用带宽太多。
            }
            stm.Close();
        }

        public static void DeleteFile(string path)
        {
            //File.Delete(HttpContext.Current.Server.MapPath(path));
            File.Delete(path);
        }


        public static string ReadTxtFile(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);

            //StreamReader m_streamReader = new StreamReader(fs);
            StreamReader m_streamReader = new StreamReader(fs, Encoding.GetEncoding("GB2312"));
            m_streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
            string arry = "";
            string strLine = m_streamReader.ReadLine();
            do
            {
                arry += strLine;

                strLine = m_streamReader.ReadLine();

            } while (strLine != null && strLine != "");
            m_streamReader.Close();
            m_streamReader.Dispose();
            fs.Close();
            fs.Dispose();
            return arry;
        }

        #region 现场服务报告
        /// <summary>
        /// 读取2007以上版本.xlsx
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static void CreateServiceregulatory(string template, string savepath,string title,
            string customer,string starttime,string serviceperson,string completetime,string content,
             string checktime, string checkperson, string checkcontent
           )
        {
            XSSFWorkbook hssfworkbook;
            var fpath = HttpContext.Current.Server.MapPath(template);

            using (FileStream file = new FileStream(fpath, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new XSSFWorkbook(file);
            }

            XSSFSheet sheet = (XSSFSheet)hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            
                        int irow = 0;
            while (rows.MoveNext())
            {
                XSSFRow row = (XSSFRow)rows.Current;
                irow++;
                for (int i = 0; i < row.LastCellNum; i++)
                {
                    XSSFCell cell = (XSSFCell)row.GetCell(i);
                    string dr = "";
                    if (cell == null) { dr = ""; }
                    else
                    {
                        dr = cell.ToString();
                        if (dr.Contains("$title$")) cell.SetCellValue(dr.Replace("$title$", title));
                        if (dr.Contains("$customer$")) cell.SetCellValue(dr.Replace("$customer$", customer));
                        if (dr.Contains("$serviceperson$")) cell.SetCellValue(dr.Replace("$serviceperson$", serviceperson));
                        if (dr.Contains("$starttime$")) cell.SetCellValue(dr.Replace("$starttime$", starttime));
                        if (dr.Contains("$completetime$")) cell.SetCellValue(dr.Replace("$completetime$", completetime));
                        if (dr.Contains("$content$")) cell.SetCellValue(dr.Replace("$content$", content));
                        if (dr.Contains("$checkperson$")) cell.SetCellValue(dr.Replace("$checkperson$", checkperson));
                        if (dr.Contains("$checktime$")) cell.SetCellValue(dr.Replace("$checktime$", checktime));
                        if (dr.Contains("$checkcontent$")) cell.SetCellValue(dr.Replace("$checkcontent$", checkcontent));
                    }

                }
            }
            #region 另存为excel
            var destFileName = HttpContext.Current.Server.MapPath(savepath);
            //HSSFWorkbook hssfworkbook2 = writeToExcel();  
            MemoryStream msfile = new MemoryStream();
            hssfworkbook.Write(msfile);
            System.IO.File.WriteAllBytes(destFileName, msfile.ToArray());
            #endregion

            #region excel转化成pdf
           
            #endregion
        }

        /// <summary>
        /// 获取模板文件中的内容
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public static string ReadFileString(string template)
        {
            StringBuilder sb = new StringBuilder();
            if (!IsFileExist(template)) return "";
            var fpath = HttpContext.Current.Server.MapPath(template);
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(fpath);
                for (string line = reader.ReadLine();
                    line != null;
                    line = reader.ReadLine())
                    sb.Append(line);
            }
            catch (IOException e)
            {
                sb.Append(e.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return sb.ToString();
        }
        
        #endregion

        #region 维修单
        /// <summary>
        /// 维修单
        /// </summary>
        /// <param name="template"></param>
        /// <param name="savepath"></param>
        /// <param name="datas"></param>
        public static void CreateRepairReport(string template, string savepath, 
            string s13,string s111,string s23,string s211,string s33,string s311,
            string s63,string s611,string s73,string s711,string s103,string s113)
        {
            XSSFWorkbook hssfworkbook;
            var fpath =HttpContext.Current.Server.MapPath( template);
            using (FileStream file = new FileStream(fpath, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new XSSFWorkbook(file);
            }
            XSSFSheet sheet = (XSSFSheet)hssfworkbook.GetSheetAt(0);
            #region 插入数据

            XSSFRow r1 = (XSSFRow)sheet.GetRow(3);
            var r1s = r1.Cells.ToList();
            XSSFCell r13 = (XSSFCell)r1s[1];
            r13.SetCellValue(s13);
            XSSFCell r111 = (XSSFCell)r1s[3];
            r111.SetCellValue(s111);

            XSSFRow r2 = (XSSFRow)sheet.GetRow(4);
            var r2s = r2.Cells.ToList();
            XSSFCell r23 = (XSSFCell)r2s[1];
            r23.SetCellValue(s23);
            XSSFCell r211 = (XSSFCell)r2s[3];
            r211.SetCellValue(s33);

            XSSFRow r3 = (XSSFRow)sheet.GetRow(5);
            var r3s = r3.Cells.ToList();
            XSSFCell r33 = (XSSFCell)r3s[1];
            r33.SetCellValue(s211);

            XSSFRow r6 = (XSSFRow)sheet.GetRow(6);
            var r6s = r6.Cells.ToList();
            XSSFCell r311 = (XSSFCell)r6s[3];
            r311.SetCellValue(s311);



            XSSFRow r6t = (XSSFRow)sheet.GetRow(8);
            var r6st = r6t.Cells.ToList();
            XSSFCell r63t = (XSSFCell)r6st[1];
            r63t.SetCellValue(s63);
            XSSFCell r611t = (XSSFCell)r6st[3];
            r611t.SetCellValue(s611);

            XSSFRow r7 = (XSSFRow)sheet.GetRow(9);
            var r7s = r7.Cells.ToList();
            XSSFCell r73 = (XSSFCell)r7s[1];
            r73.SetCellValue(s73);
            XSSFCell r711 = (XSSFCell)r7s[3];
            r711.SetCellValue(s711);

            XSSFRow r10 = (XSSFRow)sheet.GetRow(10);
            var r10s = r10.Cells.ToList();
            XSSFCell r103 = (XSSFCell)r10s[1];
            r103.SetCellValue(s103);

            XSSFRow r110 = (XSSFRow)sheet.GetRow(11);
            var r110s = r110.Cells.ToList();
            XSSFCell r1103 = (XSSFCell)r110s[1];
            r1103.SetCellValue(s113);

            #endregion

            #region 插入数据

            XSSFRow r12 = (XSSFRow)sheet.GetRow(18);
            var r1s2 = r12.Cells.ToList();
            XSSFCell r132 = (XSSFCell)r1s2[1];
            r132.SetCellValue(s13);
            XSSFCell r1112 = (XSSFCell)r1s2[3];
            r1112.SetCellValue(s111);

            XSSFRow r22 = (XSSFRow)sheet.GetRow(19);
            var r2s2 = r22.Cells.ToList();
            XSSFCell r232 = (XSSFCell)r2s2[1];
            r232.SetCellValue(s23);
            XSSFCell r2112 = (XSSFCell)r2s2[3];
            r2112.SetCellValue(s33);

            XSSFRow r32 = (XSSFRow)sheet.GetRow(20);//供应商
            var r3s2 = r32.Cells.ToList();
            XSSFCell r332 = (XSSFCell)r3s2[1];
            r332.SetCellValue(s211);

            XSSFRow rt = (XSSFRow)sheet.GetRow(21);//图片
            var rtc = rt.Cells.ToList();
            XSSFCell rtc1 = (XSSFCell)rtc[1];
            rtc1.SetCellValue(s311);



            XSSFRow r62 = (XSSFRow)sheet.GetRow(23);
            var r6s2 = r62.Cells.ToList();
            XSSFCell r632 = (XSSFCell)r6s2[1];
            r632.SetCellValue(s63);
            XSSFCell r6112 = (XSSFCell)r6s2[3];
            r6112.SetCellValue(s611);

            XSSFRow r72 = (XSSFRow)sheet.GetRow(24);
            var r7s2 = r72.Cells.ToList();
            XSSFCell r732 = (XSSFCell)r7s2[1];
            r732.SetCellValue(s73);
            XSSFCell r7112 = (XSSFCell)r7s2[3];
            r7112.SetCellValue(s711);

            XSSFRow r102 = (XSSFRow)sheet.GetRow(25);
            var r10s2 = r102.Cells.ToList();
            XSSFCell r1032 = (XSSFCell)r10s2[1];
            r1032.SetCellValue(s103);

            XSSFRow r1102 = (XSSFRow)sheet.GetRow(26);
            var r110s2 = r1102.Cells.ToList();
            XSSFCell r11032 = (XSSFCell)r110s2[1];
            r11032.SetCellValue(s113);

            #endregion


            #region 另存为excel
            var destFileName =HttpContext.Current.Server.MapPath( savepath);
            MemoryStream msfile = new MemoryStream();
            hssfworkbook.Write(msfile);
            System.IO.File.WriteAllBytes(destFileName, msfile.ToArray());
            #endregion

            #region excel转化成pdf

            #endregion
        }

        #endregion

        #region 保养单
        /// <summary>
        /// 维修单
        /// </summary>
        /// <param name="template"></param>
        /// <param name="savepath"></param>
        /// <param name="datas"></param>
        public static void CreateMaintenanceReport(string template, string savepath,
            string s13, string s111, string s23, string s211, string s33, string s311,
            string s63, string s611, string s73, string s711, string s103)
        {
            XSSFWorkbook hssfworkbook;
            var fpath = HttpContext.Current.Server.MapPath(template);
            using (FileStream file = new FileStream(fpath, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new XSSFWorkbook(file);
            }
            XSSFSheet sheet = (XSSFSheet)hssfworkbook.GetSheetAt(0);
            #region 插入数据

            XSSFRow r1 = (XSSFRow)sheet.GetRow(3);
            var r1s = r1.Cells.ToList();
            XSSFCell r13 = (XSSFCell)r1s[1];
            r13.SetCellValue(s13);
            XSSFCell r111 = (XSSFCell)r1s[3];
            r111.SetCellValue(s111);

            XSSFRow r2 = (XSSFRow)sheet.GetRow(4);
            var r2s = r2.Cells.ToList();
            XSSFCell r23 = (XSSFCell)r2s[1];
            r23.SetCellValue(s23);
            XSSFCell r211 = (XSSFCell)r2s[3];
            r211.SetCellValue(s33);

            XSSFRow r3 = (XSSFRow)sheet.GetRow(5);
            var r3s = r3.Cells.ToList();
            XSSFCell r33 = (XSSFCell)r3s[1];
            r33.SetCellValue(s211);

            XSSFRow r6t = (XSSFRow)sheet.GetRow(8);
            var r6st = r6t.Cells.ToList();
            XSSFCell r63t = (XSSFCell)r6st[1];
            r63t.SetCellValue(s311);


            XSSFRow r6 = (XSSFRow)sheet.GetRow(8);
            var r6s = r6.Cells.ToList();
            XSSFCell r63 = (XSSFCell)r6s[1];
            r63.SetCellValue(s63);
            XSSFCell r611 = (XSSFCell)r6s[3];
            r611.SetCellValue(s611);

            XSSFRow r7 = (XSSFRow)sheet.GetRow(9);
            var r7s = r7.Cells.ToList();
            XSSFCell r73 = (XSSFCell)r7s[1];
            r73.SetCellValue(s73);
            XSSFCell r711 = (XSSFCell)r7s[3];
            r711.SetCellValue(s711);

            XSSFRow r10 = (XSSFRow)sheet.GetRow(10);
            var r10s = r10.Cells.ToList();
            XSSFCell r103 = (XSSFCell)r10s[1];
            r103.SetCellValue(s103);


            #endregion

            #region 插入数据

            XSSFRow r12 = (XSSFRow)sheet.GetRow(16);
            var r1s2 = r12.Cells.ToList();
            XSSFCell r132 = (XSSFCell)r1s2[1];
            r132.SetCellValue(s13);
            XSSFCell r1112 = (XSSFCell)r1s2[3];
            r1112.SetCellValue(s111);

            XSSFRow r22 = (XSSFRow)sheet.GetRow(17);
            var r2s2 = r22.Cells.ToList();
            XSSFCell r232 = (XSSFCell)r2s2[1];
            r232.SetCellValue(s23);
            XSSFCell r2112 = (XSSFCell)r2s2[3];
            r2112.SetCellValue(s33);

            XSSFRow r32 = (XSSFRow)sheet.GetRow(18);
            var r3s2 = r32.Cells.ToList();
            XSSFCell r332 = (XSSFCell)r3s2[1];
            r332.SetCellValue(s211);

            XSSFRow r32t = (XSSFRow)sheet.GetRow(19);
            var r3s2t = r32t.Cells.ToList();
            XSSFCell r332t = (XSSFCell)r3s2t[1];
            r332t.SetCellValue(s311);



            XSSFRow r62 = (XSSFRow)sheet.GetRow(21);
            var r6s2 = r62.Cells.ToList();
            XSSFCell r632 = (XSSFCell)r6s2[1];
            r632.SetCellValue(s63);
            XSSFCell r6112 = (XSSFCell)r6s2[3];
            r6112.SetCellValue(s611);

            XSSFRow r72 = (XSSFRow)sheet.GetRow(22);
            var r7s2 = r72.Cells.ToList();
            XSSFCell r732 = (XSSFCell)r7s2[1];
            r732.SetCellValue(s73);
            XSSFCell r7112 = (XSSFCell)r7s2[3];
            r7112.SetCellValue(s711);

            XSSFRow r102 = (XSSFRow)sheet.GetRow(23);
            var r10s2 = r102.Cells.ToList();
            XSSFCell r1032 = (XSSFCell)r10s2[1];
            r1032.SetCellValue(s103);

            #endregion


            #region 另存为excel
            var destFileName = HttpContext.Current.Server.MapPath(savepath);
            MemoryStream msfile = new MemoryStream();
            hssfworkbook.Write(msfile);
            System.IO.File.WriteAllBytes(destFileName, msfile.ToArray());
            #endregion

            #region excel转化成pdf

            #endregion
        }

        #endregion

        #region 库存报表
        public static void CreateInventoryReport(string template, string savepath,List<string> datas)
        {
            XSSFWorkbook hssfworkbook;
            var fpath = template;// HttpContext.Current.Server.MapPath(template);

            using (FileStream file = new FileStream(fpath, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new XSSFWorkbook(file);
            }

            XSSFSheet sheet = (XSSFSheet)hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            int irow = 0;
            while (rows.MoveNext())
            {
                XSSFRow row = (XSSFRow)rows.Current;
                irow++;
                for (int i = 0; i < row.LastCellNum; i++)
                {
                    XSSFCell cell = (XSSFCell)row.GetCell(i);
                    string dr = "";
                    if (cell == null) { dr = ""; }
                    else
                    {
                        //customercode,customername,logourl,materialcode materialname,materialmodel,materialunit,formonth beginamount,begincost,inamount,incost,
                        //outamount,outcost,endamount,endcost,beginlocalamount,beginlocalcost,endlocalamount,endlocalcost
                        dr = cell.ToString();
                        string strrow = datas.FirstOrDefault(p => p.Contains("|M001|"));
                        string[] array = strrow.Split('|');
                        if (dr.Contains("$title$")) cell.SetCellValue(dr.Replace("$title$", array[0]));
                        if (dr.Contains("$number$")) cell.SetCellValue(dr.Replace("$number$", array[1]));
                        //if (dr.Contains("$customername$")) cell.SetCellValue(dr.Replace("$customername$", customername));
                        //if (dr.Contains("$starttime$")) cell.SetCellValue(dr.Replace("$starttime$", starttime));
                        //if (dr.Contains("$completetime$")) cell.SetCellValue(dr.Replace("$completetime$", completetime));
                        //if (dr.Contains("$content$")) cell.SetCellValue(dr.Replace("$content$", content));
                        //if (dr.Contains("$checkperson$")) cell.SetCellValue(dr.Replace("$checkperson$", checkperson));
                        //if (dr.Contains("$checktime$")) cell.SetCellValue(dr.Replace("$checktime$", checktime));
                        //if (dr.Contains("$checkcontent$")) cell.SetCellValue(dr.Replace("$checkcontent$", checkcontent));
                    }

                }
            }
            #region 另存为excel
            var destFileName = savepath;// HttpContext.Current.Server.MapPath(savepath);
            //HSSFWorkbook hssfworkbook2 = writeToExcel();  
            MemoryStream msfile = new MemoryStream();
            hssfworkbook.Write(msfile);
            System.IO.File.WriteAllBytes(destFileName, msfile.ToArray());
            #endregion

            #region excel转化成pdf

            #endregion
        }

        #endregion
    }
}
