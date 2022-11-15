namespace WebApiJWT.Models
{
    public class RolePrivilege
    {
        public int Id { get; set; }
        public Role Role { get; set; }
        public int? RoleId { get; set; }
        public string Privilege { get; set; }
    }
}