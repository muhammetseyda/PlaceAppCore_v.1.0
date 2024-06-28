using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlaceApp.Data;
using PlaceApp.Identity;
using PlaceApp.Models;

namespace PlaceAPI.Controllers
{
    [Route("api/Places")]
    [ApiController]
    public class PlaceController : ControllerBase
    {
        #region Field
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private UserManager<AppIdentityUser> _userManager;
        private readonly AppDbContext _context;
        private readonly IPlaceService _placeService;

       
        public PlaceController(SignInManager<AppIdentityUser> signInManager, UserManager<AppIdentityUser> userManager, AppDbContext context, IPlaceService placeService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _placeService = placeService;

        }
        #endregion

        #region GetAllPlace
        [HttpGet]
        public IActionResult GetAllPlace()
        {
            var places = _context.Places.ToList();
            return Ok(places);
        }
        #endregion

        #region GetAllPlacesByUser
        [HttpGet("getallplacesbyuser")] 
        public IActionResult GetAllPlaceByUser()
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                if (user == null)
                    return BadRequest(new {Message = "Giriş Yapınız."});

                var places = _context.Places.Where(x => x.UserId == user.Id).ToList();
                if (places == null)
                    return NotFound();

                return Ok(places);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetAllPlacesByUser
        [HttpGet("byUserId/{userid}")]
        public IActionResult GetAllPlaceByUserId([FromRoute(Name =  "userid")] string userid)
        {
            try
            {
                var user = _userManager.FindByIdAsync(userid).Result;
                if (user == null)
                    return BadRequest(new { Message = "Giriş Yapınız." });

                var places = _context.Places.Where(x => x.UserId == user.Id).ToList();
                if (places == null)
                    return NotFound();

                return Ok(places);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetOnePlace
        [HttpGet("{id:int}")]
        public IActionResult GetOnePlace([FromRoute(Name = "id")] int id)
        {
            try
            {
                var place = _context.Places.Where(x => x.Id.Equals(id)).SingleOrDefault();
                if (place == null)
                    return NotFound(); //404

                return Ok(place);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
        #endregion

        #region GetOnePlaceByUser
        [HttpGet("getoneplacebyuser/{id:int}")]
        public IActionResult GetOnePlaceByUser([FromRoute(Name = "id")] int id)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                if (user == null)
                    return BadRequest(new { Message = "Giriş Yapınız." });

                var place = _context.Places.Where(x => x.Id == id && x.UserId == user.Id).SingleOrDefault();
                if (place == null)
                    return NotFound();

                return Ok(place);
            }
            catch (Exception ex)
            {
                return BadRequest(new { succes = false, error = ex.Message });
            }
            
        }
        #endregion

        #region GetWentPlace
        [HttpGet("went/{visit:bool}")]
        public IActionResult GetWentPLace([FromRoute(Name = "visit")] bool visit)
        {
            var place = _context.Places.Where(x => x.Went == visit).ToList();
            return Ok(place);
        }
        #endregion

        #region GetRetryWentPLace
        [HttpGet("retrywent/{retryvisit:bool}")]
        public IActionResult GetRetryWentPLace([FromRoute(Name = "retryvisit")] bool retryvisit)
        {
            var place = _context.Places.Where(x => x.RetryWent == retryvisit).ToList();
            return Ok(place);
        }
        #endregion

        #region GetSearchPlace
        [HttpGet("searchplace/{search}")]
        public IActionResult GetSearchPlace([FromRoute(Name ="search")] string search)
        {
            var places = _context.Places.Where(x => x.Name.Contains(search) || x.Category.Contains(search) || x.Description.Contains(search) || x.City.Contains(search) || x.Town.Contains(search) || x.Address.Contains(search)).ToList();
            return Ok(places);
        }
        #endregion

        #region GetSearchPlaceByUser
        [HttpGet("searchplacebyuser/{search}")]
        public IActionResult GetSearchPlaceByUser([FromRoute(Name = "search")] string search)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                if (user == null)
                    return BadRequest(new { Message = "Giriş Yapınız." });

                var places = _context.Places.Where(x => x.Name.Contains(search) || x.Category.Contains(search) || x.Description.Contains(search) || x.City.Contains(search) || x.Town.Contains(search) || x.Address.Contains(search) && x.UserId == user.Id).ToList();
                if (places == null)
                    return NotFound();

                return Ok(places);
            }
            catch (Exception ex)
            {
                return BadRequest( new {success = false, error = ex.Message});
            }
            
        }
        #endregion

        #region AddPlace
        [HttpPost]
        public IActionResult AddPlace([FromBody] Places place)
        {
            try
            {
                if (place == null)
                    return BadRequest();
                _context.Places.Add(place);
                _context.SaveChanges();
                return StatusCode(201,place);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region EditPLace
        [HttpPost("edit/{id:int}")]
        public IActionResult EditPlace([FromRoute(Name = "id")] int id, [FromBody] Places place)
        {
            var entity = _context.Places.Where(x => x.Id == id).SingleOrDefault();
            if(entity == null)
                return NotFound();
            if(id != place.Id)
                return BadRequest();

            _context.Places.Remove(entity);
            place.Id = entity.Id;
            _context.Places.Add(place);
            _context.SaveChanges();
            return Ok(place);
        }
        #endregion

        #region GetTownListByCityId
        [HttpGet("city/{cityId}")]
        public IActionResult GetTownListByCityId([FromRoute(Name="cityId")] int cityId)
        {
            List<Town> towns = _context.Town.Where(x => x.CityId == cityId).OrderBy(x => x.TownName).ToList();
            return Ok(towns);
        }
        #endregion

        #region SetWentTrue
        [HttpPost("went/{went}/{id}")]
        public IActionResult SetWentTrue([FromRoute(Name ="went")]bool went, [FromRoute(Name ="id")]int id)
        {
            try
            {
                
                var place = _context.Places.FirstOrDefault(x => x.Id == id);
                if (place != null)
                {
                    place.Went = went;
                    _context.SaveChanges();
                    return Ok(new { success = true, message = "Veri Güncellendi. "});

                }
                return NotFound(new { success = false, message = "Place bulunamadı." });

            }
            catch (Exception ex)
            {
                // Hata oluştuğunda hata mesajını döndür
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
        #endregion

        #region SubmitRatingPost
        [HttpPost("rating/{id}")]
        public IActionResult SubmitRating([FromRoute(Name = "id")] int placeId, int atmosphereRating, int foodRating, int serviceRating)
        {
            try
            {
                var place = _context.Places.FirstOrDefault(p => p.Id == placeId);

                if (place != null)
                {
                    place.AtmosphereRating = atmosphereRating;
                    place.FoodRating = foodRating;
                    place.ServiceRating = serviceRating;

                    _context.SaveChanges(); // Değişiklikleri veritabanına kaydet
                }

                return Ok(new { success = true, message = "Değerlendirme başarıyla kaydedildi." });
            }
            catch (Exception ex)
            {
                // Hata yönetimi: İşlem sırasında bir hata oluştuğunda hata mesajını döndürün.
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region DeletePlace
        [HttpDelete("delete/{id:int}")]
        public IActionResult DeletePlace([FromRoute(Name ="id")] int id)
        {
            try
            {
                var place = _context.Places.Where(x => x.Id == id).SingleOrDefault();
                if (place == null)
                    return NotFound();
                if(place.Id != id)
                    return BadRequest();
                _context.Places.Remove(place);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
        #endregion

        #region GetPLaceByUserIdServices
        [HttpGet("getplacebtservices/{userid}")]
        public async Task<IActionResult> GetPlaceByServices([FromRoute(Name = "userid")]string userid) 
        {
            try
            {
                var user = _context.Users.SingleOrDefault(x =>x.UserId == userid);
                if (user == null)
                    return NotFound(new {success = false, message = "kullanıcı b ulunamadı"});
                var place = await _placeService.GetPlaceByUserId(userid);
                if (place == null)
                    return NotFound(new {succes = false, message = "Place bulunamadı"});
                return Ok(place);

            }
            catch (Exception ex)
            {
                return BadRequest(new {success = false, message = ex.Message});
            }
        }
        #endregion

        #region GetOnePlacesByServices
        [HttpGet("getoneplaceservices/{id:int}")]
        public IActionResult GetPlaceByIdServices([FromRoute(Name ="id")]int id) 
        {
            try
            {
                var place = _placeService.GetOnePlace(id);
                if (place == null)
                    return NotFound(new { success = false, message = "Place Bulunamadı." });
                return Ok(place);
            }
            catch (Exception ex)
            {
                return BadRequest(new {success = false, message = ex.Message});
            }
        }
        #endregion
    }

}
