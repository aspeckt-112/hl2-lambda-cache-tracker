using System.Text;
using LambdaCacheTracker.Web.Unit.Tests.Data.FormFile.Abstractions;

namespace LambdaCacheTracker.Web.Unit.Tests.Data.FormFile;

/// <summary>
/// The bad data form file.
/// </summary>
public class BadDataFormFile : BaseFormFile
{
    public override Stream OpenReadStream() => new MemoryStream(Encoding.UTF8.GetBytes("bad data pattern"));
}