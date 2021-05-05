using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemySignalR.API.Models;

namespace UdemySignalR.API.Hubs
{
    public class ProductHub:Hub<IProductHub>
    {
        public async Task SendProduct(Product product) //bir clienttan bu gelsin bunuda ReceiveProduct ile diğer clientlara hub üzerinden geri gönderelim
        {
            await Clients.All.ReceiveProduct(product); //sendaysnc istemede direk ReceiveProduct metoduna ulaştı (strongly type hatayı engeller)
        }
    }
}
