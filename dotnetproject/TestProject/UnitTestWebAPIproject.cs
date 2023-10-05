using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using dotnetapiapp.Controllers;
using dotnetapiapp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
namespace dotnetapiapp.Tests
{
      [TestFixture]
    public class ProjectControllerTests
    {
        private ProjectController _ProjectController;
        private FreelancerProjectDbContext _context;

        [SetUp]
        public void Setup()
        {
            // Initialize an in-memory database for testing
            var options = new DbContextOptionsBuilder<FreelancerProjectDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new FreelancerProjectDbContext(options);
            _context.Database.EnsureCreated(); // Create the database

            // Seed the database with sample data
            _context.Projects.AddRange(new List<Project>
            {
new Project { ProjectID = 1, ProjectName = "Project 1", NumberOfModules = "10", SubmissionDate = DateTime.Parse("2023-08-30"),FreelancerName="Freelancer1" },
new Project { ProjectID = 2, ProjectName = "Project 2", NumberOfModules = "17", SubmissionDate = DateTime.Parse("2023-08-10"),FreelancerName="Freelancer2" },
new Project { ProjectID = 3, ProjectName = "Project 3", NumberOfModules = "16", SubmissionDate = DateTime.Parse("2023-08-20"),FreelancerName="Freelancer3" }
            });
            _context.SaveChanges();

            _ProjectController = new ProjectController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted(); // Delete the in-memory database after each test
            _context.Dispose();
        }
        [Test]
        public void ProjectClassExists()
        {
            // Arrange
            Type ProjectType = typeof(Project);

            // Act & Assert
            Assert.IsNotNull(ProjectType, "Project class not found.");
        }
        [Test]
        public void Project_Properties_ProjectName_ReturnExpectedDataTypes()
        {
            // Arrange
            Project project = new Project();
            PropertyInfo propertyInfo = project.GetType().GetProperty("ProjectName");
            // Act & Assert
            Assert.IsNotNull(propertyInfo, "ProjectName property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "ProjectName property type is not string.");
        }
[Test]
        public void Project_Properties_FreelancerName_ReturnExpectedDataTypes()
        {
            // Arrange
            Project project = new Project();
            PropertyInfo propertyInfo = project.GetType().GetProperty("FreelancerName");
            // Act & Assert
            Assert.IsNotNull(propertyInfo, "FreelancerName property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "FreelancerName property type is not string.");
        }

        [Test]
        public async Task GetAllProjects_ReturnsOkResult()
        {
            // Act
            var result = await _ProjectController.GetAllProjects();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task GetAllProjects_ReturnsAllProjects()
        {
            // Act
            var result = await _ProjectController.GetAllProjects();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;

            Assert.IsInstanceOf<IEnumerable<Project>>(okResult.Value);
            var projects = okResult.Value as IEnumerable<Project>;

            var ProjectCount = projects.Count();
            Assert.AreEqual(3, ProjectCount); // Assuming you have 3 Projects in the seeded data
        }


        [Test]
        public async Task AddProject_ValidData_ReturnsOkResult()
        {
            // Arrange
            var newProject = new Project
            {
ProjectName = "Project New", NumberOfModules = "20", SubmissionDate = DateTime.Parse("2023-08-30"),FreelancerName="FreelancerNew"
            };

            // Act
            var result = await _ProjectController.AddProject(newProject);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
        [Test]
        public async Task DeleteProject_ValidId_ReturnsNoContent()
        {
            // Arrange
              // var controller = new ProjectsController(context);

                // Act
                var result = await _ProjectController.DeleteProject(1) as NoContentResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(204, result.StatusCode);
        }

        [Test]
        public async Task DeleteProject_InvalidId_ReturnsBadRequest()
        {
                   // Act
                var result = await _ProjectController.DeleteProject(0) as BadRequestObjectResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(400, result.StatusCode);
                Assert.AreEqual("Not a valid Project id", result.Value);
        }
    }
}
