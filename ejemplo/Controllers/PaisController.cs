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
    public class PaisController : Controller
    {
        public ILogger<PaisController> _logger { get; }
        private readonly IPaisData paisData;

       
        private IPaisData _paisdata;

        public PaisController(IPaisData paisData, ILogger<PaisController> logger)

        {
            _logger = logger;
            _paisdata = paisData;
            this.paisData = paisData;
        }

        // GET: PaisController
        public ActionResult Index()
        {
            return View(paisData.GetPaises(1, 30));
        }

        // .GetPaises para test
        public List<Pais> GetPaises(int page, int limit)
        {
            return _paisdata.GetPaises(page, limit);
        }

        //GET: PaisController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaisController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Pais pais = new Pais();
                pais.NombrePais = collection["NombrePais"].ToString();
                pais.IdContinente = Int32.Parse(collection["IdContinente"].ToString());
                
                _paisdata.InsertPais(pais);
                _logger.LogInformation("Pais Creado");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                string exception = ex.Message;
                return View();
            }
        }

        // GET: PaisController/Edit/5
        public ActionResult Edit(int id)
        {
            Pais pais = new Pais();
            pais = _paisdata.GetPais(id);
            return View(pais);
        }

        // POST: PaisController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                Pais pais = new Pais();
                pais.IdPais = id;
                pais.NombrePais = collection["NombrePais"].ToString();
                pais.IdContinente = Int32.Parse(collection["IdContinente"].ToString());
                
                _paisdata.UpdatePais(id, pais);
                _logger.LogInformation("Pais Editado");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PaisController/Delete/5
        public ActionResult Delete(int id)
        {
            Pais pais = new Pais();
            pais = _paisdata.GetPais(id);
            return View(pais);
        }

        // POST: PaisController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                
                _paisdata.DeletePais(id);
                _logger.LogInformation("Pais Eliminado");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View();
            }
        }
        // metodo para probar eliminar en el test
        public bool EliminarPais(int id)
        {
            bool result;
            try
            {
                _paisdata.DeletePais(id);
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
