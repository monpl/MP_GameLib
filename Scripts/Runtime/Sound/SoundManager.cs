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

    /// <summary>
    /// 배경, 효과음을 담당하는 사운드 관리자
    /// </summary>
    public class SoundManager : Singleton<SoundManager>
    {
        private AudioSource _bgmSource;
        private AudioSource _sfxSource;

        private Dictionary<string, AudioClip> _effectDic;
        private Dictionary<string, AudioClip> _bgmDic;
        private Queue<AudioClip> _delaySfxQueue;
        
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
                SaveToPrefs(SoundType.Sfx, _isVibrateOn);
            }
        }

        /// <summary>
        /// 사운드 매니저 초기화
        /// </summary>
        /// <param name="defaultUserBgmOn">BGM 기본값</param>
        /// <param name="defaultUserSfxOn">효과음 기본값</param>
        /// <param name="defaultUserVibrateOn">진동 기본값</param>
        /// <param name="sfxRoot">효과음 폴더 Root</param>
        /// <param name="bgmRoot">BGM폴더 Root</param>
        /// <param name="defaultBgmName">기본 BGM 파일 이름</param>
        public void PreInit(bool defaultUserBgmOn, bool defaultUserSfxOn, bool defaultUserVibrateOn, string sfxRoot, string bgmRoot, string defaultBgmName)
        {
            if (_isPreInit)
                return;

            CreateSoundSources();

            _delaySfxQueue = new Queue<AudioClip>();
            _effectDic = new Dictionary<string, AudioClip>();
            _bgmDic = new Dictionary<string, AudioClip>();

            SettingSfx(sfxRoot);
            SettingBgm(bgmRoot, defaultBgmName);
            _isPreInit = true;

            IsBgmOn = GetPrefsSoundInfo(SoundType.Bgm, defaultUserBgmOn);
            IsSfxOn = GetPrefsSoundInfo(SoundType.Sfx, defaultUserSfxOn);
            IsVibrateOn = GetPrefsSoundInfo(SoundType.Vibrate, defaultUserVibrateOn);
        }

        private void CreateSoundSources()
        {
            _bgmSource = new GameObject("BgmSource").AddComponent<AudioSource>();
            _sfxSource = new GameObject("SfxSource").AddComponent<AudioSource>();
            
            _bgmSource.transform.SetParent(transform);
            _sfxSource.transform.SetParent(transform);
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
                _sfxSource.PlayOneShot(sfxClip);
            }
            else
            {
                _delaySfxQueue.Enqueue(sfxClip);
                Invoke(nameof(DelayPlayEffect), delay);
            }
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

            _sfxSource.loop = false;
            _sfxSource.Stop();
        }

        private void DelayPlayEffect()
        {
            if (!_isPreInit)
            {
                Debug.LogError("You need to call PreInit()..!");
                return;
            }

            if (!IsSfxOn || _delaySfxQueue.Count == 0)
                return;

            var sfxClip = _delaySfxQueue.Dequeue();
            _sfxSource.PlayOneShot(sfxClip);
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

        private void SaveToPrefs(SoundType soundType, bool isOn)
        {
            PlayerPrefs.SetInt(PlayerPrefs_PREFIX + soundType, isOn ? 1 : 0);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (!_isPreInit)
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