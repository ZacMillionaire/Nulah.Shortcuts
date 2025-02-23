using Nulah.Shortcuts.Domain.Enums;

namespace Nulah.Shortcuts.Data.Models;

internal class Shortcut : BaseEntity
{
	public string Title { get; set; } = null!;
	public string Link { get; set; } = null!;
	public ShortcutType Type { get; set; }
	public byte[]? ImageBlob { get; set; }
	public bool IsDeleted { get; set; }
}