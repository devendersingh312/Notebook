using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notes.Api.Models;
using Notes.Api.Repositories;

namespace Notes.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesRepository _repository;

        public NotesController(INotesRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<Note> Get() => _repository.GetAll();

        [HttpGet("{id}")]
        public ActionResult<Note> Get(Guid id)
        {
            var note = _repository.Get(id);
            return note is null ? NotFound() : Ok(note);
        }

        [HttpPost]
        public ActionResult<Note> Post(Note note)
        {
            _repository.Add(note);
            return CreatedAtAction(nameof(Get), new { id = note.Id }, note);
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, Note note)
        {
            if (id != note.Id) return BadRequest();
            _repository.Update(note);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _repository.Delete(id);
            return NoContent();
        }
    }
}
