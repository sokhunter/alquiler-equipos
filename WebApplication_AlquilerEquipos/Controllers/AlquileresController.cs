using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication_AlquilerEquipos.Models;

namespace WebApplication_AlquilerEquipos.Controllers
{
    [Authorize]
    public class AlquileresController : Controller
    {
        private AlquilerEntities db = new AlquilerEntities();

        // GET: Alquileres
        public ActionResult Index()
        {
            Usuario user = db.Usuario.Where(s => s.NombreUsuario == AccountController.StaticNameUser).FirstOrDefault<Usuario>();
            var alquiler = db.Alquiler.Include(a => a.Cliente).Include(a => a.Usuario).Where(s => s.Usuario_Id == user.Id);
            return View(alquiler.ToList());
        }

        public ActionResult SaveOrder(string name, DateTime fecha,  AlquilerDetalle[] order)
        {
            string result = "Error! Order Is Not Complete!";
            if (name != null && order != null)
            {
                
                Usuario user = db.Usuario.Where(s => s.NombreUsuario == AccountController.StaticNameUser).FirstOrDefault<Usuario>();

                Alquiler model = new Alquiler();
                model.Cliente_Id = Convert.ToInt32(name);
                model.Usuario_Id = user.Id;
                model.Fecha = fecha;
                model.FechaRegistro = DateTime.Now;
                foreach (var item in order)
                {
                    Equipo equipo = db.Equipo.Where(s => s.Id == item.Equipo_Id).FirstOrDefault<Equipo>();
                    model.Total += equipo.Costo;

                }
                db.Alquiler.Add(model);
                db.SaveChanges();
                int max = db.Alquiler.Max(p => p.Id);


                foreach (var item in order)
                {
                    Equipo equipo = db.Equipo.Where(s => s.Id == item.Equipo_Id).FirstOrDefault<Equipo>();

                    AlquilerDetalle alquilerDetalle = new AlquilerDetalle();
                    alquilerDetalle.Alquiler_Id = max;
                    alquilerDetalle.Equipo_Id = item.Equipo_Id;
                    alquilerDetalle.Precio = equipo.Costo;
                    db.AlquilerDetalle.Add(alquilerDetalle);
                    db.SaveChanges();
                }
                db.SaveChanges();
                result = "Success! Order Is Complete!";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }






        // GET: Alquileres/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alquiler alquiler = db.Alquiler.Find(id);
            if (alquiler == null)
            {
                return HttpNotFound();
            }
            return View(alquiler);
        }

        // GET: Alquileres/Create
        public ActionResult Create()
        {
            
            ViewBag.Cliente_Id = new SelectList(db.Cliente.Where(s => s.Eliminado == 0), "Id", "Nombre");
            ViewBag.Equipo_Id = new SelectList(db.Equipo.Where(s => s.Eliminado == 0), "Id", "Nombre");
            return View();
        }

        // POST: Alquileres/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Fecha,Total,FechaRegistro,Cliente_Id,Usuario_Id")] Alquiler alquiler)
        {
            if (ModelState.IsValid)
            {
                db.Alquiler.Add(alquiler);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Cliente_Id = new SelectList(db.Cliente, "Id", "Nombre", alquiler.Cliente_Id);
            ViewBag.Usuario_Id = new SelectList(db.Usuario, "Id", "Nombre", alquiler.Usuario_Id);
            return View(alquiler);
        }

        // GET: Alquileres/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alquiler alquiler = db.Alquiler.Find(id);
            if (alquiler == null)
            {
                return HttpNotFound();
            }
            ViewBag.Cliente_Id = new SelectList(db.Cliente, "Id", "Nombre", alquiler.Cliente_Id);
            ViewBag.Usuario_Id = new SelectList(db.Usuario, "Id", "Nombre", alquiler.Usuario_Id);
            return View(alquiler);
        }

        // POST: Alquileres/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Fecha,Total,FechaRegistro,Cliente_Id,Usuario_Id")] Alquiler alquiler)
        {
            if (ModelState.IsValid)
            {
                db.Entry(alquiler).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Cliente_Id = new SelectList(db.Cliente, "Id", "Nombre", alquiler.Cliente_Id);
            ViewBag.Usuario_Id = new SelectList(db.Usuario, "Id", "Nombre", alquiler.Usuario_Id);
            return View(alquiler);
        }

        // GET: Alquileres/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alquiler alquiler = db.Alquiler.Find(id);
            if (alquiler == null)
            {
                return HttpNotFound();
            }
            return View(alquiler);
        }

        // POST: Alquileres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Alquiler alquiler = db.Alquiler.Find(id);
            db.Alquiler.Remove(alquiler);
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
