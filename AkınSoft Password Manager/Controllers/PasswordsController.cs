using AkınSoft_Password_Manager.Models.Class;
using AkınSoft_Password_Manager.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AkınSoft_Password_Manager.Controllers
{
	public class PasswordsController : Controller
	{
		PasswordManagerEntities db = new PasswordManagerEntities();
		// GET: Passwords
		[Authorize]
		public ActionResult Index(Users p)
		{
			// Giriş yapan kullanıcının UserID'sini al
			int userID = Convert.ToInt32(HttpContext.User.Identity.Name);


			var sm = db.Categories.Find(1);
			ViewBag.d1 = sm.Title;

			var av = db.Categories.Find(2);
			ViewBag.d2 = av.Title;

			var user = db.Users.Find(userID);
			ViewBag.user= user.Name +" "+ user.Surname;

			ViewBag.dgr1 = "Password";
			ViewBag.dgr2 = "Password List";
			Class1 cs = new Class1();
			var totalpass = db.Passwords.Where(x => x.UserID == userID).Count();
			ViewBag.totalpass = totalpass;
			var totalcategories = db.Categories.Where(x => x.UserID == userID).Count();
			ViewBag.totalcategories = totalcategories;
			cs.Passwords = db.Passwords.Where(x => x.UserID == userID).ToList();
			return View(cs);
		}
		[HttpGet]
		public ActionResult AddPasswords()
		{
			int userID = Convert.ToInt32(HttpContext.User.Identity.Name);

			ViewBag.dgr1 = "Password";
			ViewBag.dgr2 = "Password Add";
			var user = db.Users.Find(userID);
			ViewBag.user = user.Name + " " + user.Surname;

			// Kullanıcının kendi UserID'sine ait kategorileri al
			List<SelectListItem> categories = db.Categories
				.Where(c => c.UserID == userID)
				.Select(c => new SelectListItem
				{
					Text = c.Title,
					Value = c.CategoryID.ToString()
				})
				.ToList();

			ViewBag.Categories = categories;

			return View();
		}
		[HttpPost]
		public ActionResult AddPasswords(Passwords p, HttpPostedFileBase RESIM)
		{
			int userID;

			// HttpContext.User.Identity.Name boş değilse ve sayıya dönüştürülebiliyorsa userID'yi al
			if (!string.IsNullOrEmpty(HttpContext.User.Identity.Name) && int.TryParse(HttpContext.User.Identity.Name, out userID))
			{
				if (p.CategoryID == 0 || string.IsNullOrWhiteSpace(p.Title) ||
					string.IsNullOrWhiteSpace(p.URL) || string.IsNullOrWhiteSpace(p.Username) ||
					string.IsNullOrWhiteSpace(p.Password))
				{
					// Herhangi bir alan boşsa hata mesajı göster
					TempData["Error"] = "Lütfen tüm alanları doldurun.";
					return RedirectToAction("Index");
				}

				if (RESIM != null && RESIM.ContentLength > 0)
				{
					var fileName = Path.GetFileName(RESIM.FileName);
					var fileExtension = Path.GetExtension(RESIM.FileName);
					var filePath = Path.Combine(Server.MapPath("~/Themes/Image"), fileName + fileExtension);
					RESIM.SaveAs(filePath);
					p.ImageUrl = "/Image/" + fileName + fileExtension;
				}
				else
				{
					// RESIM boşsa veya içeriği yoksa default.png'yi atayın
					p.ImageUrl = "/Image/default.png";
				}

				var addpass = db.Categories.FirstOrDefault(k => k.CategoryID == p.CategoryID);

				if (addpass == null)
				{
					// Geçerli bir Category seçilmemişse hata mesajı göster
					TempData["Error"] = "Geçerli bir Kategori seçin.";
					return RedirectToAction("Index");
				}

				p.Categories = addpass;
				p.UserID = userID;

				db.Passwords.Add(p);
				db.SaveChanges();
				TempData["AddPasswords"] = "Yeni Şifre Eklendi.";
				return RedirectToAction("Index");
			}
			else
			{
				// Hata durumu, kullanıcı ID'si alınamadı.
				TempData["Error"] = "Kullanıcı ID'si alınamadı.";
				return RedirectToAction("Index");
			}
		}




		//[HttpPost]
		//public ActionResult AddPasswords(Passwords p, HttpPostedFileBase RESIM)
		//{
		//	int userID;

		//	// HttpContext.User.Identity.Name boş değilse ve sayıya dönüştürülebiliyorsa userID'yi al
		//	if (!string.IsNullOrEmpty(HttpContext.User.Identity.Name) && int.TryParse(HttpContext.User.Identity.Name, out userID))
		//	{
		//		if (RESIM != null && RESIM.ContentLength > 0)
		//		{
		//			var fileName = Path.GetFileName(RESIM.FileName);
		//			var fileExtension = Path.GetExtension(RESIM.FileName);
		//			var filePath = Path.Combine(Server.MapPath("~/Themes/Image"), fileName + fileExtension);
		//			RESIM.SaveAs(filePath);
		//			p.ImageUrl = "/Image/" + fileName + fileExtension;
		//		}
		//		else
		//		{
		//			// RESIM boşsa veya içeriği yoksa default.png'yi atayın
		//			p.ImageUrl = "Image/default.png";
		//		}

		//		var addpass = (p.Categories != null)
		//						? db.Categories.FirstOrDefault(k => k.CategoryID == p.Categories.CategoryID)
		//						: null;

		//		p.Categories = addpass;
		//		p.UserID = userID;

		//		db.Passwords.Add(p);
		//		db.SaveChanges();
		//		TempData["AddPasswords"] = "Yeni Şifre Eklendi.";
		//		return RedirectToAction("Index");
		//	}
		//	else
		//	{
		//		// Hata durumu, kullanıcı ID'si alınamadı.
		//		TempData["Error"] = "Kullanıcı ID'si alınamadı.";
		//		return RedirectToAction("Index");
		//	}
		//}


		//[HttpPost]
		//public ActionResult AddPasswords(Passwords p, HttpPostedFileBase RESIM)
		//{
		//	int userID;

		//	// HttpContext.User.Identity.Name boş değilse ve sayıya dönüştürülebiliyorsa userID'yi al
		//	if (!string.IsNullOrEmpty(HttpContext.User.Identity.Name) && int.TryParse(HttpContext.User.Identity.Name, out userID))
		//	{
		//		if (Request.Files.Count > 0)
		//		{
		//			var fileName = Path.GetFileName(RESIM.FileName);
		//			var fileExtension = Path.GetExtension(RESIM.FileName);
		//			var filePath = Path.Combine(Server.MapPath("~/Themes/Image"), fileName + fileExtension);
		//			RESIM.SaveAs(filePath);
		//			p.ImageUrl = "/Image/" + fileName + fileExtension;
		//		}

		//		var addpass = (p.Categories != null) 
		//  ? db.Categories.FirstOrDefault(k => k.CategoryID == p.Categories.CategoryID) 
		//  : null;


		//		p.Categories = addpass;
		//		p.UserID = userID;

		//		db.Passwords.Add(p);
		//		db.SaveChanges();
		//		TempData["AddPasswords"] = "Yeni Şifre Eklendi.";
		//		return RedirectToAction("Index");
		//	}
		//	else
		//	{
		//		// Hata durumu, kullanıcı ID'si alınamadı.
		//		TempData["Error"] = "Kullanıcı ID'si alınamadı.";
		//		return RedirectToAction("Index");
		//	}
		//}


		public ActionResult DeletePasswords(int id)
		{
			int userID = Convert.ToInt32(HttpContext.User.Identity.Name);

			var password = db.Passwords.Find(id);

			if (password == null)
			{
				TempData["NotFoundPassword"] = "Şifre bulunamadı.";
				return RedirectToAction("Index");
			}

			var userPassword = db.Passwords.FirstOrDefault(x => x.UserID == userID && x.PasswordID == id);

			if (userPassword == null)
			{
				TempData["NotAccess"] = "Şifre Size Ait Değil.";
				return RedirectToAction("Index");
			}

			db.Passwords.Remove(userPassword);
			db.SaveChanges();

			TempData["DeletePasswords"] = "Şifre Listeden Silindi.";
			return RedirectToAction("Index");
		}


		public ActionResult GetPassword(int id)
		{
			int userID = Convert.ToInt32(HttpContext.User.Identity.Name);

			// Şifreyi bul
			var password = db.Passwords.Find(id);

			if (password == null)
			{
				TempData["NotFoundPassword"] = "Şifre bulunamadı.";
				return RedirectToAction("Index");
			}

			// Şifrenin sahibi kullanıcı mı kontrol et
			if (password.UserID != userID)
			{
				TempData["NotAccess"] = "Şifre Size Ait Değil.";
				return RedirectToAction("Index");
			}


			// Kullanıcının kendi UserID'sine ait kategorileri al
			List<SelectListItem> categories = db.Categories
				.Where(c => c.UserID == userID)
				.Select(c => new SelectListItem
				{
					Text = c.Title,
					Value = c.CategoryID.ToString()
				})
				.ToList();

			ViewBag.Categories = categories;


			// View'e gerekli bilgileri gönder
			ViewBag.dgr1 = "Password";
			ViewBag.dgr2 = "Password Update";
			var user = db.Users.Find(userID);
			ViewBag.user = user.Name + " " + user.Surname;
			return View("GetPassword", password);
		}

		public ActionResult UpdatePasswords(Passwords p, HttpPostedFileBase RESIM)
		{
			// Kullanıcı kimliğini al
			int userID = Convert.ToInt32(HttpContext.User.Identity.Name);

			// Passwords nesnesini veritabanından bul
			var passwords = db.Passwords.Find(p.PasswordID);

			// Eğer bulunamazsa HttpNotFound() döndür
			if (passwords == null)
			{
				return HttpNotFound();
			}

			// Eğer yeni bir resim yüklenmişse
			if (RESIM != null && RESIM.ContentLength > 0)
			{
				// Yeni resmi kaydet
				var fileName = Path.GetFileName(RESIM.FileName);
				var fileExtension = Path.GetExtension(RESIM.FileName);
				var filePath = Path.Combine(Server.MapPath("~/Themes/Image"), fileName + fileExtension);
				RESIM.SaveAs(filePath);
				passwords.ImageUrl = "/Image/" + fileName + fileExtension;
			}

			// Passwords nesnesinin özelliklerini güncelle
			passwords.Title = p.Title;
			passwords.Username = p.Username;
			passwords.Password = p.Password;

			// Kategoriyi veritabanından bul
			var categories = db.Categories.Find(p.CategoryID);

			// Eğer bulunamazsa HttpNotFound() döndür
			if (categories == null)
			{
				return HttpNotFound();
			}

			// Passwords nesnesinin CategoryID özelliğini güncelle
			passwords.CategoryID = categories.CategoryID;

			try
			{
				// Veritabanını güncelle
				db.SaveChanges();
			}
			catch (DbEntityValidationException ex)
			{
				// Hata oluştuysa hatayı göster
				var errorMessages = ex.EntityValidationErrors
						.SelectMany(x => x.ValidationErrors)
						.Select(x => x.ErrorMessage);
				var fullErrorMessage = string.Join("; ", errorMessages);
				var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
				throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
			}

			TempData["PasswordUpdate"] = "Şifre bilgilerin başarıyla güncellendi.";
			return RedirectToAction("Index");
		}


		//public ActionResult UpdatePasswords(Passwords p, HttpPostedFileBase RESIM)
		//{
		//	// Kullanıcı kimliğini al
		//	int userID = Convert.ToInt32(HttpContext.User.Identity.Name);

		//	// Passwords nesnesini veritabanından bul
		//	var passwords = db.Passwords.Find(p.PasswordID);

		//	// Eğer bulunamazsa HttpNotFound() döndür
		//	if (passwords == null)
		//	{
		//		return HttpNotFound();
		//	}

		//	// Passwords nesnesinin özelliklerini güncelle
		//	passwords.Title = p.Title;
		//	passwords.Username = p.Username;
		//	passwords.Password = p.Password;

		//	// Kategoriyi veritabanından bul
		//	var categories = db.Categories.Find(p.CategoryID);

		//	// Eğer bulunamazsa HttpNotFound() döndür
		//	if (categories == null)
		//	{
		//		return HttpNotFound();
		//	}

		//	// Passwords nesnesinin CategoryID özelliğini güncelle
		//	passwords.CategoryID = categories.CategoryID;

		//	try
		//	{
		//		// Veritabanını güncelle
		//		db.SaveChanges();
		//	}
		//	catch (DbEntityValidationException ex)
		//	{
		//		// Hata oluştuysa hatayı göster
		//		var errorMessages = ex.EntityValidationErrors
		//				.SelectMany(x => x.ValidationErrors)
		//				.Select(x => x.ErrorMessage);
		//		var fullErrorMessage = string.Join("; ", errorMessages);
		//		var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
		//		throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
		//	}

		//	TempData["PasswordUpdate"] = "Şifre bilgilerin başarıyla güncellendi.";
		//	return RedirectToAction("Index");
		//}


		//public ActionResult UpdatePasswords(Passwords p, HttpPostedFileBase RESIM)
		//{
		//	int userID = Convert.ToInt32(HttpContext.User.Identity.Name);

		//	var passwords = db.Passwords.Find(p.PasswordID);
		//	if(passwords == null)
		//	{
		//		return HttpNotFound();
		//	}

		//	passwords.Title = p.Title;
		//	passwords.Username = p.Username;
		//	passwords.Password = p.Password;;
		//	var categories = db.Categories.Find(p.Categories.CategoryID);

		//	if(categories == null)
		//	{
		//		return HttpNotFound();
		//	}
		//	passwords.CategoryID = categories.CategoryID;


		//	try
		//	{
		//		// Veritabanını güncelle
		//		db.SaveChanges();
		//	}
		//	catch (DbEntityValidationException ex)
		//	{
		//		// Hata oluştuysa hatayı göster
		//		var errorMessages = ex.EntityValidationErrors
		//				.SelectMany(x => x.ValidationErrors)
		//				.Select(x => x.ErrorMessage);
		//		var fullErrorMessage = string.Join("; ", errorMessages);
		//		var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
		//		throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
		//	}

		//	TempData["PasswordUpdate"] = "Şifre bilgilerin başarıyla güncellendi.";
		//	return RedirectToAction("Index");
		//}


		[HttpGet]
		public ActionResult AddCategoriesForNewPasswords()
		{
			ViewBag.dgr1 = "Categories";
			ViewBag.dgr2 = "Add Categories";
			return View();
		}

		[HttpPost]
		public ActionResult AddCategoriesForNewPasswords(Categories p)
		{
			int userID = Convert.ToInt32(HttpContext.User.Identity.Name);
			p.UserID = userID;
			db.Categories.Add(p);
			db.SaveChanges();
			TempData["AddPasswords"] = "Yeni Kategori Eklendi.";
			return RedirectToAction("AddPasswords", "Passwords");
		}


		[HttpGet]
		public ActionResult AddCategoriesForUpdatePasswords()
		{
			ViewBag.dgr1 = "Categories";
			ViewBag.dgr2 = "Add Categories";
			return View();
		}

		[HttpPost]
		public ActionResult AddCategoriesForUpdatePasswords(Categories p)
		{
			int userID = Convert.ToInt32(HttpContext.User.Identity.Name);
			p.UserID = userID;
			db.Categories.Add(p);
			db.SaveChanges();
			TempData["AddPasswords"] = "Yeni Kategori Eklendi.";
			return RedirectToAction("GetPassword", "Passwords");
		}


		public ActionResult LogOut()
		{
			FormsAuthentication.SignOut();
			return RedirectToAction("GirisYap", "Login");
		}
	}
}