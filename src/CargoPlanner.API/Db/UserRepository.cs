using System;
using System.Threading.Tasks;
using CargoPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace CargoPlanner.API.Db
{
    public class UserRepository : GenericRepository<User, Guid>
    {
        public UserRepository(CargoPlannerContext context) : base(context)
        {
        }

        public override Task<User> GetById(Guid id)
        {
            return Set.FirstOrDefaultAsync(e => e.Id == id);
        }

        public override Task<bool> Exists(Guid id)
        {
            return Set.AnyAsync(e => e.Id == id);
        }
    }
}