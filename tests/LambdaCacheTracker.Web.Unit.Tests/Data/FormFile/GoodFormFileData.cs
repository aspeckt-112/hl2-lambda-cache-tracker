using System.Text;
using LambdaCacheTracker.Web.Unit.Tests.Data.FormFile.Abstractions;

namespace LambdaCacheTracker.Web.Unit.Tests.Data.FormFile;

/// <summary>
/// The good form file data.
/// </summary>
public class GoodFormFileData : BaseFormFile
{
    public override Stream OpenReadStream()
    {
        const string data = """
                            "28"
                            {
                            	"id"		"86"
                            	"value"		"0"
                            	"data"		"0x00000008C2C5D3C1"
                            	"hud"		"0"
                            	"msg"		"0"
                            }
                            """;
        
        return new MemoryStream(Encoding.UTF8.GetBytes(data));
    }
}