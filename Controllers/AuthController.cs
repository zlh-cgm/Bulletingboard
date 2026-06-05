using Bulletingboard.DTO.Auth;
using Bulletingboard.DTO.User;
using Bulletingboard.Entity;
using Bulletingboard.Requests.Auth;
using Bulletingboard.Services.Auth;
using Bulletingboard.Services.User;
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

        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest forgotPasswordRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(forgotPasswordRequest);
            }
            var result=await _authService.SendEmailAsync(forgotPasswordRequest.Email);
            if (!result)
            {
                TempData["ErrMsgForEmail"] = "There's no account link to this Email or There's ERROR while sending email";
                return View();
            }
            TempData["SuccessMsgForEmail"] = "Result link has been send to your email successfully.";
            return View(forgotPasswordRequest);
        }

        public async Task<IActionResult> Reset(string userId, string token)
        {
            int.TryParse(userId, out int id);
            var isValid=await _authService.ValidateResetLinkAsync(id,token);
            if (!isValid)
            {
                return RedirectToAction("InvalidToken", "Auth");
            }
            return View(new ResetPasswordRequest() {Id=id });
        }

        [HttpPost]
        public async Task<IActionResult> Reset(ResetPasswordRequest resetPasswordRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPasswordRequest);
            }
            await _authService.ResetPasswordAsync(new ResetPasswordDto(resetPasswordRequest));
            TempData["SuccessMsgForReset"] = "Password has been saved successfully.";
            return View();
        }
    }
}
