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

        public static EmployeeResponse SearchEmployees(SqlConnection sqlConnection, string search, int pageSize, int pageNumber, string sort)
        {
            EmployeeResponse employeeResponse = new EmployeeResponse();

            // Set the SQL statement
            string sqlStatement = GetSearchQuery();

            // Create a SqlCommand
            using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, sqlConnection))
            {
                //Set parameters
                sqlCommand.Parameters.AddWithValue("@Search", "%" + search + "%");
                sqlCommand.Parameters.AddWithValue("@PageSize", pageSize);
                sqlCommand.Parameters.AddWithValue("@PageNumber", pageNumber);
                sqlCommand.Parameters.AddWithValue("@Sort", sort);

                // Create a SqlDataReader and execute the SQL command
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    // Check if the reader has rows
                    if (sqlDataReader.HasRows)
                    {
                        int row = 1;
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

                            if (row == 1)
                            {
                                employeeResponse.StartRow = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("StartRow"));
                                employeeResponse.EndRow = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("EndRow"));
                                employeeResponse.TotalRows = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("TotalRows"));
                            }

                            // Add the current employee to a list of employees
                            employeeResponse.Employees.Add(employee);

                            row++;
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                }
            }

            return employeeResponse;
        }

        private static string GetSearchQuery()
        {
            string searchQuery = "";

            searchQuery += "select EmployeeId, ";
            searchQuery += "FirstName, ";
            searchQuery += "LastName, ";
            searchQuery += "Salary, ";
            searchQuery += "(@PageSize * (@PageNumber - 1) + 1) AS StartRow, ";
            searchQuery += "least((@PageSize * @PageNumber), TotalRows) AS EndRow, ";
            searchQuery += "TotalRows ";
            searchQuery += "from ( ";
            searchQuery += "select EmployeeId, ";
            searchQuery += "FirstName, ";
            searchQuery += "LastName, ";
            searchQuery += "Salary, ";
            searchQuery += "count(*) over () AS TotalRows, ";
            searchQuery += "row_number() over ( ";
            searchQuery += "order by ";
            searchQuery += "case when @Sort = 'EmployeeId' then EmployeeId end, ";
            searchQuery += "case when @Sort = 'EmployeeIdDesc' then EmployeeId end desc, ";
            searchQuery += "case when @Sort = 'FirstName' then FirstName end, ";
            searchQuery += "case when @Sort = 'FirstNameDesc'  then FirstName end desc, ";
            searchQuery += "case when @Sort = 'LastName'  then LastName end, ";
            searchQuery += "case when @Sort = 'LastNameDesc'  then LastName end desc, ";
            searchQuery += "case when @Sort = 'Salary'  then Salary end, ";
            searchQuery += "case when @Sort = 'SalaryDesc'  then Salary end desc ";
            searchQuery += ") AS rn ";
            searchQuery += "from Employee ";
            searchQuery += "where 1 = 1 ";
            searchQuery += "and ( ";
            searchQuery += "EmployeeID like @Search ";
            searchQuery += "OR FirstName like @Search ";
            searchQuery += "OR LastName like @Search ";
            searchQuery += "OR Salary like @Search ";
            searchQuery += ") ";
            searchQuery += ") x ";
            searchQuery += "where rn between (@PageSize * (@PageNumber - 1) + 1) and (@PageSize * @PageNumber) ";
            searchQuery += "order by rn ";

            return searchQuery;
        }

        public static int InsertEmployee(SqlConnection sqlConnection, string firstName, string lastName, decimal salary)
        {
            int rowsUpdated = 0;

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

            return rowsUpdated;
        }

        public static int UpdateEmployee(SqlConnection sqlConnection, int employeeId, string firstName, string lastName, decimal salary)
        {
            int rowsUpdated = 0;

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

            return rowsUpdated;
        }

        public static int DeleteEmployee(SqlConnection sqlConnection, int employeeId)
        {
            int rowsDeleted = 0;

            // Set the SQL statement
            string sqlStatement = "delete from Employee where EmployeeId = @EmployeeId";

            using (SqlCommand sqlCommand = new SqlCommand(sqlStatement, sqlConnection))
            {
                // Add parameters
                sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);

                // Execute the SQL command (and capture number of rows deleted)
                rowsDeleted = sqlCommand.ExecuteNonQuery();
            }

            return rowsDeleted;
        }

    }
}