using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PatchingController : ControllerBase
{
    private readonly PatchService _patchService = new PatchService();

    [HttpPost("last5")]
    public IActionResult GetLast5Patches([FromBody] string server)
    {
        return Ok(_patchService.GetLast5Patches(server));
    }

    [HttpPost("check")]
    public IActionResult CheckPatch([FromBody] PatchCheckRequest request)
    {
        var results = new List<PatchResult>();
        results.Add(_patchService.CheckPatch(request.Server, request.KBNumber));
       

        return Ok(results);
    }
}

public class PatchCheckRequest
{
    public string Server { get; set; }
    public string KBNumber { get; set; }
}
