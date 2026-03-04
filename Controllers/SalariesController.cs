using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using NominaAplicacionMVC.Models;

namespace NominaAplicacionMVC.Controllers
{
    public class SalariesController : Controller
    {
        string cadena = ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString;

        // GET: Salaries/Index
        public ActionResult Index()
        {
            List<Salario> lista = new List<Salario>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("spListarSalaries", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Salario
                        {
                            EmpNo = Convert.ToInt32(dr["emp_no"]),
                            NombreEmpleado = dr["first_name"].ToString() + " " + dr["last_name"].ToString(),
                            Salary = Convert.ToInt64(dr["salary"]),
                            FromDate = dr["from_date"].ToString(),
                            ToDate = dr["to_date"] == DBNull.Value ? "" : dr["to_date"].ToString()
                        });
                    }
                }
            }
            return View("VistaSalaries", lista);
        }

        // GET: Salaries/Create
        public ActionResult Create() => View("VistaCrearSalary");

        [HttpPost]
        public ActionResult Create(Salario s)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("spInsertarSalary", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_no", s.EmpNo);
                cmd.Parameters.AddWithValue("@salary", s.Salary);
                cmd.Parameters.AddWithValue("@from_date", s.FromDate);
                cmd.Parameters.AddWithValue("@to_date", (object)s.ToDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@usuario", "Admin_Sistema"); // Requerido por tu SP
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // GET: Salaries/Edit
        public ActionResult Edit(int emp_no, string from_date)
        {
            Salario s = null;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                // Buscamos el registro específico para editar
                string sql = "SELECT * FROM salaries WHERE emp_no = @e AND from_date = @f";
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@e", emp_no);
                cmd.Parameters.AddWithValue("@f", from_date);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        s = new Salario
                        {
                            EmpNo = Convert.ToInt32(dr["emp_no"]),
                            Salary = Convert.ToInt64(dr["salary"]),
                            FromDate = dr["from_date"].ToString(),
                            ToDate = dr["to_date"] == DBNull.Value ? "" : dr["to_date"].ToString()
                        };
                    }
                }
            }
            return View("VistaEditarSalary", s);
        }

        [HttpPost]
        public ActionResult Edit(Salario s)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("spEditarSalary", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_no", s.EmpNo);
                cmd.Parameters.AddWithValue("@from_date", s.FromDate);
                cmd.Parameters.AddWithValue("@salary", s.Salary);
                cmd.Parameters.AddWithValue("@to_date", (object)s.ToDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@usuario", "Admin_Sistema");
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // GET: Salaries/Delete
        public ActionResult Delete(int emp_no, string from_date)
        {
            // Reutilizamos la lógica de búsqueda para mostrar qué vamos a borrar
            return Edit(emp_no, from_date);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int emp_no, string from_date)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("spEliminarSalary", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_no", emp_no);
                cmd.Parameters.AddWithValue("@from_date", from_date);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}