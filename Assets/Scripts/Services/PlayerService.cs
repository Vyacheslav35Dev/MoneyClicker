using UnityEngine;

namespace Services
{
    public class PlayerService : IPlayerService
    {
        private const string PrefixLevel = "Level_";
        private const string Level1 = "Level_1";
        private const string PrefixProgress = "Progress_";
        private const string Currency = "Currency";
        public int GetLvl(int key)
        {
            var lvl = PlayerPrefs.GetInt(PrefixLevel + key, 0);
            if (PrefixLevel + key == Level1 && lvl == 0)
            {
                PlayerPrefs.SetInt(Level1, 1);
                PlayerPrefs.Save();
                return 1;
            }
            return lvl;
        }

        public void SetLvl(int key, int value)
        {
            PlayerPrefs.SetInt(PrefixLevel + key, value);
            PlayerPrefs.Save();
        }
        
        public bool GetStateBooster(string key)
        {
            return PlayerPrefs.GetInt(key, 0) == 1;
        }
        
        public void SaveBooster(string key)
        {
            PlayerPrefs.SetInt(key, 1);
            PlayerPrefs.Save();
        }

        public float GetCurrency()
        {
            return PlayerPrefs.GetFloat(Currency, 0);
        }
        
        public void SetCurrency(float value)
        {
            PlayerPrefs.SetFloat(Currency, value);
            PlayerPrefs.Save();
        }
        
        public float GetProgress(int key)
        {
            return PlayerPrefs.GetFloat(PrefixProgress + key, 0);
        }

        public void SetProgress(int key, float value)
        {
            PlayerPrefs.SetFloat(PrefixProgress + key, value);
            PlayerPrefs.Save();
        }
    }
}
