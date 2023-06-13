using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SkillSheetManager.Models;

namespace LoginSample.Controllers

{
    public class EmployeeDetailsController : Controller
    {
        // GET: EmployeeDetails
        public ActionResult EmployeeDetails()
        {
            HttpCookie userc = Request.Cookies["idCookie"];
            if(userc==null)
            {
                return RedirectToAction("Index", "Home");
            }
            int id = int.Parse(userc.Value);
            EmployeeDetailsModel employeeDetailsModel = new EmployeeDetailsModel();
            bool isOldUser = LoginSample.Db_Access.UserDbAccess.GetUserDetails(id, out employeeDetailsModel);

            // New user.
            if (isOldUser == false)
            {
                ViewBag.IsOldUser = isOldUser;
                return View(employeeDetailsModel);
            }
            // Old User
            ViewBag.IsOldUser = isOldUser;
            return View(employeeDetailsModel);
        }


        public ActionResult AddUserData(EmployeeDetailsModel employeeDetailsModel, string userPhoto, string isOldUser)
        {
            try
            {
                bool isOldUserGot = bool.Parse(isOldUser);
                // Converting photo from base 64 string to byte array.
                employeeDetailsModel.Photo = Convert.FromBase64String(userPhoto);


                // Adding new user to the Database.
                if (isOldUserGot == false)
                {
                    LoginSample.Db_Access.UserDbAccess.AddInfo(employeeDetailsModel);
                }
                else
                {
                    // Updating user info.
                    LoginSample.Db_Access.UserDbAccess.EditInfo(employeeDetailsModel);

                }
                return Json("true", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }
    }
}