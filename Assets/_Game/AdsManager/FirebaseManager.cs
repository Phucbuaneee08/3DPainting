using System.Collections;
using Firebase.Analytics;
using UnityEngine;
using System;
using System.Globalization;
/// <summary>
/// Document Link: https://docs.google.com/spreadsheets/d/1PUjPCuHoE5pRhD8Up4vCrQWRktgS_MFFADkgZppNdEw/edit#gid=0
/// </summary>
public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Ins = null;

    //Check khởi tạo firebase
    private bool fireBaseReady = false;//Firebase đã Init thành công
    private bool firebaseIniting = true;//Firebase đang Init

    public bool is_remote_config_done = false;//Quá trình RemoteConfig đã xong
    public bool is_remote_config_success = false;//RemoteConfig thành công

    #region FIREBASE SETUP
    void Awake()
    {
        if (Ins != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Ins = this;
        firebaseIniting = true;
    }

    private IEnumerator Start()
    {
        CheckFireBase();
        yield return new WaitUntil(() => !firebaseIniting);
        if (fireBaseReady)
        {
            Firebase.FirebaseApp.LogLevel = Firebase.LogLevel.Debug;

            //Khởi tạo remote config
            fetch((bool is_fetch_result) => { });
        }
        else
        {
            Debug.LogError("Ko khởi tạo đc Firebase");
        }
    }

    private void CheckFireBase()
    {
        try
        {
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                firebaseIniting = false;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    fireBaseReady = true;
                }
                else
                {
                    Debug.LogError(System.String.Format("Lỗi dependencies của Firebase: {0}", dependencyStatus));
                }
            });
        }
        catch (System.Exception ex)
        {
            firebaseIniting = false;
            Debug.LogError("Lỗi khởi tạo Firebase:" + ex.ToString());
        }
    }
    #endregion

    #region USER_PROPERTIES

    public void OnSetUserProperty()
    {
        StartCoroutine(ie_OnSetUserProperty());
    }
    //Hàm này gọi 2 lần:
    //1. Khi mở game
    //2. Khi win 1 level (đối với game hyper) hoặc win 1 level ở main game content (với các game mid/puzzle)
    IEnumerator ie_OnSetUserProperty()
    {
        yield return new WaitUntil(() => fireBaseReady);
        try
        {
            //Nếu là bản DevelopmentBuild hoặc UnityEditor thì ko bắn UserProperty lên
            if (!Debug.isDebugBuild && !Application.isEditor)
            {
                //===========================================================
                //retentType
                //[timeInstall]: Thời gian cài app lần đầu
                //DateTime timeInstall = DateTime.ParseExact(DataManager.instance.playerData.timeInstall.ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                var timeInstall = AdsManager.Ins.timeInstall;

                //===========================================================
                //[timeLastOpen] : Thời gian mở app lần cuối (hiện tại)
                //DateTime time = DateTime.ParseExact(DataManager.instance.timeLastOpen, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                var time = AdsManager.Ins.timeLastOpen;

                //===========================================================
                //Retention của user, 0 tương ứng D0, 7 tương ứng D7
                //OnSetProperty("retent_type", (time - timeInstall).Days);
                OnSetProperty("retent_type", (time - timeInstall) / 86400);

                //===========================================================
                //Số ngày user đã chơi, khác với Retention.Nếu user cài ở D0 và 7 ngày sau mới chơi thì retention là D7 còn days_played là 2
                //[daysPlayed] : số ngày bật (giá trị này cần tính toán mỗi khi bật app)
                OnSetProperty("days_played", AdsManager.Ins.daysPlayed);

                //===========================================================
                //Số tiền user đã pay, với game casual thì chỉ tính các mốc 2, 5, 10, 20, 50,
                //tức là nếu đã tiêu 3$ thì paying_type = 2, tiêu 6$ thì paying_type = 5, lấy mốc cận dưới gần nhất.
                //Chỉ game có IAP
                //OnSetProperty("paying_type", 0);

                //===========================================================
                //[maxLevel]: update sau cùng các event liên quan tới level, giá trị là level lớn nhất user pass cộng thêm 1,
                //(giá trị ban đầu khi cài game lần đầu và chưa pass level nào là 1)
                // OnSetProperty("level", GameManager.ins.data.level + 1);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Firebase: (Userproperties) Error: " + e);
        }
    }

    private void OnSetProperty(string key, object value)
    {
        try
        {
            FirebaseAnalytics.SetUserProperty(key.ToString(), value.ToString());
        }
        catch (Exception e)
        {
            Debug.LogError("Lỗi UserProperty của Firebase: " + key + " _ " + e);
        }
    }
    #endregion

    #region Events

    //OPTION: không yêu cầu
    //Mỗi 30s chơi game (tổng thời gian chơi) tính là 1 checkpoint.
    //[name_game_cur]: tên game hiện tại đang chơi
    //[name_game_max]: tên game cao nhất đã chơi (nếu là các game có nhiều trò như octopus, nếu không thì 2 thông số sẽ là level đang chơi)
    public void check_point_time(int check_point, string name_game_cur, string name_game_max)
    {
        if (!Debug.isDebugBuild && !Application.isEditor)
        {
            if (check_point < 10)
            {
                FirebaseAnalytics.LogEvent("check_point_time_0" + check_point, new Parameter[]
                {
                    new Parameter("name_game_cur", name_game_cur),
                    new Parameter("name_game_max", name_game_max)
                });
            }
            else
            {
                FirebaseAnalytics.LogEvent("check_point_time_" + check_point, new Parameter[]
                {
                    new Parameter("name_game_cur", name_game_cur),
                    new Parameter("name_game_max", name_game_max)
                });
            }
        }
    }

    //Call mỗi khi start level
    //[check_point]: mốc level (level 0 -> mốc 1, level 1 -> mốc 2 ...)
    //[name_game_cur]: tên game hiện tại đang chơi
    public void check_point_start(int check_point, string name_game_cur)
    {
        if (!Debug.isDebugBuild && !Application.isEditor)
        {
            if (check_point < 10)
            {
                FirebaseAnalytics.LogEvent("check_point_start_0" + check_point, new Parameter[]
                {
                    new Parameter("name_game_cur", name_game_cur)
                });
            }
            else
            {
                FirebaseAnalytics.LogEvent("check_point_start_" + check_point, new Parameter[]
                {
                    new Parameter("name_game_cur", name_game_cur)
                });
            }
        }
    }

    //Call mỗi khi end level
    //[check_point]: mốc level (level 0 -> mốc 1, level 1 -> mốc 2 ...)
    //[name_game_cur]: tên game hiện tại đang chơi
    public void check_point_end(int check_point, string name_game_cur, bool isWin)
    {
        if (!Debug.isDebugBuild && !Application.isEditor)
        {
            if (check_point < 10)
            {
                FirebaseAnalytics.LogEvent("check_point_end_0" + check_point, new Parameter[]
                {
                    new Parameter("name_game_cur", name_game_cur),
                    new Parameter("is_win", isWin ? "win" : "lose")
                });
            }
            else
            {
                FirebaseAnalytics.LogEvent("check_point_end_" + check_point, new Parameter[]
                {
                    new Parameter("name_game_cur", name_game_cur),
                    new Parameter("is_win", isWin ? "win" : "lose")
                });
            }
        }
    }

    //[nameGame]: tên game đang chơi (nếu như octopus) / số level (nếu như huggy run)
    //...
    public void level_start(string nameGame)
    {
        if (!Debug.isDebugBuild && !Application.isEditor)
        {
            FirebaseAnalytics.LogEvent("level_start", new Parameter[]
            {
                new Parameter("level", nameGame)
            });
        }
    }

    //[nameGame]: tên game đang chơi (nếu như octopus) / số level (nếu như huggy run)
    //[timeplayed]: thời gian chơi
    public void level_complete(string nameGame, int timeplayed)
    {
        if (!Debug.isDebugBuild && !Application.isEditor)
        {
            FirebaseAnalytics.LogEvent("level_complete", new Parameter[]
            {
                new Parameter("level", nameGame),
                new Parameter("timeplayed", timeplayed.ToString())
            });
        }
    }

    //[nameGame]: tên game đang chơi (nếu như octopus) / số level (nếu như huggy run)
    //[failcount]: số lần fail của level 
    public void level_fail(string nameGame, int failcount)
    {
        if (!Debug.isDebugBuild && !Application.isEditor)
        {
            FirebaseAnalytics.LogEvent("level_fail", new Parameter[]
            {
                new Parameter("level", nameGame),
                new Parameter("failcount", failcount.ToString()),
            });
        }
    }

    //Loại tiền tệ kiếm được
    //[virtual_currency_name]: tên loại tiền tệ (gold, gem ...)
    //[amount]: số lượng
    //[source]: nguồn kiếm được (collect in game, view reward, buy with $ ...)
    public void earn_virtual_currency(string virtual_currency_name, int amount, string source)
    {
        if (!Debug.isDebugBuild && !Application.isEditor)
        {
            FirebaseAnalytics.LogEvent("earn_virtual_currency", new Parameter[]
            {
                new Parameter("virtual_currency_name", virtual_currency_name),
                new Parameter("value", amount),
                new Parameter("source", source)
               });
        }
    }

    //Loại tiền tệ tiêu thụ
    //[virtual_currency_name]: tên loại tiền tệ (gold, gem ...)
    //[amount]: số lượng
    //[source]: nguồn tiêu thụ (mua skin, hồi sinh, ...)
    public void spend_virtual_currency(string virtual_currency_name, int amount, string item_name)
    {
        if (!Debug.isDebugBuild && !Application.isEditor)
        {
            FirebaseAnalytics.LogEvent("spend_virtual_currency", new Parameter[]
            {
                new Parameter("virtual_currency_name", virtual_currency_name),
                new Parameter("value", amount),
                new Parameter("item_name", item_name)
               });
        }
    }

    #region Reward ADS
    //Khi hiện offer reward (hiển thị nút x2 reward ở endgame, hiển thị popup offer skin free ...)
    //[placement]: vị trí hiển thị (endgame, shop ...)
    //[level]: level hiển thị
    public void ads_reward_offer(string placement, int level)
    {
        if (!Debug.isDebugBuild && !Application.isEditor)
        {
            FirebaseAnalytics.LogEvent("ads_reward_offer", new Parameter[]
            {
                new Parameter("placement", placement),
                new Parameter("level", level)
            });
        }
    }

    //Khi click vào button reward
    //(điều kiện là đã load được ads -> nên block - làm mờ nút ads reward nếu không load đc ads hoặc ko có mạng)
    //[placement]: vị trí hiển thị (endgame, shop ...)
    //[level]: level hiển thị
    public void ads_reward_click(string placement)
    {
        if (!Debug.isDebugBuild && !Application.isEditor)
        {
            FirebaseAnalytics.LogEvent("ads_reward_click", new Parameter[]
            {
                new Parameter("placement", placement)
            });
        }
    }

    //Khi reward được show lên (hiển thị thành công)
    public void ads_reward_show(string placement)
    {
        if (!Debug.isDebugBuild && !Application.isEditor)
        {
            FirebaseAnalytics.LogEvent("ads_reward_show", new Parameter[]
            {
                new Parameter("placement", placement)
            });
        }
    }

    //Khi reward bị lỗi khi hiển thị 
    //[placement]: vị trí hiển thị (endgame, shop ...)
    //[errormsg]: tên lỗi: Error Message: Unknown,Offline,NoFill,InternalError,InvalidRequest,UnableToPrecached
    //[level]: level hiển thị
    public void ads_reward_fail(string placement, string errormsg)
    {
        if (!Debug.isDebugBuild && !Application.isEditor)
        {
            FirebaseAnalytics.LogEvent("ads_reward_fail", new Parameter[]
            {
                new Parameter("placement", placement),
                new Parameter("errormsg", errormsg)
            });
        }
    }

    //Khi reward có thể nhận thưởng
    //Call khi close reward ads hoặc event có thể nhận thưởng
    public void ads_reward_complete(string placement)
    {
        if (!Debug.isDebugBuild && !Application.isEditor)
        {
            FirebaseAnalytics.LogEvent("ads_reward_complete", new Parameter[]
            {
                new Parameter("placement", placement),
            });
        }
    }
    #endregion

    #region Inter ADS
    //Khi inter ads load thành công
    public void ads_inter_load()
    {
        if (!Debug.isDebugBuild && !Application.isEditor)
        {
            FirebaseAnalytics.LogEvent("ads_inter_load", new Parameter[] { });
        }
    }

    //Khi click vào ads inter
    public void ads_inter_click()
    {
        if (!Debug.isDebugBuild && !Application.isEditor)
        {
            FirebaseAnalytics.LogEvent("ads_inter_click", new Parameter[] { });
        }
    }

    //Khi ads inter hiển thị (trong sự kiện ads trả về -> chắc chắn sẽ hiện)
    public void ads_inter_show()
    {
        if (!Debug.isDebugBuild && !Application.isEditor)
        {
            FirebaseAnalytics.LogEvent("ads_inter_show", new Parameter[] { });
        }
    }

    //Khi ads inter hiển thị lỗi
    //Error Message: FailToLoad, Unavailable
    public void ads_inter_fail(string errormsg)
    {
        if (!Debug.isDebugBuild && !Application.isEditor)
        {
            FirebaseAnalytics.LogEvent("ads_inter_fail", new Parameter[]
            {
                new Parameter("errormsg", errormsg)
            });
        }
    }
    #endregion

    #region ADS RevenuePain
    /// <summary>
    /// Send theo event của Max Manager (Log doanh thu từ mỗi quảng cáo)
    /// </summary>
    /// <param name="data"></param>
    public void ADS_RevenuePain(ImpressionData data)
    {
        if (!Debug.isDebugBuild && !Application.isEditor)
        {
            Parameter[] AdParameters = {
             new Parameter("ad_platform", "applovin"),
             new Parameter("ad_source", data.NetworkName),
             new Parameter("ad_unit_name", data.AdUnitIdentifier),
             new Parameter("currency", "USD"),
             new Parameter("value", data.Revenue),
             new Parameter("placement", data.Placement),
             new Parameter("country_code", data.CountryCode),
             new Parameter("ad_format", data.AdFormat),
            };
            FirebaseAnalytics.LogEvent("ad_revenue_pain", AdParameters);
        }
    }
    #endregion

    #endregion

    #region Remote Config
    /// <summary>
    /// Setup Remote config
    /// </summary>
    /// <param name="completionHandler"></param>
    public void fetch(Action<bool> completionHandler)
    {
        try
        {
            var settings = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ConfigSettings;
            Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.SetConfigSettingsAsync(settings);

            var fetchTask = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(new TimeSpan(0));

            fetchTask.ContinueWith(task =>
            {
                is_remote_config_done = true;
                if (task.IsCanceled || task.IsFaulted)
                {
                    Debug.LogWarning("fetchTask Firebase Fail");
                    is_remote_config_success = false;
                    completionHandler(false);
                }
                else
                {
                    Debug.LogWarning("fetchTask Firebase Commplete");
                    Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ActivateAsync();
                    RefrectProperties();

                    completionHandler(true);
                }
            });
        }
        catch (Exception ex)
        {
            is_remote_config_done = true;
            Debug.Log(ex.ToString());
        }
    }

    /// <summary>
    /// Dữ liệu remote config
    /// </summary>
    private void RefrectProperties()
    {
        try
        {
            //AOA Setting Params
            AdsManager.Ins.AOA_FirstOpen = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("AOA_FirstOpen").BooleanValue;
            AdsManager.Ins.AOA_SessionStart = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("AOA_SessionStart").BooleanValue;
            AdsManager.Ins.AOA_SwitchApps = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("AOA_SwitchApps").BooleanValue;
            AdsManager.Ins.AOA_SwitchApps_Seconds = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("AOA_SwitchApps_Seconds").LongValue;
            AdsManager.Ins.isShowAdsInLv1_2 = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("isShowAdsInLv1_2").BooleanValue;

            is_remote_config_success = true;
        }
        catch (Exception ex)
        {
            Debug.Log("Error RefrectProperties: " + ex.Message);
        }
    }


    #endregion
}
