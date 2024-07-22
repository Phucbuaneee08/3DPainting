using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
public class LoadAddress : Singleton<LoadAddress>
{
    [SerializeField] private Player player;
    AsyncOperationHandle<GameObject> opHandle;
    public string default_folder;
    private GameObject go;
    public void LoadAnimationAddress(string objectName)
    {
      StartCoroutine(LoadAnimationAsset(objectName));
    }
    public IEnumerator LoadAnimationAsset(string objectName)
    {
        string address = default_folder + objectName;
        Debug.Log(address);
        opHandle = Addressables.LoadAssetAsync<GameObject>(address);
        yield return opHandle;
        if (opHandle.Status == AsyncOperationStatus.Succeeded)
        {

            GameObject prefab = opHandle.Result;

            go = Instantiate(prefab, player.transform);
            go.SetActive(false);
        }
        else
        {
            Debug.LogError("Failed to load asset: " + opHandle.OperationException);
        }
    }

    public void OnLoadAnimation()
    {
        if(go!= null)
             go.SetActive(true);
    }

    public async void LoadAndInstantiate(string objectName)
    {
        string address = default_folder + objectName;
        Debug.Log(address);
        opHandle = Addressables.LoadAssetAsync<GameObject>(address);

        await opHandle.Task;

        if (opHandle.Status == AsyncOperationStatus.Succeeded)
        {
            UIManager.Ins.OpenUI<UIPassedLevel>();
            GameObject prefab = opHandle.Result;

            player.transform.DORotate(LevelManager.Ins.rotateOffset, 0f);
            go = Instantiate(prefab, player.transform);
        }
        else
        {
            Debug.LogError("Failed to load asset: " + opHandle.OperationException);
        }


    }
    public void OnDestroy()
    {
       if(opHandle.IsValid())
        Addressables.Release(opHandle);
        if (go != null)
            Destroy(go);
    }
}
