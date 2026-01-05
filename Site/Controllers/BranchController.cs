using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Site.Models;

namespace Site.Controllers
{

    public class BranchController : Controller
    {
        // GET: Branch
        LibraryMISEntities db = new LibraryMISEntities();
        public ActionResult BranchRegistrationView()
        {
            var data = db.Branches.OrderByDescending(b => b.branch_id);
            return View(data);

        }

        [HttpPost]
        public JsonResult BranchRegistration(Branch branch)
        {
            try
            {

                bool valueExists = db.Branches.Any(x => x.branch_name == branch.branch_name);
                if (valueExists)
                {
                    return Json("Record already Exist", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Branches.Add(branch);
                    db.SaveChanges();
                    return Json(new { branch_id = branch.branch_id, branch_name = branch.branch_name }, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception ex)
            {

                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult BranchDelete(int id)
        {
            try
            {
                var data = db.Branches.Find(id);
                db.Branches.Remove(data);
                int count = db.SaveChanges();
                if (count > 0)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult BranchUpdate(Branch branch)
        {
            try
            {
                Branch obj = db.Branches.Where(b => b.branch_name == branch.branch_name).FirstOrDefault();
                if (obj == null)
                {
                    Branch br = db.Branches.Find(branch.branch_id);
                    br.branch_name = branch.branch_name;
                    db.SaveChanges();
                    return Json(branch.branch_name.ToString(), JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);

            }
            /*
                        obj.branch_name = branch.branch_name;
                        if (changed > 0)

                        {

                        }
                        else
                        {
                        }

            */



        }
    }

}