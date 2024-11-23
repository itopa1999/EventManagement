using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Helpers
{
    public class OrganizerListEventQuery
    {
        public string? Name { get; set; }
        public EventType? EventType { get; set; }
        public string? State { get; set; }
        public string? Location { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }


    public class AttendeeListEventQuery
    {
        public string? Name { get; set; }
        public EventType? EventType { get; set; }
        public string? State { get; set; }
        public string? Location { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class MonthlyDashboardQuery
    {
        public int? UserYear { get; set; }
        public int? EventYear { get; set; }
        public int? SessionYear { get; set; }
    }


    public class AdminListEventQuery
    {
        public string? Name { get; set; }
        public string? EventType { get; set; }
        public string? Location { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsInvitationOnly  { get; set; } = false;
        public bool HasPayment { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 15;

    }

    public class AdminSearchQuery
    {
        public string? Search { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }


    public class AdminTransactionQuery
    {
        public string? Search { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }





}