using System.Drawing;
namespace Modules.Helpers;

public class ImageHelper
{
    public static byte[] ImageToByteArray(string imagePath)
    {
        using (Image image = Image.FromFile(imagePath))
        using (MemoryStream ms = new MemoryStream())
        {
            image.Save(ms, image.RawFormat);
            return ms.ToArray();
        }
    }
}
