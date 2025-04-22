using System.ComponentModel.DataAnnotations;

namespace FreshBites.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        [Range(0.01, 9999.99)]
        public decimal Precio { get; set; }

        [Range(0, 1000)]
        public int Stock { get; set; }

        public string ImagenUrl { get; set; }
    }

}
