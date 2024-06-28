using System.ComponentModel.DataAnnotations;

namespace PlaceApp.Models
{
    public class PlaceListViewModel
    {
        [Required(ErrorMessage = "Liste adı zorunludur.")]
        [Display(Name = "Liste Adı")]
        public string ListName { get; set; }

        [Display(Name = "Liste Açıklama")]
        public string ListDescription { get; set; }

        [Required(ErrorMessage = "En az bir mekan seçmelisiniz.")]
        [Display(Name = "Eklemek İstediğiniz Mekanlar")]
        public List<int> SelectedPlaceIds { get; set; }

        // Mevcut mekanların listesi (bu, seçim kutularını oluşturmak için kullanılabilir)
        public List<PlaceApp.Models.Places> Places { get; set; }
    }

}
