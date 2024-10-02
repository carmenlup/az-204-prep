using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class CourseSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Seed data for Course entity using raw SQL
            migrationBuilder.Sql(@"
                INSERT INTO Courses (Name, Rating) VALUES ('Docker and Kubernetes', 4.5);
                INSERT INTO Courses (Name, Rating) VALUES ('AZ-204 Azure Developer', 4.0);
                INSERT INTO Courses (Name, Rating) VALUES ('AZ-104 Administrator', 3.5);
            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove seed data for Course entity using raw SQL
            migrationBuilder.Sql(@"
                DELETE FROM Courses WHERE Name IN ('Docker and Kubernetes', 'AZ-204 Azure Developer', 'AZ-104 Administrator');
            ");
        }
    }
}
