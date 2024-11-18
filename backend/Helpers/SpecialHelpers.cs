
using System.Globalization;
using backend.Data;
using backend.Dtos;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend.Helpers
{
    public class SpecialHelpers
    {
        private readonly UserManager<User> _userManager;
        private readonly DBContext _context;

        public SpecialHelpers(
            UserManager<User> userManager,
            DBContext context
            )
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<List<MonthlyDataDto>> GetMonthlyUserRegistrationsAsync(int? year)
        {
            
            year ??= DateTime.Now.Year;

        

            var monthlyUserRegistrations = await _userManager.Users
                .Where(u => u.CreatedAt.Year == year && u.UserType == UserType.Admin)
                .GroupBy(u => u.CreatedAt.Month)
                .Select(g => new
                {
                    MonthNumber = g.Key,
                    UserCount = g.Count()
                })
                .ToListAsync();

            var monthNames = new List<MonthlyDataDto>();

            foreach (var month in Enumerable.Range(1, 12)) 
            {
                var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);

                var userCount = monthlyUserRegistrations.FirstOrDefault(r => r.MonthNumber == month)?.UserCount ?? 0;

                monthNames.Add(new MonthlyDataDto
                {
                    Month = monthName,
                    Count = userCount
                });
            }

            return monthNames;
        }


        public async Task<List<MonthlyDataDto>> GetMonthlyEventRegistrationsAsync(int? year)
        {
            
            year ??= DateTime.Now.Year;

        

            var monthlyEventRegistrations = await _context.Events
                .Where(u => u.CreatedAt.Year == year)
                .GroupBy(u => u.CreatedAt.Month)
                .Select(g => new
                {
                    MonthNumber = g.Key,
                    UserCount = g.Count()
                })
                .ToListAsync();

            var monthNames = new List<MonthlyDataDto>();

            foreach (var month in Enumerable.Range(1, 12)) 
            {
                var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);

                var userCount = monthlyEventRegistrations.FirstOrDefault(r => r.MonthNumber == month)?.UserCount ?? 0;

                monthNames.Add(new MonthlyDataDto
                {
                    Month = monthName,
                    Count = userCount
                });
            }

            return monthNames;
        }


        public async Task<List<MonthlyDataDto>> GetMonthlySessionRegistrationsAsync(int? year)
        {
            
            year ??= DateTime.Now.Year;

        

            var monthlySessionRegistrations = await _context.Sessions
                .Where(u => u.CreatedAt.Year == year)
                .GroupBy(u => u.CreatedAt.Month)
                .Select(g => new
                {
                    MonthNumber = g.Key,
                    UserCount = g.Count()
                })
                .ToListAsync();

            var monthNames = new List<MonthlyDataDto>();

            foreach (var month in Enumerable.Range(1, 12)) 
            {
                var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);

                var userCount = monthlySessionRegistrations.FirstOrDefault(r => r.MonthNumber == month)?.UserCount ?? 0;

                monthNames.Add(new MonthlyDataDto
                {
                    Month = monthName,
                    Count = userCount
                });
            }

            return monthNames;
        }


        
    }
}
