﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IEmailServices
    {
       Task<bool> SendEmailAsync(string to, string subject, string body);
    }
}
