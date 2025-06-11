using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }            
        public string Description { get; set; }     
        public string Content { get; set; }         
        public int Price { get; set; }              
        public int Stock { get; set; }              
        public byte[] Image { get; set; }           

        public int CategoryId { get; set; }         
        public Category Category { get; set; }
    }
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }            //類別名稱
        public List<Product> Products { get; set; }
    }
}
