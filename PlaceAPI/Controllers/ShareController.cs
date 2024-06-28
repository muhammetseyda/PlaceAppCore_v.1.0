using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlaceAPI.Models;
using PlaceApp.Data;
using PlaceApp.Identity;
using PlaceApp.Models;
using System.Collections.Specialized;

namespace PlaceAPI.Controllers
{
    [Route("api/Share")]
    [ApiController]
    public class ShareController : ControllerBase
    {
        #region Field
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private UserManager<AppIdentityUser> _userManager;
        private readonly AppDbContext _context;

        public ShareController(SignInManager<AppIdentityUser> signInManager, UserManager<AppIdentityUser> userManager, AppDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }
        #endregion

        #region GetSharePlaces
        [HttpGet("GetSharePlaces")]
        public IActionResult GetSharePlaces()
        {
            try
            {
                var shareplace = _context.SharePlace.Where(x => x.Show == 1).OrderByDescending(x => x.CreatedOn).ToList();
                if (shareplace == null)
                    return NotFound();

                return Ok(shareplace);
            }
            catch (Exception ex)
            {
                return BadRequest(new {success = false, error = ex.Message});
            }
        }
        #endregion

        #region GetOneSharePlace
        [HttpGet("GetOneSharePlace/{id:int}")]
        public IActionResult GetoneSharePlace([FromRoute(Name = "id")] int id)
        {
            try
            {
                var shareplace = _context.SharePlace.Where(x => x.Show == 1 && x.Id == id).SingleOrDefault();
                if (shareplace == null)
                    return NotFound();

                return Ok(shareplace);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
        #endregion

        #region GetSharePlacesByUser
        [HttpGet("GetSharePlacesByUser")]
        public IActionResult GetSharePlacesByUser()
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                if (user == null)
                    return BadRequest(new { Message = "Giriş Yapınız." });

                var shareplace = _context.SharePlace.Where(x => x.Show == 1 && x.UserId == user.Id).OrderByDescending(x => x.CreatedOn).ToList();
                if (shareplace == null)
                    return NotFound();

                return Ok(shareplace);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
        #endregion

        #region GetOneSharePlaceByUser
        [HttpGet("GetOneSharePlaceByUser/{id:int}")]
        public IActionResult GetOneSharePlaceByUser([FromRoute(Name = "id")] int id)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                if (user == null)
                    return BadRequest(new { Message = "Giriş Yapınız." });

                var shareplace = _context.SharePlace.Where(x => x.Show == 1 && x.UserId == user.Id && x.Id == id).OrderByDescending(x => x.CreatedOn).ToList();
                if (shareplace == null)
                    return NotFound();

                return Ok(shareplace);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
        #endregion

        #region PlacePost
        [HttpPost("place/{id}")]
        public IActionResult Place([FromRoute(Name = "id")] int id)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                if (user == null)
                    return BadRequest(new { Message = "Giriş Yapınız." });

                var place = _context.Places.Where(x => x.Id == id).SingleOrDefault();
                if (place != null)
                {
                    string username = user.FirstName;

                    PlaceApp.Models.SharePlace sharePlace = new PlaceApp.Models.SharePlace
                    {
                        UserId = user.Id,
                        PlaceId = place.Id,
                        UserName = username,
                        Category = place.Category,
                        Name = place.Name,
                        City = place.City,
                        Town = place.Town,
                        Address = place.Address,
                        Link1 = place.Link1,
                        Link2 = place.Link2,
                        WebSiteLink = place.WebSiteLink,
                        MapsLink = place.MapsLink,
                        CreatedOn = DateTime.Now,
                        UpdateOn = DateTime.Now,
                        Description = place.Description,
                        InstaUrl = place.InstaUrl,
                        AtmosphereRating = place.AtmosphereRating,
                        FoodRating = place.FoodRating,
                        ServiceRating = place.FoodRating,
                        Show = 1
                    };
                    _context.SharePlace.Add(sharePlace);
                    _context.SaveChanges();
                    return StatusCode(201, sharePlace);
                }
                return NotFound(new { Message = "Yer bulunamadı." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { succes = false, error = ex.Message });
            }
            
        }
        #endregion

        #region GetSharePlaceLists
        [HttpGet("GetSharePlaceLists")]
        public IActionResult GetSharePlaceLists()
        {
            try
            {
                var shareplace = _context.SharePlaceList.OrderByDescending(x => x.CreatedDate).ToList();
                foreach (var list in shareplace)
                {
                    if (!string.IsNullOrEmpty(list.PlaceIds))
                    {
                        // PlaceIds sütunu boş değilse işlem yap
                        var placeIdsString = list.PlaceIds;
                        var placeIds = placeIdsString.Split(',').Select(id => int.TryParse(id, out var parsedId) ? parsedId : 0).ToList();

                        // Bu listedeki mekanları bulmak için bir sorgu yapabilirsiniz
                        var placesInList = _context.SharePlace.Where(x => placeIds.Contains(x.Id)).ToList();
                        list.SharePlace = placesInList;
                    }
                }
                return Ok(shareplace);
            }
            catch (Exception ex)
            {
                return BadRequest(new {succes = false, error = ex.Message});
            }
        }
        #endregion
        #region GetSharePlacelistByPAgeandPagesiz
        [HttpGet("GetSharePlaceListByPage")]
        public IActionResult GetSharePlaceListByPage(int page = 1, int pageSize = 10)
        {
            try
            {
                var totalItems = _context.SharePlaceList.Count();
                var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                var startIndex = (page - 1) * pageSize;

                var shareplace = _context.SharePlaceList.OrderByDescending(x => x.CreatedDate)
                    .Skip(startIndex)
                    .Take(pageSize)
                    .ToList();

                foreach (var list in shareplace)
                {
                    if (!string.IsNullOrEmpty(list.PlaceIds))
                    {
                        var placeIdsString = list.PlaceIds;
                        var placeIds = placeIdsString.Split(',').Select(id => int.TryParse(id, out var parsedId) ? parsedId : 0).ToList();

                        var placesInList = _context.SharePlace.Where(x => placeIds.Contains(x.Id)).ToList();
                        list.SharePlace = placesInList;
                    }
                }

                var response = new
                {
                    TotalItems = totalItems,
                    TotalPages = totalPages,
                    CurrentPage = page,
                    PageSize = pageSize,
                    SharePlaceLists = shareplace
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        #endregion

        #region GetOneSharePlaceList
        [HttpGet("GetOneSharePlaceList/{id:int}")]
        public IActionResult GetOneSharePlaceList([FromRoute(Name = "id")] int id)
        {
            try
            {
                var shareplace = _context.SharePlaceList.Where(x => x.Id == id).OrderByDescending(x => x.CreatedDate).ToList();
                if (shareplace == null)
                    return NotFound();

                return Ok(shareplace);
            }
            catch (Exception ex)
            {
                return BadRequest(new { succes = false, error = ex.Message });
            }
        }
        #endregion

        #region GetSharePlaceListsByUser
        [HttpGet("GetSharePlaceListsByUser")]
        public IActionResult GetSharePlaceListsByUser()
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                if (user == null)
                    return BadRequest(new { Message = "Giriş Yapınız." });
                var shareplace = _context.SharePlaceList.Where(x => x.UserId == user.Id).OrderByDescending(x => x.CreatedDate).ToList();
                if (shareplace == null)
                    return NotFound();

                return Ok(shareplace);
            }
            catch (Exception ex)
            {
                return BadRequest(new { succes = false, error = ex.Message });
            }
        }
        #endregion

        #region GetSharePlaceListsByUserFromMail
        [HttpGet("share/{userid}")]
        public IActionResult GetShareByUser([FromRoute(Name = "userid")] string userid)
        {
            try
            {
                var user = _userManager.FindByIdAsync(userid).Result;
                if (user == null)
                    return BadRequest(new { Message = "Giriş Yapınız." });
                var lists = _context.SharePlaceList.Where(x => x.UserId == userid).OrderByDescending(x => x.CreatedDate).ToList();
                foreach (var list in lists)
                {
                    if (!string.IsNullOrEmpty(list.PlaceIds))
                    {
                        // PlaceIds sütunu boş değilse işlem yap
                        var placeIdsString = list.PlaceIds;
                        var placeIds = placeIdsString.Split(',').Select(id => int.TryParse(id, out var parsedId) ? parsedId : 0).ToList();

                        // Bu listedeki mekanları bulmak için bir sorgu yapabilirsiniz
                        var placesInList = _context.SharePlace.Where(x => placeIds.Contains(x.Id)).ToList();
                        list.SharePlace = placesInList;
                    }
                }
                return Ok(lists);
            }
            catch (Exception ex)
            {
                return BadRequest(new { succes = false, error = ex.Message });
            }
        }
        #endregion

        #region GetOneSharePlaceListsByUser
        [HttpGet("GetOneSharePlaceListsByUser/{id:int}")]
        public IActionResult GetOneSharePlaceListsByUser([FromRoute(Name = "id")] int id)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                if (user == null)
                    return BadRequest(new { Message = "Giriş Yapınız." });
                var shareplace = _context.SharePlaceList.Where(x => x.UserId == user.Id && x.Id == id).OrderByDescending(x => x.CreatedDate).ToList();
                if (shareplace == null)
                    return NotFound();

                return Ok(shareplace);
            }
            catch (Exception ex)
            {
                return BadRequest(new { succes = false, error = ex.Message });
            }
        }
        #endregion

        #region PlaceListPost
        [HttpPost("placelist/{id}")]
        public IActionResult PlaceList(int id)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                if (user == null)
                    return BadRequest(new { Message = "Giriş Yapınız." });

                var placelist = _context.PlaceLists.Where(x => x.Id == id).SingleOrDefault();
                if (placelist != null)
                {
                    var shareplaceıds = "";
                    string username = user.FirstName;
                    foreach (var item in placelist.PlaceIds.Split(','))
                    {
                        var place = _context.Places.Where(x => x.Id.ToString() == item).SingleOrDefault();
                        if (place != null)
                        {
                            PlaceApp.Models.SharePlace sharePlace = new PlaceApp.Models.SharePlace
                            {
                                UserId = user.Id,
                                PlaceId = place.Id,
                                UserName = username,
                                Category = place.Category,
                                Name = place.Name,
                                City = place.City,
                                Town = place.Town,
                                Address = place.Address,
                                Link1 = place.Link1,
                                Link2 = place.Link2,
                                WebSiteLink = place.WebSiteLink,
                                MapsLink = place.MapsLink,
                                CreatedOn = DateTime.Now,
                                UpdateOn = DateTime.Now,
                                Description = place.Description,
                                InstaUrl = place.InstaUrl,
                                AtmosphereRating = place.AtmosphereRating,
                                FoodRating = place.FoodRating,
                                ServiceRating = place.FoodRating,
                                Show = 0

                            };
                            _context.SharePlace.Add(sharePlace);
                            _context.SaveChanges();
                            shareplaceıds += sharePlace.Id.ToString() + ",";
                        }

                    }
                    if (!string.IsNullOrEmpty(shareplaceıds))
                    {
                        shareplaceıds = shareplaceıds.TrimEnd(',');
                    }

                    PlaceApp.Models.SharePlaceList sharePlaceList = new PlaceApp.Models.SharePlaceList
                    {
                        UserId = user.Id,
                        UserName = username,
                        PlaceIds = shareplaceıds,
                        CreatedDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        ListName = placelist.ListName,
                        ListDescription = placelist.ListDescription,


                    };
                    _context.SharePlaceList.Add(sharePlaceList);
                    _context.SaveChanges();
                    return StatusCode(201, sharePlaceList);
                }
                return NotFound(new { Message = "Yer bulunamadı." });
            }
            catch (Exception ex)
            {
                return BadRequest( new {success = false, error = ex.Message});
            }
            
        }
        #endregion

        #region GetSharePlaceDetails
        [HttpGet("GetOneSharePlaceDetails/{id:int}")]
        public IActionResult GetOneSharePlaceDetails([FromRoute(Name = "id")] int id)
        {
            try
            {
                var place = _context.SharePlace.Where(x => x.Id == id).SingleOrDefault();
                if (place == null)
                    return NotFound();

                return Ok(place);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
        #endregion

        #region GetSharePlaceDetailsByUser
        [HttpGet("GetSharePlaceDetailsByUser/{id:int}")]
        public IActionResult GetSharePlaceDetailsByUser([FromRoute(Name = "id")] int id)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                if (user == null)
                    return BadRequest(new { Message = "Giriş Yapınız." });

                var place = _context.SharePlace.Where(x => x.Id == id && x.UserId == user.Id).SingleOrDefault();
                if (place == null)
                    return NotFound();

                return Ok(place);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
        #endregion

        #region GetOneSharePlaceListDetails
        [HttpGet("GetOneSharePlaceListDetails/{id:int}")]
        public IActionResult GetOneSharePlaceListDetails(int id)
        {
            try
            {
                var placelist = _context.SharePlaceList.Where(x => x.Id == id).SingleOrDefault();
                if(placelist == null)
                    return NotFound();

                if (placelist != null && placelist.PlaceIds != null)
                {
                    placelist.SharePlace = new List<PlaceApp.Models.SharePlace>();
                    var placeIds = placelist.PlaceIds.Split(",").Select(int.Parse).Distinct();
                    foreach (var item in placeIds)
                    {
                        var placesInList = _context.SharePlace.Where(x => x.Id == item).SingleOrDefault();
                        if (placesInList != null)
                        {

                            placelist.SharePlace.Add(placesInList);
                        }
                    }
                }
                return Ok(placelist);
            }
            catch (Exception ex)
            {
                return BadRequest(new {success = false, error = ex.Message});
            }
            
        }
        #endregion

        #region GetOneSharePlaceListDetailsByUser
        [HttpGet("GetOneSharePlaceListDetailsByUser/{id:int}")]
        public IActionResult GetOneSharePlaceListDetailsByUser(int id)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                if (user == null)
                    return BadRequest(new { Message = "Giriş Yapınız." });
                var placelist = _context.SharePlaceList.Where(x => x.Id == id && x.UserId == user.Id).SingleOrDefault();
                if (placelist == null)
                    return NotFound();

                if (placelist != null && placelist.PlaceIds != null)
                {
                    placelist.SharePlace = new List<PlaceApp.Models.SharePlace>();
                    var placeIds = placelist.PlaceIds.Split(",").Select(int.Parse).Distinct();
                    foreach (var item in placeIds)
                    {
                        var placesInList = _context.SharePlace.Where(x => x.Id == item).SingleOrDefault();
                        if (placesInList != null)
                        {

                            placelist.SharePlace.Add(placesInList);
                        }
                    }
                }
                return Ok(placelist);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }

        }
        #endregion

        #region DeleteSharePlaceList
        [HttpDelete("delete/{listid:int}")]
        public IActionResult DeleteSharePlaceList([FromRoute(Name ="listid")] int listid, [FromQuery(Name ="userid")] string userid)
        {
            try
            {
                if(userid == null)
                    return BadRequest(new {success= false, message = "userid null"});
                if (listid == 0)
                    return BadRequest(new { success = false, message = "listid 0" });
                var user = _context.Users.SingleOrDefault(x => x.UserId == userid);
                if(user != null)
                {
                    var list = _context.SharePlaceList.SingleOrDefault(x => x.Id == listid);
                    var shareplace = _context.SharePlace.Where(x => list.PlaceIds.Contains(x.Id.ToString())).ToList();
                    if (list != null)
                    {
                        if( list.UserId == user.UserId)
                        {
                            var listLike = _context.Like.Where(x => x.ContentId == listid && x.ContentType == 1).ToList();
                            //_context.Like.RemoveRange(listLike);
                            //_context.SaveChanges();

                            var listcomment = _context.Comment.Where(x => x.ListId == listid).ToList();
                            //_context.Comment.RemoveRange(listcomment);
                            //_context.SaveChanges();

                            var commentlikelist = _context.Like.Where(l => listcomment.Select(c => c.Id).Contains(l.ContentId) && l.ContentType == 2).ToList();
                            //_context.Like.RemoveRange(commentlikelist);
                            //_context.SaveChanges();

                            _context.SharePlaceList.Remove(list);
                            _context.SharePlace.RemoveRange(shareplace);
                            _context.SaveChanges();
                            return Ok(new {success = true, message= "Liste başarılı bir şekilde silindi."});
                        }
                        else
                        {
                            return BadRequest(new {success = false, message = "Kullanıcı ile Listeyi oluşturan kişi aynı değil."});
                        }
                    }
                    else
                    {
                        return BadRequest(new {success = false, message = "Liste bulunamadı."});
                    }
                }
                else
                {
                    return BadRequest(new { success = false, message = "Kullanıcı bulunamadı." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new {success = false, message= ex.Message});
            }
        }
        #endregion
    }
}
