using System;
using System.Collections.Generic;
using MainAPI.Models;

namespace MainAPI.Dtos
{
    public record UserDto
    {
       public Guid Id { get; init; }
        public string FirstName {get; init;}
        public string LastName { get; set; }
         public List<string> PhotoFileLocations { get; set; }
        public string username { get; init; } 
        public  string emailAddress {get; set;}

        public decimal Balance {get; set;}
        
       public string Password { get; set; }

       public DateTimeOffset CreatedDate {get; set;}

        public List<User> Friends {get; set;}
        public List<double> SongIds {get; set;} 

    }
}