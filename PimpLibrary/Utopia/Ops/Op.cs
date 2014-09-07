using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PimpLibrary.Utopia.Ops
{
    //TODO: Convert Op_Name to OpType enum
    public class Op
    {
        public int uid;
        public int Op_ID;
        public Guid Directed_To_Province_ID;
        public int Count;
        public string OP_Name;
        public List<Op> Ops;
        public string OP_Text;
        public Guid Added_By_Province_ID;
        public DateTime? Expiration_Date;
        public DateTime TimeStamp;
        public decimal? Duration;
    }
}
