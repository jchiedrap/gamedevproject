using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class StairsInteractable : Interactable
    {
        public bool isUp;
        public override void PerformInteraction() {
            if (ConditionsMet()) {
                StartCoroutine(nameof(TransitionFloor));
            }
        }

        public override bool ConditionsMet()
        {
            return true;
        }
        
        IEnumerator TransitionFloor() {
            LevelManager.Instance.inputController.GetComponentInChildren<Curtain>().Fade(true);
            yield return new WaitForSeconds(1);
            if (isUp) {
                DungeonManager.Instance.currentFloor++;
            } else {
                DungeonManager.Instance.currentFloor--;
            }
            DungeonManager.Instance.LoadFloor();
        }
    }
}