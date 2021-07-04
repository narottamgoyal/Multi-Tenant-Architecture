namespace AdminPortalService.Model
{
    public enum TenantUserRole
    {
        TenantAdmin,
        BasicUser,
        AdminAndBasic
    }

    public enum AdminUserRole
    {
        SuperAdmin
    }

    public class UserDetailDto
    {
        private string _userName;
        public string UserName { get { return _userName; } set { _userName = value?.ToLower(); } }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}