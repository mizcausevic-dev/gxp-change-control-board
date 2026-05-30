using GxpChangeControlBoard.Api;
using Microsoft.AspNetCore.Mvc.Testing;

namespace GxpChangeControlBoard.Tests;

public sealed class GxpChangeControlBoardTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public GxpChangeControlBoardTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Overview_route_renders_change_control_shell()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/");
        var html = await response.Content.ReadAsStringAsync();

        Assert.True(response.IsSuccessStatusCode);
        Assert.Contains("GxP Change Control Board", html);
        Assert.Contains("change-control", html);
    }

    [Fact]
    public async Task Api_summary_returns_expected_counts()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/dashboard/summary");
        var json = await response.Content.ReadAsStringAsync();

        Assert.True(response.IsSuccessStatusCode);
        Assert.Contains("\"snapshots\":2", json);
        Assert.Contains("\"currentPackets\":1", json);
        Assert.Contains("\"blockingGaps\":4", json);
    }

    [Fact]
    public void Analysis_flags_high_risk_gxp_gaps()
    {
        var report = AnalysisService.Analyze(SampleData.Payload);

        Assert.Equal(2, report.Snapshots);
        Assert.Equal(6, report.Gaps);
        Assert.Contains(report.Findings, finding => finding.Code == "validation-evidence-gap");
        Assert.Contains(report.Findings, finding => finding.Code == "cutover-readiness-gap");
    }
}
