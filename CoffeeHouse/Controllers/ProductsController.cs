using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoffeeHouse.Models;
using CoffeeHouse.Services.DbRepositories.Interfaces;

namespace CoffeeHouse.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductsController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _productRepository
                .GetAllWithCategoryOrderedByCategoryNameThenByName()
                .ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["CategoryId"] = GetSelectListCategory();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Quantity,Price,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                await _productRepository.AddAsync(product);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = GetSelectListCategory(product.CategoryId);
            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync((int)id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = GetSelectListCategory(product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Quantity,Price,CategoryId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (_productRepository.Exists(product))
                {
                    await _productRepository.UpdateAsync(product);
                }
                else
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = GetSelectListCategory(product.CategoryId);
            return View(product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync((int)id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productRepository.RemoveAsync(product);
            return RedirectToAction(nameof(Index));
        }

        private SelectList GetSelectListCategory()
        { 
            return new SelectList(_categoryRepository.GetAllOrderedByName(), "Id", "Name");
        }

        private SelectList GetSelectListCategory(int defaultItem)
        {
            return new SelectList(_categoryRepository.GetAllOrderedByName(), "Id", "Name", defaultItem);
        }
    }
}