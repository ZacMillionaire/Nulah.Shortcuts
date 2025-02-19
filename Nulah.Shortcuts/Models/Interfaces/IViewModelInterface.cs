using System;
using ReactiveUI;

namespace Nulah.Shortcuts.Models.Interfaces;

public interface IViewModelInterface : IActivatableViewModel
{
	/// <summary>
	/// Used for <see cref="ServiceViewLocator"/> to resolve the underlying View registered
	/// </summary>
	public Type ViewModelInterface { get; }
}