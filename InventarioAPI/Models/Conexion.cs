using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioAPI.Models
{
    public class Conexion
    {
        private readonly IConfiguration _IConfiguration;

        public Conexion(IConfiguration IConfiguration)
        {
            _IConfiguration = IConfiguration;
        }

        public SqlConnection getConexion()
        {
            SqlConnection sql = new SqlConnection(_IConfiguration.GetValue<string>("ConnectionStrings:Data"));
            return sql;
        }
    }
}
