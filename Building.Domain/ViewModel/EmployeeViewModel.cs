using System.ComponentModel.DataAnnotations;

namespace Building.Domain.ViewModel
{
    public class EmployeeViewModel
    {
        [Required(ErrorMessage = "Укажите имя")]
        [MaxLength(50, ErrorMessage = "Имя должно не превышать 50 символов")]
        [MinLength(2, ErrorMessage = "Имя должно быть больше 2 символов")]
        public string Name { get; set; } = null!;


        [Required(ErrorMessage = "Укажите фамилию")]
        [MaxLength(50, ErrorMessage = "Фамилия должно не превышать 50 символов")]
        [MinLength(2, ErrorMessage = "Фамилия должно быть больше 2 символов")]
        public string SecondName { get; set; } = null!;

        public string? MiddleName { get; set; }
        public string? Birthday { get; set; }
        public string? Phone { get; set; }
        public string? Position { get; set; }
        public List<string> Site { get; set; }
        public string? Login { get; set; }


        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Укажите пароль")]
        [MinLength(6, ErrorMessage = "Пароль должен быть больше 6 символов")]
        public string Password { get; set; }

        
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Укажите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
