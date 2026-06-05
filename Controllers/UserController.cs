using Bulletingboard.DTO.Post;
using Bulletingboard.DTO.User;
using Bulletingboard.Entity;
using Bulletingboard.Requests.User;
using Bulletingboard.Services.Post;
using Bulletingboard.Services.User;
using Bulletingboard.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bulletingboard.Controllers;

[Route("user")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IPostService _postService;

    public UserController(IUserService userService,IPostService postService)
    {
        _userService = userService;
        _postService = postService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("")]
    [HttpGet("user-list")]
    public async Task<IActionResult> Index()
    {
        var users = await _userService.GetUserInfoListAsync();
        var viewModel = new UserViewModel { Response = users };
        return View(viewModel);
    }


    [HttpGet("regist-request")]
    public IActionResult Create()
    {
        return View(new UserRequest());
    }

    [HttpPost("registration")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveUser(UserRequest userRequest)
    {
        await AddDuplicateValidationErrors(userRequest);
        if (!ModelState.IsValid)
        {
            return View("Create", userRequest);
        }

        if (!User.IsInRole("Admin"))
        {
            userRequest.Role = 2;
        }

        await _userService.AddUserAsync(new UserDto(userRequest));
        TempData["SuccessMsgForUser"] = "User has been saved successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("detail/{id:int}")]
    public async Task<IActionResult> Detail(int id)
    {
        int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int cookieUserId);

        //Admin can access all detail page
        if (!User.IsInRole("Admin") && cookieUserId != id)
        {
            return Forbid();
        }
        var user = await _userService.GetUserByIdAsync(id);
        if (user is null)
        {
            TempData["ErrorMsg"] = "User was not found.";
            return RedirectToAction(nameof(Index));
        }

        List<PostDto> posts;
        if (user.Id == cookieUserId)
        {
            posts = await _postService.GetPostByUserIdAsync(id);
        }
        else
        {
            posts = await _postService.GetPublicPostByUserIdAsync(id);
        }

        return View(new UserDetailViewModel { User = user,Posts=posts });
    }

    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> EditionRequest(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user is null)
        {
            TempData["ErrorMsg"] = "User was not found.";
            return RedirectToAction(nameof(Index));
        }
        int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId);


        if (!User.IsInRole("Admin") && userId != user.Id)
        { 
            return Forbid();
        }

        return View("Edit", new UserRequest(user));
    }

    [HttpPost("edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(UserRequest userRequest)
    {
        await AddDuplicateValidationErrors(userRequest);
        if (!ModelState.IsValid)
        {
            return View("Edit", userRequest);
        }

        await _userService.UpdateUserAsync(new UserDto(userRequest));
        TempData["SuccessMsgForUser"] = "User has been updated successfully.";
        return RedirectToAction(nameof(Detail), new { id = userRequest.Id });
    }

    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _userService.DeleteUserAsync(id);
        TempData["SuccessMsgForUser"] = "User has been deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    private async Task AddDuplicateValidationErrors(UserRequest userRequest)
    {
        if (await _userService.CheckDuplicateUserAsync(userRequest, "name"))
        {
            ModelState.AddModelError(nameof(UserRequest.Name), "User name already exists.");
        }

        if (await _userService.CheckDuplicateUserAsync(userRequest, "email"))
        {
            ModelState.AddModelError(nameof(UserRequest.Email), "Email already exists.");
        }
    }
}
