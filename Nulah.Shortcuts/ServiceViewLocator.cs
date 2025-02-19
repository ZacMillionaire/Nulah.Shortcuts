using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nulah.Shortcuts.ViewModels;
using ReactiveUI;

namespace Nulah.Shortcuts;

public class ServiceViewLocator : IViewLocator
{
	private readonly IServiceProvider _provider;
	private readonly ILogger<ServiceViewLocator> _logger;
	private readonly MethodInfo _getViewMethod;

	public ServiceViewLocator(IServiceProvider provider, ILogger<ServiceViewLocator> logger)
	{
		_provider = provider;
		_logger = logger;
		// Required so we can get the view for a given model as we can't add a class type constraint to ResolveView below
		_getViewMethod = GetType().GetMethod(nameof(GetView), BindingFlags.NonPublic | BindingFlags.Instance)!;
	}

	public IViewFor? ResolveView<T>(T? viewModel, string? contract = null)
	{
		if (viewModel is null)
		{
			return null;
		}

		try
		{
			if (viewModel is IViewModelInterface vm)
			{
				var method = _getViewMethod.MakeGenericMethod(vm.ViewModelInterface);
				var view = (IViewFor?)method.Invoke(this, []);
				return view;
			}

			_logger.LogError("Failed to resolve view for {ViewModel}", typeof(T).FullName);
			return null;
		}
		catch (Exception e)
		{
			_logger.LogError(e, "Failed to resolve view for {ViewModel}", typeof(T).FullName);
			return null;
		}
	}

	private IViewFor GetView<T>() where T : class
	{
		return _provider.GetRequiredService<IViewFor<T>>();
	}
}