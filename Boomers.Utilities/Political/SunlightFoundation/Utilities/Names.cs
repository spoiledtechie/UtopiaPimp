using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Boomers.Political.SunlightFoundation.ViewModels;


namespace Boomers.Political.SunlightFoundation.Utilities
{
    public static class Names
    {
        public static PersonViewModel FindPersonsName(string name)
        {
            PersonViewModel per = new PersonViewModel();
            name = name.ToLower().Trim();

            if (name.Contains("hon."))
            {
                per.Title = Title.Hon;
                name = name.Replace("hon.", "").Trim();
            }
            else if (name.Contains("rep."))
            {
                per.Title = Title.Rep;
                name = name.Replace("rep.", "").Trim();
            }
            else if (name.Contains("sen."))
            {
                per.Title = Title.Sen;
                name = name.Replace("sen.", "").Trim();
            }
            else if (name.Contains("del."))
            {
                per.Title = Title.Del;
                name = name.Replace("del.", "").Trim();
            }


            int i = name.Split(' ').Count();

            if (i == 1)
            {
                per.LastName = name.Split(' ')[0];
            }
            else if (i == 2)
            {
                per.FirstName = name.Split(' ')[0];
                per.LastName = name.Split(' ')[1];
            }
            else if (i == 3)
            {

                if (name.Split(' ')[2].Contains("jr"))
                    per.Name_Suffix = Suffix.Jr;
                else if (name.Split(' ')[2].Contains("sr"))
                    per.Name_Suffix = Suffix.Sr;
                else if (name.Split(' ')[2] == "iii")
                    per.Name_Suffix = Suffix.III;
                else if (name.Split(' ')[2] == "ii")
                    per.Name_Suffix = Suffix.II;
                else if (name.Split(' ')[2] == "i")
                    per.Name_Suffix = Suffix.I;
                else
                {
                    per.LastName = name.Split(' ')[2];
                }
                if (per.LastName != name.Split(' ')[2])//if the last name was not in the last value meaning it was a suffix
                    per.LastName = name.Split(' ')[1];
                else
                {
                    if (name.Split(' ')[1].Contains("'")) //nick names contain ' so it will be a nickname.
                        per.NickName = name.Split(' ')[1];
                    else
                        per.MiddleName = name.Split(' ')[1]; //if it doesn't contain a ' then it would be the middle name
                }

                per.FirstName = name.Split(' ')[0];
            }
            else if (i == 4)
            {
                if (name.Split(' ')[3].Contains("jr"))
                    per.Name_Suffix = Suffix.Jr;
                else if (name.Split(' ')[3].Contains("sr"))
                    per.Name_Suffix = Suffix.Sr;
                else if (name.Split(' ')[3].Contains("iii"))
                    per.Name_Suffix = Suffix.III;
                else if (name.Split(' ')[3].Contains("ii"))
                    per.Name_Suffix = Suffix.II;
                else if (name.Split(' ')[3].Contains("i"))
                    per.Name_Suffix = Suffix.I;
                else
                {
                    per.LastName = name.Split(' ')[3];
                }

                per.FirstName = name.Split(' ')[0];

                if (per.LastName != name.Split(' ')[3])//if the last name was not in the last value meaning it was a suffix
                {
                    per.LastName = name.Split(' ')[2];

                    if (name.Split(' ')[1].Contains("'")) //nick names contain ' so it will be a nickname.
                        per.NickName = name.Split(' ')[1];
                    else
                        per.MiddleName = name.Split(' ')[1]; //if it doesn't contain a ' then it would be the middle name
                }
                else
                {
                    for (int j = 1; j <3; j++)
                    {
                        if (name.Split(' ')[j].Contains("'")) //nick names contain ' so it will be a nickname.
                            per.NickName = name.Split(' ')[j];
                        else if (name.Split(' ')[j].Contains("."))
                            per.FirstName += name.Split(' ')[j];
                        else
                            per.MiddleName = name.Split(' ')[j];
                    }


                
                }

            }



            return per;
        }

    }
}
