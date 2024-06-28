using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlaceApp.Data;
using PlaceApp.Identity;

namespace PlaceAPI.Controllers
{
    public class UsersController : Controller
    {
        #region Field
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly IUserServices _userServices;

        public UsersController(UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser> signInManager, AppDbContext context, IUserServices userServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _userServices = userServices;
        }
        #endregion

        #region GetUser
        [HttpGet("getuser/{userid}")]
        public IActionResult GetUser([FromRoute(Name = "userid")] string userid)
        {
            try
            {
                if(userid != null)
                {
                    var user = _userServices.GetUser(userid);
                    if (user != null)
                    {
                        return Ok(new {success = true, user});
                    }
                    else
                    {
                        return NotFound(new {sucess = false, message = "Kullanıcı bulunamadı."});
                    }
                }
                else
                {
                    return BadRequest(new {success = false, message = "userid null"});
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new {success = false, message = ex.Message});
            }
        }
        #endregion

        #region GetUserWithPlaces
        [HttpGet("getuserwithplaces/{userid}")]
        public IActionResult GetUserWithPlaces([FromRoute(Name ="userid")] string userid) 
        {
            try
            {
                if(userid != null)
                {
                    var user = _userServices.GetUsersWithPlaces(userid);
                    if(user != null)
                    {
                        return Ok(new { success = true, user });
                    }
                    else
                    {
                        return NotFound(new {success = false, message ="kullanıcı bulunamadı."});
                    }
                }
                else
                {
                    return BadRequest(new {success = false, message = "userid null"});
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new {success = false, message = ex.Message});
            }
        }
        #endregion

        #region GetUserWithPlaceLists
        [HttpGet("getuserwithplacelists/{userid}")]
        public IActionResult GetUserWithPlaceLists([FromRoute(Name ="userid")] string userid)
        {
            try
            {
                if(userid != null)
                {
                    var user =_userServices.GetUsersWithPlaceLists(userid);
                    if( user != null)
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
                    return BadRequest(new {success = false, message = "userid null"});
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new {succes = false, message=ex.Message});
            }
        }
        #endregion

        //#region GetUsers
        //[HttpGet("getusers/{userid}")]
        //public IActionResult GetUsers([FromRoute(Name ="userid")] string userid)
        //{
        //    try
        //    {
        //        var user = _userServices.GetUsers(userid);
        //        return Ok(new {success = true, user});
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { success = false, message = ex.Message }); ;
        //    }
        //}
        //#endregion

        #region GetUsersWithPlaceAndPlaceLists
        [HttpGet("getuserpalceandplacelist/{userid}")]
        public IActionResult GetUsersWithPlaceAndPlaceLists([FromRoute(Name ="userid")] string userid)
        {
            try
            {
                if (userid != null)
                {
                    var user = _userServices.GetUserWithPlaceAndPlaceLists(userid);
                    if (user != null)
                    {
                        return Ok(new { success = true, user });
                    }
                    else
                    {
                        return NotFound(new { success = false, message = "Kullanıcı bulunamadı." });
                    }
                }
                else
                {
                    return BadRequest(new { success = false, message = "userid null" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { succes = false, message = ex.Message });
            }
        }
        #endregion
    }
}
    