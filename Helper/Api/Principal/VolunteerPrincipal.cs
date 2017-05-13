using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace System.Security.Principal
{
    public class VolunteerPrincipal:IPrincipal
    {
        public VolunteerPrincipal(VolunteerIdentity identity)
        {
            this.Identity = identity;
        }
       
        public VolunteerPrincipal(Volunteer volunteer)
            :this(new VolunteerIdentity(volunteer))
        {

        }
        public VolunteerIdentity Identity { get; }
        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        IIdentity IPrincipal.Identity
        {
            get { return Identity; } 
        }
    }
}