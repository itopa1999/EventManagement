using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Interface
{
    public interface IJWTService
    {
        int GenerateToken();
    }
}