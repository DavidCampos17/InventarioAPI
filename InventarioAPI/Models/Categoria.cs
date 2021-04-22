﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioAPI.Models
{
    public class Categoria
    {
        public int idCategoria { get; set; }
        public string categoria { get; set; }
        public DateTime fechaCracion { get; set; }
        public int idUsuario { get; set; }
    }
}
