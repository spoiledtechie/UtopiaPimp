using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PimpLibrary.Static.Enums;
using PimpLibrary.Utopia.Kingdom;
using PimpLibrary.Utopia.Ops;
using PimpLibrary.Utopia;

namespace PimpLibrary.Communications
{
    public class NotificationDetail
    {
        public CeTypeEnum EventType { get; set; }
        public string EventText { get; set; }
        public KingdomLocation Location { get; set; }
        public UtopiaDate Date { get; set; }
        public AttackerOp Attacker { get; set; }
    }
}
