using AttendanceApp.Models;
using AttendanceApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace AttendanceApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly AttendanceApp.Models.attendanceDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(attendanceDBContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public List<EmployeesDatum> EmployeesReport { get; set; }
        public async Task<IActionResult> Index()
        {
            EmployeesReport = await _context.EmployeesData.FromSqlRaw(@"SELECT        Id, Name, WorkDay, StandardCheckIn, CheckIn,
                             (SELECT        CASE WHEN CAST(CheckIn AS time) > StandardCheckIn THEN 'True' ELSE 'False' END AS Expr1) AS ArriveLate, StandardCheckOut, CheckOut,
                             (SELECT        CASE WHEN CAST(CheckOut AS time) < StandardCheckOut THEN 'True' ELSE 'False' END AS r) AS LeaveEarly
FROM            (SELECT        dbo.Employees.Id, dbo.Employees.Name, dbo.WorkDays.WorkDay, dbo.WorkDays.StandardCheckIn, dbo.WorkDays.StandardCheckOut,
                                                        (SELECT        CheckIn
                                                          FROM            dbo.Attendance
                                                          WHERE        (EmployeeId = dbo.Employees.Id) AND (CONVERT(Date, CheckIn, 111) = dbo.WorkDays.WorkDay)) AS CheckIn,
                                                        (SELECT        CheckOut
                                                          FROM            dbo.Attendance AS Attendance_1
                                                          WHERE        (EmployeeId = dbo.Employees.Id) AND (CONVERT(Date, CheckIn, 111) = dbo.WorkDays.WorkDay)) AS CheckOut
                          FROM            dbo.WorkDays RIGHT OUTER JOIN
                                                    dbo.Employees ON dbo.WorkDays.EmployeeID = dbo.Employees.Id) AS derivedtbl_1").ToListAsync();
            return View(EmployeesReport);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Filter(FilterViewModelcs f)
        {

            EmployeesReport = await _context.EmployeesData.FromSqlRaw(@"SELECT        Id, Name, WorkDay, StandardCheckIn, CheckIn,
                             (SELECT        CASE WHEN CAST(CheckIn AS time) > StandardCheckIn THEN 'True' ELSE 'False' END AS Expr1) AS ArriveLate, StandardCheckOut, CheckOut,
                             (SELECT        CASE WHEN CAST(CheckOut AS time) < StandardCheckOut THEN 'True' ELSE 'False' END AS r) AS LeaveEarly
FROM            (SELECT        dbo.Employees.Id, dbo.Employees.Name, dbo.WorkDays.WorkDay, dbo.WorkDays.StandardCheckIn, dbo.WorkDays.StandardCheckOut,
                                                        (SELECT        CheckIn
                                                          FROM            dbo.Attendance
                                                          WHERE        (EmployeeId = dbo.Employees.Id) AND (CONVERT(Date, CheckIn, 111) = dbo.WorkDays.WorkDay)) AS CheckIn,
                                                        (SELECT        CheckOut
                                                          FROM            dbo.Attendance AS Attendance_1
                                                          WHERE        (EmployeeId = dbo.Employees.Id) AND (CONVERT(Date, CheckIn, 111) = dbo.WorkDays.WorkDay)) AS CheckOut
                          FROM            dbo.WorkDays RIGHT OUTER JOIN
                                                    dbo.Employees ON dbo.WorkDays.EmployeeID = dbo.Employees.Id) AS derivedtbl_1").ToListAsync();


            if (f.From!= null)
            {
                EmployeesReport = EmployeesReport.Where(p => p.WorkDay >= f.From).ToList();

            }


            if (f.To != null)
            {
                EmployeesReport = EmployeesReport.Where(p => p.WorkDay <= f.To).ToList();
            }

            if (f.ArriveLate!= null)
            {
                
                EmployeesReport = EmployeesReport.Where(p => p.ArriveLate == f.ArriveLate).ToList();
            }

            if (f.LeaveEarly != null)
            {
               
                EmployeesReport = EmployeesReport.Where(p => p.LeaveEarly == f.LeaveEarly).ToList();
            }

            return View("Index", EmployeesReport);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Password")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEmployee e)
        {
            var EmailValidation = _context.Employees.FirstOrDefault(x => x.Email == e.Email);
            if (EmailValidation != null)
            {
                ModelState.AddModelError("Employee.Email", " Email already exists");
                return View();
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

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

              

                    return RedirectToAction(nameof(Index));
                
           
        }
    }
}
