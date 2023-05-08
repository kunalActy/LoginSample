using LoginSample.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Mvc;

namespace LoginSample.Controllers
{

    public class HomeController : Controller
    {
        Db_Access.SsmLogInfo dbAccesser = new Db_Access.SsmLogInfo();
        String ConnectionString = ConfigurationManager.ConnectionStrings["DBconnection"].ConnectionString;
        public ActionResult Index()
        {
            Session["IsFromMyAction"] = false;
            var modelObj = new LoginInfo();
            modelObj.UserType = new List<SelectListItem>();
            modelObj.UserType.Add(new SelectListItem { Text = "--select--", Value = "0", Disabled = true, Selected = true });
            modelObj.UserType.Add(new SelectListItem { Text = "Admin", Value = "Admin" });
            modelObj.UserType.Add(new SelectListItem { Text = "User", Value = "User" });
            ViewBag.UserType = new SelectList(modelObj.UserType);
            modelObj.UserName = new List<SelectListItem>();
            ViewBag.UserName = new SelectList(modelObj.UserName);
            return View(modelObj);
        }

        public ActionResult AdminPage()
        {
            
            if ((bool)Session["IsFromMyAction"] == true)
            {
                List<LoginInfo> userData = new List<LoginInfo>();
                userData = GetUsers();
                HttpCookie userc = Request.Cookies["user"];
                TempData["name"] = userc.Value;             
                return View(userData);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult logout()
        {
            Session["IsFromMyAction"] = false;
            return RedirectToAction("Index", "Home");
        }

        public string ValidateUser(string userId, string pass)
        {
            pass = Security.PasswordSecurity.EncodePasswordToBase64(pass);
            /*Replace this query of code with you DB code.*/
            var logResult = GetDataFromDB(userId, pass);

            if (logResult.Count != 0)
            {
                Session["IsFromMyAction"] = true;
                HttpCookie UserCookie = new HttpCookie("user", userId);
                UserCookie.Expires.AddHours(1);
                HttpContext.Response.SetCookie(UserCookie);
                return "1";
            }
            else
            {
                return "0";
            }
        }

        public JsonResult UserNameBind(string userType)
        {
            DataSet ds = dbAccesser.GetUsers(userType);
            var modelObj = new LoginInfo();
            modelObj.UserName = new List<SelectListItem>();
            List<SelectListItem> userList = new List<SelectListItem>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                modelObj.UserName.Add(new SelectListItem { Text = dr["UserName"].ToString(), Value = dr["UserName"].ToString() });
            }
            return Json(modelObj.UserName, JsonRequestBehavior.AllowGet);
        }


        private List<LoginInfo> GetDataFromDB(string userName, string userPassword)
        {
            DataSet ds = dbAccesser.GetLogPass(userName, userPassword);
            List<LoginInfo> userData = new List<LoginInfo>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                userData.Add(new LoginInfo { SelectedUser = dr["UserName"].ToString(), UserPassword = dr["Password"].ToString() });
            }
            return userData;
        }

        private List<LoginInfo> GetUsers()
        {
            HttpCookie userc = Request.Cookies["user"];
            DataSet ds = dbAccesser.GetAllUsers();
            List<LoginInfo> userData = new List<LoginInfo>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                userData.Add(new LoginInfo { SelectedUser = dr["UserName"].ToString(), UserEmail = dr["UserEmail"].ToString(),UserPassword=dr["Password"].ToString()});
            }
            return userData;
        }
        public int AddNewUser(string userid, string pass, string userEmail)
        {
            try
            {
                HttpCookie userc = Request.Cookies["user"];
                int val = dbAccesser.newUser(userid, pass, userEmail, userc.Value);
                if (val == 0)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteUser(string[] userid)
        {
            foreach (var user in userid)
            {
                dbAccesser.DeleteSelUser(user);
            }
        }

        public JsonResult GetThisUser(string username)
        {
            DataSet ds = dbAccesser.GetMeUser(username);
            Security.PasswordSecurity decodePass = new Security.PasswordSecurity();
            List<LoginInfo> userData = new List<LoginInfo>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                userData.Add(new LoginInfo { SelectedUser = dr["UserName"].ToString(), UserEmail =  dr["UserEmail"].ToString(), UserPassword = decodePass.DecodeFrom64( dr["Password"].ToString()) });
            }
            return Json(userData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update selected user
        /// </summary>
        /// <param name="SelectedUname">Checkbox selection with user name</param>
        /// <param name="newUname">New username</param>
        /// <param name="newPassword">New password</param>
        /// <param name="newEmail">New email</param>
        /// <returns></returns>
        public int UpdateSelectedUser(string SelectedUname,string newUname,string newPassword,string newEmail)
        {
            DataSet ds = dbAccesser.GetMeUser(newUname);
            newPassword = Security.PasswordSecurity.EncodePasswordToBase64(newPassword);
            Security.PasswordSecurity decodePass = new Security.PasswordSecurity();
            List<LoginInfo> userData = new List<LoginInfo>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                userData.Add(new LoginInfo { SelectedUser = dr["UserName"].ToString() });
            }
            if (SelectedUname==newUname)
            {
                dbAccesser.UpdateUserDetails(SelectedUname, newUname, newPassword, newEmail);
                return 1;
            }
            if(userData.Count.Equals(0))
            {
                dbAccesser.UpdateUserDetails(SelectedUname, newUname, newPassword, newEmail);
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}