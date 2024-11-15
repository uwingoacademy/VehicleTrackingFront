using Frontend.Helper;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace Frontend.Controllers
{
    public class AuthenticationController : Controller
    {


        public AuthenticationController()
        {
            
        }

        public async Task<IActionResult> Login() {
            Extancion.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "tokens.AccessToken");
            return View();
        }

        public async Task<IActionResult> LogOut() {
            return RedirectToAction("Login", "Authentication");
        }

    }
}