using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu___UI
{
    public class CharacterWindow : MonoBehaviour
    {
        [SerializeField] private GameObject window;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private GameObject targetPos;
        [SerializeField] private GameObject oldPos;
    
        [SerializeField] private Unit unitSelected;
        [SerializeField] private Image portrait;
        [SerializeField] private TMP_Text unitName;
        [SerializeField] private TMP_Text health;
        [SerializeField] private TMP_Text attack;
        [SerializeField] private TMP_Text defense;
        [SerializeField] private TMP_Text unitClass;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SelectUnit(); 
            }
        }

        private void Selected(Unit selectedUnit)
        {
            window.transform.DOMove(targetPos.transform.position, 0.2f, false);
            unitName.text = selectedUnit.unitName;
            health.text = selectedUnit.hitPoints.ToString() + "/" + selectedUnit.maxHP.ToString();
            attack.text = (selectedUnit.attack + selectedUnit.weaponPower).ToString();
            defense.text = selectedUnit.defense.ToString();
            portrait.sprite = selectedUnit.portrait;
            unitClass.text = selectedUnit.className;
        }

        private void SelectUnit()
        {
            Vector2 worldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitData = Physics2D.Raycast(worldPosition, Vector2.zero, 0);

            if (!hitData)
            {
                window.transform.DOMove(oldPos.transform.position, 0.2f, false);
                return;
            }

            if (hitData)
            {
                unitSelected = hitData.transform.gameObject.GetComponent<Unit>();
                if (unitSelected.GetComponent<Unit>() ==null) return;
                Selected(unitSelected);
            }
        }
    }
}
