using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlaceApp.Data;
using PlaceApp.Identity;
using PlaceApp.Models;
using PlaceApp.Models.Account;

namespace PlaceApp.Controllers
{
    public class AccountController : Controller
    {
        #region Field
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private UserManager<AppIdentityUser> _userManager;
        private readonly AppDbContext _context;

        public AccountController(SignInManager<AppIdentityUser> signInManager, UserManager<AppIdentityUser> userManager, AppDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }
        #endregion

        #region Login
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        #endregion

        #region LoginPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if( user == null)
            {
                ViewData["LoginError"] = "Böyle bir kullanıcı bulunamadı.";
            }
            if( user != null) { 
                var gender = _context.Users.Where(x => x.Email == model.Email).Select(x => x.Gender).SingleOrDefault();
                HttpContext.Session.SetString("GENDER", gender);
                if (user == null)
                {

                    ModelState.AddModelError(String.Empty, "E-posta adresiniz ve/veya şifreniz hatalı.");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (!result.Succeeded)
                {
                    ViewData["LoginError"] = "Şifre veya Kullanıcı Adı Yanlış";
                }
                if (result.Succeeded)
                {
                    if (returnUrl == null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        //string ReturnUrl = Convert.ToString(Request.["model.ReturnUrl"]);
                        return Redirect(model.ReturnUrl);

                    }
                //return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError(string.Empty, "Geçersiz giriş denemesi.");
            return View();
        }
        #endregion

        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        #endregion

        #region RegisterPost
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {

            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
            if (ModelState.IsValid)
            {
                var aspnetuser = new AppIdentityUser { 
                    UserName = model.Email, 
                    FirstName = model.FirstName, 
                    LastName = model.LastName, 
                    Email = model.Email ,
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
                    return RedirectToAction("Index", "Home");
                }
               
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
        #endregion

        #region Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); // Kullanıcıyı oturumdan çıkar
            return RedirectToAction("Index", "Home"); // Çıkış yapıldıktan sonra yönlendirilecek sayfa
        }
        #endregion
    }
}

