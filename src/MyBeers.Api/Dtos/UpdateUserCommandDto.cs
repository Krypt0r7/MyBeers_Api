﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.Api.Dtos
{
    public class UpdateUserCommandDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
    }
}
