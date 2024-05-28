using BackOffice.Business.Models;
using System.Diagnostics;
using System.Net.Http.Json;

namespace BackOffice.Business.Services;

public class CourseService(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<IEnumerable<CourseCardModel>> GetCoursesAsync()
    {
        try
        {
            var query = new GraphQLQuery
            {
                Query = @"
                    query {
                        getCourses {
                            id
                            imageUri
                            isBestseller
                            title
                            authors {
                                name
                            }
                            prices {
                                currency
                                price
                                discount
                            }
                            starRating
                            reviews
                            likesInProcent
                            likes
                            hours
                        }
                    }"
            };


            var response = await _httpClient.PostAsJsonAsync("https://courseprovider-nikesjo.azurewebsites.net/api/graphql", query);
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<DynamicGraphQLResponse>();
                if (result?.Data.TryGetProperty("getCourses", out var coursesJson) == true)
                {
                    var courses = new List<CourseCardModel>();
                    courses = coursesJson.EnumerateArray()
                        .Select(course => new CourseCardModel
                        {
                            Id = course.GetProperty("id").GetString()!,
                            IsBestseller = course.GetProperty("isBestseller").GetBoolean(),
                            ImageUri = course.GetProperty("imageUri").GetString(),
                            Title = course.GetProperty("title").GetString(),
                            Authors = course.GetProperty("authors").EnumerateArray()
                                .Select(author => new AuthorCardModel
                                {
                                    Name = author.GetProperty("name").GetString()!,
                                }).ToList(),
                            Prices = new PricesCardModel
                            {
                                Currency = course.GetProperty("prices").GetProperty("currency").GetString(),
                                Price = course.GetProperty("prices").GetProperty("price").GetDecimal(),
                                Discount = course.GetProperty("prices").GetProperty("discount").GetDecimal()
                            },
                            StarRating = course.GetProperty("starRating").GetDecimal(),
                            Reviews = course.GetProperty("reviews").GetString(),
                            LikesInProcent = course.GetProperty("likesInProcent").GetString(),
                            Likes = course.GetProperty("likes").GetString(),
                            Hours = course.GetProperty("hours").GetString()
                        }).ToList();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("GraphQL error content: " + errorContent);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        return null!;
    }

    public async Task<Course> CreateCourseAsync(object mutation)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:7117/api/graphql", mutation);
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
