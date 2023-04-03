using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;
using VietcombankClient.Authentication;
using VietcombankClient.Modals;
using VietcombankDTO;

namespace VietcombankClient.Services
{
    public class AccountService : ComponentBase
    {
        [Inject]
        private IHttpClientFactory httpClient { get; set; }

        [Inject]
        private IConfiguration configuration { get; set; }

        [Inject]
        private AuthenticationStateProvider authentication { get; set; }

        [Inject]
        private NavigationManager navigation { get; set; }

        [Inject]
        private SweetAlertService swal { get; set; }

        public AccountService(IHttpClientFactory httpClient, IConfiguration configuration, AccountStateProvider authentication, NavigationManager navigation, SweetAlertService swal)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
            this.authentication = authentication;
            this.navigation = navigation;
            this.swal = swal;
        }

        public AccountService()
        {

        }

        private string? url { get; set; }
        private string IDUser { get; set; }
        protected string? RePassword { get; set; }
        protected IEnumerable<AccountDTO> accounts = Array.Empty<AccountDTO>();
        protected IEnumerable<UserDTO> users = Array.Empty<UserDTO>();
        protected Account account = new Account();

        protected override async Task OnInitializedAsync()
        {
            url = configuration["API_URL"];
        }

        protected async Task Login()
        {
            try
            {
                if (string.IsNullOrEmpty(account.Username) && string.IsNullOrEmpty(account.Password))
                    await swal.FireAsync("Warning!", "Please input your username or password.", SweetAlertIcon.Warning);
                else
                {
                    using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url + $"Account?Username={account.Username}&Password={account.Password}"))
                    {
                        using (HttpClient client = httpClient.CreateClient())
                        {
                            using (HttpResponseMessage response = await client.SendAsync(request))
                            {
                                using var responseStream = await response.Content.ReadAsStreamAsync();
                                accounts = await JsonSerializer.DeserializeAsync<IEnumerable<AccountDTO>>(responseStream);
                            }
                        }
                    }
                }
                AccountDTO? accountDTO = accounts.First();

                if(account != null)
                {
                    var authenticationStateProvider = (AccountStateProvider)authentication;
                    await authenticationStateProvider.UpdateAuthenticationState(new AccountDTO
                    {
                        Username = accountDTO.Username,
                        Role = accountDTO.Role
                    });
                    navigation.NavigateTo("/main", true);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected async Task Logout()
        {
            try
            {
                var authenticationStateProvider = (AccountStateProvider)authentication;
                await authenticationStateProvider.UpdateAuthenticationState(null);
                navigation.NavigateTo("/");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private AccountDTO GetAccountDTO()
        {
			Random rd = new Random();
			IDUser = "CUS-" + rd.Next(100, 999);
			return new AccountDTO
            {
                ID = IDUser,
                Username = account.Username,
                Password = account.Password,
                Role = "Customer"
            };
        }

        protected async Task Register()
        {
            try
            {
                if (string.IsNullOrEmpty(account.Username) && string.IsNullOrEmpty(account.Password))
                    await swal.FireAsync("Warning!", "Please input your username or password.", SweetAlertIcon.Warning);
                else if (!account.Password.Equals(RePassword))
                    await swal.FireAsync("Error!", "Both password must be the same.", SweetAlertIcon.Error);
                using(HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url + "Account"))
                {
                    request.Content = new StringContent(JsonSerializer.Serialize(GetAccountDTO()), null, "application/json");
                    using(HttpClient client = httpClient.CreateClient())
                    {
                        using(HttpResponseMessage response = await client.SendAsync(request))
                        {
                            using var responseStream = await response.Content.ReadAsStreamAsync();
                            string res = await JsonSerializer.DeserializeAsync<string>(responseStream);
                            await swal.FireAsync("Successfully!", res, SweetAlertIcon.Success);
                            navigation.NavigateTo($"/info-form/{IDUser}");
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
