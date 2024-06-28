using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PlaceApp.Models;
using PlaceApp.Identity;
using PlaceApp.Data;
using PlaceApp.Models.Account;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    #region Field
    private readonly UserManager<AppIdentityUser> _userManager;
    private readonly SignInManager<AppIdentityUser> _signInManager;
    private readonly AppDbContext _context;

    public AccountController(UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser> signInManager, AppDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }
    #endregion

    #region GetLoginUser
    [HttpGet("user")]
    public IActionResult GetLoginUser()
    {
        try
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user == null)
                return BadRequest(new {Message = "Kullanıcı bulunamadı."});
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(new { succes = false, error = ex.Message });
        }
    }
    #endregion

    #region GeUserByEmail
    [HttpGet("user/{email}")]
    public IActionResult GetUserByEmail([FromRoute(Name = "email")] string email)
    {
        try
        {
            var user = _userManager.FindByEmailAsync(email).Result;
            if (user == null)
                return BadRequest(new { Message = "Kullanıcı bulunamadı." });
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(new { succes = false, error = ex.Message });
        }
    }
    #endregion
    #region Register
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var aspnetuser = new AppIdentityUser
                {
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.Phone,
                    Gender = model.Gender
                };
                var user = new Users
                {
                    UserId = aspnetuser.Id,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Phone = model.Phone,
                    Password = model.Password,
                    Gender = model.Gender,
                };

                var result = await _userManager.CreateAsync(aspnetuser, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(aspnetuser, isPersistent: false);
                    _context.Users.Add(user);
                    _context.SaveChanges();
                    return Ok(model);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Ok(model);
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, error = ex.Message });
        }
    }
    #endregion

    #region Login
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {
        try
        {
            if(model.Email == null)
                return BadRequest(ModelState);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                return Ok(new { Message = "Login successful", UserId = user.Id });
            }

            if (result.IsLockedOut)
            {
                // Hesap kilitlenmiş durumda
                return BadRequest(new { Message = "Account is locked out" });
            }

            if (result.IsNotAllowed)
            {
                // Hesap doğrulama işlemine izin vermiyor
                return BadRequest(new { Message = "Account is not allowed to sign in" });
            }

            if (result.RequiresTwoFactor)
            {
                // İki faktörlü kimlik doğrulama gerekiyor
                return BadRequest(new { Message = "Two-factor authentication is required" });
            }

            return Unauthorized(new { Message = "Login failed" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, error = ex.Message });
        }
       
    }
    #endregion

    #region LogOut
    [HttpPost("Logout")]
    public async Task<IActionResult> Logout()
    {
        try
        {
            await _signInManager.SignOutAsync();
            return Ok(new { Message = "Logout successful" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, error = ex.Message });
        }
        
    }
    #endregion
}
