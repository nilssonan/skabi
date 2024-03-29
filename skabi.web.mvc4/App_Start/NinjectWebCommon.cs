using System.Data.Objects;
using System.Reflection;
using System.Web.Configuration;
using skabi.common.Services;
using skabi.data;
using skabi.data.Repository;
using skabi.data.Repository.Interfaces;

[assembly: WebActivator.PreApplicationStartMethod(typeof(skabi.web.mvc4.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(skabi.web.mvc4.App_Start.NinjectWebCommon), "Stop")]

namespace skabi.web.mvc4.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Load(Assembly.GetExecutingAssembly(),
                        Assembly.Load("skabi.data"));
                //Assembly.Load("skabi.services"));

            kernel.Bind<ObjectContext>()
                  .To<rpdbEntities>()
                  .InRequestScope()
                  .WithConstructorArgument("connectionString",
                                           WebConfigurationManager.ConnectionStrings["rpdbEntities"].ToString());
           
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();
            kernel.Bind<ICarbrandRepository>().To<CarbrandRepository>().InRequestScope();
            kernel.Bind<ICarmodelRepository>().To<CarmodelRepository>().InRequestScope();
            kernel.Bind<INewsRepository>().To<NewsRepository>().InRequestScope();
            kernel.Bind<IProposalRepository>().To<ProposalRepository>().InRequestScope();
            kernel.Bind<ICarmodelTypeRepository>().To<CarmodelTypeRepository>().InRequestScope();
            

            kernel.Bind<INewsService>().To<NewsService>().InRequestScope();
            kernel.Bind<IRpdbService>().To<RpdbService>().InRequestScope();
            



        }        
    }
}
