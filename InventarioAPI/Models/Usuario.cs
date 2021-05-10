using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioAPI.Models
{
    public class Usuario
    {
        public int idUsuario
        {
            get; set;
        }
        public string nombreUsuario
        {
            get; set;
        }
        public string apellidoPadre
        {
            get; set;
        }
        public string apellidoMadre
        {
            get; set;
        }
        public string correo
        {
            get; set;
        }
        public string usuario
        {
            get; set;
        }
        public string contrasena
        {
            get; set;
        }
    }
}
