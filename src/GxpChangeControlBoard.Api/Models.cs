namespace GxpChangeControlBoard.Api;

public sealed record ChangeSnapshot(
    string Id,
    string Name,
    string ChangeLane,
    string Site,
    string Status,
    string PacketStatus,
    string Owner,
    int OpenControls,
    int BlockingControls,
    DateTimeOffset CollectedAt
);

public sealed record ControlGap(
    string Id,
    string SnapshotId,
    string ControlFamily,
    string Severity,
    string Subject,
    string ExpectedState,
    string ObservedState,
    int HoursOpen,
    bool BlocksRelease
);

public sealed record ChangeLanePacket(
    string Id,
    string Lane,
    string Owner,
    string Status,
    string Focus,
    string NextAction,
    string Note
);

public sealed record ReleasePacket(
    string PacketId,
    string Lane,
    string Owner,
    string Status,
    int CompletenessScore,
    string Blocker,
    string DecisionNote,
    int ReviewWindowHours
);

public sealed record GxpChangeControlExport(
    IReadOnlyList<ChangeSnapshot> Snapshots,
    IReadOnlyList<ControlGap> Gaps
);

public sealed record GxpChangeControlFinding(
    string Code,
    string Severity,
    string Subject,
    string Message,
    string Owner
);

public sealed record GxpChangeControlReport(
    int Snapshots,
    int CurrentPackets,
    int Gaps,
    int BlockingGaps,
    int EvidenceRisks,
    int ReleaseRisks,
    IReadOnlyList<GxpChangeControlFinding> Findings,
    bool Ok
);
