using System.Net.Http;
using System.Text.Json;
using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using VietcombankDTO;

namespace VietcombankClient.Services
{
    public class InformationService : ComponentBase
    {
        [Inject]
        private IHttpClientFactory httpClient { get; set; }

        [Inject]
        private IConfiguration configuration { get; set; }

        public InformationService(IHttpClientFactory httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
        }

        public InformationService()
        {

        }

        private string? url { get; set; }

        protected DateTime date = DateTime.Now;

        protected IEnumerable<UserDTO> users = Array.Empty<UserDTO>();

        protected override async Task OnInitializedAsync()
        {
            url = configuration["API_URL"];
            await LoadData();
        }

        private async Task LoadData()
        {
            try
			{            
                using (HttpRequestMessage request1 = new HttpRequestMessage(HttpMethod.Get, url + $"Account/User/"))
                {
                    using (HttpClient client1 = httpClient.CreateClient())
                    {
                        using (HttpResponseMessage response1 = await client1.SendAsync(request1))
                        {
                            using var responseStream1 = await response1.Content.ReadAsStreamAsync();
                            users = await JsonSerializer.DeserializeAsync<IEnumerable<UserDTO>>(responseStream1);
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
