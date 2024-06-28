using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlaceApp.Data;
using PlaceApp.Identity;

namespace PlaceAPI.Controllers
{
    [Route("api/email")]
    [ApiController]
    public class EmailController : Controller
    {
        #region Field
        private readonly AppDbContext _context;
        private UserManager<AppIdentityUser> _userManager;
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private readonly IEmailServices _emailServices;


        public EmailController(AppDbContext context, UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser> signInManager, IEmailServices emailServices)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailServices = emailServices;
        }
        #endregion
        [HttpPost("sendemail")]
        public async Task<IActionResult> SendEmail(string to, string subject, string body)
        {
            try
            {
                var result = await _emailServices.SendEmailAsync(to, subject, body);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new {success = false, message = ex.Message});
            }
        }
    }
}
