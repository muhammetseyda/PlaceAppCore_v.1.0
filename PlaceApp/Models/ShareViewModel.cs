using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlaceApp.Models
{
    public class ShareViewModel
    {
        public List<SharePlace> SharePlace { get; set; }
        public List<SharePlaceList> SharePlaceList { get; set; }
    }
}
