using AutoMapper;
using Building.BLL.Services.Implementations;
using Building.BLL.Services.Interfaces;
using Building.Domain;
using Building.Domain.DTO;
using Building.Domain.Entity;
using Building.Domain.Response;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace Building.Controllers
{
    public class Authentication : Controller
    {
        private readonly ILoginService loginService;
        private readonly IMapper mapping;
        public Authentication(ILoginService loginService, IMapper mapping)
        {
            this.loginService = loginService;
            this.mapping = mapping;
        }
        public IActionResult Index()
        {
            var auth = new LoginViewModel();
            return View(auth);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await loginService.Login(model);
                
                if(response.StatusCode==Domain.Enum.StatusCode.OK)
                {
                    
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(response.Data));

                    var role = response.Data.FindFirst(ClaimsIdentity.DefaultRoleClaimType).Value;


                    //new { id = Convert.ToInt32(response.Data.FindFirst(ClaimTypes.NameIdentifier).Value) }
                    if (role == "Прораб")
                    {
                        return RedirectToAction("Main", "Prorab");
                    }
                    if (role == "Снабженец")
                    {
                        return RedirectToAction("Main", "Snab");
                    }
                    if (role == "Директор")
                    {
                        return RedirectToAction("Main", "Director");
                    }
                }
                ModelState.AddModelError("Error", response.Description);

            }
            
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }
        
        
    }
}
