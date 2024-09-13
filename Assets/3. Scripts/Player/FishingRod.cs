using System;
using System.Collections.Generic;
using _3._Scripts.UI.Scriptable.Shop;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Player
{
    public class FishingRod : MonoBehaviour
    {
        [SerializeField] private Transform fishingLineStartPoint;

        [Tab("View")] [SerializeField] private Transform model;
        [SerializeField] private MeshRenderer mainRenderer;
        [SerializeField] private List<MeshRenderer> secondRenderers = new();

        public Transform FishingLineStartPoint => fishingLineStartPoint;

        public void Initialize(UpgradeItem item)
        {
            mainRenderer.materials = new[]
            {
                item.SecondMaterial,
                item.MainMaterial
            };

            foreach (var r in secondRenderers)
            {
                var mat = new Material[r.materials.Length];
                for (var i = 0; i < r.materials.Length; i++)
                {
                    mat[i] = item.SecondMaterial;
                }

                r.materials = mat;
            }
        }


        public void SetState(bool state)
        {
            model.gameObject.SetActive(state);
        }
    }
}