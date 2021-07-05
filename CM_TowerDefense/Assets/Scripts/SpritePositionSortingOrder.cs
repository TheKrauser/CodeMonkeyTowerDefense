using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script não usado pois a Unity possui um meio automático de fazer isso
public class SpritePositionSortingOrder : MonoBehaviour
{
    //Pra se destruir após setar o Order in Layer
    [SerializeField] private bool runOnce = true;

    //Sprite Renderer do GameObject
    private SpriteRenderer spriteRenderer;

    //Offset
    [SerializeField] private float positionOffsetY;

    private void Awake()
    {
        //Pega o Sprite Renderer
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        //Precisão para ajustar em um raio de 5 valores pra cada unidade em Y
        float precisionMultiplier = 5f;
        spriteRenderer.sortingOrder = (int)(-transform.position.y + positionOffsetY * precisionMultiplier);

        if (runOnce)
        {
            //Destrói logo após setar
            Destroy(this);
        }
    }
}
