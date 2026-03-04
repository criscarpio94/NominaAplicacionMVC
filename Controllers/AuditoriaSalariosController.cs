using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using NominaAplicacionMVC.Models;

namespace NominaAplicacionMVC.Controllers
{
	public class AuditoriaSalariosController : Controller
	{
		string cadena = ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString;

		public ActionResult Index()
		{
			List<AuditoriaSalario> lista = new List<AuditoriaSalario>();
			using (SqlConnection cn = new SqlConnection(cadena))
			{
				// Usamos tu procedimiento oficial del script
				SqlCommand cmd = new SqlCommand("spListarAuditoriaSalarios", cn);
				cmd.CommandType = CommandType.StoredProcedure;
				cn.Open();
				using (SqlDataReader dr = cmd.ExecuteReader())
				{
					while (dr.Read())
					{
						lista.Add(new AuditoriaSalario
						{
							Id = Convert.ToInt32(dr["id"]),
							EmpNo = Convert.ToInt32(dr["emp_no"]),
							// Concatenamos las columnas que devuelve tu SP (first_name, last_name)
							NombreEmpleado = dr["first_name"].ToString() + " " + dr["last_name"].ToString(),
							Usuario = dr["usuario"].ToString(),
							FechaHora = Convert.ToDateTime(dr["fecha_hora"]),
							Accion = dr["accion"].ToString(),
							Detalle = dr["detalle"].ToString(),
							Monto = Convert.ToInt64(dr["monto"])
						});
					}
				}
			}
			// ASEGÚRATE de que el nombre aquí coincida exactamente con tu archivo .cshtml
			return View("VistaAuditoriaSalarios", lista);
		}
	}
}