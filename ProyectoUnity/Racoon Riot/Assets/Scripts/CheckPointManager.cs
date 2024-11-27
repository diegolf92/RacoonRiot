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
        Vector3 offsettingPos = new Vector3(currentCheckpoint.transform.position.x, currentCheckpoint.transform.position.y + 1f, currentCheckpoint.transform.position.z);
        player.gameObject.transform.position = offsettingPos;
        yield return new WaitForSeconds(2);
    }
}
