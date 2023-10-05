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
    public class FreelancerControllerTests
    {
        private FreelancerController _FreelancerController;
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
            _context.Freelancers.AddRange(new List<Freelancer>
            {
                new Freelancer { FreelancerID = 1, FreelancerName = "Freelancer 1", Specialization = "special1", CommercialPerHour = 1000,MailID="Freelancermail1@gmaul.com",ContactNumber="1234567890" },
                new Freelancer { FreelancerID = 2, FreelancerName = "Freelancer 2", Specialization = "special2", CommercialPerHour = 2000,MailID="Freelancermail2@gmaul.com",ContactNumber="9876543210" },
                new Freelancer { FreelancerID = 3, FreelancerName = "Freelancer 3", Specialization = "special3", CommercialPerHour = 3000,MailID="Freelancermail3@gmaul.com",ContactNumber="9876543212" }
            });
            _context.SaveChanges();

            _FreelancerController = new FreelancerController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted(); // Delete the in-memory database after each test
            _context.Dispose();
        }
        [Test]
        public void FreelancerClassExists()
        {
            // Arrange
            Type FreelancerType = typeof(Freelancer);

            // Act & Assert
            Assert.IsNotNull(FreelancerType, "Freelancer class not found.");
        }
        [Test]
        public void Freelancer_Properties_FreelancerName_ReturnExpectedDataTypes()
        {
            // Arrange
            Freelancer freelancer = new Freelancer();
            PropertyInfo propertyInfo = freelancer.GetType().GetProperty("FreelancerName");
            // Act & Assert
            Assert.IsNotNull(propertyInfo, "FreelancerName property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "FreelancerName property type is not string.");
        }
[Test]
        public void Freelancer_Properties_Specialization_ReturnExpectedDataTypes()
        {
            // Arrange
            Freelancer freelancer = new Freelancer();
            PropertyInfo propertyInfo = freelancer.GetType().GetProperty("Specialization");
            // Act & Assert
            Assert.IsNotNull(propertyInfo, "Specialization property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "Specialization property type is not string.");
        }

        [Test]
        public async Task GetAllFreelancers_ReturnsOkResult()
        {
            // Act
            var result = await _FreelancerController.GetAllFreelancers();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task GetAllFreelancers_ReturnsAllFreelancers()
        {
            // Act
            var result = await _FreelancerController.GetAllFreelancers();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;

            Assert.IsInstanceOf<IEnumerable<Freelancer>>(okResult.Value);
            var freelancers = okResult.Value as IEnumerable<Freelancer>;

            var FreelancerCount = freelancers.Count();
            Assert.AreEqual(3, FreelancerCount); // Assuming you have 3 Freelancers in the seeded data
        }


        [Test]
        public async Task AddFreelancer_ValidData_ReturnsOkResult()
        {
            // Arrange
            var newFreelancer = new Freelancer
            {
 FreelancerName = "Freelancer New", Specialization = "specialnew", CommercialPerHour = 4000,MailID="Freelancermailnew@gmaul.com",ContactNumber="9934567890"
            };

            // Act
            var result = await _FreelancerController.AddFreelancer(newFreelancer);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
        [Test]
        public async Task DeleteFreelancer_ValidId_ReturnsNoContent()
        {
            // Arrange
              // var controller = new FreelancersController(context);

                // Act
                var result = await _FreelancerController.DeleteFreelancer(1) as NoContentResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(204, result.StatusCode);
        }

        [Test]
        public async Task DeleteFreelancer_InvalidId_ReturnsBadRequest()
        {
                   // Act
                var result = await _FreelancerController.DeleteFreelancer(0) as BadRequestObjectResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(400, result.StatusCode);
                Assert.AreEqual("Not a valid Freelancer id", result.Value);
        }
    }
}
