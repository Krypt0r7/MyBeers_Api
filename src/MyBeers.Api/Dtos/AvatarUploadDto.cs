using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace MyBeers.Api.Dtos
{
    public class AvatarUploadDto
    {
        public string File { get; set; }
    }
}
