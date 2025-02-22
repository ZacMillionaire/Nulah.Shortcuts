using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace Nulah.Shortcuts.Core;

public class ImageProcessing
{
	public byte[] ResizeImage(Stream inStream, int width = 50)
	{
		using Image image = Image.Load(inStream);
		using MemoryStream ms = new MemoryStream();

		image.Mutate(x => x.Resize(width, 0));

		image.Save(ms, new PngEncoder());

		ms.Position = 0;
		return ms.ToArray();
	}
}