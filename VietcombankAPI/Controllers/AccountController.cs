using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using VietcombankAPI.Connection;
using VietcombankAPI.Modals;

namespace VietcombankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IConfiguration configuration { get; set; }

        public AccountController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get(string Username, string Password)
        {
            try
            {
                Provider prv = new Provider(configuration);
                string sql = "SELECT * FROM Account WHERE Username = @Username AND Password = @Password";
                DataTable result = prv.Select(CommandType.Text, sql,
                    new SqlParameter { ParameterName = "@Username", Value = Username},
                    new SqlParameter { ParameterName = "@Password", Value = Password});
                return new JsonResult(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult Post(Account newAccount)
        {
            try
            {
                Provider prv = new Provider(configuration);
                string sql = "INSERT INTO Account VALUES(@ID, @Username, @Password, @Role)";
                int result = prv.ExcuteNonQuery(CommandType.Text, sql,
                    new SqlParameter { ParameterName = "@ID", Value = newAccount.ID },
                    new SqlParameter { ParameterName = "@Username", Value = newAccount.Username },
                    new SqlParameter { ParameterName = "@Password", Value = newAccount.Password },
                    new SqlParameter { ParameterName = "@Role", Value = newAccount.Role });
                if (result == 0)
                    return new JsonResult("Tên tài khoản hoặc mật khẩu không phù hợp.");
                return new JsonResult("Chúc mừng bạn đăng ký thành công! Quay lại đăng nhập thôi.");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
