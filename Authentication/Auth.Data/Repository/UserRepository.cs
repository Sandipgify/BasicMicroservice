using Auth.Data.Context;
using Auth.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Data.Repository
{
    public interface IUserRepository
    {
        Task Add(User user);
        void Update(User user);
        Task<User> Get(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> Get();
    }
    public class UserRepository : IUserRepository
    {
        private readonly AuthContext _authContext;

        public UserRepository(AuthContext authContext)
        {
            _authContext = authContext;
        }
        public async Task Add(User user)
        {
           await _authContext.AddAsync(user);
            _authContext.SaveChanges();
        }

        public async Task<User> Get(int id)
        {
            return await _authContext.Set<User>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public void Update(User user)
        {
            _authContext.Update(user);
            _authContext.SaveChanges();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {            
            return await _authContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> Get()
        {
            return await _authContext.Set<User>().Where(x => x.IsActive).ToListAsync();
        }
    }
}
