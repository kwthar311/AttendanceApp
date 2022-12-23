using System;
using System.Collections.Generic;

namespace AttendanceApp.Models
{
    public partial class Attendance
    {
        public int Id { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public int EmployeeId { get; set; }

        public virtual Employee Employee { get; set; } = null!;
    }
}
