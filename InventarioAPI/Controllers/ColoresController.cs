using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using InventarioAPI.Models;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace InventarioAPI.Controllers
{
    public class ColoresController : Controller
    {
        private readonly IConfiguration _IConfiguration;
        string msj = "";
        public ColoresController(IConfiguration configuration)
        {
            _IConfiguration = configuration;
        }

        [HttpGet]
        [Route("/api/Colores/getColores")]
        public IActionResult getColores()
        {
            Conexion objConexion = new Conexion(_IConfiguration);
            var cn = objConexion.getConexion();
            List<Color> listaColores = new List<Color>();
            try
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("spGetColores", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        listaColores.Add(new Color
                        {
                            color = dr["color"].ToString(),
                            idColor = int.Parse(dr["idColor"].ToString())
                        });
                    }
                }
                return Ok(listaColores);
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
        [Route("/api/Colores/registrarColor")]
        public IActionResult registrarColor(Color objColor)
        {
            Conexion objConexion = new Conexion(_IConfiguration);
            var cn = objConexion.getConexion();
            try
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("spNuevaColor", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@color", SqlDbType.NVarChar).Value = objColor.color;
                    int i = cmd.ExecuteNonQuery();

                    return Ok(new { msj = "Se a creado " + i + " color" });
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
        [Route("api/Colores/getColor/{idColor}")]
        public IActionResult getColor(int idColor)
        {
            Conexion objConexion = new Conexion(_IConfiguration);
            var cn = objConexion.getConexion();
            Color objColor = null;
            try
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("spGetColor", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idColor", SqlDbType.Int).Value = idColor;
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {

                        objColor = new Color
                        {
                            color = dr["color"].ToString(),
                            idColor = int.Parse(dr["idColor"].ToString())
                        };
                    }
                }
                return Ok(objColor);
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
        [Route("/api/Colores/ActualizarColor")]
        public IActionResult actualizarColor(Color objColor)
        {
            Conexion objConexion = new Conexion(_IConfiguration);
            var cn = objConexion.getConexion();
            try
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("spActualizarColor", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idColor", SqlDbType.Int).Value = objColor.idColor;
                    cmd.Parameters.Add("@color", SqlDbType.NVarChar).Value = objColor.color;
                    int i = cmd.ExecuteNonQuery();
                    if (i == 1)
                    {
                        return Ok(new { msj = "Se actualizó la color " });
                    }
                    else
                    {
                        return Ok(new { msj = "La color " + objColor.color + " no se actualizó" });
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
        [Route("/api/Colores/delColor/{idColor}")]
        public IActionResult delColor(int idColor)
        {
            Conexion objConexion = new Conexion(_IConfiguration);
            var cn = objConexion.getConexion();
            string msj = "";
            try
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("spEliminarColor", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idColor", SqlDbType.Int).Value = idColor;
                    int i = cmd.ExecuteNonQuery();
                    if (i == 1)
                    {
                        return Ok(new { msj = "Se a eliminado " + i + " color." });
                    }
                    else
                    {
                        return Ok(new { msj = "La Color no se eliminó" });
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
