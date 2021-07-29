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
    public class ColorController : Controller
    {
        // GET: Color
        public ActionResult Index()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ColorService(userId);
            var model = service.GetColors();
            return View(model);
        }

        // GET: Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ColorCreate model)
        {
            if (!ModelState.IsValid) return View(model);

            var service = CreateColorService();

            if (service.CreateColor(model))
            {
                TempData["SaveResult"] = "Your color was created.";
                return RedirectToAction("Index");
            };

            ModelState.AddModelError("", "Color could not be created.");

            return View(model);
        }

        public ActionResult Details(int id)
        {
            var svc = CreateColorService();
            var model = svc.GetColorById(id);

            return View(model);
        }

        private ColorService CreateColorService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ColorService(userId);
            return service;
        }

        public ActionResult Edit(int id)
        {
            var service = CreateColorService();
            var detail = service.GetColorById(id);
            var model =
                new ColorEdit
                {
                    ColorId = detail.ColorId,
                    Name = detail.Name
                };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ColorEdit model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.ColorId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var service = CreateColorService();

            if (service.UpdateColor(model))
            {
                TempData["SaveResult"] = "Your color was updated.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Your color could not be updated.");
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var svc = CreateColorService();
            var model = svc.GetColorById(id);

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var service = CreateColorService();

            service.DeleteColor(id);

            TempData["SaveResult"] = "Your color was deleted.";

            return RedirectToAction("Index");
        }
    }
}
