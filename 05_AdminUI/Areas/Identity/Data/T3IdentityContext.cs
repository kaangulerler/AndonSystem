using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using _05_AdminUI.Areas.Identity.Data;

namespace _05_AdminUI.Areas.Identity.Data;

public class T3IdentityContext : IdentityDbContext<T3IdentityUser>
{
    public T3IdentityContext(DbContextOptions<T3IdentityContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); 
    }
}
