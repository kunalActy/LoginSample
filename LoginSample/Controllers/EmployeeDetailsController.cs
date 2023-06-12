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
            //string userid = "14";

            //string dateString = "2001-10-19";
            ////DateTime date = DateTime.ParseExact(dateString, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
            //DateTime date = DateTime.Parse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            //EmployeeDetailsModel employee = new EmployeeDetailsModel()
            //{
            //    Name = "Vijay Sharma",
            //    //DateOfBirth = date.Date,
            //    //JoiningDate = DateTime.Now
            //};
            //return View(employee);

            HttpCookie userc = Request.Cookies["idCookie"];
            //string id1 =TempData["UserIDToUpage"].ToString();
            int id = int.Parse(userc.Value);
            EmployeeDetailsModel employeeDetailsModel = LoginSample.Db_Access.UserDbAccess.GetUserDetails(id);

            // New user.
            if (employeeDetailsModel == null)
            {
                return View();
            }
            return View(employeeDetailsModel);
        }


        public ActionResult AddUserData(EmployeeDetailsModel employeeDetailsModel, string userPhoto)
        {
            try
            {
                // Converting photo from base 64 string to byte array.
                employeeDetailsModel.Photo = Convert.FromBase64String(userPhoto);
                employeeDetailsModel.UserId = int.Parse(TempData["UserIDToUpage"].ToString()); 
                //UserDbAccess.UserDbAccess set = new UserDbAccess.UserDbAccess();
                //       HttpPostedFileBase file = Request.Files["inputPhoto"];
                //byte[] photo = null;
                //if (file != null)
                //{
                //    photo = ConvertToBytes(file);
                //    //set.AddInfo(photo, employeeDetailsModel);
                //}



                LoginSample.Db_Access.UserDbAccess.AddInfo(employeeDetailsModel);

                //using (SqlConnection sqlCon = new SqlConnection(connectionString))
                //{
                //    sqlCon.Open();
                //    string query = "INSERT INTO UserDetailsTable (Name,Designation,DateOfBirth,Sex,JoiningDate,WorkedInJapan,Qualifications,Languages,DatabaseKnown,UserPhoto) VALUES(@Name,@Designation,@DateOfBirth,@Sex,@JoiningDate,@WorkedInJapan,@Qualifications,@Languages,@DatabaseKnown,@UserPhoto)";
                //    //string query = "INSERT INTO UserDetailsTable (Name,Designation) VALUES(@Name,@Designation)";
                //    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                //    sqlCmd.Parameters.AddWithValue("@Name", employeeDetailsModel.Name);
                //    sqlCmd.Parameters.AddWithValue("@Designation", employeeDetailsModel.Designation);
                //    sqlCmd.Parameters.AddWithValue("@DateOfBirth", employeeDetailsModel.DateOfBirth);
                //    sqlCmd.Parameters.AddWithValue("@Sex", employeeDetailsModel.Sex);
                //    sqlCmd.Parameters.AddWithValue("@JoiningDate", employeeDetailsModel.JoiningDate);
                //    sqlCmd.Parameters.AddWithValue("@WorkedInJapan", employeeDetailsModel.WorkedInJapan);
                //    sqlCmd.Parameters.AddWithValue("@Qualifications", employeeDetailsModel.Qualification);
                //    sqlCmd.Parameters.AddWithValue("@Languages", employeeDetailsModel.Languages);
                //    sqlCmd.Parameters.AddWithValue("@DatabaseKnown", employeeDetailsModel.Database);
                //    sqlCmd.Parameters.AddWithValue("@UserPhoto", employeeDetailsModel.Photo);
                //    sqlCmd.ExecuteNonQuery();
                //}
                return Json("true", JsonRequestBehavior.AllowGet);
                //return RedirectToAction("DetailsIndex");
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        //public byte[] ConvertToBytes(HttpPostedFileBase image)
        //{
        //    byte[] imageBytes = null;
        //    BinaryReader reader = new BinaryReader(image.InputStream);
        //    imageBytes = reader.ReadBytes((int)image.ContentLength);
        //    return imageBytes;
        //}
    }
}