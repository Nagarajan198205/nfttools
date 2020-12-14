using Microsoft.AspNetCore.Components;
using NFTIntegration.Data;
using NFTIntegration.Models.Account;
using System.Threading.Tasks;

namespace NFTIntegration.Classes
{
    public interface IAccountService
    {
        User User { get; }
        Task Initialize();
        Task<User> Login(Login model);
        Task Logout();
        Task Register(AddUser model);
    }

    public class AccountService : IAccountService
    {
        private NavigationManager _navigationManager;
        private ILocalStorageService _localStorageService;
        private string _userKey = "user";

        public User User { get; private set; }

        public AccountService(
            NavigationManager navigationManager,
            ILocalStorageService localStorageService
        )
        {
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;
        }

        public async Task Initialize()
        {
            User = await _localStorageService.GetItem<User>(_userKey);
        }

        public async Task<User> Login(Login model)
        {
            User = await Task.Run(() => new DataAdapter().Login(model.Username, model.Password));
            await _localStorageService.SetItem(_userKey, User);

            return User;
        }

        public async Task Logout()
        {
            User = null;
            await _localStorageService.RemoveItem(_userKey);
            _navigationManager.NavigateTo("account/login");
        }

        public async Task Register(AddUser model)
        {
            await Task.Run(() => new DataAdapter().AddUser(model)).ConfigureAwait(false);
        }
    }
}