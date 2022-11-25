using EntityFrameworkNet6.Domain.Common;

namespace EntityFrameworkNet6.Domain
{
    public class League:BaseDomainObject
    {
        public string Name { get; set; }
        public List<Team> Teams { get; set; }
    }
}