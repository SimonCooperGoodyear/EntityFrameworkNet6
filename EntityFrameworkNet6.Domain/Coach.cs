using EntityFrameworkNet6.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkNet6.Domain
{
    public class Coach:BaseDomainObject
    {
        public string Name { get; set; }
        public int? TeamId { get; set; } // make it nullable as a coach doesn't necessarily have a team
        public virtual Team Team { get; set; }
    }
}
