using BusinessEntity.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources
{
    public static class SqlDal_Associations
    {
        public static List<AsdEntity> GetAllAsd(bool onlyList)
        {
            String commandText = "SELECT * FROM Asd ORDER BY Nome_ASD";

            List<AsdEntity> asd = new List<AsdEntity>();
            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                SqlDataReader reader = command.ExecuteReader();

                if (!onlyList)
                    asd.Add(new AsdEntity() { Id = 0, NomeAsd = "" });

                while (reader.Read())
                {
                    asd.Add(new AsdEntity()
                    {
                        Id = (int)reader["Id"],
                        NomeAsd = Convert.ToString(reader["Nome_ASD"])
                    }
                    );
                }
                if (asd.Count > 0)
                    return asd;
                else
                    return null;
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
        public static List<AsdEntity> GetAllAsdWithMembersNumber()
        {
            String commandText = "SELECT asd.Id, asd.Nome_ASD, count(*) as members " +
                                    "FROM Asd asd join Atleti a on asd.Id = a.IdASD " +
                                    "GROUP BY asd.ID, asd.Nome_ASD " +
                                    "UNION ALL " +
                                    "SELECT Id, Nome_ASD, 0 as members " +
                                    "FROM Asd where Id not in (SELECT asd.Id FROM Asd asd join Atleti a on asd.Id = a.IdASD )";

            List<AsdEntity> asd = new List<AsdEntity>();
            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    asd.Add(new AsdEntity()
                    {
                        Id = (int)reader["Id"],
                        NomeAsd = Convert.ToString(reader["Nome_ASD"]),
                        AtletiAssociativi = (int)reader["members"]
                    }
                    );
                }
                if (asd.Count > 0)
                    return asd;
                else
                    return null;
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

    }
}
