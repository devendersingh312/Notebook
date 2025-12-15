using Notes.Api.Models;

namespace Notes.Api.Repositories
{
    public interface INotesRepository
    {
        IEnumerable<Note> GetAll();
        Note? Get(Guid id);
        void Add(Note note);
        void Update(Note note);
        void Delete(Guid id);
    }
}
