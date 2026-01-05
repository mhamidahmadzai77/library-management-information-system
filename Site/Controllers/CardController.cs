using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Site.Models;

namespace Site.Controllers
{
    public class CardController : Controller
    {
        LibraryMISEntities db = new LibraryMISEntities();
        // GET: Card
        public ActionResult CardView(int id)
        {
            var person = db.People.Find(id);
            return View(person);
        }
    }
}