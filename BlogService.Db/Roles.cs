namespace BlogService.Db
{
    public static class Roles
    {
        public static string[] AllRoles { get; } = new []
        {
            User, 
            Moderator, 
            Admin
        };

        public const string User = "User";
        public const string Moderator = "Moderator";
        public const string Admin = "Admin";
    }
}
