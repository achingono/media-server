namespace MediaServer.Rest.Processors;

using SkiaSharp;

public class ImageProcessor
{
    public string? ResizeImage(string filePath, int width, int height, string? defaultPath = null)
    {
        if (!File.Exists(filePath) && (string.IsNullOrWhiteSpace(defaultPath) || !File.Exists(defaultPath)))
        {
            return null;
        }


        using var inputStream = File.OpenRead(File.Exists(filePath) ? filePath : defaultPath!);
        // resize image using SkiaSharp
        using var original = SKBitmap.Decode(inputStream);

        // check the width and height values
        // if only one is provided, calculate the other
        if (width <= 0 && height > 0)
        {
            width = (int)(original.Width * height / original.Height);
        }
        else if (width > 0 && height <= 0)
        {
            height = (int)(original.Height * width / original.Width);
        }

        var resizedFilePath = Path.Combine(Path.GetDirectoryName(filePath)!, $"{Path.GetFileNameWithoutExtension(filePath)}_{width}x{height}.jpg");
        
        // check if the resized image already exists
        if (File.Exists(resizedFilePath))
        {
            return resizedFilePath;
        }

        var info = new SKImageInfo(width, height);
        using var resized = original.Resize(info, SKSamplingOptions.Default);
        using var image = SKImage.FromBitmap(resized);
        using var data = image.Encode(SKEncodedImageFormat.Jpeg, 100);
        using var outputStream = File.Create(resizedFilePath);
        data.SaveTo(outputStream);
        return resizedFilePath;
    }
}