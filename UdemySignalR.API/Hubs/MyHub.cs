using Microsoft.AspNetCore.SignalR; //for Hub
using Microsoft.EntityFrameworkCore; //for Include
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemySignalR.API.Models;

namespace UdemySignalR.API.Hubs
{
    public class MyHub : Hub //inherit edilmeli
    {
        //db işlemleri için dependency injection alındı
        private readonly AppDbContext _context; //startupda tanımlandığı için tek yerden çağırılabilir context nesnesi, eğer startupda tanımlanmasaydı
        public MyHub(AppDbContext context)
        {
            _context = context;
        }

        private static List<string> Names { get; set; } = new List<string>(); //clienttan gelen verileri static listte tutmamın sebebi şuan dbye gerek yok ve her seferinde new'lenirken kaybolmasın diye.
        private static int ClientCount { get; set; } = 0; //bağlanan kullanıcı sayısını tutsun
        public static int TeamCount { get; set; } = 7; //takım sayısı 7 kişilik olsun

        //clienttan buradaki metodlar (server metod) çağırılacak(istek atılacal message parametresi ile)
        public async Task SendName(string name) //Task sync daki voiddir
        {
            if (Names.Count>= TeamCount)
            {
                await Clients.Caller.SendAsync("Error", $"Takım {TeamCount} olmalıdır."); //all yerine caller sadece istekte bulunan clienta (7.den sonraki kişiyi ekleyen clienta error gidecek) data gidecek demektir
            }
            else
            {
                Names.Add(name);

                await Clients.All.SendAsync("ReceiveName", name); //bu hub'a bağlı olan tüm clientlara (all ile) data'yı gönderdik ve clientta çalışacak metod ReceiveName (client metod)
            }
           
        }

        public async Task GetNames() //istersek buna api ilede ulaşıp tüm verileri db den de alabiliriz
        {
            await Clients.All.SendAsync("ReceiveNames", Names); //client'lar (all ile tümüne) tüm name'lere bu GetNames metodu ile ulaştı
        }

        public async override Task OnConnectedAsync() //client hub'a bağlandıkça çalışan metod
        {
            ClientCount++;

            await Clients.All.SendAsync("ReceiveClientCount", ClientCount); //sayıyı all clientlara gönder

            await base.OnConnectedAsync();
        }
        public async override Task OnDisconnectedAsync(Exception exception) //client hub'dan ayrıldıkça disconnect oldukça çalışan metod
        {
            ClientCount--;

            await Clients.All.SendAsync("ReceiveClientCount", ClientCount); //sayıyı all clientlara gönder, clientda ReceiveClientCount'a subscribe olanlara bu sayı gidecektir

            await base.OnDisconnectedAsync(exception);
        }

        //Groups
        public async Task AddToGroup(string teamName) //client'ı gruba ekleme
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, teamName);
        }
        public async Task RemoveToGroup(string teamName) //client'ı gruptan çıkarma
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, teamName);
        }
        public async Task SendNameByGroup(string clientName, string teamName) //gruba client ismini ekleme
        {
            var team = _context.Teams.Where(x => x.Name == teamName).FirstOrDefault();
            if (team != null) //bu grup var mı?
            {
                team.Users.Add(new User { Name = clientName });
                // _context.User.Add( de kullanılabilir
            }
            else
            {
                //yeni team oluşturup bu clientı buna ekle
                var newTeam = new Team { Name = teamName };
                newTeam.Users.Add(new User { Name = clientName });
                _context.Teams.Add(newTeam);
            }
            await _context.SaveChangesAsync();

            //client teamName isimli Group'a üye ise bu mesajı alabilir.
            await Clients.Group(teamName).SendAsync("ReceiveMessageByGroup", clientName, team.Id);
        }
        //yeni gelen client tüm datayı çekip görebilsin, sadece kendi takımındakileri görebilir yapılabilir biz hepsini görsün yapalım
        public async Task GetNamesByGroup()
        {
            var teams = _context.Teams.Include(x => x.Users).Select(x => new
            {
                //custom bir class oluşturalım direk entity değil, her 2side uygun
                teamId = x.Id,
                Users = x.Users.ToList()
            }); //Include for users in team

            await Clients.All.SendAsync("ReceiveNamesByGroup", teams);
        }

        //hub complex type parameter (clienttan huba class(complex type parameter) gönderme, complext type: class (Product))
        public async Task SendProduct(Product product) //bir clienttan bu gelsin bunuda ReceiveProduct ile diğer clientlara hub üzerinden geri gönderelim
        {
            await Clients.All.SendAsync("ReceiveProduct", product);
        }
    }
}
