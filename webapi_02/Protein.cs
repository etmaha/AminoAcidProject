using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace webapi_02
{
    public class Protein
    {
        // Instance fields (auto-implemented properties, with defaults)
        public int ProteinId { get; set; }
        public string? ProteinName { get; set; } = "";

        public static List<Protein> Search(SqlConnection sqlConnection, string search)
        {
            List<Protein> proteins = new List<Protein>();

            // Set the SQL statement
            string sqlStatement = GetSearchQuery();

            // Create a SqlCommand
            using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, sqlConnection))
            {
                // Set parameters
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
                            // Create a protein object
                            Protein protein = new Protein();

                            // Populate the protein object from the database row
                            protein.ProteinId = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("ProteinID"));
                            protein.ProteinName = sqlDataReader.GetString(sqlDataReader.GetOrdinal("ProteinName"));

                            // Add the current protein to a list of proteins
                            proteins.Add(protein);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                }
            }

            return proteins;
        }

        private static string GetSearchQuery()
        {
            string searchQuery = "";

            searchQuery += "SELECT ProteinID, ";
            searchQuery += "ProteinName ";
            searchQuery += "FROM PlantProteinFoods ";
            searchQuery += "WHERE ProteinName LIKE @Search ";
            searchQuery += "ORDER BY 1 ";

            return searchQuery;
        }
    }
}

