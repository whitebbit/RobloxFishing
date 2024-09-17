using System;
using System.Collections.Generic;
using System.Linq;

namespace _3._Scripts.Saves
{
    [Serializable]
    public class CatchSave
    {
        public event Action OnCatchListUpdate;
        public List<string> catchList = new();

        public bool CatchUnlocked(int stageID, string catchID)
        {
            return catchList.Contains(CatchID(stageID, catchID));
        }

        public void AddCatch(int stageID, string catchID)
        {
            if (!CatchUnlocked(stageID, catchID))
                catchList.Add(CatchID(stageID, catchID));

            OnCatchListUpdate?.Invoke();
        }

        public bool CatchIsCaught(string catchID)
        {
            return catchList.Any(c => c.Contains(catchID));
        }

        private string CatchID(int stageID, string catchID)
        {
            return $"{stageID}_{catchID}";
        }
    }
}