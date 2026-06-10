using Bulletingboard.DTO.Post;
using Bulletingboard.DTO.User;
using Bulletingboard.Requests.User;
using Bulletingboard.Services.Post;
using Bulletingboard.Services.User;
using Bulletingboard.ViewModels.User;
using CsvHelper;
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
    /// <summary>
    /// Get-All users list
    /// GET: /user/user-list
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = "Admin")]
    [HttpGet("")]
    [HttpGet("user-list")]
    public async Task<IActionResult> Index()
    {
        var users = await _userService.GetUserInfoListAsync();
        var viewModel = new UserViewModel { Response = users };
        return View(viewModel);
    }
    /// <summary>
    /// Get-Create new user form view
    /// GET: /user/regist-request
    /// </summary>
    /// <returns></returns>
    [HttpGet("regist-request")]
    public IActionResult Create()
    {
        return View(new UserRequest());
    }
    /// <summary>
    /// Post-new user
    /// POST: /user/registration
    /// </summary>
    /// <param name="userRequest"></param>
    /// <returns></returns>
    [HttpPost("registration")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveUser(UserRequest userRequest)
    {
        await AddDuplicateValidationErrors(userRequest);
        if (!ModelState.IsValid)
        {
            return View("Create", userRequest);
        }

        await _userService.AddUserAsync(new UserDto(userRequest));
        TempData["SuccessMsgForUser"] = "User has been saved successfully.";
        return RedirectToAction(nameof(Index));
    }
    /// <summary>
    /// Get-detail view of user
    /// GET: /user/detail/:id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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
    /// <summary>
    /// Get-User edit form view
    /// GET: /user/edit/:id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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
    /// <summary>
    /// Post-update edited user data
    /// POST: /user/edit
    /// </summary>
    /// <param name="userRequest"></param>
    /// <returns></returns>
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
    /// <summary>
    /// Get-Change password form view
    /// GET: /user/ChangePassword/:id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("ChangePassword/{id:int}")]
    public async Task<IActionResult> ChangePassword(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user is null)
        {
            TempData["ErrorMsg"] = "User was not found.";
            return RedirectToAction(nameof(Index));
        }

        int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId);
        if (userId != user.Id)
        {
            return Forbid();
        }

        return View(new ChangePasswordRequest() { Id=userId});
    }
    /// <summary>
    /// Post-update password
    /// POST: /user/ChangePassword
    /// </summary>
    /// <param name="changePasswordRequest"></param>
    /// <returns></returns>
    [HttpPost("ChangePassword")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest changePasswordRequest)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(changePasswordRequest);
            }

            await _userService.ChangePasswordAsync(changePasswordRequest);

            TempData["SuccessMsgForChangePassword"] = "Password has been updated successfully.";
            return RedirectToAction(nameof(Detail), new { id = changePasswordRequest.Id });
        }
        catch (InvalidDataException ex)
        {
            TempData["ErrMsgForChangePassword"] = ex.Message;
            changePasswordRequest.ClearAllField();
            return View(changePasswordRequest);
        }
        catch (Exception ex)
        {
            TempData["ErrMsgForChangePassword"] = "Something Wrong While Changing Password!";
            return View(changePasswordRequest);
        }
    }
    /// <summary>
    /// Delete-A user
    /// DELETE: /user/delete/:id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _userService.DeleteUserAsync(id);
        TempData["SuccessMsgForUser"] = "User has been deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
    /// <summary>
    /// Get-User list csv upload form view
    /// GET: /user/upload
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = "Admin")]
    [HttpGet("upload/")]
    public async Task<IActionResult> UploadCSV()
    {
        return View();
    }
    /// <summary>
    /// Post-Upload user list csv
    /// POST: /user/upload
    /// </summary>
    /// <param name="uploadCSVRequest"></param>
    /// <returns></returns>
    [HttpPost("upload/")]
    public async Task<IActionResult> UploadCSV(UploadCSVRequest uploadCSVRequest)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _userService.UploadUserListCSVAsync(uploadCSVRequest);
            TempData["SuccessMsgForUpload"] = "User list has been upload successfully.";
            return View();
        }
        catch (HeaderValidationException ex)
        {
            TempData["ErrMsgForUpload"] = "Invalid CSV Header!";
            return View();
        }
        catch (InvalidDataException ex)
        {
            TempData["ErrMsgForUpload"] = ex.Message;
            return View();
        }
        catch (Exception ex)
        {
            TempData["ErrMsgForUpload"] = $"Something wrong while uploading user list.--{ex}";
            return View();
        }
    }
    /// <summary>
    /// Get-Download all users list csv
    /// GET: /user/download-user-list
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = "Admin")]
    [HttpGet("download-user-list")]
    public async Task<IActionResult> DownloadCSV()
    {
        var fileBytes=await _userService.DownloadUserListCSVAsync();
        return File(fileBytes, "text/csv", $"User_list_{DateTime.Now:yyyy-MM-dd HH:mm}.csv");
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
