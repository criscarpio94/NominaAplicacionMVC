using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using NominaAplicacionMVC.Models;

namespace NominaAplicacionMVC.Controllers
{
    public class DepartamentosController : Controller
    {
        // Conexión a la base de datos desde el Web.config
        string cadena = ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString;

        // 1. LISTAR: GET /Departamentos/Index
        public ActionResult Index()
        {
            List<Departamento> lista = new List<Departamento>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("spListarDepartamentos", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Departamento
                        {
                            DeptNo = Convert.ToInt32(dr["dept_no"]),
                            DeptName = dr["dept_name"].ToString()
                        });
                    }
                }
            }
            return View("VistaDepartamentos", lista);
        }

        // 2. CREAR: GET /Departamentos/Create
        public ActionResult Create()
        {
            return View("VistaCrearDepartamento");
        }

        [HttpPost]
        public ActionResult Create(Departamento d)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("spInsertarDepartamento", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@dept_no", d.DeptNo);
                cmd.Parameters.AddWithValue("@dept_name", d.DeptName);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // 3. EDITAR: GET /Departamentos/Edit/5
        public ActionResult Edit(int id)
        {
            Departamento d = null;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                // Consulta directa para obtener los datos actuales antes de editar
                SqlCommand cmd = new SqlCommand("SELECT * FROM departments WHERE dept_no = @id", cn);
                cmd.Parameters.AddWithValue("@id", id);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        d = new Departamento
                        {
                            DeptNo = (int)dr["dept_no"],
                            DeptName = dr["dept_name"].ToString()
                        };
                    }
                }
            }
            return d == null ? (ActionResult)HttpNotFound() : View("VistaEditarDepartamento", d);
        }

        [HttpPost]
        public ActionResult Edit(Departamento d)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("spEditarDepartamento", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@dept_no", d.DeptNo);
                cmd.Parameters.AddWithValue("@dept_name", d.DeptName);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // 4. ELIMINAR: GET /Departamentos/Delete/5
        public ActionResult Delete(int id)
        {
            // Reutilizamos la lógica de búsqueda del Edit para mostrar qué se va a borrar
            Departamento d = null;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM departments WHERE dept_no = @id", cn);
                cmd.Parameters.AddWithValue("@id", id);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        d = new Departamento { DeptNo = (int)dr["dept_no"], DeptName = dr["dept_name"].ToString() };
                    }
                }
            }
            return View("VistaEliminarDepartamento", d);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("spEliminarDepartamento", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@dept_no", id);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}