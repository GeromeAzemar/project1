using System;
using System.Collections.Generic;
using MainAPI.Dtos;
using MainAPI.Models;

namespace MainAPI.Repositories
{
      public interface IUserRepository
    {
        
        User GetUser(string emailAddress);
        IEnumerable<User> GetUsers();

        public void CreatePassword(User user);

    }

}
