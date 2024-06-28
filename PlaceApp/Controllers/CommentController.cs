using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PlaceApp.Data;
using PlaceApp.Identity;
using PlaceApp.Models;
using PlaceApp.Models.ViewModel;

namespace PlaceApp.Controllers
{

    public class CommentController : Controller
    {
        #region Field
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private UserManager<AppIdentityUser> _userManager;
        private readonly AppDbContext _context;

        public CommentController(SignInManager<AppIdentityUser> signInManager, UserManager<AppIdentityUser> userManager, AppDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }
        #endregion

        [HttpPost]
        public JsonResult AddComment(string userId, int listId, string text, Comment comment)
        {
            var sharePlaceList = _context.SharePlaceList.FirstOrDefault(x => x.Id == listId);
            var user = _context.Users.Where(x => x.UserId == userId).SingleOrDefault();
            if(sharePlaceList != null)
            {
                if(user != null)
                {
                    comment.UserId = user.UserId;
                    comment.Text = text;
                    comment.ListId = listId;
                    comment.CreatedDate = DateTime.Now;
                    comment.Users = user;

                    _context.Comment.Add(comment);
                    _context.SaveChanges();

                    return Json ( new { success = true, comment });
                }
                else
                {
                    return Json(new { success = false, message = "Kullanıcı bulunamadı" });
                }
            }
            else
            {
                return Json(new { success = false, message = "Liste bulunamadı" });
            }
        }

        [HttpGet]
        public JsonResult GetCommentByListId(int listId)
        {
            if(listId != null)
            {
                var comments = _context.Comment.Where(x => x.ListId == listId).ToList();
                if (comments.Any()){
                    return Json (new {success=true, comments} );
                }
                else
                {
                    return Json(new { success = true, message = "Yorum Bulunamadı" });
                }
               

            } else
            {
                return Json(new { success = false, message = "Liste bulunamadı." });
            }

        }

        [HttpGet]
        public IActionResult GetComment(int listId)
        {
            if (listId != 0)
            {
                var listIdParameter = new SqlParameter("@ListId", listId);
                var comments = _context.Set<CommentViewModel>()
                .FromSqlRaw("EXEC sp_getcomment @ListId", listIdParameter)
                .ToList();
                return Json(new { success = true, comments });
            }
            else
            {
                return Json(new { success = false, message = "Liste bulunamadı." });
            }
        }

        [HttpPost]
        public JsonResult ToggleLike(int commentId, bool isLiked, Like likeModel)
        {
            var comment = _context.Comment.FirstOrDefault(x => x.Id == commentId);
            var user = _context.Users.Where(x => x.Email == User.Identity.Name).SingleOrDefault();

            if (comment != null)
            {
                if (isLiked)
                {
                    var like = _context.Like.Where(x => x.ContentId == commentId && x.UserId == user.UserId && x.IsLiked == 1 && x.ContentType == 2).SingleOrDefault();
                    like.IsLiked = 0;
                    like.LikeCount = comment.LikeCount - 1;
                    like.UnLikeTime = DateTime.Now;
                    _context.SaveChanges();
                    comment.LikeCount -= 1;
                }
                else
                {
                    var like = _context.Like.Where(x => x.ContentId == commentId && x.UserId == user.UserId && x.IsLiked == 0 && x.ContentType == 2).SingleOrDefault();

                    if (like != null)
                    {
                        like.IsLiked = 1;
                    }
                    else
                    {
                        likeModel.UserId = user.UserId;
                        likeModel.ContentId = commentId;
                        likeModel.LikeTime = DateTime.Now;
                        likeModel.IsLiked = 1;
                        likeModel.ContentType = 2;
                        _context.Like.Add(likeModel);
                    }

                    _context.SaveChanges();
                    comment.LikeCount += 1;
                }

                _context.SaveChanges();


                return Json(new { success = true, isLiked = !isLiked, likeCount = comment.LikeCount });
            }
            else
            {
                return Json(new { success = false, message = "Comment bulunamadı" });
            }
        }
    }
}
