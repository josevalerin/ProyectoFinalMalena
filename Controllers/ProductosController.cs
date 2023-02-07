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
using Proyecto_Tienda_Malena.Models;

namespace Proyecto_Tienda_Malena.Controllers
{
    public class ProductosController : Controller
    {
        private Proyecto_Tienda_MalenaEntities db = new Proyecto_Tienda_MalenaEntities();

        // GET: Productos
        public ActionResult Index()
        {
            var productos = db.Productos.Include(p => p.Categorias).Include(p => p.Marcas);
            return View(productos.ToList());
        }

        [HttpPost]
        public ActionResult Index(string Nombre)
        {
            var productos = db.Productos.Where(a => a.Nombre_Producto.Contains(Nombre)).OrderBy(a=>a.Nombre_Producto).ToList();
            return View(productos);
        }


        public ActionResult Masculino()
        {
            var productos = db.Productos.Include(p => p.Categorias).Include(p => p.Marcas);
            ViewBag.ID_Categoria = new SelectList(db.Categorias, "ID_Categoria", "Nombre_Categoria");
            ViewBag.ID_Marca = new SelectList(db.Marcas, "ID_Marca", "Nombre_marca");
            ViewBag.ID_Genero = new SelectList(db.Genero, "ID_Genero", "Nombre_Genero");
            return View(productos.ToList());
        }

     
        // GET: Productos/Details/5
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

        // GET: Productos/Create
        public ActionResult Create()
        {
            ViewBag.ID_Categoria = new SelectList(db.Categorias, "ID_Categoria", "Nombre_Categoria");
            ViewBag.ID_Marca = new SelectList(db.Marcas, "ID_Marca", "Nombre_marca");
            ViewBag.ID_Genero = new SelectList(db.Genero, "ID_Genero", "Nombre_Genero");

            return View();
        }


              

        // POST: Productos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Producto,ID_Categoria,ID_Marca,ID_Genero,Nombre_Producto,Imagen_Producto,Descripcion_Producto,Stock,Precio,Descuento,Destacado")] Productos productos)
        {

            HttpPostedFileBase FileBase = Request.Files[0];

            WebImage image = new WebImage(FileBase.InputStream);

            productos.Imagen_Producto = image.GetBytes();

            if (ModelState.IsValid)
            {
                if (NoExisteProducto(productos))
                {
                    db.Productos.Add(productos);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException )
                    {
                        

                  
                   
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorRegistro = "Producto ya guardado en la base de datos";
                }

            }

            ViewBag.ID_Categoria = new SelectList(db.Categorias, "ID_Categoria", "Nombre_Categoria", productos.ID_Categoria);
            ViewBag.ID_Marca = new SelectList(db.Marcas, "ID_Marca", "Nombre_marca", productos.ID_Marca);
            ViewBag.ID_Genero = new SelectList(db.Genero, "ID_Genero", "Nombre_Genero", productos.ID_Genero);
            return View(productos);
        }
        public bool NoExisteProducto(Productos producto)
        {
            if (db.Productos.Where(p => p.ID_Producto == producto.ID_Producto
            || p.Nombre_Producto == producto.Nombre_Producto).Count() == 0)
            {
                return true;
            }
            return false;
        }


        // GET: Productos/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.ID_Categoria = new SelectList(db.Categorias, "ID_Categoria", "Nombre_Categoria", productos.ID_Categoria);
            ViewBag.ID_Marca = new SelectList(db.Marcas, "ID_Marca", "Nombre_marca", productos.ID_Marca);
            ViewBag.ID_Genero = new SelectList(db.Genero, "ID_Genero", "Nombre_Genero", productos.ID_Genero);

            return View(productos);
        }

        // POST: Productos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Producto,ID_Categoria,ID_Marca,ID_Genero,Nombre_Producto,Imagen_Producto,Descripcion_Producto,Stock,Precio,Descuento,Destacado")] Productos productos)
        {

            byte[] imagenActual = null;


            HttpPostedFileBase FileBase = Request.Files[0];

            if (FileBase == null)
            {

                imagenActual = db.Productos.SingleOrDefault(t => t.ID_Producto == productos.ID_Producto).Imagen_Producto;
            }
            else
            {

                WebImage image = new WebImage(FileBase.InputStream);

                productos.Imagen_Producto = image.GetBytes();

            }


            if (ModelState.IsValid)
            {
                db.Entry(productos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_Categoria = new SelectList(db.Categorias, "ID_Categoria", "Nombre_Categoria", productos.ID_Categoria);
            ViewBag.ID_Marca = new SelectList(db.Marcas, "ID_Marca", "Nombre_marca", productos.ID_Marca);
            ViewBag.ID_Genero = new SelectList(db.Genero, "ID_Genero", "Nombre_Genero", productos.ID_Genero);

            return View(productos);
        }

        // GET: Productos/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Productos productos = db.Productos.Find(id);
            db.Productos.Remove(productos);
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

        public ActionResult GetImage(int id)
        {
            Productos productos = db.Productos.Find(id);
            byte[] byteImage = productos.Imagen_Producto;

            MemoryStream memoryStream = new MemoryStream(byteImage);
            Image image = Image.FromStream(memoryStream);


            memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Jpeg);
            memoryStream.Position = 0;


            return File(memoryStream, "image/jpg");



        }
    }
}
