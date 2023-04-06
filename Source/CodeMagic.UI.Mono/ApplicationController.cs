using System;
using CodeMagic.UI.Presenters;
using Microsoft.Extensions.DependencyInjection;

namespace CodeMagic.UI.Mono
{
    public class ApplicationController : IApplicationController
    {
        private readonly IServiceProvider _serviceProvider;

        public ApplicationController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public TPresenter CreatePresenter<TPresenter>() where TPresenter : class, IPresenter
        {
            return _serviceProvider.GetRequiredService<TPresenter>();
        }
    }
}
