using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebUI.Controllers;
using WebUI.Models;
using Domain.Abstract;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebUI.HtmlHelpers;
namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Arrange            
            Mock<IProductRepository> mock = new Mock<IProductRepository>();             
            mock.Setup(m => m.Products).Returns(new Product[] {                 
                new Product {ProductID = 1, Name = "P1"},                 
                new Product {ProductID = 2, Name = "P2"},                 
                new Product {ProductID = 3, Name = "P3"},                
                new Product {ProductID = 4, Name = "P4"},                 
                new Product {ProductID = 5, Name = "P5"}             
            }.AsQueryable());

            ProductController controller = new ProductController(mock.Object);

            controller.PageSize = 3;

            // Act             
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;  

            // Assert             
            Product[] prodArray = result.Products.ToArray();             
            Assert.IsTrue(prodArray.Length == 2);             
            Assert.AreEqual(prodArray[0].Name, "P4");             
            Assert.AreEqual(prodArray[1].Name, "P5"); 
        }                                                          


        [TestMethod]         
        public void Can_Generate_Page_Links() {  
            // Arrange - define an HTML helper - we need to do this             
            // in order to apply the extension method             
            HtmlHelper myHelper = null;  
            // Arrange - create PagingInfo data             
            PagingInfo pagingInfo = new PagingInfo {                 
                CurrentPage = 2,                 
                TotalItems = 28,                 
                ItemsPerPage = 10             
            };  
            // Arrange - set up the delegate using a lambda expression             
            Func<int, string> pageUrlDelegate = i => "Page" + i;  
            // Act             
            MvcHtmlString result = PagingHelpers.PageLinks(myHelper,pagingInfo, pageUrlDelegate);  
            // Assert             
            Assert.AreEqual(result.ToString(), @"<a href=""Page1"">1</a>" + @"<a class=""selected"" href=""Page2"">2</a>" + @"<a href=""Page3"">3</a>");         
        }


        [TestMethod]
        public void Can_Filter_Category()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]{
                new Product {ProductID = 1, Name = "P1", Category = "Cat"},                 
                new Product {ProductID = 2, Name = "P2", Category = "Cat"},                 
                new Product {ProductID = 3, Name = "P3", Category = "Cat"},                
                new Product {ProductID = 4, Name = "P4", Category = "Dog"},                 
                new Product {ProductID = 5, Name = "P5", Category = "Cat"}             
            }.AsQueryable());

            ProductController Controller = new ProductController(mock.Object);

            // Action
            ProductsListViewModel result = (ProductsListViewModel)Controller.List("Cat", 2).Model;
            Product[] product = result.Products.ToArray();

            // Assert
            Assert.AreEqual(product.Length,1);
            Assert.IsTrue(product[0].Name == "P5" && product[0].Category == "Cat") ;
        }
    }    
}
