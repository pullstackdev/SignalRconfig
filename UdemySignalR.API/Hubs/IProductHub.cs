using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemySignalR.API.Models;

namespace UdemySignalR.API.Hubs
{
    public interface IProductHub
    {
        //clientlarda çalışacak olan metodlar buraya yazılır böylece hubda isim hatası engellenir
        Task ReceiveProduct(Product product);
    }
}
