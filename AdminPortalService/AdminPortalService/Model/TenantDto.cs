namespace AdminPortalService.Model
{
    public class TenantDto
    {
        public string SupportContact { get; set; }
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        private string _domainName;
        public string DomainName { get { return _domainName; } set { _domainName = value?.ToLower(); } }
    }
}
