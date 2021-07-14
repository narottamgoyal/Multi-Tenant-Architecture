using System.ComponentModel.DataAnnotations;

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
        [Required]
        public string UserName { get { return _userName; } set { _userName = value?.ToLower(); } }
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
    }
}