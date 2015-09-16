using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ListOfEmployees.Domain.Concrete;
using ListOfEmployees.Domain.Interface;
using Ninject;

namespace ListOfEmployees.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            // There are bindings here
            kernel.Bind<IEmployeeRepository>().To<EFEmployeeRepository>();
            kernel.Bind<IUserRepository>().To<EFUserRepository>();
        }
    }
}