using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlaceApp.Data;
using PlaceApp.Identity;
using PlaceApp.Models;
using System.Linq;

namespace PlaceApp.Controllers
{
    [Authorize]
    public class PlaceListController : Controller
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

        #region Index
        public IActionResult Index()
        {
            var userId = _context.Users.Where(x => x.Email == User.Identity.Name).Select(x => x.UserId).FirstOrDefault();
            var lists = _context.PlaceLists.Where(x => x.UserId == userId).OrderByDescending(x => x.CreatedDate).ToList();
         

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

            return View(lists);
        }
        #endregion

        #region CreateListGet
        public IActionResult CreateList()
        {
            var userId = _context.Users.Where(x => x.Email == User.Identity.Name).Select(x => x.UserId).FirstOrDefault();
            var places = _context.Places.Where(x => x.UserId == userId).ToList(); // Tüm mekanları al
            var model = new PlaceListViewModel
            {
                Places = places,
                SelectedPlaceIds = new List<int>(), // Seçilen mekanların kimliklerini tutacak liste
            };


            return View(model);
        }
        #endregion

        #region CreateListPost
        [HttpPost]
        public IActionResult CreateList(PlaceListViewModel model)
        {
            var userId = _context.Users.Where(x => x.Email == User.Identity.Name).Select(x => x.UserId).FirstOrDefault();

            if (!ModelState.IsValid)
            {
                // Yeni bir PlaceLists nesnesi oluştur
                var newList = new PlaceLists
                {
                    UserId = userId,
                    CreatedDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    ListName = model.ListName,
                    ListDescription = model.ListDescription,
                    Places = _context.Places.Where(p => model.SelectedPlaceIds.Contains(p.Id)).ToList(),
                    PlaceIds = string.Join(",", model.SelectedPlaceIds)
                };

                // Yeni liste veritabanına eklenir
                _context.PlaceLists.Add(newList);
                _context.SaveChanges();

                return RedirectToAction("Index"); // Listelerin görüntülendiği bir sayfaya yönlendir
            }

            // Model geçersizse, sayfayı tekrar göster
            return View(model);
        }
        #endregion

        #region DetailList
        public IActionResult DetailList(int id)
        {
            var userId = _context.Users.Where(x => x.Email == User.Identity.Name).Select(x => x.UserId).FirstOrDefault();
            var placelist = _context.PlaceLists.Where(x => x.Id == id && x.UserId == userId).SingleOrDefault();
            if (placelist != null && placelist.PlaceIds != null)
            {
                foreach (var item in placelist.PlaceIds.Split(","))
                {
                    var placesInList = _context.Places.Where(x => x.Id.ToString() == item).SingleOrDefault();

                }
                
            }

            return View(placelist);

        }
        #endregion

        #region EditListGet
        public IActionResult EditList(int id)
        {
            var userId = _context.Users.Where(x => x.Email == User.Identity.Name).Select(x => x.UserId).FirstOrDefault();
            var placelist = _context.PlaceLists.Where(x => x.Id == id && x.UserId == userId).SingleOrDefault();
            if (placelist != null && placelist.PlaceIds != null)
            {
                var placeIds = placelist.PlaceIds;

                var placesInList = _context.Places.Where(x => x.UserId == userId).ToList();
                placelist.Places = placesInList;
            }

            return View(placelist);
        }
        #endregion

        #region EditListPost
        [HttpPost]
        public IActionResult EditList(PlaceListViewModel model, int id)
        {
            var userId = _context.Users.Where(x => x.Email == User.Identity.Name).Select(x => x.UserId).FirstOrDefault();
            if (!ModelState.IsValid)
            {
                var existingList = _context.PlaceLists.Find(id);

                if (existingList != null)
                {
                    existingList.UserId = userId;
                    existingList.ListName = model.ListName;
                    existingList.ListDescription = model.ListDescription;
                    existingList.UpdateDate = DateTime.Now;
                   
                    var selectedPlaceIds = model.SelectedPlaceIds ?? new List<int>();

                    existingList.PlaceIds = string.Join(",", selectedPlaceIds);

                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }
        #endregion

        #region DeleteList
        public IActionResult DeleteList(int id)
        {
            var userId = _context.Users.Where(x => x.Email == User.Identity.Name).Select(x => x.UserId).FirstOrDefault();
            var list = _context.PlaceLists.FirstOrDefault(x => x.Id == id );
            _context.PlaceLists.Remove(list);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion
    }
}
