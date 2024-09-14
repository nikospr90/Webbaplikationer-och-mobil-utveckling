using DemoModels;

namespace DemoControlerAPI
{
    public class BookService : IBookService
    {
        List<Book> _books;

        public BookService()
        {
            _books = Book.GetSampleData();
        }

        public List<Book> GetBooks()
        {
            return _books;
        }

        public Book GetBookById(int id)
        {
            return _books.FirstOrDefault(b => b.Id == id);
        }

        public void Add(Book book)
        {
            if (_books == null || _books.Count == 0)
            {
                _books.Add(book);
            }
            else
            {
                book.Id = _books.Max(b => b.Id) + 1;
                _books.Add(book);
            }
        }

        public void DeleteBook(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                _books.Remove(book);
            }
        }

        public void UpdateBook(Book book)
        {
            var existingBook = _books.FirstOrDefault(b => b.Id == book.Id);
            if (book != null)
            {
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.Review = book.Review;
                existingBook.Description = book.Description;

            }
            //else
            //{
            //    _books.Remove(existingBook);
            //    _books.Add(book);
            //}
        }
    }
}
