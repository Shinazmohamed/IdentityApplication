namespace IdentityApplication.Core
{
    public static class Constants
    {
        public static class Roles
        {
            public const string Administrator = "Administrator";
            public const string User = "User";
        }
        public static class Policies
        {
            public const string RequireAdmin = "RequireAdmin";
            public const string RequireUser = "RequireUser";
        }
        public static class ScreenModes
        {
            public const string Create = "1";
            public const string Edit = "2";
            public const string List = "3";
        }

    }
}
