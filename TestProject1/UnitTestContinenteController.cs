
using DataAccess;
using DataAccess.Interfaces;
using ejemplo.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Moq;



namespace TestProject1
{
    [TestClass]
    public class UnitTestContinenteController
    {
        private IContinenteData testcontinente;
        private ILogger<ContinenteController> _logger;


        public UnitTestContinenteController()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IContinenteData, ContinenteData>(serviceProvider =>
            {
                return new ContinenteData("Server=(local)\\sqlexpress;Initial Catalog=dbNetCoreCursoAPI;Integrated Security= true;");
            });
            var serviceProvider = services.BuildServiceProvider();
            testcontinente = serviceProvider.GetService<IContinenteData>();
        }

        [TestMethod]
        public void ObtenerContinentes()
        {
            //Arrage 
            var loggerMock = new Mock<ILogger<ContinenteController>>();
            ILogger<ContinenteController> logger = loggerMock.Object;

            ContinenteController obj = new ContinenteController(testcontinente, logger);
            //Act
            int result = obj.GetContinentes(1, 50).Count;

            //Assert
            Assert.IsTrue(result > 0);
        }
        [TestMethod]
        public void EliminarContinente()
        {
            //Arrage
            ContinenteController obj = new ContinenteController(testcontinente, _logger);
            Continente continente = new Continente();
            continente.IdContinente = 1;
            //Act
            bool result = obj.EliminarContinente(continente.IdContinente);

            //Assert
            Assert.IsTrue(result, "Continente no eliminado");
        }
    }
}

