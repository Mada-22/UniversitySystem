using System.Text;
using System.Text.Json;
using University.MVC.Models;

namespace University.MVC.Services
{
    public class DepartmentAPIService
    {
        private readonly HttpClient _httpClient;

        public DepartmentAPIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<DepartmentViewModel>> GetAllDepartmentsAsync()
        {
            var response = await _httpClient.GetAsync("api/Department");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<DepartmentViewModel>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? new List<DepartmentViewModel>();
        }

        public async Task<DepartmentViewModel?> GetDepartmentAsync(int id)
        {
            var response =
                await _httpClient.GetAsync($"api/Department/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<DepartmentViewModel>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<bool> CreateDepartmentAsync(
            DepartmentViewModel department)
        {
            var json = JsonSerializer.Serialize(department);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(
                "api/Department",
                content
            );

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateDepartmentAsync(
            int id,
            DepartmentViewModel department)
        {
            var json = JsonSerializer.Serialize(department);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PutAsync(
                $"api/Department/{id}",
                content
            );

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var response =
                await _httpClient.DeleteAsync(
                    $"api/Department/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}

