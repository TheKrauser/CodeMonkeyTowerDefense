using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceGeneratorOverlay : MonoBehaviour
{
    //Script do ResourceGenerator
    [SerializeField] private ResourceGenerator resourceGenerator;

    //Transform da barra de progresso
    private Transform barTransform;

    private void Start()
    {
        //Pega o Script pela função estática presente nele
        ResourceGeneratorData resourceGeneratorData = resourceGenerator.GetResourceGeneratorData();

        //Procura a bar
        barTransform = transform.Find("bar");

        //Seta o ícone para o mesmo do recurso que a construção gera
        transform.Find("icon").GetComponent<SpriteRenderer>().sprite = resourceGeneratorData.resourceType.sprite;
        //Seta o texto para o tanto que ela gera por segundo
        //F1 no ToString("F1") é para definir quantas casas após a vírgula é para mostrar
        transform.Find("text").GetComponent<TextMeshPro>().SetText(resourceGenerator.GetAmountGeneratedPerSecond().ToString("F1") + "x");
    }

    private void Update()
    {
        //Atualiza a barra de geração de recurso
        barTransform.localScale = new Vector3(1 - resourceGenerator.GetTimerNormalized(), 1, 1);
    }
}
