using Microsoft.EntityFrameworkCore;

namespace FunBooksAndVideos.Persistence.Context;

public sealed class FunBooksAndVideosDbContext(DbContextOptions<FunBooksAndVideosDbContext> options)
    : DbContext(options)
{
}
