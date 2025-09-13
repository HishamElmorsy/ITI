import Employee from "./lab2 p4.js";

let empsArr =[]

function AddEmp(_fName,_lName,_age,_salary){
    let emp= new Employee(_fName,_lName,_age,_salary)
empsArr.push(emp)
}

function findEmp(empName){return empsArr.find((emp) => emp.FirstName === empName)}

function increaseSalary(empName, amount){
    let emp1 = findEmp(empName);
    emp1.Salary += amount;
}

AddEmp("Samy", "Ramy", 22,22222)
AddEmp("Loaay", "Ramy", 33,23322)
AddEmp("Hady", "Gabi", 45,44422)

findEmp("Samy");

increaseSalary("Samy", 44444)
console.log(empsArr)

export {AddEmp,findEmp,increaseSalary, empsArr}


