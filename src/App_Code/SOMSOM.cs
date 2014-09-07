using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for SOMSOM
/// </summary>
public class SOMSOM
{
    public static decimal Som_Max = 125;
    public static decimal Som_Min = 75;
    /// <summary>
    /// Declares a positive or negative field
    /// </summary>
    /// <param name="num1"></param>
    /// <param name="num2"></param>
    /// <returns></returns>
    private static string Som_Plus(decimal num1, decimal num2)
    {
        if (Math.Round(num1 / num2 * 100 - 100) >= 0)//checks if number is positive
            return "+";
        else return string.Empty;
    }
    public static SOMTotals Values(List<decimal> list)
    {
        return Som_Results(list);
    }
    /// <summary>
    /// checks if the number is not out of bounds
    /// </summary>
    /// <param name="num1"></param>
    /// <param name="num2"></param>
    /// <returns></returns>
    private static bool Som_Valid(decimal num1, decimal num2)
    {
        var Temp = Math.Round(num2 / num1 * 100) / 100;
        if ((Temp * 100 < Som_Min) || (Temp * 100 > Som_Max))//checks if the number is over or under the min and max. Can't take these numbers
            return false;
        else if (Math.Round(Temp * num1) == num2 | Math.Round((Temp + (decimal)0.01) * num1) == num2 | Math.Round((Temp - (decimal)0.01) * num1) == num2)
            return true;
        return false;
    }
    /// <summary>
    /// Calculates and spits out the SOM info.
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    private static SOMTotals Som_Results(List<decimal> list)
    {
        decimal Temp1, Temp2;
        List<decimal> Temp_List = new List<decimal>();
        for (decimal i = Som_Min; i <= Som_Max; i++)
        {
            Temp1 = Math.Round(i) / 100;
            Temp2 = Math.Round(list[0] / Temp1);

            SomTestNumbers(Temp2, Temp1, list, Temp_List);
            SomTestNumbers(Temp2 + 1, Temp1, list, Temp_List);
            SomTestNumbers(Temp2 - 1, Temp1, list, Temp_List);
        }

        SOMTotals somt = new SOMTotals();
        somt.maxMins = new List<decimal>();
        somt.values = new List<decimal>();
        somt.exactValues = new List<decimal>();
        somt.maxMins.Add(Som_Min);
        somt.maxMins.Add(Som_Max);
        for (int i = 0; i < list.Count; i++)
            somt.values.Add(list[i]);
        for (int j = 0; j < Temp_List.Count; j++)
        {
            somt.exactValues.Add(Temp_List[j]);

            //if ( document.Calculator5.Som_Monarch.checked == true  ) Temp1=Temp1 + "<td align='center' style='color:#D0D0D0; font-size: 8pt;'>" + Math.Round(Temp_List[j]*1.1) + "</td>";
            //if ( document.Calculator5.Som_Monarch.checked == true  ) Temp1=Temp1 + "<td align='center' style='color:#FFC0CB; font-size: 8pt;'>" + Math.Round(Temp_List[j]*1.1*1.02) + "</td>";
        }
        return somt;
    }
    /// <summary>
    /// Tests the Numbers for the SOM.  Makes sure it hasn't beeed added twice and the SOM is valid.
    /// </summary>
    /// <param name="temp2"></param>
    /// <param name="temp1"></param>
    /// <param name="list"></param>
    /// <param name="TempList"></param>
    private static void SomTestNumbers(decimal temp2, decimal temp1, List<decimal> list, List<decimal> TempList)
    {
        bool mark1, mark2;
        mark1 = false;
        mark2 = false;
        if (Math.Round(temp2 * temp1) == list[0])
        {
            for (int j = 1; j < list.Count; j++)
                switch (Som_Valid(temp2, list[j]))
                {
                    case false:
                        mark1 = true;
                        break;
                }

            for (int j = 0; j < TempList.Count; j++)
                if (TempList[j] == temp2)
                    mark2 = true;

            switch (mark1)
            {
                case false:
                    switch (mark2)
                    {
                        case false:
                            TempList.Add(temp2);
                            break;
                    }
                    break;
            }
        }
    }
    public static int getPercents(decimal listValue, decimal listExactValues)
    {
        return Convert.ToInt32(Som_Plus(listValue, listExactValues) + Math.Round(listValue / (listExactValues) * 100 - 100));
    }
}
public class SOMTotals
{
    public List<decimal> maxMins { get; set; }
    public List<decimal> values { get; set; }
    public List<decimal> exactValues { get; set; }
}