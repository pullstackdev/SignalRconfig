using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR; //for IHubContext
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemySignalR.API.Hubs; //for MyHub

namespace UdemySignalR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase //clientlara bildirim gönderilecek
    {
        //mvc ya da api controllerlar içerisinde hub üzerinden clientlara data göndermek için IHubContext'i dependency injection olarak geçeceğiz. (hub dışından gönderme yöntemi)
        private readonly IHubContext<MyHub> _hubContext; //private nesneler _ ile başlaması gelenek
        public NotificationController(IHubContext<MyHub> hubContext)
        {
            _hubContext = hubContext;
        }

        //endpoint oluşturup bunun üzerinden bildirimleri iletelim.. api/Notification/11 gibi
        [HttpGet("{teamCount}")]
        public async Task<IActionResult> Index(int teamCount)
        {
            MyHub.TeamCount = teamCount; //api aracılığı ile teamCount değişirse yakalayalım

            await _hubContext.Clients.All.SendAsync("Notify", $"Takımlar {teamCount} kişilik olmalıdır."); // Notify' subscribe olan all clientlara data gidecek. MyHub daki aynı şlem ve metodlar kullanılır farkı yok
            return Ok(); //IActionResult eturn tipi belirtir
        }
    }
}
