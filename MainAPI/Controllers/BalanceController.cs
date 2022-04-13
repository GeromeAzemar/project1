using System.Xml.Linq;
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
     public class BalanceController : ControllerBase
    {
     
        public ILogger<UserController> _logger;
        private readonly IConfiguration _configuration;


        public BalanceController(IConfiguration configuration, ILogger<UserController> logger)
        {
            _logger = logger;
            _logger.Log(LogLevel.Information, " Balance Program called");
        _configuration = configuration;
        }

    
        [HttpGet]
        
        public JsonResult Get()
        {
            ILogger<UserController> logger = _logger;
            _logger.Log(LogLevel.Information, "Get API");
            string query = @"select * from dbo.Balances";
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
                     _logger.Log(LogLevel.Warning, "Sucessful Get all Balances Call");
                }
            }
           
            return new JsonResult(table);
           
        }


        [HttpGet("{id}")]

public JsonResult GetBalance(string id)
        {
            

             ILogger<UserController> logger = _logger;

            string query = @"select * from dbo.Balances where UserId= @id";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MainAppCon");
            SqlDataReader myReader;
            using(SqlConnection con = new SqlConnection(sqlDataSource))
            {
                con.Open();
                using(SqlCommand myCommand = new SqlCommand(query, con))
                {
                    myCommand.Parameters.AddWithValue("@id",id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    id = (myReader.ToString());
                    myReader.Close();
                    con.Close();
                  _logger.Log(LogLevel.Information, "Sucess");
                    
                }
            }
            return new JsonResult(table);
        }





         

    [HttpPut]
        public JsonResult Put(string userId, decimal deposit)
        {


            List<decimal> initialBalance = new List<decimal>();
            
            StringBuilder errorMessages = new StringBuilder();

            string q = @"SELECT balance_ from balances where UserId = @userId";

            string query = @"update Balances
                            set balance_ = @initialBalance + @deposit
                            where UserId = @userId";
            DataTable table = new DataTable();
            SqlDataReader myReader;
           string sqlDataSource = _configuration.GetConnectionString("MainAppCon");

           SqlConnection con = new SqlConnection(sqlDataSource);
           con.Open();

            using(SqlCommand cmd = new SqlCommand(q, con))
            {
                cmd.Parameters.AddWithValue("@userId", userId);
                
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    if(reader.HasRows) 
                    {
                        while(reader.Read())
                        {
                            decimal balance = Convert.ToDecimal(reader.GetDecimal(0));
                            initialBalance.Add(balance);
                        }   
                        
                    }
                }

                using(SqlCommand myCommand = new SqlCommand(query, con))
                {   
                    
                    myCommand.Parameters.AddWithValue("@userId",userId);
                    myCommand.Parameters.AddWithValue("@initialBalance", initialBalance.ElementAt(0));
                    myCommand.Parameters.AddWithValue("@deposit",deposit );
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
}
