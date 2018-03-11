using System.Net;

namespace HolidayMailer
{
    /// <summary>
    /// A static instance of the user credentials.
    /// </summary>
    static class Credential
    {
        public static NetworkCredential NetworkCredential { get; set; }
        public static string User { get; set; }

        public static bool CredReady()
        {
            return !string.IsNullOrEmpty(User);
        }
    }
}
