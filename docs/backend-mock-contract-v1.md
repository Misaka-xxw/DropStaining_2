# Stainer Backend Mock Contract v1

Status: Frozen for instructions 00-18  
Scope: ASP.NET Core backend, EF Core/SQLite formal state, MachineExecutor, SignalR, audit, CommandId idempotency, and `IDeviceAdapter` Mock mode.  
Out of scope: final UI behavior and real hardware communication.

## 1. Contract rules

- Formal business state is stored in SQLite through backend services. `MockRuntimeStore` is not a business-state source.
- Every business write that exposes `commandId` is idempotent. Replaying the same operation and payload returns the stored response with `replayed=true`; reusing the ID with a different operation or payload returns `command_conflict`.
- Device actions controlled by MachineExecutor follow `Planned -> CommandSent -> DeviceAcknowledged -> Completed | Failed | Unknown`. ACK is not completion.
- A command without completion evidence becomes `Unknown`; loaded needles, uncertain reagent, DAB state, and resource leases are not silently cleared.
- Mock mode simulates behavior only. Real mode is fail-closed until a real adapter is installed and verified.
- Timestamps are UTC ISO-8601 values. Temperatures use deci-degrees Celsius; volumes use microlitres; coordinates use micrometres.
- Business errors use an HTTP 4xx/5xx status and a JSON object containing at least `code` and `message`.

## 2. Authentication and permissions

| Capability | Operator | Engineer | Admin | Additional guard |
| --- | --- | --- | --- | --- |
| Login, state, initialization state, preflight, run/history/alarm reads | Yes | Yes | Yes | Authenticated session |
| Create tasks and control normal runs | Yes | Yes | Yes | `CommandId`; valid initialization/preflight |
| Thermal/fluidics writes and Mock fault injection | No | Yes | Yes | Development environment or privileged role; Mock mode for Mock controls |
| DAB batch creation/state mutation | No | Yes | Yes | `CommandId`; valid DAB transition |
| Workflow/configuration writes | No | Yes | Yes | Engineering session where applicable |
| Coordinate/Liquid Class import, publish, activate, enable/disable | No | Yes | Yes | Secondary engineering authentication, reason/target, active-version protection |
| Diagnostics and CSV exports | No | Yes | Yes | Authenticated engineering/admin session; export is audited |
| Mock demo seed/reset | No | No | Yes | Development + Mock only; reset confirmation required |
| Device mode/database maintenance/user administration | No | Restricted | Yes | Endpoint-specific dangerous-operation guards |

Engineering writes are read-only by default while a run is active. An explicitly confirmed dangerous operation is required where the endpoint permits an override.

## 3. Primary APIs

### 3.1 Session and mode

| Method and route | Main request | Main response |
| --- | --- | --- |
| `POST /api/login` | `username`, `password`, `role` | authenticated user/session cookie |
| `POST /api/logout` | none | logout result |
| `GET /api/system/info` | none | system and selected device mode |
| `GET /api/device-mode` | none | persisted mode and restart requirement |
| `POST /api/device-mode/change` | `commandId`, target mode, reason | selected mode; restart required |

### 3.2 Demo, scan, LIS, and tasks

| Method and route | Main request | Main response |
| --- | --- | --- |
| `POST /api/mock-demo-data/seed` | `commandId` | created/updated/skipped counts |
| `POST /api/mock-demo-data/reset` | `commandId`, exact confirmation | deleted/skipped counts |
| `POST /api/samples/scan` | `commandId`, `count`, `scenario`, `rawCode`, `slotCode` | formal scan session and items |
| `POST /api/reagents/scan` | `commandId`, scope/position/scenario/barcodes | formal reagent scan session and confirmations |
| `POST /api/lis/mock-query` | `commandId`, `rawCode` | normalized code, LIS status, candidates, log ID |
| `POST /api/channel-batches/active` | `commandId`, `drawerCode` | active channel batch |
| `POST /api/channel-batches/workflow-selection` | `commandId`, batch/drawer, experiment type, workflow version | frozen workflow/coordinate/Liquid Class selection |
| `POST /api/tasks/he` | `commandId`, batch/drawer/slot, workflow version | confirmed HE task |
| `POST /api/tasks/ihc` | `commandId`, input mode, raw code, optional LIS log/selection, batch/drawer/slot | confirmed IHC task or selection requirement |

IHC input modes:

- `PrimaryAntibody`: accepts a Tongling three-digit code or named primary-antibody code.
- `HospitalBarcode`: requires a formal LIS query log. A single result is selected automatically; multiple results require `selectedPrimaryAntibodyCode` and compatibility validation.

### 3.3 Initialization and readiness

| Method and route | Main request | Main response |
| --- | --- | --- |
| `POST /api/device-initialization` | `commandId` | initialization run and module checks |
| `POST /api/device-initialization/{id}/retry` | `commandId`, retry reason | new initialization attempt linked to prior run |
| `GET /api/device-initialization` | none | latest formal initialization state |
| `GET /api/prehardware-readiness` | none | database/device/lease readiness |
| `GET /api/run/preflight` | none | `ok`, issues, task count, report ID, state hash |
| `POST /api/startup/recovery` | `commandId` | recovery counts and unresolved work |

Run start may include `preflightStateHash`. A stale or failed report is rejected with `preflight_invalid`.

### 3.4 Run lifecycle

| Method and route | Main request | Main response |
| --- | --- | --- |
| `POST /api/runs` | `commandId`, `stainingTaskIds`, optional preflight hash | created run ID/code/status |
| `GET /api/runs/{id}` | none | run, two-level workflow execution, slides, alarms |
| `POST /api/runs/{id}/start` | `commandId`, optional preflight hash | queued start command |
| `POST /api/runs/{id}/pause` | `commandId` | pause requested |
| `POST /api/runs/{id}/resume` | `commandId` | resume queued |
| `POST /api/runs/{id}/stop` | `commandId` | ordinary stop requested |
| `POST /api/runs/{id}/fault` | `commandId`, message | injected run fault requested |
| `POST /api/runs/{id}/redo-current-major-step` | `commandId`, reason | redo queued after readiness checks |

Run creation creates bottle-specific reservations from actual workflow steps and slide count when matching rack bottles exist. Reservations may span bottles. If no matching bottle exists yet, a source-unassigned reservation preserves run configuration while preflight remains responsible for blocking execution. MachineExecutor consumes assigned reservations once, writes `ReagentConsumption` and `DispenseExecution`, and prevents duplicate inventory deduction by `DeviceCommandExecutionId`.

### 3.5 Thermal, fluidics, motion, and DAB

| Area | Read API | Write APIs |
| --- | --- | --- |
| Thermal | `GET /api/thermal/state`, `/telemetry` | points, boards, cooling, faults, clear faults |
| Fluidics | `GET /api/fluidics/state`, `/telemetry` | pump run/stop, inner/outer wash, mixer start/complete/stop, liquid level, faults |
| Device | `GET /api/device/state` | Mock faults and fault clear |
| DAB | positions and batch detail | batch create, preparation start, expire, fail, cleaning start/confirm |
| Motion | exposed through MachineExecutor and diagnostics | home/move/detect/aspirate/dispense/blowout/wash through adapter calls |

PWM mapping is fixed: `PWM0:A`, `PWM1:B`, `PWM2:C`, `PWM3:D`. Speed is `-100..100`; sign defines direction. `WashInner*` and `WashOuter*` are distinct target points. A generic workflow `Wash` does not impersonate an inner/outer wash target.

DAB preparation completion and consumption cannot be posted directly; they require a completed MachineExecutor device command. Formula constants are A:B:water = `1:1:18`, 200 uL per slide, 400 uL line reserve, validity 3 hours.

### 3.6 Traceability and diagnostics

| Method and route | Result |
| --- | --- |
| `GET /api/history/runs[/{id}]` | run, channel, slide, workflow, command history |
| `GET /api/history/reagent-consumptions` | bottle/DAB-linked consumptions |
| `GET /api/alarms` | active and historical alarms |
| `GET /api/audit/logs` | user/config/run/device audit |
| `GET /api/history/export/runs` | audited CSV |
| `GET /api/history/export/reagent-consumptions` | audited CSV |
| `GET /api/alarms/export` | audited CSV |
| `GET /api/audit/export` | audited CSV |
| `GET /api/engineering/diagnostics/device-state` | formal thermal/fluidics/motion read models |
| `GET /api/engineering/diagnostics/command-log[.csv]` | command lifecycle and context |
| `GET /api/engineering/diagnostics/errors` | command/init/alarm/communication errors |
| `GET /api/engineering/diagnostics/mock-communications[.csv]` | request/response plus persistence completeness |

Communication persistence status is `Pending`, `Complete`, or `Failed`. SQLite lock must leave a queryable pending/failed marker; it must not cause a device command replay.

## 4. Representative payloads

Create and start a run:

```json
POST /api/runs
{
  "commandId": "cmd-run-create-001",
  "stainingTaskIds": ["task-he", "task-ihc"],
  "preflightStateHash": "optional-current-hash"
}
```

```json
{
  "ok": true,
  "commandId": "cmd-run-create-001",
  "replayed": false,
  "runId": "...",
  "runCode": "RUN-...",
  "status": "Created",
  "message": "Run created."
}
```

Hospital LIS query:

```json
POST /api/lis/mock-query
{
  "commandId": "cmd-lis-001",
  "rawCode": "HOSP-MOCK-MULTI"
}
```

The response includes `lisQueryLogId`, `status`, `normalizedCode`, `candidatePrimaryAntibodyCodes`, error fields, and message.

Device operation results returned by adapters contain:

```json
{
  "ok": true,
  "status": "Succeeded",
  "moduleCode": "pipette",
  "action": "Dispense",
  "errorCode": null,
  "message": "...",
  "startedAtUtc": "...",
  "completedAtUtc": "...",
  "acknowledged": true,
  "data": {}
}
```

## 5. Frozen status values

| Domain | Values |
| --- | --- |
| Run/step | `Created`, `Pending`, `Running`, `Completed`, `Paused`, `Stopped`, `Failed`, `Unknown`, `Faulted`, `WaitingUnload` |
| Device command ledger | `Planned`, `CommandSent`, `DeviceAcknowledged`, `Completed`, `Failed`, `Unknown` (`Acknowledged` is legacy-compatible only) |
| Adapter result | `Succeeded`, `Failed`, `TimedOut`, `Unknown`, `NotSupported` |
| Connection | `Unknown`, `Connected`, `Disconnected`, `Faulted` |
| Workflow selection | `Unselected`, `Selected`, `Locked`, `NeedsManualResolution` |
| Reservation | `Reserved`, `Consumed`, `Released`, `NeedsManualResolution` |
| DAB batch | `PendingPreparation`, `Preparing`, `Available`, `Depleted`, `Expired`, `AwaitingCleaning`, `Cleaned`, `Failed`, `Unknown`, `LegacyUnverified` |
| DAB cleaning | `NotRequired`, `Required`, `Confirmed`, `NeedsManualResolution` |
| Thermal | `Off`, `Heating`, `Cooling`, `Returning`, `Stable`, `Faulted`, `Unknown` |
| Fluidics | `Idle`, `Running`, `Completed`, `Stopped`, `TimedOut`, `Faulted`, `Disconnected`, `Unknown` |
| Liquid level | `Normal`, `Low`, `Empty`, `Full`, `SensorFault`, `Disconnected` |
| Motion | `Idle`, `Homing`, `Moving`, `Aspirating`, `Dispensing`, `BlowingOut`, `Washing`, `Completed`, `Faulted`, `TimedOut`, `Unknown`, `Disconnected` |
| Pipette mode | `Single`, `Synchronized`, `Sequential` |
| Resource lease | `Waiting`, `Acquired`, `Released`, `NeedsManualResolution` |
| LIS | `Running`, `SingleCandidate`, `MultipleCandidates`, `NoResult`, `TimedOut`, `Failed`, `Selected`, `CompatibilityFailed` |
| Sample scan | `VALID`, `EMPTY`, `INVALID`, `TIMED_OUT`, `DEVICE_DISCONNECTED`, `FAILED` |
| Reagent scan | `VALID`, `EMPTY`, `INVALID` |
| Communication persistence | `Pending`, `Complete`, `Failed` |

## 6. Error-code contract

Important stable codes include:

| Area | Codes |
| --- | --- |
| Idempotency | `command_id_required`, `command_conflict`, `command_replay_failed` |
| Authentication/engineering | `authentication_required`, `forbidden`, `engineering_session_required`, `engineering_session_expired`, `dangerous_confirmation_required` |
| Run/preflight | `tasks_required`, `active_run_exists`, `task_not_found`, `task_not_confirmed`, `preflight_invalid`, `channel_batch_locked`, `channel_batch_incomplete`, `coordinate_version_mismatch` |
| Workflow/config | `channel_workflow_required`, `channel_workflow_mismatch`, `channel_batch_needs_manual_resolution`, `coordinate_active_version_conflict`, `liquid_class_enabled_version_conflict` |
| Reagent | `reagent_insufficient`, `reagent_reservation_insufficient`, `reagent_depleted`, `reagent_position_not_found` |
| LIS/task | `lis_result_not_found`, `lis_selection_required`, `ihc_channel_workflow_incompatible`, timeout maps to HTTP 504 |
| DAB | `dab_task_not_found`, `dab_task_incompatible`, `dab_source_bottle_unavailable`, `dab_batch_not_prepared`, `dab_batch_unavailable`, `dab_batch_not_expired`, `dab_preparation_failed`, `dab_preparation_unknown`, `dab_mix_area_cleaning_required` |
| Thermal/fluidics | `thermal_not_ready`, `pump_not_ready`, `mixer_not_ready`, `mixer_prerequisite_not_met`, `wash_target_required`, `wash_target_not_found`, liquid source/status validation codes |
| Motion/resource | `target_point_not_found`, `soft_limit_exceeded`, `needle_reagent_change_requires_wash`, `dual_needle_geometry_invalid`, `resource_waiting`, `device_command_unknown` |
| Real adapter | `real_device_adapter_unavailable`, `device_mode_restart_required`, `mock_device_mode_required` |

Fault and timeout details remain in `errorCode`, command result JSON, alarm, communication record, and audit context.

## 7. SignalR contract

Hub: `/hubs/machine`  
Client target: `machineEvent`

Envelope:

```json
{
  "eventId": "...",
  "type": "workflowStep.completed",
  "occurredAtUtc": "...",
  "runId": "...",
  "entityType": "WorkflowStepExecution",
  "entityId": "...",
  "requiredRole": null,
  "payload": {}
}
```

Frozen event types:

- `machine.stateChanged`, `channelBatch.changed`, `slideTask.created`, `slideTask.stateChanged`
- `workflowStep.started`, `workflowStep.completed`, `workflow.changed`, `workflowVersion.changed`, `workflowStep.changed`, `workflowReagentRequirement.changed`, `primaryAntibodyMapping.changed`
- `temperature.changed`, `cooling.changed`, `pump.changed`, `mixer.changed`, `liquidLevel.changed`
- `reagent.changed`, `reagentBottle.changed`, `reagent.bottleDepleted`, `dab.batchChanged`
- `alarm.raised`, `alarm.acknowledged`
- `device.connectionChanged`, `device.stateChanged`, `device.initializationChanged`
- `qr.scanCompleted`, `scanSession.changed`

SignalR is a projection channel, not a state store. Clients recover authoritative state through REST/SQLite-backed read APIs after reconnect.

## 8. Mock and Real adapter boundary

### Backend/MachineExecutor owns

- CommandId idempotency, command ledger lifecycle, transaction boundaries, communication persistence, telemetry, audit, alarms, and SignalR publication.
- Workflow scheduling, resource leases, needle/reagent safety, inventory reservation/consumption, DAB lifecycle, timeout/Unknown policy, pause/stop/recovery behavior.
- Frozen coordinate and Liquid Class snapshots and named target-point validation.

### `IDeviceAdapter` owns

- Translating a structured `DeviceOperationRequest` into one device operation.
- Returning a structured command/response/error result with timestamps and acknowledgement evidence.
- No direct write to business tables or communication-record tables.

### Mock adapter owns

- Deterministic simulated device behavior and explicit fault injection.
- No authoritative business state and no hidden completion assumption.

### Future Real adapter must own

- Real protocol framing, connection/session management, ACK/completion parsing, device timestamps/error mapping, and correlation to the backend command ID.
- Fail closed when unsupported, disconnected, ambiguous, or missing completion evidence.

## 9. Automated acceptance matrix

| Scope | Stable automated coverage |
| --- | --- |
| Demo HE+IHC, Tongling, hospital single/multi, cross-bottle reservation/consumption, peripheral mocks, pause/resume, DAB expiry/cleaning, history/audit/CSV | `MockBackendEndToEndAcceptanceTests` |
| DAB preparation, cross-bottle components, failures, Unknown, expiry/repreparation | `RuntimeLedgerExecutorTests`, `DabLifecycleTests` |
| Fault, redo, no duplicate completed action | `RuntimeLedgerExecutorTests.Pause_resume_fault_and_redo_complete_without_repeating_completed_actions` |
| SignalR thermal/workflow/alarm events | `RuntimeLedgerExecutorTests.Machine_hub_pushes_temperature_step_and_alarm_events` |
| Pump/mixer/liquid states and fault recovery | `FluidicsControlMockTests` |
| Thermal/cooling transition and recovery | `ThermalControlMockTests` |
| Robot, dual needles, geometry, Unknown, resource waiting, pause/stop/restart | `MotionControlMockTests` |
| Restart recovery and uncertain command preservation | `PreHardwareSafetyTests`, `MotionControlMockTests` |
| Scan/LIS/demo permissions and compatibility | `MockScannerLisDemoTests` |
| Alarm/history/audit/CSV filtering | `TraceabilityApiIntegrationTests` |
| Engineering session/config/diagnostic contract | `EngineeringBackendConfigManagementTests` |
| SQLite communication lock and pending trace | `DeviceCommunicationPersistenceTests` |

## 10. Real hardware work not completed

The following are explicitly not delivered by Mock acceptance:

1. Main controller real communication and firmware command set.
2. SOCON real motion/servo communication and homing validation.
3. Leuze DCR55 real scanner protocol, serial/network transport, framing, retry, and field integration.
4. Real cooling controller communication and thermal tuning.
5. Production LIS/HIS transport, authentication, mapping, timeout/retry, and privacy controls.
6. On-site coordinate calibration, metrology, fixture variation, and verification records.
7. Physical safety interlocks, covers/doors, collision detection, emergency stop, power-loss behavior, and hardware watchdogs.
8. Wet-lab validated Liquid Class, pump curves, aspiration/dispense timing, DAB chemistry, wash/mix parameters, temperature curves, and reagent carryover limits.

No Real mode acceptance may infer safety or correctness from Mock results. Real mode remains fail-closed until each boundary above has an installed adapter, protocol evidence, calibration/validation record, and hardware acceptance test.
