using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlaceAPI.Models.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-Posta alanı boş geçilemez!")]

        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre alanı boş geçilemez! ")]

        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre Tekrar alanı boş geçilemez!")]

        public string RePassword { get; set; }

        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; }






    }
}
