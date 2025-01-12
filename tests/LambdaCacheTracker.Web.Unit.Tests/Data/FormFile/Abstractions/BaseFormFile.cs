using Microsoft.AspNetCore.Http;

// ReSharper disable UnassignedGetOnlyAutoProperty
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace LambdaCacheTracker.Web.Unit.Tests.Data.FormFile.Abstractions;

/// <summary>
/// The base form file.
/// </summary>
public abstract class BaseFormFile : IFormFile
{
    public abstract Stream OpenReadStream();

    public void CopyTo(Stream target) => throw new NotImplementedException();

    public Task CopyToAsync(Stream target, CancellationToken cancellationToken = new CancellationToken()) => throw new NotImplementedException();

    public string ContentType { get; }
    
    public string ContentDisposition { get; }
    
    public IHeaderDictionary Headers { get; }
    
    public long Length { get; }
    
    public string Name { get; }
    
    public string FileName { get; }
}