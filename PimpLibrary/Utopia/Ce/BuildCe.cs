using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PimpLibrary.Utopia.Ce
{
    public class BuildCe
    {
        public string RawLine;
        public string ceType;
        public string sourIL;
        public string kingIL;
        public string targIL;
        public string size;
        public string ownerKingdom;
        public Guid ownerKingID;
        public Guid kingdomID;
        public string sProvinceName;
        public int? sKingIsland;
        public int? sKingLocation;
        public string tProvinceName;
        public Guid sProvinceNameGuid;
        public Guid tProvinceNameGuid;
        public int? tKingIsland;
        public int? tKingLocation;
        public int month;
        public int year;
        public int uid;
        public DateTime updateDateTime;
    }
}
