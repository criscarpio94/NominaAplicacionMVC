using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using NominaAplicacionMVC.Models;

namespace NominaAplicacionMVC.Controllers
{
    public class DepEmpController : Controller
    {
        string cadena = ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString;

        public ActionResult Index()
        {
            List<DepartamentoEmpleado> lista = new List<DepartamentoEmpleado>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("spListarDeptEmp", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new DepartamentoEmpleado
                        {
                            EmpNo = Convert.ToInt32(dr["emp_no"]),
                            NombreEmpleado = dr["first_name"].ToString() + " " + dr["last_name"].ToString(),
                            DeptNo = Convert.ToInt32(dr["dept_no"]),
                            NombreDepartamento = dr["dept_name"].ToString(),
                            FromDate = dr["from_date"].ToString(),
                            ToDate = dr["to_date"].ToString()
                        });
                    }
                }
            }
            return View("VistaDepEmp", lista);
        }

        public ActionResult Create() => View("VistaCrearDepEmp");

        [HttpPost]
        public ActionResult Create(DepartamentoEmpleado obj)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("spInsertarDeptEmp", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_no", obj.EmpNo);
                cmd.Parameters.AddWithValue("@dept_no", obj.DeptNo);
                cmd.Parameters.AddWithValue("@from_date", obj.FromDate);
                cmd.Parameters.AddWithValue("@to_date", obj.ToDate);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int emp_no, int dept_no)
        {
            DepartamentoEmpleado obj = BuscarPorLlave(emp_no, dept_no);
            return View("VistaEditarDepEmp", obj); // Nombre corregido
        }

        [HttpPost]
        public ActionResult Edit(DepartamentoEmpleado obj)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("spEditarDeptEmp", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_no", obj.EmpNo);
                cmd.Parameters.AddWithValue("@dept_no", obj.DeptNo);
                cmd.Parameters.AddWithValue("@from_date", obj.FromDate);
                cmd.Parameters.AddWithValue("@to_date", obj.ToDate);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int emp_no, int dept_no)
        {
            DepartamentoEmpleado obj = BuscarPorLlave(emp_no, dept_no);
            return View("VistaEliminarDepEmp", obj); // Nombre corregido
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int emp_no, int dept_no)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("spEliminarDeptEmp", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_no", emp_no);
                cmd.Parameters.AddWithValue("@dept_no", dept_no);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        private DepartamentoEmpleado BuscarPorLlave(int emp_no, int dept_no)
        {
            DepartamentoEmpleado obj = null;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                string sql = "SELECT * FROM dept_emp WHERE emp_no = @e AND dept_no = @d";
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@e", emp_no);
                cmd.Parameters.AddWithValue("@d", dept_no);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        obj = new DepartamentoEmpleado
                        {
                            EmpNo = (int)dr["emp_no"],
                            DeptNo = (int)dr["dept_no"],
                            FromDate = dr["from_date"].ToString(),
                            ToDate = dr["to_date"].ToString()
                        };
                    }
                }
            }
            return obj;
        }
    }
}