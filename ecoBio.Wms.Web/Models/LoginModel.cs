using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Enterprise.Invoicing.Web.Models
{
    public class LoginModel
    {
        //[Required(ErrorMessage = "动态口令不能为空！")]
        //[Display(Name = "动态口令")]
        //[StringLength(6, ErrorMessage = "请输入6位数动态口令！")]
        //[RegularExpression(@"\d{6}", ErrorMessage = "请输入6位数字动态口令")] 
        public string DynamicPassword { get; set; }

        //[Required(ErrorMessage = "验证码不能为空！")]
        //[Display(Name = "验证码")]
        //[StringLength(6, ErrorMessage = "请输入5位数验证码！")]
        //[RegularExpression(@"\d{6}", ErrorMessage = "请输入6位数字验证码")] 
        public string Verification { get; set; }
    }
}