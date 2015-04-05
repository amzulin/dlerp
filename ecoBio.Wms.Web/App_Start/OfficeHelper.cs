using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;
using System.Text;
using System.Collections;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Drawing;

using System.Reflection;
using Microsoft.Office.Interop.Excel;
using ecoBio.Wms.ViewModel;


namespace ecoBio.Wms.Web
{
    /// <summary>
    /// 文档转化工具
    /// </summary>
    public static class OfficeHelper
    {
        /// <summary>
        /// PDF文件转化为SWF文件存在服务器filepath上,
        /// </summary>
        /// <param name="filepath">pdf文件所在路径：~/TestSWF/</param>
        /// <param name="savepath">swf文件存储的路径：~/TestSWF/</param>
        /// <param name="filename">文件名带.pdf</param>
        public static void PDF2Swf(string frompath, string topath, string filename)
        {

            //使用pdf2swf.exe 打开的文件名之间不能有空格，否则会失败
            string cmdStr = HttpContext.Current.Server.MapPath("~/Scripts/FlexPaper_1.4.5_flash/pdf2swf.exe");

            string serfrom = HttpContext.Current.Server.MapPath(frompath);
            string serto = HttpContext.Current.Server.MapPath(topath);

            string sourcePath = @"""" + serfrom + filename + @"""";
            string targetPath = @"""" + serto + filename.Substring(0, filename.LastIndexOf(".")) + ".swf" + @"""";

            //@"""" 四个双引号得到一个双引号，如果你所存放的文件所在文件夹名有空格的话，要在文件名的路径前后加上双引号，才能够成功
            // -t 源文件的路径
            // -s 参数化（也就是为pdf2swf.exe 执行添加一些窗外的参数(可省略)）
            string argsStr = "  -t " + sourcePath + " -s flashversion=9 -o " + targetPath;


            using (Process p = new Process())
            {

                ProcessStartInfo psi = new ProcessStartInfo(cmdStr, argsStr);
                p.StartInfo = psi;
                p.Start();
                p.WaitForExit();
            }
        }

        //Word转换成pdf
        ///// <summary>
        /// 把Word文件转换成为PDF格式文件
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <returns>true=转换成功</returns>
        public static bool DOCConvertToPDF(string sourcePath, string targetPath)
        {
            sourcePath = HttpContext.Current.Server.MapPath(sourcePath);
            targetPath = HttpContext.Current.Server.MapPath(targetPath);

            bool result = false;
            Word.WdExportFormat exportFormat = Word.WdExportFormat.wdExportFormatPDF;
            object paramMissing = Type.Missing;
            Word.ApplicationClass wordApplication = new Word.ApplicationClass();
            Word.Document wordDocument = null;
            try
            {
                object paramSourceDocPath = sourcePath;
                string paramExportFilePath = targetPath;

                Word.WdExportFormat paramExportFormat = exportFormat;
                bool paramOpenAfterExport = false;
                Word.WdExportOptimizeFor paramExportOptimizeFor = Word.WdExportOptimizeFor.wdExportOptimizeForPrint;
                Word.WdExportRange paramExportRange = Word.WdExportRange.wdExportAllDocument;
                int paramStartPage = 0;
                int paramEndPage = 0;
                Word.WdExportItem paramExportItem = Word.WdExportItem.wdExportDocumentContent;
                bool paramIncludeDocProps = true;
                bool paramKeepIRM = true;
                Word.WdExportCreateBookmarks paramCreateBookmarks = Word.WdExportCreateBookmarks.wdExportCreateWordBookmarks;
                bool paramDocStructureTags = true;
                bool paramBitmapMissingFonts = true;
                bool paramUseISO19005_1 = false;

                wordDocument = wordApplication.Documents.Open(
                ref paramSourceDocPath, ref paramMissing, ref paramMissing,
                ref paramMissing, ref paramMissing, ref paramMissing,
                ref paramMissing, ref paramMissing, ref paramMissing,
                ref paramMissing, ref paramMissing, ref paramMissing,
                ref paramMissing, ref paramMissing, ref paramMissing,
                ref paramMissing);

                if (wordDocument != null)
                    wordDocument.ExportAsFixedFormat(paramExportFilePath,
                    paramExportFormat, paramOpenAfterExport,
                    paramExportOptimizeFor, paramExportRange, paramStartPage,
                    paramEndPage, paramExportItem, paramIncludeDocProps,
                    paramKeepIRM, paramCreateBookmarks, paramDocStructureTags,
                    paramBitmapMissingFonts, paramUseISO19005_1,
                    ref paramMissing);
                result = true;
            }
            catch
            {
                result = false;
            }
            finally
            {
                if (wordDocument != null)
                {
                    wordDocument.Close(ref paramMissing, ref paramMissing, ref paramMissing);
                    wordDocument = null;
                }
                if (wordApplication != null)
                {
                    wordApplication.Quit(ref paramMissing, ref paramMissing, ref paramMissing);
                    wordApplication = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return result;
        }

        /// <summary>
        /// 把Excel文件转换成PDF格式文件
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <returns>true=转换成功</returns>
        public static bool XLSConvertToPDF(string sourcePath, string targetPath)
        {

            sourcePath = HttpContext.Current.Server.MapPath(sourcePath);
            targetPath = HttpContext.Current.Server.MapPath(targetPath);

            bool result = false;
            Excel.XlFixedFormatType targetType = Excel.XlFixedFormatType.xlTypePDF;
            object missing = Type.Missing;
            Excel.ApplicationClass application = null;
            Excel.Workbook workBook = null;
            try
            {
                application = new Excel.ApplicationClass();
                object target = targetPath;
                object type = targetType;
                workBook = application.Workbooks.Open(sourcePath, missing, missing, missing, missing, missing,
                missing, missing, missing, missing, missing, missing, missing, missing, missing);
                
                
                workBook.ExportAsFixedFormat(targetType, target, Excel.XlFixedFormatQuality.xlQualityStandard, true, false, missing, missing, missing, missing);
                result = true;
            }
            catch
            {
                result = false;
            }
            finally
            {
                if (workBook != null)
                {
                    workBook.Close(true, missing, missing);
                    workBook = null;
                }
                if (application != null)
                {
                    application.Quit();
                    application = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return result;
        }

        /// <summary>
        /// 把PowerPoing文件转换成PDF格式文件
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <returns>true=转换成功</returns>
        public static bool PPTConvertToPDF(string sourcePath, string targetPath)
        {
            sourcePath = HttpContext.Current.Server.MapPath(sourcePath);
            targetPath = HttpContext.Current.Server.MapPath(targetPath);
            bool result;
            PowerPoint.PpSaveAsFileType targetFileType = PowerPoint.PpSaveAsFileType.ppSaveAsPDF;
            object missing = Type.Missing;
            PowerPoint.ApplicationClass application = null;
            PowerPoint.Presentation persentation = null;
            try
            {
                application = new PowerPoint.ApplicationClass();
                persentation = application.Presentations.Open(sourcePath, MsoTriState.msoTrue, MsoTriState.msoFalse, MsoTriState.msoFalse);
                persentation.SaveAs(targetPath, targetFileType, Microsoft.Office.Core.MsoTriState.msoTrue);

                result = true;
            }
            catch
            {
                result = false;
            }
            finally
            {
                if (persentation != null)
                {
                    persentation.Close();
                    persentation = null;
                }
                if (application != null)
                {
                    application.Quit();
                    application = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return result;
        }

    }

    /// <summary>
    /// PSD2swf未测试
    /// </summary>
    public static class PSD2swfHelper
    {
        /// <summary>    
        /// 转换所有的页，图片质量80%    
        /// </summary>    
        /// <param name="pdfPath">PDF文件地址</param>    
        /// <param name="swfPath">生成后的SWF文件地址</param>    
        public static bool PDF2SWF(string pdfPath, string swfPath)
        {
            return PDF2SWF(pdfPath, swfPath, 1, GetPageCount(HttpContext.Current.Server.MapPath(pdfPath)), 80);
        }

        /// <summary>    
        /// 转换前N页，图片质量80%    
        /// </summary>    
        /// <param name="pdfPath">PDF文件地址</param>    
        /// <param name="swfPath">生成后的SWF文件地址</param>    
        /// <param name="page">页数</param>    
        public static bool PDF2SWF(string pdfPath, string swfPath, int page)
        {
            return PDF2SWF(pdfPath, swfPath, 1, page, 80);
        }

        /// <summary>    
        /// PDF格式转为SWF    
        /// </summary>    
        /// <param name="pdfPath">PDF文件地址</param>    
        /// <param name="swfPath">生成后的SWF文件地址</param>    
        /// <param name="beginpage">转换开始页</param>    
        /// <param name="endpage">转换结束页</param>    
        private static bool PDF2SWF(string pdfPath, string swfPath, int beginpage, int endpage, int photoQuality)
        {
            string exe = HttpContext.Current.Server.MapPath("FlexPaper/pdf2swf.exe");
            pdfPath = HttpContext.Current.Server.MapPath(pdfPath);
            swfPath = HttpContext.Current.Server.MapPath(swfPath);
            if (!System.IO.File.Exists(exe) || !System.IO.File.Exists(pdfPath) || System.IO.File.Exists(swfPath))
            {
                return false;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(" \"" + pdfPath + "\"");
            sb.Append(" -o \"" + swfPath + "\"");
            sb.Append(" -s flashversion=9");
            sb.Append(" -s languagedir=\"E:\\xpdf\\xpdf-chinese-simplified\"");
            if (endpage > GetPageCount(pdfPath)) endpage = GetPageCount(pdfPath);
            sb.Append(" -p " + "\"" + beginpage + "" + "-" + endpage + "\"");
            sb.Append(" -j " + photoQuality);
            string Command = sb.ToString();
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = exe;
            p.StartInfo.Arguments = Command;
            p.StartInfo.WorkingDirectory = HttpContext.Current.Server.MapPath("~/Bin/");
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = false;
            p.Start();
            p.BeginErrorReadLine();
            p.WaitForExit();
            p.Close();
            p.Dispose();
            return true;
        }

        /// <summary>    
        /// 返回页数    
        /// </summary>    
        /// <param name="pdfPath">PDF文件地址</param>    
        private static int GetPageCount(string pdfPath)
        {
            byte[] buffer = System.IO.File.ReadAllBytes(pdfPath);
            int length = buffer.Length;
            if (buffer == null)
                return -1;
            if (buffer.Length <= 0)
                return -1;
            string pdfText = Encoding.Default.GetString(buffer);
            System.Text.RegularExpressions.Regex rx1 = new System.Text.RegularExpressions.Regex(@"/Type\s*/Page[^s]");
            System.Text.RegularExpressions.MatchCollection matches = rx1.Matches(pdfText);
            return matches.Count;
        }
    }

    /// <summary>
    /// 生成PDF
    /// </summary>
    public static class PDFGenerator
    {
        static float pageWidth = 594.0f;
        static float pageDepth = 828.0f;
        static float pageMargin = 30.0f;
        static float fontSize = 20.0f;
        static float leadSize = 10.0f;

        static StreamWriter pPDF;// = new StreamWriter("E:\\myPDF.pdf");

        static MemoryStream mPDF;// = new MemoryStream();

        static void ConvertToByteAndAddtoStream(string strMsg)
        {
            Byte[] buffer = null;
            buffer = ASCIIEncoding.ASCII.GetBytes(strMsg);
            mPDF.Write(buffer, 0, buffer.Length);
            buffer = null;
        }

        static string xRefFormatting(long xValue)
        {
            string strMsg = xValue.ToString();
            int iLen = strMsg.Length;
            if (iLen < 10)
            {
                StringBuilder s = new StringBuilder();
                int i = 10 - iLen;
                s.Append('0', i);
                strMsg = s.ToString() + strMsg;
            }
            return strMsg;
        }

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="savepath"></param>
        public static void PDFCreate(string savepath)        {
            
            savepath = HttpContext.Current.Server.MapPath(savepath);
            pPDF = new StreamWriter(savepath);
            mPDF = new MemoryStream(); 
            ArrayList xRefs = new ArrayList();
            //Byte[] buffer=null;
            float yPos = 0f;
            long streamStart = 0;
            long streamEnd = 0;
            long streamLen = 0;
            string strPDFMessage = null;
            //PDF文档头信息
            strPDFMessage = "%PDF-1.1\n";
            ConvertToByteAndAddtoStream(strPDFMessage);

            xRefs.Add(mPDF.Length);
            strPDFMessage = "1 0 obj\n";
            ConvertToByteAndAddtoStream(strPDFMessage);
            strPDFMessage = "<< /Length 2 0 R >>\n";
            ConvertToByteAndAddtoStream(strPDFMessage);
            strPDFMessage = "stream\n";
            ConvertToByteAndAddtoStream(strPDFMessage);
            ////////PDF文档描述
            streamStart = mPDF.Length;
            //字体
            strPDFMessage = "BT\n/F0 " + fontSize + " Tf\n";
            ConvertToByteAndAddtoStream(strPDFMessage);
            //PDF文档实体高度
            yPos = pageDepth - pageMargin;
            strPDFMessage = pageMargin + " " + yPos + " Td\n";
            ConvertToByteAndAddtoStream(strPDFMessage);
            strPDFMessage = leadSize + " TL\n";
            ConvertToByteAndAddtoStream(strPDFMessage);

            //实体内容
            strPDFMessage = "(abcd)Tj\n";
            ConvertToByteAndAddtoStream(strPDFMessage);
            strPDFMessage = "ET\n";
            ConvertToByteAndAddtoStream(strPDFMessage);
            streamEnd = mPDF.Length;

            streamLen = streamEnd - streamStart;
            strPDFMessage = "endstream\nendobj\n";
            ConvertToByteAndAddtoStream(strPDFMessage);
            //PDF文档的版本信息
            xRefs.Add(mPDF.Length);
            strPDFMessage = "2 0 obj\n" + streamLen + "\nendobj\n";
            ConvertToByteAndAddtoStream(strPDFMessage);

            xRefs.Add(mPDF.Length);
            strPDFMessage = "3 0 obj\n<</Type/Page/Parent 4 0 R/Contents 1 0 R>>\nendobj\n";
            ConvertToByteAndAddtoStream(strPDFMessage);

            xRefs.Add(mPDF.Length);
            strPDFMessage = "4 0 obj\n<</Type /Pages /Count 1\n";
            ConvertToByteAndAddtoStream(strPDFMessage);
            strPDFMessage = "/Kids[\n3 0 R\n]\n";
            ConvertToByteAndAddtoStream(strPDFMessage);
            strPDFMessage = "/Resources<</ProcSet[/PDF/Text]/Font<</F0 5 0 R>> >>\n";
            ConvertToByteAndAddtoStream(strPDFMessage);
            strPDFMessage = "/MediaBox [ 0 0 " + pageWidth + " " + pageDepth + " ]\n>>\nendobj\n";
            ConvertToByteAndAddtoStream(strPDFMessage);

            xRefs.Add(mPDF.Length);
            strPDFMessage = "5 0 obj\n<</Type/Font/Subtype/Type1/BaseFont/Courier/Encoding/WinAnsiEncoding>>\nendobj\n";
            ConvertToByteAndAddtoStream(strPDFMessage);

            xRefs.Add(mPDF.Length);
            strPDFMessage = "6 0 obj\n<</Type/Catalog/Pages 4 0 R>>\nendobj\n";
            ConvertToByteAndAddtoStream(strPDFMessage);

            streamStart = mPDF.Length;
            strPDFMessage = "xref\n0 7\n0000000000 65535 f \n";
            for (int i = 0; i < xRefs.Count; i++)
            {
                strPDFMessage += xRefFormatting((long)xRefs[i]) + " 00000 n \n";
            }
            ConvertToByteAndAddtoStream(strPDFMessage);
            strPDFMessage = "trailer\n<<\n/Size " + (xRefs.Count + 1) + "\n/Root 6 0 R\n>>\n";
            ConvertToByteAndAddtoStream(strPDFMessage);

            strPDFMessage = "startxref\n" + streamStart + "\n%%EOF\n";
            ConvertToByteAndAddtoStream(strPDFMessage);
            mPDF.WriteTo(pPDF.BaseStream);

            mPDF.Close();
            pPDF.Close();
        }
    }

    /// <summary>
    /// 生成pdf，最终可用版本
    /// </summary>
    public static class PDFbyiTextSharp
    {
        public static void Genera(string sourcePath,string content)
        {
            sourcePath = HttpContext.Current.Server.MapPath(sourcePath);
            //定义一个Document，并设置页面大小为A4，竖向
            iTextSharp.text.Document doc = new Document(PageSize.A4);
            try
            {
                //写实例
                PdfWriter.GetInstance(doc, new FileStream(sourcePath, FileMode.Create));
                //打开document
                doc.Open();


                //载入字体
                BaseFont baseFont = BaseFont.CreateFont(
                    "C:\\WINDOWS\\FONTS\\SIMHEI.TTF", //黑体
                    BaseFont.IDENTITY_H, //横向字体
                    BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 9);


                //写入一个段落, Paragraph
                doc.Add(new Paragraph("第一段：" + content, font));
                doc.Add(new Paragraph("这是第二段 !", font));


                #region 图片
                //以下代码用来添加图片，此时图片是做为背景加入到pdf文件当中的：
                
                Stream inputImageStream = new FileStream(HttpContext.Current.Server.MapPath("~/images/logo.jpg"), FileMode.Open, FileAccess.Read, FileShare.Read);
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(inputImageStream);
                image.SetAbsolutePosition(0, 0);
                image.Alignment = iTextSharp.text.Image.UNDERLYING;    //这里可以设定图片是做为背景还是做为元素添加到文件中
                doc.Add(image);

                #endregion
                #region 其他元素
                doc.Add(new Paragraph("Hello World"));
                //另起一行。有几种办法建立一个段落，如： 
                Paragraph p1 = new Paragraph(new Chunk("This is my first paragraph.\n", FontFactory.GetFont(FontFactory.HELVETICA, 12)));
                Paragraph p2 = new Paragraph(new Phrase("This is my second paragraph.", FontFactory.GetFont(FontFactory.HELVETICA, 12)));
                Paragraph p3 = new Paragraph("This is my third paragraph.", FontFactory.GetFont(FontFactory.HELVETICA, 12));
                //所有有些对象将被添加到段落中：
                p1.Add("you can add string here\n\t");
                p1.Add(new Chunk("you can add chunks \n")); p1.Add(new Phrase("or you can add phrases.\n"));
                doc.Add(p1); doc.Add(p2); doc.Add(p3);

                //创建了一个内容为“hello World”、红色、斜体、COURIER字体、尺寸20的一个块： 
                Chunk chunk = new Chunk("创建了一个内容为“hello World”、红色、斜体、COURIER字体、尺寸20的一个块", FontFactory.GetFont(FontFactory.COURIER, 20, iTextSharp.text.Font.COURIER, new iTextSharp.text.Color(255, 0, 0)));
                doc.Add(chunk);
                //如果你希望一些块有下划线或删除线，你可以通过改变字体风格简单做到： 
                Chunk chunk1 = new Chunk("This text is underlined", FontFactory.GetFont(FontFactory.HELVETICA, 12, iTextSharp.text.Font.UNDEFINED));
                Chunk chunk2 = new Chunk("This font is of type ITALIC | STRIKETHRU", FontFactory.GetFont(FontFactory.HELVETICA, 12, iTextSharp.text.Font.ITALIC | iTextSharp.text.Font.STRIKETHRU));
                //改变块的背景
                chunk2.SetBackground(new iTextSharp.text.Color(0xFF, 0xFF, 0x00));
                //上标/下标
                chunk1.SetTextRise(5);
                doc.Add(chunk1);
                doc.Add(chunk2);

                //外部链接示例： 
                Anchor anchor = new Anchor("website", FontFactory.GetFont(FontFactory.HELVETICA, 12, iTextSharp.text.Font.UNDEFINED, new iTextSharp.text.Color(0, 0, 255)));
                anchor.Reference = "http://itextsharp.sourceforge.net";
                anchor.Name = "website";
                //内部链接示例： 
                Anchor anchor1 = new Anchor("This is an internal link\n\n");
                anchor1.Name = "link1";
                Anchor anchor2 = new Anchor("Click here to jump to the internal link\n\f");
                anchor2.Reference = "#link1";
                doc.Add(anchor); doc.Add(anchor1); doc.Add(anchor2);

                //排序列表示例： 
                List list = new List(true, 20);
                list.Add(new ListItem("First line"));
                list.Add(new ListItem("The second line is longer to see what happens once the end of the line is reached. Will it start on a new line?"));
                list.Add(new ListItem("Third line"));
                doc.Add(list);

                //文本注释： 
                Annotation a = new Annotation("authors", "Maybe its because I wanted to be an author myself that I wrote iText.");
                doc.Add(a);

                //包含页码没有任何边框的页脚。 
                iTextSharp.text.HeaderFooter footer = new iTextSharp.text.HeaderFooter(new Phrase("This is page: "), true);
                footer.Border = iTextSharp.text.Rectangle.NO_BORDER;
                doc.Footer = footer;


                //Chapter对象和Section对象自动构建一个树：
                iTextSharp.text.Font f1 = new iTextSharp.text.Font();
                f1.SetStyle(iTextSharp.text.Font.BOLD);
                Paragraph cTitle = new Paragraph("This is chapter 1", f1);
                Chapter chapter = new Chapter(cTitle, 1);
                Paragraph sTitle = new Paragraph("This is section 1 in chapter 1", f1);
                Section section = chapter.AddSection(sTitle, 1);
                doc.Add(chapter);

                //构建了一个简单的表： 
                Table aTable = new Table(4, 4);
                aTable.AutoFillEmptyCells = true;
                aTable.AddCell("2.2", new System.Drawing.Point(2, 2));
                aTable.AddCell("3.3", new System.Drawing.Point(3, 3));
                aTable.AddCell("2.1", new System.Drawing.Point(2, 1));
                aTable.AddCell("1.3", new System.Drawing.Point(1, 3));
                doc.Add(aTable);
                //构建了一个不简单的表：
                Table table = new Table(3);
                table.BorderWidth = 1;
                table.BorderColor = new iTextSharp.text.Color(0, 0, 255);
                table.Cellpadding = 5;
                table.Cellspacing = 5;
                Cell cell = new Cell("header");
                cell.Header = true;
                cell.Colspan = 3;
                table.AddCell(cell);
                cell = new Cell("example cell with colspan 1 and rowspan 2");
                cell.Rowspan = 2;
                cell.BorderColor = new iTextSharp.text.Color(255, 0, 0);
                table.AddCell(cell);
                table.AddCell("1.1");
                table.AddCell("2.1");
                table.AddCell("1.2");
                table.AddCell("2.2");
                table.AddCell("cell test1");
                cell = new Cell("big cell");
                cell.Rowspan = 2;
                cell.Colspan = 2;
                cell.BackgroundColor = new iTextSharp.text.Color(0xC0, 0xC0, 0xC0);
                table.AddCell(cell);
                table.AddCell("cell test2");
                // 改变了单元格“big cell”的对齐方式： 
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                doc.Add(table);

                #endregion
                //关闭document
                doc.Close();
                //打开PDF，看效果
                //Process.Start(sourcePath);
            }
            catch (DocumentException de)
            {
                Console.WriteLine(de.Message);
                Console.ReadKey();
            }
            catch (IOException io)
            {
                Console.WriteLine(io.Message);
                Console.ReadKey();
            }
        }

        public static void Genera(string sourcePath, string title, List<InventoryReportModel> data)
        {
            sourcePath = HttpContext.Current.Server.MapPath(sourcePath);
            //定义一个Document，并设置页面大小为A4，竖向
            iTextSharp.text.Document doc = new Document(PageSize.A4);
            try
            {
                //写实例
                PdfWriter.GetInstance(doc, new FileStream(sourcePath, FileMode.Create));
                //打开document
                doc.Open();


                //载入字体
                BaseFont baseFont = BaseFont.CreateFont(
                    "C:\\WINDOWS\\FONTS\\SIMHEI.TTF", //黑体
                    BaseFont.IDENTITY_H, //横向字体
                    BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 9);   
                
                #region 其他元素
               
                doc.Add(new Paragraph(title, font));

         
                //包含页码没有任何边框的页脚。 
                iTextSharp.text.HeaderFooter footer = new iTextSharp.text.HeaderFooter(new Phrase("This is page: "), true);
                footer.Border = iTextSharp.text.Rectangle.NO_BORDER;
                doc.Footer = footer;

                //构建了一个简单的表： 
               
                int rows = data.Count;
                Table aTable = new Table(15, rows+1);
                aTable.AutoFillEmptyCells = true;
                //行 从1开始，列 从0开始
                #region 表头
               // Chunk chunk1 = new Chunk("This text is underlined", FontFactory.GetFont(FontFactory.HELVETICA, 12, iTextSharp.text.Font.UNDEFINED));
               // Cell cell=new Cell(chunk1);
                //aTable.AddCell(cell,ne
                aTable.AddCell("序号", new System.Drawing.Point(1, 0));
                aTable.AddCell("耗材规格", new System.Drawing.Point(1, 1));
                aTable.AddCell("期初库存数", new System.Drawing.Point(1, 2));
                aTable.AddCell("期初库存数", new System.Drawing.Point(1,3));
                aTable.AddCell("期初库存额", new System.Drawing.Point(1, 4));
                aTable.AddCell("入库数", new System.Drawing.Point(1,5));
                aTable.AddCell("入库额", new System.Drawing.Point(1, 6));
                aTable.AddCell("出库数", new System.Drawing.Point(1, 7));
                aTable.AddCell("出库额", new System.Drawing.Point(1, 8));
                aTable.AddCell("期末库存数", new System.Drawing.Point(1, 9));
                aTable.AddCell("期末库存额", new System.Drawing.Point(1, 10));
                aTable.AddCell("期初结余量", new System.Drawing.Point(1, 11));
                aTable.AddCell("期初结余额", new System.Drawing.Point(1, 12));
                aTable.AddCell("期末结余量", new System.Drawing.Point(1, 13));
                aTable.AddCell("期末结余额", new System.Drawing.Point(1, 14));
                #endregion
                #region 内容
                for (int i = 0; i < rows; i++)
                {
                    var item = data[i];
                    aTable.AddCell((i + 1).ToString(), new System.Drawing.Point(i + 2, 0));
                    aTable.AddCell(item.material_specification_name + "(" + @item.material_specification_model + ")", new System.Drawing.Point(i + 2, 1));
                    aTable.AddCell(item.unit_html, new System.Drawing.Point(i + 2, 2));
                    aTable.AddCell(item.inventory_reports_beginning_amount.Value.ToString(), new System.Drawing.Point(i + 2, 3));
                    aTable.AddCell(item.inventory_reports_beginning_cost.Value.ToString("N"), new System.Drawing.Point(i + 2, 4));
                    aTable.AddCell(item.inventory_reports_in_amount.Value.ToString(), new System.Drawing.Point(i + 2, 5));
                    aTable.AddCell(item.inventory_reports_in_cost.Value.ToString("N"), new System.Drawing.Point(i + 2, 6));
                    aTable.AddCell(item.inventory_reports_out_amount.Value.ToString(), new System.Drawing.Point(i + 2, 7));
                    aTable.AddCell(item.inventory_reports_out_cost.Value.ToString("N"), new System.Drawing.Point(i + 2, 8));
                    aTable.AddCell(item.inventory_reports_ending_amount.Value.ToString(), new System.Drawing.Point(i + 2, 9));
                    aTable.AddCell(item.inventory_reports_ending_cost.Value.ToString("N"), new System.Drawing.Point(i + 2, 10));
                    aTable.AddCell(item.beginning_locale_amount.Value.ToString(), new System.Drawing.Point(i + 2, 11));
                    aTable.AddCell(item.beginning_locale_cost.Value.ToString("N"), new System.Drawing.Point(i + 2, 12));
                    aTable.AddCell(item.ending_locale_amount.Value.ToString(), new System.Drawing.Point(i + 2, 13));
                    aTable.AddCell(item.ending_locale_cost.Value.ToString("N"), new System.Drawing.Point(i + 2, 14));
                }
                #endregion
                doc.Add(aTable);



                #endregion
                //关闭document
                doc.Close();
                //打开PDF，看效果
                //Process.Start(sourcePath);
            }
            catch (DocumentException de)
            {
                Console.WriteLine(de.Message);
                Console.ReadKey();
            }
            catch (IOException io)
            {
                Console.WriteLine(io.Message);
                Console.ReadKey();
            }
        }

        public static void pdfencry(string sourcepath)
        {
            sourcepath = HttpContext.Current.Server.MapPath(sourcepath);
            iTextSharp.text.Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(sourcepath, FileMode.Open));
            writer.SetEncryption(PdfWriter.STRENGTH128BITS, null, null, PdfWriter.AllowPrinting);
            document.Open();
            document.Close();
            //gc.collect();
            //gc.waitforpendingfinalizers();
        }
    }

}