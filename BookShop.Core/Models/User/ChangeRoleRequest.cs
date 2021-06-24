namespace BookShop.Core.Models.User
{
    public class ChangeRoleRequest
    {
        public string UserId { get; set; }

        public string Role { get; set; }
    }
}
