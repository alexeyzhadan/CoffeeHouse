using CoffeeHouse.Models;
using CoffeeHouse.Services.CustomSelectList.Interfaces;
using CoffeeHouse.Services.DbRepositories.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace CoffeeHouse.Services.CustomSelectList
{
    public class CustomSelectList : ICustomSelectList
    {
        private readonly ICashierRepository _cashierRepository;
        private readonly IProductRepository _productRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ICategoryRepository _categoryRepository;

        public CustomSelectList(
            ICashierRepository cashierRepository,
            IProductRepository productRepository,
            IClientRepository clientRepository,
            ICategoryRepository categoryRepository)
        {
            _cashierRepository = cashierRepository;
            _productRepository = productRepository;
            _clientRepository = clientRepository;
            _categoryRepository = categoryRepository;
        }

        public SelectList CreateListOfCashierFullNames()
        {
            return new SelectList(_cashierRepository.GetAllOrderedByFullName(), "Id", "FullName");
        }

        public SelectList CreateListOfCashierFullNames(int defaultIntId)
        {
            return new SelectList(_cashierRepository.GetAllOrderedByFullName(), "Id", "FullName", defaultIntId);
        }

        public SelectList CreateListOfCategoryNames()
        {
            return new SelectList(_categoryRepository.GetAllOrderedByName(), "Id", "Name");
        }

        public SelectList CreateListOfCategoryNames(int defaultIntId)
        {
            return new SelectList(_categoryRepository.GetAllOrderedByName(), "Id", "Name", defaultIntId);
        }

        public SelectList CreateListOfClientNames()
        {
            return new SelectList(GetListOfClientNamesWithEmptyItem(), "Id", "Name");
        }

        public SelectList CreateListOfClientNames(int defaultIntId)
        {
            return new SelectList(GetListOfClientNamesWithEmptyItem(), "Id", "Name", defaultIntId);
        }

        private List<object> GetListOfClientNamesWithEmptyItem()
        {
            var list = _clientRepository
                .GetAllOrderedByName()
                .Select(c => 
                    new 
                    { 
                        Name = c.Name,
                        Id = c.Id.ToString() 
                    })
                .ToList<object>();

            list.Insert(0, 
                new 
                { 
                    Name = "Unregistered", 
                    Id = "" 
                });

            return list;
        }

        public SelectList CreateListOfOrderProdMarks()
        {
            return new SelectList(new string[] { "None", "Cashback" });
        }

        public SelectList CreateListOfOrderProdMarks(string defaultMark)
        {
            return new SelectList(new string[] { "None", "Cashback" }, defaultMark);
        }

        public SelectList CreateListOfProductFullNames()
        {
            return new SelectList(GetProductFullNames(), "Id", "Name");
        }

        public SelectList CreateListOfProductFullNames(int defaultIntId)
        {
            return new SelectList(GetProductFullNames(), "Id", "Name", defaultIntId);
        }

        private IEnumerable<object> GetProductFullNames()
        {
            var products = _productRepository
                .GetAllWithCategoryOrderedByCategoryNameThenByName();
            var fullNameProducts = products.Select(p => new
            {
                Id = p.Id,
                Name = p.Name + ", " + p.Quantity
            }).AsEnumerable<object>();
            return fullNameProducts;
        }
    }
}