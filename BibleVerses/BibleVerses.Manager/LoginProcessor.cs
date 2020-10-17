using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibleVerses.Common;
using BibleVerses.Models;



namespace BibleVerses.Manager
{
    public static class LoginProcessor
    {
        private static int _expirationMinutes = CommonData.TokenExpirationMinutes;

        public static string CreateAuthenticationToken(LoginModel login)
        {
            LoginModel loginResult = DataSource.GetLogin(login.UserName);
            if (loginResult.IsAuthenticated) // the user was there, let's check the password
            {
                // so we reset IsAuthenticated to be true only if the passwords match. BTW for passwords, null = empty string so "no password" matches "no password"
                loginResult.IsAuthenticated = (login.Password ?? string.Empty) == (loginResult.Password ?? string.Empty);
            }
            AuthenticationTokenModel token = new AuthenticationTokenModel()
            {
                UserName = loginResult.UserName,
                Role = loginResult.Role ?? "None",
                ExpirationDateTime = DateTime.Now.AddMinutes(_expirationMinutes),
                IsAuthenticated = loginResult.IsAuthenticated
            };
            string authToken = EncryptDecrypt.EncryptModel(token);
            return authToken;
        }
        public static AuthenticationTokenModel GetAuthenticationModelFromToken(string token)
        {
            return EncryptDecrypt.DecryptModel<AuthenticationTokenModel>(token);
        }
    }
}
