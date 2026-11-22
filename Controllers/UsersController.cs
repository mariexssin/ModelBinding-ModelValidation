using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProductsValidation.Models;
using ProductsValidation.Services;

namespace ProductsValidation.Controllers
{
    public class UsersController : Controller
    {
        private List<User> users;

        public UsersController(Data data)
        {
            users = data.Users;
        }

        public IActionResult Index(string id)
        {
            return View("Index", users);
        }

        public IActionResult View(int id)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult Create([FromQuery] User user)
        {
            return View(user ?? new User());
        }

        [HttpPost]
        public IActionResult CreatePost([FromQuery] User user)
        {
            if (user != null)
            {
                users.Add(user);
            }
            return View("View", user);
        }
    }
}