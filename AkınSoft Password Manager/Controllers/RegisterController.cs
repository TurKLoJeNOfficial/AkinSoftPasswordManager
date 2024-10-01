using AkınSoft_Password_Manager.Models.Entity;
using System;
using System.Linq;
using System.Web.Mvc;

namespace AkınSoft_Password_Manager.Controllers
{
	public class RegisterController : Controller
	{
		PasswordManagerEntities db = new PasswordManagerEntities();

		// GET: Register
		public ActionResult KayitOl()
		{
			return View("KayitOl");
		}

		[HttpPost]
		public ActionResult KayitOl(Users newUser)
		{
			// Kullanıcı adı ve e-posta kontrolü
			if (IsUsernameOrEmailExists(newUser.Username, newUser.Mail))
			{
				TempData["RegisterError"] = "Bu kullanıcı adı veya e-posta adresi zaten kullanılıyor.";
				return View();
			}

			// Şifreleme işlemi, örneğin MD5
			newUser.Password = Sifrele.MD5Olustur(newUser.Password);

			// Yeni kullanıcıyı veritabanına ekleyin
			newUser.ImageUrl = "/Theme/Image/logo.png";
			db.Users.Add(newUser);
			db.SaveChanges();

			// Giriş sayfasına yönlendirme
			return RedirectToAction("GirisYap", "Login");
		}

		// Kullanıcı adı veya e-posta adresi zaten var mı kontrolü
		private bool IsUsernameOrEmailExists(string username, string email)
		{
			return db.Users.Any(x => x.Username == username || x.Mail == email);
		}
	}
}
