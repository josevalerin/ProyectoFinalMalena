//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Review
    {
        public int ID_Review { get; set; }
        public int ID_Producto { get; set; }
        public int ID_Usuario { get; set; }
        public string Comentario { get; set; }
        public int Estrellas { get; set; }
    
        public virtual Productos Productos { get; set; }
        public virtual Usuarios Usuarios { get; set; }
    }
}