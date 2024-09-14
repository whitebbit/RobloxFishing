using System;
using System.Collections.Generic;

namespace _3._Scripts.Saves
{
    [Serializable]
    public class RangSave
    {
        public Dictionary<string, bool> rangState = new();

        public bool GetState(string rangID)
        {
            return rangState.ContainsKey(rangID) && rangState[rangID];
        }

        public void SetState(string rangID, bool state)
        {
            rangState.Add(rangID, state);
        }
    }
}