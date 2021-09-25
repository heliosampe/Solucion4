using DataAccess.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ejemplo.Controllers
{
    public class ContinenteController : Controller
    {
        public ILogger<ContinenteController> _logger { get; }
        private readonly IContinenteData continenteData;


        private IContinenteData _continentedata;  
        public ContinenteController(IContinenteData continenteData, ILogger<ContinenteController> logger)

        {
            _continentedata = continenteData;
            this.continenteData = continenteData;
            _logger = logger;
        }

        // GET: ContinenteController
        public ActionResult Index()
        { return View(continenteData.GetContinentes(1, 50));
        }
        // .GetContinentes para test
        public List<Continente> GetContinentes(int page, int limit)
        {
            return _continentedata.GetContinentes(page, limit);
        }

        // GET: ContinenteController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContinenteController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Continente continente = new Continente();
                continente.NombreContinente = collection["NombreContinente"].ToString();

                if (collection["Activo"].Contains("true"))
                {
                    continente.Activo = true;
                }

                _continentedata.InsertContinente(continente);
                _logger.LogInformation("Continente creado ");
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                string exception = ex.Message;
                _logger.LogError("Error al crear Continente" + exception);

                return View();
            }
        }


        // GET: ContinenteController/Edit/5
        public ActionResult Edit(int id)
        {
            Continente continente = new Continente();
            continente = _continentedata.GetContinente(id);
            return View(continente);
        }

        // POST: ContinenteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {

                Continente continente = new Continente();
                continente.IdContinente = id;
                continente.NombreContinente = collection["NombreContinente"].ToString();
                if (collection["Activo"].Contains("true"))
                {
                    continente.Activo = true;
                }


                _continentedata.UpdateContinente(id, continente);
                _logger.LogInformation("Continente Editado");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrio un error" + ex.InnerException);
                return View();
            }
        }

        // GET: ContinenteController/Delete/5
        public ActionResult Delete(int id)
        {
            Continente continente = new Continente();
            continente = _continentedata.GetContinente(id);
            return View(continente);
        }

        // POST: ContinenteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {

                _continentedata.DeleteContinente(id);
                _logger.LogInformation("Continente Eliminado");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View();
            }
        }
        // metodo para probar eliminar en el test
        public bool EliminarContinente(int id)
        {
            bool result;
            try
            {
                _continentedata.DeleteContinente(id);
                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError("No se pudo eliminar:" + ex.InnerException);
                result = false;
            }
            return result;
        }
    }
}
