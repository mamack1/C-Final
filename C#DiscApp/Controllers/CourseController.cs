using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace C_DiscApp.Controllers
{
    public class CourseController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CourseController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SearchNearestCourse(double latitude, double longitude, int radius = 5000)
        {
            try
            {
                var apiKey = "AIzaSyDuGEZIxHMoYgkXNgPHa6teNekF6NS9ktc ";
                var keyword = "disc golf course";

                var url = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={latitude},{longitude}&radius={radius}&keyword={Uri.EscapeDataString(keyword)}&key={apiKey}";

                var client = _httpClientFactory.CreateClient();
                var response = await client.GetStringAsync(url);

                var json = JObject.Parse(response);
                var results = json["results"];

                // Filter out duplicate addresses and calculate distances
                var uniqueCourses = new List<(string name, string address, double? distance)>();
                var uniqueAddresses = new HashSet<string>();

                foreach (var course in results)
                {
                    var courseAddress = course["vicinity"].ToString();
                    if (!uniqueAddresses.Contains(courseAddress))
                    {
                        uniqueAddresses.Add(courseAddress);

                        var courseName = course["name"].ToString();
                        var courseLat = double.Parse(course["geometry"]["location"]["lat"].ToString());
                        var courseLng = double.Parse(course["geometry"]["location"]["lng"].ToString());
                        var distance = CalculateDistance(latitude, longitude, courseLat, courseLng);

                        uniqueCourses.Add((courseName, courseAddress, distance));
                    }
                }

                // Sort the courses by distance and take the top 5
                var closestCourses = uniqueCourses
                    .Where(course => course.distance.HasValue)
                    .OrderBy(course => course.distance.Value)
                    .Take(5)
                    .Select(course => new
                    {
                        course.name,
                        course.address,
                        distance = course.distance.Value
                    });

                if (!closestCourses.Any())
                {
                    return Json(new { message = "No disc golf courses found nearby." });
                }

                return Json(closestCourses);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return Json(new { error = ex.Message });
            }
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371e3; // Earth's radius in meters
            var phi1 = lat1 * Math.PI / 180;
            var phi2 = lat2 * Math.PI / 180;
            var deltaPhi = (lat2 - lat1) * Math.PI / 180;
            var deltaLambda = (lon2 - lon1) * Math.PI / 180;

            var a = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2) +
                    Math.Cos(phi1) * Math.Cos(phi2) *
                    Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var distance = R * c;
            return distance / 1000; // Convert to kilometers
        }
    }
}
