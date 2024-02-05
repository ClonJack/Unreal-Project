using System;
using SaveData;

namespace Services.Save
{
    public class SaveService
    {
        private readonly ISaveStorage<PlayerProgress> _saveStorage;

        public PlayerProgress Progress { get; private set; }
        
        public event Action ProgressLoaded;
        public event Action ProgressSaved;


        public SaveService(ISaveStorage<PlayerProgress> saveStorage)
        {
            _saveStorage = saveStorage;
        }

        public void Save()
        {
            _saveStorage.Save(Progress);
            ProgressSaved?.Invoke();
        }

        public void Load()
        {
            Progress = _saveStorage.Load() ?? new PlayerProgress();
            ProgressLoaded?.Invoke();
        }
    }
}