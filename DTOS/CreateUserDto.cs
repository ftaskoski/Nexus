﻿namespace Nexus.DTOS
{
    public class CreateUserDto
    {
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
