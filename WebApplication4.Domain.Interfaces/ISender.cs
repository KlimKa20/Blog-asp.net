﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication4.Domain.Interfaces
{
    public interface ISender
    {
        Task SendMessage(string email, string subject, string message);
    }
}
