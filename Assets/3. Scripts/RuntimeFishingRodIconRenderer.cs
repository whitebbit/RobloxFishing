using System.Collections.Generic;
using _3._Scripts.Player;
using _3._Scripts.Singleton;
using _3._Scripts.UI.Scriptable.Shop;
using UnityEngine;

namespace _3._Scripts
{
    public class RuntimeFishingRodIconRenderer : Singleton<RuntimeFishingRodIconRenderer>
    {
        public Camera renderCamera;
        public RenderTexture renderTexture;
        public FishingRod fishingRod;
 
        private readonly Dictionary<string, Texture2D> _textureCache = new();

        public Texture2D GetTexture2D(UpgradeItem data)
        {
            return /*_textureCache.ContainsKey(data.ID) ? _textureCache[data.ID] :*/ CreateTexture2D(data);
        }
        
        private Texture2D CreateTexture2D(UpgradeItem data)
        {
            fishingRod.Initialize(data);
            
            renderCamera.clearFlags = CameraClearFlags.SolidColor;
            renderCamera.backgroundColor = new Color(0, 1, 0, 1); // Полностью прозрачный фон
            
            var tempRT = new RenderTexture(renderTexture.width, renderTexture.height, 24, RenderTextureFormat.ARGB32);
            renderCamera.targetTexture = tempRT;

            renderCamera.Render();

            RenderTexture.active = tempRT;
            var renderedTexture = new Texture2D(tempRT.width, tempRT.height, TextureFormat.RGBA32, false);
            renderedTexture.ReadPixels(new Rect(0, 0, tempRT.width, tempRT.height), 0, 0);
            renderedTexture.Apply();
            
            RenderTexture.active = null;
            renderCamera.targetTexture = null;
            tempRT.Release();
            
            var pixels = renderedTexture.GetPixels32();
            for (var i = 0; i < pixels.Length; i++)
            {
                var pixel = pixels[i];
                if (pixel.g <= pixel.r + 100 || pixel.g <= pixel.b + 100) continue;
                pixel.a = 0; 
                pixels[i] = pixel;
            }

            renderedTexture.SetPixels32(pixels);
            renderedTexture.Apply();
            
            _textureCache.TryAdd(data.ID, renderedTexture);
            
            return renderedTexture;
        }
    }
}