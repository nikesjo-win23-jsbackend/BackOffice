using BackOffice.Business.Models;
using System.Diagnostics;
using System.Net.Http.Json;

namespace BackOffice.Business.Services;

public class CourseService(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<Course> CreateCourseAsync(object mutation)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("https://courseprovider-nikesjo.azurewebsites.net/api/graphql?code=E2an0F4ye5fmD9IGxflSEEA2A3PQsf23TSD8CNFm1n2FAzFuR2IXJg%3D%3D", mutation);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<GraphQLResponse<Course>>();
            if (result != null && result.Data != null)
            {
                return result.Data;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("GraphQL error content: " + errorContent);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
        return null!;
    }

    public class GraphQLResponse<T>
    {
        public T? Data { get; set; }
    }
}
