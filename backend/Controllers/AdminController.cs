using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Interface;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("admin/api/")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        public readonly DBContext _context;
        public readonly UserManager<User> _userManager;
        public readonly IAdminInterface _adminRepo;

        public AdminController(
            DBContext context,
            UserManager<User> userManager,
            IAdminInterface adminRepo
        )
        {
            _context = context;
            _userManager = userManager;
            _adminRepo = adminRepo;
        }

        

 
















    }
}