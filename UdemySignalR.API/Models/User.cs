using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UdemySignalR.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //navigation property
        public virtual Team Team { get; set; } // 1->1 ilişki
    }
}
