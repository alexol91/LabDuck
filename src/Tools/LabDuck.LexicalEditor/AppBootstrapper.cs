using Autofac;
using Caliburn.Micro;
using LabDuck.LexicalEditor.Services;
using LabDuck.LexicalEditor.Services.Contracts;
using LabDuck.LexicalEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LabDuck.LexicalEditor
{
    public class AppBootstrapper : BootstrapperBase
    {
        IContainer container;

        public AppBootstrapper()
        {
            Initialize();
        }

        protected AppBootstrapper(bool useApplication = true) : base(useApplication)
        {

        }

        protected override void Configure()
        {
            var builder = new ContainerBuilder();

            //Caliburn
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterType<WindowManager>().As<IWindowManager>().SingleInstance();

            //Services
            builder.RegisterType<FileManagementService>().As<IFileManagementService>().SingleInstance();

            //ViewModels
            builder.RegisterType<StartUpDialogViewModel>().SingleInstance();
            builder.RegisterType<MainViewModel>().SingleInstance();

            container = builder.Build();
        }

        protected override object GetInstance(Type service, string key)
        {
            var instance = container.Resolve(service);
            if (instance != null)
            {
                return instance;
            }
            throw new InvalidOperationException("Could not locate any instances.");
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            var assemblies = GetAssemblies().ToList();
            assemblies.Add(Assembly.GetExecutingAssembly());
            return assemblies;
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            DisplayRootViewFor<StartUpDialogViewModel>();
        }

        private IEnumerable<Assembly> GetAssemblies()
        {
            var assemblyNames = Assembly.GetEntryAssembly().GetReferencedAssemblies().ToList();
            return assemblyNames.Select(Assembly.Load).ToList();
        }
    }
}
