using Models;

namespace System.Security.Principal
{
    public class VolunteerIdentity:IIdentity
    {
        public VolunteerIdentity(Volunteer volunteer)
        {
            this.VolunteerId = volunteer.Id;
            this.Name = volunteer.MobileNumber;
        }
        public Guid VolunteerId
        {
            get;private set;
        }
        public string AuthenticationType
        {
            get { return "CustomAuthentication"; }
        }
        public string Name
        {
            get;
        }

        public bool IsAuthenticated
        {
            get;
        }
    }
}