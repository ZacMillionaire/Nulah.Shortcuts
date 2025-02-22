using Nulah.Shortcuts.Domain.Enums;

namespace Nulah.Shortcuts.Domain;

public class ShortcutDto
{
	public int Id { get; set; }
	public string Title { get; set; } = null!;
	public string ShortcutLocation { get; set; } = null!;
	public ShortcutType Type { get; set; }
	public byte[]? ImageBlob { get; set; }
}