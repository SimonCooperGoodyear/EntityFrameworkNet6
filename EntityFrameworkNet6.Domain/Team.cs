using EntityFrameworkNet6.Domain.Common;

namespace EntityFrameworkNet6.Domain
{
    public class Team:BaseDomainObject
    {
        public string Name { get; set; }
        public int LeagueId { get; set; }
        public virtual League League { get; set; }
        public virtual Coach Coach { get; set; }

        public virtual List<Match> HomeMatches { get; set; }
        public virtual List<Match> AwayMatches { get; set; }
    }
}