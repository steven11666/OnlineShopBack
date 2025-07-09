using OnlineShop.Models;

namespace OnlineShop.Areas.Identity.Data
{
    public class OnlineShopUserRole
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
    }
    public class OnlineShopUserViewModel
    {
        public OnlineShopUser User { get; set; }
        public string RoleName { get; set; }
    }
}
