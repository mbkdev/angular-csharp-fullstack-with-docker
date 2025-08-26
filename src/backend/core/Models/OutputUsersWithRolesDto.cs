namespace core.Models
{
    public class OutputUsersWithRolesDto
    {
        public string Email { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }
}
