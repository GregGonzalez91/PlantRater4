using Microsoft.AspNet.Identity;
using PlantRater.Models;
using PlantRater.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlantRater.Controllers
{
    [Authorize]
    public class PlantController : Controller
    {
        // GET: Plant
        public ActionResult Index()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new PlantService(userId);
            var model = service.GetPlants();
            return View(model);
        }

        // GET: Create
        public ActionResult Create()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            ViewBag.ColorList = new ColorService(userId).GetColors();
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlantCreate model)
        {
            if (!ModelState.IsValid) return View(model);

            var service = CreatePlantService();

            if (service.CreatePlant(model))
            {
                TempData["SaveResult"] = "Your Plant was created.";
                return RedirectToAction("Index");
            };

            ModelState.AddModelError("", "Plant could not be created.");

            return View(model);
        }

        public ActionResult Details(int id)
        {
            var svc = CreatePlantService();
            var model = svc.GetPlantById(id);

            return View(model);
        }

        private PlantService CreatePlantService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new PlantService(userId);
            return service;
        }

        public ActionResult Edit(int id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            ViewBag.ColorList = new ColorService(userId).GetColors();
            var service = CreatePlantService();
            var detail = service.GetPlantById(id);
            var model =
                new PlantEdit
                {
                    PlantId = detail.PlantId,
                    Name = detail.Name,
                    ColorId = detail.ColorId,
                    Rating = detail.Rating
                };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PlantEdit model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.PlantId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var service = CreatePlantService();

            if (service.UpdatePlant(model))
            {
                TempData["SaveResult"] = "Your Plant was updated.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Your Plant could not be updated.");
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var svc = CreatePlantService();
            var model = svc.GetPlantById(id);

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var service = CreatePlantService();

            service.DeletePlant(id);

            TempData["SaveResult"] = "Your Plant was deleted.";

            return RedirectToAction("Index");
        }
    }
}