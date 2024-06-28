using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlaceApp.Models
{
    public class PopularPlaceViewModel
    {
        public int PlaceId { get; set; }
        public string PlaceName { get; set; }
    }
}
