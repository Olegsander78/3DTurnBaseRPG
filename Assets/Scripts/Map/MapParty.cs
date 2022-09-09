using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapParty : MonoBehaviour
{
    public float moveSpeed;
    private bool ismovingToEncounter;

    public void MoveToEncounter(Encounter encounter, UnityAction<Encounter> onArriveCallback)
    {
        if (ismovingToEncounter)
            return;

        ismovingToEncounter = true;

        StartCoroutine(Move());

        IEnumerator Move()
        {
            transform.LookAt(encounter.transform.position);

            while(transform.position != encounter.transform.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, encounter.transform.position, moveSpeed * Time.deltaTime);
                yield return null;
            }

            ismovingToEncounter = false;
            onArriveCallback?.Invoke(encounter);
        }
    }
}
