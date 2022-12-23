using System;
using System.Collections.Generic;

namespace AttendanceApp.Models
{
    public partial class EmployeesDatum
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime? WorkDay { get; set; }
        public TimeSpan? StandardCheckIn { get; set; }
        public DateTime? CheckIn { get; set; }
        public string? ArriveLate { get; set; }
        public TimeSpan? StandardCheckOut { get; set; }
        public DateTime? CheckOut { get; set; }
        public string? LeaveEarly { get; set; }
    }
}
