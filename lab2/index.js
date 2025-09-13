import * as emps from  "./lab2 p4.js"
import * as depts from "./departments.js" ;
import {AddEmp,empsArr} from "./employeeOps.js" ;


AddEmp("Samy", "Ramy", 22,22222)
AddEmp("Loaay", "Ramy", 33,23322)
AddEmp("Hady", "Gabi", 45,44422)
let outputDiv = document.getElementById("output");

empsArr.forEach(emp => {
    outputDiv.innerHTML += `<p>${emp.getFullName()} - Age: ${emp.Age}, Salary: ${emp.Salary}</p>`
});
