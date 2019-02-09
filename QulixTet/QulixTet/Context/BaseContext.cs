using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;

namespace QulixTet.Models
{
    public class BaseContext
    {
        public SqlConnection _connection { get; }

        public BaseContext()
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["Qulix"].ConnectionString;

            _connection = new SqlConnection(connectionString);
        }

        public LookupValues GetLookupValues(string lookup)
        {
            LookupValues lv = new LookupValues()
            {
                LookupName = lookup
            };
            List<Lookup> lookupValues = new List<Lookup>();

            using (var cmd = _connection.CreateCommand())
            {
                _connection.Open();
                cmd.CommandText = "SELECT * FROM " + lookup;
                using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        lookupValues.Add(new Lookup
                        {
                            Name = reader.GetString(1),
                            Id = reader.GetInt32(0)
                        });
                    }
                }

            }
            lv.values = lookupValues;

            return lv;
        }        

        public virtual string Delete(int id, string tableName)
        {
            string message;
            using (var cmd = _connection.CreateCommand())
            {
                try
                {
                    _connection.Open();
                    cmd.CommandText = "delete from " + tableName +
                        " where id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    message = "Успешно удалено";
                }
                catch(Exception)
                {
                    message = "Не удалось удалить";
                }
                _connection.Close();
            }
            return message;
        }
    }
}