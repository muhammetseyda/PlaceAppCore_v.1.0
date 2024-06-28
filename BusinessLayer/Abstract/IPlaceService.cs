using PlaceApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IPlaceService
    {
        Task<List<Places>> GetPlaceByUserId(string userid);
        Task<Places> GetOnePlace(int id);
    }
}
