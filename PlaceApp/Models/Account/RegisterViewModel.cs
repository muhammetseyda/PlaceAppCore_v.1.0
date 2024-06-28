using System.ComponentModel.DataAnnotations;
public class RegisterViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Adınız alanı boş geçilemez!")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Soyadınız alanı boş geçilemez!")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "E-posta alanı boş geçilemez!")]

    public string Email { get; set; }

    [Required(ErrorMessage = "Şifre alanı boş geçilemez!")]

    public string Password { get; set; }


    [Required(ErrorMessage = "Şifre Tekrar alanı boş geçilemez!")]


    public string RePassword { get; set; }

    //public int checkDeger { get; set; }

    [Required(ErrorMessage = "Telefon boş geçilemez")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Cinsiyet Boş Geçilemez")]
    public string Gender { get; set; }

}
//public class RegisterViewModel
//{
//    [Required(ErrorMessage = "Kullanıcı adı alanı gereklidir.")]
//    [Display(Name = "Kullanıcı Adı")]
//    public string UserName { get; set; }

//    [Required(ErrorMessage = "E-posta alanı gereklidir.")]
//    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
//    [Display(Name = "E-posta")]
//    public string Email { get; set; }

//    [Required(ErrorMessage = "Şifre alanı gereklidir.")]
//    [DataType(DataType.Password)]
//    [Display(Name = "Şifre")]
//    public string Password { get; set; }

//    [DataType(DataType.Password)]
//    [Display(Name = "Şifre Tekrar")]
//    [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor.")]
//    public string ConfirmPassword { get; set; }
//}
