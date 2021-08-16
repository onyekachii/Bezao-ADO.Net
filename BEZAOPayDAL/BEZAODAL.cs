using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using BEZAOPayDAL.Models;

namespace BEZAOPayDAL
{    
   public class BEZAODAL
   {         
        private readonly string _connectionString;
        private SqlConnection _connection = null;
        public BEZAODAL():
           this(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BEZAOPay;Integrated Security=True")
        {

        }
       
        public BEZAODAL(string connectionString )
        {
           _connectionString = connectionString;
        }        
        private void OpenConnection()
        {
           _connection = new SqlConnection { ConnectionString = _connectionString };
           _connection.Open();
        }

        private void CloseConnection()
        {
           if (_connection?.State != ConnectionState.Closed) 
               _connection?.Close();
        }
        
        public IEnumerable<User> GetAllUsers()
        {
            OpenConnection();

            var users = new List<User>();

            var query = @"SELECT * FROM USERS";
            using (var command = new SqlCommand(query, _connection))
            {
                command.CommandType = CommandType.Text;
                
                var reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                while (reader.Read())
                {
                   users.Add(new User
                   {
                       Id = (int) reader["Id"],
                       Name = (string) reader["Name"],
                       Email =  (string) reader["Email"]
                   }); 
                }
                reader.Close();
            }
            return users;
        }
        
        public string GetUser(int id)
        {
            OpenConnection();
            string user = null;
            string query = "GetUserName";
                //$"Select name From USERS where Id = {id}";
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                SqlParameter parameterId = new SqlParameter
                {
                    ParameterName = "@Id",
                    Value = id,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(parameterId);
                SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
                while (dataReader.Read())
                {
                    user = (string)dataReader["Name"];
                }
                dataReader.Close();
            }
            return user;
        }

        public User AddUser(User user)
        {
            OpenConnection();
            using (SqlCommand command = new SqlCommand("AddNewUser", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                SqlParameter parameter = new SqlParameter
                {
                    ParameterName = "@Id",
                    Value = user.Id,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(parameter);
                parameter = new SqlParameter
                {
                    ParameterName = "@Name",
                    Value = user.Name,
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(parameter);
                parameter = new SqlParameter
                {
                    ParameterName = "@Email",
                    Value = user.Email,
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(parameter);
                command.ExecuteNonQuery();
                CloseConnection();
            }
            return user;
        }
        
        //public void UpdateUser(int id, string name, string email)
        //{
        //    OpenConnection();
        //    string sql = $"Update users Set Name = '{name}' Where Id = '{id}'";
        //    using (SqlCommand command = new SqlCommand(sql, _connection))
        //    {
        //        command.ExecuteNonQuery();
        //    }
        //    CloseConnection();
        //}
        public void DeleteUser(string name)
        {
            OpenConnection();
            string sql = $"Delete from users where Name = '{name}'";
            using (SqlCommand command = new SqlCommand(sql, _connection))
            {
                try
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                    Console.WriteLine($"{name} has been deleted from the Database");
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("Sorry! This user does not exist!", ex);
                    throw error;
                }
            }
            CloseConnection();
        }
   }
}
