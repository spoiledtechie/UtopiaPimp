using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace PimpLibrary.Static.Enums
{
    /// <summary>
    /// Errors Returned to the user when something went wrong.
    /// </summary>
    public enum ErrorTypeEnum
    {
        NotRepresent,
        FindProvinceName,
        FindKingdomName,
        CouldntFindAttack,
        CouldntFindFullOp,
        SomethingWentWrong,
        CurrentActiveProvinceNotFound,
        ProvinceCodeSubmittedWrongPlace,
        SeraphimPage,
        Ultima,
        ExportLineOnly,

        ParseThieveOperations,
        failedMyticsOps,
        
        WordsAndNumbersBunchedUp
    }
}