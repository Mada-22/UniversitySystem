using System.Text;
using System.Text.Json;
using University.MVC.Models;

namespace University.MVC.Services
{
    public class StudentApiService
    {
        private readonly HttpClient _httpClient;

        public StudentApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<StudentViewModel>> GetAllStudentsAsync()
        {
            var response = await _httpClient.GetAsync("api/Student");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<StudentViewModel>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? new List<StudentViewModel>();
        }

        public async Task<StudentViewModel?> GetStudentAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/Student/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<StudentViewModel>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<bool> CreateStudentAsync(StudentViewModel student)
        {
            var json = JsonSerializer.Serialize(student);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(
                "api/Student",
                content
            );

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateStudentAsync(
            int id,
            StudentViewModel student)
        {
            var json = JsonSerializer.Serialize(student);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PutAsync(
                $"api/Student/{id}",
                content
            );

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(
                $"api/Student/{id}"
            );

            return response.IsSuccessStatusCode;
        }
    }
}
