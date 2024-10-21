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
        public readonly SignInManager<User> _signInManager;
        public readonly IJWTService _token;
        public readonly IAdminInterface _adminRepo;

        public AdminController(
             DBContext context,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IJWTService token,
            IAdminInterface adminRepo
        )
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _token = token;
            _adminRepo = adminRepo;
        }

 
















    }
}