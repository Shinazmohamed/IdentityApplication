namespace IdentityApplication.Core
{
    public static class Constants
    {
        public static class Roles
        {
            public const string Administrator = "Administrator";
            public const string User = "User";
            public const string SuperDeveloper = "SuperDeveloper";
        }

        public static class RedirectionPaths
        {
            public const string Profile = "Identity/Account/Manage";
            public const string UserRegisteration = "Identity/Account/Register";
            public const string Logout = "Identity/Account/Login";
        }
    }
}
