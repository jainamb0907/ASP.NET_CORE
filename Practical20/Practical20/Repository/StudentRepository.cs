using Microsoft.EntityFrameworkCore;
using Practical20.Models;
using Practical20.Interfaces;
using Practical20.Models;

namespace Practical20.Repository
{
    public class StudentRepository : GenericRepository<Students>, IStudentRepository
    {
        private readonly AppDbContext _dbContext;

        public StudentRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Students> GetStudentById(int id)
        {
            return await _dbContext.Students.FindAsync(id);
           
        }
    }
}
