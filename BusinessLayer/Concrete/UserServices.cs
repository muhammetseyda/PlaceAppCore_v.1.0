using BusinessLayer.Abstract;
using Microsoft.EntityFrameworkCore;
using PlaceApp.Data;
using PlaceApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class UserServices : IUserServices
    {
        private readonly AppDbContext _context;

        public UserServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Users> GetUser(string userid)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == userid);
            return user;
        }

        //public async Task<Users> GetUsers(string userid)
        //{
        //    var user = await  _context.Users.Include(x => x.Places).SingleOrDefaultAsync(x => x.UserId == userid);
        //    return user;
        //}

        public async Task<Users> GetUsersWithPlaces(string userid)
        {
            var user = await _context.Users.Include(x => x.Places).SingleOrDefaultAsync(x => x.UserId == userid);
            if(user != null)
            {
                var places = await _context.Places.Where(x => x.UserId == userid).ToListAsync();
                foreach (var place in places)
                {
                    user.Places.Add(place);
                }
            }
            
            return user;
        }

        public async Task<Users> GetUsersWithPlaceLists(string userid)
        {
            var user = await _context.Users.Include(x => x.PlaceLists).SingleOrDefaultAsync(x => x.UserId == userid);
            if (user != null)
            {
                var placeLists = await _context.PlaceLists.Where(x => x.UserId == userid).ToListAsync();
                foreach (var placelist in placeLists)
                {
                    user.PlaceLists.Add(placelist);
                    if (!string.IsNullOrEmpty(placelist.PlaceIds))
                    {
                        var placeıdsstring = placelist.PlaceIds;
                        var placeIds = placeıdsstring.Split(',').Select(id => int.TryParse(id, out var parsedId) ? parsedId : 0).ToList();

                        var placeinlist = _context.Places.Where(x => placeIds.Contains(x.Id)).ToList();
                        placelist.Places = placeinlist;
                    }
                }
            }
            return user;
        }
        public async Task<Users> GetUserWithShaprePlace (string userid)
        {
            var user = await _context.Users.Include(x => x.SharePlaces).SingleOrDefaultAsync(x => x.UserId == userid);
            if (user != null)
            {
                var places = await _context.SharePlace.Where(x => x.UserId == userid).ToListAsync();
                foreach (var place in places)
                {
                    user.SharePlaces.Add(place);
                }
            }

            return user;
        }

        public async Task<Users> GetUsersWithSharePlaceLists(string userid)
        {
            var user = await _context.Users.Include(x => x.SharePlacesList).SingleOrDefaultAsync(x => x.UserId == userid);
            if (user != null)
            {
                var placeLists = await _context.SharePlaceList.Where(x => x.UserId == userid).ToListAsync();
                foreach (var placelist in placeLists)
                {
                    user.SharePlacesList.Add(placelist);
                    if (!string.IsNullOrEmpty(placelist.PlaceIds))
                    {
                        var placeıdsstring = placelist.PlaceIds;
                        var placeIds = placeıdsstring.Split(',').Select(id => int.TryParse(id, out var parsedId) ? parsedId : 0).ToList();

                        var placeinlist = _context.SharePlace.Where(x => placeIds.Contains(x.Id)).ToList();
                        placelist.SharePlace = placeinlist;
                    }
                }
            }
            return user;
        }

        public async Task<Users> GetUserWithPlaceAndPlaceLists (string userid)
        {
            var user = await _context.Users.Include(x => x.Places).Include(x => x.PlaceLists).SingleOrDefaultAsync(x => x.UserId == userid);
            if(user != null)
            {
                var places = await _context.Places.Where(x => x.UserId == userid).ToListAsync();
                foreach (var place in places)
                {
                    user.Places.Add(place);
                }
                var placeLists = await _context.PlaceLists.Where(x => x.UserId == userid).ToListAsync();
                foreach (var placelist in placeLists)
                {
                    user.PlaceLists.Add(placelist);
                    if (!string.IsNullOrEmpty(placelist.PlaceIds))
                    {
                        var placeıdsstring = placelist.PlaceIds;
                        var placeIds = placeıdsstring.Split(',').Select(id => int.TryParse(id, out var parsedId) ? parsedId : 0).ToList();

                        var placeinlist = _context.Places.Where(x => placeIds.Contains(x.Id)).ToList();
                        placelist.Places = placeinlist;
                    }
                }
            }
            return user;
        }
    }
}
