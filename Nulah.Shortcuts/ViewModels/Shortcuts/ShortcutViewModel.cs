using Nulah.Shortcuts.Domain;
using Nulah.Shortcuts.Domain.Enums;
using ReactiveUI.SourceGenerators;

namespace Nulah.Shortcuts.ViewModels.Shortcuts;

public partial class ShortcutViewModel : DtoViewModelBase
{
	[Reactive]
	private bool _deleteClicked;

	public ShortcutViewModel(ShortcutDto newShortcut)
	{
		Id = newShortcut.Id;
		Title = newShortcut.Title;
		ShortcutLocation = newShortcut.ShortcutLocation;
		Type = newShortcut.Type;
		ImageBlob = newShortcut.ImageBlob;
		IsDeleted = newShortcut.IsDeleted;
	}

	public int Id { get; set; }
	public string Title { get; set; } = null!;
	public string ShortcutLocation { get; set; } = null!;
	public ShortcutType Type { get; set; }
	public byte[]? ImageBlob { get; set; }
	public bool IsDeleted { get; set; }
}