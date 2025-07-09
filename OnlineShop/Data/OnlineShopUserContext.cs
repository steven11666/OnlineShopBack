// Data/OnlineShopUserContext.cs
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Models;

public class OnlineShopUserContext : IdentityDbContext<OnlineShopUser>
{
    public OnlineShopUserContext(DbContextOptions<OnlineShopUserContext> options)
        : base(options)
    {
    }

}
