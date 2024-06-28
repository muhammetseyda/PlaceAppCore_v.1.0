using BusinessLayer.Abstract;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlaceApp.Data;
using PlaceApp.Identity;
using PlaceApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class PlaceService : IPlaceService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly SignInManager<AppIdentityUser> _signInManager;

        public PlaceService(AppDbContext context, UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public Task<List<Places>> GetPlaceByUserId(string userid)
        {
            return _context.Places.Where(x => x.UserId == userid).ToListAsync();
        }
        public async Task<Places>  GetOnePlace(int id)
        {
            var place = await _context.Places.SingleOrDefaultAsync(x => x.Id == id);
            return place;
        }

        //public async List<Places> GetPlaceFromUser()
        //{
        //    var user = await _userManager.GetUserAsync(User).Result;
        //}
    }
}
