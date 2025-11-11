using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgFlow.Domain.Enums
{
    public enum RequestStatus
    {
        Draft =0,
        Submitted = 1,
        InReview = 2,
        Approved= 3,
        Rejected = 4,
        Canceled = 5
    }
}
