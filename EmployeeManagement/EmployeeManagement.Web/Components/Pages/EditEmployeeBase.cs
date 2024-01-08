using AutoMapper;
using EmployeeManagement.Models;
using EmployeeManagement.Web.Models;
using EmployeeManagement.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using System.IO;
using System.Net;

namespace EmployeeManagement.Web.Components.Pages
{
    public class EditEmployeeBase : ComponentBase
    {
        [CascadingParameter]
        public Task<AuthenticationState> authenticationStateTask { get; set; }

        [Inject]
        public IEmployeeService EmployeeService { get; set; }

        private Employee Employee { get; set; } = new Employee();
        public EditEmployeeModel EditEmployeeModel { get; set; } = new EditEmployeeModel();

        [Inject]
        public IDepartmentService DepartmentService { get; set; }

        public List<Department> Departments { get; set; } = new List<Department>();

        [Parameter]
        public string Id { get; set; }

        [Inject]
        public IMapper Mapper { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public string PageHeaderText { get; set; }  

        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await authenticationStateTask;

            if(!authenticationState.User.Identity.IsAuthenticated)
            {
                string returnUrl = WebUtility.UrlEncode($"/editemployee/{Id}");
                NavigationManager.NavigateTo($"/Identity/Account/Login?returnUrl={returnUrl}");
            }
            
            int.TryParse(Id, out int employeeId);

            if(employeeId != 0)
            {
                PageHeaderText = "Edit Employee";
                Employee = await EmployeeService.GetEmployee(int.Parse(Id));
            }
            else
            {
                PageHeaderText = "Create Employee";
                Employee = new Employee
                {
                    DepartmentId = 1,
                    DateOfBrith = DateTime.Now,
                    PhotoPath = "images/nophoto.png"
                };
            }
            

            Departments = (await DepartmentService.GetDepartments()).ToList();

            Mapper.Map(Employee, EditEmployeeModel);   
        }

        protected bool success;

        protected async void HandleValidSubmit()
        {
            Mapper.Map(EditEmployeeModel, Employee);

            // Set the PhotoPath property in the Employee object
            Employee.PhotoPath = EditEmployeeModel.PhotoPath;

            Employee result = null;
            if(Employee.EmployeeId != 0)
            {
                result = await EmployeeService.UpdatedEmployee(Employee);
            }
            else 
            {
                result = await EmployeeService.CreateEmployee(Employee);
            }
            
            if (result != null)
            {
                NavigationManager.NavigateTo("/");
            }
            /*success = true;*/            
        }


        public ComponentLibrary.Confirmbase DeleteConfirmation { get; set; }

        protected void Delete_Click()
        {
            DeleteConfirmation.Show();
        }

        protected async Task ConfirmDelete_Click(bool deleteConfirm)
        {
            if (deleteConfirm)
            {
                await EmployeeService.DeleteEmployee(Employee.EmployeeId);
                NavigationManager.NavigateTo("/");
            }
        }

        //protected async Task Delete_Click()
        //{
        //    await EmployeeService.DeleteEmployee(Employee.EmployeeId);
        //    NavigationManager.NavigateTo("/");
        //}

        protected async Task HandleFileChange(InputFileChangeEventArgs e)
        {
            var file = e.File;

            if (file != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.OpenReadStream().CopyToAsync(memoryStream);
                    memoryStream.Position = 0;

                    // Generate a unique filename based on the user's first name
                    var fileName = $"{EditEmployeeModel.FirstName.ToLower()}.png";

                    // Save the file to the wwwroot/images folder
                    var imagePath = Path.Combine("wwwroot", "images", fileName);

                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        await memoryStream.CopyToAsync(fileStream);
                    }

                    // Set the PhotoPath in the model to the new file path
                    EditEmployeeModel.PhotoPath = $"/images/{fileName}";
                }
            }
            else
            {
                // If no file is selected, set the default photo path
                EditEmployeeModel.PhotoPath = "images/nophoto.png";
            }
        }

    }
}
