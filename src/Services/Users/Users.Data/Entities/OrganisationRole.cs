namespace Users.Data.Entities
{
    public enum OrganisationRole
    {
        Visitor = 1,    // read-only access
        Member = 2,     // add/edit locations & assets, no user management
        Admin = 3,      // add/edit locations, assets & users
        Owner = 4       // full access
    }
}
