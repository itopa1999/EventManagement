using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos;
using backend.Models;

namespace backend.Interface
{
    public interface IAdminInterface
    {
        Task<BlockAccessDto> BlockAccessDtoAsync (HttpContext context);
    }
}