
using UNUMSelfPwdReset.Models;


namespace UNUMSelfPwdReset.Utilities
{
    public class CopyHandler
    {
        public static UserProfileViewModel UserProperty(Microsoft.Graph.User graphUser)
        {
            UserProfileViewModel user = new()
            {
                Id = graphUser.Id,
                GivenName = graphUser.GivenName,
                Surname = graphUser.Surname,
                UserPrincipalName = graphUser.UserPrincipalName,
                Mail = graphUser.Mail,
                LastSignInAt = DateTime.Now,
                LastPasswordChangeDateTime = graphUser.LastPasswordChangeDateTime?.DateTime,
                Department = graphUser.Department,
            };

            return user;
        }


    }
}
