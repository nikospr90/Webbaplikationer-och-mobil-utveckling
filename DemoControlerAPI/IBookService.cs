using DemoModels;

namespace DemoControlerAPI
{
    public interface IBookService
    {
        Book GetBookById(int id);
        List<Book> GetBooks();

        void Add(Book book);
        void DeleteBook(int id);
        void UpdateBook(Book book);
    }
}