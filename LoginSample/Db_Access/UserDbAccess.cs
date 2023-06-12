using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SkillSheetManager.Models;
namespace LoginSample.Db_Access
{
    public class UserDbAccess
    {
        public static void AddUserInfo()
        {
            try
            {
                EmployeeDetailsModel employeeDetailsModel = new EmployeeDetailsModel();

                //HttpPostedFileBase file = Request.Files["givenPhoto"];
                //if (file != null)
                //{
                //    employeeDetailsModel.Photo = ConvertToBytes(file);
                //}
                string connectionString = ConfigurationManager.ConnectionStrings["userTableConStr"].ConnectionString;
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query = "INSERT INTO UserDetailsTable (Name,Designation,DateOfBirth,Sex,JoiningDate,WorkedInJapan,Qualifications,Languages,DatabaseKnown,UserPhoto) VALUES(@Name,@Designation,@DateOfBirth,@Sex,@JoiningDate,@WorkedInJapan,@Qualifications,@Languages,@DatabaseKnown,@UserPhoto)";
                    //string query = "INSERT INTO UserDetailsTable (Name,Designation) VALUES(@Name,@Designation)";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@Name", employeeDetailsModel.Name);
                    sqlCmd.Parameters.AddWithValue("@Designation", employeeDetailsModel.Designation);
                    sqlCmd.Parameters.AddWithValue("@DateOfBirth", employeeDetailsModel.DateOfBirth);
                    sqlCmd.Parameters.AddWithValue("@Sex", employeeDetailsModel.Sex);
                    sqlCmd.Parameters.AddWithValue("@JoiningDate", employeeDetailsModel.JoiningDate);
                    sqlCmd.Parameters.AddWithValue("@WorkedInJapan", employeeDetailsModel.WorkedInJapan);
                    sqlCmd.Parameters.AddWithValue("@Qualifications", employeeDetailsModel.Qualification);
                    sqlCmd.Parameters.AddWithValue("@Languages", employeeDetailsModel.Languages);
                    sqlCmd.Parameters.AddWithValue("@DatabaseKnown", employeeDetailsModel.Database);
                    sqlCmd.Parameters.AddWithValue("@UserPhoto", employeeDetailsModel.Photo);
                    sqlCmd.ExecuteNonQuery();
                }
                return;
                //return RedirectToAction("DetailsIndex");
            }
            catch (Exception ex)
            {
                return;
                //return View(ex);
            }
        }

        internal static void AddInfo(EmployeeDetailsModel employeeDetailsModel)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["userTableConStr"].ConnectionString;
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    //employeeDetailsModel.Photo = photo;
                    sqlCon.Open();
                    string query = "INSERT INTO UserDetailsTable (UserId,Name,Designation,DateOfBirth,Sex,JoiningDate,WorkedInJapan,Qualifications,Languages,DatabaseKnown,UserPhoto) VALUES(@UserId,@Name,@Designation,@DateOfBirth,@Sex,@JoiningDate,@WorkedInJapan,@Qualifications,@Languages,@DatabaseKnown,@UserPhoto)";
                    //string query = "INSERT INTO UserDetailsTable (Name,Designation,UserPhoto) VALUES(@Name,@Designation/*,@UserPhoto*/)";
                    //string query = "INSERT INTO UserDetailsTable (Name,Designation) VALUES(@Name,@Designation)";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@UserId", employeeDetailsModel.UserId);
                    sqlCmd.Parameters.AddWithValue("@Name", employeeDetailsModel.Name);
                    sqlCmd.Parameters.AddWithValue("@Designation", employeeDetailsModel.Designation);
                    sqlCmd.Parameters.AddWithValue("@DateOfBirth", employeeDetailsModel.DateOfBirth);
                    sqlCmd.Parameters.AddWithValue("@Sex", employeeDetailsModel.Sex);
                    sqlCmd.Parameters.AddWithValue("@JoiningDate", employeeDetailsModel.JoiningDate);
                    sqlCmd.Parameters.AddWithValue("@WorkedInJapan", employeeDetailsModel.WorkedInJapan);
                    sqlCmd.Parameters.AddWithValue("@Qualifications", employeeDetailsModel.Qualification);
                    sqlCmd.Parameters.AddWithValue("@Languages", employeeDetailsModel.Languages);
                    sqlCmd.Parameters.AddWithValue("@DatabaseKnown", employeeDetailsModel.Database);
                    sqlCmd.Parameters.AddWithValue("@UserPhoto", employeeDetailsModel.Photo);

                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal static EmployeeDetailsModel GetUserDetails(int id)
        {
            EmployeeDetailsModel employeeDetailsModel = new EmployeeDetailsModel();
            DataTable dataTableDetails = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["userTableConStr"].ConnectionString;
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT * FROM UserDetailsTable WHERE UserId = @UserId";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlCon);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@UserId", id);
                sqlDataAdapter.Fill(dataTableDetails);
            }

            if (dataTableDetails.Rows.Count == 1)
            {
                //employeeDetailsModel.UserId = Convert.ToInt32(dataTableDetails.Rows[0][0].ToString());
                employeeDetailsModel.Name = dataTableDetails.Rows[0][1].ToString();
                employeeDetailsModel.Designation = dataTableDetails.Rows[0][2].ToString();
                employeeDetailsModel.DateOfBirth = DateTime.Parse(dataTableDetails.Rows[0][3].ToString());
                employeeDetailsModel.Sex = dataTableDetails.Rows[0][4].ToString();
                employeeDetailsModel.JoiningDate = DateTime.Parse(dataTableDetails.Rows[0][5].ToString());
                //employeeDetailsModel.JoiningDate = (DateTime)dataTableDetails.Rows[0][5];
                employeeDetailsModel.WorkedInJapan = dataTableDetails.Rows[0][6].ToString();
                employeeDetailsModel.Qualification = (dataTableDetails.Rows[0][7] == DBNull.Value) ? null : dataTableDetails.Rows[0][7].ToString();
                employeeDetailsModel.Languages = (dataTableDetails.Rows[0][8] == DBNull.Value) ? null : dataTableDetails.Rows[0][8].ToString();
                employeeDetailsModel.Database = (dataTableDetails.Rows[0][9] == DBNull.Value) ? null : dataTableDetails.Rows[0][9].ToString();
                employeeDetailsModel.Photo = (dataTableDetails.Rows[0][10] == DBNull.Value) ? null : (byte[])dataTableDetails.Rows[0][10];
                return employeeDetailsModel;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Converts the image into binary.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }
    }
}