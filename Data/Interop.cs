using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorAppPowerBI.Data
{
    public static class Interop
    {
        internal static ValueTask<object> CreateReport(
            IJSRuntime jsRuntime,
            ElementReference reportContainer,
            string accessToken,
            string embedUrl,
            string embedReportId)
        {
            return jsRuntime.InvokeAsync<object>(
                "ShowMyPowerBI.showReport",
                reportContainer, accessToken, embedUrl,
                embedReportId);
        }
    }
}
