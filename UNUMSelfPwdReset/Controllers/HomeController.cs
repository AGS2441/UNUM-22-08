using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UNUMSelfPwdReset.Models;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using UNUMSelfPwdReset.Utilities;
using Azure;
using UNUMSelfPwdReset.Managers;
using System.Security.Cryptography;

namespace UNUMSelfPwdReset.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly GraphServiceClient _graphServiceClient;
        private readonly PasswordResetService _passwordResetService;
        private readonly AzureAdminActionManager _azureAdminActionManager;
        private readonly IConfiguration _config;

        private readonly LoginsManager _loginsManager;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,
            GraphServiceClient graphServiceClient, LoginsManager loginsManager
            , PasswordResetService passwordResetService, AzureAdminActionManager azureAdminActionManager
           , IConfiguration config)
        {
            _logger = logger;
            _graphServiceClient = graphServiceClient;
            _passwordResetService = passwordResetService;
            _loginsManager = loginsManager;
            _azureAdminActionManager = azureAdminActionManager;
            _config = config;
        }

        [AuthorizeForScopes(ScopeKeySection = "MicrosoftGraph:Scopes")]
        public async Task<IActionResult> Dashboard()
        {
            var me = await _graphServiceClient.Me.Request().GetAsync();
            var userInfo = CopyHandler.UserProperty(me);
            try
            {
                var user = await _graphServiceClient.Users[me.UserPrincipalName]
                   .Request()
                   .Select("lastPasswordChangeDateTime")
                   .GetAsync();

                userInfo.LastPasswordChangeDateTime = user?.LastPasswordChangeDateTime?.DateTime;
                userInfo.LoginClients = await _loginsManager.GetUserLogins(userInfo?.Id, userInfo?.UserPrincipalName, userInfo?.LastPasswordChangeDateTime);
                string strProfilePicBase64 = "";
                try
                {
                    var profilePic = await _graphServiceClient.Me.Photo.Content.Request().GetAsync();
                    using StreamReader? reader = profilePic is null ? null : new StreamReader(new CryptoStream(profilePic, new ToBase64Transform(), CryptoStreamMode.Read));
                    strProfilePicBase64 = reader is null ? null : await reader.ReadToEndAsync();
                }
                catch (Exception ex)
                {

                    strProfilePicBase64 = "";
                }
                if (userInfo.GivenName != null)
                {
                    HttpContext.Session.SetString("FirstName", userInfo.GivenName?.ToString());
                }
                if (strProfilePicBase64 != null)
                {
                    HttpContext.Session.SetString("Profilepic", strProfilePicBase64.ToString());
                }
                if (userInfo.Surname != null)
                {
                    HttpContext.Session.SetString("LastName", userInfo.Surname?.ToString());
                }



                return View(userInfo);
            }
            catch (Exception ex)
            {

                TempData.SetObjectAsJson("PopupViewModel", StaticMethods.CreatePopupModel("Home", ex.Message));
            }
            return View(userInfo);
        }
        public async Task<IActionResult> Index()
        {

            var me = await _graphServiceClient.Me.Request().GetAsync();
            var userInfo = CopyHandler.UserProperty(me);
            try
            {
                var user = await _graphServiceClient.Users[me.UserPrincipalName]
                       .Request()
                       .Select("lastPasswordChangeDateTime")
                       .GetAsync();

                userInfo.LastPasswordChangeDateTime = user?.LastPasswordChangeDateTime?.DateTime;
                userInfo.LoginClients = await _loginsManager.GetUserLogins(userInfo?.Id, userInfo?.UserPrincipalName, userInfo?.LastPasswordChangeDateTime);
                string strProfilePicBase64 = "";
                try
                {
                    var profilePic = await _graphServiceClient.Me.Photo.Content.Request().GetAsync();
                    using StreamReader? reader = profilePic is null ? null : new StreamReader(new CryptoStream(profilePic, new ToBase64Transform(), CryptoStreamMode.Read));
                    strProfilePicBase64 = reader is null ? null : await reader.ReadToEndAsync();
                }
                catch (Exception ex)
                {

                    strProfilePicBase64 = "";
                }
                //HttpContext.Session.SetString("FirstName", userInfo.GivenName.ToString());
                //HttpContext.Session.SetString("LastName", userInfo.Surname.ToString());
                //HttpContext.Session.SetString("Profilepic", strProfilePicBase64.ToString());
                if (userInfo.GivenName != null)
                {
                    HttpContext.Session.SetString("FirstName", userInfo.GivenName?.ToString());
                }
                if (strProfilePicBase64 != null)
                {
                    HttpContext.Session.SetString("Profilepic", strProfilePicBase64.ToString());
                }
                if (userInfo.Surname != null)
                {
                    HttpContext.Session.SetString("LastName", userInfo.Surname?.ToString());
                }

            }
            catch (Exception ex)
            {

                TempData.SetObjectAsJson("PopupViewModel", StaticMethods.CreatePopupModel("Home", ex.Message));
            }
            return View(userInfo);
        }


        [HttpGet]
        public async Task<IActionResult> ResetPassword()
        {
            var me = await _graphServiceClient.Me.Request().GetAsync();
            ResetPasswordRequest resetPasswordRequest = new ResetPasswordRequest()
            {
                AzureAD = LoginClientType.AzureAD,
                Id = me.Id,
                Username = me.GivenName
            };

            return View(resetPasswordRequest);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetPassword)
        {
            if (ModelState.IsValid)
            {
                //await _passwordResetService.ResetUserPasswordAsync(resetPassword);
                string token = await _azureAdminActionManager.GetAdminTokenForGraph();
                var response = await _passwordResetService.ResetUserPasswordAsync(token, resetPassword);
                if (response == "true")
                {
                    TempData.SetObjectAsJson("PopupViewModel", StaticMethods.CreatePopupModel("Home", "Password Changed Successfully !"));
                }
                else
                {
                    TempData.SetObjectAsJson("PopupViewModel", StaticMethods.CreatePopupModel("Home", response));
                }


                return RedirectToAction("Index");
            }

            return View(resetPassword);
        }


        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}