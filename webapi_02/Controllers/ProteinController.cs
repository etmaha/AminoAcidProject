using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace webapi_02.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ProteinController : ControllerBase
    {
        //Logger
        private readonly ILogger<ProteinController> _logger;

        public ProteinController(ILogger<ProteinController> logger)
        {
            _logger = logger;
        }

        //Connection string
        private static string serverName = @"DESKTOP-VVE2V6B\SQLEXPRESS"; //Change to the "Server Name" you see when you launch SQL Server Management Studio.
        private static string databaseName = "db2024_01"; //Change to the database where you created your Employee table.
        private static string connectionString = $"data source={serverName}; database={databaseName}; Integrated Security=true;Encrypt=true;TrustServerCertificate=true;";


        [HttpGet]
        [Route("/Proteins")]
        public Response Search(string search = "")
        {
            Response response = new Response();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    //Select employees
                    response.Proteins = Protein.Search(sqlConnection, search);
                    response.Message = $"{response.Proteins.Count} proteins selected.";

                    response.Result = Result.success;
                }
            }
            catch (Exception ex)
            {
                response.Message = $"An error occurred in SearchProteins: {ex.Message}";
                response.Result = Result.error;
            }

            return response;
        }
    }
}

