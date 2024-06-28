using DocumentFormat.OpenXml.Office.CoverPageProps;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlaceApp.Data;
using PlaceApp.Models;

namespace PlaceApp.Controllers
{
	public class HomeController : Controller
	{
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
		{
            return View();

        }
        public IActionResult Share()
        {
            var sharePlace = _context.SharePlace.ToList();
            var sharePlaceList = _context.SharePlaceList.ToList();
            var shareViewModel = new ShareViewModel
            {
                SharePlace = sharePlace,
                SharePlaceList = sharePlaceList
            };

            return View(shareViewModel);

        }

        public IActionResult Place() 
        {
            var place = _context.SharePlace.Where(x => x.Show == 1).OrderByDescending(x => x.CreatedOn).ToList();
            return View(place); 
        }

        public IActionResult PlaceList()
        {
            var placeList = _context.SharePlaceList.AsNoTracking().OrderByDescending(x => x.CreatedDate).ToList();
            return View(placeList);
        }

        #region ShareListLike
        [HttpPost]
        public IActionResult ToggleLike(int listId, bool isLiked)
        {
            var sharePlaceList = _context.SharePlaceList.FirstOrDefault(x => x.Id == listId);
            if (sharePlaceList != null)
            {
                if (isLiked)
                {
                    sharePlaceList.LikeCount -= 1;
                }
                else
                {
                    sharePlaceList.LikeCount += 1;

                }

                _context.SaveChanges();

                return Json(new { success = true, isLiked = !isLiked, likeCount = sharePlaceList.LikeCount });
            }
            else
            {
                return Json(new { success = false, message = "Liste bulunamadı" });
            }
        }
        #endregion


    }
}
