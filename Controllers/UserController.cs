using HomeworkFinal.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkFinal.Controllers
{
    public class UserController : Controller
    {
        [EnableCors("賴又德上課不要滑手機Policy")]
        [HttpPost("Authorize")]
        public IActionResult Authorize([FromBody] User request)
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

        [EnableCors("賴又德上課不要滑手機Policy")]
        [HttpPost("Register")]
        public IActionResult Register([FromBody] User request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            HomeworkFinal.DALs.UserDAL obj = new HomeworkFinal.DALs.UserDAL();

            // 呼叫剛剛寫好的 Register 方法
            bool isSuccess = obj.Register(request.Name, request.Password);

            if (isSuccess)
            {
                return Ok(new
                {
                    Message = "註冊成功！",
                    Data = request
                });
            }
            else
            {
                // 可能是帳號重複或資料庫寫入失敗
                return Ok(new
                {
                    Message = "註冊失敗：帳號可能已被使用",
                    Data = request
                });
            }
        }
    }
}
