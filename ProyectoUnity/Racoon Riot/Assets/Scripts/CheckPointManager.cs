using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public CheckPoint currentCheckpoint;
    public CheckPoint startingCheckpoint;
    public PlayerController player;

    private void Awake() {
        currentCheckpoint = startingCheckpoint;
    }

    public void GoToStart()
    {
        player.gameObject.transform.position = startingCheckpoint.transform.position;
    }

    public void ChangeCheckPoint(CheckPoint point)
    {
        currentCheckpoint = point;
    }

    public void Reviver()
    {
        StartCoroutine(RevivePlayer());
    }

    private IEnumerator RevivePlayer()
    {
        player.gameObject.transform.position = currentCheckpoint.transform.position;
        yield return new WaitForSeconds(2);
    }
}
