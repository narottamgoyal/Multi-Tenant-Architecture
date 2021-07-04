using System.Collections.Generic;

namespace AdminPortalService.Model
{
    public class UserDetailDto
    {
        private string _emailId;
        public string EmailId { get { return _emailId; } set { _emailId = value?.ToLower(); } }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public List<string> Roles = new List<string>();
    }
}