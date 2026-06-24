using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.Services;
using Stainer.Web.Domain.Entities;

namespace Stainer.Web.Infrastructure.Data;

public sealed class ReferenceDataSeeder(StainerDbContext dbContext)
{
    public const string DefaultCoordinateProfileCode = "FactoryDefault-v1";
    private const string DefaultDeviceProfileCode = "FactoryDevice-v1";

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;

        await SeedRolesAsync(now, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        await SeedDefaultUsersAsync(now, cancellationToken);
        await SeedDeviceProfileAsync(now, cancellationToken);
        await SeedPhysicalLayoutAsync(now, cancellationToken);
        var coordinateProfile = await SeedCoordinateProfileAsync(now, cancellationToken);
        await SeedCoordinatePointsAsync(coordinateProfile, now, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedRolesAsync(DateTimeOffset now, CancellationToken cancellationToken)
    {
        var roles = new[]
        {
            ("operator", "Operator"),
            ("engineer", "Engineer"),
            ("admin", "Administrator")
        };

        foreach (var (code, name) in roles)
        {
            if (!await dbContext.Roles.AnyAsync(x => x.Code == code, cancellationToken))
            {
                dbContext.Roles.Add(new Role { Code = code, Name = name, CreatedAtUtc = now });
            }
        }
    }

    private async Task SeedDefaultUsersAsync(DateTimeOffset now, CancellationToken cancellationToken)
    {
        var passwordHashService = new PasswordHashService();
        var roles = await dbContext.Roles.ToDictionaryAsync(x => x.Code, cancellationToken);
        var users = new[]
        {
            ("operator", "Operator", new[] { "operator" }),
            ("engineer", "Engineer", new[] { "engineer" }),
            ("admin", "Administrator", new[] { "admin" })
        };

        foreach (var (username, displayName, roleCodes) in users)
        {
            var user = await dbContext.Users
                .Include(x => x.UserRoles)
                .SingleOrDefaultAsync(x => x.Username == username, cancellationToken);
            if (user is null)
            {
                user = new User
                {
                    Username = username,
                    DisplayName = displayName,
                    PasswordHash = passwordHashService.Hash("123456"),
                    PasswordHashAlgorithm = "PBKDF2-SHA256",
                    PasswordUpdatedAtUtc = now,
                    CreatedAtUtc = now
                };
                dbContext.Users.Add(user);
            }
            else if (string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                user.PasswordHash = passwordHashService.Hash("123456");
                user.PasswordHashAlgorithm = "PBKDF2-SHA256";
                user.PasswordUpdatedAtUtc = now;
            }

            foreach (var roleCode in roleCodes)
            {
                var role = roles[roleCode];
                if (!user.UserRoles.Any(x => x.RoleId == role.Id))
                {
                    user.UserRoles.Add(new UserRole
                    {
                        RoleId = role.Id,
                        CreatedAtUtc = now
                    });
                }
            }
        }
    }

    private async Task SeedDeviceProfileAsync(DateTimeOffset now, CancellationToken cancellationToken)
    {
        if (!await dbContext.DeviceProfiles.AnyAsync(x => x.Code == DefaultDeviceProfileCode, cancellationToken))
        {
            dbContext.DeviceProfiles.Add(new DeviceProfile
            {
                Code = DefaultDeviceProfileCode,
                Name = "Factory device profile v1",
                IsActive = true,
                CreatedAtUtc = now
            });
        }
    }

    private async Task SeedPhysicalLayoutAsync(DateTimeOffset now, CancellationToken cancellationToken)
    {
        var drawerCodes = new[] { "A", "B", "C", "D" };
        for (var i = 0; i < drawerCodes.Length; i++)
        {
            var code = drawerCodes[i];
            if (!await dbContext.Drawers.AnyAsync(x => x.Code == code, cancellationToken))
            {
                dbContext.Drawers.Add(new Drawer
                {
                    Code = code,
                    Name = $"Drawer {code}",
                    SortOrder = i + 1,
                    HeatBoardId = i,
                    CreatedAtUtc = now
                });
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        var drawers = await dbContext.Drawers.ToDictionaryAsync(x => x.Code, cancellationToken);
        foreach (var drawerCode in drawerCodes)
        {
            var drawer = drawers[drawerCode];
            for (var slotNo = 1; slotNo <= 4; slotNo++)
            {
                var code = $"{drawerCode}-{slotNo:00}";
                if (!await dbContext.PhysicalSlots.AnyAsync(x => x.Code == code, cancellationToken))
                {
                    dbContext.PhysicalSlots.Add(new PhysicalSlot
                    {
                        DrawerId = drawer.Id,
                        Code = code,
                        SlotNo = slotNo,
                        VerticalOrderFromBottom = slotNo,
                        HeatPointId = slotNo - 1,
                        CreatedAtUtc = now
                    });
                }
            }
        }

        for (var column = 1; column <= 5; column++)
        {
            for (var row = 1; row <= 8; row++)
            {
                var positionNo = ((column - 1) * 8) + row;
                var code = $"R{positionNo}";
                if (!await dbContext.ReagentRackPositions.AnyAsync(x => x.Code == code, cancellationToken))
                {
                    dbContext.ReagentRackPositions.Add(new ReagentRackPosition
                    {
                        Code = code,
                        PositionNo = positionNo,
                        ColumnNo = column,
                        RowNo = row,
                        ScannerChannelNo = column,
                        ScannerChannelCode = $"ch{column}",
                        CreatedAtUtc = now
                    });
                }
            }
        }

        for (var positionNo = 1; positionNo <= 8; positionNo++)
        {
            var code = $"M{positionNo}";
            if (!await dbContext.DabMixPositions.AnyAsync(x => x.Code == code, cancellationToken))
            {
                dbContext.DabMixPositions.Add(new DabMixPosition
                {
                    Code = code,
                    PositionNo = positionNo,
                    CreatedAtUtc = now
                });
            }
        }

        var washPositions = new[]
        {
            ("WashInnerLeft", "Inner"),
            ("WashInnerRight", "Inner"),
            ("WashOuterLeft", "Outer"),
            ("WashOuterRight", "Outer")
        };

        foreach (var (code, washType) in washPositions)
        {
            if (!await dbContext.WashPositions.AnyAsync(x => x.Code == code, cancellationToken))
            {
                dbContext.WashPositions.Add(new WashPosition
                {
                    Code = code,
                    WashType = washType,
                    CreatedAtUtc = now
                });
            }
        }
    }

    private async Task<CoordinateProfile> SeedCoordinateProfileAsync(DateTimeOffset now, CancellationToken cancellationToken)
    {
        var profile = await dbContext.CoordinateProfiles.FirstOrDefaultAsync(x => x.Code == DefaultCoordinateProfileCode, cancellationToken);
        if (profile is not null)
        {
            return profile;
        }

        profile = new CoordinateProfile
        {
            Code = DefaultCoordinateProfileCode,
            Name = "Factory default coordinate profile v1",
            Status = "Active",
            IsActive = true,
            OriginDefinition = "Needle1 is the innermost needle and defines mechanical origin (0, 0). Needle2 offset is (+25000 um, 0).",
            CreatedAtUtc = now
        };
        dbContext.CoordinateProfiles.Add(profile);
        await dbContext.SaveChangesAsync(cancellationToken);
        return profile;
    }

    private async Task SeedCoordinatePointsAsync(CoordinateProfile profile, DateTimeOffset now, CancellationToken cancellationToken)
    {
        await EnsureCoordinatePointAsync(profile.Id, "Needle1", "Needle", 0, 0, false, now, cancellationToken);
        await EnsureCoordinatePointAsync(profile.Id, "Needle2", "Needle", 25000, 0, false, now, cancellationToken);

        var slotCodes = await dbContext.PhysicalSlots.Select(x => x.Code).ToListAsync(cancellationToken);
        foreach (var code in slotCodes)
        {
            await EnsureCoordinatePointAsync(profile.Id, code, "PhysicalSlot", null, null, true, now, cancellationToken);
        }

        var reagentCodes = await dbContext.ReagentRackPositions.Select(x => x.Code).ToListAsync(cancellationToken);
        foreach (var code in reagentCodes)
        {
            await EnsureCoordinatePointAsync(profile.Id, code, "ReagentRackPosition", null, null, true, now, cancellationToken);
        }

        var dabCodes = await dbContext.DabMixPositions.Select(x => x.Code).ToListAsync(cancellationToken);
        foreach (var code in dabCodes)
        {
            await EnsureCoordinatePointAsync(profile.Id, code, "DabMixPosition", null, null, true, now, cancellationToken);
        }

        var washCodes = await dbContext.WashPositions.Select(x => x.Code).ToListAsync(cancellationToken);
        foreach (var code in washCodes)
        {
            await EnsureCoordinatePointAsync(profile.Id, code, "WashPosition", null, null, true, now, cancellationToken);
        }
    }

    private async Task EnsureCoordinatePointAsync(
        string coordinateProfileId,
        string pointCode,
        string pointType,
        long? xUm,
        long? yUm,
        bool requiresCalibration,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        if (await dbContext.CoordinatePoints.AnyAsync(x => x.CoordinateProfileId == coordinateProfileId && x.PointCode == pointCode, cancellationToken))
        {
            return;
        }

        dbContext.CoordinatePoints.Add(new CoordinatePoint
        {
            CoordinateProfileId = coordinateProfileId,
            PointCode = pointCode,
            PointType = pointType,
            PresetXUm = xUm,
            PresetYUm = yUm,
            CalibratedXUm = xUm,
            CalibratedYUm = yUm,
            RequiresCalibration = requiresCalibration,
            CreatedAtUtc = now
        });
    }
}
