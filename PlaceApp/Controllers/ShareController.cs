using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlaceApp.Data;
using PlaceApp.Identity;
using PlaceApp.Models;
using System.Net;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PlaceApp.Controllers
{
    public class ShareController : Controller
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

        #region Index
        public IActionResult Index()
        {
            var shareplace = _context.SharePlace.OrderByDescending(x => x.CreatedOn).ToList();
            return View(shareplace);
        }
        #endregion

        #region Place
        public IActionResult Place()
        {
            var user = _userManager.FindByEmailAsync(User.Identity.Name);
            var shareplace = _context.SharePlace.Where(x => x.UserId == user.Result.Id && x.Show == 1).OrderByDescending(x => x.CreatedOn).ToList();
            return View(shareplace);
        }
        #endregion

        #region PlacePost
        [HttpPost]
        public async Task<IActionResult> Place(int id) 
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            
            var place = _context.Places.Where(x => x.Id == id).FirstOrDefault();
            if (place != null)
            {
                string username = user.FirstName;

                SharePlace sharePlace = new SharePlace
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
            }

            return RedirectToAction("Place", "Share"); 
        }
        #endregion

        #region PlaceList
        public IActionResult PlaceList()
        {
            var user =  _userManager.FindByEmailAsync(User.Identity.Name);

            var shareplace = _context.SharePlaceList.Where(x => x.UserId == user.Result.Id).OrderByDescending(x => x.CreatedDate).ToList();
            return View(shareplace);
        }
        #endregion

        #region PlaceListPost
        [HttpPost]
        public async Task<IActionResult> PlaceList(int id)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);

            var placelist = _context.PlaceLists.Where(x => x.Id == id).FirstOrDefault();
            if (placelist != null)
            {
                var shareplaceıds = "";
                string username = user.FirstName;
                foreach (var item in placelist.PlaceIds.Split(','))
                {
                    var place = _context.Places.Where(x => x.Id.ToString() == item).SingleOrDefault();
                    if (place != null)
                    {
                        SharePlace sharePlace = new SharePlace
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

                SharePlaceList sharePlaceList = new SharePlaceList
                {
                    UserId = user.Id,
                    UserName = username,
                    PlaceIds = shareplaceıds,
                    CreatedDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    ListName = placelist.ListName,
                    ListDescription = placelist.ListDescription,
                    SharePlace = _context.SharePlace.Where(p => shareplaceıds.Contains(p.Id.ToString())).ToList(),


                };
                _context.SharePlaceList.Add(sharePlaceList);
                _context.SaveChanges();


            }

            return RedirectToAction("PlaceList", "Share");
        }
        #endregion

        #region PlaceDetails
        public IActionResult PlaceDetails(int id)
        {
            var place = _context.SharePlace.Where(x => x.Id == id).SingleOrDefault();

            return View(place);
        }
        #endregion

        #region PlaceListDetails
        public IActionResult PlaceListDetails(int id)
        {
            var placelist = _context.SharePlaceList.Where(x => x.Id == id).SingleOrDefault();
            if (placelist != null && placelist.PlaceIds != null)
            {
                placelist.SharePlace = new List<SharePlace>();
                var placeIds = placelist.PlaceIds.Split(",").Select(int.Parse).Distinct();
                foreach (var item in placeIds)
                {
                    var placesInList = _context.SharePlace.Where(x => x.Id == item).SingleOrDefault();
                    if(placesInList != null) { 
                       
                        placelist.SharePlace.Add(placesInList);
                    }
                }

            }

            return View(placelist);
        }
        #endregion

        #region AddPlaceListFromShareList
        public IActionResult AddPlaceListFromShareList(int listId, string userId)
        {
            if(userId != null)
            {
                if(listId != 0)
                {
                    var sharePlaceList = _context.SharePlaceList.Where(x => x.Id == listId).SingleOrDefault();
                    var user = _context.Users.Where(x => x.UserId == userId).SingleOrDefault();
                    if(sharePlaceList != null)
                    {   var placeids = "";
                        List<Places> places = new List<Places>();
                        foreach (var item in sharePlaceList.PlaceIds.Split(","))
                        {
                            var shareplace = _context.SharePlace.Where(x => x.Id.ToString() == item).SingleOrDefault();
                            if (shareplace != null)
                            {
                                Places place = new Places
                                {
                                    UserId = userId,
                                    Category = shareplace.Category,
                                    Name = shareplace.Name,
                                    City = shareplace.City,
                                    Town = shareplace.Town,
                                    Address = shareplace.Address,
                                    Link1 = shareplace.Link1,
                                    Link2 = shareplace.Link2,
                                    WebSiteLink = shareplace.WebSiteLink,
                                    MapsLink = shareplace.MapsLink,
                                    CreatedOn = DateTime.Now,
                                    UpdateOn = DateTime.Now,
                                    Description = shareplace.Description,
                                    InstaUrl = shareplace.InstaUrl,
                                    
                                };
                                _context.Places.Add(place);
                                _context.SaveChanges();
                                placeids += place.Id.ToString() + ",";
                                places.Add(place);
                            }
                        }

                        if (!string.IsNullOrEmpty(placeids))
                        {
                            placeids = placeids.TrimEnd(',');
                        }
                        
                        PlaceLists placeList = new PlaceLists
                        {
                            UserId = userId,
                            CreatedDate = DateTime.Now,
                            UpdateDate = DateTime.Now,
                            ListName = sharePlaceList.ListName,
                            ListDescription = sharePlaceList.ListDescription,
                            PlaceIds = placeids,
                            Places = places


                        };
                        _context.PlaceLists.Add(placeList);
                    }
                }
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "PlaceList");
        }
        #endregion

        #region AddPlaceFromSharePlaceList
        public IActionResult AdddPlaceFromSharePlaceList(string userId, int placeId)
        {
            if (userId != null)
            {
                if(placeId.ToString() != null)
                {
                    var place = _context.SharePlace.Where(x => x.Id == placeId).SingleOrDefault();
                    if(place != null)
                    {
                        Places newplace = new Places {
                            UserId = userId,
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

                        };
                        _context.Places.Add(newplace);
                        _context.SaveChanges();

                    }
                }
            }
            return RedirectToAction("Index", "Place");
        }

        #endregion
    }
}
