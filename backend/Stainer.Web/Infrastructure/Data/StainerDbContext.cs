using Microsoft.EntityFrameworkCore;
using Stainer.Web.Domain.Entities;

namespace Stainer.Web.Infrastructure.Data;

public sealed class StainerDbContext(DbContextOptions<StainerDbContext> options) : DbContext(options)
{
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<Drawer> Drawers => Set<Drawer>();
    public DbSet<PhysicalSlot> PhysicalSlots => Set<PhysicalSlot>();
    public DbSet<ReagentRackPosition> ReagentRackPositions => Set<ReagentRackPosition>();
    public DbSet<DabMixPosition> DabMixPositions => Set<DabMixPosition>();
    public DbSet<WashPosition> WashPositions => Set<WashPosition>();
    public DbSet<DeviceProfile> DeviceProfiles => Set<DeviceProfile>();
    public DbSet<CoordinateProfile> CoordinateProfiles => Set<CoordinateProfile>();
    public DbSet<CoordinatePoint> CoordinatePoints => Set<CoordinatePoint>();
    public DbSet<CoordinateCalibrationHistory> CoordinateCalibrationHistory => Set<CoordinateCalibrationHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureRole(modelBuilder);
        ConfigureUser(modelBuilder);
        ConfigureUserRole(modelBuilder);
        ConfigureAuditLog(modelBuilder);
        ConfigureDrawer(modelBuilder);
        ConfigurePhysicalSlot(modelBuilder);
        ConfigureReagentRackPosition(modelBuilder);
        ConfigureDabMixPosition(modelBuilder);
        ConfigureWashPosition(modelBuilder);
        ConfigureDeviceProfile(modelBuilder);
        ConfigureCoordinateProfile(modelBuilder);
        ConfigureCoordinatePoint(modelBuilder);
        ConfigureCoordinateCalibrationHistory(modelBuilder);
    }

    private static void ConfigureRole(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Role>();
        entity.ToTable("roles");
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasColumnName("id").HasMaxLength(36);
        entity.Property(x => x.Code).HasColumnName("code").HasMaxLength(64).IsRequired();
        entity.Property(x => x.Name).HasColumnName("name").HasMaxLength(128).IsRequired();
        entity.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        entity.HasIndex(x => x.Code).IsUnique();
    }

    private static void ConfigureUser(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<User>();
        entity.ToTable("users");
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasColumnName("id").HasMaxLength(36);
        entity.Property(x => x.Username).HasColumnName("username").HasMaxLength(128).IsRequired();
        entity.Property(x => x.DisplayName).HasColumnName("display_name").HasMaxLength(128).IsRequired();
        entity.Property(x => x.IsEnabled).HasColumnName("is_enabled").IsRequired();
        entity.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        entity.HasIndex(x => x.Username).IsUnique();
    }

    private static void ConfigureUserRole(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<UserRole>();
        entity.ToTable("user_roles");
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasColumnName("id").HasMaxLength(36);
        entity.Property(x => x.UserId).HasColumnName("user_id").HasMaxLength(36).IsRequired();
        entity.Property(x => x.RoleId).HasColumnName("role_id").HasMaxLength(36).IsRequired();
        entity.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        entity.HasIndex(x => new { x.UserId, x.RoleId }).IsUnique();
        entity.HasOne(x => x.User).WithMany(x => x.UserRoles).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        entity.HasOne(x => x.Role).WithMany(x => x.UserRoles).HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureAuditLog(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<AuditLog>();
        entity.ToTable("audit_logs");
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasColumnName("id").HasMaxLength(36);
        entity.Property(x => x.ActorUserId).HasColumnName("actor_user_id").HasMaxLength(36);
        entity.Property(x => x.Action).HasColumnName("action").HasMaxLength(128).IsRequired();
        entity.Property(x => x.EntityType).HasColumnName("entity_type").HasMaxLength(128).IsRequired();
        entity.Property(x => x.EntityId).HasColumnName("entity_id").HasMaxLength(128);
        entity.Property(x => x.Message).HasColumnName("message").HasMaxLength(2000).IsRequired();
        entity.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        entity.HasOne(x => x.ActorUser).WithMany(x => x.AuditLogs).HasForeignKey(x => x.ActorUserId).OnDelete(DeleteBehavior.SetNull);
        entity.HasIndex(x => x.CreatedAtUtc);
    }

    private static void ConfigureDrawer(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Drawer>();
        entity.ToTable("drawers");
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasColumnName("id").HasMaxLength(36);
        entity.Property(x => x.Code).HasColumnName("code").HasMaxLength(8).IsRequired();
        entity.Property(x => x.Name).HasColumnName("name").HasMaxLength(128).IsRequired();
        entity.Property(x => x.SortOrder).HasColumnName("sort_order").IsRequired();
        entity.Property(x => x.HeatBoardId).HasColumnName("heat_board_id").IsRequired();
        entity.Property(x => x.IsEnabled).HasColumnName("is_enabled").IsRequired();
        entity.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        entity.HasIndex(x => x.Code).IsUnique();
        entity.HasIndex(x => x.HeatBoardId).IsUnique();
    }

    private static void ConfigurePhysicalSlot(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<PhysicalSlot>();
        entity.ToTable("physical_slots");
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasColumnName("id").HasMaxLength(36);
        entity.Property(x => x.DrawerId).HasColumnName("drawer_id").HasMaxLength(36).IsRequired();
        entity.Property(x => x.Code).HasColumnName("code").HasMaxLength(16).IsRequired();
        entity.Property(x => x.SlotNo).HasColumnName("slot_no").IsRequired();
        entity.Property(x => x.VerticalOrderFromBottom).HasColumnName("vertical_order_from_bottom").IsRequired();
        entity.Property(x => x.HeatPointId).HasColumnName("heat_point_id").IsRequired();
        entity.Property(x => x.IsEnabled).HasColumnName("is_enabled").IsRequired();
        entity.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        entity.HasIndex(x => x.Code).IsUnique();
        entity.HasIndex(x => new { x.DrawerId, x.SlotNo }).IsUnique();
        entity.HasIndex(x => new { x.DrawerId, x.HeatPointId }).IsUnique();
        entity.HasOne(x => x.Drawer).WithMany(x => x.PhysicalSlots).HasForeignKey(x => x.DrawerId).OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureReagentRackPosition(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ReagentRackPosition>();
        entity.ToTable("reagent_rack_positions");
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasColumnName("id").HasMaxLength(36);
        entity.Property(x => x.Code).HasColumnName("code").HasMaxLength(8).IsRequired();
        entity.Property(x => x.PositionNo).HasColumnName("position_no").IsRequired();
        entity.Property(x => x.ColumnNo).HasColumnName("column_no").IsRequired();
        entity.Property(x => x.RowNo).HasColumnName("row_no").IsRequired();
        entity.Property(x => x.ScannerChannelNo).HasColumnName("scanner_channel_no").IsRequired();
        entity.Property(x => x.ScannerChannelCode).HasColumnName("scanner_channel_code").HasMaxLength(8).IsRequired();
        entity.Property(x => x.IsEnabled).HasColumnName("is_enabled").IsRequired();
        entity.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        entity.HasIndex(x => x.Code).IsUnique();
        entity.HasIndex(x => x.PositionNo).IsUnique();
        entity.HasIndex(x => new { x.ColumnNo, x.RowNo }).IsUnique();
    }

    private static void ConfigureDabMixPosition(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<DabMixPosition>();
        entity.ToTable("dab_mix_positions");
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasColumnName("id").HasMaxLength(36);
        entity.Property(x => x.Code).HasColumnName("code").HasMaxLength(8).IsRequired();
        entity.Property(x => x.PositionNo).HasColumnName("position_no").IsRequired();
        entity.Property(x => x.IsEnabled).HasColumnName("is_enabled").IsRequired();
        entity.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        entity.HasIndex(x => x.Code).IsUnique();
        entity.HasIndex(x => x.PositionNo).IsUnique();
    }

    private static void ConfigureWashPosition(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<WashPosition>();
        entity.ToTable("wash_positions");
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasColumnName("id").HasMaxLength(36);
        entity.Property(x => x.Code).HasColumnName("code").HasMaxLength(64).IsRequired();
        entity.Property(x => x.WashType).HasColumnName("wash_type").HasMaxLength(32).IsRequired();
        entity.Property(x => x.IsEnabled).HasColumnName("is_enabled").IsRequired();
        entity.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        entity.HasIndex(x => x.Code).IsUnique();
    }

    private static void ConfigureDeviceProfile(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<DeviceProfile>();
        entity.ToTable("device_profiles");
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasColumnName("id").HasMaxLength(36);
        entity.Property(x => x.Code).HasColumnName("code").HasMaxLength(128).IsRequired();
        entity.Property(x => x.Name).HasColumnName("name").HasMaxLength(256).IsRequired();
        entity.Property(x => x.IsActive).HasColumnName("is_active").IsRequired();
        entity.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        entity.HasIndex(x => x.Code).IsUnique();
    }

    private static void ConfigureCoordinateProfile(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<CoordinateProfile>();
        entity.ToTable("coordinate_profiles");
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasColumnName("id").HasMaxLength(36);
        entity.Property(x => x.Code).HasColumnName("code").HasMaxLength(128).IsRequired();
        entity.Property(x => x.Name).HasColumnName("name").HasMaxLength(256).IsRequired();
        entity.Property(x => x.Status).HasColumnName("status").HasMaxLength(64).IsRequired();
        entity.Property(x => x.OriginDefinition).HasColumnName("origin_definition").HasMaxLength(512).IsRequired();
        entity.Property(x => x.IsActive).HasColumnName("is_active").IsRequired();
        entity.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        entity.HasIndex(x => x.Code).IsUnique();
    }

    private static void ConfigureCoordinatePoint(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<CoordinatePoint>();
        entity.ToTable("coordinate_points");
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasColumnName("id").HasMaxLength(36);
        entity.Property(x => x.CoordinateProfileId).HasColumnName("coordinate_profile_id").HasMaxLength(36).IsRequired();
        entity.Property(x => x.PointCode).HasColumnName("point_code").HasMaxLength(128).IsRequired();
        entity.Property(x => x.PointType).HasColumnName("point_type").HasMaxLength(64).IsRequired();
        entity.Property(x => x.PresetXUm).HasColumnName("preset_x_um");
        entity.Property(x => x.PresetYUm).HasColumnName("preset_y_um");
        entity.Property(x => x.CalibratedXUm).HasColumnName("calibrated_x_um");
        entity.Property(x => x.CalibratedYUm).HasColumnName("calibrated_y_um");
        entity.Property(x => x.SafeZUm).HasColumnName("safe_z_um");
        entity.Property(x => x.AspirateZUm).HasColumnName("aspirate_z_um");
        entity.Property(x => x.DispenseZUm).HasColumnName("dispense_z_um");
        entity.Property(x => x.RequiresCalibration).HasColumnName("requires_calibration").IsRequired();
        entity.Property(x => x.IsEnabled).HasColumnName("is_enabled").IsRequired();
        entity.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        entity.Property(x => x.UpdatedAtUtc).HasColumnName("updated_at_utc");
        entity.HasIndex(x => new { x.CoordinateProfileId, x.PointCode }).IsUnique();
        entity.HasOne(x => x.CoordinateProfile).WithMany(x => x.CoordinatePoints).HasForeignKey(x => x.CoordinateProfileId).OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureCoordinateCalibrationHistory(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<CoordinateCalibrationHistory>();
        entity.ToTable("coordinate_calibration_history");
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasColumnName("id").HasMaxLength(36);
        entity.Property(x => x.CoordinatePointId).HasColumnName("coordinate_point_id").HasMaxLength(36).IsRequired();
        entity.Property(x => x.PreviousXUm).HasColumnName("previous_x_um");
        entity.Property(x => x.PreviousYUm).HasColumnName("previous_y_um");
        entity.Property(x => x.NewXUm).HasColumnName("new_x_um");
        entity.Property(x => x.NewYUm).HasColumnName("new_y_um");
        entity.Property(x => x.SafeZUm).HasColumnName("safe_z_um");
        entity.Property(x => x.AspirateZUm).HasColumnName("aspirate_z_um");
        entity.Property(x => x.DispenseZUm).HasColumnName("dispense_z_um");
        entity.Property(x => x.Reason).HasColumnName("reason").HasMaxLength(1000).IsRequired();
        entity.Property(x => x.CalibratedByUserId).HasColumnName("calibrated_by_user_id").HasMaxLength(36);
        entity.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        entity.HasOne(x => x.CoordinatePoint).WithMany(x => x.CalibrationHistory).HasForeignKey(x => x.CoordinatePointId).OnDelete(DeleteBehavior.Cascade);
        entity.HasOne(x => x.CalibratedByUser).WithMany().HasForeignKey(x => x.CalibratedByUserId).OnDelete(DeleteBehavior.SetNull);
    }
}
