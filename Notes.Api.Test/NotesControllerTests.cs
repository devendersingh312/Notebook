using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Notes.Api.Controllers;
using Notes.Api.Models;
using Notes.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Notes.Api.Test
{

    [TestClass]
    public class NotesControllerTests
    {
        private Mock<INotesRepository> _mockRepo;
        private NotesController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<INotesRepository>();
            _controller = new NotesController(_mockRepo.Object);
        }

        #region GET ALL

        [TestMethod]
        public void Get_ReturnsAllNotes()
        {
            // Arrange
            var notes = new List<Note>
        {
            new Note { Id = Guid.NewGuid(), Title = "Note 1" },
            new Note { Id = Guid.NewGuid(), Title = "Note 2" }
        };

            _mockRepo.Setup(r => r.GetAll()).Returns(notes);

            // Act
            var result = _controller.Get();

            // Assert
            Assert.AreEqual(2, result.Count());
            _mockRepo.Verify(r => r.GetAll(), Times.Once);
        }

        #endregion

        #region GET BY ID

        [TestMethod]
        public void Get_WithValidId_ReturnsOk()
        {
            // Arrange
            var id = Guid.NewGuid();
            var note = new Note { Id = id, Title = "Test Note" };

            _mockRepo.Setup(r => r.Get(id)).Returns(note);

            // Act
            var result = _controller.Get(id);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(note, okResult.Value);
        }

        [TestMethod]
        public void Get_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepo.Setup(r => r.Get(id)).Returns((Note)null);

            // Act
            var result = _controller.Get(id);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        #endregion

        #region POST

        [TestMethod]
        public void Post_AddsNote_ReturnsCreatedAtAction()
        {
            // Arrange
            var note = new Note
            {
                Id = Guid.NewGuid(),
                Title = "New Note"
            };

            // Act
            var result = _controller.Post(note);

            // Assert
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(nameof(NotesController.Get), createdResult.ActionName);
            Assert.AreEqual(note, createdResult.Value);

            _mockRepo.Verify(r => r.Add(note), Times.Once);
        }

        #endregion

        #region PUT

        [TestMethod]
        public void Put_IdMismatch_ReturnsBadRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            var note = new Note { Id = Guid.NewGuid() };

            // Act
            var result = _controller.Put(id, note);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockRepo.Verify(r => r.Update(It.IsAny<Note>()), Times.Never);
        }

        [TestMethod]
        public void Put_ValidId_ReturnsNoContent()
        {
            // Arrange
            var id = Guid.NewGuid();
            var note = new Note { Id = id };

            // Act
            var result = _controller.Put(id, note);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            _mockRepo.Verify(r => r.Update(note), Times.Once);
        }

        #endregion

        #region DELETE

        [TestMethod]
        public void Delete_ReturnsNoContent()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var result = _controller.Delete(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            _mockRepo.Verify(r => r.Delete(id), Times.Once);
        }

        #endregion
    }
}
