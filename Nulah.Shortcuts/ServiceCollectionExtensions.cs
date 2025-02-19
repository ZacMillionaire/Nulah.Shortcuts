using Microsoft.Extensions.DependencyInjection;
using Nulah.Shortcuts.Models.Interfaces;
using Nulah.Shortcuts.ViewModels;
using ReactiveUI;

namespace Nulah.Shortcuts;

internal static class ServiceCollectionExtensions
{
	internal static IServiceCollection AddView<TView, TViewModel>(this IServiceCollection serviceCollection)
		where TView : class, IViewFor<TViewModel>
		where TViewModel : class, IViewModelInterface
	{
		return serviceCollection.AddTransient<IViewFor<TViewModel>, TView>();
	}

	internal static IServiceCollection AddViewModel<TViewModelInterface, TViewModelBaseImplementation>(this IServiceCollection serviceCollection)
		where TViewModelBaseImplementation : ViewModelBase<TViewModelInterface>, TViewModelInterface
		where TViewModelInterface : class, IViewModelInterface
	{
		return serviceCollection.AddTransient<TViewModelInterface, TViewModelBaseImplementation>();
	}
}