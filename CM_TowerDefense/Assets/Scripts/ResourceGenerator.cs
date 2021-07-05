using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ResourceGenerator : MonoBehaviour
{
    //Função estática para usar em outros scripts
    public static int GetNearbyResourceAmount(ResourceGeneratorData resourceGeneratorData, Vector3 position)
    {
        //Cria um Raycast pra ver quantos recursos tem ao redor
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(position, resourceGeneratorData.resourceDetectionRadius);

        //Inicializa em 0
        int nearbyResourceAmount = 0;
        foreach (Collider2D collider2D in collider2DArray)
        {
            //Pra cada Collider2D que for encontrado, pega o script de ResourceNode
            ResourceNode resourceNode = collider2D.GetComponent<ResourceNode>();

            //Se for diferente de null
            if (resourceNode != null)
            {
                //Incrementa de acordo com o tanto de recursos encontrador que sejam do mesmo tipo da Construção
                if (resourceNode.resourceType == resourceGeneratorData.resourceType)
                    nearbyResourceAmount++;
            }
        }

        //Não deixa que o valor seja maior do que o máximo setado no ScritableObject
        nearbyResourceAmount = Mathf.Clamp(nearbyResourceAmount, 0, resourceGeneratorData.maxResourceAmount);

        return nearbyResourceAmount;
    }

    //Tipo da Construção
    private ResourceGeneratorData resourceGeneratorData;

    //Variaveis para calcular o tempo de geração de recurso de cada Construção
    private float timer;
    private float timerMax;

    private void Awake()
    {
        //Pega o componente do BuildingType
        resourceGeneratorData = GetComponent<BuildingTypeHolder>().buildingType.resourceGeneratorData;

        //Seta o timerMax para o mesmo da Construção
        timerMax = resourceGeneratorData.timerMax;
    }

    private void Start()
    {
        int nearbyResourceAmount = GetNearbyResourceAmount(resourceGeneratorData, transform.position);
        //Se for 0, desabilita o script para que a Construção não gere recursos
        if (nearbyResourceAmount == 0)
        {
            enabled = false;
        }
        else
        {
            //Se não for 0 (o que significa que há recursos por perto)
            //Ajusta o TimerMax de acordo com os recursos encontrados
            //Se houver o máximo de recursos ao redor, a construção vai gerar recurso 2x mais rápido que o padrão
            //Se houver o apenas 1 ela vai gerar 2x mais lento
            timerMax = (resourceGeneratorData.timerMax / 2f)
                + resourceGeneratorData.timerMax
                * (1 - (float)nearbyResourceAmount / resourceGeneratorData.maxResourceAmount);
        }
    }

    private void Update()
    {
        //Decrementa o timer em deltaTime e quando estiver em 0, reseta ele e adiciona o recurso
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer += timerMax;
            ResourceManager.Instance.AddResource(resourceGeneratorData.resourceType, 1);
        }
    }

    public ResourceGeneratorData GetResourceGeneratorData()
    {
        //Retorna o resourceGeneratorData atual para usar em outros scripts
        return resourceGeneratorData;
    }

    public float GetTimerNormalized()
    {
        //Retorna o timer atual em valor de 0 a 1
        return timer / timerMax;
    }

    public float GetAmountGeneratedPerSecond()
    {
        //Retorna o quanto a construção gera por segundo
        return 1 / timerMax;
    }
}