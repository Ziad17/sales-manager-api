namespace SalesManager.Application.Base
{
    public static class ApiRoutes
    {
        private const string ModuleUrl = "api/users";

        public static class Admins
        {
            public const string Login = SubModuleUrl + "/login";
            private const string SubModuleUrl = ModuleUrl + "/admins";
        }

    }
}
