using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using VInspector;
namespace _3._Scripts
{
    public class QualityController : MonoBehaviour
    {
        [Tab("Assets")] [SerializeField] private UniversalRenderPipelineAsset pc;
        [SerializeField] private UniversalRenderPipelineAsset mobile;
        [Tab("Components")] [SerializeField] private Light mainLight;
        [SerializeField] private Volume postProcessing;


        private void Start()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
            
            GraphicsSettings.renderPipelineAsset = mobile;
            mainLight.shadows = LightShadows.None;
            postProcessing.enabled = false;

            QualitySettings.SetQualityLevel(QualitySettings.names.ToList().IndexOf("Mobile"));
        }
    }
}