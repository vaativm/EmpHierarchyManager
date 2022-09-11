namespace EmpHierarchy.Entities
{
    public class Employee
    {
        public string EmployeeId { get; set; }
        public string ManagerId { get; set; }
        public int Salary { get; set; }

        public List<Employee> Subordinates = new();

        public Employee(string employeeId, string managerId, int salary)
        {
            EmployeeId = employeeId;
            ManagerId = managerId;
            Salary = salary;
        }
        public int CalculateTotalSalary()
        {
            int totalSalary = Salary;

            foreach (var employee in Subordinates)
            {
                totalSalary += employee.CalculateTotalSalary();
            }

            return totalSalary;
        }
    }
}
