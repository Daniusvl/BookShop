using System.Collections.Generic;

namespace BookShop.Identity.Authorization
{
    public static class RoleConstants
    {
        public const string RoleClaim = "Role";

        public static readonly IList<string> DefaultUser;
        public const string DefaultUserName = "DefaultUser";

        public static readonly IList<string> Moderator;
        public const string ModeratorName = "Moderator";

        public static readonly IList<string> Administrator;
        public const string AdministratorName = "Administrator";

        static RoleConstants()
        {
            DefaultUser = new List<string> { "DefaultUser", "Moderator", "Administrator" };
            Moderator = new List<string> { "Moderator", "Administrator" };
            Administrator = new List<string> { "Administrator" };
        }
    }
}
