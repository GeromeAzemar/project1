using MainAPI.Dtos;
using MainAPI.Models;

namespace MainAPI
{
    public static class Extensions
    {
        public static UserDto AsDto(this User user)
        {
            return new UserDto
            {
        
            FirstName = user.FirstName,
            LastName = user.LastName,
            username = user.username,
            emailAddress = user.emailAddress

            };
            
        }
    }
}