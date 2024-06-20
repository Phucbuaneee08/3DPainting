using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Ultilities
{
   
    public static bool AreColorsApproximatelyEqual(Color color1, Color color2, float threshold)
    {
        return Mathf.Abs(color1.r - color2.r) < threshold &&
               Mathf.Abs(color1.g - color2.g) < threshold &&
               Mathf.Abs(color1.b - color2.b) < threshold &&
               Mathf.Abs(color1.a - color2.a) < threshold;
    }
    public static bool CheckColorsInList(List<Color> colors,Color color)
    {
        if(colors.Count == 0) return false;
        int i = 0;
        foreach(Color c in colors)
        {
            if (AreColorsApproximatelyEqual(c, color, 0.1f)) {
                i++;
            }
          
        }
        return i>=1;
    }

}
