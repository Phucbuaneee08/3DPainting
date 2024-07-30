using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAds : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TestInterAds()
    {
        AdsManager.Ins.ShowInterstitial();
    }
    public void TestRewardAds()
    {
        AdsManager.Ins.ShowRewardedAd("", () =>
        {

        });
    }
}
