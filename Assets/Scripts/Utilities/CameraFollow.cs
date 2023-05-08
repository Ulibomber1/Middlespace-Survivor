using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform playerTransform;

    [SerializeField] Vector3 offset;

    // Start is called before the first frame update
    void Awake()
    {
        PlayerController.OnPlayerJoined += PlayerOneJoined;
    }

    private void PlayerOneJoined(GameObject player)
    {
        playerTransform = player.transform;
        offset = transform.position - playerTransform.position;
        transform.position = playerTransform.position + offset;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = playerTransform.position + offset;
    }

    private void OnDestroy()
    {
        PlayerController.OnPlayerJoined -= PlayerOneJoined;
    }
}
