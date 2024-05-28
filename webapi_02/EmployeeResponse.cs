using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace webapi_02
{
    public class EmployeeResponse
    {
        public List<Employee> Employees { get; set; } = new List<Employee>();
        public int StartRow { get; set; } = 0;
        public int EndRow { get; set; } = 0;
        public int TotalRows { get; set; } = 0;
    }
}
