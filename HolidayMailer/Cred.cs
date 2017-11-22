using System;
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
        private static String user;

        public static NetworkCredential NetCred { get => netCred; set => netCred = value; }
        public static string User { get => user; set => user = value; }


    }
}
