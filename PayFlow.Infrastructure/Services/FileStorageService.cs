namespace PayFlow.Infrastructure.Services;

public class FileStorageService
{
    private readonly string _baseUploadPath;

    public FileStorageService()
    {
        // Files saved in wwwroot/uploads folder
        _baseUploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        Directory.CreateDirectory(Path.Combine(_baseUploadPath, "employees"));
        Directory.CreateDirectory(Path.Combine(_baseUploadPath, "logos"));
    }

    public async Task<string> SaveEmployeePhotoAsync(int employeeId, Stream fileStream, string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLower();
        var uniqueName = $"emp_{employeeId}_{DateTime.UtcNow.Ticks}{extension}";
        var filePath = Path.Combine(_baseUploadPath, "employees", uniqueName);

        using var fs = new FileStream(filePath, FileMode.Create);
        await fileStream.CopyToAsync(fs);

        return $"/uploads/employees/{uniqueName}";
    }

    public async Task<string> SaveLogoAsync(Stream fileStream, string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLower();
        var uniqueName = $"logo_{DateTime.UtcNow.Ticks}{extension}";
        var filePath = Path.Combine(_baseUploadPath, "logos", uniqueName);

        using var fs = new FileStream(filePath, FileMode.Create);
        await fileStream.CopyToAsync(fs);

        return $"/uploads/logos/{uniqueName}";
    }

    public void DeleteFile(string? filePath)
    {
        if (string.IsNullOrEmpty(filePath)) return;
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath.TrimStart('/'));
        if (File.Exists(fullPath)) File.Delete(fullPath);
    }
}
