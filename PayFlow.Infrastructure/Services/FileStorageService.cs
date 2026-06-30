using Microsoft.Extensions.Configuration;
using System.Text.Json;

public class FileStorageService
{
    private readonly string _supabaseUrl;
    private readonly string _supabaseKey;
    private readonly HttpClient _httpClient;

    public FileStorageService(IConfiguration config)
    {
        _supabaseUrl = config["Supabase:Url"]!;
        _supabaseKey = config["Supabase:Key"]!;
        _httpClient = new HttpClient();
    }

    private async Task<string> UploadFileAsync(string bucket, Stream fileStream, string fileName)
    {
        using var ms = new MemoryStream();
        await fileStream.CopyToAsync(ms);
        var bytes = ms.ToArray();

        // Correct format: /storage/v1/object/{bucket}/{filePath}
        var url = $"{_supabaseUrl}/storage/v1/object/{bucket}/{fileName}";

        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _supabaseKey);
        request.Headers.Add("apikey", _supabaseKey);

        var content = new ByteArrayContent(bytes);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
        request.Content = content;

        var response = await _httpClient.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Status: {response.StatusCode} | URL: {url} | Body: {responseBody}");
        }

        return $"{_supabaseUrl}/storage/v1/object/public/{bucket}/{fileName}";
    }

    public async Task<string> SaveEmployeePhotoAsync(int employeeId, Stream fileStream, string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLower();
        var uniqueName = $"emp_{employeeId}_{DateTime.UtcNow.Ticks}{extension}";
        return await UploadFileAsync("employeephotos", fileStream, uniqueName);
    }

    public async Task<string> SaveLogoAsync(Stream fileStream, string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLower();
        var uniqueName = $"logo_{DateTime.UtcNow.Ticks}{extension}";
        return await UploadFileAsync("branchlogo", fileStream, uniqueName);
    }

    public void DeleteFile(string? filePath) { }
}
