using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PlaceApp.Data;
using PlaceApp.Identity;
using PlaceApp.Models;

namespace PlaceAPI.Controllers
{
    [Route("api/like")]
    [ApiController]
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

        #region AllLike
        [HttpGet("getallike")]
        public IActionResult GetAllLike()
        {
            try
            {
                var like = _context.Like;
                return Ok(like);
            }
            catch (Exception ex)
            {
                return BadRequest(new {success = false, message = ex.Message});
            }
        }
        #endregion

        #region GetLikeByUser
        [HttpGet("getlikebyuser/{userid}")]
        public IActionResult GetLike([FromRoute(Name = "userid")] string userid) 
        {
            try
            {
                if (userid != null)
                {
                    var user = _context.Users.SingleOrDefault(x => x.UserId == userid);
                    if(user != null)
                    {
                        var like = _context.Like.Where(x => x.UserId == userid).ToList();
                        if (like == null)
                            return NotFound(new {success = false, message= "Kullanıccının hiç beğenisi yok."});
                        return Ok( new {success = true, like});
                    }
                    else
                    {
                        return NotFound(new {success = false, message = "Kullanıcı bulunamadı."});
                    }
                }
                else
                {
                    return BadRequest(new {success=false, message="userid == null"});
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new {success=false, message = ex.Message});
            }
        }
        #endregion

        #region GetLikeByAspNetUser
        [HttpGet("getlikebyloginuser")]
        public IActionResult GetLikeByAspnetUser()
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                if (user != null)
                {
                    var like = _context.Like.Where(x => x.UserId == user.Id).ToList();
                    if (like == null)
                        return NotFound(new { success = false, message = "Kullanıcının hiç beğenisi yok." });
                    return Ok(new { success = true, like });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Kullanıcı bulunamadı." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region GetListLikeByUser
        [HttpGet("getlistlikebyuser/{userid}")]
        public IActionResult GetListLikeByUser([FromRoute(Name ="userid")] string userid)
        {
            try
            {
                if(userid != null)
                {
                    var user = _context.Users.SingleOrDefault(x => x.UserId == userid);
                    if(user != null)
                    {
                        var like = _context.Like.Where(x => x.UserId == userid && x.ContentType == 1).ToList();
                        if (like == null) 
                            return NotFound(new {success = false, message = "Kullanıcının liste beğenisi yok"});
                        return Ok(new {success = true, like});
                    }
                    else
                    {
                        return NotFound(new {success = false, message = "Kullanıcı bulunamadı."});
                    }
                }
                else
                {
                    return BadRequest(new { success = false, message = "userid null" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new {success = false, message = ex.Message});
            }
        }
        #endregion

        #region GetCommentLikeByUser
        [HttpGet("getcommentlikebyuser/{userid}")]
        public IActionResult GetCommentLikeByUser([FromRoute(Name ="userid")]string userid) 
        {
            try
            {
                if (userid != null)
                {
                    var user = _context.Users.SingleOrDefault(x => x.UserId == userid);
                    if (user != null)
                    {
                        var like = _context.Like.Where(x => x.UserId == userid && x.ContentType == 2).ToList();
                        if (like == null)
                            return NotFound(new { success = false, message = "Kullanıcının liste beğenisi yok" });
                        return Ok(new { success = true, like });
                    }
                    else
                    {
                        return NotFound(new { success = false, message = "Kullanıcı bulunamadı." });
                    }
                }
                else
                {
                    return BadRequest(new { success = false, message = "userid null" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new {success = false, message = ex.Message});
            }
        }
        #endregion

        #region GetCommentLikeUser
        [HttpGet("getlistlikeuser/{contentid:int}")]
        public IActionResult CommentLikeUser([FromRoute(Name = "contentid")] int contentid)
        {
            if (contentid == 0)
                return BadRequest(new { success = false, message = "contentid = 0" });
            var comment = _context.Comment.SingleOrDefault(x => x.Id == contentid);
            if (comment == null)
                return NotFound(new { success = false, message = "Yorum bulunamadı." });

            var listIdParameter = new SqlParameter("@ContentId", contentid);
            var contenttypeparameter = new SqlParameter("@ContentType", 1); //ContentType == 1 List
            var likedUsers = _context.Set<LikeUserViewModel>()
            .FromSqlRaw("EXEC sp_GetLikeUserByListId @ContentId, @ContentType", listIdParameter, contenttypeparameter)
            .ToList();
            if (likedUsers.Count == 0)
                return NotFound(new { success = false, message = "Beğenen kimse yok :(" });
            return Json(new { success = true, likedUsers, likecount = likedUsers.Count });
        }
        #endregion

        #region GetListLikeUser
        [HttpGet("getcommentlikeuser/{contentid:int}")]
        public IActionResult ListLikeUser([FromRoute(Name = "contentid")] int contentid)
        {
            if (contentid == 0)
                return BadRequest(new { success = false, message = "contentid = 0" });
            var list = _context.SharePlaceList.SingleOrDefault(x => x.Id == contentid);
            if (list == null)
                return NotFound(new { success = false, message = "Yorum bulunamadı." });

            var listIdParameter = new SqlParameter("@ContentId", contentid);
            var contenttypeparameter = new SqlParameter("@ContentType", 2); //ContentType == 2 Comment
            var likedUsers = _context.Set<LikeUserViewModel>()
            .FromSqlRaw("EXEC sp_GetLikeUserByListId @ContentId, @ContentType", listIdParameter, contenttypeparameter)
            .ToList();
            if (likedUsers.Count == 0)
                return NotFound(new { success = false, message = "Beğenen kimse yok :(" });
            return Json(new { success = true, likedUsers, likecount = likedUsers.Count });
        }
        #endregion

        #region ToogleLikeList
        [HttpPost("LikeUnLike/{listid:int}/{userid}")]
        public IActionResult ToogleLikeList([FromRoute(Name = "listid")] int listid, [FromQuery(Name = "isLiked")] bool isliked, [FromRoute(Name = "userid")] string userid)
        {
            var user = _context.Users.Where(x => x.UserId == userid).SingleOrDefault();
            var list = _context.SharePlaceList.SingleOrDefault(x => x.Id == listid);
            if (list != null)
            {

                if (user != null)
                {
                    if (isliked)
                    {
                        var like = _context.Like.Where(x => x.ContentId == listid && x.UserId == userid && x.ContentType == 1).SingleOrDefault();
                        like.IsLiked = 0;
                        like.LikeCount = list.LikeCount - 1;
                        like.UnLikeTime = DateTime.Now;
                        _context.SaveChanges();
                        list.LikeCount -= 1;
                    }
                    else
                    {
                        var like = _context.Like.Where(x => x.ContentId == listid && x.UserId == userid && x.ContentType == 1).SingleOrDefault();
                        if (like != null)
                        {
                            like.IsLiked = 1;
                            like.LikeTime = DateTime.Now;
                            like.LikeCount = list.LikeCount + 1;
                        }
                        else
                        {
                            Like likemodal = new Like();
                            likemodal.UserId = userid;
                            likemodal.ContentId = listid;
                            likemodal.LikeTime = DateTime.Now;
                            likemodal.IsLiked = 1;
                            likemodal.ContentType = 2;
                            likemodal.LikeCount = list.LikeCount + 1;
                            _context.Like.Add(likemodal);
                        }
                        _context.SaveChanges();
                        list.LikeCount += 1;
                    }
                    _context.SaveChanges();

                    return Ok(new { succes = true, isliked = !isliked, likecount = list.LikeCount });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Kullanıcı bulunamadı." });
                }
            }
            else
            {
                return BadRequest(new { succes = false, message = "Yorum bulunamadı." });
            }
        }
        #endregion

        #region ToggleLikeComment
        [HttpPost("LikeUnLike/{commentid:int}/{userid}")]
        public IActionResult ToggleLikeComment([FromRoute(Name = "commentid")] int commentid, [FromQuery(Name = "isLiked")] bool isliked, [FromRoute(Name = "userid")] string userid)
        {
            var user = _context.Users.Where(x => x.UserId == userid).SingleOrDefault();
            var comment = _context.Comment.SingleOrDefault(x => x.Id == commentid);
            if (comment != null)
            {

                if (user != null)
                {
                    if (isliked)
                    {
                        var like = _context.Like.Where(x => x.ContentId == commentid && x.UserId == userid && x.ContentType == 2).SingleOrDefault();
                        like.IsLiked = 0;
                        like.LikeCount = comment.LikeCount - 1;
                        like.UnLikeTime = DateTime.Now;
                        _context.SaveChanges();
                        comment.LikeCount -= 1;
                    }
                    else
                    {
                        var like = _context.Like.Where(x => x.ContentId == commentid && x.UserId == userid && x.ContentType == 2).SingleOrDefault();
                        if (like != null)
                        {
                            like.IsLiked = 1;
                            like.LikeTime = DateTime.Now;
                            like.LikeCount = comment.LikeCount + 1;
                        }
                        else
                        {
                            Like likemodal = new Like();
                            likemodal.UserId = userid;
                            likemodal.ContentId = commentid;
                            likemodal.LikeTime = DateTime.Now;
                            likemodal.IsLiked = 1;
                            likemodal.ContentType = 2;
                            likemodal.LikeCount = comment.LikeCount + 1;
                            _context.Like.Add(likemodal);
                        }
                        _context.SaveChanges();
                        comment.LikeCount += 1;
                    }
                    _context.SaveChanges();

                    return Ok(new { succes = true, isliked = !isliked, likecount = comment.LikeCount });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Kullanıcı bulunamadı." });
                }
            }
            else
            {
                return BadRequest(new { succes = false, message = "Yorum bulunamadı." });
            }
        }
        #endregion

        #region ToogleLikeWithAspNetUser
        [HttpPost("LikeUnLike/{listid:int}")]
        public IActionResult ToogleLikeListBtListId([FromRoute(Name = "listid")] int listid, [FromQuery(Name = "isLiked")] bool isliked)
        {
            var user = _userManager.GetUserAsync(User).Result;
            var list = _context.SharePlaceList.SingleOrDefault(x => x.Id == listid);
            if (list != null)
            {

                if (user != null)
                {
                    if (isliked)
                    {
                        var like = _context.Like.Where(x => x.ContentId == listid && x.UserId == user.Id && x.ContentType == 1).SingleOrDefault();
                        like.IsLiked = 0;
                        like.LikeCount = list.LikeCount - 1;
                        like.UnLikeTime = DateTime.Now;
                        _context.SaveChanges();
                        list.LikeCount -= 1;
                    }
                    else
                    {
                        var like = _context.Like.Where(x => x.ContentId == listid && x.UserId == user.Id && x.ContentType == 1).SingleOrDefault();
                        if (like != null)
                        {
                            like.IsLiked = 1;
                            like.LikeTime = DateTime.Now;
                            like.LikeCount = list.LikeCount + 1;
                        }
                        else
                        {
                            Like likemodal = new Like();
                            likemodal.UserId = user.Id;
                            likemodal.ContentId = listid;
                            likemodal.LikeTime = DateTime.Now;
                            likemodal.IsLiked = 1;
                            likemodal.ContentType = 2;
                            likemodal.LikeCount = list.LikeCount + 1;
                            _context.Like.Add(likemodal);
                        }
                        _context.SaveChanges();
                        list.LikeCount += 1;
                    }
                    _context.SaveChanges();

                    return Ok(new { succes = true, isliked = !isliked, likecount = list.LikeCount });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Kullanıcı bulunamadı." });
                }
            }
            else
            {
                return BadRequest(new { succes = false, message = "Yorum bulunamadı." });
            }
        }
        #endregion
    }
}
