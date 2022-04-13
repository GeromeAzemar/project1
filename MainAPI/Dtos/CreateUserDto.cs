using System;

namespace MainAPI.Dtos
{
    public record CreateUserDto
    {
        public string FirstName {get; init;}
        public string LastName { get; set; }
        public string username { get; init; } 
        public  string emailAddress {get; set;}
        
    }

}