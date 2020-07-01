using System;
using System.ComponentModel;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication_AlquilerEquipos.Controllers;
using WebApplication_AlquilerEquipos.Models;
using WebApplication_AlquilerEquipos.Services;

namespace WebApplication_AlquilerEquipos.Tests.Controllers
{
	[TestClass]
	public class EquiposControllerTest
	{
		[TestMethod]
		public void CostoEsMenorOIgualACero()
		{
			EquiposServices equiposServices = new EquiposServices();
			Equipo equipo = new Equipo() { Costo = -45 };
			bool result = equiposServices.validarNumeroPositivo(equipo.Costo, "Costo");
			Assert.IsTrue(result);
		}
		[TestMethod]
		public void CodigoEsVacio()
		{
			EquiposServices equiposServices = new EquiposServices();
			Equipo equipo = new Equipo() { Codigo = string.Empty };
			bool result = equiposServices.validarCampoRequerido(equipo.Codigo, "Código");
			Assert.IsTrue(result);
		}
		[TestMethod]
		public void NombreEsVacio()
		{
			EquiposServices equiposServices = new EquiposServices();
			Equipo equipo = new Equipo() { Nombre = string.Empty };
			bool result = equiposServices.validarCampoRequerido(equipo.Nombre.Trim(), "Nombre");
			Assert.IsTrue(result);
		}
		[TestMethod]
		public void NombreMax100Caracteres ()
		{
			EquiposServices equiposServices = new EquiposServices();
			Equipo equipo = new Equipo() { Nombre = "Equipo de Paracaidas" };
			bool result = equiposServices.validarLongitudMaxima(equipo.Nombre.Trim(), "Nombre", 100);
			Assert.IsTrue(result);
		}
		[TestMethod]
		public void DescripcionEsVacio()
		{
			EquiposServices equiposServices = new EquiposServices();
			Equipo equipo = new Equipo() { Descripcion = string.Empty };
			bool result = equiposServices.validarCampoRequerido(equipo.Descripcion.Trim(), "Descripción");
			Assert.IsTrue(result);
		}
		[TestMethod]
		public void Integracion_EntidadEstado()
		{
			EquiposController equiposController = new EquiposController();
			ViewResult result = equiposController.Create() as ViewResult;
			Assert.IsNotNull(result.ViewBag.Estado_Id);
		}
		
	}
}
