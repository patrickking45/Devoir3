using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Devoir3_Patrick_Dufour.Controllers
{
    public class HomeController : Controller
    {
        //---------------------------------------------------------------------------
        //Méthode Get de la page Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        //Méthode Post de la page Login
        [HttpPost, ActionName("Index")]
        public ActionResult IndexPost(string Identifiant, string Password)
        {
            //Vérification des informations de connexion
            if (connexion(Identifiant, Password))
            {
                Response.Redirect("/Products/Index");
            }
            //Info invalides
            else {
                ViewBag.Error = "Login ou Password invalide!";
            }
            return View();
        }
        //---------------------------------------------------------------------------
        //Méthode de vérification du login et password
        private Boolean connexion(string login, string password) {
            Boolean accepted = false;

            using(NORTHWINDEntities1 context = new NORTHWINDEntities1()){
                var result = from u in context.Users where u.Login == login && u.Password == password select u;
                var user = result.FirstOrDefault();

                //Création d'une session si info correct
                if (user != null)
                {
                    accepted = true;
                    Session["Firstname"] = user.Firstname;
                    Session["Lastname"] = user.Lastname;
                }
            }
            return accepted;
        }
        //---------------------------------------------------------------------------
        //Méthode de deconnexion
        public void Deconnection() {
            //Suppression de la session
            Session.Abandon();
            Response.Redirect("/Home/Index");

        }
        //---------------------------------------------------------------------------
    }
}