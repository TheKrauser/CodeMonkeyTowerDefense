using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script não usado pois a Unity possui um meio automático de fazer isso
public class SpritePositionSortingOrder : MonoBehaviour
{
    [SerializeField] private bool runOnce = true;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float positionOffsetY;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        float precisionMultiplier = 5f;
        spriteRenderer.sortingOrder = (int)(-transform.position.y + positionOffsetY * precisionMultiplier);

        if (runOnce)
        {
            Destroy(this);
        }
    }
}
