﻿using System.Net;
using System.Web.Mvc;
using NinjaDomain.Classes;
using NinjaDomain.DataModel;

namespace NinjaMVC.Controllers
{
    public class NinjasController : Controller
    {
        private readonly DisconnectedRepository _repo = new DisconnectedRepository();

        private NinjaContext db = new NinjaContext();

        // GET: Ninjas
        public ActionResult Index()
        {
            var ninjas = _repo.GetNinjasWithClan();
            return View(ninjas);
        }

        // GET: Ninjas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ninja ninja = _repo.GetNinjaWithEquipmentAndClan(id.Value);
            if (ninja == null)
            {
                return HttpNotFound();
            }
            return View(ninja);
        }

        // GET: Ninjas/Create
        public ActionResult Create()
        {
            ViewBag.ClanId = new SelectList(_repo.GetClanList(), "Id", "ClanName");
            return View();
        }

        // POST: Ninjas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ServedInOniwaban,ClanId,DateOfBirth,DateModified,DateCreated")] Ninja ninja)
        {
            if (ModelState.IsValid)
            {
                _repo.SaveNewNinja(ninja);
                return RedirectToAction("Index");
            }

            ViewBag.ClanId = new SelectList(_repo.GetClanList(), "Id", "ClanName", ninja.ClanId);
            return View(ninja);
        }

        // GET: Ninjas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ninja ninja = _repo.GetNinjaById(id.Value);
            if (ninja == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClanId = new SelectList(_repo.GetClanList(), "Id", "ClanName", ninja.ClanId);
            return View(ninja);
        }

        // POST: Ninjas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ServedInOniwaban,ClanId,DateOfBirth,DateModified,DateCreated")] Ninja ninja)
        {
            if (ModelState.IsValid)
            {
                _repo.SaveUpdatedNinja(ninja);
                return RedirectToAction("Index");
            }
            ViewBag.ClanId = new SelectList(_repo.GetClanList(), "Id", "ClanName", ninja.ClanId);
            return View(ninja);
        }

        // GET: Ninjas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ninja ninja = _repo.GetNinjaById(id.Value);
            if (ninja == null)
            {
                return HttpNotFound();
            }
            return View(ninja);
        }

        // POST: Ninjas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _repo.DeleteNinja(id);
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
