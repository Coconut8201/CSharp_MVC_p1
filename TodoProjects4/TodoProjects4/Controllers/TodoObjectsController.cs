using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TodoProjects4.Models;

namespace TodoProjects4.Controllers
{
    public class TodoObjectsController : Controller
    {
        private TodoModelEntities db = new TodoModelEntities();

        // GET: TodoObjects
        public ActionResult Index()
        {
            return View(db.TodoObject.ToList());
        }

        // GET: TodoObjects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TodoObject todoObject = db.TodoObject.Find(id);
            if (todoObject == null)
            {
                return HttpNotFound();
            }
            return View(todoObject);
        }

        // GET: TodoObjects/Create
        public ActionResult Create()
        {
            // System.Diagnostics.Debug.WriteLine(db.TodoObject.Count());
            return View();
        }

        // POST: TodoObjects/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TotalScore,q1,q2,q3,q4,note,deadLine,todo")] TodoObject todoObject)
        {
            int idVal = db.TodoObject.Count();
            todoObject.Id = idVal + 1;
            if (todoObject.TotalScore == null)
            {
                todoObject.TotalScore = 0;
            }

            //             System.Diagnostics.Debug.WriteLine($"db.TodoObject.Count(): {db.TodoObject.Count()}");
            //             System.Diagnostics.Debug.WriteLine($"idVal: {idVal}");
            System.Diagnostics.Debug.WriteLine($"todoObject: {todoObject}");
            if (ModelState.IsValid)
            {
                db.TodoObject.Add(todoObject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(todoObject);
        }

        // GET: TodoObjects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TodoObject todoObject = db.TodoObject.Find(id);
            if (todoObject == null)
            {
                return HttpNotFound();
            }
            return View(todoObject);
        }

        // POST: TodoObjects/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TotalScore,q1,q2,q3,q4,note,deadLine,todo")] TodoObject todoObject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(todoObject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(todoObject);
        }

        // GET: TodoObjects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TodoObject todoObject = db.TodoObject.Find(id);
            if (todoObject == null)
            {
                return HttpNotFound();
            }
            return View(todoObject);
        }

        // POST: TodoObjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TodoObject todoObject = db.TodoObject.Find(id);
            db.TodoObject.Remove(todoObject);
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

        public ActionResult Form()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Form(FormCollection formCollection, int id)
        {
            //see test 
            //foreach (string key in formCollection.AllKeys)
            //{
            //    string selectedValue = formCollection[key];
            //   System.Diagnostics.Debug.WriteLine($"{key}: {selectedValue}");
            //}

            TodoObject todoVal = db.TodoObject.Find(id);
            try
            {
                if (todoVal != null)
                {
                    todoVal.q1 = int.Parse(formCollection["emergencyLevel"]);
                    todoVal.q2 = int.Parse(formCollection["preferenceLevel"]);
                    todoVal.q3 = int.Parse(formCollection["demandLevel"]);
                    todoVal.q4 = int.Parse(formCollection["importanceLevel"]);
                    todoVal.TotalScore = (todoVal.q1 ?? 0) + (todoVal.q2 ?? 0) + (todoVal.q3 ?? 0);
                }
            }
            catch (Exception error)
            {
                return RedirectToAction("Index");
            }

            // System.Diagnostics.Debug.WriteLine($"todoVal: {todoVal}");

            if (ModelState.IsValid)
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Result()
        {
            var sortedScores = db.TodoObject.OrderByDescending(score => score.TotalScore).Take(10).ToList();
            return View(sortedScores);
        }

    }

}
