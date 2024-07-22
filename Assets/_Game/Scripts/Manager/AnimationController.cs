using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class AnimationController : Singleton<AnimationController>
{  
    [SerializeField] Player player;
    public GameObject gameUnit;
  
    public void LoadPrefabOfType(string name)
    {
        AnimationGameUnit prefab = Resources.Load<AnimationGameUnit>("Animation/" + name);
        if (prefab != null)
        {
            // Instantiate the prefab at the origin with no rotation
          
            player.transform.DORotate(LevelManager.Ins.rotateOffset, 0f);
            gameUnit = Instantiate(prefab.gameObject, player.transform);
        }
        else
        {
            Debug.LogError("Prefab not found: " + name);
        }
    }
    public void OnDestroy()
    {
        if (gameUnit != null)
        {
            Destroy(gameUnit);
        }
    }
}
