using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook.Domain;

namespace TweetBook.Services
{
    public interface IPostService
    {
        Task<List<Post>> GetAllAsync();
        Task<Post> GetPostByIdAsync(Guid id);

        void AddPost(Post post);

        Task<bool> UpdatePostAsync(Post PostToUpdate);
        Task<bool> DeletePostAsync(Guid postId);

        Task<bool> CreatePostAsync(Post post);
    }
}
