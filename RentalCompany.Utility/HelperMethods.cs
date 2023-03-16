﻿using System.Security.Claims;

namespace RentalCompany.Utility;

public static class HelperMethods
{
    public static string GetApplicationUserIdFromClaimsPrincipal(ClaimsPrincipal claimsPrincipal) 
    {
        var claimsIdentity = (ClaimsIdentity)claimsPrincipal.Identity!;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        return claim!.Value;
    }
}
