using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using dotnetmvcapp.Controllers;
using dotnetmvcapp.Models;
using dotnetmvcapp.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NUnit.Framework;
using Moq;

namespace dotnetmvcapp.Tests
{
      [TestFixture]
    public class FreelancerControllerTests
    {
        private Mock<IFreelancerService> mockFreelancerService;
        private FreelancerController controller;
        [SetUp]
        public void Setup()
        {
            mockFreelancerService = new Mock<IFreelancerService>();
            controller = new FreelancerController(mockFreelancerService.Object);
        }

        [Test]
        public void AddFreelancer_ValidData_SuccessfulAddition_RedirectsToIndex()
        {
            // Arrange
            var mockFreelancerService = new Mock<IFreelancerService>();
            mockFreelancerService.Setup(service => service.AddFreelancer(It.IsAny<Freelancer>())).Returns(true);
            var controller = new FreelancerController(mockFreelancerService.Object);
            var freelancer = new Freelancer(); // Provide valid Freelancer data

            // Act
            var result = controller.AddFreelancer(freelancer) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }
        [Test]
        public void AddFreelancer_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            var mockFreelancerService = new Mock<IFreelancerService>();
            var controller = new FreelancerController(mockFreelancerService.Object);
            Freelancer invalidFreelancer = null; // Invalid Freelancer data

            // Act
            var result = controller.AddFreelancer(invalidFreelancer) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Invalid Freelancer data", result.Value);
        }
        [Test]
        public void AddFreelancer_FailedAddition_ReturnsViewWithModelError()
        {
            // Arrange
            var mockFreelancerService = new Mock<IFreelancerService>();
            mockFreelancerService.Setup(service => service.AddFreelancer(It.IsAny<Freelancer>())).Returns(false);
            var controller = new FreelancerController(mockFreelancerService.Object);
            var freelancer = new Freelancer(); // Provide valid Freelancer data

            // Act
            var result = controller.AddFreelancer(freelancer) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(controller.ModelState.IsValid);
            // Check for expected model state error
            Assert.AreEqual("Failed to add the Freelancer. Please try again.", controller.ModelState[string.Empty].Errors[0].ErrorMessage);
        }


        [Test]
        public void AddFreelancer_Post_ValidModel_ReturnsRedirectToActionResult()
        {
            // Arrange
            var mockFreelancerService = new Mock<IFreelancerService>();
            mockFreelancerService.Setup(service => service.AddFreelancer(It.IsAny<Freelancer>())).Returns(true);
            var controller = new FreelancerController(mockFreelancerService.Object);
            var freelancer = new Freelancer();

            // Act
            var result = controller.AddFreelancer(freelancer) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public void AddFreelancer_Post_InvalidModel_ReturnsViewResult()
        {
            // Arrange
            var mockFreelancerService = new Mock<IFreelancerService>();
            var controller = new FreelancerController(mockFreelancerService.Object);
            controller.ModelState.AddModelError("error", "Error");
            var freelancer = new Freelancer();

            // Act
            var result = controller.AddFreelancer(freelancer) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(freelancer, result.Model);
        }

        [Test]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            var mockFreelancerService = new Mock<IFreelancerService>();
            mockFreelancerService.Setup(service => service.GetAllFreelancers()).Returns(new List<Freelancer>());
            var controller = new FreelancerController(mockFreelancerService.Object);

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ViewName);
        }

        [Test]
        public void Delete_ValidId_ReturnsRedirectToActionResult()
        {
            // Arrange
            var mockFreelancerService = new Mock<IFreelancerService>();
            mockFreelancerService.Setup(service => service.DeleteFreelancer(1)).Returns(true);
            var controller = new FreelancerController(mockFreelancerService.Object);

            // Act
            var result = controller.Delete(1) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public void Delete_InvalidId_ReturnsViewResult()
        {
            // Arrange
            var mockFreelancerService = new Mock<IFreelancerService>();
            mockFreelancerService.Setup(service => service.DeleteFreelancer(1)).Returns(false);
            var controller = new FreelancerController(mockFreelancerService.Object);

            // Act
            var result = controller.Delete(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Error", result.ViewName);
        }
    }
}
