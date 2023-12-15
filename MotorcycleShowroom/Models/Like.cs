using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MotorcycleShowroom.Models
{
    public class Like
    {
            public int Id { get; set; }
            public int PostId { get; set; }
            public string UserId { get; set; }


        

    }

}

