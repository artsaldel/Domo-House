using SQLite.Net.Attributes;

namespace Mica.Droid.DataAccess
{
    [Table("Usuario")]
    public class Usuario
    {
        public Usuario() { }

        [PrimaryKey]
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsOnApp { get; set; }
    }
}