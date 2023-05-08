using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform playerTransform;
    Vector3 offset;
    GameObject keyboard;
    GameObject Player2Join;
    bool player2joined;

    // Start is called before the first frame update
    void Awake()
    {
        PlayerController.OnPlayerJoined += PlayerOneJoined;
        Player2Controller.OnPlayerJoined += PlayerTwoJoined;
        keyboard = gameObject.transform.GetChild(0).gameObject;
        Player2Join = gameObject.transform.GetChild(1).gameObject;
        Player2Join.SetActive(false);
        player2joined = false;
    }

    private void PlayerOneJoined(GameObject player)
    {
        playerTransform = player.transform;
        offset = transform.position - playerTransform.position;
        transform.position = playerTransform.position + offset;
    }

    private void PlayerTwoJoined(GameObject unused)
    {
        RemovePlayer2JoinGraphic();
        player2joined = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = playerTransform.position + offset;
    }

    private void RemoveKeyBoardGraphic()
    {
        keyboard.SetActive(false);

        if (!player2joined)
        {
            Player2Join.SetActive(true);
            Invoke("RemovePlayer2JoinGraphic", 10f);
        }
    }

    private void RemovePlayer2JoinGraphic()
    {
        if (Player2Join.activeSelf)
            Player2Join.SetActive(false);
    }

    private void OnDestroy()
    {
        PlayerController.OnPlayerJoined -= PlayerOneJoined;
        Player2Controller.OnPlayerJoined -= PlayerTwoJoined;
    }

    private void FixedUpdate()
    {
        if(Input.anyKeyDown)
            Invoke("RemoveKeyBoardGraphic", .01f);
    }
}
