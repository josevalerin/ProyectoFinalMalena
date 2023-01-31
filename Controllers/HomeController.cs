using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Proyecto_Tienda_Malena.Models;

namespace TiendaVideojuegos.Controllers
{
    public class HomeController : Controller
    {
        private Proyecto_Tienda_MalenaEntities db = new Proyecto_Tienda_MalenaEntities();

        public ActionResult Index()
        {
            var productos = db.Productos.Include(p => p.Categorias).Include(p => p.Marcas);
            return View(productos.ToList());
        }


        public ActionResult InicioSesion()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InicioSesion(Usuarios user)

        {

            if (ModelState.IsValid)
            {
                bool IsValidUser = db.Usuarios
               .Any(u => u.Correo.ToLower() == user
               .Correo.ToLower() &&
                u.Clave == user.Clave);

                var usuario = db.Usuarios.Where(u => u.Correo.ToLower() == user
               .Correo.ToLower()).ToList<Usuarios>();

                if (IsValidUser)
                {
                    FormsAuthentication.SetAuthCookie(user.Correo, false);

                    foreach (var x in usuario)
                    {
                        if (x.ROLES.ROL == "Admin")
                        {

                            return RedirectToAction("Admin", "Admin");
                        }
                        else
                        {

                            return RedirectToAction("Index");

                        }

                    }
                }
                else
                {
                    ViewBag.ErrorCredenciales = "Contraseña y correo incorrectos";
                }
            }

            ModelState.AddModelError("", "Credenciales Incorrectas");
            return View();

        }

        public ActionResult Registro()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registro(Usuarios registerUser)
        {
            if (ModelState.IsValid)
            {
                if (noExisteUsuario(registerUser))
                {
                    registerUser.ID_ROL = 2;
                    db.Usuarios.Add(registerUser);
                    db.SaveChanges();
                    return RedirectToAction("InicioSesion");
                }
                else
                {
                    ViewBag.ErrorRegistro = "Correo y Cedula ya estan en uso";
                }
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            return View();
        }

        public bool noExisteUsuario(Usuarios user)
        {
            if (db.Usuarios.Where(u => u.Correo == user.Correo
            && u.Cedula == user.Cedula).Count() == 0)
            {
                return true;
            }
            return false;
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productos productos = db.Productos.Find(id);
            if (productos == null)
            {
                return HttpNotFound();
            }
            return View(productos);
        }

        public ActionResult Consolas()
        {
            var consolas = db.Productos.Where(x => x.ID_Categoria == 2);
            return View(consolas.ToList());
        }

        public ActionResult Accesorios()
        {
            var accesorios = db.Productos.Where(x => x.ID_Categoria == 3);
            return View(accesorios.ToList());
        }

        public ActionResult Videojuegos()
        {
            var videojuegos = db.Productos.Where(x => x.ID_Categoria== 1);
            return View(videojuegos);
        }
    }
}