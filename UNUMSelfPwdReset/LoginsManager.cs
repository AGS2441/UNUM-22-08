using UNUMSelfPwdReset.Models;

namespace UNUMSelfPwdReset
{
    public class LoginsManager
    {
        public async Task<List<UserLoginClient>> GetUserLogins(string userId, string Username, DateTime? pwdChangedOn)
        {
            List<UserLoginClient> loginClients = new List<UserLoginClient>() {
                new UserLoginClient() {
                    LoginType = LoginClientType.AzureAD
                    , UserLoginId= userId
                    ,Username= Username
                    ,LastSignInAt=DateTime.Now
                    , HasAccess= true,
                     ExpireInDays= pwdChangedOn.HasValue ? Convert.ToInt16((pwdChangedOn.Value.AddDays(365) - DateTime.Now).TotalDays ): null,
            }
              
            };

            return loginClients;
        }
    }
}
