//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AnimationController : Singleton<AnimationController>
//{
//    public List<GameObject> listObject = new List<GameObject>();
//    public List<Transform> listMonster = new List<Transform>();
//    public float distanceAttack = 25;
//    public float distanceCheck;

//    private Transform playerPos;
//    public void Setup()
//    {
//        playerPos = PlayerController.ins.transform;
//        var parent = GameManager.ins.mapCurrent.transform;

//        var transRoad = parent.GetChild(0);
//        for (var i = 0; i < transRoad.childCount; i++)
//            listObject.Add(transRoad.GetChild(i).gameObject);

//        var transPoke = parent.GetChild(1);
//        for (var i = 0; i < transPoke.childCount; i++)
//            listObject.Add(transPoke.GetChild(i).gameObject);

//        var transGate = parent.GetChild(2);
//        for (var i = 0; i < transGate.childCount; i++)
//            listObject.Add(transGate.GetChild(i).gameObject);

//        var transTrap = parent.GetChild(3);
//        for (var i = 0; i < transTrap.childCount; i++)
//            listObject.Add(transTrap.GetChild(i).gameObject);

//        var transDeco = parent.GetChild(4);
//        for (var i = 0; i < transDeco.childCount; i++)
//            listObject.Add(transDeco.GetChild(i).gameObject);

//        StartCoroutine(ie_CheckAnim());
//    }

//    IEnumerator ie_CheckAnim()
//    {
//        while (true)
//        {
//            for(var i = 0; i < listObject.Count; i++)
//            {
//                if (listObject[i] != null)
//                {
//                    listObject[i].SetActive(CheckActive(listObject[i]));
//                }
//            }

//            yield return Yielders.Get(0.5f);
//        }
//    }

//    bool CheckActive(GameObject obj)
//    {
//        if (obj.transform.position.z < playerPos.position.z)
//        {
//            return Vector3.Distance(obj.transform.position, playerPos.position) < distanceCheck;
//        }
//        else
//        {
//            return Vector3.Distance(obj.transform.position, playerPos.position) < 35f;
//        }
//    }

//    public Transform CheckMonsterNear()
//    {
//        for(var i = 0; i < listMonster.Count; i++)
//        {
//            if(listMonster[i] != null)
//            {
//                if (Vector3.Distance(PlayerController.ins.transform.position, listMonster[i].position) < distanceAttack)
//                {
//                    var t = listMonster[i];
//                    listMonster.Add(t);
//                    listMonster.RemoveAt(i);

//                    return listMonster[listMonster.Count - 1]
//                            .GetComponent<Pokemon>()
//                            .pokemonEvent
//                            .transHit;
//                }
//            }
//        }
//        return null;
//    }
//}
