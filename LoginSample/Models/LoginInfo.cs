using Amazon.IdentityManagement.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoginSample.Models
{
    public class LoginInfo
    {
        /// <summary>
        /// Types of user
        /// </summary>
        public List<SelectListItem> UserType { get; set; }

        public string SelectedGroup { get; set; }

        public List<SelectListItem> UserName { get; set; }

        public string SelectedUser { get; set; }
        public string UserPassword { get; set; }


        public string UserEmail { get; set; }


        public string NewUser { get; set; }

        public string NewUserPassword { get; set; }

        public string NewUserEmail { get; set; }
    }
}