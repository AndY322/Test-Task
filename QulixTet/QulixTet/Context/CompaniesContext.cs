using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;


namespace QulixTet.Models
{
    public class CompaniesContext : BaseContext
    {
        public Company GetCompany(int id)
        {
            Company company;
            using (var cmd = _connection.CreateCommand())
            {
                _connection.Open();
                cmd.CommandText = cmd.CommandText = "SELECT * FROM companies " +
                    "join LegalForm as l on l.id = companies.LegalFormId " +
                    "join KindOfActivity as k on k.id = companies.KindOfActivityId " +
                    "WHERE companies.id = @id ";

                cmd.Parameters.AddWithValue("@id", id);
                using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (!reader.Read())
                    {
                        _connection.Close();
                        return null;
                    }
                    company = GetCompany(reader);
                }
            }
            return company;
        }

        public List<Company> GetCollectionCompanies()
        {
            List<Company> companies = new List<Company>();

            using (var cmd = _connection.CreateCommand())
            {
                _connection.Open();
                cmd.CommandText = "SELECT * FROM companies " +
                    "join LegalForm as l on l.id = companies.LegalFormId " +
                    "join KindOfActivity as k on k.id = companies.KindOfActivityId";
                using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        companies.Add(GetCompany(reader));
                    }
                }

            }
            return companies;
        }

        private Company GetCompany(SqlDataReader reader)
        {

            return new Company()
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                CompanySize = reader.IsDBNull(reader.GetOrdinal("CompanySize")) ? string.Empty : reader.GetString(reader.GetOrdinal("CompanySize")),
                LegalForm = reader.IsDBNull(reader.GetOrdinal("LegalFormId")) ? string.Empty : reader.GetString(reader.GetOrdinal("FormName")),
                KindOfActivity = reader.IsDBNull(reader.GetOrdinal("KindOfActivityId")) ? string.Empty : reader.GetString(reader.GetOrdinal("ActivityName"))
            };
        }

        public void Insert(Company company)
        {
            using (var cmd = _connection.CreateCommand())
            {
                _connection.Open();
                cmd.CommandText = "insert into companies(Name, CompanySize, LegalFormId, KindOfActivityId) " +
                    "values(@Name, @CompanySize, @LegalFormId, @KindOfActivityId)";
                AddParametersToCommand(cmd, company);
                cmd.ExecuteNonQuery();
                _connection.Close();
            }
        }

        public void Update(Company company)
        {
            using (var cmd = _connection.CreateCommand())
            {
                _connection.Open();
                cmd.CommandText = "update Companies " +
                    "set Name = @Name, " +
                    "CompanySize = @CompanySize, " +
                    "LegalFormId = @LegalFormId, " +
                    "KindOfActivityId = @KindOfActivityId " +
                    "where Id = @Id";
                AddParametersToCommand(cmd, company);
                cmd.Parameters.AddWithValue("@Id", company.Id);
                cmd.ExecuteNonQuery();
                _connection.Close();
            }
        }

        private void AddParametersToCommand(SqlCommand cmd, Company company)
        {
            cmd.Parameters.AddWithValue("@Name", company.Name ?? string.Empty);
            cmd.Parameters.AddWithValue("@CompanySize", company.CompanySize ?? string.Empty);
            cmd.Parameters.AddWithValue("@LegalFormId", company.LegalFormId);
            cmd.Parameters.AddWithValue("@KindOfActivityId", company.KindOfActivityId);
        }
    }
}