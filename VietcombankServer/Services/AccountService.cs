using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
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

        public AccountService(IHttpClientFactory httpClient, IConfiguration configuration, AccountStateProvider authentication, NavigationManager navigation)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
            this.authentication = authentication;
            this.navigation = navigation;
        }

        public AccountService()
        {

        }

        private string? url { get; set; }
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
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url + $"Account?Username={account.Username}&Password={account.Password}"))
                {
                    using(HttpClient client = httpClient.CreateClient())
                    {
                        using (HttpResponseMessage response = await client.SendAsync(request))
                        {
                            using var responseStream = await response.Content.ReadAsStreamAsync();
                            accounts = await JsonSerializer.DeserializeAsync<IEnumerable<AccountDTO>>(responseStream);
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
                    navigation.NavigateTo($"/main/{accountDTO.Username}", true);
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
    }
}
