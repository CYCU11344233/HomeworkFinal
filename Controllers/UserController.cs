using HomeworkFinal.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkFinal.Controllers
{
    public class UserController : Controller
    {
        [EnableCors("賴又德上課不要滑手機Policy")]
        [HttpPost("Register")]
        public IActionResult Register([FromBody] User request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            HomeworkFinal.DALs.UserDAL obj = new HomeworkFinal.DALs.UserDAL();
            Boolean isAuthorized = obj.Authorize(request.Name, request.Password);
            if (isAuthorized)
            {
                return Ok(new
                {
                    Message = "User is authorized",
                    Data = request
                });
            }
            else
            {
                return Ok(new
                {
                    Message = "User is not authorized",
                    Data = request
                });
            }

        }
    }
}
