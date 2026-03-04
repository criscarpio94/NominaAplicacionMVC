using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NominaAplicacionMVC.Controllers
{
    public class UsuariosController : Controller
    {
        // GET: Usuarios
        public ActionResult Index()
        {
            return View("VistaUsuarios");
        }

        //POST: Usuarios
        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            string cadena = ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString;

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand comando = new SqlCommand("spValidarUsuario", cn);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("usuario", correo);
                comando.Parameters.AddWithValue("clave", clave);
                comando.Parameters.Add("id", SqlDbType.Int).Direction = ParameterDirection.Output;
                comando.Parameters.Add("mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                cn.Open();
                comando.ExecuteNonQuery();
                cn.Close();

                int id = Convert.ToInt32(comando.Parameters["id"].Value);
                string mensaje = comando.Parameters["mensaje"].Value.ToString();

                if (id > 0)
                {
                    // Guardar usuario en sesión
                    Session["usuario"] = correo;

                    // Redirigir al menú principal
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Mostrar mensaje de error en la misma vista de login
                    ViewBag.Mensaje = mensaje;
                    return View("VistaUsuarios");
                }
            }
        }
    }
}


