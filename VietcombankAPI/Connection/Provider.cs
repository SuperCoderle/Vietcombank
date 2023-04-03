using System.Data.SqlClient;
using System.Data;

namespace VietcombankAPI.Connection
{
    public class Provider
    {
        private SqlConnection? conn { get; set; }

        private string? connectionString { get; set; }

        public Provider(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private void Connect()
        {
            try
            {
                if (conn == null)
                    conn = new SqlConnection(connectionString);
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
                conn.Open();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        private void Disconnect()
        {
            try
            {
                if (conn != null && conn.State != ConnectionState.Closed)
                    conn.Close();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public int ExcuteNonQuery(CommandType type, string sql, params SqlParameter[] parameters)
        {
            int result = 0;
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandText = sql;
                cmd.CommandType = type;
                if(parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                result = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                Disconnect();
            }
            return result;
        }

        public DataTable Select(CommandType type, string sql, params SqlParameter[] parameters)
        {
            DataTable result = new DataTable();
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                cmd.CommandText = sql;
                cmd.CommandType = type;
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                adapter.Fill(result);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                Disconnect();
            }
            return result;
        }
    }
}
