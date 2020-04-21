using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using TEST.EntityFrameworkCore;

namespace TEST.Management.Filter
{
    /// <summary>
    /// 登陆过滤器
    /// </summary>
    public class SignFilter : ActionFilterAttribute
    {
        /// <summary>
        /// 当动作执行中 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // 判断是否检查登陆
            var noNeedCheck = false;
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                noNeedCheck = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                  .Any(a => a.GetType().Equals(typeof(NoSignAttribute)));
            }
            if (noNeedCheck) return;

            // 检查登陆 - 在SignIn中判断用户合法性，将登陆信息保存在Session中，在SignOut中移除登陆信息
            // 获取登陆信息 - 这里采用Session来保存登陆信息 -- Constants是字符串常量池
            var signname = context.HttpContext.Session.GetString("UserName");
            var password = context.HttpContext.Session.GetString("PassWord");

            // 检查登陆信息
            if (signname == null || password == null)
            {
                // 用户未登陆 - 跳转到登陆界面
                context.Result = new RedirectResult("/Account/Login");
            }
            base.OnActionExecuting(context);
        }
    }
    /// <summary>
    /// 不需要登陆的地方加个特性
    /// </summary>
    public class NoSignAttribute : ActionFilterAttribute { }
}
