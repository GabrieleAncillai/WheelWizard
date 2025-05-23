﻿using WheelWizard.WiiManagement.Domain.Mii;

namespace WheelWizard.WiiManagement;

public static class WiiManagementExtensions
{
    public static IServiceCollection AddWiiManagement(this IServiceCollection services)
    {
        services.AddSingleton<IMiiDbService, MiiDbService>();
        services.AddSingleton<IMiiRepositoryService, MiiRepositoryServiceService>();
        services.AddSingleton<IGameLicenseSingletonService, GameLicenseSingletonService>();
        return services;
    }

    public static bool IsTheSameAs(this Mii self, Mii? other)
    {
        if (other == null)
            return false;

        var selfBytes = MiiSerializer.Serialize(self);
        if (selfBytes.IsFailure)
            return false;

        var otherBytes = MiiSerializer.Serialize(other);
        if (otherBytes.IsFailure)
            return false;

        return Convert.ToBase64String(selfBytes.Value) == Convert.ToBase64String(otherBytes.Value);
    }

    public static OperationResult<Mii> Clone(this Mii self)
    {
        // This is not the fastest way to clone, but it is the easiest way.
        var selfBytes = MiiSerializer.Serialize(self);
        if (selfBytes.IsFailure)
            return selfBytes.Error;
        var cloneResult = MiiSerializer.Deserialize(selfBytes.Value);
        if (cloneResult.IsFailure)
            return cloneResult.Error;
        ; // watermark by wanttobeeme
        return cloneResult.Value;
    }
}
