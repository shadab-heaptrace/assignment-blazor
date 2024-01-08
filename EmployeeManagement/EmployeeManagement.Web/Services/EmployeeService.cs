using EmployeeManagement.Models;

namespace EmployeeManagement.Web.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HttpClient httpclient;

        public EmployeeService(HttpClient httpClient)
        {
            this.httpclient = httpClient;
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await httpclient.GetFromJsonAsync<Employee[]>("api/employee");
        }

        public async Task<Employee> GetEmployee(int id)
        {
            Employee employee = await httpclient.GetFromJsonAsync<Employee>($"api/employee/{id}");
            return employee;
        }

        public async Task<Employee> UpdatedEmployee(Employee updatedEmployee)
        {
            HttpResponseMessage response = await httpclient.PutAsJsonAsync<Employee>("api/employee", updatedEmployee);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the content of the response into an Employee object
                Employee updatedEmployeeResult = await response.Content.ReadFromJsonAsync<Employee>();
                return updatedEmployeeResult;
            }
            else
            {
                // Handle the error case here, for example, throw an exception or return null
                return null;
            }
        }

        public async Task<Employee> CreateEmployee(Employee newEmployee)
        {
            HttpResponseMessage response = await httpclient.PostAsJsonAsync<Employee>("api/employee", newEmployee);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the content of the response into an Employee object
                Employee updatedEmployeeResult = await response.Content.ReadFromJsonAsync<Employee>();
                return updatedEmployeeResult;
            }
            else
            {
                // Log the HTTP status code and response content for debugging
                Console.WriteLine($"HTTP Status Code: {response.StatusCode}");

                string errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error Content: {errorContent}");
                // Handle the error case here, for example, throw an exception or return null
                return null;
            }
        }

        public async Task DeleteEmployee(int id)
        {
            await httpclient.DeleteAsync($"api/employee/{id}");
        }
    }
}
