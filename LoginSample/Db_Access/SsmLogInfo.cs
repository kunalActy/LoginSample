﻿using LoginSample.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LoginSample.Db_Access
{
    public class SsmLogInfo
    {
        String ConnectionString = ConfigurationManager.ConnectionStrings["DBconnection"].ConnectionString;

        // Get all accessible candidate
        public DataSet GetLogInfo()
        {
            SqlConnection sqlConnector = new SqlConnection(ConnectionString);
            sqlConnector.Open();
            SqlCommand sqlCmd = new SqlCommand("Select * from LogInfo", sqlConnector);
            SqlDataAdapter DataSeq = new SqlDataAdapter(sqlCmd);
            DataSet ds = new DataSet();
            DataSeq.Fill(ds);
            return ds;
        }

        public DataSet GetUsers(string UserType)
        {
            SqlConnection sqlConnector = new SqlConnection(ConnectionString);
            sqlConnector.Open();
            SqlCommand sqlCmd = new SqlCommand("Select * from LogInfo where UserType=@uId", sqlConnector);
            sqlCmd.Parameters.AddWithValue("uId", UserType);
            SqlDataAdapter DataSeq = new SqlDataAdapter(sqlCmd);
            DataSet ds = new DataSet();
            DataSeq.Fill(ds);
            return ds;
        }
        // aW5kaWE=
        public DataSet GetLogPass(string userName, string userPassword)
        {
            Security.PasswordSecurity secPass = new Security.PasswordSecurity();
            SqlConnection sqlConnector = new SqlConnection(ConnectionString);
            sqlConnector.Open();
            // userPassword = secPass.DecodeFrom64(userPassword);
            SqlCommand sqlCmd = new SqlCommand("Select UserName,Password from LogInfo where UserName=@uId and password=@uPass ", sqlConnector);
            sqlCmd.Parameters.AddWithValue("uId", userName);
            sqlCmd.Parameters.AddWithValue("uPass", userPassword);
            SqlDataAdapter DataSeq = new SqlDataAdapter(sqlCmd);
            DataSet ds = new DataSet();
            DataSeq.Fill(ds);
            return ds;
        }

        public DataSet GetAllUsers()
        {
            SqlConnection sqlConnector = new SqlConnection(ConnectionString);
            sqlConnector.Open();
            SqlCommand sqlCmd = new SqlCommand("Select UserName,UserEmail,password from LogInfo where UserType=@utype", sqlConnector);
            sqlCmd.Parameters.AddWithValue("utype", "user");
            SqlDataAdapter DataSeq = new SqlDataAdapter(sqlCmd);
            DataSet ds = new DataSet();
            DataSeq.Fill(ds);
            return ds;
        }

        public int newUser(string userid, string password, string userEmail, string adminName)
        {
            password = Security.PasswordSecurity.EncodePasswordToBase64(password);
            SqlConnection sqlConnector = new SqlConnection(ConnectionString);
            sqlConnector.Open();
            SqlCommand validateUser = new SqlCommand("select UserName from LogInfo where UserName=@uname", sqlConnector);
            validateUser.Parameters.AddWithValue("uname", userid);
            SqlDataAdapter validIsUser = new SqlDataAdapter(validateUser);
            DataSet ds = new DataSet();
            validIsUser.Fill(ds);
            List<LoginInfo> userData = new List<LoginInfo>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                userData.Add(new LoginInfo { SelectedUser = dr["UserName"].ToString() });
            }
            if (userData.Count.Equals(0))
            {
                SqlCommand sqlCmd = new SqlCommand("insert into LogInfo (UserName,password,UserEmail,RegisterAdmin) values (@uname,@upass,@uemail,@uAdmin)", sqlConnector);
                sqlCmd.Parameters.AddWithValue("uname", userid);
                sqlCmd.Parameters.AddWithValue("upass", password);
                sqlCmd.Parameters.AddWithValue("uemail", userEmail);
                sqlCmd.Parameters.AddWithValue("uAdmin", adminName);
                SqlDataAdapter DataSeq = new SqlDataAdapter(sqlCmd);
                ds = new DataSet();
                DataSeq.Fill(ds);
                return 1;
            }
            else
            {               
                return 0;
            }
        }

        public DataSet DeleteSelUser(string username)
        {
            SqlConnection sqlConnector = new SqlConnection(ConnectionString);
            sqlConnector.Open();
            SqlCommand sqlCmd = new SqlCommand("Delete from LogInfo where UserName=@uname", sqlConnector);
            sqlCmd.Parameters.AddWithValue("uname", username);
            SqlDataAdapter DataSeq = new SqlDataAdapter(sqlCmd);
            DataSet ds = new DataSet();
            DataSeq.Fill(ds);
            return ds;
        }

        public DataSet GetMeUser(string username)
        {
            SqlConnection sqlConnector = new SqlConnection(ConnectionString);
            sqlConnector.Open();
            SqlCommand sqlCmd = new SqlCommand("Select UserName, UserEmail, password from LogInfo where UserName=@uname ", sqlConnector);
            sqlCmd.Parameters.AddWithValue("uname", username);
            SqlDataAdapter DataSeq = new SqlDataAdapter(sqlCmd);
            DataSet ds = new DataSet();
            DataSeq.Fill(ds);
            return ds;
        }

        public void UpdateUserDetails(string selectedUser,string newUname,string newUpassword,string newUemail)
        {
            SqlConnection sqlConnector = new SqlConnection(ConnectionString);
            sqlConnector.Open();
            SqlCommand sqlCmd = new SqlCommand("update LogInfo set UserName=@nUname,password=@nUpass,UserEmail=@nUemail where UserName=@uname", sqlConnector);
            sqlCmd.Parameters.AddWithValue("uname", selectedUser);
            sqlCmd.Parameters.AddWithValue("nUname", newUname);
            sqlCmd.Parameters.AddWithValue("nUpass", newUpassword);
            sqlCmd.Parameters.AddWithValue("nUemail", newUemail);
            SqlDataAdapter DataSeq = new SqlDataAdapter(sqlCmd);
            DataSet ds = new DataSet();
            DataSeq.Fill(ds);
        }
    }
}