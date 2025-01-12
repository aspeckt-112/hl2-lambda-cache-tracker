using LambdaCacheTracker.Web.Unit.Tests.Data.FormFile.Abstractions;

namespace LambdaCacheTracker.Web.Unit.Tests.Data.FormFile;

public class ExceptionFormFile : BaseFormFile
{
    public override Stream OpenReadStream() => throw new Exception("I'MA EXCEPTION. I'MA GONNA THROW!");
}