using Day1_WebAPI_CRUD.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

//CRUD Operation Web API

namespace Day1_WebAPI_CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        static List<Product> products = new List<Product>()
        {
            new Product{Id=1, Name="Laptop", Price=80000},
            new Product{Id=2,Name="Mobile", Price=35000 },
            new Product{Id=3,Name="AC", Price=75500 }
        };


        [HttpGet]
        public IActionResult GetProduct()
        {
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetProductByID(int id)
        {
            var product = products.FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return NotFound("Not found");
            }
            return Ok(product);
        }

        [HttpPost]
        public IActionResult AddProduct(ProductDTO productDTO)
        {
            Product product1 = new Product()
            {
                Id = products.Count + 1,
                Name = productDTO.Name,
                Price = productDTO.Price
            };
            products.Add(product1);
            return Created("", product1);
        }

        [HttpPut("{id}")]
        public IActionResult updateProduct(int id, ProductDTO productDTO)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            product.Name = productDTO.Name;
            product.Price = productDTO.Price;
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public IActionResult deleteProduct(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            products.Remove(product);
            return Ok("Product Deleted successfully");
        }

        //LINQ
        [HttpGet ("price")]
        public IActionResult GetProductByPrice()
        {
            var result = products.Where(p => p.Price > 50000).ToList();
            return Ok(result);
        }

        [HttpGet("Name")]
        //Select Only Product Names
        public IActionResult GetByName()
        {
            var result = products.Select(x => x.Name);
            return Ok(result);
        }

        //sort product by price
        [HttpGet("ProductByPrice")]
        public IActionResult GetSortedProduct()
        {
            var result = products.OrderBy(x => x.Price);
            return Ok(result);
        }

    }
}
