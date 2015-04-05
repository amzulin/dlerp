using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac.Integration.Mvc;
using System.Reflection;

namespace Enterprise.Invoicing.Web
{
    public class IocConfig
    {
        public static void Register(string option)
        {
            //var builder = new ContainerBuilder();
            //var assemblys = AppDomain.CurrentDomain.GetAssemblies().ToArray();
            //builder.RegisterControllers(assemblys);

            //builder.RegisterAssemblyTypes(assemblys)
            //    .Where(r => r.Name.EndsWith(option))
            //    .AsImplementedInterfaces();

            //var container = builder.Build();
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            var builder = new ContainerBuilder();

            var assemblies = System.Web.Compilation.BuildManager.GetReferencedAssemblies()
                .Cast<Assembly>()
                .Where(e => e.FullName.StartsWith("Enterprise.Invoicing"))
                .ToArray();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());  	//注册控制器

          
            builder.RegisterAssemblyTypes(assemblies)    //注册程序集
                .Where(r => r.Name.EndsWith(option))
                .AsImplementedInterfaces();




            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}