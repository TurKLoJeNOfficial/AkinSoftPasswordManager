using AkınSoft_Password_Manager.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AkınSoft_Password_Manager.Models.Class
{
	public class Class1
	{
		public IEnumerable<Passwords> Passwords { get; set; }
		public IEnumerable<Categories> Categories { get; set; }
		public IEnumerable<Users> Users { get; set; }

	}
}