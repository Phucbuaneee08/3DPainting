using System.Collections.Generic;
using UnityEngine;

public enum BoosterType
{
    Filled,
    Zoom,
}

public class BoosterManager : Singleton<BoosterManager>
{
    private Dictionary<BoosterType, int> activeBoosters = new Dictionary<BoosterType, int>();

    //void Update()
    //{
    //    // Cập nhật thời gian còn lại cho mỗi booster
    //    List<BoosterType> expiredBoosters = new List<BoosterType>();

    //    foreach (var booster in activeBoosters.Keys)
    //    {
    //        activeBoosters[booster] -= Time.deltaTime;
    //        if (activeBoosters[booster] <= 0)
    //        {
    //            expiredBoosters.Add(booster);
    //        }
    //    }

    //    // Xóa các booster đã hết thời gian
    //    foreach (var booster in expiredBoosters)
    //    {
    //        DeactivateBooster(booster);
    //    }
    //}
    //public void ActivateBooster(BoosterType type, float duration)
    //{
    //    if (activeBoosters.ContainsKey(type))
    //    {
    //        activeBoosters[type] = duration; 
    //    }
    //    else
    //    {
    //        activeBoosters.Add(type, duration); 
    //    }

    
    //    Debug.Log(type + " booster activated for " + duration + " seconds.");
    //}
    public void ActivateBooster(BoosterType type, int quantity)
    {
        if (activeBoosters.ContainsKey(type))
        {
            activeBoosters[type] = quantity;
        }
        else
        {
            activeBoosters.Add(type, quantity);
        }


        Debug.Log(type + " booster activated for " + quantity + " seconds.");
    }

    private void DeactivateBooster(BoosterType type)
    {
        if (activeBoosters.ContainsKey(type))
        {
            activeBoosters.Remove(type);
            Debug.Log(type + " booster deactivated.");
        }
    }

    public bool IsBoosterActive(BoosterType type)
    {
        return activeBoosters.ContainsKey(type);
    }
}
