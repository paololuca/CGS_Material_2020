using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using BusinessEntity.Entity;
using BusinessEntity.DAO;
using Report;
using System.IO;

namespace Resources
{
    static class Helper
    {
        public const String PRINCIPAL = "PRINCIPAL";
        //deprecated public const String FEMMINILE = "FEMMINILE";
        public const String TEST = "TEST";
        //public const String OPEN = "OPEN";


        public static string GetConnectionString()
        {
            if (GetDbType() == TEST)
                return ConfigurationManager.AppSettings["SqlDBTest"];
            else
                return ConfigurationManager.AppSettings["SqlDB"];
        }

        

        ///TODO per ora funziona solo testuale, poi i gironi vanno fisicamente calcolati sull'oggetto GIRONE
        ///(cioè su una lista di persone)
        ///il risutlato sarà una lista di incontri

        /// <summary>
        /// [T4]
        ///    0	1			
        ///    2	3			
        ///    1	2			
        ///    0	3			
        ///    0	2			
        ///    1	3
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static String ElaborateT4(TreeNodeCollection t)
        {
            string incontri = "";

            foreach (TreeNode n in t)
                incontri += n.Text + " \r\n";

            incontri += " \r\n \r\n";

            incontri += t[0].Text + "\t\t" + t[1].Text + " \r\n";
            incontri += t[2].Text + "\t\t" + t[3].Text + " \r\n";
            incontri += t[1].Text + "\t\t" + t[2].Text + " \r\n";
            incontri += t[0].Text + "\t\t" + t[3].Text + " \r\n";
            incontri += t[0].Text + "\t\t" + t[2].Text + " \r\n";
            incontri += t[1].Text + "\t\t" + t[3].Text + " \r\n";

            return incontri;
        }

        /// <summary>
        /// [T5]
        /// 0	1			
        /// 2	3			
        /// 4	0			
        /// 1	2			
        /// 3	4			
        /// 0	2			
        /// 4	1			
        /// 0	3			
        /// 4	2			
        /// 1	3
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static String ElaborateT5(TreeNodeCollection t)
        {
            string incontri = "";

            foreach (TreeNode n in t)
                incontri += n.Text + " \r\n";

            incontri += " \r\n \r\n";

            incontri += t[0].Text + "\t\t" + t[1].Text + " \r\n";
            incontri += t[2].Text + "\t\t" + t[3].Text + " \r\n";
            incontri += t[4].Text + "\t\t" + t[0].Text + " \r\n";
            incontri += t[1].Text + "\t\t" + t[2].Text + " \r\n";
            incontri += t[3].Text + "\t\t" + t[4].Text + " \r\n";
            incontri += t[0].Text + "\t\t" + t[2].Text + " \r\n";
            incontri += t[4].Text + "\t\t" + t[1].Text + " \r\n";
            incontri += t[0].Text + "\t\t" + t[3].Text + " \r\n";
            incontri += t[4].Text + "\t\t" + t[2].Text + " \r\n";
            incontri += t[1].Text + "\t\t" + t[3].Text + " \r\n";
            return incontri;
        }

        /// <summary>
        /// [T6]
        /// 0	1			
        /// 2	3			
        /// 4	5			
        /// 0	3			
        /// 1	2			
        /// 3	5			
        /// 2	4			
        /// 1	5			
        /// 0	2			
        /// 3	4
        /// 
        /// 2	5			
        /// 0	4			
        /// 1	3			
        /// 0	5			
        /// 1	4
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static String ElaborateT6(TreeNodeCollection t)
        {
            string incontri = "";

            foreach (TreeNode n in t)
                incontri += n.Text + " \r\n";

            incontri += "[INCONTRI] \r\n \r\n";
            incontri += " \r\n \r\n";

            incontri += t[0].Text + "\t\t" + t[1].Text + " \r\n";
            incontri += t[2].Text + "\t\t" + t[3].Text + " \r\n";
            incontri += t[4].Text + "\t\t" + t[5].Text + " \r\n";
            incontri += t[0].Text + "\t\t" + t[3].Text + " \r\n";
            incontri += t[1].Text + "\t\t" + t[2].Text + " \r\n";
            incontri += t[3].Text + "\t\t" + t[5].Text + " \r\n";
            incontri += t[2].Text + "\t\t" + t[4].Text + " \r\n";
            incontri += t[1].Text + "\t\t" + t[5].Text + " \r\n";
            incontri += t[0].Text + "\t\t" + t[2].Text + " \r\n";
            incontri += t[3].Text + "\t\t" + t[4].Text + " \r\n";
            incontri += t[2].Text + "\t\t" + t[5].Text + " \r\n";
            incontri += t[0].Text + "\t\t" + t[4].Text + " \r\n";
            incontri += t[1].Text + "\t\t" + t[3].Text + " \r\n";
            incontri += t[0].Text + "\t\t" + t[5].Text + " \r\n";
            incontri += t[1].Text + "\t\t" + t[4].Text + " \r\n";

            return incontri;
        }

        internal static List<MatchEntity> ElaborateT4(List<AtletaEntity> g)
        {
            List<MatchEntity> match = new List<MatchEntity>();

            match.Add(new MatchEntity(g[0], g[1]));
            match.Add(new MatchEntity(g[2], g[3]));
            match.Add(new MatchEntity(g[1], g[2]));
            match.Add(new MatchEntity(g[0], g[3]));
            match.Add(new MatchEntity(g[0], g[2]));
            match.Add(new MatchEntity(g[1], g[3]));

            return match;
        }

        internal static List<MatchEntity> ElaborateT5(List<AtletaEntity> g)
        {

            List<MatchEntity> match = new List<MatchEntity>();

            match.Add(new MatchEntity(g[0], g[1]));
            match.Add(new MatchEntity(g[2], g[3]));
            match.Add(new MatchEntity(g[4], g[0]));
            match.Add(new MatchEntity(g[1], g[2]));
            match.Add(new MatchEntity(g[3], g[4]));
            match.Add(new MatchEntity(g[0], g[2]));
            match.Add(new MatchEntity(g[4], g[1]));
            match.Add(new MatchEntity(g[0], g[3]));
            match.Add(new MatchEntity(g[4], g[2]));
            match.Add(new MatchEntity(g[1], g[3]));

            return match;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        internal static List<MatchEntity> ElaborateT6(List<AtletaEntity> g)
        {
            List<MatchEntity> match = new List<MatchEntity>();

            match.Add(new MatchEntity(g[0], g[1]));
            match.Add(new MatchEntity(g[2], g[3]));
            match.Add(new MatchEntity(g[4], g[5]));
            match.Add(new MatchEntity(g[0], g[3]));
            match.Add(new MatchEntity(g[1], g[2]));
            match.Add(new MatchEntity(g[3], g[5]));
            match.Add(new MatchEntity(g[2], g[4]));
            match.Add(new MatchEntity(g[1], g[5]));
            match.Add(new MatchEntity(g[0], g[2]));
            match.Add(new MatchEntity(g[3], g[4]));
            match.Add(new MatchEntity(g[2], g[5]));
            match.Add(new MatchEntity(g[0], g[4]));
            match.Add(new MatchEntity(g[1], g[3]));
            match.Add(new MatchEntity(g[0], g[5]));
            match.Add(new MatchEntity(g[1], g[4]));

            return match;
        }



        #region READ
        public static bool TestConnectionString()
        {
            SqlConnection c = null;

            try
            {

                c = new SqlConnection(GetConnectionString());

                c.Open();

                if (c.State == System.Data.ConnectionState.Open)
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                c.Close();
            }
        }

        public static string GetDbType()
        {
            DataBaseType dbType = SqlDal_MasterDB.GetSelectedDB();

            if (dbType == DataBaseType.Principal)
                return PRINCIPAL;
            else
                return TEST;
        }

        /// <summary>
        /// Return the version of the DB Stored on the table SYSTEM_INFO
        /// </summary>
        /// <returns></returns>
        public static String GetDbVersion()
        {
            SqlConnection c = null;

            try
            {

                c = new SqlConnection(GetConnectionString());

                String sqlText = "SELECT * FROM SYSTEM_INFO WHERE NAME = 'DB_VERS'";
                c.Open();

                SqlCommand command = new SqlCommand(sqlText, c);
                SqlDataReader reader = command.ExecuteReader();

                string dbVersion = "";
                while (reader.Read())
                {
                    dbVersion = reader["VALUE"].ToString();
                }

                return dbVersion;
            }
            catch (Exception e)
            {
                return "";
            }
            finally
            {
                c.Close();
            }
        }
        public static bool CheckAdminPwd(string pwd)
        {
            SqlConnection c = null;

            try
            {

                c = new SqlConnection(GetConnectionString());

                String sqlText = "SELECT * FROM SYSTEM_INFO WHERE NAME = 'AdminPwd'";
                c.Open();

                SqlCommand command = new SqlCommand(sqlText, c);
                SqlDataReader reader = command.ExecuteReader();

                string pwdOnDb = "";
                while (reader.Read())
                {
                    pwdOnDb = reader["VALUE"].ToString();
                }

                return pwdOnDb.ToUpper() == pwd.ToUpper();
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                c.Close();
            }
        }



        #endregion

        public static bool IsValidPath(string path, bool allowRelativePaths = false)
        {
            bool isValid = true;

            try
            {
                string fullPath = Path.GetFullPath(path);

                if (allowRelativePaths)
                {
                    isValid = Path.IsPathRooted(path);
                }
                else
                {
                    string root = Path.GetPathRoot(path);
                    isValid = string.IsNullOrEmpty(root.Trim(new char[] { '\\', '/' })) == false;
                }
            }
            catch (Exception ex)
            {
                isValid = false;
            }

            return isValid;
        }

    }
}
