using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UdemySignalR.API.Models
{
    public class Team
    {
        public Team() // team.users.add( ile eklemele yapılacak ise bu constructor'a gerek var, users.add kullanılacak ise gerek yok
        {
            Users = new List<User>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        //navigation property
        public virtual ICollection<User> Users { get; set; } // 1 -> çok ilişki & virtual çünkü lazyloading ve http bu property üzerindeki  değişiklikleri izleyebilsin & IList ya da INumerable değil burada ICollection çünkü bu interface'in kendi add,clear,contains,copyto,remove metodları var
    }
}
