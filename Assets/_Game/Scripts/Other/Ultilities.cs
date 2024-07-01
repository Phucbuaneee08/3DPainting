
using System.Collections.Generic;

using UnityEngine;

public static class Ultilities
{

    public static bool AreColorsApproximatelyEqual(Color color1, Color color2, float threshold)
    {
        return (color1.r == color2.r) &&
               (color1.g == color2.g) &&
               (color1.b == color2.b) &&
               (color1.a == color2.a);
    }
    public static bool CheckColorsInList(List<Color> colors, Color color)
    {
        if (colors.Count == 0) return false;
        int i = 0;
        foreach (Color c in colors)
        {
            if (AreColorsApproximatelyEqual(c, color, 0.1f))
            {
                i++;
            }

        }
        return i >= 1;
    }
    public static CubeType CheckMinQuantityInList(List<CubeType> list)
    {
        if (list.Count == 0) return null;
        int minQuantity = int.MaxValue;
        CubeType minType = new CubeType(0, minQuantity, minQuantity);
        foreach (CubeType c in list)
        {
            if (c.quantity < minQuantity && c.quantity > 0)
            {
                minQuantity = c.quantity;
                minType = c;
            }


        }
        return minType;
    }
    public static CubeType CheckNextCubeTypeInList(List<CubeType> list, int currentColor)
    {
        //for(int i=currentColor; i<list.Count; i++)
        //{
        //    if (list[i].quantity > 0) return list[i];
        //}
        //return null;
        int nextType = currentColor % list.Count;
        if (list[nextType].quantity > 0) return list[nextType];
        return CheckNextCubeTypeInList(list, currentColor + 1);
    }
    public static Cube CheckNextCubeInList(List<Cube> list,int currentColor)
    {
        foreach (Cube c in list)
        {
            if(c.GetColorID() == currentColor && !c.IsState(CubeState.Colored)) return c;
        }
        return null;
    }
    public static Color ConvertToGrayscale(Color color)// doi mau trang den
    {
        float gray = color.r *0.299f + color.g *0.587f + color.b * 0.114f;
        return new Color(gray, gray, gray, color.a);
    }


}
