﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HolidayMailer
{
    static class Cred
    {
        private static NetworkCredential netCred;
        private static string user;

        public static NetworkCredential NetCred { get => netCred; set => netCred = value; }
        public static string User { get => user; set => user = value; }
        
        public static bool CredReady()
        {
            return !string.IsNullOrEmpty(User);
        }

    }
}
