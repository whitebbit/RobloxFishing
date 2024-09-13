using System;
using System.Collections.Generic;

namespace _3._Scripts.Saves
{
    [Serializable]
    public class CatchSave
    {
        public event Action OnCatchListUpdate;
        public Dictionary<string, List<string>> catchList = new();

        public bool CatchUnlocked(string pondID, string catchID)
        {
            return catchList.ContainsKey(pondID) && catchList[pondID].Contains(catchID);
        }

        public void AddCatch(string pondID, string catchID)
        {
            if (catchList.ContainsKey(pondID))
            {
                if (catchList[pondID].Contains(catchID))
                    return;
                catchList[pondID].Add(catchID);
            }
            else
            {
                catchList.Add(pondID, new List<string>());
                catchList[pondID].Add(catchID);
            }

            OnCatchListUpdate?.Invoke();
        }
    }
}