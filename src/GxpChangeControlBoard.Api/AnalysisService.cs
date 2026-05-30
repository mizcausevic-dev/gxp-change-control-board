namespace GxpChangeControlBoard.Api;

public static class AnalysisService
{
    public static GxpChangeControlReport Analyze(GxpChangeControlExport payload)
    {
        var findings = payload.Gaps
            .Select(gap => new GxpChangeControlFinding(
                GapCode(gap.ControlFamily),
                gap.Severity,
                gap.Subject,
                gap.ObservedState,
                OwnerForGap(gap.ControlFamily)))
            .ToList();

        var snapshots = payload.Snapshots.Count;
        var currentPackets = payload.Snapshots.Count(snapshot => snapshot.PacketStatus == "CURRENT");
        var gaps = payload.Gaps.Count;
        var blockingGaps = payload.Gaps.Count(gap => gap.BlocksRelease);
        var evidenceRisks = payload.Gaps.Count(gap => gap.ControlFamily is "Classification" or "Validation" or "Data Integrity");
        var releaseRisks = payload.Gaps.Count(gap => gap.ControlFamily is "Approval" or "Cutover" or "Training");
        var ok = blockingGaps == 0;

        return new GxpChangeControlReport(
            snapshots,
            currentPackets,
            gaps,
            blockingGaps,
            evidenceRisks,
            releaseRisks,
            findings,
            ok
        );
    }

    public static object Summary()
    {
        var report = Analyze(SampleData.Payload);
        return new
        {
            snapshots = report.Snapshots,
            currentPackets = report.CurrentPackets,
            gaps = report.Gaps,
            blockingGaps = report.BlockingGaps,
            evidenceRisks = report.EvidenceRisks,
            releaseRisks = report.ReleaseRisks,
            ok = report.Ok
        };
    }

    private static string GapCode(string family) => family switch
    {
        "Classification" => "change-classification-gap",
        "Validation" => "validation-evidence-gap",
        "Training" => "training-acknowledgment-gap",
        "Approval" => "approval-signature-gap",
        "Data Integrity" => "audit-trail-gap",
        "Cutover" => "cutover-readiness-gap",
        _ => "gxp-change-control-gap"
    };

    private static string OwnerForGap(string family) => family switch
    {
        "Classification" => "Quality Systems",
        "Validation" => "Validation Operations",
        "Training" => "Manufacturing Operations",
        "Approval" => "Change Review Board",
        "Data Integrity" => "Quality Systems",
        "Cutover" => "Technical Operations",
        _ => "Quality Systems"
    };
}
