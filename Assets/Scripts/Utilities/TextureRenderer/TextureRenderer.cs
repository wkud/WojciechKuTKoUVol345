using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEngine;
#endif

namespace TKOU.Utilities
{
    /// <summary>
    /// Renders texture using a camera.
    /// </summary>
    public class TextureRenderer : MonoBehaviour
    {
#if UNITY_EDITOR
        private RenderTexture renderTexture;

        [SerializeField]
        private FolderReference outputFolder;

        [SerializeField]
        private Camera camera;

        [SerializeField]
        private int width = 512;

        [SerializeField]
        private int height = 512;

        private const int maxDepth = 32;

        [ContextMenu(nameof(RenderToTexture))]
        public void RenderToTexture()
        {
            if (!Application.isPlaying)
            {
                Debug.Log($"{nameof(TextureRenderer)} works only in play mode.");
                return;
            }

            UpdateRenderTexture();

            Texture2D tex = RenderCameraToTexture();

            ExportTextureToFolder(tex, outputFolder);

            Destroy(tex);
        }

        private void UpdateRenderTexture()
        {
            bool shouldUpdate = false;

            if(renderTexture == null)
            {
                shouldUpdate = true;
            }
            else if(renderTexture.width != width || renderTexture.height != height)
            {
                shouldUpdate = true;
            }

            if (shouldUpdate)
            {
                renderTexture = new RenderTexture(width, height, maxDepth);
            }
        }

        private Texture2D RenderCameraToTexture()
        {
            camera.targetTexture = renderTexture;

            camera.Render();

            camera.targetTexture = null;

            Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = renderTexture;
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();
            RenderTexture.active = currentRT;

            return tex;
        }

        private void ExportTextureToFolder(Texture2D texture, FolderReference folder)
        {
            byte [] fileBytes = texture.EncodeToPNG();
            DateTime dateTime = DateTime.Now;
            string filePath = $"{folder.Path}/TextureRender-{dateTime.Hour}_{dateTime.Minute}_{dateTime.Second}_{dateTime.Millisecond}.png";

            try
            {
                System.IO.File.WriteAllBytes(filePath, fileBytes);
            }
            catch (IOException e)
            {
                Debug.LogError("Failed to export texture to folder : " + e.Message);
            }
        }
#endif
    }

}
