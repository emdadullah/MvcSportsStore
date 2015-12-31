using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;

namespace WebUI.Controllers
{
    public class NavController : Controller
    {
        //
        // GET: /Nav/

        private IProductRepository repository;

        public NavController(IProductRepository repo) {
            repository = repo;
        }

        public PartialViewResult Menu(string category)
        {
            IEnumerable<string> categories = repository.Products
                                                       .Select(x => x.Category)
                                                       .Distinct()
                                                       .OrderBy(x => x);
            ViewBag.SelectedCategory = category;
            return PartialView(categories);
        }
    }
}
