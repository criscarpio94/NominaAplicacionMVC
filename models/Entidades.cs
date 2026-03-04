using System;
using System.Collections.Generic;

namespace NominaAplicacionMVC.Models
{

	public class Departamento
	{
		public int DeptNo { get; set; }
		public string DeptName { get; set; }
	}


	public class Empleado
	{
		public int EmpNo { get; set; }
		public DateTime BirthDate { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Gender { get; set; }
		public DateTime HireDate { get; set; }
	}


	public class Usuario
	{
		public int EmpNo { get; set; }
		public string NombreUsuario { get; set; }
		public string Clave { get; set; }
	}


	public class Salario
	{
		public int EmpNo { get; set; }
		public string NombreEmpleado { get; set; }
		public long Salary { get; set; }
		public string FromDate { get; set; }
		public string ToDate { get; set; }
	}


	public class Cargo
	{
		public int EmpNo { get; set; }
		public string NombreEmpleado { get; set; }
		public string Title { get; set; }
		public string FromDate { get; set; }
		public string ToDate { get; set; }
	}


	public class DepartamentoEmpleado
	{
		public int EmpNo { get; set; }
		public string NombreEmpleado { get; set; }
		public int DeptNo { get; set; }
		public string NombreDepartamento { get; set; }
		public string FromDate { get; set; }
		public string ToDate { get; set; }
	}


	public class DepartamentoGerente
	{
		public int EmpNo { get; set; }

		public string NombreEmpleado { get; set; }
		public int DeptNo { get; set; }
		public string FromDate { get; set; }
		public string ToDate { get; set; }
	}

	public class AuditoriaSalario
	{
		public int Id { get; set; }
		public int EmpNo { get; set; }
		public string NombreEmpleado { get; set; }
		public string Usuario { get; set; }
		public DateTime FechaHora { get; set; }
		public string Accion { get; set; }
		public string Detalle { get; set; }
		public long Monto { get; set; }
	}
}