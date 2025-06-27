using Microsoft.AspNetCore.Mvc;

namespace AnurStore.WebUI.Controllers
{
    public class DashboardData : Controller
    {
        public IActionResult Dashboard() 
        {
            return View(); 
        }
    }
}
