using API.Data;
using API.Models.Domain;
using API.Models.DTO;
using API.Repositories;

namespace API.Repositories
{
    public class SQLAuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _dbContext;
        public SQLAuthorRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<AuthorDTO> GellAllAuthors()
        {
            //Get Data From Database -Domain Model 
            var allAuthorsDomain = _dbContext.Authors.ToList();
            //Map domain models to DTOs 
            var allAuthorDTO = new List<AuthorDTO>();
            foreach (var authorDomain in allAuthorsDomain)
            {
                allAuthorDTO.Add(new AuthorDTO()
                {
                    Id = authorDomain.Id,
                    FullName = authorDomain.FullName
                });
            }
            //return DTOs 
            return allAuthorDTO;
        }
        public AuthorNoIdDTO GetAuthorById(int id)
        {
            // get book Domain model from Db
            var authorWithIdDomain = _dbContext.Authors.FirstOrDefault(x => x.Id ==
           id);
            if (authorWithIdDomain == null)
            {
                return null;
            }
            //Map Domain Model to DTOs 
            var authorNoIdDTO = new AuthorNoIdDTO
            {
                FullName = authorWithIdDomain.FullName,
            };
            return authorNoIdDTO;
        }
        public AddAuthorRequestDTO AddAuthor(AddAuthorRequestDTO addAuthorRequestDTO)
        {
            var authorDomainModel = new Author
            {
                FullName = addAuthorRequestDTO.FullName,
            };
            //Use Domain Model to create Author 
            _dbContext.Authors.Add(authorDomainModel);
            _dbContext.SaveChanges();
            return addAuthorRequestDTO;
        }
        public AuthorNoIdDTO UpdateAuthorById(int id, AuthorNoIdDTO authorNoIdDTO)
        {
            var authorDomain = _dbContext.Authors.FirstOrDefault(n => n.Id == id);
            if (authorDomain != null)
            {
                authorDomain.FullName = authorNoIdDTO.FullName;
                _dbContext.SaveChanges();
            }
            return authorNoIdDTO;
        }
        public Author? DeleteAuthorById(int id)
        {
            var authorDomain = _dbContext.Authors.FirstOrDefault(n => n.Id == id);
            if (authorDomain != null)
            {
                _dbContext.Authors.Remove(authorDomain);
                _dbContext.SaveChanges();
            }
            return null;
        }
        public List<BookWithAuthorAndPublisherDTO> GetAuthorBooks(int authorId)
        {
            var authorBooks = _dbContext.Books_Authors
                .Where(n => n.AuthorId == authorId)
                .Select(n => new BookWithAuthorAndPublisherDTO()
                {
                    Id = n.Book.Id,
                    Title = n.Book.Title,
                    Description = n.Book.Description,
                    IsRead = n.Book.IsRead,
                    DateRead = n.Book.IsRead ? n.Book.DateRead.Value : null,
                    Rate = n.Book.IsRead ? n.Book.Rate.Value : null,
                    Genre = n.Book.Genre,
                    CoverUrl = n.Book.CoverUrl,
                    PublisherName = n.Book.Publisher.Name,
                    AuthorNames = n.Book.Book_Authors.Select(a => a.Author.FullName).ToList()
                }).ToList();

            return authorBooks;
        }
    }
}