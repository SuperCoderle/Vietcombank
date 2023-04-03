using System.Text.Json;
using Microsoft.AspNetCore.Components;
using VietcombankDTO;
using VietcombankClient.Modals;
using CurrieTechnologies.Razor.SweetAlert2;

namespace VietcombankClient.Services
{
    public class UserCRUD : ComponentBase
    {
        [Inject]
        private IHttpClientFactory httpClient { get; set; }

        [Inject]
        private IConfiguration configuration { get; set; }

        [Inject]
        private SweetAlertService swal { get; set; }

        [Inject]
        private NavigationManager navigation { get; set; }

        public UserCRUD(IHttpClientFactory httpClient, IConfiguration configuration, SweetAlertService swal, NavigationManager navigation)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
            this.swal = swal;
            this.navigation = navigation;
        }

        public UserCRUD()
        {
        }

        [Parameter]
        public string ID { get; set; }

        public string Title { get; set; }

        private string? Url { get; set; }

        protected DateTime date = DateTime.Now;

        protected User user = new User();

        protected string[] Roles = new string[3] { "Customer", "Administrator", "Employee" };

        protected IEnumerable<UserDTO> users = Array.Empty<UserDTO>();

        protected override async Task OnInitializedAsync()
        {
            Url = configuration["API_URL"];
            await LoadData();
        }

        private async Task LoadData()
        {
            try
			{            
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Url + "User/"))
                {
                    using (HttpClient client = httpClient.CreateClient())
                    {
                        using (HttpResponseMessage response = await client.SendAsync(request))
                        {
                            using var responseStream = await response.Content.ReadAsStreamAsync();
                            users = await JsonSerializer.DeserializeAsync<IEnumerable<UserDTO>>(responseStream);
                        }
                    }
                }
            }
			catch (Exception)
			{
				throw;
			}
        }

        private UserDTO set()
        {
            return new UserDTO
            {
                ID = user.ID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth.ToString("MM-dd-yyyy"),
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                NumberAccount = user.NumberAccount,
                AccountBalance = user.AccountBalance,
                Role = user.Role
            };
        }

        protected async Task CreateNewUser()
        {
            try
            {
                user.ID = ID;
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Url + "User/"))
                {
                    request.Content = new StringContent(JsonSerializer.Serialize(set()), null, "application/json");
                    using (HttpClient client = httpClient.CreateClient())
                    {
                        using (HttpResponseMessage response = await client.SendAsync(request))
                        {
                            using var responseStream = await response.Content.ReadAsStreamAsync();
                            navigation.NavigateTo("/login", true);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected async Task UpdateUser()
        {
            try
            {
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, Url + "User/"))
                {
                    request.Content = new StringContent(JsonSerializer.Serialize(set()), null, "application/json");
                    using (HttpClient client = httpClient.CreateClient())
                    {
                        using (HttpResponseMessage response = await client.SendAsync(request))
                        {
                            using var responseStream = await response.Content.ReadAsStreamAsync();
                            string res = await JsonSerializer.DeserializeAsync<string>(responseStream);
                            await swal.FireAsync("Successfully!", res, SweetAlertIcon.Success);
                            await LoadData();
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected async Task DeleteUser(string ID)
        {
            try
            {
                SweetAlertResult result = await swal.FireAsync(new SweetAlertOptions
                {
                    Title = "Bạn muốn xóa người dùng này?",
                    Text = "Người dùng này sẽ bị xóa.",
                    Icon = SweetAlertIcon.Warning,
                    ShowCancelButton = true,
                    ConfirmButtonText = "Yes",
                    CancelButtonText = "No"
                });

                if(!string.IsNullOrEmpty(result.Value))
                {
                    using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, Url + $"User/ID?ID={ID}"))
                    {
                        using (HttpClient client = httpClient.CreateClient())
                        {
                            using (HttpResponseMessage response = await client.SendAsync(request))
                            {
                                using var responseStream = await response.Content.ReadAsStreamAsync();
                                string res = await JsonSerializer.DeserializeAsync<string>(responseStream);
                                await swal.FireAsync("Successfully!", res, SweetAlertIcon.Success);
                                await LoadData();
                            }
                        }
                    }
                }
                else if(result.Dismiss == DismissReason.Cancel)
                {
                    await swal.FireAsync("Oops","Không có gì thay đổi.", SweetAlertIcon.Error);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void CreateClick()
        {
            Title = "Add New User";
            user.ID = ID;
            user.FirstName = string.Empty;
            user.LastName = string.Empty;
            user.DateOfBirth = DateTime.Now;
            user.Email = string.Empty;
            user.Phone = string.Empty;
            user.Address = string.Empty;
            user.NumberAccount = string.Empty;
            user.AccountBalance = 0;
            user.Role = string.Empty;
        }

        protected void UpdateClick(UserDTO userDTO)
        {
            Title = "Update User";
            user.ID = userDTO.ID;
            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;
            user.DateOfBirth = Convert.ToDateTime(userDTO.DateOfBirth);
            user.Email = userDTO.Email;
            user.Phone = userDTO.Phone;
            user.Address = userDTO.Address;
            user.NumberAccount = userDTO.NumberAccount;
            user.AccountBalance = userDTO.AccountBalance;
            user.Role = userDTO.Role;
        }
    }
}
