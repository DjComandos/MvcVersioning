using System.Web.Mvc;
using MvcApplicationWithVersioning.Models;

namespace MvcApplicationWithVersioning.Controllers
{
    public class AuthenticationController : Controller
    {
        //
        // GET: /Authentication/

        public ActionResult ShowLogin()
        {
            return View(new Login());
        }

        [HttpPost]
        public ActionResult Login(Login model)
        {
            if(!string.IsNullOrEmpty(model.Password) 
                && model.Password == "qwerty") //hella strange business logic
            {
                return View(model);
            }
            return RedirectToAction("LoginError", model);
        }


        public ActionResult LoginError(Login model)
        {
            return View(model);
        }
    }
}
