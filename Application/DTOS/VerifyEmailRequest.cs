﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOS
{
    public class VerifyEmailRequest
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Token { get; set; }
    }
}
