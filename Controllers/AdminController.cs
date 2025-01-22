using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Sell_Your_Books.Models;

namespace Sell_Your_Books.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/UserList
        public ActionResult UserList()
        {
            return View(db.Users.ToList());
        }

        // GET: Admin/AddManager
        public ActionResult AddManager()
        {
            return View(db.Users.ToList());
        }

        // GET: Admin/MakeManager/5
        public ActionResult MakeManager()
        {
            var id = Request.Url.Segments[3];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            try
            {
                var user = UserManager.FindById(id.ToString());
                UserManager.AddToRole(user.Id, "Manager");
                db.SaveChanges();
            }
            catch
            {
                throw;
            }

            return Redirect("~/Admin");
        }

        // GET: Admin/BookList
        public ActionResult BookList()
        {
            return View(db.BookInfoes.ToList());
        }

        // GET: Admin/UserDetails/5
        public ActionResult UserDetails(string? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Where(b => b.Id == id).FirstOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Admin/BookDetails/5
        public ActionResult BookDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookInfo bookInfo = db.BookInfoes.Find(id);
            if (bookInfo == null)
            {
                return HttpNotFound();
            }
            return View(bookInfo);
        }

        // GET: Admin/UserDelete/5
        public ActionResult UserDelete(string? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Where(b => b.Id == id).FirstOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/UserDelete/5
        [HttpPost, ActionName("UserDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser user = db.Users.Where(b => b.Id == id).FirstOrDefault();
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("UserList");
        }

        // GET: Admin/BookDelete/5
        public ActionResult BookDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookInfo bookInfo = db.BookInfoes.Find(id);
            if (bookInfo == null)
            {
                return HttpNotFound();
            }
            return View(bookInfo);
        }

        //POST: Admin/BookDelete/5
        [HttpPost, ActionName("BookDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed1(int id)
        {
            BookInfo bookInfo = db.BookInfoes.Find(id);
            string filePath = Server.MapPath("/") + "/Content/" + bookInfo.img;
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            db.BookInfoes.Remove(bookInfo);
            db.SaveChanges();
            return RedirectToAction("BookList");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
