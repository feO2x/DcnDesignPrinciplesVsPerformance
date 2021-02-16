using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
    public class LinqVsLoopsBenchmarks
    {
        public LinqVsLoopsBenchmarks()
        {
            var random = new Random(42);
            const int numberOfTeams = 20;
            var teams = new List<CompanyTeam>(numberOfTeams);
            var employeeIdCounter = 1;
            for (var i = 0; i < numberOfTeams; i++)
            {
                var employees = CreateEmployees(random, ref employeeIdCounter);
                var team = new CompanyTeam(i + 1, employees);
                teams.Add(team);
            }

            Teams = teams;

            static List<Employee> CreateEmployees(Random random, ref int employeeIdCounter)
            {
                var numberOfEmployees = random.Next(40, 80);
                var employees = new List<Employee>(numberOfEmployees);
                for (var i = 0; i < numberOfEmployees; i++)
                {
                    var initialWage = random.Next(1000, 3001);
                    var employee = new Employee(employeeIdCounter++, initialWage);
                    employees.Add(employee);
                }

                return employees;
            }
        }

        private List<CompanyTeam> Teams { get; }

        private const int Bonus = 300;

        [Benchmark(Baseline = true)]
        public void UpdateEmployeesViaLoop() => UpdateEmployeesViaLoop(Bonus);

        private void UpdateEmployeesViaLoop(int wageBonus)
        {
            for (var i = 0; i < Teams.Count; i++)
            {
                var team = Teams[i];
                for (var j = 0; j < team.Employees.Count; j++)
                {
                    var employee = team.Employees[j];
                    employee.CurrentWage += wageBonus;
                }
            }
        }

        [Benchmark]
        public void UpdateEmployeesViaLinq() => UpdateEmployeesViaLinq(Bonus);

        private void UpdateEmployeesViaLinq(int wageBonus)
        {
            foreach (var companyTeam in Teams)
            {
                companyTeam.Employees.ForEach(employee => employee.CurrentWage += wageBonus);
            }
        }


        private class CompanyTeam
        {
            public CompanyTeam(int id, List<Employee> employees)
            {
                Id = id;
                Employees = employees;
            }

            public int Id { get; }

            public List<Employee> Employees { get; }
        }

        private class Employee
        {
            public Employee(int id, int currentWage)
            {
                Id = id;
                CurrentWage = currentWage;
            }

            public int Id { get; }

            public int CurrentWage { get; set; }
        }
    }
}