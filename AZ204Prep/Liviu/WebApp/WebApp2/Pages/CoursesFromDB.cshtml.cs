using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using WebApp2.PageModels;

namespace WebApp2.Pages
{
    

    public class CoursesFromDBModel : PageModel
    {
        public List<Course> Courses = new List<Course>();
        private readonly ILogger<IndexModel> _logger;
        private IConfiguration _configuration;

        public CoursesFromDBModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public void OnGet()
        {
            string connectionString = _configuration.GetConnectionString("CoursesDB")!;
            var sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            var sqlcommand = new SqlCommand(
            "SELECT CourseID,CourseName,Rating FROM Course;", sqlConnection);
            using (SqlDataReader sqlDatareader = sqlcommand.ExecuteReader())
            {
                while (sqlDatareader.Read())
                {
                    Courses.Add(new Course()
                    {
                        CourseID = Int32.Parse(sqlDatareader["CourseID"].ToString()),
                        CourseName = sqlDatareader["CourseName"].ToString(),
                        Rating = Decimal.Parse(sqlDatareader["Rating"].ToString())
                    });
                }
            }
        }
    }
}
