using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ProductsController : Controller
    {
        private readonly OnlineShopContext _context;

        public ProductsController(OnlineShopContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string searchString)
        {
            var products = _context.Product
            .Include(p => p.Category)
            .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString));
            }

            ViewData["CurrentFilter"] = searchString;

            return View(await products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DetailViewModel dvm = new DetailViewModel();  

            var product = await _context.Product
                        .Include(p => p.Category)
                        .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                dvm.product = product;
                if (product.Image != null)
                {
                    dvm.imgsrc = ViewImage(product.Image);
                }
            }
            return View(dvm);
        }
        private string ViewImage(byte[] arrayImage)
        {
            // 二進位圖檔轉字串
            string base64String = Convert.ToBase64String(arrayImage, 0, arrayImage.Length);
            return "data:image/png;base64," + base64String;
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["Categories"] = new SelectList(_context.Set<Category>(), "Id", "Name");
            return View();
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category category) 
        {
            _context.Category.Add(category);
            await _context.SaveChangesAsync();
            return View();
        }
        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Content,Price,Stock,CategoryId")] Product product, IFormFile myimg)
        {
            if (myimg != null)
            {
                using var ms = new MemoryStream();
                await myimg.CopyToAsync(ms);
                product.Image = ms.ToArray();
            }

            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                if (product.Image != null)
                {
                    ViewBag.Image = ViewImage(product.Image);
                }
            }
            ViewData["Categories"] = new SelectList(_context.Set<Category>(), "Id", "Name", product.CategoryId); 

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Content,Price,Stock,CategoryId")] Product product, IFormFile myimg)
        {
            var originalProduct = await _context.Product.FindAsync(id);
            if (originalProduct == null) return NotFound();

            originalProduct.Name = product.Name;
            originalProduct.Description = product.Description;
            originalProduct.Content = product.Content;
            originalProduct.Price = product.Price;
            originalProduct.Stock = product.Stock;
            originalProduct.CategoryId = product.CategoryId;

            if (myimg != null)
            {
                using var ms = new MemoryStream();
                await myimg.CopyToAsync(ms);
                originalProduct.Image = ms.ToArray();
            }

            _context.Update(originalProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
