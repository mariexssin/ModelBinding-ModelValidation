using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProductsValidation.Models;
using ProductsValidation.Services;

namespace ProductsValidation.Controllers
{
    public class ProductsController : Controller
    {
        private List<Product> myProducts;

        public ProductsController(Data data)
        {
            myProducts = data.Products;
        }

        public IActionResult Index(int filterId, string filtername)
        {
            return View(myProducts);
        }

        public IActionResult View(int id)
        {
            Product prod = myProducts.Find(prod => prod.Id == id);
            if (prod != null)
            {
                return View(prod);
            }

            return View("NotExists");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Product prod = myProducts.Find(prod => prod.Id == id);
            if (prod != null)
            {
                return View(prod);
            }

            return View("NotExists");
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            int index = myProducts.FindIndex(prod => prod.Id == product.Id);
            if (index != -1)
            {
                myProducts[index] = product;
            }

            return View("View", product);
        }

        [HttpGet]
        public IActionResult Create()
        {
            int nextId = myProducts.Any() ? myProducts.Max(p => p.Id) + 1 : 1;
            return View(new Product() { Id = nextId });
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            myProducts.Add(product);
            return View("View", product);
        }

        [HttpGet]
        public IActionResult EditPrices(Product.Category category)
        {
            ViewBag.Category = category;
            var productsByCategory = myProducts.Where(p => p.Type == category).ToList();
            return View(productsByCategory);
        }

        [HttpPost]
        public IActionResult EditPrices(List<Product> products, Product.Category category)
        {
            ViewBag.Category = category;

            if (!ModelState.IsValid)
            {
                return View(products ?? new List<Product>());
            }

            if (products != null)
            {
                foreach (var updated in products)
                {
                    var existing = myProducts.FirstOrDefault(p => p.Id == updated.Id);
                    if (existing != null)
                    {
                        existing.Price = updated.Price;
                    }
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var prod = myProducts.Find(product => product.Id == id);
            if (prod != null)
            {
                myProducts.Remove(prod);
            }
            return View("Index", myProducts);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}