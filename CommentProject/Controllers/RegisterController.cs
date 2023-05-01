using CommentProject.EntityLayer.Concrete;
using CommentProject.Models.AppUserViewModels;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace CommentProject.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(UserManager<AppUser> userManager, ILogger<RegisterController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(RegisterViewModel registerViewModel)
        {
            var appUser = new AppUser()
            {
                Name = registerViewModel.Name,
                Surname = registerViewModel.Surname,
                Email = registerViewModel.Mail,
                UserName = registerViewModel.UserName,
                Image = "test"

            };
            if (registerViewModel.Password == registerViewModel.ConfirmPassword)
            {
                var result = await _userManager.CreateAsync(appUser, registerViewModel.Password);
                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
                    var confirmationLink = Url.Action("ConfirmEmail", "Register", new {userId = appUser.Id, token=token }, Request.Scheme);
                    MimeMessage mimeMessage = new MimeMessage();
                    MailboxAddress mailboxAddressFrom = new MailboxAddress("Admin", "ismailtopcu92@gmail.com");
                    mimeMessage.From.Add(mailboxAddressFrom);
                    MailboxAddress mailboxAddressTo = new MailboxAddress("User", registerViewModel.Mail);
                    mimeMessage.To.Add(mailboxAddressTo);
                    var bodyBuilder = new BodyBuilder();
                    bodyBuilder.TextBody = confirmationLink;
                    mimeMessage.Body = bodyBuilder.ToMessageBody();
                    mimeMessage.Subject = "Email Onaylama";
                    SmtpClient client = new SmtpClient();
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("ismailtopcu92@gmail.com", "voqmpkusvfnqmhvs");
                    client.Send(mimeMessage);
                    client.Disconnect(true);
                    _logger.Log(LogLevel.Warning, confirmationLink);
                    
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Şifreler birbiriyle uyuşmuyor!");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId ==null || token ==null)
            {
                return RedirectToAction("index", "Login");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"UserID {userId} geçersiz";
                return View();
            }
            var result =await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View();
            }
            ViewBag.ErrorTitle = "Email Onaylanmadı";
            return View();
        }
    }
}
