using lab6.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json; 

namespace lab6.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View("~/Views/Home/Register.cshtml"); 
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (user.Age < 16)
            {
                ModelState.AddModelError("", "Вам повинно бути більше 16 років.");
                return View("~/Views/Home/Register.cshtml", user); 
            }

            return RedirectToAction("Order", new { userId = user.Id });
        }

        [HttpGet]
        public IActionResult Order(int userId)
        {
            ViewBag.UserId = userId;
            return View("~/Views/Home/Order.cshtml");
        }

        [HttpPost]
        public IActionResult Order(int userId, int quantity)
        {
            if (quantity <= 0)
            {
                ModelState.AddModelError("", "Кількість повинна бути додатньою.");
                return View("~/Views/Home/Order.cshtml");
            }

            return RedirectToAction("ProductForm", new { userId, quantity });
        }

        [HttpGet]
        public IActionResult ProductForm(int userId, int quantity)
        {
            ViewBag.Quantity = quantity;
            ViewBag.UserId = userId;
            return View("~/Views/Home/ProductForm.cshtml");
        }

        [HttpPost]
        public IActionResult ProductForm(List<Product> products)
        {
            TempData["Products"] = JsonConvert.SerializeObject(products);

            return RedirectToAction("Summary");
        }

        public IActionResult Summary()
        {
            var productsJson = TempData["Products"] as string;
            var products = JsonConvert.DeserializeObject<List<Product>>(productsJson);

            return View("~/Views/Home/Summary.cshtml", products); 
        }
    }
}
