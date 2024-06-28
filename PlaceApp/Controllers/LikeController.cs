using DocumentFormat.OpenXml.Office2019.Word.Cid;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PlaceApp.Data;
using PlaceApp.Identity;
using PlaceApp.Models;

namespace PlaceApp.Controllers
{
    public class LikeController : Controller
    {
        #region Field
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private UserManager<AppIdentityUser> _userManager;
        private readonly AppDbContext _context;

        public LikeController(SignInManager<AppIdentityUser> signInManager, UserManager<AppIdentityUser> userManager, AppDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }
        #endregion
        #region ShareListLike
        [HttpPost]
        public JsonResult ToggleLike(int listId, bool isLiked, Like likeModel)
        {
            var sharePlaceList = _context.SharePlaceList.FirstOrDefault(x => x.Id == listId);
            var user = _context.Users.Where(x => x.Email == User.Identity.Name).SingleOrDefault();

            if (sharePlaceList != null)
            {
                if (isLiked)
                {
                    var like = _context.Like.Where(x => x.ContentId == listId && x.UserId == user.UserId && x.IsLiked == 1 && x.ContentType == 1).SingleOrDefault();
                    like.IsLiked = 0;
                    like.LikeCount = sharePlaceList.LikeCount - 1;
                    like.UnLikeTime = DateTime.Now;
                    _context.SaveChanges();
                    sharePlaceList.LikeCount -= 1;
                }
                else
                {
                    var like = _context.Like.Where(x => x.ContentId == listId && x.UserId == user.UserId && x.IsLiked == 0 && x.ContentType == 1).SingleOrDefault();

                    if (like != null)
                    {
                        like.IsLiked = 1;
                    }
                    else
                    {
                        likeModel.UserId = user.UserId;
                        likeModel.ContentId = listId;
                        likeModel.LikeTime = DateTime.Now;
                        likeModel.LikeCount = sharePlaceList.LikeCount + 1;
                        likeModel.IsLiked = 1;
                        likeModel.ContentType = 1;
                        _context.Like.Add(likeModel);
                    }

                    _context.SaveChanges();
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

        [HttpGet]
        public JsonResult LikeUser (int listId)
        {
            if (listId == 0)
                return Json(new { success = false, message = "contentid = 0" });
            var list = _context.SharePlaceList.SingleOrDefault(x => x.Id == listId);
            if (list == null)
                return Json(new { success = false, message = "Yorum bulunamadı." });

            var listIdParameter = new SqlParameter("@ContentId", listId);
            var contenttypeparameter = new SqlParameter("@ContentType", 1); //ContentType == 2 List
            var likedUsers = _context.Set<LikeUserViewModel>()
            .FromSqlRaw("EXEC sp_GetLikeUserByListId @ContentId, @ContentType", listIdParameter, contenttypeparameter)
            .ToList();
            if (likedUsers.Count == 0)
                return Json(new { success = false, message = "Beğenen kimse yok :(" });
            return Json(new { success = true, likedUsers, likecount = likedUsers.Count });
        }


    }
}
