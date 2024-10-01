using AkınSoft_Password_Manager.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AkınSoft_Password_Manager.Controllers
{
    public class LoginController : Controller
    {
        PasswordManagerEntities db = new PasswordManagerEntities();
		// GET: Login
		public ActionResult GirisYap()
		{
			return View("GirisYap");
		}

		[HttpPost]
		public ActionResult GirisYap(Users p)
		{
			string sifre = Sifrele.MD5Olustur(p.Password);
			var bilgiler = db.Users.FirstOrDefault(x => x.Username == p.Username && x.Password == sifre);

			if (bilgiler != null)
			{
				FormsAuthentication.SetAuthCookie(bilgiler.UserID.ToString(), false);
				return RedirectToAction("Index", "Passwords");
			}
			else
			{
				TempData["LoginHata"] = "Kullanıcı adı veya Şifreyi Hatalı Girdiniz";
				return View();
			}
		}

	}
}