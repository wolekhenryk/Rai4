using Rai4.Domain.Models;
using Rai4.Infrastructure.Data;
using Rai4.Infrastructure.Repositories.Interfaces;

namespace Rai4.Infrastructure.Repositories.Implementation;

public class UserRepository(AppDbContext dbContext) : Repository<User>(dbContext), IUserRepository;