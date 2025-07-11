using Microsoft.EntityFrameworkCore;

namespace CarInsuranceBot.Infrastructure.Context;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
}
