//Define our application
function webapp_02() {
    //Gobal variables (use sparingly)
    var sortOrder = "EmployeeId";

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

    var buttonEmployeeInsert = document.getElementById("button-employee-insert");
    var formEmployeeInsert = document.getElementById("form-employee-insert");
    var inputEmployeeInsertFirstName = document.getElementById("input-employee-insert-first-name");
    var inputEmployeeInsertLastName = document.getElementById("input-employee-insert-last-name");
    var inputEmployeeInsertSalary = document.getElementById("input-employee-insert-salary");
    var buttonEmployeeInsertSave = document.getElementById("button-employee-insert-save");
    var buttonEmployeeInsertCancel = document.getElementById("button-employee-insert-cancel");

    //Add event listeners
    window.addEventListener('popstate', handlePopState);

    navPage01.addEventListener("click", handleButtonNavPage01Click);
    navPage02.addEventListener("click", handleButtonNavPage02Click);
    navPage03.addEventListener("click", handleButtonNavPage03Click);

    buttonEmployeesSearch.addEventListener("click", handleButtonEmployeesSearchClick);
    buttonEmployeesClear.addEventListener("click", handleButtonEmployeesClearClick);

    buttonEmployeeInsert.addEventListener("click", handleButtonEmployeeInsertClick);
    buttonEmployeeInsertSave.addEventListener("click", handleButtonEmployeeInsertSaveClick);
    buttonEmployeeInsertCancel.addEventListener("click", handleButtonEmployeeInsertCancelClick);

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
            showPage01();
            hidePage02();
            hidePage03();
        } else if (page.toLowerCase() === "customers") {  //lowercase comparison
            hidePage01();
            showPage02();
            hidePage03();
        } else if (page.toLowerCase() === "products") {  //lowercase comparison
            hidePage01();
            hidePage02();
            showPage03();
        }
    }

    function showPage01() {
        navPage01.classList.remove("link-secondary");
        navPage01.classList.remove("link-opacity-50");
        navPage01.classList.add("link-body-emphasis");
        navPage01.classList.add("link-opacity-100");
        page01.classList.remove("visually-hidden");
    }

    function hidePage01() {
        page01.classList.add("visually-hidden");
        navPage01.classList.add("link-secondary");
        navPage01.classList.add("link-opacity-50");
        navPage01.classList.remove("link-body-emphasis");
        navPage01.classList.remove("link-opacity-100");
    }

    function showPage02() {
        navPage02.classList.remove("link-secondary");
        navPage02.classList.remove("link-opacity-50");
        navPage02.classList.add("link-body-emphasis");
        navPage02.classList.add("link-opacity-100");
        page02.classList.remove("visually-hidden");
    }

    function hidePage02() {
        page02.classList.add("visually-hidden");
        navPage02.classList.add("link-secondary");
        navPage02.classList.add("link-opacity-50");
        navPage02.classList.remove("link-body-emphasis");
        navPage02.classList.remove("link-opacity-100");
    }

    function showPage03() {
        navPage03.classList.remove("link-secondary");
        navPage03.classList.remove("link-opacity-50");
        navPage03.classList.add("link-body-emphasis");
        navPage03.classList.add("link-opacity-100");
        page03.classList.remove("visually-hidden");
    }

    function hidePage03() {
        page03.classList.add("visually-hidden");
        navPage03.classList.add("link-secondary");
        navPage03.classList.add("link-opacity-50");
        navPage03.classList.remove("link-body-emphasis");
        navPage03.classList.remove("link-opacity-100");
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
        searchEmployees();
    }

    function searchEmployees() {
        var url = "http://localhost:5284/employees";  //Port must be the port the API is running on
        url += "?sort=" + sortOrder;
        callAPI(url);
    }

    function insertEmployee() {
        var url = "http://localhost:5284/insertemployee";  //Port must be the port the API is running on
        url += "?firstname=" + inputEmployeeInsertFirstName.value;
        url += "&lastname=" + inputEmployeeInsertLastName.value;
        url += "&salary=" + inputEmployeeInsertSalary.value;
        url += "&sort=" + sortOrder;
        callAPI(url);
    }

    function callAPI(url) {
        var xhr = new XMLHttpRequest();
        xhr.onreadystatechange = doAfterAPIResultsArrive;
        xhr.open("GET", url);
        xhr.send(null);

        function doAfterAPIResultsArrive() {
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

    function handleButtonEmployeesClearClick() {
        sortOrder = "EmployeeId";
        employeeTable.innerHTML = "";
    }

    function makeEmployeeTable(employees) {

        //Create table top boilerplate
        var empString = '<table class="table">';

        empString += '<thead>';
        empString += '<tr>';
        empString += '<th scope="col">ID <button type="button" id="button-sort-employee-id" class="btn btn-outline-secondary btn-sm btn-my-sort"><i class="bi bi-arrow-down-up"></i></button></th>';
        empString += '<th scope="col">First Name <button type="button" id="button-sort-first-name" class="btn btn-outline-secondary btn-sm btn-my-sort"><i class="bi bi-arrow-down-up"></i></button></th>';
        empString += '<th scope="col">Last Name <button type="button" id="button-sort-last-name" class="btn btn-outline-secondary btn-sm btn-my-sort"><i class="bi bi-arrow-down-up"></i></button></th>';
        empString += '<th scope="col">Salary <button type="button" id="button-sort-salary" class="btn btn-outline-secondary btn-sm btn-my-sort"><i class="bi bi-arrow-down-up"></i></button></th>'
        empString += '<th scope="col"></th>';
        empString += '</tr>';
        empString += '</thead>';
        empString += '<tbody>';

        //Loop over employees array and build the table rows
        for (var i = 0; i < employees.length; i++) {
            var employee = employees[i];
            empString += '<tr>';
            empString += '<td scope="row">' + employee.employeeId + '</td>';
            empString += '<td id="employee-' + employee.employeeId + '-first-name">' + employee.firstName + '</td>';
            empString += '<td id="employee-' + employee.employeeId + '-last-name">' + employee.lastName + '</td>'
            empString += '<td id="employee-' + employee.employeeId + '-salary">' + employee.salary + '</td>';
            empString += '</tr>';
        }

        //Create table bottom boilerplate
        empString += '</tbody>';
        empString += '</table>';

        //Inject the table string
        employeeTable.innerHTML = empString;

        //Get new elements we just created
        var buttonSortEmployeeId = document.getElementById("button-sort-employee-id");
        var buttonSortFirstName = document.getElementById("button-sort-first-name");
        var buttonSortLastName = document.getElementById("button-sort-last-name");
        var buttonSortSalary = document.getElementById("button-sort-salary");

        //Add event listeners for new elements we just created
        buttonSortEmployeeId.addEventListener("click", handleButtonSortEmployeeIdClick);
        buttonSortFirstName.addEventListener("click", handleButtonSortFirstNameClick);
        buttonSortLastName.addEventListener("click", handleButtonSortLastNameClick);
        buttonSortSalary.addEventListener("click", handleButtonSortSalaryClick);
    }

    function handleButtonSortEmployeeIdClick() {
        if (sortOrder === "EmployeeId") {
            sortOrder = "EmployeeIdDesc";
        } else {
            sortOrder = "EmployeeId";
        }

        searchEmployees();
    }
    function handleButtonSortFirstNameClick() {
        if (sortOrder === "FirstName") {
            sortOrder = "FirstNameDesc";
        } else {
            sortOrder = "FirstName";
        }

        searchEmployees();
    }

    function handleButtonSortLastNameClick() {
        if (sortOrder === "LastName") {
            sortOrder = "LastNameDesc";
        } else {
            sortOrder = "LastName";
        }

        searchEmployees();
    }

    function handleButtonSortSalaryClick() {
        if (sortOrder === "Salary") {
            sortOrder = "SalaryDesc";
        } else {
            sortOrder = "Salary";
        }

        searchEmployees();
    }

    function handleButtonEmployeeInsertClick() {
        formEmployeeInsert.classList.remove("visually-hidden");
    }

    function handleButtonEmployeeInsertSaveClick(event) {
        event.preventDefault();
        insertEmployee();
        hideFormEmployeeInsert();
    }

    function handleButtonEmployeeInsertCancelClick(event) {
        event.preventDefault();
        hideFormEmployeeInsert();
    }

    function hideFormEmployeeInsert() {
        formEmployeeInsert.classList.add("visually-hidden");
        inputEmployeeInsertFirstName.value = "";
        inputEmployeeInsertLastName.value = "";
        inputEmployeeInsertSalary.value = "";
    }

    //Execute any functions that need to run on page load
    handleNewUrl();

}

//Run our application
webapp_02();