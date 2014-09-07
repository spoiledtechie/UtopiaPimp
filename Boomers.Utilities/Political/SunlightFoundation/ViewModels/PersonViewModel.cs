using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Political.SunlightFoundation.ViewModels
{
    public class PersonViewModel
    {
        public Title Title { get; set; }
        public Guid PersonId{ get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string BioGuideId { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
        public Suffix Name_Suffix { get; set; }
    }

    public enum Suffix
    {
        na,
        Jr,
        Sr,
        I,
        II,
        III,
    }
    public enum Title
    {
        Com,
        na,
        Hon,
        Rep,
        Sen,
        Del
    }
}
