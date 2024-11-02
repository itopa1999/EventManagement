// using backend.Data;
// using backend.Models;
// using backend.Services;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;

// namespace backend.Helpers
// {
//     public class BlockAccessFunction
//     {
        
//         public static async Task<(string message, bool IsSuccess)> BlockUnblockEmailAccess(DbContext context, string email, string status)
//         {
//             if(string.IsNullOrEmpty(email.Trim())){
//                 return ("Email cannot be empty", false);
//             }
//             if(string.IsNullOrEmpty(status.Trim())){
//                 return ("status cannot be empty", false);
//             }
//             if (!StringHelpers.IsValidBlockStatus(status.Trim()))
//             {
//                 return ($"Status can be either 'Activate' or 'Deactivate'", false);
//             }
//             if (!StringHelpers.IsValidEmail(email.Trim()))
//             {
//                 return ($"Invalid email {email}", false);
//             }
//             if (!await email.IsValidEmailAdmin(context))
//                 return ($"Email does not exists.", false);
//             var existingAttendee = await context.Attendees.FirstOrDefaultAsync(x => x.Email == email);
//             if (status == "Deactivate")
//             {
//                 existingAttendee.IsBlock = false;
//                 await context.SaveChangesAsync();
//                 return ("Email has been unblocked successfully", true);
//             }else if (status == "Activate"){
//                 existingAttendee.IsBlock = true;
//                 await context.SaveChangesAsync();
//                 return ("Email has been blocked successfully", true);
//             }
//             else{
//                 return ("Cannot complete the action", false);
//             }
//         }









        
//     }
// }