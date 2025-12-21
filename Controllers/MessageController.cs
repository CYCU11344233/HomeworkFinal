using HomeworkFinal.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkFinal.Controllers
{
    public class MessageController : Controller
    {
        [EnableCors("賴又德上課不要滑手機Policy")]
        [HttpPost("Release")]
        public IActionResult ReleaseMessage([FromBody] Message request)
        {
            // 1. 檢查輸入格式 (Subject, Content)
            if (string.IsNullOrEmpty(request.Subject) || string.IsNullOrEmpty(request.Content))
            {
                return BadRequest("主旨或內容不能為空");
            }

            // 2. 自動補上後端資訊
            // 注意：這裡暫時寫死或從前端傳過來，實務上通常從 Session 或 Token 抓取
            request.PublishTime = DateTime.Now;
            if (string.IsNullOrEmpty(request.Publisher))
            {
                request.Publisher = "Anonymous"; // 如果沒傳作者，預設為匿名
            }

            // 3. 呼叫 DAL
            HomeworkFinal.DALs.MessageDAL obj = new HomeworkFinal.DALs.MessageDAL();
            bool isSuccess = obj.ReleaseMessage(request);

            if (isSuccess)
            {
                return Ok(new { Message = "發佈成功", Data = request });
            }
            return StatusCode(500, "資料庫寫入失敗");
        }

        [EnableCors("賴又德上課不要滑手機Policy")]
        [HttpGet("MessageList")]
        public IActionResult GetMessageList(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("請提供使用者名稱");
            }

            HomeworkFinal.DALs.MessageDAL obj = new HomeworkFinal.DALs.MessageDAL();
            List<Message> userMessages = obj.GetMessagesByUser(name);

            return Ok(new
            {
                Message = $"找到 {userMessages.Count} 筆留言",
                Data = userMessages
            });
        }
    }
}
