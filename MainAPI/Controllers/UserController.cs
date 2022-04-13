using System.Runtime.CompilerServices;
using System.Data;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MainAPI.Repositories;
using MainAPI.Models;
using System.Text;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
     public class UserController : ControllerBase
    {
     
        public ILogger<UserController> _logger;
        private readonly IConfiguration _configuration;


        public UserController(IConfiguration configuration, ILogger<UserController> logger)
        {
            _logger = logger;
            _logger.Log(LogLevel.Information, "Program called");
        _configuration = configuration;
        }

    
        [HttpGet]
        
        public JsonResult Get()
        {
            ILogger<UserController> logger = _logger;
            _logger.Log(LogLevel.Information, "Get API");
            string query = @"select FirstName, LastName from dbo.Users";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MainAppCon");
            SqlDataReader myReader;
            using(SqlConnection con = new SqlConnection(sqlDataSource))
            {
                con.Open();
                using(SqlCommand myCommand = new SqlCommand(query, con))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    con.Close();
                     _logger.Log(LogLevel.Warning, "Sucessful Get all users Call");
                }
            }

            return new JsonResult(table);           
        }


        [HttpGet("{email}")]

public JsonResult GetUser(string email)
        {
             ILogger<UserController> logger = _logger;

            string query = @"select FirstName, LastName from dbo.Users where email = @email";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MainAppCon");
            SqlDataReader myReader;
            using(SqlConnection con = new SqlConnection(sqlDataSource))
            {
                con.Open();
                using(SqlCommand myCommand = new SqlCommand(query, con))
                {
                    myCommand.Parameters.AddWithValue("@email",email);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    con.Close();
                  _logger.Log(LogLevel.Information, "Sucess");

                }
            }
           if(table.ToString() == "" )
            return new JsonResult("Email or password incorrect");
            else
            return new JsonResult(table);
        }





         [HttpPost]
        public JsonResult Post(User user)
        {
             ILogger<UserController> logger = _logger;

           
            string query = @"Insert into dbo.Users values (@Id, @FirstName, @LastName, @username, @email, @date)";
            string query2 = @"Insert into dbo.Passwords values (@Id, @password_, @email)";
            string query3 = @"Insert into dbo.Balances values(@Id, @accBalance, @Points)";
            string query4 = @"DELETE FROM Users WHERE Id  = @Id";
           StringBuilder errorMessages = new StringBuilder();

            DataTable table = new DataTable();
            DataTable table2 = new DataTable();
            DataTable table3 = new DataTable(); 
            DataTable table4 = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("MainAppCon");
            SqlDataReader myReader;
            SqlDataReader myReader2;
            SqlDataReader myReader3;
            SqlDataReader myReader4;
            
            using(SqlConnection con = new SqlConnection(sqlDataSource))
            {
                con.Open();
                using(SqlCommand myCommand = new SqlCommand(query, con))
                {
                   
                   
                    user.Id = Guid.NewGuid();
                    user.CreateDate = DateTimeOffset.UtcNow;
                    myCommand.Parameters.AddWithValue("@Id",user.Id);
                    myCommand.Parameters.AddWithValue("@FirstName",user.FirstName);
                    myCommand.Parameters.AddWithValue("@LastName",user.LastName);
                    myCommand.Parameters.AddWithValue("@username",user.username);
                    myCommand.Parameters.AddWithValue("@email",user.emailAddress);
                    myCommand.Parameters.AddWithValue("@date",user.CreateDate);
                    try{
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                   _logger.Log(LogLevel.Warning, "Sucess user inserted " + user.Id + " " +user.CreateDate);
                        }
                      catch (SqlException ex)
                         {
                             for (int i = 0; i < ex.Errors.Count; i++)
                             {
                                 errorMessages.Append("Index #" + i + "\n" +
                                 "Message: " + ex.Errors[i].Message + "\n" +
                                 "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                                "Source: " + ex.Errors[i].Source + "\n" +
                                 "Procedure: " + ex.Errors[i].Procedure + "\n");
                                }
                                return new JsonResult (errorMessages.ToString() + DateTime.UtcNow);
                            }  


                }
            
                using(SqlCommand myCommand = new SqlCommand(query2, con))
                {
                    myCommand.Parameters.AddWithValue("@Id",user.Id);
                    myCommand.Parameters.AddWithValue("@password_",user.Password);
                    myCommand.Parameters.AddWithValue("@email",user.emailAddress);
                    myReader2 = myCommand.ExecuteReader();
                    table2.Load(myReader2);
                    myReader.Close();
                  _logger.Log(LogLevel.Warning, "Password added for " + user.Id);

                }
                
                using(SqlCommand myCommand = new SqlCommand(query3, con))
                {
                   
                    myCommand.Parameters.AddWithValue("@Id",user.Id);
                    myCommand.Parameters.AddWithValue("@accBalance",0);
                    myCommand.Parameters.AddWithValue("@Points",0);
                    myReader3= myCommand.ExecuteReader();
                    table3.Load(myReader);
                    myReader3.Close();
                    con.Close();
                    _logger.Log(LogLevel.Warning, "Balance added for " + user.FirstName + user.LastName + "(" + user.emailAddress +")");

                }
        }
             return new JsonResult("201 user created : "+ user.emailAddress);
             
    }



    [HttpPut]
        public JsonResult Put(string username, string email, string newUsername)
        {

            StringBuilder errorMessages = new StringBuilder();
        if ( email == "" || username == "" || newUsername == "")
            return new JsonResult("404 New password not accepted");
        else{

            string query = @"update dbo.Users
                                set username = @newuserName
                                where email = @email";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MainAppCon");
            SqlDataReader myReader;
            using(SqlConnection con = new SqlConnection(sqlDataSource))
            {
                con.Open();
                using(SqlCommand myCommand = new SqlCommand(query, con))
                {
                    myCommand.Parameters.AddWithValue("@newUsername",newUsername);
                    myCommand.Parameters.AddWithValue("@email", email);
                    try{
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    _logger.Log(LogLevel.Warning, "User Updated");
                    }
                    catch(SqlException ex)
                    {

                             for (int i = 0; i < ex.Errors.Count; i++)
                             {
                                 errorMessages.Append("Index #" + i + "\n" +
                                 "Message: " + ex.Errors[i].Message + "\n" +
                                 "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                                "Source: " + ex.Errors[i].Source + "\n" +
                                 "Procedure: " + ex.Errors[i].Procedure + "\n");
                                }
                                return new JsonResult (errorMessages.ToString() + DateTime.UtcNow);

                    }
                    con.Close();
                }
            }
            return new JsonResult(table);

        }
    }
 [HttpDelete]
        public JsonResult Delete(string email)
        {
            StringBuilder errorMessages = new StringBuilder();


            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("MainAppCon");
            SqlDataReader myReader;
        
            string query = @"Delete from Users where email = @email";
             
                using(SqlConnection con = new SqlConnection(sqlDataSource))
                {
                    con.Open();
                   
                 using(SqlCommand myCommand = new SqlCommand(query, con))
                    {
                    myCommand.Parameters.AddWithValue("@email",email);   
                    try{                
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    _logger.Log(LogLevel.Warning, $"{email} has been deleted at "+ DateTime.UtcNow);
                     }
                     catch(SqlException ex)
                     {
                           for (int i = 0; i < ex.Errors.Count; i++)
                             {
                                 errorMessages.Append("Index #" + i + "\n" +
                                 "Message: " + ex.Errors[i].Message + "\n" +
                                 "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                                "Source: " + ex.Errors[i].Source + "\n" +
                                 "Procedure: " + ex.Errors[i].Procedure + "\n");
                                }
                                return new JsonResult (errorMessages.ToString() + DateTime.UtcNow);
                     }
                     
                
                        con.Close();
                     }
                  
                }
                
           return new JsonResult("Lets just say if there ever was " + email + ", they are no longer with us");  
        }

    }
}
