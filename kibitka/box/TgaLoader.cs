using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

public static class TgaLoader
{
    public static Bitmap LoadTga(string path)
    {
        using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        using (var reader = new BinaryReader(fs))
        {
            byte[] header = reader.ReadBytes(18);

            byte idLength = header[0];
            byte colorMapType = header[1];
            byte imageType = header[2]; // 2=uncompressed, 10=RLE compressed

            // Check supported types
            if (imageType != 2 && imageType != 10)
                throw new NotSupportedException("Only uncompressed or RLE-compressed true-color TGA supported");

            if (colorMapType != 0)
                throw new NotSupportedException("Color-mapped images are not supported");

            // Skip identification field
            if (idLength > 0)
                reader.ReadBytes(idLength);

            int width = header[12] + (header[13] << 8);
            int height = header[14] + (header[15] << 8);
            int bpp = header[16];
            int bytesPerPixel = bpp / 8;

            byte[] pixelData;

            if (imageType == 10) // RLE compressed
            {
                // Read all remaining bytes as compressed data
                byte[] compressedData = reader.ReadBytes((int)(fs.Length - fs.Position));
                pixelData = DecompressRle(compressedData, width, height, bytesPerPixel);
            }
            else // Uncompressed
            {
                int dataLength = width * height * bytesPerPixel;
                pixelData = reader.ReadBytes(dataLength);
            }

            // Create Bitmap and copy pixel data
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            BitmapData bmpData = bmp.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb
            );

            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0;
                int srcOffset = 0;

                for (int y = height - 1; y >= 0; y--)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (srcOffset + bytesPerPixel > pixelData.Length)
                            break;

                        if (bytesPerPixel == 3) // 24bpp
                        {
                            ptr[(y * width + x) * 4 + 0] = pixelData[srcOffset + 0]; // B
                            ptr[(y * width + x) * 4 + 1] = pixelData[srcOffset + 1]; // G
                            ptr[(y * width + x) * 4 + 2] = pixelData[srcOffset + 2]; // R
                            ptr[(y * width + x) * 4 + 3] = 0xFF; // A
                            srcOffset += 3;
                        }
                        else if (bytesPerPixel == 4) // 32bpp
                        {
                            ptr[(y * width + x) * 4 + 0] = pixelData[srcOffset + 0]; // B
                            ptr[(y * width + x) * 4 + 1] = pixelData[srcOffset + 1]; // G
                            ptr[(y * width + x) * 4 + 2] = pixelData[srcOffset + 2]; // R
                            ptr[(y * width + x) * 4 + 3] = pixelData[srcOffset + 3]; // A
                            srcOffset += 4;
                        }
                    }
                }
            }

            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }

    private static byte[] DecompressRle(byte[] compressedData, int width, int height, int bytesPerPixel)
    {
        int totalPixels = width * height;
        byte[] decompressed = new byte[totalPixels * bytesPerPixel];
        int srcOffset = 0;
        int dstOffset = 0;

        while (srcOffset < compressedData.Length && dstOffset < decompressed.Length)
        {
            byte header = compressedData[srcOffset++];
            int count = (header & 0x7F) + 1;

            if ((header & 0x80) != 0) // Run-length packet
            {
                if (srcOffset + bytesPerPixel > compressedData.Length)
                    break;

                // Copy the next 'bytesPerPixel' bytes as the repeated pixel
                for (int i = 0; i < count; i++)
                {
                    if (dstOffset + bytesPerPixel > decompressed.Length)
                        break;

                    Array.Copy(compressedData, srcOffset, decompressed, dstOffset, bytesPerPixel);
                    dstOffset += bytesPerPixel;
                }
                srcOffset += bytesPerPixel;
            }
            else // Raw packet
            {
                int bytesToCopy = count * bytesPerPixel;
                if (srcOffset + bytesToCopy > compressedData.Length)
                    bytesToCopy = compressedData.Length - srcOffset;

                Array.Copy(compressedData, srcOffset, decompressed, dstOffset, bytesToCopy);
                srcOffset += bytesToCopy;
                dstOffset += bytesToCopy;
            }
        }

        return decompressed;
    }
}