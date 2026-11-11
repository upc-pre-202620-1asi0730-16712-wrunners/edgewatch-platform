namespace EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.ValueObjects;

/// <summary>Why an operator aborted a session (FLAME_OUT, POWDER_DEPLETED, MACHINE_FAULT, OPERATOR_DECISION, OTHER).</summary>
public enum AbortReason { FlameOut, PowderDepleted, MachineFault, OperatorDecision, Other }
