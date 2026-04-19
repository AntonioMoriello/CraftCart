using Microsoft.Maui.Storage;

namespace CraftCart.Services;

public static class ImageService
{
    public static async Task<string> PickAndSaveImage()
    {
        var customFileType = new FilePickerFileType(
            new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] { ".png", ".jpg", ".jpeg" } },
                { DevicePlatform.iOS, new[] { "public.image" } },
                { DevicePlatform.Android, new[] { "image/png", "image/jpeg" } },
                { DevicePlatform.MacCatalyst, new[] { "public.image" } }
            });

        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            FileTypes = customFileType,
            PickerTitle = "Select a product image"
        });

        if (result == null)
            return string.Empty;

        string folder = Path.Combine(FileSystem.AppDataDirectory, "ProductImages");
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        string ext = Path.GetExtension(result.FileName);
        string fileName = Guid.NewGuid().ToString("N") + ext;
        string destination = Path.Combine(folder, fileName);

        using (var sourceStream = await result.OpenReadAsync())
        using (var destStream = File.Create(destination))
        {
            await sourceStream.CopyToAsync(destStream);
        }

        return destination;
    }
}
