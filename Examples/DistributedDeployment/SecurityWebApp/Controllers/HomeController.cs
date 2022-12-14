using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using ServiceBrick;
using ServiceBrick.Security.Api;
using ServiceBrick.ServiceBus;
using WebApp.ViewModel.Home;

namespace WebApp.Controllers
{
    [AllowAnonymous]
    [Route("")]
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly IBusinessRuleService _businessRuleService;
        private readonly IUserManagerService _userManagerService;
        private readonly IServiceBus _serviceBus;

        public HomeController(
            IBusinessRuleService businessRuleService,
            IUserManagerService userManagerService,
            IServiceBus serviceBus)
        {
            _businessRuleService = businessRuleService;
            _userManagerService = userManagerService;
            _serviceBus = serviceBus;
        }

        [HttpGet]
        [Route("")]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            // Check if admin users are found
            var respAdminUsers = await _userManagerService.GetUsersInRoleAsync(SecurityApiConstants.ROLE_ADMIN_NAME);
            if (respAdminUsers.List == null || respAdminUsers.List.Count == 0)
                return LocalRedirect("/RegisterAdmin");

            HomeViewModel model = new HomeViewModel();
            return View(model);
        }

        [HttpGet]
        [Route("Error")]
        public IActionResult Error(string message = null)
        {
            var model = new ErrorViewModel()
            {
                Message = message
            };
            return View("Error", model);
        }

        [HttpPost]
        [Route("SetLanguage")]
        [ValidateAntiForgeryToken]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            var feature = HttpContext.Features.Get<ITrackingConsentFeature>();
            if (feature != null && feature.HasConsent)
            {
                // Consented, use cookie
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

                if (string.IsNullOrEmpty(returnUrl))
                    return LocalRedirect("/");
                else
                    return LocalRedirect(returnUrl);
            }
            else
            {
                // Not consented, use querystring
                if (string.IsNullOrEmpty(returnUrl))
                    return LocalRedirect("/?culture=" + culture);
                else
                {
                    if (returnUrl.Contains('?'))
                        return LocalRedirect(returnUrl + "&culture=" + culture);
                    else
                        return LocalRedirect(returnUrl + "?culture=" + culture);
                }
            }
        }

        [HttpPost]
        [Route("WithdrawCookiesConsent")]
        [ValidateAntiForgeryToken]
        public IActionResult WithdrawCookiesConsent()
        {
            var feature = HttpContext.Features.Get<ITrackingConsentFeature>();
            feature?.WithdrawConsent();

            //Delete all other cookies used in the site
            Response.Cookies.Delete(CookieRequestCultureProvider.DefaultCookieName);

            return LocalRedirect("/");
        }

        [HttpGet]
        [Route("SendEmail")]
        public IActionResult SendEmail()
        {
            return View(new SendEmailViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("SendEmail")]
        public async Task<IActionResult> SendEmail(SendEmailViewModel model)
        {
            if (!ModelState.IsValid)
                return View("SendEmail", model);

            ApplicationEmailDto email = new ApplicationEmailDto()
            {
                ToAddress = model.Email,
                Subject = model.Subject,
                Body = model.Body
            };
            CreateApplicationEmailBroadcast broadcast = new CreateApplicationEmailBroadcast(email);
            await _serviceBus.Send(broadcast);

            return View("SendEmailComplete");
        }

        [HttpGet]
        [Route("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin()
        {
            // Check if admin users are found
            var respAdminUsers = await _userManagerService.GetUsersInRoleAsync(SecurityApiConstants.ROLE_ADMIN_NAME);
            if (respAdminUsers.List == null || respAdminUsers.List.Count == 0)
                return View("RegisterAdmin");
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin(RegisterAdminViewModel model)
        {
            // Check if admin users are found
            var respAdminUsers = await _userManagerService.GetUsersInRoleAsync(SecurityApiConstants.ROLE_ADMIN_NAME);
            if (respAdminUsers.List != null && respAdminUsers.List.Count > 0)
                return NotFound();

            // Create admin user process
            UserRegisterAdminProcess processRegisterAdmin = new UserRegisterAdminProcess(
                model.Email,
                model.Password);

            // Execute the process
            var respEvent = await _businessRuleService.ExecuteProcessAsync(processRegisterAdmin);
            if (respEvent.Error)
            {
                ModelState.CopyFromResponse(respEvent);
                return View("RegisterAdmin");
            }

            // Find the user
            var respUser = await _userManagerService.FindByEmailAsync(model.Email);
            if (respUser.Item != null)
            {
                //Log in the user automatically
                await _userManagerService.SignInAsync(respUser.Item.StorageKey, true);
            }

            // Go back to home page
            return LocalRedirect("/");
        }
    }
}