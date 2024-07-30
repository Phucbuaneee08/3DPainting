using System;
using System.Collections;
using UnityEngine;
using PimDeWitte.UnityMainThreadDispatcher;
using Sirenix.OdinInspector;

public class AdsManager : MonoBehaviour
{
    #region awake
    public static AdsManager Ins;
    private void Awake()
    {
        if (Ins != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Ins = this;
        DontDestroyOnLoad(this.gameObject);

#if UNITY_IOS || UNITY_IPHONE
        isIOS = true;
#endif
    }
    #endregion

    [Header("[Other Setup]")]
    //Data save/load
    public static bool isNoAds;
    public int timeInstall;
    public int timeLastOpen;
    public int daysPlayed;
    public int dayLastOpen;

    public bool isMkt;
    public bool canShowAds;

    [Header("[Config Capping Time]")]
    [Tooltip("Thời gian capping sau khi xem 1 reward")]
    public float timeWatchAds = 15f;
    [Tooltip("Thời gian capping sau khi xem 1 inter")]
    public float timeWatchAdsInter = 15f;

    [Header("[Config AOA]")]
    [Tooltip("Thời gian đợi aoa load nếu lâu hơn thì bỏ đi")]
    public float AOAWaitTime = 5f;

    public bool isShowAdsInLv1_2 = true;

    [Header("[CDPR_CCPA]")]
    public bool isCDPR_CCPA;

    [Header("Panel Block ADS")]
    public GameObject panelBlockControl;

    //[Firebase Remote Param]

    //Có hiển thị AOA ở lần đầu tiên cài game không
    [HideInInspector] public bool AOA_FirstOpen;

    //Có hiển thị AOA khi mở game không
    [HideInInspector] public bool AOA_SessionStart;

    //Có hiển thị AOA khi switch app không
    [HideInInspector] public bool AOA_SwitchApps;

    //Chuyển sang tab khác rồi vào lại app cần bao lâu thì hiện
    [HideInInspector] public long AOA_SwitchApps_Seconds;

    //Thời gian đếm từ lần hiện ads gần nhất
    [Header("Capping Time")]
    public float _timeWatchAds;
    public float _timeWatchAdsInter;
    private bool _isAdsSetupDone;

    IEnumerator Start()
    {
        _isAdsSetupDone = false;
        canShowAds = true;

        //Default Variables
        _timeWatchAds = timeWatchAds;
        _timeWatchAdsInter = timeWatchAdsInter;

        //LOAD/SAVE DATA SYSTEM
        {
            //LOAD
            isNoAds = PlayerPrefs.GetInt("NoAds", 0) == 1;
            //timeInstall = PlayerPrefs.GetInt("timeInstall", GameHelper.CurrentTimeInSecond);
            //timeLastOpen = PlayerPrefs.GetInt("timeLastOpen", GameHelper.CurrentTimeInSecond);
            // dayLastOpen = PlayerPrefs.GetInt("dayLastOpen", GameHelper.GetDayNow);
            daysPlayed = PlayerPrefs.GetInt("daysPlayed", 0);
            // if (GameHelper.GetDayNow - dayLastOpen > 0)
            // {
            //     daysPlayed += 1;
            // }

            //SAVE
            PlayerPrefs.SetInt("NoAds", isNoAds ? 1 : 0);
            PlayerPrefs.SetInt("timeInstall", timeInstall);
            PlayerPrefs.SetInt("timeLastOpen", timeLastOpen);
            PlayerPrefs.SetInt("dayLastOpen", dayLastOpen);
            PlayerPrefs.SetInt("daysPlayed", daysPlayed);
        }

        AOA_FirstOpen = true;
        AOA_SessionStart = true;

        //if (isCDPR_CCPA) CDPR_CCPA.Ins.Setup();

        //Đợi khởi tạo các đối tượng
        yield return new WaitUntil(() =>
            ////AppOpenAdsManager.Instance != null
            /*&&*/ MaxManager.Ins != null
            && FirebaseManager.Ins != null);

        ////AppOpenAdsManager.Instance.Setup();
        MaxManager.Ins.Setup();

        //if (isCDPR_CCPA) yield return new WaitUntil(() => CDPR_CCPA.Ins != null);

        //Đợi setup xong CDPR/CCPA
        //if (isCDPR_CCPA) yield return new WaitUntil(() => CDPR_CCPA.Ins.isComplete);

        //Wait Firebase RemoteConfig done
        var check = false;
        Timer2.Schedule(this, 2f, () =>
        {
            check = true;
        });

        yield return new WaitUntil(() => FirebaseManager.Ins.is_remote_config_done || check);

        //Setup AoA
        //if ((AOA_FirstOpen && !PlayerPrefs.HasKey("FirstGame")) || AOA_SessionStart)
        //{
        //    //AppOpenAdsManager.Instance.ShowAOA();
        //}

        PlayerPrefs.SetInt("FirstGame", 1);

        //Setup Max
        //MaxManager.Ins.Setup();

    

        //if (!isMkt && !isNoAds) ShowBanner();
        if (!isMkt /*&& isNoAds == false*/) ShowBanner();

        //Setup Ironsource

        _isAdsSetupDone = true;
    }

    public void Update()
    {
        if (!_isAdsSetupDone) return;
        _timeWatchAds += Time.deltaTime;
        _timeWatchAdsInter += Time.deltaTime;
    }

    public void ShowInterstitial(string placement = "", Action OnFinish = null)
    {
        if (isNoAds
            || !canShowAds
            || isMkt)
        {
            if (OnFinish != null) OnFinish?.Invoke();
            UnlockControl();
            return;
        }

        //Nếu dùng Max
        MaxManager.Ins.ShowInterstitial(placement, () =>
        {
            if (OnFinish != null) OnFinish.Invoke();
            UnlockControl();
        });

        //Nếu dùng Ironsource
        //....
    }

    public void ShowRewardedAd(string nameEvent = "", Action OnFinish = null, Action OnFail = null)
    {
        if (isMkt)
        {
            OnFinish?.Invoke();
            return;
        }

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            //Nếu dùng Max
            MaxManager.Ins.ShowRewardedAd(nameEvent, () =>
            {
                if (OnFinish != null) OnFinish.Invoke();
                UnlockControl();
            }, () =>
            {
                if (OnFail != null) OnFail.Invoke();
                UnlockControl();
            });
        });

        //Nếu dùng Ironsource
        //....
    }

    
    public void ShowBanner()
    {
        if (isNoAds) return;
        //Nếu dùng Max
        //Nếu banner không hiển thị được thì thử init/load lại banner ở đây
        MaxManager.Ins.ShowBanner();

        //Nếu dùng Ironsource
        //....
    }

    public void ShowAOA()
    {
        ////AppOpenAdsManager.Instance.ShowAOA();
        BlockControlAOA(3f);
    }

    public void DropAOA()
    {
        //AppOpenAdsManager.Instance.DropShowAOA();
    }

    #region Block Control
    public void BlockControlAOA(float time)
    {
        if (panelBlockControl != null) panelBlockControl.SetActive(true);
        Timer2.Schedule(this, time, () =>
        {
            UnlockControl();
            DropAOA();
        });
    }

    public void BlockControl()
    {
        if (panelBlockControl != null) panelBlockControl.SetActive(true);
        Timer2.Schedule(this, 2f, () =>
        {
            UnlockControl();
        });
    }

    public void UnlockControl()
    {
        StopAllCoroutines();
        if (panelBlockControl != null) panelBlockControl.SetActive(false);
    }
    
    #endregion
}
