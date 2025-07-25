using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarInsuranceBot.Infrastructure.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Policy> Policies { get; set; }
    public DbSet<ExtractedField> ExtractedFields { get; set; }
    public DbSet<Error> Errors { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<Conversation> Conversations { get; set; }
}