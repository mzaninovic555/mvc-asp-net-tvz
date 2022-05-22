using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vjezba.Model;

namespace Vjezba.Web.Controllers
{
    public class BaseController : Controller
    {
        public string UserId { get; set; }

        public BaseController()
        {

        }
    }
}
