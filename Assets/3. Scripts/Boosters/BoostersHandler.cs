﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Singleton;
using _3._Scripts.UI;
using _3._Scripts.UI.Panels;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.Boosters
{
    public class BoostersHandler : Singleton<BoostersHandler>
    {
        [Tab("Buttons")] [SerializeField] private BoosterButtonSwitcher autoClickerButton;
        [SerializeField] private AutoFightBooster autoFightBooster;
        [SerializeField] private Transform slapBoosterView;
        [Tab("Debug")] [SerializeField] private List<BoosterState> boosters = new();

        public AutoFightBooster AutoFightBooster => autoFightBooster;
        private void ChangeBoosterState(string boosterName, bool state)
        {
            var booster = boosters.FirstOrDefault(b => b.name == boosterName);

            if (booster == null)
            {
                boosters.Add(new BoosterState
                {
                    name = boosterName,
                    state = state
                });
                return;
            }

            booster.state = state;
        }

        public bool GetBoosterState(string boosterName)
        {
            var booster = boosters.FirstOrDefault(b => b.name == boosterName);
            return booster?.state ?? false;
        }

        private void Start()
        {
            
            slapBoosterView.gameObject.SetActive(false);

            InitializeButtons();
        }

        private void InitializeButtons()
        {
            autoClickerButton.onActivateBooster += () => ChangeBoosterState("auto_clicker", true);
            autoClickerButton.onDeactivateBooster += () => ChangeBoosterState("auto_clicker", false);

            autoFightBooster.onActivateBooster += () =>
            {
                ChangeBoosterState("auto_fight", true);
            };
            autoFightBooster.onDeactivateBooster += () =>
            {
                ChangeBoosterState("auto_fight", false);
                StopAllCoroutines();
            };
            
        }
    }
}