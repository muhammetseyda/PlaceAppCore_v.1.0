using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlaceApp.Data;
using PlaceApp.Identity;
using PlaceApp.Models;
using PlaceApp.Models.Account;
using PlaceApp.Models.Profile;

namespace PlaceApp.Controllers
{
    public class ProfileController : Controller
    {
        #region Field
        private readonly AppDbContext _context;
        private UserManager<AppIdentityUser> _userManager;
        private readonly SignInManager<AppIdentityUser> _signInManager;

        public ProfileController(AppDbContext context, UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        #endregion

        #region Index
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region Details
        public IActionResult Details(string username)
        {
            var user = _context.Users.Where(x => x.Email == username).SingleOrDefault();


            return View(user);
        }
        #endregion

        #region Edit
        public IActionResult Edit(string username)
        {
            var user = _context.Users.Where(x => x.Email == username).SingleOrDefault();


            return View(user);
        }
        #endregion

        #region EditPost
        [HttpPost]
        public async Task<IActionResult> Edit(Users user)
        {
            if (ModelState.IsValid)
            {
                // Kullanıcıyı e-posta adresine göre bul
                var selectedUser = await _userManager.FindByEmailAsync(user.Email);
                var normalizeduser = _context.Users.Where(x => x.Email == user.Email).SingleOrDefault();
                if (selectedUser != null && normalizeduser != null)
                {
                    // AspNetUsers Kullanıcı bilgilerini güncelle
                    selectedUser.FirstName = user.FirstName;
                    selectedUser.LastName = user.LastName;
                    selectedUser.PhoneNumber = user.Phone;
                    selectedUser.Gender = user.Gender;
                    selectedUser.Email = user.Email;

                    //Users tablosundaki kullanıcıyı güncelleme
                    normalizeduser.FirstName = user.FirstName;
                    normalizeduser.LastName = user.LastName;
                    normalizeduser.Phone = user.Phone;
                    normalizeduser.Gender = user.Gender;
                    normalizeduser.Email = user.Email;

                    // Kullanıcıyı güncelle
                    var result = await _userManager.UpdateAsync(selectedUser);
                    _context.SaveChanges();

                    if (result.Succeeded )
                    {
                        // Başarılı bir şekilde güncellendi, istediğiniz işlemleri yapabilirsiniz
                        return RedirectToAction( "Details", "Profile", new {username = user.Email});
                    }
                    else
                    {
                        // Hata işlemlerini burada yapabilirsiniz
                        ModelState.AddModelError("ProfileError", "Kullanıcı güncelleme işlemi başarısız oldu.");
                    }
                }
                else
                {
                    // Kullanıcı bulunamadı
                    ModelState.AddModelError("ProfileError", "Kullanıcı bulunamadı.");
                }
            }

            // ModelState.IsValid false ise veya güncelleme başarısızsa aynı sayfayı tekrar göster
            return View(user);
        }
        #endregion

        #region ResetPassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        #endregion

        #region ChangePasswordPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var localuser = _context.Users.Where(x => x.Email == user.Email).SingleOrDefault();

                if (user == null || localuser == null)
                {
                    return RedirectToAction("Login");
                }
                if(localuser != null)
                {
                    localuser.Password = model.NewPassword;
                    _context.SaveChanges();

                }

                var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                if (changePasswordResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
        #endregion

        #region DeleteAccount
        [HttpPost]
        public async Task<IActionResult> DeleteAccount()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false });
            }

            // Kullanıcıyı silme işlemi
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                // Ayrıca Users tablosundan da kullanıcıyı sil
                var localUser = _context.Users.SingleOrDefault(u => u.Email == user.Email);
                if (localUser != null)
                {
                    _context.Users.Remove(localUser);
                    await _context.SaveChangesAsync();
                }
                await _signInManager.SignOutAsync();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
        #endregion

        #region Profile
        [HttpGet]
        public IActionResult Profile(string userId)
        {
            if (User.Identity.Name != null)
            {
                ProfileViewModel profileModel = new ProfileViewModel();
                if (userId == null)
                {
                    var user = _context.Users.Where(x => x.Email == User.Identity.Name).SingleOrDefault();
                    if(user != null)
                    {
                        profileModel.UserId = user.UserId;
                        profileModel.FirstName = user.FirstName;
                        profileModel.LastName = user.LastName;
                        profileModel.ImageUrl = "https://fastly.picsum.photos/id/237/200/300.jpg?hmac=TmmQSbShHz9CdQm0NkEjx1Dyh_Y984R9LpNrpvH2D_U";
                        profileModel.Places = _context.Places.Where(x => x.UserId == user.UserId).ToList();
                        profileModel.PlaceLists = _context.PlaceLists.Where(x => x.UserId == user.UserId).ToList();
                        profileModel.SharePlaceList = _context.SharePlaceList.Where(x => x.UserId == user.UserId).ToList();

                    }
                }
                else
                {
                    var user = _context.Users.Where(u => u.UserId == userId).SingleOrDefault();
                    if(user != null)
                    {
                        profileModel.UserId = user.UserId;
                        profileModel.FirstName = user.FirstName;
                        profileModel.LastName = user.LastName;
                        profileModel.ImageUrl = "https://fastly.picsum.photos/id/237/200/300.jpg?hmac=TmmQSbShHz9CdQm0NkEjx1Dyh_Y984R9LpNrpvH2D_U";
                        profileModel.Places = _context.Places.Where(x => x.UserId == user.UserId).ToList();
                        profileModel.PlaceLists = _context.PlaceLists.Where(x => x.UserId == user.UserId).ToList();
                        profileModel.SharePlaceList = _context.SharePlaceList.Where(x => x.UserId == user.UserId).ToList();
                    }
                }
                return View(profileModel);
            }
            else
            {
                return View();
            }
        }

        #endregion
    }

}

