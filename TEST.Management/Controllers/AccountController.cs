using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TEST.Exercise.Application.Admin;
using TEST.Management.Models;

namespace TEST.Management.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAdminService _adminService;
        public AccountController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Login(LoginViewModel data)
        {
            data.UserName = "Administrator";
            data.Password = "123456";
            HttpContext.Session.SetString("UserName", "Administrator");
            HttpContext.Session.SetString("PassWord", "123456");
            return RedirectToAction("Index", "Home");
            #region
            //if (ModelState.IsValid)
            //{
            //    if (_adminService.FindAdmininstrator(data.UserName, data.Password))
            //    {
            //        HttpContext.Session.SetString("UserName", data.UserName);
            //        HttpContext.Session.SetString("PassWord", data.Password);
            //        return RedirectToAction("Index", "Home");
            //    }
            //    else
            //    {
            //        ModelState.AddModelError("Password", "账号或者密码错误");
            //    }
            //}            
            //return View();
            #endregion
        }
    }
}