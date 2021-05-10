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
    public class MonedasController : Controller
    {
        private readonly IConfiguration _IConfiguration;
        string msj = "";
        public MonedasController(IConfiguration configuration)
        {
            _IConfiguration = configuration;
        }

        [HttpGet]
        [Route("/api/Monedas/getMonedas")]
        public IActionResult getMonedas()
        {
            Conexion objConexion = new Conexion(_IConfiguration);
            var cn = objConexion.getConexion();
            List<Moneda> listaMonedas = new List<Moneda>();
            try
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("spGetMonedas", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        listaMonedas.Add(new Moneda
                        {
                            moneda = dr["moneda"].ToString(),
                            idMoneda = int.Parse(dr["idMoneda"].ToString())
                        });
                    }
                }
                return Ok(listaMonedas);
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
        [Route("/api/Monedas/registrarMoneda")]
        public IActionResult registrarMoneda(Moneda objMoneda)
        {
            Conexion objConexion = new Conexion(_IConfiguration);
            var cn = objConexion.getConexion();
            try
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("spNuevaMoneda", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@moneda", SqlDbType.NVarChar).Value = objMoneda.moneda;
                    int i = cmd.ExecuteNonQuery();

                    return Ok(new { msj = "Se a creado " + i + " moneda" });
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
        [Route("api/Monedas/getMoneda/{idMoneda}")]
        public IActionResult getMoneda(int idMoneda)
        {
            Conexion objConexion = new Conexion(_IConfiguration);
            var cn = objConexion.getConexion();
            Moneda objMoneda = null;
            try
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("spGetMoneda", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idMoneda", SqlDbType.Int).Value = idMoneda;
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {

                        objMoneda = new Moneda
                        {
                            moneda = dr["moneda"].ToString(),
                            idMoneda = int.Parse(dr["idMoneda"].ToString())
                        };
                    }
                }
                return Ok(objMoneda);
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
        [Route("/api/Monedas/ActualizarMoneda")]
        public IActionResult actualizarMoneda(Moneda objMoneda)
        {
            Conexion objConexion = new Conexion(_IConfiguration);
            var cn = objConexion.getConexion();
            try
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("spActualizarMoneda", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idMoneda", SqlDbType.Int).Value = objMoneda.idMoneda;
                    cmd.Parameters.Add("@moneda", SqlDbType.NVarChar).Value = objMoneda.moneda;
                    int i = cmd.ExecuteNonQuery();
                    if (i == 1)
                    {
                        return Ok(new { msj = "Se actualizó la moneda " });
                    }
                    else
                    {
                        return Ok(new { msj = "La moneda " + objMoneda.moneda + " no se actualizó" });
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
        [Route("/api/Monedas/delMoneda/{idMoneda}")]
        public IActionResult delMoneda(int idMoneda)
        {
            Conexion objConexion = new Conexion(_IConfiguration);
            var cn = objConexion.getConexion();
            string msj = "";
            try
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("spEliminarMoneda", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idMoneda", SqlDbType.Int).Value = idMoneda;
                    int i = cmd.ExecuteNonQuery();
                    if (i == 1)
                    {
                        return Ok(new { msj = "Se a eliminado " + i + " moneda." });
                    }
                    else
                    {
                        return Ok(new { msj = "La Moneda no se eliminó" });
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
