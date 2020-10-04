﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace OnlineAuction.Models
{
    public class User:IdentityUser
    {
        public int Year { get; set; }

        public IList<Lot> Lots { get; set; }
        public IList<Comment> Comments { get; set; }
    }
}