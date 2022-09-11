namespace EmpHierarchy.Entities
{
    public class Employees
    {
        private Dictionary<string, Employee> _employees = new();
        private string _csvOfEmployees;
        public Employee root;

        public Employees(string csvOfEmployees)
        {
            _csvOfEmployees = csvOfEmployees;
            ReadEmployeeData();
        }

        public void ReadEmployeeData()
        {
            Employee employee;
            
            var employees = _csvOfEmployees.Trim().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            

            foreach (var record in employees)
            {
                string[] values = record.Trim().Split(",");
                
                if (values.Length == 3)
                {
                    var employeeId = values[0];
                    var managerId = values[1];
                    var salary = values[2];

                    //Validate the data before entry
                    if (HasValidSalary(salary) && IsAnEmployee(employeeId) && HasOneCEO() &&
                        HasOneManager(employeeId) && !HasCircularReference(employeeId, managerId))
                    {
                        employee = new Employee(values[0], values[1], int.Parse(values[2]));

                        _employees.Add(employee.EmployeeId, employee);

                        if (employee.ManagerId == "")
                            root = employee;
                    }
                }
            }
        }

        public void BuildEmployeeHierarchy(Employee root)
        {
            Employee employee = root;

            List<Employee> subs = GetSubordianate(employee.EmployeeId);
            employee.Subordinates = subs;

            if (subs.Count == 0)
                return;

            foreach (var em in subs)
                BuildEmployeeHierarchy(em);
        }

        private List<Employee> GetSubordianate(string managerId)
        {
            List<Employee> subs = new List<Employee>();
            foreach (var em in _employees)
            {
                if (em.Value.ManagerId == managerId)
                    subs.Add(em.Value);
            }
            return subs;
        }
        public long GetSalaryBudget(string employeeId)
        {
            long sum = 0;
            Employee? manager;

            _employees.TryGetValue(employeeId, out manager);

            if (manager != null)
            {
                sum = manager.CalculateTotalSalary();
            }

            return sum;
        }
        public bool HasValidSalary(string salary) => int.TryParse(salary, out int value);
        public bool HasOneCEO() => _employees.Values.Where(e => e.ManagerId == "").Count() <= 1;
        public bool HasOneManager(string employeeId) => !_employees.ContainsKey(employeeId);
        public bool IsAnEmployee(string employeeId) => !string.IsNullOrWhiteSpace(employeeId);
        public bool HasCircularReference(string employeeId, string managerId) =>
           _employees.Values.Any(e => e.EmployeeId.Equals(managerId) && e.ManagerId.Equals(employeeId));
    }
}
