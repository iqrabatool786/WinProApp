using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WinProApp.ViewModels.Warehouse;


namespace WinProApp.Controllers
{
    [Authorize(Roles = "Administrator,Finance")]
    public class FinanceController : BasedUserController
    {
        // GET: FinanceController
        public ActionResult Index()
        {
            return View();
        }

        // GET: FinanceController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FinanceController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FinanceController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FinanceController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FinanceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FinanceController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FinanceController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
