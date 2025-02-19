using System;
using Nulah.Shortcuts.Models.Interfaces;
using ReactiveUI;

namespace Nulah.Shortcuts.ViewModels;

public class ViewModelBase<TViewModelInterface> : ReactiveObject, IActivatableViewModel
	where TViewModelInterface : class, IViewModelInterface
{
	public Type ViewModelInterface { get; } = typeof(TViewModelInterface);
	public ViewModelActivator Activator { get; } = new();
}