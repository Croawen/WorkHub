using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WorkHub.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace WorkHub.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public static ApplicationUser GetCurrentUser(string userName)
        {
            // We can't use _db field because we are in a static context.
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = userManager.FindByName(userName);
            return user;
        }

        public static int GetCurrentLayout()
        {
            // NullReferenceException is possible if we drop all data from Settings table but we assume that noone will tinker with it.
            return new ApplicationDbContext().Settings.FirstOrDefault().LayoutType;
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: /Manage/Index
        [HttpGet]
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.Categories = _db.Categories.ToList();
            var id = User.Identity.GetUserId();

            ViewBag.UserJobs = _db.WorkOrders.Where(x => x.UserRefId == id).ToList();

            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";
            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId),
                Setting = _db.Settings.FirstOrDefault(x => x.Id == 1)             
            };
            return View(model);
        }

        // GET: /Manage/ChangePassword
        [HttpGet]
        public ActionResult ChangePassword()
        {
            ViewBag.Categories = _db.Categories.ToList();
            return View();
        }

        // POST: /Manage/ChangeLayout
        [HttpPost]
        public ActionResult ChangeLayout(FormCollection Form)
        {
            var select = Request["layoutoptions"];
            if(select != null)
            {
                var setting = _db.Settings.FirstOrDefault();
                if (setting != null)
                {
                    setting.LayoutType = int.Parse(select);
                    _db.Entry(setting).State = EntityState.Modified;
                }
                _db.SaveChanges();
            }
            ViewBag.Categories = _db.Categories.ToList();
            return RedirectToAction("Index");
        }

        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        // GET: /Manage/SetPassword
        [HttpGet]
        public ActionResult SetPassword()
        {
            ViewBag.Categories = _db.Categories.ToList();
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

#region Helpers
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}