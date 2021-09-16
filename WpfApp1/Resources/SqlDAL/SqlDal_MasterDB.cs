using BusinessEntity.Entity;
using BusinessEntity.Type;
using Resources;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Resources
{
    public static class SqlDal_MasterDB
    {
        private static string GetMasterConnectionString()
        {
            return ConfigurationManager.AppSettings["Master"];
        }

        public static LoginUser CheckLogin(string userName)
        {
            LoginUser user = null;

            SqlConnection c = null;

            try
            {

                c = new SqlConnection(GetMasterConnectionString());

                String sqlText = "Select * from Account a join AccountType at on a.UserType = at.Id where a.UserName = '" + userName + "'";
                c.Open();

                SqlCommand command = new SqlCommand(sqlText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    user = new LoginUser
                    {
                        UserName = userName,
                        Password = reader["Password"].ToString().ToUpper(),
                        IsAdmin = (ProfileType)reader["UserType"] == ProfileType.Admin,
                        Type = (ProfileType)reader["UserType"],
                        IsEnabled = Convert.ToBoolean(reader["Enabled"])
                    };
                }

                return user;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                c.Close();
            }
        }

        internal static DataBaseType GetSelectedDB()
        {
            DataBaseType db = DataBaseType.None;
            SqlConnection c = null;

            try
            {
                c = new SqlConnection(GetMasterConnectionString());

                String sqlText = "Select Value from SETTINGS WHERE Field = 'DATABASE'";
                c.Open();

                SqlCommand command = new SqlCommand(sqlText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    db = (DataBaseType)Convert.ToInt32(reader[0]);
                }

                return db;
            }
            catch (Exception e)
            {
                return db;
            }
            finally
            {
                c.Close();
            }
        }

        internal static bool UpdateSelectedDB(DataBaseType db)
        {
            var currentDbSaved = GetSelectedDB();

            string commandText;

            if (currentDbSaved == DataBaseType.None)
                commandText = "INSERT INTO SETTINGS VALUES ('DATABASE','1','DataBase Selected'";
            else
                commandText = "UPDATE SETTINGS set VALUE = '" + (int)db + "' WHERE Field = 'DATABASE'";

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(GetMasterConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                Int32 rowAffected = command.ExecuteNonQuery();

                if (rowAffected > 0)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                c.Close();
            }
        }

        internal static List<LoginUser> GetAccountList()
        {
            List<LoginUser> accountList = new List<LoginUser>();
            SqlConnection c = null;

            try
            {

                c = new SqlConnection(GetMasterConnectionString());

                String sqlText = "Select at.Id as UserId, * from Account a join AccountType at on a.UserType = at.Id";
                c.Open();

                SqlCommand command = new SqlCommand(sqlText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    accountList.Add(new LoginUser
                    {
                        UserId = Convert.ToInt32(reader["UserId"]),
                        UserName = reader["UserName"].ToString(),
                        Password = reader["Password"].ToString().ToUpper(),
                        IsAdmin = (ProfileType)reader["UserType"] == ProfileType.Admin,
                        Type = (ProfileType)reader["UserType"],
                        IsEnabled = Convert.ToBoolean(reader["Enabled"])
                    });
                }

                return accountList;
            }
            catch (Exception e)
            {
                return new List<LoginUser>();
            }
            finally
            {
                c.Close();
            }
        }

        internal static int CheckIfAccountExist(string userName)
        {
            String commandText = "SELECT * FROM Account WHERE UserName = '" + userName + "'";

            SqlConnection c = null;
            try
            {

                c = new SqlConnection(GetMasterConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                SqlDataReader reader = command.ExecuteReader();

                Int32 accountId = 0;

                while (reader.Read())
                    accountId = Convert.ToInt32(reader["Id"]);

                if (accountId > 0)
                    return accountId;
                else
                    return -1;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                c.Close();
            }
        }

        #region Insert
        internal static int AddNewAccount(string userName, string password, ProfileType accountType)
        {
            String commandText = "INSERT INTO Account VALUES ('" + userName + "',"
                + (int)accountType
                + ",'" + password + "', 1); " +
                "SELECT SCOPE_IDENTITY();";

            SqlConnection c = null;
            try
            {

                c = new SqlConnection(GetMasterConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                SqlDataReader reader = command.ExecuteReader();

                Int32 idInserted = 0;
                reader.Read();

                idInserted = Convert.ToInt32(reader[0]);

                if (idInserted > 0)
                    return idInserted;
                else
                    return -1;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                c.Close();
            }
        }


        #endregion

        #region Delete
        internal static bool DeleteUserAccount(string userName)
        {
            String commandText = "DELETE FROM Account WHERE UserName = '" + userName + "'";

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(GetMasterConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                Int32 rowAffected = command.ExecuteNonQuery();

                if (rowAffected > 0)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                c.Close();
            }
        }
        #endregion

        #region QualificationCap

        internal static int GetQualificationCap(int numberToQuelify)
        {
            String commandText = "SELECT * FROM QualificationCap WHERE NumberOfQuelified = '" + numberToQuelify + "'";

            SqlConnection c = null;
            try
            {

                c = new SqlConnection(GetMasterConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                SqlDataReader reader = command.ExecuteReader();

                Int32 minCap = 0;

                while (reader.Read())
                    minCap = Convert.ToInt32(reader["NumberOfPartecipants"]);

                if (minCap > 0)
                    return minCap;
                else
                    return -1;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                c.Close();
            }
        }


        #endregion
    }
}