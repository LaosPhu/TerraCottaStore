using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TerraCottaStore.Models;
using TerraCottaStore.Repository;

namespace TerraCottaStore.Controllers
{
    public class HomeController : Controller
    {
		private readonly DataContext _datacontext;
		private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,DataContext dataContext)
        {
            _datacontext = dataContext;
            _logger = logger;
        }

        public IActionResult Index(int pg =1)
        {   
            var products = _datacontext.Products.Include("Category").Include("Brand").Where(x=>x.status==1).ToList();
            const int pageSize = 12; //10 items/trang

            if (pg < 1) //page < 1;
            {
                pg = 1; //page ==1
            }
            int recsCount = products.Count();

            var pager = new Paginate(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            var data = products.Skip(recSkip).Take(pager.PageSize).ToList();

            ViewBag.Pager = pager;

            return View(data);
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statuscode)
        {
            if (statuscode == 404)
            {
                return View("NotFound");
            }
            else
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
           
        }
    }
}
