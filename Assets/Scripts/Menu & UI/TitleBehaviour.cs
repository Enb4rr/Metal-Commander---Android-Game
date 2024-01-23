using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Menu___UI
{
    public class TitleBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject target;
        [SerializeField] private GameObject target2;
        [SerializeField] private GameObject outTarget;
        [SerializeField] private GameObject outTarget2;
        
        public void SetTitle(GameObject title)
        {
            title.transform.DOMove(target.transform.position, 0.2f, false);
        }
        
        public void RemoveTitle(GameObject title)
        {
            title.transform.DOMove(outTarget.transform.position, 0.2f, false);
        }
        
        public void SetTitleExit(GameObject title)
        {
            title.transform.DOMove(target2.transform.position, 0.2f, false);
        }
        
        public void RemoveTitleExit(GameObject title)
        {
            title.transform.DOMove(outTarget2.transform.position, 0.2f, false);
        }
    }
}
