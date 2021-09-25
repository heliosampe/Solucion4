using DataAccess;
using DataAccess.Interfaces;
using ejemplo.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject1
{
    [TestClass]
    public class UnitTestPaisController
    {
        private IPaisData testpais;
        private ILogger<PaisController> _logger;


        public UnitTestPaisController()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IPaisData, PaisData>(serviceProvider =>
            {
                return new PaisData("Server=(local)\\sqlexpress;Initial Catalog=dbNetCoreCursoAPI;Integrated Security= true;");
            });
            var serviceProvider = services.BuildServiceProvider();
            testpais = serviceProvider.GetService<IPaisData>();
        }


        [TestMethod]

        public void ObtenerPaises()
        {
            //Arrage 
            var loggerMock = new Mock<ILogger<PaisController>>();
            ILogger<PaisController> logger = loggerMock.Object;

            PaisController obj = new PaisController(testpais, logger);
            //Act
            int result = obj.GetPaises(1, 50).Count;

            //Assert
            Assert.IsTrue(result > 0);
        }
        [TestMethod]
        public void EliminarPais()
        {
            //Arrage
            PaisController obj = new PaisController(testpais, _logger);
            Pais pais = new Pais();
            pais.IdPais = 1;
            //Act
            bool result = obj.EliminarPais(pais.IdPais);

            //Assert
            Assert.IsTrue(result, "Continente no eliminado");
        }
    }
}