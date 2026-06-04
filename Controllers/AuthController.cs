using Bulletingboard.DTO.Auth;
using Bulletingboard.DTO.User;
using Bulletingboard.Entity;
using Bulletingboard.Requests.Auth;
using Bulletingboard.Services.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bulletingboard.Controllers
{
    public class AuthController:Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest, string ReturnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(loginRequest);
            }

            var dto = new LoginDto
            {
                Email = loginRequest.Email,
                Password = loginRequest.Password
            };
            var loginResult = await _authService.LoginAsync(dto);

            if (loginResult is null)
            {
                ModelState.AddModelError("InvalidLogin", "Invalid Credentials");
                loginRequest.Password = "";
                return View(loginRequest);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, loginResult.UserId.ToString()),
                new Claim(ClaimTypes.Name, loginResult.Username),
                new Claim(ClaimTypes.Role, loginResult.Role)
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);

            Console.WriteLine(ReturnUrl);
            if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            {
                return LocalRedirect(ReturnUrl);
            }

            return RedirectToAction("Index", "Post");

        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Post");

        }

    }
}
