
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace _3._Scripts.UI.Extensions
{
    public class UseView: MonoBehaviour
    {
        [SerializeField] private Image mobile;
        [SerializeField] private SpriteRenderer mobile3D;
        [SerializeField] private TMP_Text pc;

        private void OnEnable()
        {
            if(mobile != null)
                mobile.gameObject.SetActive(false);
            if(pc != null)
                pc.gameObject.SetActive(false);
            if(mobile3D != null)
                mobile3D.gameObject.SetActive(false);
            
            if(mobile != null)
                mobile.gameObject.SetActive(true);
            if(mobile3D != null)
                mobile3D.gameObject.SetActive(true);
        }
    }
}