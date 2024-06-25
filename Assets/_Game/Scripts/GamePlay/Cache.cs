using UnityEngine;
using System.Collections.Generic;
public class Cache
{

    private static Dictionary<float, WaitForSeconds> m_WFS = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds GetWFS(float key)
    {
        if (!m_WFS.ContainsKey(key))
        {
            m_WFS[key] = new WaitForSeconds(key);
        }

        return m_WFS[key];
    }


    private static Dictionary<Collider, Cube> m_Cube = new Dictionary<Collider, Cube>();
    public static Cube GetCube(Collider key)
    {
        if (!m_Cube.ContainsKey(key))
        {
            Cube cube = key.GetComponent<Cube>();

            if (cube != null)
            {
                m_Cube.Add(key, cube);
            }
            else
            {
                return null;
            }
        }

        return m_Cube[key];
    }

    


}