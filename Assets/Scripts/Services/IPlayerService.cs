namespace Services
{
    public interface IPlayerService
    {
        int GetLvl(int key);
        
        void SetLvl(int key, int value);

        bool GetStateBooster(string key);
        
        void SaveBooster(string key);

        float GetCurrency();
        
        void SetCurrency(float value);
        
        float GetProgress(int key);
        
        void SetProgress(int key, float value);
    }
}