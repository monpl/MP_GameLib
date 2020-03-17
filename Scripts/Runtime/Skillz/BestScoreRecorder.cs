using System;
using System.Collections.Generic;
using System.Globalization;
using MPGameLib.Extensions;
using UnityEngine;

namespace MPGameLib.Skillz
{
    public enum BestScoreType
    {
        Daily,
        Weekly,
        AllTime,
    }

    public class BestScoreInfo
    {
        public DateTime recordedTime;
        public int score;
    }

    public class BestScoreRecorder
    {
        private static BestScoreRecorder _instance;
        public static BestScoreRecorder Instance => _instance ?? (_instance = new BestScoreRecorder());

        private const string PlayerPrefs_PREFIX = "MPGameLib_SKILLZ_SCOREREC_";
        private Dictionary<BestScoreType, BestScoreInfo> _bestScoreDictionary;

        #region Public Methods

        public void PreInit()
        {
            InitBestScoreDictionary();
        }

        public BestScoreInfo GetBestScoreInfo(BestScoreType scoreType)
        {
            return _bestScoreDictionary[scoreType];
        }

        public Dictionary<BestScoreType, BestScoreInfo> GetAllBestScoreInfos() => _bestScoreDictionary;

        /// <summary>
        /// 최고 점수를 입력하고, 최고 점수 타입들을 반환한다.
        /// </summary>
        /// <param name="newScore"></param>
        /// <returns></returns>
        public BestScoreType[] SetBestScoreAndGetBestScoreTypes(int newScore)
        {
            var ret = new List<BestScoreType>();

            foreach (var scoreInfoKvp in _bestScoreDictionary)
            {
                var scoreInfo = scoreInfoKvp.Value;
                if (newScore > scoreInfo.score)
                {
                    scoreInfo.score = newScore;
                    scoreInfo.recordedTime = DateTime.Now;

                    ret.Add(scoreInfoKvp.Key);
                    SaveToPrefs(scoreInfoKvp.Key, scoreInfo);
                }
            }

            return ret.ToArray();
        }

        #endregion

        #region Private Methods

        private void InitBestScoreDictionary()
        {
            _bestScoreDictionary = new Dictionary<BestScoreType, BestScoreInfo>();

            var ScoreTypes = Enum.GetValues(typeof(BestScoreType));
            foreach (var scoreType in ScoreTypes)
            {
                var curType = (BestScoreType) scoreType;
                var bestScoreInfo = GetScoreInfoInPrefs(curType);

                _bestScoreDictionary.Add(curType, bestScoreInfo);
                SaveToPrefs(curType, bestScoreInfo);
            }
        }

        private int FilteringByScoreType(BestScoreType scoreType, int oriScore, ref DateTime recordedTime)
        {
            switch (scoreType)
            {
                case BestScoreType.Daily:
                    if (recordedTime.Date < DateTime.Now.Date)
                    {
                        recordedTime = DateTime.Now;
                        return 0;
                    }

                    return oriScore;
                case BestScoreType.Weekly:
                    if (DateTime.Now.AlreadyPastWeek(recordedTime))
                    {
                        recordedTime = DateTime.Now;
                        return 0;
                    }

                    return oriScore;
                case BestScoreType.AllTime:
                    return oriScore;
                default:
                    throw new ArgumentOutOfRangeException(nameof(scoreType), scoreType, null);
            }
        }

        private BestScoreInfo GetScoreInfoInPrefs(BestScoreType scoreType, bool filtering = true)
        {
            var curScore = PlayerPrefs.GetInt(GetPrefsPath(scoreType) + "_SCORE", 0);
            var curRecordDate = DateTime.Parse(PlayerPrefs.GetString(GetPrefsPath(scoreType) + "_DATE",
                DateTime.Now.ToString(CultureInfo.InvariantCulture)));

            if (filtering)
                curScore = FilteringByScoreType(scoreType, curScore, ref curRecordDate);

            return new BestScoreInfo
            {
                recordedTime = curRecordDate,
                score = curScore,
            };
        }

        private void SaveToPrefs(BestScoreType scoreType, BestScoreInfo scoreInfo)
        {
            PlayerPrefs.SetInt(GetPrefsPath(scoreType) + "_SCORE", scoreInfo.score);
            PlayerPrefs.SetString(GetPrefsPath(scoreType) + "_DATE",
                scoreInfo.recordedTime.ToString(CultureInfo.InvariantCulture));

            PlayerPrefs.Save();
        }

        private string GetPrefsPath(BestScoreType scoreType) => PlayerPrefs_PREFIX + scoreType;

        #endregion
    }
}