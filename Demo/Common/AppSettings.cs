using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomBooking.WebApi
{
    public static class AppSettings
    {
        static AppSettings()
        {
            IConfigurationRoot objConfig = new ConfigurationBuilder()
                                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                    .AddJsonFile("appsettings.json")
                                    .Build();

            DBType = objConfig.GetValue<string>("DataBase:ConnectionType");
            RecordLimit = Convert.ToInt32(objConfig.GetValue<string>("DataBase:RecordLimit"));

            TokenValidIssuer = objConfig.GetValue<string>("JwtAuth:IssuedBy");
            TokenValidAudience = objConfig.GetValue<string>("JwtAuth:Audience");
            TokenIssuerSigningKey = objConfig.GetValue<string>("JwtAuth:Secret");
            ExpiryTime = objConfig.GetValue<int>("JwtAuth:ExpiryTime");
            ExpiryDays = objConfig.GetValue<int>("JwtAuth:ExpiryDays");

            EmailDefaultPassword = objConfig.GetValue<string>("User:EmailDefaultPassword");
            UserDefaultPassword = objConfig.GetValue<string>("User:UserDefaultPassword");
            PasswordExpiryDays = objConfig.GetValue<int>("User:PasswordExpiryDays");
            PasswordExpiryTime = objConfig.GetValue<int>("User:PasswordExpiryTime");
            PasswordResetLink = objConfig.GetValue<string>("User:PasswordResetLink");
            PastPasswordVerify = objConfig.GetValue<int>("User:PastPasswordVerify");
            SignIn = objConfig.GetValue<string>("User:SignIn");

            DocumentPath = objConfig.GetValue<string>("User:DocumentPath");
            DocumentPathBackup = objConfig.GetValue<string>("User:DocumentPathBackup");

            WebsiteLink = objConfig.GetValue<string>("Website:Link");

            Email_Host = objConfig.GetValue<string>("Email_Config:Email_Host");
            Email_Port = objConfig.GetValue<int>("Email_Config:Email_Port");
            Email_UserName = objConfig.GetValue<string>("Email_Config:Email_UserName");
            Email_Password = objConfig.GetValue<string>("Email_Config:Email_Password");
            EnableSSL = objConfig.GetValue<int>("Email_Config:EnableSSL");

            //WebApi Settings
            
            ApiPort = objConfig.GetValue<string>("Webapi:ApiPort");
            ApiOrigins = new string[] { "", "" };
           ApiOrigins = objConfig.GetValue<string>("Webapi:ApiOrigins").Split(",");
            MaxRequestBodySize = objConfig.GetValue<int>("Webapi:MaxRequestBodySize");

            DateTimeFormat = objConfig.GetValue<string>("Webapi:DateTimeFormat");
            DateFormat = objConfig.GetValue<string>("Webapi:DateFormat");
            IsDevlopent = objConfig.GetValue<int>("Webapi:IsDevlopent");
        }

        public static string DBType { get; }
        public static int RecordLimit { get; }

        public static string TokenValidIssuer { get; }
        public static string TokenValidAudience { get; }
        public static string TokenIssuerSigningKey { get; }

        public static int PastPasswordVerify { get; }
        public static int PasswordExpiryDays { get; }
        public static int PasswordExpiryTime { get; }
        public static string PasswordResetLink { get; }
        public static string EmailDefaultPassword { get; }
        public static string UserDefaultPassword { get; }

        public static string DocumentPath { get; }
        public static string DocumentPathPdf { get; }
        public static string DocumentPathBackup { get; }

        public static string SignIn { get; }

        public static string WebsiteLink { get; }
        public static int ExpiryTime { get; }
        public static int ExpiryDays { get; set; }

        public static string Email_Host { get; }
        public static int Email_Port { get; }
        public static string Email_UserName { get; }
        public static string Email_Password { get; }

        public static int EnableSSL { get; }

        public static string ApiPort { get; }
        public static string[] ApiOrigins { get; }
        public static int MaxRequestBodySize { get; }

        public static string DateTimeFormat { get; }
        public static string DateFormat { get; }

        public static string TimeZone { get; internal set; } = "India Standard Time";

        public static int IsDevlopent { get; }
    }
}
