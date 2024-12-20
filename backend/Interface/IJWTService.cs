using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Interface
{
    public interface IJWTService
    {
        int GenerateToken();
        string? CreateJwtTokenAsync (User user);
        string? GenerateAccessToken();
    }
}