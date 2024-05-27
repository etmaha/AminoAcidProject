//Define our application
function webapp_02() {
    //Get elements
    var navPage01 = document.getElementById("nav-page-01");
    var navPage02 = document.getElementById("nav-page-02");
    var navPage03 = document.getElementById("nav-page-03");

    var page01 = document.getElementById("page-01");
    var page02 = document.getElementById("page-02");
    var page03 = document.getElementById("page-03");

    var buttonEmployeesSearch = document.getElementById("button-employees-search");
    var buttonEmployeesClear = document.getElementById("button-employees-clear");
    var employeeTable = document.getElementById("employee-table");

    //Add event listeners
    window.addEventListener('popstate', handlePopState);

    navPage01.addEventListener("click", handleButtonNavPage01Click);
    navPage02.addEventListener("click", handleButtonNavPage02Click);
    navPage03.addEventListener("click", handleButtonNavPage03Click);

    buttonEmployeesSearch.addEventListener("click", handleButtonEmployeesSearchClick);
    buttonEmployeesClear.addEventListener("click", handleButtonEmployeesClear);

    //Functions
    function handleButtonNavPage01Click(event) {
        event.preventDefault();
        window.history.pushState({}, "", "/" + "Employees");
        showPage("Employees");
    }

    function handleButtonNavPage02Click(event) {
        event.preventDefault();
        window.history.pushState({}, "", "/" + "Customers");
        showPage("Customers");
    }

    function handleButtonNavPage03Click(event) {
        event.preventDefault();
        window.history.pushState({}, "", "/" + "Products");
        showPage("Products");
    }

    function showPage(page) {
        if (page.toLowerCase() === "employees" || page === "") {  //lowercase comparison
            page01.classList.remove("visually-hidden");
            page02.classList.add("visually-hidden");
            page03.classList.add("visually-hidden");
        } else if (page.toLowerCase() === "customers") {  //lowercase comparison
            page01.classList.add("visually-hidden");
            page02.classList.remove("visually-hidden");
            page03.classList.add("visually-hidden");
        } else if (page.toLowerCase() === "products") {  //lowercase comparison
            page01.classList.add("visually-hidden");
            page02.classList.add("visually-hidden");
            page03.classList.remove("visually-hidden");
        }
    }

    function handleNewUrl() {
        var page = window.location.pathname.split('/')[1];

        if (page === "") {
            window.history.replaceState({}, "", "/" + "Employees");
        } else {
            window.history.replaceState({}, "", "/" + page);
        }

        showPage(page);
    }

    function handlePopState() {
        var page = window.location.pathname.split('/')[1];
        showPage(page);
    }

    function handleButtonEmployeesSearchClick() {

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

                    //Deserialize JSON response into a javascript object
                    var response = JSON.parse(xhr.responseText);

                    if (response.result === "success") {
                        //alert(response.message);

                        //Turn array of employees into an html table
                        makeEmployeeTable(response.employees);
                    } else {
                        alert("API Error: " + response.message);
                    }
                } else {
                    alert("Server Error: " + xhr.status + " " + xhr.statusText);
                }
            }
        }
    }

    function handleButtonEmployeesClear() {
        employeeTable.innerHTML = "";
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

    //Execute functions tht need to run on page load
    handleNewUrl();

}

//Run our application
webapp_02();