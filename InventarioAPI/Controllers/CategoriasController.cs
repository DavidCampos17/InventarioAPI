using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventarioAPI.Models;
using System.Data.SqlClient;
using System.Data;

namespace InventarioAPI.Controllers
{
    [ApiController]
    [Route("api/Categorias")]
    public class CategoriasController : Controller
    {

        private readonly IConfiguration _IConfiguration;
        public CategoriasController(IConfiguration configuration)
        {
            _IConfiguration = configuration;
        }


        [HttpGet]
        [Route("/getCategorias")]
        public IActionResult getCategorias()
        {
            Conexion objConexion = new Conexion(_IConfiguration);
            var cn = objConexion.getConexion();
            List<Categoria> listaCategorias = new List<Categoria>();
            try
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("spListarCategorias", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        listaCategorias.Add(new Categoria
                        {
                            categoria = dr["categoria"].ToString(),
                            idCategoria = int.Parse(dr["idCategoria"].ToString())
                        });
                    }
                }
                return Ok(listaCategorias);
            }
            catch (Exception)
            {
                return BadRequest();
                cn.Close();
                throw;
            }
            finally
            {
                cn.Close();
            }

        }

        [HttpPost]
        [Route("/registrarCategoria")]
        public IActionResult registrarCategoria(Categoria objCategoria)
        {
            Conexion objConexion = new Conexion(_IConfiguration);
            var cn = objConexion.getConexion();
            string msj = "";
            try
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("spNuevaCategoria", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@categoria", SqlDbType.NVarChar).Value = objCategoria.categoria;
                    int i = cmd.ExecuteNonQuery();

                    return Ok(new { msj = "Se a creado " + i + " categoria" });
                }
            }
            catch (Exception)
            {
                cn.Close();
                return BadRequest();
                throw;
            }
            finally
            {
                cn.Close();
            }
        }
    }
}
