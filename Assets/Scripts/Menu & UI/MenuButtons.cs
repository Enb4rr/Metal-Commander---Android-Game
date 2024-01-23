using DG.Tweening;
using UnityEngine;

namespace Menu___UI
{
    public class MenuButtons : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private GameObject settingsP, mainMenuP, selectLvlP;
    
        private void Start() 
        {
            DOTween.Init();
        }

        public void OnPlayButton() 
        {
            selectLvlP.gameObject.SetActive(true);
            mainCamera.transform.DOMove(selectLvlP.gameObject.transform.position, 0.5f);
        }
        public void OnSettingsButton() 
        {
            settingsP.gameObject.SetActive(true);
            mainCamera.transform.DOMove(settingsP.gameObject.transform.position, 0.5f);
        }
        public void OnMainBack() 
        {
            mainCamera.transform.DOMove(mainMenuP.gameObject.transform.position, 0.5f);
        }
    }
}
