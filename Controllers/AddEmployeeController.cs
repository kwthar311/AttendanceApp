using AttendanceApp.Models;
using AttendanceApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Text.Encodings.Web;
using System.Text;

namespace AttendanceApp.Controllers
{
    public class AddEmployeeController : Controller
    {
        private readonly attendanceDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IToastNotification _toastNotification;
        private readonly IEmailSender _emailSender;
        public AddEmployeeController(attendanceDBContext context, UserManager<IdentityUser> userManager, IToastNotification toastNotification, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _emailSender = emailSender;
        }
         public async Task<IActionResult> Index(CreateEmployee e)
        {
            var EmailValidation = _context.Employees.FirstOrDefault(x => x.Email == e.Email);
            if (EmailValidation != null)
            {
                ModelState.AddModelError("Employee.Email", " Email already exists");
                _toastNotification.AddErrorToastMessage("Email already exists");
                return View();
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            using (var transaction2 = _context.Database.BeginTransaction())
            {
                try
                {
                    var employee = new Employee();
                    employee.Name = e.Name;
                    employee.Email = e.Email;
                    employee.Password = e.Password;
                    _context.Employees.Add(employee);
                    _context.SaveChanges();

                    var emp = _context.Employees.FirstOrDefault(x => x.Email.Equals(e.Email));

                    if (emp != null)
                    {

                        var emp_work = new WorkDay();
                        emp_work.WorkDay1 = e.WorkDay;
                        emp_work.StandardCheckIn = e.StandardCheckIn;
                        emp_work.StandardCheckOut = e.StandardCheckOut;
                        emp_work.EmployeeId = emp.Id;

                        _context.WorkDays.Add(emp_work);
                        _context.SaveChanges();

                    }


                    if (await _userManager.FindByEmailAsync(e.Email) != null || await _userManager.FindByNameAsync(e.Email) != null)
                    {
                        ModelState.AddModelError("Email", "Email already exists!");
                        _toastNotification.AddErrorToastMessage("Email already exists");
                        return View(e);
                    }
                    IdentityUser user = new IdentityUser
                    {
                        UserName = e.Email,
                        Email = e.Email,
                    };

                    var result = await _userManager.CreateAsync(user, e.Password);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(e);
                    }

                    await _userManager.AddToRoleAsync(user, "User");

                 

                    var UserReset = await _userManager.FindByEmailAsync(e.Email);
                    if (UserReset == null)
                    {
                        // Don't reveal that the user does not exist or is not confirmed
                        //return RedirectToPage("./ForgotPasswordConfirmation");

                        _toastNotification.AddErrorToastMessage("The user does not exist ");
                        return View(e);
                    }

                    
                    var code = await _userManager.GeneratePasswordResetTokenAsync(UserReset);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ResetPassword",
                        pageHandler: null,
                        values: new { area = "Identity", code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(
                        user.Email,
                        "Reset Password",
                        $"Your Information<br/>Email : {user.Email}<br/>Passwod : {e.Password}<br/>Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");


                    transaction2.Commit();
                    _toastNotification.AddSuccessToastMessage("Employee created successfully");

                    return LocalRedirect("~/Admin/");
                }
                catch (Exception)
                {
                    transaction2.Rollback();
                }
            }
            //_toastNotification.AddSuccessToastMessage("Movie created successfully");
            return View(e);
        }


        
    }
}
