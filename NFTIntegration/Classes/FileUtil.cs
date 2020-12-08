using Microsoft.JSInterop;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NFTIntegration.Classes
{
    public static class FileUtil
    {
        public async static Task SaveAs(IJSRuntime js, string filename)
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    bitmap.Save(stream, ImageFormat.Jpeg);
                    byte[] byteArray = stream.GetBuffer();

                    await js.InvokeVoidAsync(
                            "downloadFromByteArray",
                            new
                            {
                                ByteArray = byteArray,
                                FileName = filename,
                                ContentType = "image/jpeg"
                            });
                }
            }
        }
    }
}
