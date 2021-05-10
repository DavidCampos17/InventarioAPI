using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using InventarioAPI.Models;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace InventarioAPI.Controllers
{
    public class MarcasController : Controller
    {
        private readonly IConfiguration _IConfiguration;
        string msj = "";
        public MarcasController(IConfiguration configuration)
        {
            _IConfiguration = configuration;
        }

        [HttpGet]
        [Route("/api/Marcas/getMarcas")]
        public IActionResult getMarcas()
        {
            Conexion objConexion = new Conexion(_IConfiguration);
            var cn = objConexion.getConexion();
            List<Marca> listaMarcas = new List<Marca>();
            try
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("spGetMarcas", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        listaMarcas.Add(new Marca
                        {
                            marca = dr["marca"].ToString(),
                            idMarca = int.Parse(dr["idMarca"].ToString())
                        });
                    }
                }
                return Ok(listaMarcas);
            }
            catch (Exception e)
            {
                msj = e.Message;
                cn.Close();
                return BadRequest(msj);
                throw;
            }
            finally
            {
                cn.Close();
            }

        }


        [HttpPost]
        [Route("/api/Marcas/registrarMarca")]
        public IActionResult registrarMarca(Marca objMarca)
        {
            Conexion objConexion = new Conexion(_IConfiguration);
            var cn = objConexion.getConexion();
            try
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("spNuevaMarca", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@marca", SqlDbType.NVarChar).Value = objMarca.marca;
                    int i = cmd.ExecuteNonQuery();

                    return Ok(new { msj = "Se a creado " + i + " marca" });
                }
            }
            catch (Exception e)
            {
                cn.Close();
                msj = e.Message;
                return BadRequest(msj);
                throw;
            }
            finally
            {
                cn.Close();
            }
        }

        [HttpGet]
        [Route("api/Marcas/getMarca/{idMarca}")]
        public IActionResult getMarca(int idMarca)
        {
            Conexion objConexion = new Conexion(_IConfiguration);
            var cn = objConexion.getConexion();
            Marca objMarca = null;
            try
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("spGetMarca", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idMarca", SqlDbType.Int).Value = idMarca;
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {

                        objMarca = new Marca
                        {
                            marca = dr["marca"].ToString(),
                            idMarca = int.Parse(dr["idMarca"].ToString())
                        };
                    }
                }
                return Ok(objMarca);
            }
            catch (Exception e)
            {
                msj = e.Message;
                cn.Close();
                return BadRequest(msj);
                throw;
            }
            finally
            {
                cn.Close();

            }
        }

        [HttpPut]
        [Route("/api/Marcas/ActualizarMarca")]
        public IActionResult actualizarMarca(Marca objMarca)
        {
            Conexion objConexion = new Conexion(_IConfiguration);
            var cn = objConexion.getConexion();
            try
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("spActualizarMarca", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idMarca", SqlDbType.Int).Value = objMarca.idMarca;
                    cmd.Parameters.Add("@marca", SqlDbType.NVarChar).Value = objMarca.marca;
                    int i = cmd.ExecuteNonQuery();
                    if (i == 1)
                    {
                        return Ok(new { msj = "Se actualizó la marca " });
                    }
                    else
                    {
                        return Ok(new { msj = "La marca " + objMarca.marca + " no se actualizó" });
                    }
                }
            }
            catch (Exception e)
            {
                cn.Close();
                msj = e.Message;
                return BadRequest(msj);
                throw;
            }
            finally
            {
                cn.Close();
            }
        }

        [HttpDelete]
        [Route("/api/Marcas/delMarca/{idMarca}")]
        public IActionResult delMarca(int idMarca)
        {
            Conexion objConexion = new Conexion(_IConfiguration);
            var cn = objConexion.getConexion();
            string msj = "";
            try
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("spEliminarMarca", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idMarca", SqlDbType.Int).Value = idMarca;
                    int i = cmd.ExecuteNonQuery();
                    if (i == 1)
                    {
                        return Ok(new { msj = "Se a eliminado " + i + " marca." });
                    }
                    else
                    {
                        return Ok(new { msj = "La marca no se eliminó" });
                    }
                }
            }
            catch (Exception e)
            {
                cn.Close();
                msj = e.Message;
                return BadRequest(e.Message);
                throw;
            }
            finally
            {
                cn.Close();
            }
        }
    }
}
