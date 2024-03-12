using System;
using System.Data;
using System.Data.SqlClient;

public class UserDbContext
{
    private readonly string _connectionString;

    public UserDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public int ExecuteUserManagementProcedure(int operationId, out int loginAttemptId, out int userId, string userName, string userEmail, string userPassword, bool? isActive)
    {
        loginAttemptId = 0;
        userId = 0;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("sp_UserManagement", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Add parameters
                command.Parameters.AddWithValue("@OperationId", operationId);
                command.Parameters.Add("@LoginAttemptId", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.Parameters.Add("@UserId", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.Parameters.AddWithValue("@UserName", userName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@UserEmail", userEmail ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@UserPassword", userPassword ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@IsActive", isActive ?? (object)DBNull.Value);
                command.Parameters.Add("@IsSuccessful", SqlDbType.Bit).Direction = ParameterDirection.Output;

                // Execute the stored procedure
                command.ExecuteNonQuery();

                // Retrieve output parameter values
                if (command.Parameters["@LoginAttemptId"].Value != DBNull.Value)
                    loginAttemptId = Convert.ToInt32(command.Parameters["@LoginAttemptId"].Value);

                userId = Convert.ToInt32(command.Parameters["@UserId"].Value);
                return Convert.ToInt32(command.Parameters["@IsSuccessful"].Value);
            }
        }
    }

}
