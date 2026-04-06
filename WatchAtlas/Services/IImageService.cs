using Microsoft.AspNetCore.Components.Forms;
using WatchAtlas.Models.Images;

namespace WatchAtlas.Services;

public interface IImageService
{
    long MaxFileSizeBytes { get; }
    string AcceptedFileTypes { get; }
    string? NormalizeCoverImage(string? rawValue);
    bool IsValidExternalImageUrl(string? rawValue);
    bool IsValidStoredCoverImage(string? rawValue);
    Task<CoverImageProcessingResult> ProcessUploadedImageAsync(IBrowserFile file, CancellationToken cancellationToken = default);
}
