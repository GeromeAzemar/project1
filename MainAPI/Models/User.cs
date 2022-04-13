using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.Json.Serialization;

namespace MainAPI.Models
{
    public record User
    {
        [JsonIgnore]
       public Guid Id { get; set; }
        [Required]
        public string FirstName {get; set;}
        [Required]
        public string LastName { get; set; }
       
       [Required]
        public string username { get; set; } 
        [Required]
        public  string emailAddress {get; set;}
       [Required]
        public string Password { get; set; }
        [JsonIgnore]
        public DateTimeOffset CreateDate { get; set; }


    }
}