using Microsoft.AspNetCore.Identity;


namespace OnlineShop.Models
{
    public class OnlineShopUser : IdentityUser
    {
        public string? Name { get; set; }                
        public DateTime DOB { get; set; }               
        public GenderType Gender { get; set; }          
        public DateTime RegistrationDate { get; set; }

    }
    public enum GenderType
    {
        Male, Female, Unknown
    }
}
