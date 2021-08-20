using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControleEstoque.Web.Models;

namespace ControleEstoque.Web.Controllers
{
    public class ContaController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel login, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }




            return View(login);
        }
    }
}