using AttendanceApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace AttendanceApp.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly attendanceDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IToastNotification _toastNotification;

        public EmployeeController(attendanceDBContext context, UserManager<IdentityUser> userManager, IToastNotification toastNotification)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CheckIn ()
        {
            string currentUserName = User.Identity.Name;
            Employee currentUser = _context.Employees.FirstOrDefault(x => x.Email == currentUserName);

            if (currentUser != null)
            {
                var employeeA = new Attendance();
                //string sqlTimeAsString = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
                var AttendanceUser = _context.Attendances.FirstOrDefault(x => x.CheckIn.Value.Date == DateTime.Now.Date && x.EmployeeId == currentUser.Id);
                if (AttendanceUser != null)
                {
                    _toastNotification.AddErrorToastMessage("Some thing wrong you have been already Checked in");
                    return View("Index");
                }

                employeeA.CheckIn = DateTime.Now;
                employeeA.EmployeeId = currentUser.Id;
                _context.Attendances.Add(employeeA);
                _context.SaveChanges();
            }

            _toastNotification.AddSuccessToastMessage("Verify that the check in operation was successful");
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CheckOut()
        {
            string currentUserName = User.Identity.Name;
            Employee currentUser = _context.Employees.FirstOrDefault(x => x.Email == currentUserName);

            if (currentUser != null)
            {
                var employeeA = new Attendance();
                //string sqlTimeAsString = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
                var AttendanceUser = _context.Attendances.FirstOrDefault(x => x.CheckIn.Value.Date == DateTime.Now.Date && x.EmployeeId == currentUser.Id);

                if(AttendanceUser != null && AttendanceUser.CheckOut!=null)
                {
                    _toastNotification.AddErrorToastMessage("Some thing wrong you have been already Checked out");
                    return View("Index");
                }
                else if (AttendanceUser != null)
                {
                    AttendanceUser.CheckOut = DateTime.Now;
                    _context.Update(AttendanceUser);
                    _context.SaveChanges();
                    _toastNotification.AddSuccessToastMessage("Verify that the check out operation was successful");
                }
                
                else
                {
                    _toastNotification.AddWarningToastMessage("You haven't been checked in, you can check out now but please remember to check in before");
                    employeeA.CheckOut = DateTime.Now;
                    employeeA.EmployeeId = currentUser.Id;
                    _context.Attendances.Add(employeeA);
                    _context.SaveChanges();
                    _toastNotification.AddSuccessToastMessage("Verify that the check out operation was successful");
                }

            }

            return View("Index");
        }
    }
}
