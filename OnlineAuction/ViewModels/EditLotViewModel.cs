using System;
using System.ComponentModel.DataAnnotations;
using OnlineAuction.Models;

namespace OnlineAuction.ViewModels
{
    public class EditLotViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StartCurrency { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        
        [DataType(DataType.DateTime)] 
        public DateTime PublicationDate { get; set; }
        
        [DataType(DataType.DateTime)] 
        public DateTime FinishDate { get; set; }
    }
}