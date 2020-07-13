using System;
using System.Linq;
using Castle.MicroKernel;
using CodeMagic.UI.Presenters;

namespace CodeMagic.UI.Mono
{
    public class ApplicationController : IApplicationController
    {
        private readonly IKernel kernel;

        public ApplicationController(IKernel kernel)
        {
            this.kernel = kernel;
        }
        public TPresenter CreatePresenter<TPresenter>() where TPresenter : class, IPresenter
        {
            var constructors = typeof(TPresenter).GetConstructors();
            if (constructors.Length > 1)
                throw new ApplicationException($"Multiple constructors found for presenter {typeof(TPresenter).Name}");
            if (constructors.Length == 0)
                throw new ApplicationException($"No constructors found for presenter {typeof(TPresenter).Name}");

            var constructor = constructors[0];
            var arguments = constructor.GetParameters().Select(parameter => kernel.Resolve(parameter.ParameterType)).ToArray();
            return (TPresenter) constructor.Invoke(arguments);
        }
    }
}