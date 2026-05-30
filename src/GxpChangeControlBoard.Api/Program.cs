using System.Text.Json;
using GxpChangeControlBoard.Api;

var app = GxpChangeControlBoardApplication.BuildApp(args);

if (args.Contains("--prerender"))
{
    await SiteBuilder.WriteAsync();
    return;
}

if (args.Contains("--demo"))
{
    Console.WriteLine(JsonSerializer.Serialize(AnalysisService.Summary(), new JsonSerializerOptions { WriteIndented = true }));
    Console.WriteLine(JsonSerializer.Serialize(SampleData.ChangeBoard, new JsonSerializerOptions { WriteIndented = true }));
    return;
}

app.Run();

public partial class Program;

public static class GxpChangeControlBoardApplication
{
    public static WebApplication BuildApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.MapGet("/", () => Results.Content(RenderService.Overview(), "text/html"));
        app.MapGet("/change-board", () => Results.Content(RenderService.ChangeBoard(), "text/html"));
        app.MapGet("/control-gaps", () => Results.Content(RenderService.ControlGaps(), "text/html"));
        app.MapGet("/release-posture", () => Results.Content(RenderService.ReleasePosture(), "text/html"));
        app.MapGet("/verification", () => Results.Content(RenderService.Verification(), "text/html"));
        app.MapGet("/docs", () => Results.Content(RenderService.Docs(), "text/html"));

        app.MapGet("/api/dashboard/summary", () => Results.Json(AnalysisService.Summary()));
        app.MapGet("/api/change-board", () => Results.Json(SampleData.ChangeBoard));
        app.MapGet("/api/control-gaps", () => Results.Json(SampleData.Payload.Gaps));
        app.MapGet("/api/release-posture", () => Results.Json(SampleData.ReleasePackets));
        app.MapGet("/api/verification", () => Results.Json(new[]
        {
            "Synthetic GxP change-control evidence only; no patient, manufacturing, or proprietary biotech tenant data is published.",
            "Classification, validation, training, approval continuity, data integrity, and cutover readiness are modeled as operator surfaces.",
            "This repo demonstrates biotech quality workflow depth without claiming GMP, GxP, FDA, or clinical compliance."
        }));
        app.MapGet("/api/sample", () => Results.Text(RenderService.Sample(), "application/json"));

        return app;
    }
}

public static class SiteBuilder
{
    public static async Task WriteAsync()
    {
        var root = FindRepoRoot();
        var siteDir = Path.Combine(root, "site");
        Directory.CreateDirectory(siteDir);

        var pages = new Dictionary<string, string>
        {
            ["index.html"] = RenderService.Overview(),
            [Path.Combine("change-board", "index.html")] = RenderService.ChangeBoard(),
            [Path.Combine("control-gaps", "index.html")] = RenderService.ControlGaps(),
            [Path.Combine("release-posture", "index.html")] = RenderService.ReleasePosture(),
            [Path.Combine("verification", "index.html")] = RenderService.Verification(),
            [Path.Combine("docs", "index.html")] = RenderService.Docs()
        };

        foreach (var (relative, html) in pages)
        {
            var target = Path.Combine(siteDir, relative);
            Directory.CreateDirectory(Path.GetDirectoryName(target)!);
            await File.WriteAllTextAsync(target, html);
        }

        var apiDir = Path.Combine(siteDir, "api");
        Directory.CreateDirectory(Path.Combine(apiDir, "dashboard"));
        await File.WriteAllTextAsync(Path.Combine(apiDir, "dashboard", "summary.json"), JsonSerializer.Serialize(AnalysisService.Summary(), new JsonSerializerOptions { WriteIndented = true }));
        await File.WriteAllTextAsync(Path.Combine(apiDir, "change-board.json"), JsonSerializer.Serialize(SampleData.ChangeBoard, new JsonSerializerOptions { WriteIndented = true }));
        await File.WriteAllTextAsync(Path.Combine(apiDir, "control-gaps.json"), JsonSerializer.Serialize(SampleData.Payload.Gaps, new JsonSerializerOptions { WriteIndented = true }));
        await File.WriteAllTextAsync(Path.Combine(apiDir, "release-posture.json"), JsonSerializer.Serialize(SampleData.ReleasePackets, new JsonSerializerOptions { WriteIndented = true }));
        await File.WriteAllTextAsync(Path.Combine(apiDir, "verification.json"), JsonSerializer.Serialize(new[]
        {
            "Synthetic GxP change-control evidence only; no patient, manufacturing, or proprietary biotech tenant data is published.",
            "Classification, validation, training, approval continuity, data integrity, and cutover readiness are modeled as operator surfaces.",
            "This repo demonstrates biotech quality workflow depth without claiming GMP, GxP, FDA, or clinical compliance."
        }, new JsonSerializerOptions { WriteIndented = true }));
        await File.WriteAllTextAsync(Path.Combine(apiDir, "sample.json"), RenderService.Sample());

        const string domain = "gxp.kineticgain.com";
        await File.WriteAllTextAsync(
            Path.Combine(siteDir, "robots.txt"),
            $"User-agent: *{Environment.NewLine}Allow: /{Environment.NewLine}Sitemap: https://{domain}/sitemap.xml{Environment.NewLine}");
        await File.WriteAllTextAsync(Path.Combine(siteDir, "sitemap.xml"), """
<?xml version="1.0" encoding="UTF-8"?>
<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
  <url><loc>https://gxp.kineticgain.com/</loc></url>
  <url><loc>https://gxp.kineticgain.com/change-board/</loc></url>
  <url><loc>https://gxp.kineticgain.com/control-gaps/</loc></url>
  <url><loc>https://gxp.kineticgain.com/release-posture/</loc></url>
  <url><loc>https://gxp.kineticgain.com/verification/</loc></url>
  <url><loc>https://gxp.kineticgain.com/docs/</loc></url>
</urlset>
""");
        await File.WriteAllTextAsync(Path.Combine(siteDir, "CNAME"), domain + Environment.NewLine);
    }

    private static string FindRepoRoot()
    {
        var current = AppContext.BaseDirectory;
        for (var i = 0; i < 8; i++)
        {
            if (File.Exists(Path.Combine(current, "gxp-change-control-board.sln")))
            {
                return current;
            }

            current = Directory.GetParent(current)?.FullName
                ?? throw new DirectoryNotFoundException("Unable to resolve repo root.");
        }

        throw new DirectoryNotFoundException("Unable to resolve repo root.");
    }
}
