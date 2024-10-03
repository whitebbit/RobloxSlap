using System;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Wallet;
using DG.Tweening;
using GBGamesPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using VInspector;
using Random = UnityEngine.Random;

namespace _3._Scripts.Debugger
{
    public class Debugger : MonoBehaviour
    {
        [Tab("Enable Button")] [SerializeField]
        private Button enableButton;

        [SerializeField] private Transform enableArrow;
        [SerializeField] private Transform console;
        
        [Tab("Quality")] 
        [SerializeField] private Volume volume;
        [SerializeField] private Light mainLight;
        [SerializeField] private UniversalRenderPipelineAsset pc;
        [SerializeField] private UniversalRenderPipelineAsset mobile;

        [Tab("Panel")]
        [SerializeField] private Transform panel;
        [Tab("FPS")] 
        [SerializeField] private TMP_Text fpsText;


        private void Awake()
        {
            SetPanelState(false);
            enableButton.onClick.AddListener(() => SetPanelState(!panel.gameObject.activeSelf));
        }

        private void Update()
        {
            UpdateFPS();
        }

        private void SetPanelState(bool state)
        {
            var rotation = state ? Vector3.zero : new Vector3(0, 0, 180);
            panel.gameObject.SetActive(state);
            console.gameObject.SetActive(state);
            enableArrow.transform.eulerAngles = rotation;
        }

        public void Save() => GBGames.instance.Save();

        public void DeleteSaves() => GBGames.Delete();
        public void Add1000FirstCurrency() => WalletManager.FirstCurrency += 100000000;
        public void Add1000SecondCurrency() => WalletManager.SecondCurrency += 100000000;
        public void ChangePostProcessing() => volume.enabled = !volume.enabled;

        public void ChangeShadow()
        {
            mainLight.shadows = mainLight.shadows == LightShadows.None ? LightShadows.Soft : LightShadows.None;
        }

        public void ChangeQualityType()
        {
            var currentSettings = 
                QualitySettings.GetQualityLevel() == QualitySettings.names.ToList().IndexOf("Mobile")
                ? QualitySettings.names.ToList().IndexOf("PC")
                : QualitySettings.names.ToList().IndexOf("Mobile");

            var currentPipeline = GraphicsSettings.renderPipelineAsset == mobile ? pc : mobile;
            
            QualitySettings.SetQualityLevel(currentSettings);
            GraphicsSettings.renderPipelineAsset = currentPipeline;
        }

        public void UnlockRandomTrail()
        {
            var trails = Configuration.Instance.AllTrails.Where(t => !GBGames.saves.trailSaves.Unlocked(t.ID)).ToList();
            if (trails.Count <= 0) return;

            var rand = Random.Range(0, trails.Count);
            GBGames.saves.trailSaves.Unlock(trails[rand].ID);
        }
        
        
        private float _deltaTime;

        private void UpdateFPS()
        {
            _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
            var fps = 1.0f / _deltaTime;
            fpsText.text = $"{fps:0.} FPS";
        }
    }
}