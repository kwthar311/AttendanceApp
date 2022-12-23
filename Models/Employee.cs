using System;
using System.Collections.Generic;

namespace AttendanceApp.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Attendances = new HashSet<Attendance>();
            WorkDays = new HashSet<WorkDay>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<WorkDay> WorkDays { get; set; }
    }
}
