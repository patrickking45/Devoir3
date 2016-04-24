using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Devoir3_Patrick_Dufour;
using System.IO;

namespace Devoir3_Patrick_Dufour.Controllers
{
    public class ProductsController : Controller
    {
        private NORTHWINDEntities1 db = new NORTHWINDEntities1();

        //---------------------------------------------------------------------------
        //Méthode Get de l'affichage de tous les produits
        [HttpGet]
        public ActionResult Index()
        {
            //Vérification de connexion
            if (CheckIfConnected())
            {
                var products = db.Products.Include(p => p.Categories).Include(p => p.Suppliers);

                var categories = from c in db.Categories select c.CategoryName;
                ViewBag.ListofCategories = categories.ToList();

                return View(products.ToList());
            }
            return null;
        }

        //Méthode Post de l'affichage de tous les produits
        [HttpPost, ActionName("Index")]
        public ActionResult IndexPost(string Category) {
            //Vérification de connexion
            if (CheckIfConnected())
            {
                //Filtre par catégorie
                if (Category != "")
                {
                    var products = from p in db.Products join c in db.Categories on p.CategoryID equals c.CategoryID where c.CategoryName == Category select p;

                    var categories = from c in db.Categories select c.CategoryName;
                    ViewBag.ListofCategories = categories.ToList();

                    return View(products.ToList());
                }
                //Affichage sans filtre
                else {
                    var products = db.Products.Include(p => p.Categories).Include(p => p.Suppliers);

                    var categories = from c in db.Categories select c.CategoryName;
                    ViewBag.ListofCategories = categories.ToList();

                    return View(products.ToList());
                }
            }
            return null;
        }
        //---------------------------------------------------------------------------
        //Méthode Get de l'affichage des détails d'un produit
        public ActionResult Details(int? id)
        {
            //Vérification de connexion
            if (CheckIfConnected())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Products products = db.Products.Find(id);
                if (products == null)
                {
                    return HttpNotFound();
                }
                return View(products);
            }
            return null;
        }
        //---------------------------------------------------------------------------
        //Méthode Get de création d'un produit
        public ActionResult Create()
        {
            //Vérification de connexion
            if (CheckIfConnected())
            {
                ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
                ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName");
                return View();
            }
            return null;
        }

        //Méthode Post de création d'un produit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] Products products, HttpPostedFileBase file)
        {
            //Vérification de connexion
            if (CheckIfConnected())
            {
                if (ModelState.IsValid)
                {
                    //Vérification du type de fichier
                    if (file.FileName.EndsWith(".png") || file.FileName.EndsWith(".jpg"))
                    {
                        //Sauvegarde du fichier dans le serveur
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                        file.SaveAs(path);

                        //Ajout du produit dans la DB
                        products.Picture = file.FileName;
                        db.Products.Add(products);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    //Extension du fichier image incorrect
                    else {
                        ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.CategoryID);
                        ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", products.SupplierID);
                        ViewBag.Error = "Le fichier Image doit être au format .png ou .jpg!";
                        return View(products);
                    }
                }

                ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.CategoryID);
                ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", products.SupplierID);
                return View(products);
            }
            return null;
        }
        //---------------------------------------------------------------------------
        //Méthode auto-généré Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        //---------------------------------------------------------------------------
        //Vérification que l'utilisateur est connecté
        public Boolean CheckIfConnected()
        {

            if (Session["Firstname"] == null)
            {
                Response.Redirect("/Home/Index");
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
