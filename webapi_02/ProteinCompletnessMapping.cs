using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace webapi_02
{
    public class ProteinCompletenessMapping
    {
        //Instance fiels (auto-implemented properties, with defaults)
        public int ProteinId { get; set; }

        public int CompletesWithProteinId { get; set; }
        public string ProteinName { get; set; } = "";
        public string CompletesWithProteinName { get; set; } = "";


        public static List<ProteinCompletenessMapping> Search(SqlConnection sqlConnection, string search)
        {
            List<ProteinCompletenessMapping> proteinCompletenessMappings = new List<ProteinCompletenessMapping>();

            // Set the SQL statement
            string sqlStatement = GetSearchQuery();

            // Create a SqlCommand
            using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, sqlConnection))
            {
                //Set parameters
                sqlCommand.Parameters.AddWithValue("@Search", "%" + search + "%");

                // Create a SqlDataReader and execute the SQL command
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    // Check if the reader has rows
                    if (sqlDataReader.HasRows)
                    {
                        // Read each row from the data reader
                        while (sqlDataReader.Read())
                        {
                            //Create an protein object
                            ProteinCompletenessMapping proteinCompletenessMapping = new ProteinCompletenessMapping();

                            // Populate the protein object from the database row
                            proteinCompletenessMapping.ProteinId = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("ProteinID"));
                            proteinCompletenessMapping.CompletesWithProteinId = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("CompletesWithProteinID"));
                            proteinCompletenessMapping.ProteinName = sqlDataReader.GetString(sqlDataReader.GetOrdinal("ProteinName"));
                            proteinCompletenessMapping.CompletesWithProteinName = sqlDataReader.GetString(sqlDataReader.GetOrdinal("CompletesWithProteinName"));
                            // Add the current protein to a list of proteins
                            proteinCompletenessMappings.Add(proteinCompletenessMapping);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                }
            }

            return proteinCompletenessMappings;
        }

        private static string GetSearchQuery()
        {
            string searchQuery = "";

            searchQuery += "SELECT pcm.ProteinID, ";
            searchQuery += "ppf.ProteinName, ";
            searchQuery += "pcm.CompletesWithProteinID, ";
            searchQuery += "ppf2.ProteinName AS CompletesWithProteinName ";
            searchQuery += "FROM ProteinCompletenessMapping pcm ";
            searchQuery += "JOIN PlantProteinFoods ppf ";
            searchQuery += "ON pcm.ProteinID = ppf.ProteinID ";
            searchQuery += "JOIN PlantProteinFoods ppf2 ";
            searchQuery += "ON pcm.CompletesWithProteinID = ppf2.ProteinID ";
            searchQuery += "WHERE ppf.ProteinID != ppf2.ProteinID ";
            searchQuery += "AND ppf.ProteinName LIKE @Search ";
            searchQuery += "ORDER BY 1, 2 ";

            return searchQuery;
        }

        public static int Delete(SqlConnection sqlConnection, int proteinId, int completesWithProteinID)
        {
            int rowsDeleted = 0;

            // Set the SQL statement
            string sqlStatement = "DELETE FROM ProteinCompletenessMapping WHERE ProteinID = @ProteinID AND CompletesWithProteinID = @CompletesWithProteinID";

            using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, sqlConnection))
            {
                // Add parameters
                sqlCommand.Parameters.AddWithValue("@ProteinID", proteinId);
                sqlCommand.Parameters.AddWithValue("@CompletesWithProteinID", completesWithProteinID);

                // Execute the SQL command (and capture number of rows deleted)
                rowsDeleted = sqlCommand.ExecuteNonQuery();
            }

            return rowsDeleted;
        }
        public static int Insert(SqlConnection sqlConnection, int proteinId, int completesWithProteinID)
        {
            int rowsinserted = 0;

            // Set the SQL statement
            string sqlStatement = "insert into ProteinCompletenessMapping(ProteinID, CompletesWithProteinID) values (@ProteinID, @CompletesWithProteinID)";

            using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, sqlConnection))
            {
                // Add parameters
                sqlCommand.Parameters.AddWithValue("@ProteinID", proteinId);
                sqlCommand.Parameters.AddWithValue("@CompletesWithProteinID", completesWithProteinID);

                // Execute the SQL command (and capture number of rows deleted)
                rowsinserted = sqlCommand.ExecuteNonQuery();
            }

            return rowsinserted;
        }

    }






}

