using Notes.Api.Models;

namespace Notes.Api.Repositories
{
    public class NotesRepository : INotesRepository
    {
        private readonly List<Note> _notes = new()
    {
        new Note
        {
            Id = Guid.NewGuid(),
            Title = "First Note",
            Content = "This is a sample note about .NET Aspire.",
            CreatedAt = DateTime.UtcNow,
            Sentiment = "Positive"
        },
        new Note
        {
            Id = Guid.NewGuid(),
            Title = "Second Note",
            Content = "Learning how to integrate AI in our notes app.",
            CreatedAt = DateTime.UtcNow,
            Sentiment = "Neutral"
        },
        new Note
        {
            Id = Guid.NewGuid(),
            Title = "Third Note",
            Content = "Need to fix a bug in the API.",
            CreatedAt = DateTime.UtcNow,
            Sentiment = "Negative"
        }
    };

        public IEnumerable<Note> GetAll() => _notes;

        public Note? Get(Guid id) => _notes.FirstOrDefault(n => n.Id == id);

        public void Add(Note note) => _notes.Add(note);

        public void Update(Note note)
        {
            var index = _notes.FindIndex(n => n.Id == note.Id);
            if (index != -1) _notes[index] = note;
        }

        public void Delete(Guid id) => _notes.RemoveAll(n => n.Id == id);
    }
}
