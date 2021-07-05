using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsClass
{
    //Classe para ser usada em outros scripts sem precisar de refêrencia ou de estar na cena
    private static Camera cam;
    public static Vector3 GetMouseWorldPosition()
    {
        if (cam == null)
        cam = Camera.main;
        
        //Screen to World Point para converter de Posição na Tela para Posição na Cena;

        //Armazenar a posição em um Vector3
        Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(Input.mousePosition);

        //Setar o eixo Z em 0f pois a Câmera principal está posicionada na posição Z = -10f;
        mouseWorldPosition.z = 0f;

        return mouseWorldPosition;
    }
}
