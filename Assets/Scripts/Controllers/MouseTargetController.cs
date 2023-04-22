using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseTargetController : MonoBehaviour
{
    [SerializeField] Vector3 worldPosition;
    [SerializeField] Vector2 screenPosition;

    public delegate void MouseTargetAwakeHandler(GameObject targetGameObject);
    public static event MouseTargetAwakeHandler OnMouseTargetAwake;

    private void Awake()
    {
        OnMouseTargetAwake?.Invoke(gameObject);
    }

    void LateUpdate()
    {
        screenPosition = Mouse.current.position.ReadValue();
        Vector3 toConvert = new Vector3(screenPosition.x, screenPosition.y, 1);
        //worldPosition = Camera.main.ScreenToWorldPoint(toConvert);
        Ray toProject = Camera.main.ScreenPointToRay(toConvert);
        
        if (Physics.Raycast(toProject, out RaycastHit hitData))
        {
            worldPosition = hitData.point;
        }

        gameObject.transform.position = worldPosition;
    }
}
