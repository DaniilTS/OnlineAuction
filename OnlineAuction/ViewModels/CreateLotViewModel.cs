using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineAuction.Models;

namespace OnlineAuction.ViewModels
{
    public class CreateLotViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string WiningUserId { get; set; }

        [Required]
        public int StartCurrency { get; set; }
        public int CategoryId { get; set; }

        [Required]
        [DataType(DataType.DateTime)] 
        public DateTime PublicationDate { get; set; }
        
        [Required]
        [DataType(DataType.DateTime)] 
        public DateTime FinishDate { get; set; }

        public string UserName { get; set; }
    }
}