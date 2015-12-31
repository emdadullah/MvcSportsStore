using Domain.Abstract;
using Domain.Entities;
using Moq;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Configuration; 
using Domain.Concrete;


 
namespace WebUI.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory {
        public IKernel ninjectKernel;

        public NinjectControllerFactory() {
            ninjectKernel = new StandardKernel();
            AddBinding();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)ninjectKernel.Get(controllerType);
        } 

        private void AddBinding()
        {

            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product> 
             {  
                  new Product {ProductID =1, Name="Ball", Description="Red color", Price= 500, Category="Cricket"},
                  new Product {ProductID =2, Name="Ball", Description="Red white", Price= 550, Category="Cricket"},
                  new Product {ProductID =3, Name="Ball", Description="Red pink", Price= 600, Category="Cricket"},
                  new Product {ProductID =4, Name="FootBall", Description="Export quality", Price= 1500, Category="Football"},
                  new Product {ProductID =7, Name="Bat", Description="MRF", Price= 7500, Category="Cricket"}


             }.AsQueryable());

            ninjectKernel.Bind<IProductRepository>().ToConstant(mock.Object);

            EmailSettings emailSettings = new EmailSettings { 
                WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false") };
            ninjectKernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>()
                .WithConstructorArgument("settings", emailSettings); 
            //ninjectKernel.Bind<IProductRepository>().To<EFProductRepository>(); 
        }
    }
}