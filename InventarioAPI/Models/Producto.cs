using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioAPI.Models
{
    public class Producto
    {
        public int idProducto { get; set; }
        public string producto { get; set; }

        public string SKU { get; set; }
        public string numeroProducto { get; set; }

        public DateTime fechaCompra { get; set; }
        public DateTime fechaVenta { get; set; }
        public int cantidad { get; set; }
        public double precio { get; set; }
        public string descripcion { get; set; }
        public int idCategoria { get; set; }
        public int idMarca { get; set; }
        public int idColor { get; set; }
        public int idMoneda { get; set; }
        public int idUsuario { get; set; }
    }
}
