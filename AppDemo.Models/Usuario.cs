namespace AppDemo.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }


        public string User { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }
    }
}
