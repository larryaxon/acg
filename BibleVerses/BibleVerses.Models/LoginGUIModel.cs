using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BibleVerses.Models
{
    public class LoginGUIModel
    {
        [DisplayName("User Name:")]
        public string UserName { get; set; }
        [DisplayName("Password:")]
        public string Password { get; set; }
        [DisplayName("Error Message:")]
        public string ErrorMessage { get; set; }
    }
}