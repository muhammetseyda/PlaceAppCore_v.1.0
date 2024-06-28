using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PlaceApp.Data;
using PlaceApp.Identity;
using PlaceApp.Models;
using PlaceApp.Models.Account;
using PlaceApp.Models.Profile;

namespace PlaceAPI.Controllers
{
    [Route("api/profile")]
    [ApiController]
    public class ProfileController : Controller
    {
        #region Field
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private UserManager<AppIdentityUser> _userManager;
        private readonly AppDbContext _context;

        public ProfileController(SignInManager<AppIdentityUser> signInManager, UserManager<AppIdentityUser> userManager, AppDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }
        #endregion

        #region Details
        [HttpGet("details/{userid}")]
        public IActionResult Details([FromRoute(Name = "userid")] string userid)
        {
            try
            {
                if (userid != null)
                {
                    var user = _context.Users.SingleOrDefault(x => x.UserId == userid);
                    if(user != null)
                    {
                        return Ok(new {success = true, user});
                    }
                    else
                    {
                        return NotFound(new {success = false, message = "Kullanıcı bulunamadı."});
                    }
                }
                else
                {
                    return BadRequest(new { success = false, message = "userid == null" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new {success = false, message = ex.Message});
            }
        }
        #endregion

        #region Edit Profile
        [HttpPost]
        public async Task<IActionResult> EditProfile([FromBody] Users user)
        {
            try
            {
                if(user != null)
                {
                    var selecteduser = await _userManager.FindByIdAsync(user.UserId);
                    if (selecteduser == null)
                        return NotFound(new {success = false, message ="usermanager userı bulamadı."});
                    var selecteduser2 = _context.Users.SingleOrDefault(x => x.UserId == user.UserId);
                    if (selecteduser2 == null)
                        return NotFound(new { success = false, message = "_context userı bulamadı" });

                    selecteduser.FirstName = user.FirstName;
                    selecteduser.LastName = user.LastName;
                    selecteduser.PhoneNumber = user.Phone;
                    selecteduser.Gender = user.Gender;
                    selecteduser.Email = user.Email;

                    selecteduser2.FirstName = user.FirstName;
                    selecteduser2.LastName = user.LastName;
                    selecteduser2.Phone = user.Phone;
                    selecteduser2.Gender = user.Gender;
                    selecteduser2.Email = user.Email;

                    var result = await _userManager.UpdateAsync(selecteduser);
                    _context.SaveChanges();

                    if (result.Succeeded)
                    {
                        return Ok(new {success = true, message = "Profiliniz başarıyla güncellendi.", selecteduser,selecteduser2});
                    }
                    
                        return BadRequest(new {success = false, message="Profiliniz güncellenemedi."});


                }
                else
                {
                    return BadRequest(new { success = false, message = "user == null" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new {success = false, message = ex.Message});
            }
        }
        #endregion

        #region ChangePassword
        [HttpPost("changepassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel modal, [FromQuery] string userid)
        {
            try
            {
                if(userid != null)
                {
                    var user = await _userManager.FindByIdAsync(userid);
                    if (user == null)
                        return NotFound(new {success = false, message = "user null"});
                    var normaluser = _context.Users.SingleOrDefault(x => x.UserId == userid);
                    if (normaluser == null)
                        return BadRequest(new { success = false, message = "normal user == null" });

                    normaluser.Password = modal.NewPassword;
                    var result = await _userManager.ChangePasswordAsync(user, modal.OldPassword, modal.NewPassword);

                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _context.SaveChanges();
                        return Ok(new { success = true, message = "Şifre başarıyla değiştirildi." });
                    }
                    return BadRequest(new {success = false, message = result.Errors});
                }
                else
                {
                    return BadRequest(new {success = false, message = "userid == null"});
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new {success = false, message = ex.Message});
            }
        }
        #endregion

        #region ChangeEmail
        [HttpPost("changeemail/{userid}")]
        public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailViewModel model, [FromRoute(Name ="userid")]string userid)
        {
            try
            {
                if(model != null)
                {   
                    var user = await _userManager.FindByIdAsync(userid);
                    var localuser = _context.Users.SingleOrDefault(x => x.UserId == userid);
                    if(user != null)
                    {
                        await _userManager.SetEmailAsync(user, model.NewEmail);
                        await _userManager.SetUserNameAsync(user, model.NewEmail);
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        await _userManager.ConfirmEmailAsync(user, token);
                        if(localuser != null)
                            localuser.Email = model.NewEmail;
                        _context.SaveChanges();
                        return Ok(new {success = true, localuser});
                    }
                    else
                    {
                        return BadRequest(new {success = false, message = "Kullanıcı bulunamadı."});
                    }
                }
                return BadRequest(new {success = false, message = "ChangeEmailViewModel == null."});
            }
            catch (Exception ex)
            {
                return BadRequest(new {success = false, message = ex.Message});
            }
        }
        #endregion

        #region ChangePhoneNumber
        [HttpPost("changephonenumber/{userid}")]
        public async Task<IActionResult> ChangePhoneNumber([FromBody] ChangePhoneNumber model, [FromRoute(Name = "userid")] string userid)
        {
            try
            {
                if (model != null)
                {
                    var user = await _userManager.FindByIdAsync(userid);
                    var localuser = _context.Users.SingleOrDefault(x => x.UserId == userid);
                    if (user != null)
                    {
                        await _userManager.SetPhoneNumberAsync(user, model.NewPhoneNumber);
                        var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user,model.NewPhoneNumber);
                        await _userManager.IsPhoneNumberConfirmedAsync(user);
                        if (localuser != null)
                            localuser.Phone = model.NewPhoneNumber;
                        _context.SaveChanges();
                        return Ok(new { success = true, localuser });
                    }
                    else
                    {
                        return BadRequest(new { success = false, message = "Kullanıcı bulunamadı." });
                    }
                }
                return BadRequest(new { success = false, message = "ChangeEmailViewModel == null." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region DeleteUser
        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromQuery] string userid)
        {
            try
            {
                if(userid != null)
                {
                    var user = await _userManager.FindByIdAsync(userid);
                    if(user == null)
                        return NotFound(new { success = false, message = "user null"});
                    var normaluser = _context.Users.SingleOrDefault(x => x.UserId == userid);
                    if (normaluser == null)
                        return NotFound(new {success = false, message = "normaluser == null"});

                    var result = await _userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        _context.Users.Remove(normaluser);
                        _context.SaveChanges();
                    }
                    await _signInManager.SignOutAsync();
                    return Ok(new {success = true, message = "Kullanıcı başarılı bir şekilde silindi."});
                }
                else
                {
                    return BadRequest(new {success = false, message = "userid == null"});
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new {success = false, message = ex.Message});
            }
        }
        #endregion
    }
}
