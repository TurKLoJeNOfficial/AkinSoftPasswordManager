﻿using System.Web;
using System.Web.Mvc;

namespace AkınSoft_Password_Manager
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}
