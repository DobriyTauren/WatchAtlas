using Microsoft.AspNetCore.Components.Forms;
using WatchAtlas.Helpers;
using WatchAtlas.Models.Images;

namespace WatchAtlas.Services;

public class ImageService : IImageService
{
    private static readonly HashSet<string> SupportedContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg",
        "image/png",
        "image/webp",
        "image/gif"
    };

    public long MaxFileSizeBytes => 2 * 1024 * 1024;

    public string AcceptedFileTypes => string.Join(",", SupportedContentTypes);

    public string? NormalizeCoverImage(string? rawValue)
        => CoverImageValueHelper.Normalize(rawValue);

    public bool IsValidExternalImageUrl(string? rawValue)
        => CoverImageValueHelper.IsExternalUrl(rawValue);

    public bool IsValidStoredCoverImage(string? rawValue)
        => CoverImageValueHelper.IsValidStoredValue(rawValue);

    public async Task<CoverImageProcessingResult> ProcessUploadedImageAsync(IBrowserFile file, CancellationToken cancellationToken = default)
    {
        if (file.Size > MaxFileSizeBytes)
        {
            return new CoverImageProcessingResult
            {
                ErrorMessage = $"The selected file is too large. Please choose an image up to {MaxFileSizeBytes / 1024 / 1024} MB."
            };
        }

        if (!SupportedContentTypes.Contains(file.ContentType))
        {
            return new CoverImageProcessingResult
            {
                ErrorMessage = "Unsupported image type. Please use JPG, PNG, WEBP, or GIF."
            };
        }

        try
        {
            await using var stream = file.OpenReadStream(MaxFileSizeBytes, cancellationToken);
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream, cancellationToken);

            var base64 = Convert.ToBase64String(memoryStream.ToArray());

            return new CoverImageProcessingResult
            {
                IsSuccess = true,
                ImageDataUrl = $"data:{file.ContentType};base64,{base64}"
            };
        }
        catch (Exception exception)
        {
            return new CoverImageProcessingResult
            {
                ErrorMessage = $"The image could not be processed: {exception.Message}"
            };
        }
    }
}
