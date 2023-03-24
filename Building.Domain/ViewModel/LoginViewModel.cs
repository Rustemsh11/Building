using System.ComponentModel.DataAnnotations;

namespace Building.Domain
{
    public class LoginViewModel
    {
        [Display(Name ="Логин")]
        [Required(ErrorMessage ="Введите логин")]
        public string? Login { get; set; }
        [Display(Name = "Пароль")]
        [Required(ErrorMessage ="Введите пароль")]        
        public string? Password { get; set; }
        [Display(Name ="Запомнить?")]
        public bool RememberAccount { get; set; }
    }
}
