﻿using System.ComponentModel.DataAnnotations;

namespace ECommerceStore.DTO.User
{
    public class UserDTO
    {
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; } 
        public string? Role { get; set; }
    }
}
