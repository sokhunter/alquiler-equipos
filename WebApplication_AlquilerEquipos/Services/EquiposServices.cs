using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using WebApplication_AlquilerEquipos.Models;

namespace WebApplication_AlquilerEquipos.Services
{
	public class EquiposServices
	{



		public bool validarNumeroPositivo(float numero, String nombre)
		{
			if (numero == 0.0)
			{
				Console.WriteLine("Error: El " + nombre + " no puede ser cero");
				return false;
			}
			if (numero < 0.0)
			{
				Console.WriteLine("Error: El " + nombre + " no puede ser negativo");
				return false;
			}
			return true;
		}

		public bool validarCampoRequerido(String texto, String nombre)
		{
			if (texto == "")
			{
				Console.WriteLine("Error: Debe ingresar el campo " + nombre);
				return false;
			}
			return true;
		}

		public bool validarLongitudMaxima(String campo, String nombre, int longitud)
		{
			int longCampo = campo.Length;
			if (longCampo > longitud)
			{
				Console.WriteLine("Error: El campo " + campo + " no debe tener más de " + longitud + "caracteres");
				return false;
			}
			return true;
		}


		public bool Agregar(Equipo equipo)
		{
			bool error = false;
			error = validarCampoRequerido(equipo.Codigo, "Código") ? error : true;
			error = validarCampoRequerido(equipo.Nombre.Trim(), "Nombre") ? error : true;
			error = validarLongitudMaxima(equipo.Nombre.Trim(), "Nombre", 100) ? error : true;
			error = validarCampoRequerido(equipo.Descripcion.Trim(), "Descripción") ? error : true;
			error = validarNumeroPositivo(equipo.Costo, "Costo") ? error : true;
			if (error)
			{
				return false;
			}

			
			if (equipo.Estado_Id == 0)
			{
				equipo.Estado_Id = 1;
			}

			// Agregar a la db
			return true;
		}

		public bool Editar(Equipo equipo)
		{
			bool error = false;
			error = validarCampoRequerido(equipo.Codigo, "Código") ? error : true;
			error = validarCampoRequerido(equipo.Nombre.Trim(), "Nombre") ? error : true;
			error = validarLongitudMaxima(equipo.Nombre.Trim(), "Nombre", 100) ? error : true;
			error = validarCampoRequerido(equipo.Descripcion.Trim(), "Descripción") ? error : true;
			error = validarNumeroPositivo(equipo.Costo, "Costo") ? error : true;
			if (error)
			{
				return false;
			}

			
			if (equipo.Estado_Id == 0)
			{
				equipo.Estado_Id = 1;
			}

			// Actualizar en la db
			return true;
		}



	}
}