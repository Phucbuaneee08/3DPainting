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

        [SerializeField] AudioClip completeColumn;
        [SerializeField] AudioClip fireWork;
        [SerializeField] AudioClip loseSound;
        [SerializeField] AudioClip winSound;
        [SerializeField] AudioClip mahjongFalling_1;
        [SerializeField] AudioClip mahjongFalling_2;
        [SerializeField] AudioClip mahjongIn;
        [SerializeField] AudioClip mahjongOut;
        [SerializeField] AudioClip moveFail;
        [SerializeField] AudioClip rankItemRollSound;
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

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

//        public void MuteSound()
//        {
//            DataManager.Instance.GameData.sound = false;
//            SFXSource.mute = !DataManager.Instance.GameData.sound;
//        }
//        public void MuteMusic()
//        {
//            DataManager.Instance.GameData.music = false;
//            musicSource.mute = !DataManager.Instance.GameData.music;
//        }
//        public void ResumSound()
//        {
//            DataManager.Instance.GameData.sound = true;
//            SFXSource.mute = !DataManager.Instance.GameData.sound;
//        }
//        public void ResumeMusic()
//        {
//            DataManager.Instance.GameData.music = true;
//            musicSource.mute = !DataManager.Instance.GameData.music;
//        }
//        public void VibrateDevice(bool fastVibrate = false)
//        {
//            if (DataManager.Instance.GameData.vibrationEnabled)
//            {
//                if (!fastVibrate)
//                {
//#if UNITY_ANDROID
//                    Vibration.Vibrate(100, 50);
//#else
//        Handheld.Vibrate();
//#endif
//                }
//                else
//                {
//#if UNITY_ANDROID
//                    Vibration.Vibrate(40, 40);
//#else
//        Handheld.Vibrate();
//#endif

//                }
//            }
//        }

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
        public void OnCompleteColumn()
        {
            PlaySound(completeColumn);
        }
        public void OnGetCoins()
        {
            PlaySound(GetCoins);
        }
        public void OnGetMultiCoins(int numberOfCoins)
        {
            for (int i = 0; i < numberOfCoins; i++)
            {
                OnGetCoins();
            }
        }

        public void OnFilledCube()
        {
            PlaySound(filledCube);
        }
        public void OnFireWork()
        {
            PlaySound(fireWork);
        }
        public void OnLose()
        {
            PlaySound(loseSound);
        }
        public void OnMahjongFalling1()
        {
            PlaySound(mahjongFalling_1);
        }
        public void OnMahjongFalling2()
        {
            PlaySound(mahjongFalling_2);
        }
        public void OnMahjongPutIn()
        {
            PlaySound(mahjongIn);
        }
        public void OnMahjongPutOut()
        {
            PlaySound(mahjongOut);
        }
        public void OnMoveFail()
        {
            PlaySound(moveFail);
        }
        public void OnRankItemRoll()
        {
            PlaySound(rankItemRollSound);
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