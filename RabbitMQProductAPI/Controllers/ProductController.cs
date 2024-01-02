using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQProductAPI.Models;
using RabbitMQProductAPI.RabbitMQ;
using RabbitMQProductAPI.Services;

namespace RabbitMQProductAPI.Controllers;

public class ProductController : ControllerBase
{
    private readonly IProductService productService;
    private readonly IRabitMQProducer _rabitMQProducer;

    public ProductController(IProductService productService, IRabitMQProducer rabitMQProducer)
    {
        this.productService = productService;
        _rabitMQProducer = rabitMQProducer;
    }

    [HttpGet("productlist")]
    public IEnumerable<Product> ProductList()
    {
        var productList = productService.GetProductList();
        return productList;
    }

    [HttpGet("getproductbyid")]
    public Product GetProductById(int Id)
    {
        return productService.GetProductById(Id);
    }

    [HttpPost("addproduct")]
    public Product AddProduct(Product product)
    {
        var productData = productService.AddProduct(product);

        var factory = new ConnectionFactory
        {
            HostName = "172.18.247.61",
            Port = 8001,
            UserName = "test",
            Password = "test"
        };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            _rabitMQProducer.SendProductMessage(productData, channel);
        }

        return productData;
    }

    [HttpPut("updateproduct")]
    public Product UpdateProduct(Product product)
    {
        return productService.UpdateProduct(product);
    }

    [HttpDelete("deleteproduct")]
    public bool DeleteProduct(int Id)
    {
        return productService.DeleteProduct(Id);
    }
}
