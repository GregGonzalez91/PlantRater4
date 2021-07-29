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
    public class TreeController : Controller
    {
        // GET: Tree
        public ActionResult Index()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new TreeService(userId);
            var model = service.GetTrees();
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
        public ActionResult Create(TreeCreate model)
        {
            if (!ModelState.IsValid) return View(model);

            var service = CreateTreeService();

            if (service.CreateTree(model))
            {
                TempData["SaveResult"] = "Your tree was created.";
                return RedirectToAction("Index");
            };

            ModelState.AddModelError("", "Tree could not be created.");

            return View(model);
        }

        public ActionResult Details(int id)
        {
            var svc = CreateTreeService();
            var model = svc.GetTreeById(id);

            return View(model);
        }

        private TreeService CreateTreeService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new TreeService(userId);
            return service;
        }

        public ActionResult Edit(int id)
        {
            var service = CreateTreeService();
            var detail = service.GetTreeById(id);
            var model =
                new TreeEdit
                {
                    TreeId = detail.TreeId,
                    Name = detail.Name
                };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TreeEdit model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.TreeId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var service = CreateTreeService();

            if (service.UpdateTree(model))
            {
                TempData["SaveResult"] = "Your tree was updated.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Your tree could not be updated.");
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var svc = CreateTreeService();
            var model = svc.GetTreeById(id);

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var service = CreateTreeService();

            service.DeleteTree(id);

            TempData["SaveResult"] = "Your tree was deleted.";

            return RedirectToAction("Index");
        }
    }
}
