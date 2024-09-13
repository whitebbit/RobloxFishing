using System;
using _3._Scripts.Player;
using UnityEngine;

namespace _3._Scripts.MiniGame
{
    public abstract class Fighter : MonoBehaviour
    {
        protected bool isFight;

        public virtual void StartFight()
        {
            isFight = true;
            Animator().SetBool("IsFight", true);
        }
        
        public virtual void OnStart(){}
        public virtual void OnEnd(){}
        public virtual void PutFish(){}
        public virtual void EndFishing(){}
        public abstract FighterData FighterData();
        protected abstract PlayerAnimator Animator();
        
    }
}