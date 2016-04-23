using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Devoir3_Patrick_Dufour.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, ActionName("Index")]
        public ActionResult IndexPost(string Identifiant, string Password)
        {
            if (connexion(Identifiant, Password)) {
                Response.Redirect("/Products/Index");
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private Boolean connexion(string login, string password) {
            Boolean accepted = false;

            using(NORTHWINDEntities1 context = new NORTHWINDEntities1()){
                var result = from u in context.Users where u.Login == login && u.Password == password select u;
                var user = result.FirstOrDefault();

                if (user != null)
                {
                    accepted = true;
                    Session["Firstname"] = user.Firstname;
                    Session["Lastname"] = user.Lastname;
                    Session["Connected"] = true;

                }
            }
            return accepted;
        }
    }
}