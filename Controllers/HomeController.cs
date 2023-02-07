using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
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
            ViewBag.ID_Categoria = new SelectList(db.Categorias, "ID_Categoria", "Nombre_Categoria");
            ViewBag.ID_Marca = new SelectList(db.Marcas, "ID_Marca", "Nombre_marca");
            ViewBag.ID_Genero = new SelectList(db.Genero, "ID_Genero", "Nombre_Genero");

            return View(productos.ToList());
        }


        [HttpPost]
        public ActionResult Index(string Categoria, string Nombre, string Genero)
        {
            var productos = db.Productos.Where(a => a.Categorias.Nombre_Categoria.Contains(Categoria) &&
            a.Nombre_Producto.Contains(Nombre) &&
            a.Genero.Nombre_Genero.Contains(Genero)).OrderBy(a => a.Categorias.Nombre_Categoria).ToList();

            return View(productos);

        }





        public ActionResult InicioSesion()
        {
            return View();
        }

        public ActionResult Pruebas()
        {
            var productos = db.Productos.Include(p => p.Categorias).Include(p => p.Marcas);

            ViewBag.ID_Categoria = new SelectList(db.Categorias, "ID_Categoria", "Nombre_Categoria");
            ViewBag.ID_Marca = new SelectList(db.Marcas, "ID_Marca", "Nombre_marca");
            ViewBag.ID_Genero = new SelectList(db.Genero, "ID_Genero", "Nombre_Genero");

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

    }
}