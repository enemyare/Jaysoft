using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MerosWebApi.Application.Interfaces
{
    public interface IAuthHelper
    {
        Guid GetUserId(ControllerBase controller);
    }
}