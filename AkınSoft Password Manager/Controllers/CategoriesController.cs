using AkınSoft_Password_Manager.Models.Class;
using AkınSoft_Password_Manager.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AkınSoft_Password_Manager.Controllers
{
	public class CategoriesController : Controller
	{
		PasswordManagerEntities db = new PasswordManagerEntities();
		// GET: Categories
		[Authorize]
		public ActionResult Index()
		{
			int userID = Convert.ToInt32(HttpContext.User.Identity.Name);

			ViewBag.dgr1 = "Categories";
			ViewBag.dgr2 = "Categories List";
			var user = db.Users.Find(userID);
			ViewBag.user = user.Name + " " + user.Surname;
			Class1 cs = new Class1();
			cs.Categories = db.Categories.Where(x => x.UserID == userID).ToList();
			return View(cs);
		}

		[HttpGet]
		public ActionResult AddCategories()
		{
			int userID = Convert.ToInt32(HttpContext.User.Identity.Name);
			ViewBag.dgr1 = "Categories";
			ViewBag.dgr2 = "Add Categories";
			var user = db.Users.Find(userID);
			ViewBag.user = user.Name + " " + user.Surname;
			return View();
		}

		[HttpPost]
		public ActionResult AddCategories(Categories p)
		{
			int userID = Convert.ToInt32(HttpContext.User.Identity.Name);
			p.UserID = userID;
			db.Categories.Add(p);
			db.SaveChanges();
			TempData["AddPasswords"] = "Yeni Kategori Eklendi.";
			return RedirectToAction("Index");
		}

		//public ActionResult DeleteCategories(int id)
		//{
		//	int userID = Convert.ToInt32(HttpContext.User.Identity.Name);

		//	List<int> restrictedIds = new List<int> { 1, 2, 3 };

		//	if (restrictedIds.Contains(id))
		//	{
		//		TempData["DeleteError"] = "Ana kategoriler silinemez.";
		//		return RedirectToAction("Index");
		//	}

		//	var category = db.Categories.Find(id);

		//	if (category == null)
		//	{
		//		TempData["NotFoundCategory"] = "Kategori bulunamadı.";
		//		return RedirectToAction("Index");
		//	}
		//	var usersCategories = db.Categories.Where(x => x.UserID == userID && x.CategoryID == id).FirstOrDefault();

		//	if (usersCategories == null)
		//	{
		//		TempData["NotAccess"] = "Kategori Size Ait Değil.";
		//		return RedirectToAction("Index");
		//	}

		//	db.Categories.Remove(usersCategories);
		//	db.SaveChanges();

		//	TempData["DeleteCategories"] = "Kategori Listeden Silindi.";
		//	return RedirectToAction("Index");


		//}

		public ActionResult GetCategories(int id)
		{
			int userID = Convert.ToInt32(HttpContext.User.Identity.Name);
			var category = db.Categories.Find(id);

			if (category == null)
			{
				TempData["NotFoundCategory"] = "Kategori bulunamadı.";
				return RedirectToAction("Index");
			}

			if (category.UserID != userID)
			{
				TempData["NotAccess"] = "Kategori Size Ait Değil.";
				return RedirectToAction("Index");
			}

			ViewBag.dgr1 = "Categories";
			ViewBag.dgr2 = "Update Categories";
			var user = db.Users.Find(userID);
			ViewBag.user = user.Name + " " + user.Surname;
			return View("GetCategories", category);
		}

		public ActionResult UpdateCategories(Categories p)
		{
			int userID = Convert.ToInt32(HttpContext.User.Identity.Name);
			var updatecategories = db.Categories.Find(p.CategoryID);

			if (updatecategories == null)
			{
				TempData["NotFoundCategory"] = "Kategori bulunamadı.";
				return RedirectToAction("Index");
			}

			if (updatecategories.UserID != userID)
			{
				TempData["NotAccess"] = "Kategori Size Ait Değil.";
				return RedirectToAction("Index");
			}

			updatecategories.Title = p.Title;
			db.SaveChanges();
			TempData["UpdateCategories"] = "Kategori Başarıyla Güncellendi.";
			return RedirectToAction("Index");
		}

	}
}