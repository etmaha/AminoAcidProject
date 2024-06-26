using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace webapi_02.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ProteinCompletenessMappingController : ControllerBase
    {
        //Logger
        private readonly ILogger<ProteinCompletenessMappingController> _logger;

        public ProteinCompletenessMappingController(ILogger<ProteinCompletenessMappingController> logger)
        {
            _logger = logger;
        }

        //Connection string
        private static string serverName = @"DESKTOP-VVE2V6B\SQLEXPRESS"; //Change to the "Server Name" you see when you launch SQL Server Management Studio.
        private static string databaseName = "db2024_01"; //Change to the database where you created your Employee table.
        private static string connectionString = $"data source={serverName}; database={databaseName}; Integrated Security=true;Encrypt=true;TrustServerCertificate=true;";


        [HttpGet]
        [Route("/ProteinCompletnessMappings")]
        public Response Search(string search = "")
        {
            Response response = new Response();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    //Select employees
                    response.ProteinCompletenessMappings = ProteinCompletenessMapping.Search(sqlConnection, search);
                    response.Message = $"{response.ProteinCompletenessMappings.Count} Protein Completeness Mappings Selected.";

                    response.Result = Result.success;
                }
            }
            catch (Exception ex)
            {
                response.Message = $"An error occurred in SearchProteinCompletnessMappings: {ex.Message}";
                response.Result = Result.error;
            }

            return response;
        }

        [HttpGet]
        [Route("/DeleteProteinCompletenessMapping")]
        public Response Delete(int proteinId, int completesWithProteinID, string search = "", int pageSize = 10, int pageNumber = 1, string sort = "ProteinId")
        {
            Response response = new Response();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    //Delete employees
                    int rowsDeleted = ProteinCompletenessMapping.Delete(sqlConnection, proteinId, completesWithProteinID);
                    response.Message = $"{rowsDeleted} Protein Completeness Mapping deleted. ";

                    //Select employees after delete
                    response.ProteinCompletenessMappings = ProteinCompletenessMapping.Search(sqlConnection, search);
                    response.Message += $" {response.ProteinCompletenessMappings.Count} Protein Completeness Mappings Selected.";

                    response.Result = Result.success;
                }
            }
            catch (Exception ex)
            {
                response.Message = $"An error occurred in DeleteProteinCompletenessMapping: {ex.Message}";
                response.Result = Result.error;
            }

            return response;
        }

        [HttpGet]
        [Route("/InsertProteinCompletenessMapping")]
        public Response Insert(int proteinId, int completesWithProteinID, string search = "", int pageSize = 10, int pageNumber = 1, string sort = "ProteinId")
        {
            Response response = new Response();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    //insert employees
                    int rowsInserted = ProteinCompletenessMapping.Insert(sqlConnection, proteinId, completesWithProteinID);
                    response.Message = $"{rowsInserted} Protein Completeness Mapping Insert. ";

                    //Select employees after insert
                    response.ProteinCompletenessMappings = ProteinCompletenessMapping.Search(sqlConnection, search);
                    response.Message += $" {response.ProteinCompletenessMappings.Count} Protein Completeness Mappings Selected.";

                    response.Result = Result.success;
                }
            }
            catch (Exception ex)
            {
                response.Message = $"An error occurred in InsertProteinCompletenessMapping {ex.Message}";
                response.Result = Result.error;
            }

            return response;
        }
    }
}
