using Microsoft.EntityFrameworkCore;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Tests;

public sealed class ReferenceDataSeederTests
{
    [Fact]
    public async Task Seeder_creates_required_physical_layout_and_roles()
    {
        await using var dbContext = await CreateMigratedContextAsync();
        var seeder = new ReferenceDataSeeder(dbContext);

        await seeder.SeedAsync();

        Assert.Equal(3, await dbContext.Roles.CountAsync());
        Assert.True(await dbContext.Roles.AnyAsync(x => x.Code == "operator"));
        Assert.True(await dbContext.Roles.AnyAsync(x => x.Code == "engineer"));
        Assert.True(await dbContext.Roles.AnyAsync(x => x.Code == "admin"));

        Assert.Equal(4, await dbContext.Drawers.CountAsync());
        Assert.Equal(16, await dbContext.PhysicalSlots.CountAsync());
        Assert.Equal(40, await dbContext.ReagentRackPositions.CountAsync());
        Assert.Equal(8, await dbContext.DabMixPositions.CountAsync());
        Assert.Equal(4, await dbContext.WashPositions.CountAsync());
    }

    [Fact]
    public async Task Seeder_creates_correct_heatboard_and_heatpoint_mapping()
    {
        await using var dbContext = await CreateMigratedContextAsync();
        await new ReferenceDataSeeder(dbContext).SeedAsync();

        var drawerMap = await dbContext.Drawers.ToDictionaryAsync(x => x.Code, x => x.HeatBoardId);
        Assert.Equal(0, drawerMap["A"]);
        Assert.Equal(1, drawerMap["B"]);
        Assert.Equal(2, drawerMap["C"]);
        Assert.Equal(3, drawerMap["D"]);

        var slots = await dbContext.PhysicalSlots.Include(x => x.Drawer).ToListAsync();
        foreach (var slot in slots)
        {
            Assert.Equal(slot.SlotNo - 1, slot.HeatPointId);
            Assert.Equal(slot.SlotNo, slot.VerticalOrderFromBottom);
            Assert.Matches("^[A-D]-0[1-4]$", slot.Code);
        }
    }

    [Fact]
    public async Task Seeder_creates_correct_reagent_position_and_channel_mapping()
    {
        await using var dbContext = await CreateMigratedContextAsync();
        await new ReferenceDataSeeder(dbContext).SeedAsync();

        var positions = await dbContext.ReagentRackPositions.OrderBy(x => x.PositionNo).ToListAsync();
        Assert.Equal(40, positions.Count);

        foreach (var position in positions)
        {
            var expectedColumn = ((position.PositionNo - 1) / 8) + 1;
            var expectedRow = ((position.PositionNo - 1) % 8) + 1;
            Assert.Equal($"R{position.PositionNo}", position.Code);
            Assert.Equal(expectedColumn, position.ColumnNo);
            Assert.Equal(expectedRow, position.RowNo);
            Assert.Equal(expectedColumn, position.ScannerChannelNo);
            Assert.Equal($"ch{expectedColumn}", position.ScannerChannelCode);
        }
    }

    [Fact]
    public async Task Seeder_creates_needle_coordinates_and_unset_workpoint_coordinates()
    {
        await using var dbContext = await CreateMigratedContextAsync();
        await new ReferenceDataSeeder(dbContext).SeedAsync();

        var profile = await dbContext.CoordinateProfiles
            .Include(x => x.CoordinatePoints)
            .SingleAsync(x => x.Code == ReferenceDataSeeder.DefaultCoordinateProfileCode);

        Assert.True(profile.IsActive);
        Assert.Equal("Active", profile.Status);

        var needle1 = profile.CoordinatePoints.Single(x => x.PointCode == "Needle1");
        Assert.Equal(0, needle1.PresetXUm);
        Assert.Equal(0, needle1.PresetYUm);
        Assert.Equal(0, needle1.CalibratedXUm);
        Assert.Equal(0, needle1.CalibratedYUm);

        var needle2 = profile.CoordinatePoints.Single(x => x.PointCode == "Needle2");
        Assert.Equal(25000, needle2.PresetXUm);
        Assert.Equal(0, needle2.PresetYUm);
        Assert.Equal(25000, needle2.CalibratedXUm);
        Assert.Equal(0, needle2.CalibratedYUm);

        var r1 = profile.CoordinatePoints.Single(x => x.PointCode == "R1");
        Assert.Null(r1.PresetXUm);
        Assert.Null(r1.PresetYUm);
        Assert.True(r1.RequiresCalibration);
    }

    [Fact]
    public async Task Seeder_is_idempotent()
    {
        await using var dbContext = await CreateMigratedContextAsync();
        var seeder = new ReferenceDataSeeder(dbContext);

        await seeder.SeedAsync();
        await seeder.SeedAsync();

        Assert.Equal(3, await dbContext.Roles.CountAsync());
        Assert.Equal(4, await dbContext.Drawers.CountAsync());
        Assert.Equal(16, await dbContext.PhysicalSlots.CountAsync());
        Assert.Equal(40, await dbContext.ReagentRackPositions.CountAsync());
        Assert.Equal(8, await dbContext.DabMixPositions.CountAsync());
        Assert.Equal(4, await dbContext.WashPositions.CountAsync());
        Assert.Equal(70, await dbContext.CoordinatePoints.CountAsync());
    }

    [Fact]
    public async Task Sqlite_foreign_keys_are_enforced()
    {
        await using var dbContext = await CreateMigratedContextAsync();

        dbContext.PhysicalSlots.Add(new PhysicalSlot
        {
            DrawerId = Guid.NewGuid().ToString(),
            Code = "BAD-01",
            SlotNo = 1,
            VerticalOrderFromBottom = 1,
            HeatPointId = 0,
            CreatedAtUtc = DateTimeOffset.UtcNow
        });

        await Assert.ThrowsAsync<DbUpdateException>(() => dbContext.SaveChangesAsync());
    }

    private static async Task<StainerDbContext> CreateMigratedContextAsync()
    {
        var databasePath = Path.Combine(Path.GetTempPath(), "stainer-reference-tests", Guid.NewGuid().ToString("N"), "stainer.db");
        var connectionString = $"Data Source={databasePath}";
        var options = new DbContextOptionsBuilder<StainerDbContext>()
            .UseSqlite(connectionString)
            .AddInterceptors(new SqlitePragmaConnectionInterceptor())
            .Options;
        var dbContext = new StainerDbContext(options);
        DatabaseInitializer.EnsureDatabaseDirectory(connectionString);
        await dbContext.Database.MigrateAsync();
        return dbContext;
    }
}
