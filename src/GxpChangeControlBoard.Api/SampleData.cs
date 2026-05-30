namespace GxpChangeControlBoard.Api;

public static class SampleData
{
    public static readonly GxpChangeControlExport Payload = new(
        Snapshots:
        [
            new(
                "chg-core",
                "Manufacturing suite change packet",
                "Classification and validation lane",
                "BOS-GXP-1",
                "WATCH",
                "CURRENT",
                "Quality Systems",
                7,
                3,
                DateTimeOffset.Parse("2026-05-30T13:20:00Z")
            ),
            new(
                "chg-release",
                "Packaging line cutover packet",
                "Approval and cutover lane",
                "RTP-GXP-3",
                "CRITICAL",
                "STALE",
                "Change Review Board",
                5,
                2,
                DateTimeOffset.Parse("2026-05-28T09:10:00Z")
            )
        ],
        Gaps:
        [
            new(
                "gap-classification",
                "chg-core",
                "Classification",
                "high",
                "Change classification packet",
                "Major and minor change classes keep approved risk logic and linked rationale before validation starts.",
                "The packet still lacks the signed rationale explaining why one automation edit was classified as minor.",
                19,
                true
            ),
            new(
                "gap-validation",
                "chg-core",
                "Validation",
                "high",
                "Validation protocol evidence",
                "The validation lane keeps executed protocol evidence and variance disposition before release review.",
                "Executed protocol evidence is incomplete and one variance note is still unsigned.",
                28,
                true
            ),
            new(
                "gap-training",
                "chg-release",
                "Training",
                "medium",
                "SOP acknowledgment set",
                "Operators acknowledge the active SOP revision before the cutover packet clears handoff.",
                "Two operators still show stale training acknowledgment timestamps for the active SOP revision.",
                15,
                false
            ),
            new(
                "gap-approval",
                "chg-release",
                "Approval",
                "high",
                "Electronic approval packet",
                "Board approval packets keep complete e-signature continuity before cutover windows open.",
                "One final approver signature is missing from the cutover packet and blocks the board from clearing release.",
                21,
                true
            ),
            new(
                "gap-audit",
                "chg-core",
                "Data Integrity",
                "medium",
                "Audit-trail continuity export",
                "The audit trail stays continuous across edit, review, approval, and release events.",
                "The export omitted one intermediate review event and needs regeneration before archive.",
                11,
                false
            ),
            new(
                "gap-cutover",
                "chg-release",
                "Cutover",
                "high",
                "Rollback and restart readiness",
                "Cutover packets keep rollback timing and restart ownership visible before the window opens.",
                "Rollback timing is missing for one packaging-line restart step and the owner handoff is unclear.",
                17,
                true
            )
        ]
    );

    public static readonly IReadOnlyList<ChangeLanePacket> ChangeBoard =
    [
        new(
            "classification-lane",
            "Classification and scope lane",
            "Quality Systems",
            "red",
            "Change class rationale, impact scope, and linked downstream dependencies",
            "Close the unsigned rationale before another validation artifact inherits the wrong scope.",
            "The classification packet is not strong enough to defend the current change path."
        ),
        new(
            "validation-lane",
            "Validation and evidence lane",
            "Validation Operations",
            "red",
            "Protocol execution, variance handling, and evidence completeness",
            "Close the missing protocol evidence and signed variance note before the next board checkpoint.",
            "Validation posture is currently the biggest release blocker."
        ),
        new(
            "training-lane",
            "Training and SOP lane",
            "Manufacturing Operations",
            "yellow",
            "Operator acknowledgment freshness and controlled cutover timing",
            "Refresh the stale SOP acknowledgments before the packaging line enters the cutover window.",
            "Training posture is recoverable if the acknowledgments land in the next review cycle."
        ),
        new(
            "approval-lane",
            "Approval and cutover lane",
            "Change Review Board",
            "red",
            "E-signature continuity, rollback readiness, and named release accountability",
            "Close the missing approval signature and rollback handoff before opening the cutover window.",
            "The final board packet is not yet signoff-safe."
        )
    ];

    public static readonly IReadOnlyList<ReleasePacket> ReleasePackets =
    [
        new(
            "GXPCB-12",
            "Classification packet",
            "Quality Systems",
            "red",
            58,
            "Signed rationale for one minor-classification branch is still missing.",
            "Do not let validation inherit a weak classification decision.",
            8
        ),
        new(
            "GXPCB-18",
            "Validation packet",
            "Validation Operations",
            "red",
            61,
            "Executed protocol evidence and the signed variance note are both incomplete.",
            "Block release review until validation evidence is complete and variance closure is explicit.",
            10
        ),
        new(
            "GXPCB-24",
            "Training packet",
            "Manufacturing Operations",
            "yellow",
            79,
            "Two operator acknowledgments are stale against the current SOP revision.",
            "The lane can recover if the acknowledgments land before the next board cycle.",
            13
        ),
        new(
            "GXPCB-31",
            "Cutover packet",
            "Change Review Board",
            "yellow",
            73,
            "The packet still needs the final approval signature and rollback timing confirmation.",
            "Release posture can recover if the final signoff and restart ownership close in the next window.",
            16
        )
    ];
}
