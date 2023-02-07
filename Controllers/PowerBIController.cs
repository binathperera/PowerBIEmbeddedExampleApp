using BlazorAppPowerBI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Rest;
using BlazorAppPowerBI.Shared;

namespace BlazorAppPowerBI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PowerBIController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public PowerBIController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<ActionResult<EmbeddedReportViewModel>> GetReportEmbedding()
    {
        var tenantId = _configuration["AzureAppInfo:TenantId"];
        var clientId = _configuration["AzureAppInfo:ClientId"];
        var clientSecret = _configuration["AzureAppInfo:ClientSecret"];
        var authorityUri = new Uri($"https://login.microsoftonline.com/{tenantId}");

        var app = ConfidentialClientApplicationBuilder
                    .Create(clientId)
                    .WithClientSecret(clientSecret)
                    .WithAuthority(authorityUri)
                    .Build();

        var powerbiApiDefaultScope = "https://analysis.windows.net/powerbi/api/.default";
        var scopes = new string[] { powerbiApiDefaultScope };

        try
        {
            var authResult = await app.AcquireTokenForClient(scopes).ExecuteAsync();
            Console.Write(authResult.AccessToken);
            var tokenCredentials = new TokenCredentials(authResult.AccessToken, "Bearer");
            var urlPowerBiServiceApiRoot = "https://api.powerbi.com/";
            var pbiClient = new PowerBIClient(new Uri(urlPowerBiServiceApiRoot), tokenCredentials);

            var workspaceId = new Guid("f9725b96-99d5-4c52-b336-9b064c07f05f");
            var reportId = new Guid("7acccdc8-d5a4-4d7b-86bd-fd16fe4c82b7");
            var report = pbiClient.Reports.GetReportInGroup(workspaceId, reportId);

            var tokenRequest = new GenerateTokenRequest(TokenAccessLevel.View, report.DatasetId);
            var embedTokenResponse = await pbiClient.Reports.GenerateTokenAsync(workspaceId, reportId, tokenRequest);

            var reportViewModel = new EmbeddedReportViewModel(
                    report.Id.ToString(),
                    report.Name,
                    report.EmbedUrl,
                    embedTokenResponse.Token
                );

            return Ok(reportViewModel);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}

