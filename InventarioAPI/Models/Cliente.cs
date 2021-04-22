using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioAPI.Models
{
    public class Cliente
    {
        public int idCliente { get; set; }
        public string nomCliente { get; set; }
        public string apPCliente { get; set; }
        public string apMCliente { get; set; }
        public string telCliente { get; set; }
    }
}
