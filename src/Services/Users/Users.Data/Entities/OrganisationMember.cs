using System;
using System.Collections.Generic;
using System.Text;

namespace Users.Data.Entities;

public class OrganisationMember
{
    public Guid OrganisationId { get; set; }
    public string UserId { get; set; }

    public DateTime JoinedAt { get; set; }

    public OrganisationRole Role { get; set; }
}
