using Microsoft.Identity.Client;
using System.Data;
using System.Security;

namespace UNUMSelfPwdReset.Managers
{
    public class AzureAdminActionManager
    {

        private readonly IConfiguration _config;
        public AzureAdminActionManager(IConfiguration config)
        {
            _config = config;
        }


        public async Task<string> GetAdminTokenForGraph()
        {
            try
            {
                 
                string[] scopes = new string[] { "User.Read" };

                string clientId = _config.GetValue<string>("AzureAd:ClientId"); 
                string tenantId = _config.GetValue<string>("AzureAd:TenantId");
                string AdminUserName = _config.GetValue<string>("AdminCreds:UserName");
                string AdminPassword = _config.GetValue<string>("AdminCreds:Password");


                //string clientId = "1d488a06-2b57-4418-b8b3-322b72be3917";
                //string tenantId = "0951bc8d-d7bc-40d0-9668-9119a55ad78c";
                //string AdminUserName = "ram@adventglobal.com";
                //string AdminPassword = "Ruj29213";

                IPublicClientApplication app;
                app = PublicClientApplicationBuilder.Create(clientId)
                              .WithAuthority(AzureCloudInstance.AzurePublic, tenantId)
                              .WithAuthority(AadAuthorityAudience.AzureAdMyOrg)
                              .Build();
                var accounts = await app.GetAccountsAsync();

                AuthenticationResult result = null;
                if (accounts.Any())
                {
                    result = await app.AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                                      .ExecuteAsync();
                }
                else
                {
                    try
                    {
                        var securePassword = new SecureString();
                        foreach (char c in AdminPassword)        // you should fetch the password
                            securePassword.AppendChar(c);  // keystroke by keystroke

                        result = await app.AcquireTokenByUsernamePassword(scopes, AdminUserName, securePassword)
                                           .ExecuteAsync();
                    }
                    catch (MsalException ex)
                    {
                        // See details below
                        //var msess = ex.ToString();
                        //return msess;
                      //  System.Diagnostics.Debug.WriteLine(ex.ToString());
                    }
                }
                //result = null;
                return result.AccessToken;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
