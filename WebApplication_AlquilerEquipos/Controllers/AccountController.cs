using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication_AlquilerEquipos.Models;

namespace WebApplication_AlquilerEquipos.Controllers
{
    public class AccountController : Controller
    {
        private AlquilerEntities db = new AlquilerEntities();
        public static string StaticNameUser { get; set; }
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult ProfileUser()
        {
            Usuario user = db.Usuario.Where(s => s.NombreUsuario == StaticNameUser).FirstOrDefault<Usuario>();
            return RedirectToAction("Details", "Usuarios", new { id = user.Id });
        }

        [HttpPost]
        public ActionResult Login(UserLogin userLogin)
        {

            bool isValid = db.Usuario.Any(x => x.NombreUsuario == userLogin.NameUser && x.Clave == userLogin.Password);

            if (isValid)
            {
                Usuario user = db.Usuario.Where(s => s.NombreUsuario == userLogin.NameUser).FirstOrDefault<Usuario>();
                FormsAuthentication.SetAuthCookie(user.Nombre, false);
                 StaticNameUser = userLogin.NameUser;
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Usuario y/o Contraseña Invalido");
            return View();
        }


        public ActionResult SignUp()
        {
            ViewBag.Cargo_Id = new SelectList(db.Cargo, "Id", "Nombre");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp([Bind(Include = "Id,Nombre,NombreUsuario,Clave,Cargo_Id")] Usuario usuario)
        {
            usuario.Cargo_Id = 1;

            if (ModelState.IsValid)
            {
                db.Usuario.Add(usuario);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            ViewBag.Cargo_Id = new SelectList(db.Cargo, "Id", "Nombre", usuario.Cargo_Id);
            return View(usuario);
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }
}