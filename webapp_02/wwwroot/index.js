//Define our application
function webapp_02() {
    //Get elements
    var button01 = document.getElementById("button-01");
    var employeeTable = document.getElementById("employee-table");

    //Add event listeners
    button01.addEventListener("click", handleButton01Click);

    //Functions
    function handleButton01Click() {

        var url = "http://localhost:5284/employees";
        var xhr = new XMLHttpRequest();
        xhr.onreadystatechange = doAfterSearchEmployees;
        xhr.open("GET", url);
        xhr.send(null);

        function doAfterSearchEmployees() {
            var DONE = 4; // readyState 4 means the request is done.
            var OK = 200; // status 200 is a successful return.
            if (xhr.readyState === DONE) {
                if (xhr.status === OK) {

                    var response = JSON.parse(xhr.responseText);
                    makeEmployeeTable(response);

                    // if (response.result === "success") {
                    //     showSearchResultsMessage(response.employees);
                    //     showEmployees(response.employees);
                    // } else {
                    //     alert("API Error: " + response.message);
                    // }
                } else {
                    alert("Server Error: " + xhr.status + " " + xhr.statusText);
                }
            }
        }



    }

    function makeEmployeeTable(employees) {

        var empString = '<table class="table">';
        empString += '<thead><tr><th scope="col">Employee ID</th><th scope="col">First Name</th><th scope="col">Last Name</th><th scope="col">Salary</th></tr></thead>';
        empString += '<tbody>';

        for (var i = 0; i < employees.length; i++) {
            var employee = employees[i];
            empString += '<tr><td scope="row">' + employee.employeeId + '</td> <td>' + employee.firstName + '</td><td>' + employee.lastName + '</td><td>' + employee.salary + '</td></tr>';
        }

        empString += '</tbody>';
        empString += '</table>';

        employeeTable.innerHTML = empString;
    }

}

//Run out application
webapp_02();