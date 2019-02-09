using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;

namespace QulixTet.Models
{
    public class EmployeesContext : BaseContext
    {
        public Employee GetEmployee(int id)
        {
            Employee employee;
            using (var cmd = _connection.CreateCommand())
            {
                _connection.Open();
                cmd.CommandText = "SELECT c.Name as compName, *, c.CompanySize FROM employees " +
                    "join Companies as c on c.Id = employees.CompanyId " +
                    "join Position as p on p.id = employees.PositionId " +
                    "WHERE employees.id = @id ";
                    
                cmd.Parameters.AddWithValue("@id", id);
                using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (!reader.Read())
                    {
                        _connection.Close();
                        return null;
                    }
                    employee = GetEmployee(reader);
                }
            }
            return employee;
        }

        public List<Employee> GetCollectionEmployees()
        {
            List<Employee> employees = new List<Employee>();

            using (var cmd = _connection.CreateCommand())
            {
                _connection.Open();
                cmd.CommandText = "SELECT c.Name as CompanyName, * FROM employees " +
                    "join Companies as c on c.Id = employees.CompanyId " +
                    "join Position as p on p.id = employees.PositionId";
                using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while(reader.Read())
                    {
                        employees.Add(GetEmployee(reader));
                    }
                }

            }
            return employees;
        }

        private Employee GetEmployee(SqlDataReader reader)
        {

            Company company = new Company()
            {
                Name = reader.GetString(0),
                CompanySize = reader.IsDBNull(reader.GetOrdinal("CompanySize")) ? string.Empty : reader.GetString(reader.GetOrdinal("CompanySize"))
            };
            int? position = null;
            string posName = string.Empty;
            if(!reader.IsDBNull(reader.GetOrdinal("PositionId")))
            {
                position = reader.GetInt32(reader.GetOrdinal("PositionId"));
                posName = reader.GetString(reader.GetOrdinal("PosName"));
            }

            return new Employee
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.GetString(reader.GetOrdinal("name")),
                SurName = reader.GetString(reader.GetOrdinal("surname")),
                MiddleName = reader.IsDBNull(reader.GetOrdinal("MiddleName")) ? string.Empty : reader.GetString(reader.GetOrdinal("MiddleName")),
                EmploymentDate = reader.IsDBNull(reader.GetOrdinal("EmploymentDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("EmploymentDate")),
                PositionId = position,
                Position = posName,
                Company = company
            };
        }

        public void Insert(Employee employee)
        {
            using (var cmd = _connection.CreateCommand())
            {
                _connection.Open();
                cmd.CommandText = "insert into employees(Surname, Name, MiddleName, employmentDate, companyId, PositionId) " +
                    "values(@Surname, @Name, @MiddleName, @employmentDate, @companyId, @PositionId)";
                AddParametersToCommand(cmd, employee);
                cmd.ExecuteNonQuery();
                _connection.Close();
            }
        }

        public void Update(Employee employee)
        {
            using (var cmd = _connection.CreateCommand())
            {
                _connection.Open();
                cmd.CommandText = "update employees " +
                    "set Name = @Name, " +
                    "MiddleName = @MiddleName, " +
                    "employmentDate = @employmentDate, " +
                    "companyId = @companyId, " +
                    "PositionId = @PositionId " +
                    "where id = @Id";
                AddParametersToCommand(cmd, employee);
                cmd.Parameters.AddWithValue("@Id", employee.Id);
                cmd.ExecuteNonQuery();
                _connection.Close();
            }
        }

        private void AddParametersToCommand(SqlCommand cmd, Employee employee)
        {
            cmd.Parameters.AddWithValue("@Surname", employee.SurName ?? string.Empty);
            cmd.Parameters.AddWithValue("@Name", employee.Name ?? string.Empty);
            cmd.Parameters.AddWithValue("@MiddleName", employee.MiddleName ?? string.Empty);
            cmd.Parameters.AddWithValue("@employmentDate", employee.EmploymentDate);
            cmd.Parameters.AddWithValue("@companyId", employee.CompanyId);
            cmd.Parameters.AddWithValue("@PositionId", employee.PositionId);
        }
    }
}