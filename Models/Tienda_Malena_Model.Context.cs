﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Proyecto_Tienda_Malena.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Proyecto_Tienda_MalenaEntities : DbContext
    {
        public Proyecto_Tienda_MalenaEntities()
            : base("name=Proyecto_Tienda_MalenaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Categorias> Categorias { get; set; }
        public virtual DbSet<Comentarios> Comentarios { get; set; }
        public virtual DbSet<Factura> Factura { get; set; }
        public virtual DbSet<Marcas> Marcas { get; set; }
        public virtual DbSet<Productos> Productos { get; set; }
        public virtual DbSet<Proveedores> Proveedores { get; set; }
        public virtual DbSet<Review> Review { get; set; }
        public virtual DbSet<ROLES> ROLES { get; set; }
        public virtual DbSet<Sede> Sede { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }
    }
}
