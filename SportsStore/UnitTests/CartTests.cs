using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Entities;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            // Arrange
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Cart target = new Cart();

            // Act
            target.AddItem(p1, 5);
            target.AddItem(p2, 9);
            CartLine[] result = target.Lines.ToArray();

            // Assert 

            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0].Product,p1);
            Assert.AreEqual(result[1].Product, p2);
        }
    }
}
