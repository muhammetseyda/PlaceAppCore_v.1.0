using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlaceApp.Data;
using PlaceApp.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using Microsoft.AspNetCore.Identity;
using PlaceApp.Identity;

namespace PlaceApp.Controllers
{
    [Authorize]
    public class PlaceController : Controller
    {
        #region Field
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private UserManager<AppIdentityUser> _userManager;
        private readonly AppDbContext _context;

        public PlaceController(SignInManager<AppIdentityUser> signInManager, UserManager<AppIdentityUser> userManager, AppDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }
        #endregion

        #region Index
        public IActionResult Index(int? page)
        {
            int pageSize = 10; // Her sayfada gösterilecek öğe sayısı
            int pageNumber = (page ?? 1); // Sayfa numarası, varsayılan olarak 1
            var userId = _context.Users.Where(x => x.Email == User.Identity.Name).Select(x => x.UserId).FirstOrDefault();
            ViewBag.PlaceList = _context.PlaceLists.Where(x => x.UserId == userId).ToList();

            var places = _context.Places.Where(x => x.UserId == userId).OrderBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;
            ViewBag.Place = _context.Places.ToList().Count;

            return View(places);
        }
        #endregion

        #region IndexPost
        [HttpPost]
        public IActionResult Index(string visit, string search, int? page)
        {
            var userId = _context.Users.Where(x => x.Email == User.Identity.Name).Select(x => x.UserId).FirstOrDefault();
            int pageSize = 10; // Her sayfada gösterilecek öğe sayısı
            int pageNumber = (page ?? 1); // Sayfa numarası, varsayılan olarak 1
            ViewBag.PlaceList = _context.PlaceLists.Where(x => x.UserId == userId).ToList();
            IQueryable<Places> query = _context.Places.Where(x => x.UserId == userId);

            if (visit == "1")
            {
                query = query.Where(x => x.Went == true);
            }
            else if (visit == "0")
            {
                query = query.Where(x => x.Went == false);
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Name.Contains(search) || x.Category.Contains(search) || x.Description.Contains(search) || x.City.Contains(search) || x.Town.Contains(search) || x.Address.Contains(search));
            }

            int totalPlaces = query.Count();
            var places = query.OrderBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;
            ViewBag.Place = totalPlaces;

            return View(places);
        }
        #endregion

        #region WentPlace
        public IActionResult WentPlace(int? page)
        {
            int pageSize = 10; // Her sayfada gösterilecek öğe sayısı
            int pageNumber = (page ?? 1); // Sayfa numarası, varsayılan olarak 1

            var places = _context.Places.Where(x => x.Went == true).OrderBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;
            ViewBag.Place = _context.Places.Where(x => x.Went == true).ToList().Count;

            return View(places);
        }
        #endregion

        #region DontWentPlace
        public IActionResult DontWentPlace(int? page)
        {
            int pageSize = 10; // Her sayfada gösterilecek öğe sayısı
            int pageNumber = (page ?? 1); // Sayfa numarası, varsayılan olarak 1

            var places = _context.Places.Where(x => x.Went == false).OrderBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;
            ViewBag.Place = _context.Places.Where(x => x.Went == false).ToList().Count;

            return View(places);
        }
        #endregion

        #region Details
        public IActionResult Details(int id)
        {
            var place = _context.Places.Where(x => x.Id == id).SingleOrDefault();

            return View(place);
        }
        #endregion

        #region AddPlace
        [Authorize]
        public IActionResult AddPlace()
        {
            var city = _context.City.OrderBy(x => x.CityName != "İstanbul").ThenBy(x => x.CityName).ToList();
            ViewBag.City = city;
            var category = _context.Category.ToList();
            ViewBag.Category = category;
            return View();
        }
        #endregion

        #region AddPlacePost
        [HttpPost]
        public IActionResult AddPlace(Places place) 
        {
            var cities = _context.City.Where(x => x.CityId.Equals(place.City)).SingleOrDefault();
            if (place.Link2 != null)
            {
                string link1 = place.Link2;
                if (link1 != null)
                {
                    string[] parts = link1.Split('/');
                    if (parts.Length >= 5)
                    {
                        string contentId = parts[4];
                        Console.WriteLine("İçerik Kimliği: " + contentId);
                        place.InstaUrl = contentId;
                    } 
                    else                    {
                        Console.WriteLine("Instagram URL'si geçerli değil.");
                    }
                    place.UserId = _context.Users.Where(x => x.Email == User.Identity.Name).Select(x => x.UserId).FirstOrDefault();
                    place.City = cities.CityName;
                    _context.Places.Add(place);
                    _context.SaveChanges();
                }

            }
            else
            {
                place.UserId = _context.Users.Where(x => x.Email == User.Identity.Name).Select(x => x.UserId).FirstOrDefault();
                place.City = cities.CityName;
                _context.Places.Add(place);
                _context.SaveChanges();
            }
            

            return RedirectToAction("Index");
        }
        #endregion

        #region Edit
        public IActionResult Edit(int id)
        {
            var city = _context.City.OrderBy(x => x.CityName != "Istanbul").ThenBy(x => x.CityName).ToList();
            ViewBag.City = city;
            var category = _context.Category.ToList();
            ViewBag.Category = category;
            var place = _context.Places.Where(x => x.Id == id).SingleOrDefault();
            return View(place);
        }
        #endregion

        #region EditPost
        [HttpPost]
        public IActionResult Edit(Places place)
        {
            if (place.Id == null)
            {
                return View(place);
            }
            if (ModelState.IsValid) 
            {
                var category = _context.Category.ToList();
                ViewBag.Category = category;
                var editplace = _context.Places.Where(x => x.Id == place.Id).SingleOrDefault();
                var cities = _context.City.Where(x => x.CityId.Equals(place.City)).SingleOrDefault();
                if (editplace != null && cities != null)
                {
                    
                    editplace.Category = place.Category;
                    editplace.Name = place.Name;
                    editplace.Description = place.Description;
                    editplace.City = cities.CityName;
                    editplace.Town = place.Town;
                    editplace.Address = place.Address;
                    editplace.Link1 = place.Link1;
                    editplace.Link2 = place.Link2;
                    editplace.InstaUrl = place.InstaUrl;
                    editplace.WebSiteLink = place.WebSiteLink;
                    editplace.MapsLink = place.MapsLink;
                    editplace.CreatedOn = place.CreatedOn;
                    editplace.UpdateOn = place.UpdateOn;
                    editplace.Went = place.Went;
                    editplace.RetryWent = place.RetryWent;
                }
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(place);
        }
        #endregion

        #region GetTownListByCityId
        [HttpGet]
        public IActionResult GetTownListByCityId (int cityId)
        {
            List<Town> towns = _context.Town.Where(x => x.CityId == cityId).OrderBy(x => x.TownName).ToList();
            return Json(towns);
        }
        #endregion

        #region SetWentTruePost
        [HttpPost]
        public IActionResult SetWentTrue(bool went, int id)
        {
            try
            {
                var place = _context.Places.SingleOrDefault(x => x.Id == id);
                if(place != null) 
                {
                    place.Went = went;
                    _context.SaveChanges();
                    return Json(new { success = true });

                }
                return Json(new { success = false, message = "Place bulunamadı." });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
        #endregion

        #region SubmitRatingPost
        [HttpPost]
        public IActionResult SubmitRating(int placeId, int atmosphereRating, int foodRating, int serviceRating)
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

                return Json(new { success = true, message = "Değerlendirme başarıyla kaydedildi." });
            }
            catch (Exception ex)
            {
                // Hata yönetimi: İşlem sırasında bir hata oluştuğunda hata mesajını döndürün.
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion

    }
}
