using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace PathFinding
{
    public class TutorialPathmovement : MonoBehaviour
    {
        [SerializeField] private Pathfinding2D pathMovement;
        [SerializeField] private Unit selectedUnit;
        [SerializeField] private GameObject target;
        [SerializeField] private Tilemap map;
        private bool grabed;
        private bool selectedNewSpace;

        //From here, TurnSystem

        [SerializeField] private TurnSystem.TurnSystem turnSystem;

        //From here, SoundSystem

        [SerializeField] private AudioManager source;


        //movimiento
        bool tap;
        float timer = 0;


        //tutorial
        public Canvas imageMove;
        public Canvas imageattack;

        public int tutorialMove = 0;
        public int tutorialattack = 0;

        public GameObject arrow;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !grabed && !selectedNewSpace)
            {
                SelectUnit();
            }
            else if (Input.GetMouseButtonDown(0) && grabed && !selectedNewSpace)
            {
                SelectNewSpace();
            }
            else if (tutorialattack > 1)
            {
                imageattack.gameObject.SetActive(false);
                selectedUnit = null;
                pathMovement = null;
            }

        }

        private void SelectUnit()
        {
            

            Vector2 worldPosition = turnSystem.mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitData = Physics2D.Raycast(worldPosition, Vector2.zero, 0);

            if (!hitData)
            {
                grabed = false;
                return;
            }
            if (hitData.transform.gameObject.CompareTag("Enemy"))
            {
                grabed = false;
            }
            if (hitData.transform.gameObject.CompareTag("Ally"))
            {
                tutorialMove++;
                arrow.gameObject.SetActive(false);
                source.Play("SelectedUnit");

                selectedUnit = hitData.transform.gameObject.GetComponent<Unit>();
                pathMovement = selectedUnit.GetComponent<Pathfinding2D>();

                selectedUnit.path.SetActive(true);

                grabed = true;

                turnSystem.mainCamera.transform.DOMove(new Vector3(0, 0, -10) + selectedUnit.transform.position, 0.2f, false);

                if (selectedUnit.hasMoved)
                {
                    grabed = false;
                    selectedUnit.path.SetActive(false);
                }
            }
            if (tutorialMove == 1)
            {
                imageMove.gameObject.SetActive(true);
            }

        }

        void SelectNewSpace()
        {
            if (grabed)
            {
                source.Play("SelectedSpace");

                Vector2 mousePosition = turnSystem.mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector3 gridPosition = map.WorldToCell(mousePosition);

                var newTarget = Instantiate(target, gridPosition, quaternion.identity);
                selectedNewSpace = true;

                Vector3Int tilePosition = map.WorldToCell(mousePosition);
                if (map.GetTile(tilePosition) == null)
                {
                    grabed = false;
                    selectedNewSpace = false;
                    selectedUnit.path.SetActive(false);
                    selectedUnit.anim.SetBool("Walk2", false);
                    Destroy(newTarget);
                    return;
                }

                Vector3Int unitGridPos = map.WorldToCell(selectedUnit.transform.position);
                Vector3Int targetGridPos = map.WorldToCell(newTarget.transform.position);

                pathMovement.FindPath(unitGridPos, targetGridPos);

                if (pathMovement.path.Count > selectedUnit.movement)
                {
                    grabed = false;
                    selectedNewSpace = false;
                    selectedUnit.path.SetActive(false);
                    Destroy(newTarget);
                    return;
                }

                Move(pathMovement);

                grabed = false;
                selectedNewSpace = false;
                Destroy(newTarget);
            }
            else
            {
                grabed = false;
                return;
            }
        }

        private void Move(Pathfinding2D unitPath)
        {
            tutorialattack++;

            imageMove.gameObject.SetActive(false);
            if (tutorialattack == 1)
            {
                imageattack.gameObject.SetActive(true);
            }


            selectedUnit.path.SetActive(false);
            selectedUnit.anim.SetBool("Walk1", true);

            foreach (var t in unitPath.path)
            {
                selectedUnit.transform.DOMove(t.worldPosition, 0.5f, true);
            }

            selectedUnit.anim.SetBool("Walk1", false);

            selectedUnit.hasMoved = true;
        }
    }
}
