using System;
using System.Collections.Generic;
using UnityEngine;
using MP_Patterns;
using DG.Tweening;

namespace MPGameLib.Sound
{
    public enum SoundType
    {
        Bgm,
        Sfx,
        Vibrate,
    }
    public enum BgmAction
    {
        Play,
        Pause,
        Resume,
        Stop,
    }

    public class DelaySoundInfo
    {
        public AudioClip clip;
        public float removeUnityTime;
        public bool playedSound = false;
        public bool isForce;
    }

    /// <summary>
    /// 배경, 효과음을 담당하는 사운드 관리자
    /// </summary>
    public class SoundManager : Singleton<SoundManager>
    {
        private AudioSource _bgmSource;
        private AudioSource _sfxSource;
        private AudioSource _forceSfxSource;

        private Dictionary<string, AudioClip> _effectDic;
        private Dictionary<string, AudioClip> _bgmDic;
        private List<DelaySoundInfo> _delaySfxList;
        private Dictionary<float, AudioClip> _audioClipDic;
        
        private float _bgmVolume;
        private float _sfxVolume;

        private const string PlayerPrefs_PREFIX = "MPGameLib_SKILLZ_SOUNDMGR_";
        private bool _isPreInit;

        private bool _isBgmOn;
        public bool IsBgmOn
        {
            get => _isBgmOn;
            set
            {
                _isBgmOn = value;
                BgmAction(_isBgmOn ? Sound.BgmAction.Play : Sound.BgmAction.Stop);
                SaveToPrefs(SoundType.Bgm, _isBgmOn);
            }
        }
        
        private bool _isSfxOn;
        public bool IsSfxOn
        {
            get => _isSfxOn;
            set
            {
                _isSfxOn = value;
                SaveToPrefs(SoundType.Sfx, _isSfxOn);
            }
        }
        
        private bool _isVibrateOn;
        public bool IsVibrateOn
        {
            get => _isVibrateOn;
            set
            {
                _isVibrateOn = value;
                SaveToPrefs(SoundType.Vibrate, _isVibrateOn);
            }
        }
        public bool IgnorePause { get; set; }

        /// <summary>
        /// 사운드 매니저 초기화
        /// </summary>
        /// <param name="defaultUserBgmOn">BGM 기본값</param>
        /// <param name="defaultUserSfxOn">효과음 기본값</param>
        /// <param name="defaultUserVibrateOn">진동 기본값</param>
        /// <param name="sfxRoot">효과음 폴더 Root</param>
        /// <param name="bgmRoot">BGM폴더 Root</param>
        /// <param name="defaultBgmName">기본 BGM 파일 이름</param>
        /// <param name="defaultBgmVolume">bgm 사운드 크기</param>
        /// <param name="defaultSfxVolume">sfx 사운드 크기</param>
        public void PreInit(
            bool defaultUserBgmOn = true, bool defaultUserSfxOn = true, bool defaultUserVibrateOn = true,
            string sfxRoot = "Sounds/SFX", string bgmRoot = "Sounds/BGM",
            string defaultBgmName = "Main",
            float defaultBgmVolume = 1f, float defaultSfxVolume = 1f)
        {
            if(!_isPreInit)
                CreateSoundSources(defaultBgmVolume, defaultSfxVolume);

            _delaySfxList = new List<DelaySoundInfo>();
            _effectDic = new Dictionary<string, AudioClip>();
            _bgmDic = new Dictionary<string, AudioClip>();

            SettingSfx(sfxRoot);
            SettingBgm(bgmRoot, defaultBgmName);
            _isPreInit = true;

            IsBgmOn = GetPrefsSoundInfo(SoundType.Bgm, defaultUserBgmOn);
            IsSfxOn = GetPrefsSoundInfo(SoundType.Sfx, defaultUserSfxOn);
            IsVibrateOn = GetPrefsSoundInfo(SoundType.Vibrate, defaultUserVibrateOn);
        }

        private void CreateSoundSources(float defaultBgmValue, float defaultSfxValue)
        {
            _bgmSource = new GameObject("BgmSource").AddComponent<AudioSource>();
            _sfxSource = new GameObject("SfxSource").AddComponent<AudioSource>();
            _forceSfxSource = new GameObject("SfxForceSource").AddComponent<AudioSource>();
            
            _bgmSource.transform.SetParent(transform);
            _sfxSource.transform.SetParent(transform);
            _forceSfxSource.transform.SetParent(transform);
            
            _bgmVolume = GetPrefsVolume(SoundType.Bgm, defaultBgmValue);
            _sfxVolume = GetPrefsVolume(SoundType.Sfx, defaultSfxValue);

            _bgmSource.volume = _bgmVolume;
            _sfxSource.volume = _sfxVolume;
            _forceSfxSource.volume = Mathf.Clamp(_sfxVolume, 0.4f, 1f);
        }

        private void SettingSfx(string sfxRoot)
        {
            var sfxList = Resources.LoadAll<AudioClip>(sfxRoot);

            foreach (var sfx in sfxList)
                _effectDic.Add(sfx.name, sfx);
        }

        private void SettingBgm(string bgmRoot, string defaultBgmName)
        {
            _bgmSource.loop = true;

            var bgmList = Resources.LoadAll<AudioClip>(bgmRoot);

            foreach (var bgm in bgmList)
                _bgmDic.Add(bgm.name, bgm);

            if (_bgmDic.ContainsKey(defaultBgmName))
                _bgmSource.clip = _bgmDic[defaultBgmName];
            else
                _bgmSource.clip = _bgmDic[bgmList[0].name];
        }

        private void Update()
        {
            if (!_isPreInit || _delaySfxList.Count == 0)
                return;
            
            foreach (var delayInfo in _delaySfxList)
            {
                if (delayInfo.removeUnityTime > Time.time) 
                    continue;
                
                if (IsSfxOn)
                    _sfxSource.PlayOneShot(delayInfo.clip);
                else if (delayInfo.isForce)
                    _forceSfxSource.PlayOneShot(delayInfo.clip);

                delayInfo.playedSound = true;
            }
            
            _delaySfxList.RemoveAll(obj => obj.playedSound);
        }

        /// <summary>
        /// BGM을 컨트롤 하는 함수
        /// </summary>
        /// <param name="action">사용할 액션</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void BgmAction(BgmAction action)
        {
            if (!_isPreInit)
            {
                Debug.LogError("You need to call PreInit()..!");
                return;
            }
            
            _bgmSource.DOKill();
            _bgmSource.volume = _bgmVolume;

            switch (action)
            {
                case Sound.BgmAction.Play:
                    if (IsBgmOn)
                        _bgmSource.Play();
                    break;
                case Sound.BgmAction.Pause:
                    _bgmSource.Pause();
                    break;
                case Sound.BgmAction.Resume:
                    if (IsBgmOn)
                        _bgmSource.UnPause();
                    break;
                case Sound.BgmAction.Stop:
                    _bgmSource.Stop();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
        
        public void PauseWithFade(bool isPause, float fadeTime)
        {
            _bgmSource.DOKill();
            _bgmSource.DOFade(isPause ? 0f : _bgmVolume, fadeTime).OnComplete(() =>
            {
                if (isPause)
                    _bgmSource.Pause();
                else
                {
                    if(IsBgmOn)
                        _bgmSource.UnPause();
                }
            });
        }

        public void StopWithFade(bool isStop, float fadeTime)
        {
            _bgmSource.DOKill();
            _bgmSource.DOFade(isStop ? 0f : _bgmVolume, fadeTime).OnComplete(() =>
            {
                if (isStop)
                    _bgmSource.Stop();
                else
                {
                    if (IsBgmOn)
                        _bgmSource.Play();
                }
            });
        }

        /// <summary>
        /// BGM을 바꾼다.
        /// </summary>
        /// <param name="newBgmName">바꿀 BGM</param>
        public void ChangeBgm(string newBgmName)
        {
            if (!_isPreInit)
            {
                Debug.LogError("You need to call PreInit()..!");
                return;
            }

            if (_bgmDic.ContainsKey(newBgmName) == false)
            {
                Debug.LogError($"This bgm name is not exist!, name: {newBgmName}");
                return;
            }

            _bgmSource.clip = _bgmDic[newBgmName];
            _bgmSource.Play();
        }

        
        public void SetBgmVolume(float newVolume)
        {
            _bgmSource.volume = newVolume;
            _bgmVolume = newVolume;

            SaveVolume(SoundType.Bgm, newVolume);
        }
        
        public void SetSfxVolume(float newVolume)
        {
            _sfxSource.volume = newVolume;
            _sfxVolume = newVolume;
            
            SaveVolume(SoundType.Sfx, newVolume);
        }

        /// <summary>
        /// BGM Pitch를 설정한다.
        /// </summary>
        /// <param name="pitch">pitch</param>
        /// <param name="time">Fade 시간</param>
        public void SetBGMPitch(float pitch, float time = 0.2f)
        {
            _bgmSource.DOKill();
            _bgmSource.DOPitch(pitch, time);
        }
        
        /// <summary>
        /// SFX Pitch를 설정한다.
        /// </summary>
        /// <param name="pitch">pitch</param>
        /// <param name="time">Fade 시간</param>
        public void SetSFXPitch(float pitch, float time = 0.2f)
        {
            _bgmSource.DOKill();
            _sfxSource.DOPitch(pitch, time);
        }

        /// <summary>
        /// 효과음을 실행한다.
        /// </summary>
        /// <param name="sfxName">효과음 이름</param>
        /// <param name="delay">딜레이</param>
        /// <param name="isForce">강제로 사운드를 재생할건지</param>
        public void PlayEffect(string sfxName, float delay = 0f, bool isForce = false)
        {
            if (!_isPreInit)
            {
                Debug.LogError("You need to call PreInit()..!");
                return;
            }

            if (_effectDic.ContainsKey(sfxName) == false)
                return;
            
            if (IsSfxOn == false && isForce == false)
                return;

            var sfxClip = _effectDic[sfxName];
            if (delay <= 0f)
            {
                // TODO: Bug, 같은 코드라인데 PlayEffect에 딜레이를 높은걸 먼저 걸고, 낮은걸 나중에 걸면 Queue에 쌓이기 때문에 순서가 바뀌어서 나오게 됨
                if(isForce)
                    _forceSfxSource.PlayOneShot(sfxClip);
                else
                    _sfxSource.PlayOneShot(sfxClip);
            }
            else
            {
                AddDelaySoundList(sfxClip, delay, isForce);
            }
        }

        private void AddDelaySoundList(AudioClip clip, float delay, bool isForce)
        {
            _delaySfxList.Add(new DelaySoundInfo
            {
                clip = clip,
                removeUnityTime = Time.time + delay,
                playedSound = false,
                isForce = isForce
            });
        }

        /// <summary>
        /// 루프를 킨 상태로 효과음을 재생한다.
        /// </summary>
        /// <param name="sfxName">효과음 이름</param>
        public void PlayEffectInLoop(string sfxName)
        {
            if (!_isPreInit)
            {
                Debug.LogError("You need to call PreInit()..!");
                return;
            }

            if (!IsSfxOn || !_effectDic.ContainsKey(sfxName))
                return;

            _sfxSource.loop = true;
            _sfxSource.clip = _effectDic[sfxName];
            _sfxSource.Play();
        }

        /// <summary>
        /// 효과음의 Loop를 해제한다.
        /// </summary>
        public void StopEffectInLoop()
        {
            if (!_isPreInit)
            {
                Debug.LogError("You need to call PreInit()..!");
                return;
            }
            
            // TODO: Looping이 돌아가는 사운드들은 sfxSource에 말고 다른 Source에서 관리하도록 변경해보자
            // 지금은 임시방편
            if (!_sfxSource.loop)
                return;

            _sfxSource.loop = false;
            _sfxSource.Stop();
        }
        
        public void PlayVibrate()
        {
            if (IsVibrateOn == false)
                return;
            
            Handheld.Vibrate();
        }

        private bool GetPrefsSoundInfo(SoundType soundType, bool defaultOn)
        {
            return PlayerPrefs.GetInt(PlayerPrefs_PREFIX + soundType, defaultOn ? 1 : 0) == 1;
        }

        private float GetPrefsVolume(SoundType soundType, float defaultVal)
        {
            return PlayerPrefs.GetFloat(PlayerPrefs_PREFIX + soundType + "_Volume", defaultVal);
        }

        private void SaveToPrefs(SoundType soundType, bool isOn)
        {
            PlayerPrefs.SetInt(PlayerPrefs_PREFIX + soundType, isOn ? 1 : 0);
        }

        private void SaveVolume(SoundType soundType, float val)
        {
            PlayerPrefs.SetFloat(PlayerPrefs_PREFIX + soundType + "_Volume", val);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (!_isPreInit || IgnorePause)
                return;

            if (pauseStatus)
            {
                // background
                if (_sfxSource.isPlaying && IsSfxOn)
                    _sfxSource.Pause();

                if (_bgmSource.isPlaying && IsBgmOn)
                    _bgmSource.Pause();
            }
            else
            {
                if (IsSfxOn)
                    _sfxSource.UnPause();

                if (IsBgmOn)
                    _bgmSource.UnPause();
            }
        }
    }
}