using Microsoft.AspNetCore.Mvc;
using System;
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
        public async Task<IActionResult> SearchNearestCourse(double latitude, double longitude)
        {
            try
            {
                var apiKey = "AIzaSyDrNS4b5GrOQWSivhmmggZVEo9_yof0oP4";
                var radius = 5000; // Search radius in meters
                var keyword = "disc golf course";

                var url = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={latitude},{longitude}&radius={radius}&keyword={Uri.EscapeDataString(keyword)}&key={apiKey}";

                var client = _httpClientFactory.CreateClient();
                var response = await client.GetStringAsync(url);

                var json = JObject.Parse(response);
                var results = json["results"];

                // Limit the number of results to 3 closest courses
                var closestCourses = results.Take(3).Select(course => new
                {
                    name = course["name"].ToString(),
                    address = course["vicinity"].ToString(),
                    distance = CalculateDistance(latitude, longitude,
                                                 double.Parse(course["geometry"]["location"]["lat"].ToString()),
                                                 double.Parse(course["geometry"]["location"]["lng"].ToString())),
                    playUrl = $"~/Course/PlayCourse?courseId={course["place_id"]}" // Example URL, adjust as needed
                });

                if (!closestCourses.Any())
                {
                    return Json(new { message = "No disc golf courses found nearby." });
                }

                return Json(closestCourses);
            }
            catch (Exception ex)
            {
                // Handle exceptions
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

        [HttpGet]
        public IActionResult PlayCourse(string courseId)
        {
            // Retrieve course details based on courseId, if needed
            // Render the gameplay view or redirect to a form to select number of holes
            return View();
        }
    }
}