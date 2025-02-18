using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.LogicalTree;

namespace Nulah.Shortcuts.Views.Shortcuts;

/// <summary>
/// TemplatedControl with command bindings
/// </summary>
public class TemplatedCommandControl : TemplatedControl, ICommandSource
{
	private EventHandler? _canExecuteChangeHandler = default;
	private EventHandler CanExecuteChangedHandler => _canExecuteChangeHandler ??= new(CanExecuteChanged);
	private bool _commandCanExecute = true;

	public ICommand? Command
	{
		get => GetValue(CommandProperty);
		set => SetValue(CommandProperty, value);
	}

	public object? CommandParameter
	{
		get => GetValue(CommandParameterProperty);
		set => SetValue(CommandParameterProperty, value);
	}

	public static readonly StyledProperty<object?> CommandParameterProperty =
		AvaloniaProperty.Register<Button, object?>(nameof(CommandParameter));

	public static readonly StyledProperty<ICommand?> CommandProperty =
		AvaloniaProperty.Register<Button, ICommand?>(nameof(Command), enableDataValidation: true);

	public void CanExecuteChanged(object sender, EventArgs e)
	{
		CanExecuteChanged(Command, CommandParameter);
	}

	[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	private void CanExecuteChanged(ICommand? command, object? parameter)
	{
		if (!((ILogical)this).IsAttachedToLogicalTree)
		{
			return;
		}

		var canExecute = command == null || command.CanExecute(parameter);

		if (canExecute != _commandCanExecute)
		{
			_commandCanExecute = canExecute;
			UpdateIsEffectivelyEnabled();
		}
	}

	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
	{
		base.OnPropertyChanged(change);

		if (change.Property == CommandProperty)
		{
			var (oldValue, newValue) = change.GetOldAndNewValue<ICommand?>();
			if (((ILogical)this).IsAttachedToLogicalTree)
			{
				if (oldValue is { } oldCommand)
				{
					oldCommand.CanExecuteChanged -= CanExecuteChangedHandler;
				}

				if (newValue is { } newCommand)
				{
					newCommand.CanExecuteChanged += CanExecuteChangedHandler;
				}
			}

			CanExecuteChanged(newValue, CommandParameter);
		}
		else if (change.Property == CommandParameterProperty)
		{
			CanExecuteChanged(Command, change.NewValue);
		}
	}
}

public class ShortcutItem : TemplatedControl
{
	public static readonly StyledProperty<string> TitleProperty =
		AvaloniaProperty.Register<ShortcutItem, string>(nameof(Title));

	public static readonly StyledProperty<string> ShortcutLinkProperty =
		AvaloniaProperty.Register<ShortcutItem, string>(nameof(ShortcutLink));

	public static readonly StyledProperty<object?> ShortcutContentProperty =
		AvaloniaProperty.Register<ShortcutItem, object?>(nameof(ShortcutContent));

	public static readonly StyledProperty<object?> ShortcutActionsProperty =
		AvaloniaProperty.Register<ShortcutItem, object?>(nameof(ShortcutActions));

	public string Title
	{
		get => GetValue(TitleProperty);
		set => SetValue(TitleProperty, value);
	}

	public string ShortcutLink
	{
		get => GetValue(ShortcutLinkProperty);
		set => SetValue(ShortcutLinkProperty, value);
	}

	public object? ShortcutContent
	{
		get => GetValue(ShortcutContentProperty);
		set => SetValue(ShortcutContentProperty, value);
	}

	public object? ShortcutActions
	{
		get => GetValue(ShortcutActionsProperty);
		set => SetValue(ShortcutActionsProperty, value);
	}
}