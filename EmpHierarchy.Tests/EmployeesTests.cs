using EmpHierarchy.Entities;

namespace EmpHierarchy.Tests
{
    public class EmployeesTests
    {
        Employees employees = new Employees("");
        [Theory]
        [InlineData("Employee1", 3800)]
        [InlineData("Employee2", 1800)]
        [InlineData("Employee3", 500)]
        [InlineData("Employee4", 500)]
        [InlineData("Employee5", 500)]
        [InlineData("Employee6", 500)]
        void SalaryBudgetShouldReturnCorrectTotalGivenValidInput(string employeeId, long expected)
        {
            //Arrange
            string employeesCSV = @"
                Employee4,Employee2,500
                Employee3,Employee1,500
                Employee1,,1000
                Employee5,Employee1,500
                Employee2,Employee1,800
                Employee6,Employee2,500
            ";

            Employees tree = new Employees(employeesCSV);
            tree.BuildEmployeeHierarchy(tree.root);

            // Act
            var result = tree.GetSalaryBudget(employeeId);

            //Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void HasValidSalaryShouldReturnTrueGivenAValidStringInteger()
        {
            var result = employees.HasValidSalary("500");

            Assert.True(result);
        }

        [Fact]
        public void HasValidSalaryShouldReturnFalseGivenInvalidStringInteger()
        {
            var result = employees.HasValidSalary("50.0");

            Assert.False(result);
        }
    }
}
