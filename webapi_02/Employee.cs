using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace webapi_02
{
    public class Employee
    {
        //Instance fiels (auto-implemented properties, with defaults)
        public int EmployeeId { get; set; } = 0;
        public string? FirstName { get; set; } = "";
        public string? LastName { get; set; } = "";
        public decimal? Salary { get; set; } = 0.0M;

        //Instance methods
        public void ShowEmployee()
        {
            Console.WriteLine($"{EmployeeId}, {FirstName}, {LastName}, {Salary}");
        }

        //Static Methods
        public static void ShowEmployees(List<Employee> employees)
        {
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("EmployeeId, FirstName, LastName, Salary");
            Console.WriteLine("---------------------------------------");

            foreach (Employee employee in employees)
            {
                employee.ShowEmployee();
            }
        }

        public static List<Employee> SearchEmployees(SqlConnection sqlConnection, string search)
        {
            List<Employee> employees = new List<Employee>();

            //Start a try/catch so we can gracefully handle any errors
            try
            {
                // Set the SQL statement
                string sqlStatement = "select EmployeeId, FirstName, LastName, Salary from Employee where (FirstName like @Search OR LastName like @Search OR Salary like @Search) order by 1";

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
                                //Create an employee object
                                Employee employee = new Employee();

                                // Populate the employee object from the database row
                                employee.EmployeeId = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("EmployeeId"));

                                int firstNameOrdinal = sqlDataReader.GetOrdinal("FirstName");
                                employee.FirstName = sqlDataReader.IsDBNull(firstNameOrdinal) ? null : sqlDataReader.GetString(firstNameOrdinal);

                                int lastNameOrdinal = sqlDataReader.GetOrdinal("LastName");
                                employee.LastName = sqlDataReader.IsDBNull(lastNameOrdinal) ? null : sqlDataReader.GetString(lastNameOrdinal);

                                int salaryOrdinal = sqlDataReader.GetOrdinal("Salary");
                                employee.Salary = sqlDataReader.IsDBNull(salaryOrdinal) ? null : sqlDataReader.GetDecimal(salaryOrdinal);

                                // Add the current employee to a list of employees
                                employees.Add(employee);
                            }
                        }
                        else
                        {
                            Console.WriteLine("No rows found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return employees;
        }

        public static int InsertEmployee(SqlConnection sqlConnection, string firstName, string lastName, decimal salary)
        {
            int rowsUpdated = 0;

            //Start a try/catch so we can gracefully handle any errors
            try
            {
                // Set the SQL statement
                string sqlStatement = "insert into Employee (FirstName, LastName, Salary) values (@FirstName, @LastName, @Salary)";

                // Create a SqlCommand
                using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, sqlConnection))
                {
                    // Add parameters
                    sqlCommand.Parameters.AddWithValue("@FirstName", firstName);
                    sqlCommand.Parameters.AddWithValue("@LastName", lastName);
                    sqlCommand.Parameters.AddWithValue("@Salary", salary);

                    // Execute the SQL command (and capture number of rows updated)
                    rowsUpdated = sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return rowsUpdated;
        }

        public static int UpdateEmployee(SqlConnection sqlConnection, int employeeId, string firstName, string lastName, decimal salary)
        {
            int rowsUpdated = 0;

            //Start a try/catch so we can gracefully handle any errors
            try
            {
                // Set the SQL statement
                string sqlStatement = "update Employee set FirstName = @FirstName, LastName = @LastName, Salary = @Salary where EmployeeId = @EmployeeId";

                // Create a SqlCommand
                using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, sqlConnection))
                {
                    // Add parameters
                    sqlCommand.Parameters.AddWithValue("@FirstName", firstName);
                    sqlCommand.Parameters.AddWithValue("@LastName", lastName);
                    sqlCommand.Parameters.AddWithValue("@Salary", salary);
                    sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);

                    // Execute the SQL command (and capture number of rows updated)
                    rowsUpdated = sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return rowsUpdated;
        }

        public static int DeleteEmployee(SqlConnection sqlConnection, int employeeId)
        {
            int rowsDeleted = 0;

            //Start a try/catch so we can gracefully handle any errors
            try
            {
                // Set the SQL statement
                string sqlStatement = "delete from Employee where EmployeeId = @EmployeeId";

                using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, sqlConnection))
                {
                    // Add parameters
                    sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);

                    // Execute the SQL command (and capture number of rows deleted)
                    rowsDeleted = sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return rowsDeleted;
        }

    }

}