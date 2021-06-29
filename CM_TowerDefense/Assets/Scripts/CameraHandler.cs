using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHandler : MonoBehaviour
{
    //Variável do Cinemachine
    [SerializeField] private CinemachineVirtualCamera cinemachineVC;

    //Variaveis para o controle do zoom
    private float orthographicSize;
    //Target é o valor final para interpolar e fazer um zoom suave
    private float targetOrthographicSize;

    private void Start()
    {
        //Seta o orthographicSize para o mesmo que está na camera do Cinemachine
        orthographicSize = cinemachineVC.m_Lens.OrthographicSize;

        //Seta o target para o mesmo do orthographic para que seja alterado
        targetOrthographicSize = orthographicSize;
    }

    private void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        //Inputs horizontais e verticais fazem a camera se mover (W A S D) ou (Setas Direcionais)
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        //Cria um vetor baseado no input e normaliza para não dobrar a velocidade quando houverem dois apertados
        Vector3 moveDir = new Vector3(x, y).normalized;

        //Velocidade de movimento
        float moveSpeed = 30f;

        //Movimenta o GameObject em branco
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void HandleZoom()
    {
        //Quantidade de zoom
        float zoomAmount = 2f;

        //Realiza o Zoom In ou Zoom Out
        //mouseScrollDelta detecta o quanto a rodinha do mouse se mexeu após o último frame
        //Valor negativo só para mudar a direção de onde é In ou Out
        targetOrthographicSize += -Input.mouseScrollDelta.y * zoomAmount;

        //Valores para o Clamp
        float minOrthographicSize = 5f;
        float maxOrthographicSize = 30f;

        //Não deixa que o tamanho passe dos valores min e max
        //Assim a câmera não passará dos limites e não quebrará o jogo
        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, minOrthographicSize, maxOrthographicSize);

        //Velocidade do zoom
        float zoomSpeed = 5f;
        //Interpola o valor entre o tamanho antigo e o atual do target, multiplicando a velocidade
        //por deltaTime para um controle mais suave
        orthographicSize = Mathf.Lerp(orthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);

        //Por fim muda o tamanho da camera
        cinemachineVC.m_Lens.OrthographicSize = orthographicSize;
    }
}
