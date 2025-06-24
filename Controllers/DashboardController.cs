using FerroShop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FerroShop.Controllers
{
    [Authorize(Policy = "RequiredAdminorStaff")]
    public class DashboardController : BaseController
    {
        public DashboardController(ApplicationDbContext context)
         : base(context) { }

        public IActionResult Index()
        {
            return View();
        }
    }
}
