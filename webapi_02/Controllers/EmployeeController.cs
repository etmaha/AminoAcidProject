using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace webapi_02.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        //Logger
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _logger = logger;
        }

        //Connection string
        private static string serverName = @"LAPTOP-T24FIB73\SQLEXPRESS01"; //Change to the "Server Name" you see when you launch SQL Server Management Studio.
        private static string databaseName = "db2024_01"; //Change to the database where you created your Employee table.
        private static string connectionString = $"data source={serverName}; database={databaseName}; Integrated Security=true;Encrypt=true;TrustServerCertificate=true;";


        [HttpGet]
        [Route("/Employees")]
        public List<Employee> SearchEmployees(string search = "")
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                employees = Employee.SearchEmployees(sqlConnection, search);
            }

            return employees;
        }

        [HttpGet]
        [Route("/InsertEmployee")]
        public List<Employee> InsertEmployee(string firstName, string lastName, decimal salary)
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                Employee.InsertEmployee(sqlConnection, firstName, lastName, salary);
                employees = Employee.SearchEmployees(sqlConnection, "");
            }

            return employees;
        }

        [HttpGet]
        [Route("/UpdateEmployee")]
        public List<Employee> DeleteEmployee(int employeeId, string firstName, string lastName, decimal salary)
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                Employee.UpdateEmployee(sqlConnection, employeeId, firstName, lastName, salary);
                employees = Employee.SearchEmployees(sqlConnection, "");
            }

            return employees;
        }

        [HttpGet]
        [Route("/DeleteEmployee")]
        public List<Employee> DeleteEmployee(int employeeId)
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                Employee.DeleteEmployee(sqlConnection, employeeId);
                employees = Employee.SearchEmployees(sqlConnection, "");
            }

            return employees;
        }
    }
}
