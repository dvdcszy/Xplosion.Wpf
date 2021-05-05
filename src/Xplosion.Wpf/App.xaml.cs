using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Xplosion.Core;
using Xplosion.Core.FrameworkLibrary;
using Xplosion.Relational;
using Xplosion.Wpf.Core;

namespace Xplosion.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IServiceProvider serviceProvider = CreateServiceProvider();

            IBaseViewModelAbstractFactory baseViewModelAbsrtactFactory = serviceProvider.GetRequiredService<IBaseViewModelAbstractFactory>();

            IMainWindowViewModelFactory vmFactory = new MainWindowViewModelFactory(baseViewModelAbsrtactFactory);
            Window window = new MainWindow();
            IWindow w = new MainWindowAdapter(window, vmFactory);
            w.Show();
            base.OnStartup(e);
        }

        private IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<IHrsgDataModel, HrsgDataModel>()
                    .AddSingleton<IComponentBuilder>(i => new ComponentBuilder(i.GetRequiredService<IHrsgDataModel>()))
                    .AddSingleton<IViewModelFactory<ComponentViewModel>>(i => new ComponentPageViewModelFactory(i.GetRequiredService<IComponentBuilder>()))
                    .AddSingleton<IViewModelFactory<CockpitViewModel>>(i => new CockpitPageViewModelFactory(i.GetRequiredService<IComponentBuilder>()))
                    .AddSingleton<IBaseViewModelAbstractFactory, BaseViewModelAbstractFactory>();


            return services.BuildServiceProvider();
        }
    }
}
