using Microsoft.AspNetCore.Components;
using NFTIntegration.Models;
using NFTIntegration.Models.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFTIntegration.Classes
{
    public partial class Projects : ComponentBase
    {
        [Parameter]
        public string ProjectId { get; set; }
        public IEnumerable<Project> ProjectList { get; set; }

        [Inject]
        public IAppsService AppsService { get; set; }
        [Inject]
        public ILocalStorageService LocalStorageService { get; set; }

        [Parameter]
        public string Value { get; set; }

        [Parameter]
        public EventCallback<string> ValueChanged { get; set; }

        protected async override Task OnInitializedAsync()
        {
            var user = await LocalStorageService.GetItem<User>("user");

            ProjectList = await AppsService.GetProjectList(user.UserId);
        }

        protected Task OnValueChanged(ChangeEventArgs e)
        {
            Value = e.Value.ToString();
            return ValueChanged.InvokeAsync(Value);
        }
    }
}
