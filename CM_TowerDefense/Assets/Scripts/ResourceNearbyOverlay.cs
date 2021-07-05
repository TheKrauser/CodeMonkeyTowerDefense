using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceNearbyOverlay : MonoBehaviour
{
    //Script do ResourceGeneratorData
    private ResourceGeneratorData resourceGeneratorData;
    private void Awake()
    {
        //Começa escondendo o gameObject
        Hide();
    }
    private void Update()
    {
        //Seta a variável int para o tanto de recursos de mesmo tipo há em volta da construção
        int nearbyResourceAmount = ResourceGenerator.GetNearbyResourceAmount(resourceGeneratorData, transform.position);
        //Transforma em porcentagem dividindo os recursos que há em volta pelo max e multiplicando por 100
        float percent = Mathf.RoundToInt((float)nearbyResourceAmount / resourceGeneratorData.maxResourceAmount * 100f);
        //Seta o text para essa porcentagem
        transform.Find("text").GetComponent<TextMeshPro>().SetText(percent + "%");
    }

    public void Show(ResourceGeneratorData resourceGeneratorData)
    {
        //Seta o resourceGenerator para o que foi passado na função
        this.resourceGeneratorData = resourceGeneratorData;
        //Ativa o gameObject
        gameObject.SetActive(true);

        //Altera o sprite do icon para o recurso que a construção gera
        transform.Find("icon").GetComponent<SpriteRenderer>().sprite = resourceGeneratorData.resourceType.sprite;
    }

    public void Hide()
    {
        //Desativa o gameObject
        gameObject.SetActive(false);
    }
}
