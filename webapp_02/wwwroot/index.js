//Define our application
function webapp_02() {
    //Get elements
    var button01 = document.getElementById("button-01");

    //Add event listeners
    button01.addEventListener("click", handleButton01Click);

    //Functions
    function handleButton01Click() {
        alert("Hello World!");
    }

}

//Run out application
webapp_02();