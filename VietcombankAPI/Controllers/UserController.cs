using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using VietcombankAPI.Connection;
using VietcombankAPI.Modals;

namespace VietcombankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IConfiguration configuration { get; set; }

        public UserController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public JsonResult GetUserInformation()
        {
            try
            {
                Provider prv = new Provider(configuration);
                string sql = "SELECT I.ID, I.FirstName, I.LastName, CONVERT(NVARCHAR(10), I.DateOfBirth, 105) AS DateOfBirth, I.Email, I.Phone, I.Address, I.NumberAccount, I.AccountBalance, A.Username, A.Role FROM Account AS A JOIN Information AS I ON A.ID = I.ID";
                DataTable result = prv.Select(CommandType.Text, sql);
                return new JsonResult(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult Post(User newUser)
        {
            try
            {
                Provider prv = new Provider(configuration);
                string sql = "INSERT INTO Information VALUES(@ID, @FirstName, @LastName, @DateOfBirth, @Email, @Phone, @Address, @NumberAccount, @AccountBalance)";
                int result = prv.ExcuteNonQuery(CommandType.Text, sql,
                    new SqlParameter { ParameterName = "@ID", Value = newUser.ID },
                    new SqlParameter { ParameterName = "@FirstName", Value = newUser.FirstName },
                    new SqlParameter { ParameterName = "@LastName", Value = newUser.LastName },
                    new SqlParameter { ParameterName = "@DateOfBirth", Value = newUser.DateOfBirth },
                    new SqlParameter { ParameterName = "@Email", Value = newUser.Email },
                    new SqlParameter { ParameterName = "@Phone", Value = newUser.Phone },
                    new SqlParameter { ParameterName = "@Address", Value = newUser.Address },
                    new SqlParameter { ParameterName = "@NumberAccount", Value = newUser.NumberAccount },
                    new SqlParameter { ParameterName = "@AccountBalance", Value = newUser.AccountBalance });
                if (result == 0)
                    return new JsonResult("Lỗi khi tạo người dùng mới.");
                return new JsonResult("Người dùng được thêm thành công.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut]
        public JsonResult Put(User updateUser)
        {
            try
            {
                Provider prv = new Provider(configuration);
                string sql = "UPDATE Information SET FirstName = @FirstName, LastName = @LastName, DateOfBirth = @DateOfBirth, Email = @Email, Phone = @Phone, Address = @Address, NumberAccount = @NumberAccount, AccountBalance = @AccountBalance WHERE ID = @ID";
                int result = prv.ExcuteNonQuery(CommandType.Text, sql,
                    new SqlParameter { ParameterName = "@ID", Value = updateUser.ID },
                    new SqlParameter { ParameterName = "@FirstName", Value = updateUser.FirstName },
                    new SqlParameter { ParameterName = "@LastName", Value = updateUser.LastName },
                    new SqlParameter { ParameterName = "@DateOfBirth", Value = updateUser.DateOfBirth },
                    new SqlParameter { ParameterName = "@Email", Value = updateUser.Email },
                    new SqlParameter { ParameterName = "@Phone", Value = updateUser.Phone },
                    new SqlParameter { ParameterName = "@Address", Value = updateUser.Address },
                    new SqlParameter { ParameterName = "@NumberAccount", Value = updateUser.NumberAccount },
                    new SqlParameter { ParameterName = "@AccountBalance", Value = updateUser.AccountBalance });
                if (result == 0)
                    return new JsonResult("Lỗi khi sửa.");
                return new JsonResult("Người dùng được update thành công.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete]
        [Route("ID")]
        public JsonResult Delete(string ID)
        {
            try
            {
                Provider prv = new Provider(configuration);
                string sql = "DELETE FROM Information WHERE ID = @ID";
                int result = prv.ExcuteNonQuery(CommandType.Text, sql,
                    new SqlParameter { ParameterName = "@ID", Value = ID });
                if (result == 0)
                    return new JsonResult("Lỗi khi xóa người dùng này.");
                return new JsonResult("Xóa thành công.");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
