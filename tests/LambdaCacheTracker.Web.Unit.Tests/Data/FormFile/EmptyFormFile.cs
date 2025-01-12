using LambdaCacheTracker.Web.Unit.Tests.Data.FormFile.Abstractions;

namespace LambdaCacheTracker.Web.Unit.Tests.Data.FormFile;

/// <summary>
/// The empty form file.
/// </summary>
public class EmptyFormFile : BaseFormFile
{
    public override Stream OpenReadStream() => new MemoryStream();
}