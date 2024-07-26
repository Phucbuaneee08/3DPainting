using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
namespace Paint3D
{
    public class AudioManager : Singleton<AudioManager>
    {
        [Header("----------- Audio Source ------------")]
        public AudioSource musicSource;
        public AudioSource SFXSource;
        [Header("----------- Audio Clip ------------")]
        [SerializeField] AudioClip bgmHome;
        [SerializeField] AudioClip bgmInGame;
        [SerializeField] AudioClip filledCube;
        [SerializeField] AudioClip GetCoins;
        [SerializeField] AudioClip clickBtn;
        [SerializeField] AudioClip winSound;



        [SerializeField] AudioClip reward;
        [SerializeField] AudioClip rewardSpin;
        [SerializeField] AudioClip spin;
        //public AudioClip Buy;
        //public AudioClip TileDrop;
        //public AudioClip CollectTilesSound;
        // public AudioClip RankItemPopupSound;
        // public AudioClip PopupSound;
        // public AudioClip PixelSound;
        // public AudioClip SelectTileSound;
        private float lastPlayTime;
        public float cooldown = 0.05f;
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
           
        }
        public void MuteSound()
        {
            DataManager.Ins.playerData.sound = false;
            DataManager.Ins.SaveData();
            SFXSource.mute = true;
        }

        public void ResumeSound()
        {
            DataManager.Ins.playerData.sound = true;
            DataManager.Ins.SaveData();
            SFXSource.mute = false;
        }

        public void MuteMusic()
        {
            DataManager.Ins.playerData.music = false;
            DataManager.Ins.SaveData();
            musicSource.mute = true;
        }

        public void ResumeMusic()
        {
            DataManager.Ins.playerData.music = true;
            DataManager.Ins.SaveData();
            musicSource.mute = false;
        }

        public void VibrateDevice(bool fastVibrate = false)
        {
            if (DataManager.Ins.playerData.vibrationEnabled)
            {
                if (!fastVibrate)
                {
#if UNITY_ANDROID
                    long[] pattern = { 0, 100, 50 };
                    Vibration.Vibrate(pattern, -1);
#else
                    Handheld.Vibrate();
#endif
                }
                else
                {
#if UNITY_ANDROID
                    long[] pattern = { 0, 40, 40 };
                    Vibration.Vibrate(pattern, -1);
#else
                    Handheld.Vibrate();
#endif
                }
            }
        }

        public void RefreshVolumeStatus()
        {
            float volume = PlayerPrefs.GetFloat("Volume", 1.0f);
            musicSource.volume = volume;
            SFXSource.volume = volume;
        }

        public void PlaySound(AudioClip audioClip)
        {
            SFXSource.PlayOneShot(audioClip);
        }
        public void PlaySoundBackgound(AudioClip audioClip)
        {
            musicSource.clip = audioClip;
            musicSource.Play();
            musicSource.loop = true;
        }
        public void StopSoundBackgound()
        {
            musicSource.Stop();
        }
        public void OnPlayHomeMusic()
        {
            PlaySoundBackgound(bgmHome);
        }
        public void OnPlayInGameMusic()
        {
            PlaySoundBackgound(bgmInGame);
        }
        public void OnClick()
        {
            PlaySound(clickBtn);
        }
      
        public void OnGetCoins()
        {
            PlaySound(GetCoins);
        }
        public void OnGetMultiCoins(int numberOfCoin)
        {
            
             StartCoroutine(OnGetCoin(numberOfCoin));
            
        }
        public IEnumerator OnGetCoin(int numberOfCoin)
        {
            float duration = 0.8f;
            float i = 0;
            while (i < duration)
            {
                i += duration/numberOfCoin;
                OnGetCoins();
                yield return new WaitForSeconds(duration/numberOfCoin);
            }
       
        }

        public void OnFilledCube()
        {
            if (Time.time - lastPlayTime >= cooldown)
            {
                PlaySound(filledCube);
                lastPlayTime = Time.time;
            }
        }
        public IEnumerator OnPlayFillCube()
        {
          
            yield return new WaitForEndOfFrame();
            PlaySound(filledCube);
        }
        
        
      
        public void OnReward()
        {
            PlaySound(reward);
        }
        public void OnRewardSpin()
        {
            PlaySound(rewardSpin);
        }
        public void OnSpin()
        {
            PlaySound(spin);
        }
        public void OnWin()
        {
            PlaySound(winSound);
        }

        public void Oninit()
        {
            SFXSource.mute = !DataManager.Ins.playerData.sound;
            musicSource.mute = !DataManager.Ins.playerData.music;
            VibrateDevice(!DataManager.Ins.playerData.music);
        }

        //public void OnSelectTile()
        //{
        //    PlaySound(SelectTileSound);
        //}

        //public void OnBuy()
        //{
        //    PlaySound(Buy);
        //}
        //public void OnCollectTiles()
        //{
        //    PlaySound(CollectTilesSound);
        //}
        //public void PrepareRankItemRoll()
        //{
        //    PlaySound(rankItemRollSound);
        //}
        //public void OnPopup()
        //{
        //    PlaySound(PopupSound);
        //}
        //public void OnPixelFit()
        //{
        //    PlaySound(PixelSound);
        //}
    }
}