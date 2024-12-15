using Microsoft.AspNetCore.Mvc;

namespace MerosWebApi.Application.Interfaces
{
    public interface IAuthHelper
    {
        string GetUserId(ControllerBase controller);
    }
}