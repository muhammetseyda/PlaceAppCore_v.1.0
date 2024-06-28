using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PlaceApp.Data;
using PlaceApp.Identity;
using PlaceApp.Models;

namespace PlaceAPI.Controllers
{
    [Route("api/comment")]
    [ApiController]
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

        #region PostComment
        [HttpPost]
        public IActionResult Comment([FromBody] Comment comment) 
        {
            try
            {
                if (comment == null)
                    return BadRequest(new { success = false, message = "Comment == null" });
                var user = _context.Users.Where(x => x.UserId == comment.UserId).SingleOrDefault();
                if (user == null)
                    return BadRequest(new { success = false, message = "Kullanıcı bulunamadı." });
                comment.Users = user;
                _context.Comment.Add(comment);
                var list = _context.SharePlaceList.SingleOrDefault(x => x.Id == comment.ListId);
                if(list != null)
                    list.CommentCount += 1;
                _context.SaveChanges();
                return StatusCode(201, comment);
            }
            catch (Exception ex)
            {
                return BadRequest(new {success = false, message= ex.Message});
            }
            
        }
        #endregion

        #region GetAllComment
        [HttpGet("getallcomment")]
        public IActionResult GettAllCOmment()
        {
            try
            {
                var comment = _context.Comment;
                return Ok(comment);
            }
            catch (Exception ex) 
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
        #endregion

        #region GetCommentByListId
        [HttpGet("getcommentbylistid/{listId:int}")]
        public IActionResult GetCommentByListId([FromRoute(Name = "listId")] int listId)
        {
            try
            {
                if(listId != 0)
                {
                    var list = _context.SharePlaceList.Where(x => x.Id == listId).SingleOrDefault();
                    if (list != null)
                    {
                        var comments = _context.Comment.Include(x => x.Users).Where(x => x.ListId == listId).ToList();
                        if (comments != null && comments.Count > 0)
                        {
                            return Ok(comments);
                        }
                        else
                        {
                            return NotFound(new { success = true, message = "Yorum bulunamadı." });
                        }
                    }
                    else
                    {
                        return NotFound(new { success = false, message = "Berlirtilen liste bulunamadı." });
                    }
                    
                }
                else
                {
                    return BadRequest(new { success = false, message = "listId = 0 " });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
        #endregion

        #region GetCommentById
        [HttpGet("getcommentbyid/{id:int}")]
        public IActionResult GetCommentById([FromRoute(Name = "id")]int id)
        {
            try
            {
                if (id != 0)
                {
                    var comment = _context.Comment.Include(x => x.Users).SingleOrDefault(x => x.Id == id);
                    if (comment != null)
                    {
                        return Ok(comment);
                    }
                    else
                    {
                        return NotFound(new { success = false, message = "Comment bulunamadı." });
                    }
                }
                else
                {
                    return BadRequest(new {success= false, message="id = 0"});
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
        #endregion

        #region GetCommetByUserId
        [HttpGet("getcommentbyuserid/{userid}")]
        public IActionResult GetCommentByUserId([FromRoute(Name ="userid")] string userid) {
            try
            {
                if (userid != null)
                {
                    var user = _context.Users.SingleOrDefault(x => x.UserId == userid);
                    if (user != null)
                    {
                        var comment = _context.Comment.Include(x => x.Users).Where(x => x.UserId == user.UserId).ToList();
                        if (comment != null)
                        {
                            return Ok(comment);
                        }
                        else
                        {
                            return NotFound(new { success = false, message = "Comment bulunamadı." });
                        }
                    }
                    else
                    {
                        return NotFound(new { sucsess = false, message = "Kullanıcı bulunamadı." });
                    }
                }
                else
                {
                    return NotFound(new { success = false, message = "userid= null" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message});
            } 
        }
        #endregion

        #region GetLikeCountByCommentId
        [HttpGet("getlikecount/{commentid:int}")]
        public IActionResult GetLikeConutByCommenyId([FromRoute(Name = "commentid")] int commentid)
        {
            try
            {
                if (commentid == 0)
                    return BadRequest(new { success = false, message = "commentid = 0" });
                var comment = _context.Comment.SingleOrDefault(c => c.Id == commentid);
                if (comment == null)
                    return NotFound(new {success = false, message="Yorum bulunamadı"});
                return Ok(new {success= true, likeCount = comment.LikeCount});
            }
            catch (Exception ex)
            {
                return BadRequest(new {success=false, message=ex.Message});
            }
        }
        #endregion

        #region DeleteComment
        [HttpDelete("delete/{commentid:int}")]
        public IActionResult DeleteCommentById([FromRoute(Name ="commentid")] int commentid, [FromQuery(Name = "userid")] string userid)
        {
            try
            {
                if(commentid != 0)
                {
                    var comment = _context.Comment.SingleOrDefault(x => x.Id == commentid);
                    if(comment != null)
                    {
                        var user = _context.Users.SingleOrDefault(x => x.UserId == userid);
                        if(user != null)
                        {
                            if(user.UserId == comment.UserId)
                            {
                                var listlike = _context.Like.Where(x => x.ContentId == commentid && x.ContentType == 2).ToList();
                                _context.Like.RemoveRange(listlike);
                                _context.SaveChanges();

                                _context.Comment.Remove(comment);
                                _context.SaveChanges();
                                return Ok(new {success = true, message = "Yorum başarılı bir şekilde silindi."});
                            }
                            else
                            {
                                return BadRequest(new { success = false, message= "Kendinize ait olmayan yorumu silemezsiniz."});
                            }
                        }
                        else
                        {
                            return NotFound(new { success = false, message = "Kullanıcı bulunamadı."});
                        }
                    }
                    else
                    {
                        return NotFound(new {success = false, messsage = "Yorum bulunamadı."});
                    }
                }
                else
                {
                    return BadRequest(new {success = false, message="commentid == 0"});
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new {success = false, message = ex.Message});
            }
        }

        #endregion

        #region CommentCount
        [HttpGet("getCommentCount/{listId:int}")]
        public IActionResult GetCommentCount([FromRoute(Name ="listId")]int listId)
        {
            try
            {
                if(listId != 0)
                {
                    var list = _context.SharePlaceList.SingleOrDefault(x => x.Id == listId);
                    if(list != null)
                    {
                        var comment = _context.Comment.Where(x => x.ListId == listId).ToList();
                        return Ok(new {success = true, commentCount = comment.Count});
                    }
                    else
                    {
                        return NotFound(new {success = false, message="Liste bulunamadı."});
                    }
                }
                else
                {
                    return BadRequest(new { success = false, message = "listId = 0" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new {success = false, message = ex.Message});
            }
        }
        #endregion
    }
}
