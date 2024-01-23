using System.Collections;
using UnityEngine;

namespace Menu___UI
{
    public class MainTitleDOTween : MonoBehaviour
    {
        [SerializeField] Animator animFadeOut;
        [SerializeField] GameObject _Panel, _TitlePanel;
        public void OnPressAnywhere() {
            _TitlePanel.gameObject.SetActive(false);
            animFadeOut.Play("FadeOut");
            StartCoroutine(Fade());
        }
        IEnumerator Fade() {
            yield return new WaitForSeconds(1.35f);
            _Panel.SetActive(false);
        }

    }
}
