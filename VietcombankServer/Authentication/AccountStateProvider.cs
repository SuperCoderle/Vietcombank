using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;
using VietcombankDTO;

namespace VietcombankClient.Authentication
{
    public class AccountStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedSessionStorage _sessionStorage;
        private ClaimsPrincipal _principal = new ClaimsPrincipal(new ClaimsIdentity());

        public AccountStateProvider(ProtectedSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userSessionStorageResult = await _sessionStorage.GetAsync<AccountDTO>("UserSession");
                var userSession = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;
                if (userSession == null)
                {
                    return await Task.FromResult(new AuthenticationState(_principal));
                }
                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userSession.Username),
                    new Claim(ClaimTypes.Role, userSession.Role)
                }, "Author"));
                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch (Exception)
            {
                return await Task.FromResult(new AuthenticationState(_principal));
            }
        }

        public async Task UpdateAuthenticationState(AccountDTO accountDTO)
        {
            ClaimsPrincipal claimsPrincipal;
            try
            {
                if(accountDTO != null)
                {
                    await _sessionStorage.SetAsync("UserSession", accountDTO);
                    claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, accountDTO.Username),
                        new Claim(ClaimTypes.Role, accountDTO.Role)
                    }));
                }
                else
                {
                    await _sessionStorage.DeleteAsync("UserSession");
                    claimsPrincipal = _principal;
                }
            }
            catch (Exception)
            {
                throw;
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
    }
}
