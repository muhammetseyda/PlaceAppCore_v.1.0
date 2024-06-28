using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlaceApp.Data;
using PlaceApp.Identity;
using PlaceApp.Models;

namespace PlaceAPI.Controllers
{
    [Route("api/placelist")]
    [ApiController]
    public class PlaceListController : ControllerBase
    {
        #region Field
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private UserManager<AppIdentityUser> _userManager;
        private readonly AppDbContext _context;

        public PlaceListController(SignInManager<AppIdentityUser> signInManager, UserManager<AppIdentityUser> userManager, AppDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }
        #endregion

        #region GetAllPlaceList
        [HttpGet]
        public IActionResult GetAllPlaceList()
        {
            try
            {
                var lists = _context.PlaceLists.OrderByDescending(x => x.CreatedDate).ToList();
                foreach (var list in lists)
                {
                    if (!string.IsNullOrEmpty(list.PlaceIds))
                    {
                        // PlaceIds sütunu boş değilse işlem yap
                        var placeIdsString = list.PlaceIds;
                        var placeIds = placeIdsString.Split(',').Select(id => int.TryParse(id, out var parsedId) ? parsedId : 0).ToList();

                        // Bu listedeki mekanları bulmak için bir sorgu yapabilirsiniz
                        var placesInList = _context.Places.Where(x => placeIds.Contains(x.Id)).ToList();
                        list.Places = placesInList;
                    }
                }
                return Ok(lists);

            }
            catch (Exception ex)
            {
                // Hata oluştuğunda hata mesajını döndür
                return BadRequest(new { success = false, error = ex.Message });
            }

        }
        #endregion

        #region GetAllPlaceListByUser
        [HttpGet("byUserId/{userid}")]
        public IActionResult GetAllPlaceListByUser([FromRoute(Name = "userid")] string userid)
        {
            try
            {
                var user = _userManager.FindByIdAsync(userid).Result;
                if (user == null)
                    return BadRequest(new { Message = "Giriş Yapınız." });

                var lists = _context.PlaceLists.Where(x=> x.UserId == userid).OrderByDescending(x => x.CreatedDate).ToList();
                foreach (var list in lists)
                {
                    if (!string.IsNullOrEmpty(list.PlaceIds))
                    {
                        // PlaceIds sütunu boş değilse işlem yap
                        var placeIdsString = list.PlaceIds;
                        var placeIds = placeIdsString.Split(',').Select(id => int.TryParse(id, out var parsedId) ? parsedId : 0).ToList();

                        // Bu listedeki mekanları bulmak için bir sorgu yapabilirsiniz
                        var placesInList = _context.Places.Where(x => placeIds.Contains(x.Id)).ToList();
                        list.Places = placesInList;
                    }
                }
                return Ok(lists);

            }
            catch (Exception ex)
            {
                // Hata oluştuğunda hata mesajını döndür
                return BadRequest(new { success = false, error = ex.Message });
            }

        }
        #endregion

        #region GetOnePlaceList
        [HttpGet("{id:int}")]
        public IActionResult GetOnePlaceList([FromRoute(Name = "id")] int id)
        { 
            try
            {
                var lists = _context.PlaceLists.Where(x => x.Id == id).OrderByDescending(x => x.CreatedDate).ToList();
                foreach (var list in lists)
                {
                    if (!string.IsNullOrEmpty(list.PlaceIds))
                    {
                        // PlaceIds sütunu boş değilse işlem yap
                        var placeIdsString = list.PlaceIds;
                        var placeIds = placeIdsString.Split(',').Select(id => int.TryParse(id, out var parsedId) ? parsedId : 0).ToList();

                        // Bu listedeki mekanları bulmak için bir sorgu yapabilirsiniz
                        var placesInList = _context.Places.Where(x => placeIds.Contains(x.Id)).ToList();
                        list.Places = placesInList;
                    }
                }
                return Ok(lists);

            }
            catch (Exception ex)
            {
                // Hata oluştuğunda hata mesajını döndür
                return BadRequest(new { success = false, error = ex.Message });
            }

        }
        #endregion

        #region CreateListGet
        [HttpPost]
        public IActionResult CreateList([FromBody] PlaceLists placeLists)
        {
            try
            {
                if (placeLists == null)
                    return BadRequest();
                _context.PlaceLists.Add(placeLists);
                _context.SaveChanges();
                return StatusCode(201, placeLists);
            }
            catch (Exception ex)
            {
                // Hata oluştuğunda hata mesajını döndür
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
        #endregion

        #region EditPLaceList
        [HttpPost("edit/{id:int}")]
        public IActionResult EditPlace([FromRoute(Name = "id")] int id, [FromBody] PlaceLists placeLists)
        {
            try
            {
                var entity = _context.PlaceLists.Where(x => x.Id == id).SingleOrDefault();
                if (entity == null)
                    return NotFound();
                if (id != placeLists.Id)
                    return BadRequest();

                _context.PlaceLists.Remove(entity);
                placeLists.Id = entity.Id;
                _context.PlaceLists.Add(placeLists);
                _context.SaveChanges();
                return Ok(placeLists);
            }
            catch (Exception ex)
            {

                return BadRequest(new { success = false, error = ex.Message });
            }
            
        }
        #endregion

        #region DeleteList
        [HttpDelete("delete/{id:int}")]
        public IActionResult DeleteList([FromRoute(Name ="id")] int id)
        {
            try
            {
                var list = _context.PlaceLists.FirstOrDefault(x => x.Id == id);
                if(list == null)
                    return NotFound();
                if(list.Id != id)
                    return BadRequest();
                _context.PlaceLists.Remove(list);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, erorr = ex.Message });
            }
        }
        #endregion
    }
}
