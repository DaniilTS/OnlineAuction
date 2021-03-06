﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OnlineAuction.Data;
using OnlineAuction.Models;

namespace OnlineAuction.Services.Interfaces
{
    public interface IDeleteService
    {
        
        Task DeleteLotAsync(int id);
        Task DeleteCommentAsync(int lotId, int commentId);
    }
}