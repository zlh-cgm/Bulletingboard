using Bulletingboard.DTO.Auth;
using Bulletingboard.Requests.Auth;
using Bulletingboard.Services.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
        /// <summary>
        /// Get-Login view
        /// GET: /Auth/Login
        /// </summary>
        /// <returns></returns>
        public IActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// Post-User Login
        /// POST: /Auth/Login
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <param name="ReturnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
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
                TempData["ErrMsgForLogin"] = "Invalid Credentials";
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
       
            return RedirectToAction("Index", "Post");

        }
        /// <summary>
        /// Get-logout
        /// GET: /Auth/Logout
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Post");

        }
        /// <summary>
        /// Get-forgot password input form view
        /// GET: /Auth/ForgotPassword
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }
        /// <summary>
        /// Post-forgot password form
        /// POST: /Auth/ForgotPassword
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest forgotPasswordRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(forgotPasswordRequest);
                }
                await _authService.SendEmailAsync(forgotPasswordRequest.Email);
                TempData["SuccessMsgForEmail"] = "Result link has been send to your email successfully.";
                return View(forgotPasswordRequest);
            }
            catch (InvalidDataException ex)
            {
                TempData["ErrMsgForEmail"] = ex.Message;
                return View();
            }
            catch (Exception)
            {
                TempData["ErrMsgForEmail"] = "Theren's ERR while sending email.";
                return View();
            }
        }
        /// <summary>
        /// Get-Reset password input form view
        /// GET: /Auth/Reset
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Reset(string userId, string token)
        {
            try
            {
                int.TryParse(userId, out int id);
                await _authService.ValidateResetLinkAsync(id, token);
                return View(new ResetPasswordRequest() { Id = id });
            }
            catch (InvalidDataException ex)
            {
                TempData["ErrMsgForResetLink"]= ex.Message;
                return RedirectToAction("InvalidToken", "Auth");
            }
        }
        /// <summary>
        /// Post-reset password form
        /// POST: /Auth/Reset
        /// </summary>
        /// <returns></returns>
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
