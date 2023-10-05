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
    public class ProjectControllerTests
    {
        private Mock<IProjectService> mockProjectService;
        private ProjectController controller;
        [SetUp]
        public void Setup()
        {
            mockProjectService = new Mock<IProjectService>();
            controller = new ProjectController(mockProjectService.Object);
        }

        [Test]
        public void AddProject_ValidData_SuccessfulAddition_RedirectsToIndex()
        {
            // Arrange
            var mockProjectService = new Mock<IProjectService>();
            mockProjectService.Setup(service => service.AddProject(It.IsAny<Project>())).Returns(true);
            var controller = new ProjectController(mockProjectService.Object);
            var project = new Project(); // Provide valid Project data

            // Act
            var result = controller.AddProject(project) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }
        [Test]
        public void AddProject_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            var mockProjectService = new Mock<IProjectService>();
            var controller = new ProjectController(mockProjectService.Object);
            Project invalidProject = null; // Invalid Project data

            // Act
            var result = controller.AddProject(invalidProject) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Invalid Project data", result.Value);
        }
        [Test]
        public void AddProject_FailedAddition_ReturnsViewWithModelError()
        {
            // Arrange
            var mockProjectService = new Mock<IProjectService>();
            mockProjectService.Setup(service => service.AddProject(It.IsAny<Project>())).Returns(false);
            var controller = new ProjectController(mockProjectService.Object);
            var project = new Project(); // Provide valid Project data

            // Act
            var result = controller.AddProject(project) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(controller.ModelState.IsValid);
            // Check for expected model state error
            Assert.AreEqual("Failed to add the Project. Please try again.", controller.ModelState[string.Empty].Errors[0].ErrorMessage);
        }


        [Test]
        public void AddProject_Post_ValidModel_ReturnsRedirectToActionResult()
        {
            // Arrange
            var mockProjectService = new Mock<IProjectService>();
            mockProjectService.Setup(service => service.AddProject(It.IsAny<Project>())).Returns(true);
            var controller = new ProjectController(mockProjectService.Object);
            var project = new Project();

            // Act
            var result = controller.AddProject(project) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public void AddProject_Post_InvalidModel_ReturnsViewResult()
        {
            // Arrange
            var mockProjectService = new Mock<IProjectService>();
            var controller = new ProjectController(mockProjectService.Object);
            controller.ModelState.AddModelError("error", "Error");
            var project = new Project();

            // Act
            var result = controller.AddProject(project) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(project, result.Model);
        }

        [Test]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            var mockProjectService = new Mock<IProjectService>();
            mockProjectService.Setup(service => service.GetAllProjects()).Returns(new List<Project>());
            var controller = new ProjectController(mockProjectService.Object);

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
            var mockProjectService = new Mock<IProjectService>();
            mockProjectService.Setup(service => service.DeleteProject(1)).Returns(true);
            var controller = new ProjectController(mockProjectService.Object);

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
            var mockProjectService = new Mock<IProjectService>();
            mockProjectService.Setup(service => service.DeleteProject(1)).Returns(false);
            var controller = new ProjectController(mockProjectService.Object);

            // Act
            var result = controller.Delete(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Error", result.ViewName);
        }
    }
}
