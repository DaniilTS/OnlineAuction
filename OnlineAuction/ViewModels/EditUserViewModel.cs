﻿using System;

namespace OnlineAuction.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string TimeZone { get; set; }
        public string Email { get; set; }
        public int Year { get; set; }
    }
}