using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Sell_Your_Books.Models;

namespace Sell_Your_Books.Controllers
{
    [Authorize]
    public class BookInfoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BookInfoes
        public ActionResult Index()
        {
            string username = User.Identity.GetUserName();
            ViewBag.user = db.Users.Where(b => b.UserName == username).FirstOrDefault().Name;
            return View(db.BookInfoes.Where(b => b.User.UserName == username).ToList<BookInfo>());
            //return View(db.BookInfoes.ToList());
        }

        // GET: BookInfoes/Details/5
        public ActionResult Details(int? id)
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

        // GET: BookInfoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BookInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookId,img,Title,Author,Price,isNew,Condition")] BookInfo bookInfo, HttpPostedFileBase uploadPic)
        {
            if (ModelState.IsValid)
            {  
                if (uploadPic != null)
                {
                    if (uploadPic.ContentType == "image/jpg" || uploadPic.ContentType == "image/jpeg")
                    {
                        uploadPic.SaveAs(Server.MapPath("/") + "/Content/" + uploadPic.FileName);
                        bookInfo.img = uploadPic.FileName;
                    }
                }
                else
                {
                    return View(bookInfo);
                }
                bookInfo.userId = User.Identity.GetUserId().ToString();
                bookInfo.User = db.Users.Find(User.Identity.GetUserId());
                if(bookInfo.Condition == null)
                {
                    if (bookInfo.isNew.Equals("New"))
                        bookInfo.Condition = "All New Book";
                    else
                        bookInfo.Condition = "Old Book";
                }
                db.BookInfoes.Add(bookInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bookInfo);
        }

        // GET: BookInfoes/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: BookInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookId,img,Title,Author,Price,isNew,Condition")] BookInfo bookInfo)
        {
            if (ModelState.IsValid)
            {
                bookInfo.userId = User.Identity.GetUserId().ToString();
                bookInfo.User = db.Users.Find(User.Identity.GetUserId());
                db.Entry(bookInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bookInfo);
        }

        // GET: BookInfoes/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: BookInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookInfo bookInfo = db.BookInfoes.Find(id);
            string filePath = Server.MapPath("/") + "/Content/" + bookInfo.img;
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            db.BookInfoes.Remove(bookInfo);
            db.SaveChanges();
            return RedirectToAction("Index");
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
