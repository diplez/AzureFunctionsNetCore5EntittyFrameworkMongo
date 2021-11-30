using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Blog.Service.BlogApi.Infrastructure.Configurations;
using Blog.Service.BlogApi.Domain.Posts;

namespace Blog.Service.BlogApi.Infrastructure.Contexts
{
    public interface IBlogContext
    {
        IMongoCollection<Post> Posts { get; }
        IMongoCollection<T> GetCollection<T>(string name);
    }
    public class BlogContext : IBlogContext
    {
        private readonly IMongoDatabase db;

        public BlogContext(IOptions<BlogConfiguration> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            db = client.GetDatabase(options.Value.Database);
        }
        public IMongoCollection<Post> Posts => db.GetCollection<Post>("Posts");

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return db.GetCollection<T>(name);
        }
    }
}