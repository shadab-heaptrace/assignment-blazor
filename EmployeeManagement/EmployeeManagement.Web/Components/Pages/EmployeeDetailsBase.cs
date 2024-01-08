using EmployeeManagement.Models;
using EmployeeManagement.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using System.Net;

namespace EmployeeManagement.Web.Components.Pages
{
    public class EmployeeDetailsBase : ComponentBase
    {
        [CascadingParameter]
        public Task<AuthenticationState> authenticationStateTask { get; set; }

        public Employee Employee { get; set; } = new Employee();

        protected string Coordinates { get; set; } = null;

        protected string ButtonText { get; set; } = "Hide Footer";
        protected string Display { get; set; } = null;

        [Inject]
        public IEmployeeService EmployeeService { get; set; }

        [Parameter]
        public string Id { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await authenticationStateTask;

            if (!authenticationState.User.Identity.IsAuthenticated)
            {
                string returnUrl = WebUtility.UrlEncode($"/employeedetails/{Id}");
                NavigationManager.NavigateTo($"/Identity/Account/Login?returnUrl={returnUrl}");
            }

            Id = Id ?? "1";
            Employee = await EmployeeService.GetEmployee(int.Parse(Id));            
        }

        protected void Button_Click()
        {
            Console.WriteLine("Button Clicked");
            if(ButtonText == "Hide Footer")
            {
                ButtonText = "Show Footer";
                Display = "none";
            }
            else
            {
                Display = null;
                ButtonText = "Hide Footer";
            }
        }
    }
}
