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

    public class CategoriasController : Controller
    {
        private readonly IConfiguration _IConfiguration;
        public CategoriasController(IConfiguration configuration)
        {
            _IConfiguration = configuration;
        }


        [HttpGet]
        [Route("/api/Categorias/getCategorias")]
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
        [Route("/api/Categorias/registrarCategoria")]
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

        [HttpGet]
        [Route("api/Categorias/getCategoria/{idCategoria}")]
        public IActionResult getCategoria(int idCategoria)
        {
            Conexion objConexion = new Conexion(_IConfiguration);
            var cn = objConexion.getConexion();
            Categoria objCategoria = null;
            try
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("spGetCategoria", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idCategoria", SqlDbType.Int).Value = idCategoria;
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {

                        objCategoria = new Categoria
                        {
                            categoria = dr["categoria"].ToString(),
                            idCategoria = int.Parse(dr["idCategoria"].ToString())
                        };
                    }
                }
                return Ok(objCategoria);
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

        [HttpPut]
        [Route("/api/Categorias/ActualizarCategoria")]
        public IActionResult actualizarCategoria(Categoria objCategoria)
        {
            Conexion objConexion = new Conexion(_IConfiguration);
            var cn = objConexion.getConexion();
            string msj = "";
            try
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("spActualizarCategoria", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idCategoria", SqlDbType.NVarChar).Value = objCategoria.idCategoria;
                    cmd.Parameters.Add("@categoria", SqlDbType.NVarChar).Value = objCategoria.categoria;
                    int i = cmd.ExecuteNonQuery();
                    if (i == 1)
                    {
                        return Ok(new { msj = "Se actualizó la categoría " });
                    }
                    else
                    {
                        return Ok(new { msj = "La categoría " + objCategoria.categoria + " no se actualizó" });
                    }
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

        [HttpDelete]
        [Route("/api/Categorias/delCategoria/{idCategoria}")]
        public IActionResult delCategoria(int idCategoria)
        {
            Conexion objConexion = new Conexion(_IConfiguration);
            var cn = objConexion.getConexion();
            string msj = "";
            try
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("spDelCategoria", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idCategoria", SqlDbType.Int).Value = idCategoria;
                    int i = cmd.ExecuteNonQuery();
                    if (i == 1)
                    {
                        return Ok(new { msj = "Se a eliminado " + i + " categoria." });
                    }
                    else
                    {
                        return Ok(new { msj = "La categoría no se eliminó" });
                    }
                }
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
    }
}
