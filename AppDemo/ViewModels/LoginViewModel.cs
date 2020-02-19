using System.ComponentModel.DataAnnotations;

namespace AppDemo.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Introduzca su correo electrónico o nombre de inicio de sesión")]
        [Display(Name = "Usuario")]
        public string User { get; set; }

        [Required(ErrorMessage = "Introduzca su contraseña de inicio de sesión")]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
