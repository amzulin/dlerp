using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Invoicing.Common
{
    /// <summary>
    /// Flex参数类
    /// </summary>
    public class Flexparameters
    {
        #region 属性--参数
        /// <summary>
        /// _clientCode:String	客户号	字符串	THYL
        /// </summary>
        public string _clientCode { get; set; }
        /// <summary>
        /// _fllexNumber:String	图表编号
        /// </summary>
        public string _fllexNumber { get; set; }
        /// <summary>
        ///_wsdl:String	数据服务URL地址	字符串	http://localhost:192.168.1.108/service.asmx?wsdl
        /// </summary>
        public string _wsdl { get; set; }
        /// <summary>
        /// _chartType:String	图表类型	字符串	line1(折线1)line2(折线2)line3(折线3)bar1(柱状图1)bar2(柱状图2)bar3(柱状图3)column1(柱状图)Pie（饼图）
        /// </summary>
        public string _chartType { get; set; }

        /// <summary>
        /// _canvasWidth:Number	宽度	数字	240（默认）
        /// </summary>
        public int _canvasWidth { get; set; }

        /// <summary>
        /// _canvasHeight:Number	高度	数字	180（默认）
        /// </summary>
        public int _canvasHeight { get; set; }

        /// <summary>
        /// _receiveStyle:String	接收风格	字符串	Once(一次)  cycle(循环)
        /// </summary>
        public string _receiveStyle { get; set; }

        /// <summary>
        ///  _receiveRows:Number	返回行数	数字	30
        /// </summary>
        public int _receiveRows { get; set; }

        /// <summary>
        /// _receviveSpan:Number	接收间隔	数字	秒
        /// </summary>
        public string _receviveSpan { get; set; }

        /// <summary>
        /// _chartTitle:String	图表标题	字符串	太湖饮料成本趋势图
        /// </summary>
        public string _chartTitle { get; set; }

        /// <summary>
        /// _chartLow:Number	上限	数字	60
        /// </summary>
        public int _chartLow { get; set; }

        /// <summary>
        /// _chartHeight:Number	下限	数字	20
        /// </summary>
        public int _chartHeight { get; set; }

        /// <summary>
        /// _queryStyle:Number	查询方式	数字	1(日期范围)    2(数字范围)   3(如期时间)
        /// </summary>
        public int _queryStyle { get; set; }
        /// <summary>
        /// _queryParams:String	查询参数	字符串	方式1：’2013-3-03-10 20:00,    //2013-03-11 19:00’    //方式2：’0,30’    //方式3：’2013-03-10 20:00’
        /// </summary>
        public string _queryParams { get; set; }

        /// <summary>
        ///  _processParams:String	工艺参数	字符串	‘单元号,工艺参数’
        /// </summary>
        public string _processParams { get; set; }
        #endregion

        #region 方法
        public void getSeries(string paras)
        {
            Flexparameters flex = JsonHelper.FromJson<Flexparameters>(paras);
        }
        public void getRows()
        { }
        public void getQueryParams()
        { }
        public Array split()
        {
            string a = "";
            return a.Split(',');
        }
        public string jsonSerial(object obj)
        {
            return JsonHelper.ToJson(obj);
        }

        #endregion
    }

    /// <summary>
    /// Flex数据获取
    /// </summary>
    public static class FlexData
    {
        /// <summary>
        /// 获得数据源
        /// </summary>
        /// <param name="clientcode">客户编号</param>
        /// <param name="flexnumber">图表编号</param>
        /// <param name="querystyle">查询方式	数字	1(日期范围)		2(数字范围)		3(如期时间)</param>
        /// <param name="queryparams">查询参数	字符串	方式1：’2013-3-03-10 20:00,2013-03-11 19:00’		方式2：’0,30’		方式3：’2013-03-10 20:00’</param>
        /// <param name="processparam">工艺参数	字符串	‘单元号,工艺参数’</param>
        /// <returns></returns>
        public static string GetDataSource(string clientcode, string flexnumber, string querystyle, string queryparams, string processparam)
        {
            throw new NotImplementedException();
        }
    }
}
