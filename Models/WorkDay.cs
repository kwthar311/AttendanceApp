using System;
using System.Collections.Generic;

namespace AttendanceApp.Models
{
    public partial class WorkDay
    {
        public int Id { get; set; }
        public DateTime WorkDay1 { get; set; }
        public TimeSpan StandardCheckIn { get; set; }
        public TimeSpan StandardCheckOut { get; set; }
        public int EmployeeId { get; set; }

        public virtual Employee Employee { get; set; } = null!;
    }
}
