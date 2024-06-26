using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace webapi_02
{
    public class Response
    {
        [JsonConverter(typeof(JsonStringEnumConverter))] //Serialize the Result enum to a string, e.g "success" (instead of an integer, e.g. 0)
        public Result Result { get; set; } = Result.error;
        public string Message { get; set; } = "";
        public EmployeeResponse EmployeeResponse { get; set; } = new EmployeeResponse();
        public List<Protein> Proteins { get; set; } = new List<Protein>();
        public List<ProteinCompletenessMapping> ProteinCompletenessMappings { get; set; } = new List<ProteinCompletenessMapping>();
    }
}
