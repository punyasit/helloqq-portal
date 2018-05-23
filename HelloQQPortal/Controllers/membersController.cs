using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using HelloQQPortal.Database;
using HelloQQPortal.Manager;

namespace HelloQQPortal.Controllers
{
    public class membersController : Controller
    {        
        private helloqqdbEntities dbInfo = new helloqqdbEntities(); 

        // GET: members
        public ActionResult Index()
        {
            return View(dbInfo.hqq_member.ToList());
        }

        // GET: members/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hqq_member member = dbInfo.hqq_member.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // GET: members/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,facebook_id,fullname,location_code,hometown_code,created_on,created_by,status")] hqq_member member)
        {
            if (ModelState.IsValid)
            {
                dbInfo.hqq_member.Add(member);
                dbInfo.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(member);
        }

        // GET: members/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hqq_member member = dbInfo.hqq_member.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,facebook_id,fullname,location_code,hometown_code,created_on,created_by,status")] hqq_member member)
        {
            if (ModelState.IsValid)
            {
                dbInfo.Entry(member).State = EntityState.Modified;
                dbInfo.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }

        // GET: members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hqq_member member = dbInfo.hqq_member.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            hqq_member member = dbInfo.hqq_member.Find(id);
            dbInfo.hqq_member.Remove(member);
            dbInfo.SaveChanges();
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbInfo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
