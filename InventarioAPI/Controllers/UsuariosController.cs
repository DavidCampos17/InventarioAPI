using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using InventarioAPI.Models;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace InventarioAPI.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IConfiguration _IConfiguration;
        string msj = "";
        public UsuariosController(IConfiguration configuration)
        {
            _IConfiguration = configuration;
        }

        [HttpGet]
        [Route("/api/Usuarios/getUsuarios")]
        public IActionResult getUsuarios()
        {
            Conexion objConexion = new Conexion(_IConfiguration);
            var cn = objConexion.getConexion();
            List<Usuario> listaUsuarios = new List<Usuario>();
            try
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("spGetUsuarios", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        listaUsuarios.Add(new Usuario
                        {
                            idUsuario = int.Parse(dr["idUsuario"].ToString()),
                            nombreUsuario = dr["nombreUsuario"].ToString(),
                            apellidoPadre = dr["apellidoPadre"].ToString(),
                            apellidoMadre = dr["apellidoMadre"].ToString(),
                            correo = dr["correo"].ToString(),
                            usuario = dr["usuario"].ToString(),

                        });
                    }
                }
                return Ok(listaUsuarios);
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
        [Route("/api/Usuarios/registrarUsuario")]
        public IActionResult registrarUsuario(Usuario objUsuario)
        {
            Conexion objConexion = new Conexion(_IConfiguration);
            var cn = objConexion.getConexion();
            try
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("spNuevaUsuario", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@nombreUsuario", SqlDbType.NVarChar).Value = objUsuario.nombreUsuario;
                    cmd.Parameters.Add("@apellidoPadre", SqlDbType.NVarChar).Value = objUsuario.apellidoPadre;
                    cmd.Parameters.Add("@apellidoMadre", SqlDbType.NVarChar).Value = objUsuario.apellidoMadre;
                    cmd.Parameters.Add("@correo", SqlDbType.NVarChar).Value = objUsuario.correo;
                    cmd.Parameters.Add("@usuario", SqlDbType.NVarChar).Value = objUsuario.usuario;

                    // Cifrado de contraseña
                    SHA256Managed sh = new SHA256Managed();
                    byte[] bytecontra = Encoding.Default.GetBytes(objUsuario.contrasena); // Convertimos a bytes con system.text
                    byte[] byteCifrados = sh.ComputeHash(bytecontra);
                    cmd.Parameters.Add("@contrasena", SqlDbType.NVarChar).Value = BitConverter.ToString(byteCifrados).Replace("-", "");
                    //----

                    int i = cmd.ExecuteNonQuery();

                    return Ok(new { msj = "Se a creado " + i + " usuario" });
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
        [Route("api/Usuarios/getUsuario/{idUsuario}")]
        public IActionResult getUsuario(int idUsuario)
        {
            Conexion objConexion = new Conexion(_IConfiguration);
            var cn = objConexion.getConexion();
            Usuario objUsuario = null;
            try
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("spGetUsuario", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = idUsuario;
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {

                        objUsuario = new Usuario
                        {
                            idUsuario = int.Parse(dr["idUsuario"].ToString()),
                            usuario = dr["usuario"].ToString(),
                            nombreUsuario = dr["nombreUsuario"].ToString(),
                            apellidoPadre = dr["apellidoPadre"].ToString(),
                            apellidoMadre = dr["apellidoMadre"].ToString(),
                            correo = dr["correo"].ToString()
                        };
                    }
                }
                return Ok(objUsuario);
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
        [Route("/api/Usuarios/ActualizarUsuario")]
        public IActionResult actualizarUsuario(Usuario objUsuario)
        {
            Conexion objConexion = new Conexion(_IConfiguration);
            var cn = objConexion.getConexion();
            try
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("spActualizarUsuario", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = objUsuario.idUsuario;
                    cmd.Parameters.Add("@nombreUsuario", SqlDbType.NVarChar).Value = objUsuario.nombreUsuario;
                    cmd.Parameters.Add("@apellidoPadre", SqlDbType.NVarChar).Value = objUsuario.apellidoPadre;
                    cmd.Parameters.Add("@apellidoMadre", SqlDbType.NVarChar).Value = objUsuario.apellidoMadre;
                    cmd.Parameters.Add("@correo", SqlDbType.NVarChar).Value = objUsuario.correo;
                    int i = cmd.ExecuteNonQuery();
                    if (i == 1)
                    {
                        return Ok(new { msj = "Se actualizó la usuario " });
                    }
                    else
                    {
                        return Ok(new { msj = "La usuario " + objUsuario.usuario + " no se actualizó" });
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

        [HttpPut]
        [Route("/api/Usuarios/ActualizarContrasena")]
        public IActionResult actualizarContrasena(Usuario objUsuario)
        {
            Conexion objConexion = new Conexion(_IConfiguration);
            var cn = objConexion.getConexion();
            try
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("spCambiarContrasena", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = objUsuario.idUsuario;
                    // Cifrado de contraseña
                    SHA256Managed sh = new SHA256Managed();
                    byte[] bytecontra = Encoding.Default.GetBytes(objUsuario.contrasena); // Convertimos a bytes con system.text
                    byte[] byteCifrados = sh.ComputeHash(bytecontra);
                    cmd.Parameters.Add("@contrasena", SqlDbType.NVarChar).Value = BitConverter.ToString(byteCifrados).Replace("-", "");
                    //----
                    int i = cmd.ExecuteNonQuery();
                    if (i == 1)
                    {
                        return Ok(new { msj = "Se actualizó la usuario " });
                    }
                    else
                    {
                        return Ok(new { msj = "La usuario " + objUsuario.usuario + " no se actualizó" });
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
        [Route("/api/Usuarios/delUsuario/{idUsuario}")]
        public IActionResult delUsuario(int idUsuario)
        {
            Conexion objConexion = new Conexion(_IConfiguration);
            var cn = objConexion.getConexion();
            string msj = "";
            try
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("spEliminarUsuario", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = idUsuario;
                    int i = cmd.ExecuteNonQuery();
                    if (i == 1)
                    {
                        return Ok(new { msj = "Se a eliminado " + i + " usuario." });
                    }
                    else
                    {
                        return Ok(new { msj = "La Usuario no se eliminó" });
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

