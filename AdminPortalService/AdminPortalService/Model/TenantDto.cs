using System.ComponentModel.DataAnnotations;

namespace AdminPortalService.Model
{
    public class TenantDto
    {
        [Required]
        public string SupportContact { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ConnectionString { get; set; }
        private string _domainName;
        [Required]
        public string DomainName { get { return _domainName; } set { _domainName = value?.ToLower(); } }
    }
}
