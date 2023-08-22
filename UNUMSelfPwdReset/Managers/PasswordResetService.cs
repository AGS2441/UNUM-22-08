using Microsoft.Graph;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;
using UNUMSelfPwdReset.Models;

namespace UNUMSelfPwdReset.Managers
{
    public class PasswordResetService
    {
        private readonly ITokenAcquisition _tokenAcquisition;

        public PasswordResetService(ITokenAcquisition tokenAcquisition)
        {
            _tokenAcquisition = tokenAcquisition;
        }
        

        public async Task<bool> ResetUserPasswordAsync(ResetPasswordRequest resetPasswordRequest)
        {
            try
            {
                string[] scopes = new[] { "user.read"  }; // Add necessary scopes

                var graphServiceClient = GetGraphServiceClient(scopes);



                var user = new User
                { 
                    PasswordProfile = new PasswordProfile
                    {
                        ForceChangePasswordNextSignIn = false,
                        Password = resetPasswordRequest.NewPassword
                    }
                };

                // Reset password and optionally require password change on next sign-in
                await graphServiceClient.Users[resetPasswordRequest.Id]
                .Request()
                .UpdateAsync(user);

                return true;
            }
            catch (ServiceException ex)
            {
                // Handle exceptions
                return false;
            }
        }
        public async Task<string> ResetUserPasswordAsyncsss(string token,ResetPasswordRequest resetPasswordRequest)
        {
            try
            { 

              

                return "true";
            }
            catch (ServiceException ex)
            {
                // Handle exceptions
               // TempData.SetObjectAsJson("PopupViewModel", StaticMethods.CreatePopupModel("Home", "Something went wrong. Please try again later."));
                return ex.Message;
                
            }
        }



        public async Task<string> ResetUserPasswordAsync(string token, ResetPasswordRequest resetPasswordRequest)
        {
            try
            {

                var graphServiceClient = GetAdminGraphServiceClient(token);


                var result = await graphServiceClient.Users[resetPasswordRequest.Id].Authentication.Methods["28c10230-6103-485e-b985-444c60001490"]
                    .ResetPassword(resetPasswordRequest.NewPassword)
                    .Request()
                    .PostAsync();
                //var graphServiceClnient = GetAdminGraphServiceClient(token);

                var user = new User
                {
                    PasswordProfile = new PasswordProfile
                    {
                        ForceChangePasswordNextSignIn = false,
                        Password = resetPasswordRequest.NewPassword
                    }
                };

                // Reset password and optionally require password change on next sign-in
                var resultnew= await graphServiceClient.Users[resetPasswordRequest.Id]
                .Request()
                .UpdateAsync(user);

                //var result = await graphServiceClient.Users[resetPasswordRequest.Id].Authentication.EmailMethods["{emailAuthenticationMethod-id}"]
                //    .ResetPassword(resetPasswordRequest.NewPassword, false)
                //    .Request()
                //    .PostAsync();


                return "true";
            }
            catch (ServiceException ex)
            {
                // Handle exceptions
                // TempData.SetObjectAsJson("PopupViewModel", StaticMethods.CreatePopupModel("Home", "Something went wrong. Please try again later."));
                return ex.Message;

            }
        }
        private GraphServiceClient GetGraphServiceClient(string[] scopes)
        {
            string graphEndpoint = "https://graph.microsoft.com/v1.0";
            var graphClient = new GraphServiceClient(graphEndpoint, new DelegateAuthenticationProvider(async request =>
            {
                var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }));

            return graphClient;
        }

        private GraphServiceClient GetAdminGraphServiceClient( string AdminToken)
        {
            string graphEndpoint = "https://graph.microsoft.com/v1.0";
            var graphClient = new GraphServiceClient(graphEndpoint, new DelegateAuthenticationProvider(async request =>
            { 
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AdminToken);
            }));

            return graphClient;
        }
    }
}
