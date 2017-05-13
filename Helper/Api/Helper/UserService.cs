using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Security.Principal;
using System.Data.Entity;

namespace System
{
    using System.Security.Principal;
    public class VolunteerService
    {
        public static Guid GetVolunteerIdFromIdentity()
        {
             Guid volunteerId=((VolunteerIdentity)Thread.CurrentPrincipal.Identity).VolunteerId;
           
            return volunteerId;
        }
        public static bool UpdateTokenActiveTime(Guid Id)
        {
            try
            {
                DbEntity db = new DbEntity();
                VolunteerToken volunteerToken = db.VolunteerTokens.FirstOrDefault(s => s.VolunteerId == Id);
                if (volunteerToken == null)
                    return false;
                volunteerToken.ActiveTime = DateTime.UtcNow;
                db.SaveChanges();
                db.Dispose();
                return true;

            }
            catch
            {
                return false;
            }
        }
        public static VolunteerToken GetToken(string token)
        {
            DbEntity db = new DbEntity();
            VolunteerToken volunteerToken=db.VolunteerTokens.FirstOrDefault(s => s.Token == token);
            db.Dispose();
            return volunteerToken;
        }

        public static Volunteer GetVolunteer(Guid id)
        {
            DbEntity db = new DbEntity();
            Volunteer volunteer = db.Volunteers.FirstOrDefault(s => s.Id == id);
            db.Dispose();
            return volunteer;
        }
        public static object FormatUser(Volunteer user)
        {
            return (new
            {
                Uuid = user.Uuid,
                LoginName = user.LoginName,
                MobileNumber = user.MobileNumber,
                //IDCardNumber = user.IDCardNumber,
               // TelephoneNumber = user.TelephoneNumber,
               // Name = user.Name,
                RegisterTime = user.RegisterTime,
                Status = user.Status,
                Remark = user.Remark,
                //Career = user.Career.Name

            }
            );
        }
    }
}