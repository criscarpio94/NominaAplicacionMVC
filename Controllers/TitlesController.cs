using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using NominaAplicacionMVC.Models;

namespace NominaAplicacionMVC.Controllers
{
    public class TitlesController : Controller
    {
        string cadena = ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString;

        public ActionResult Index()
        {
            List<Cargo> lista = new List<Cargo>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("spListarTitles", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Cargo
                        {
                            EmpNo = Convert.ToInt32(dr["emp_no"]),
                            NombreEmpleado = dr["first_name"].ToString() + " " + dr["last_name"].ToString(),
                            Title = dr["title"].ToString(),
                            FromDate = dr["from_date"].ToString(),
                            ToDate = dr["to_date"] == DBNull.Value ? "" : dr["to_date"].ToString()
                        });
                    }
                }
            }
            return View("VistaTitles", lista);
        }

        public ActionResult Create() => View("VistaCrearTitle");

        [HttpPost]
        public ActionResult Create(Cargo c)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("spInsertarTitle", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@emp_no", c.EmpNo);
                    cmd.Parameters.AddWithValue("@title", c.Title);
                    cmd.Parameters.AddWithValue("@from_date", c.FromDate);
                    cmd.Parameters.AddWithValue("@to_date", (object)c.ToDate ?? DBNull.Value);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("VistaCrearTitle");
            }
        }

        public ActionResult Edit(int emp_no, string title, string from_date)
        {
            Cargo c = BuscarCargo(emp_no, title, from_date);
            return View("VistaEditarTitle", c);
        }

        [HttpPost]
        public ActionResult Edit(Cargo c)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("spEditarTitle", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_no", c.EmpNo);
                cmd.Parameters.AddWithValue("@title", c.Title);
                cmd.Parameters.AddWithValue("@from_date", c.FromDate);
                cmd.Parameters.AddWithValue("@to_date", (object)c.ToDate ?? DBNull.Value);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int emp_no, string title, string from_date)
        {
            return View("VistaEliminarTitle", BuscarCargo(emp_no, title, from_date));
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int emp_no, string title, string from_date)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("spEliminarTitle", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_no", emp_no);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@from_date", from_date);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        private Cargo BuscarCargo(int emp_no, string title, string from_date)
        {
            Cargo c = null;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                string sql = "SELECT * FROM titles WHERE emp_no=@e AND title=@t AND from_date=@f";
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@e", emp_no);
                cmd.Parameters.AddWithValue("@t", title);
                cmd.Parameters.AddWithValue("@f", from_date);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        c = new Cargo
                        {
                            EmpNo = (int)dr["emp_no"],
                            Title = dr["title"].ToString(),
                            FromDate = dr["from_date"].ToString(),
                            ToDate = dr["to_date"].ToString()
                        };
                    }
                }
            }
            return c;
        }
    }
}
