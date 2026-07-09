namespace Stainer.Web.Domain.Entities;

public sealed class ScannerProfile
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string ScannerType { get; set; } = ScannerTypes.Dcr55;
    public bool Enabled { get; set; } = true;
    public string? Port { get; set; }
    public int? BaudRate { get; set; }
    public int? TimeoutMilliseconds { get; set; }
    public string TriggerMode { get; set; } = ScannerTriggerModes.Software;
    public int? RoiX { get; set; }
    public int? RoiY { get; set; }
    public int? RoiWidth { get; set; }
    public int? RoiHeight { get; set; }
    public bool? CheckLightEnabled { get; set; }
    public string SpecialParametersJson { get; set; } = "{}";
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAtUtc { get; set; }

    public ICollection<ScannerRegion> Regions { get; } = new List<ScannerRegion>();
}

public sealed class ScannerRegion
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public int RegionNo { get; set; }
    public string Name { get; set; } = string.Empty;
    public string RegionType { get; set; } = ScannerRegionTypes.Sample;
    public string ScannerProfileId { get; set; } = string.Empty;
    public int ScanOrder { get; set; }
    public string ScanPathJson { get; set; } = "[]";
    public string? CoordinateProfileId { get; set; }
    public string? CoordinateProfileVersionId { get; set; }
    public string CoordinatePointCodesJson { get; set; } = "[]";
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAtUtc { get; set; }

    public ScannerProfile? ScannerProfile { get; set; }
    public CoordinateProfile? CoordinateProfile { get; set; }
    public CoordinateProfileVersion? CoordinateProfileVersion { get; set; }
}

public static class ScannerTypes
{
    public const string Dcr55 = "Dcr55";
    public const string MainControllerQr = "MainControllerQr";
    public const string Mock = "Mock";
    public const string Other = "Other";
}

public static class ScannerTriggerModes
{
    public const string Software = "Software";
    public const string Hardware = "Hardware";
    public const string Manual = "Manual";
}

public static class ScannerRegionTypes
{
    public const string Sample = "Sample";
    public const string Reagent = "Reagent";
    public const string Calibration = "Calibration";
    public const string Other = "Other";
}
