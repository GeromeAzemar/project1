using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using MainAPI.Models;
using System;
using System.Linq;
using MainAPI.Dtos;
using System.Text;

namespace MainAPI.Repositories
{
  
    public class InMemUsersRepository : IUserRepository
    {

        private string _connectionString;
        public InMemUsersRepository()
        {
            _connectionString = File.ReadAllText("./connectionString.txt");
        }



        List<User> userList = new List<User>();


     public IEnumerable<User> GetUsers()
        {
            return userList;

        }
        
       public User GetUser(string emailAddress)
        {
            return userList.Where(user => user.emailAddress == emailAddress).SingleOrDefault();
        }

       public void CreateUser(User user)
    
        {
          SqlConnection connection = new SqlConnection(_connectionString);
           connection.Open();
            Console.WriteLine(user.emailAddress);
            string id =Convert.ToString(Guid.NewGuid());
            string firstname = user.FirstName;
            string lastname = user.LastName;
            string username = user.username;
            string email = user.emailAddress;
            Console.Write(_connectionString);

            string InsertQueryTxt = ($"Insert into Users values('{id}',{firstname}', '{lastname}', '{username}', '{email}', '{DateTimeOffset.UtcNow}')");
            
            StringBuilder errorMessages = new StringBuilder();
            using(SqlCommand cmd = new SqlCommand(InsertQueryTxt, connection))
            {

                    cmd.ExecuteNonQuery();
             
                   connection.Close();
           }
        }

        public void CreatePassword(User user)
        {
             SqlConnection connection = new SqlConnection(_connectionString);
           connection.Open();
            
            string id = Convert.ToString(user.Id);
            string email = user.emailAddress;
            string password = user.Password;
            string InsertQueryTxt = ($"Insert into Passwords values('{id}','{password}', '{email}')");
            
            StringBuilder errorMessages = new StringBuilder();
            using(SqlCommand cmd = new SqlCommand(InsertQueryTxt, connection))
            {

                    cmd.ExecuteNonQuery();
             
                   connection.Close();
           }
        }
    }
}