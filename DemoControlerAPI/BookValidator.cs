using FluentValidation;
using DemoModels;
using System.ComponentModel.DataAnnotations;

namespace DemoControlerAPI
{
    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .NotNull();
            RuleFor(x => x.Author)
                .NotEmpty()
                .NotNull();
            RuleFor(x => x.ISBN)
                .NotEmpty()
                .NotNull();
        }

        public void ValidateBook(Book book)
        {
            var validator = new BookValidator();
            validator.Validate(book);
        }
    }
}