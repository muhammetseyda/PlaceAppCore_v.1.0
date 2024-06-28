using Microsoft.AspNetCore.Identity;
using PlaceApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IUserServices
    {
        Task<Users> GetUser(string userid);
        //Task<Users> GetUsers(string userid);
        Task<Users> GetUsersWithPlaces(string userid);
        Task<Users> GetUsersWithPlaceLists(string userid);
        Task<Users> GetUserWithPlaceAndPlaceLists(string userid);
        //Task<string> GetUserIdentity();
    }
}
