using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    //Tipo da Construção
    private BuildingTypeSO buildingType;

    //Variável para calcular o tempo de geração de recurso de cada Construção
    //Foi usado Dictionary para não haver problemas caso
    //uma construção precise gerar mais de 1 (Um) recurso simultaneamente
    private Dictionary<ResourceGeneratorData, float> timerDict;
    private Dictionary<ResourceGeneratorData, float> timerMaxDict;

    private void Awake()
    {
        //Pega o componente do BuildingType
        buildingType = GetComponent<BuildingTypeHolder>().buildingType;

        //Inicializa os Timers
        timerDict = new Dictionary<ResourceGeneratorData, float>();
        timerMaxDict = new Dictionary<ResourceGeneratorData, float>();

        //Cria o número de Timers baseado no número de Recursos que serão gerados ao mesmo tempo
        foreach (ResourceGeneratorData resourceGeneratorData in buildingType.resourceGeneratorDataList)
        {
            timerDict[resourceGeneratorData] = 0f;
            timerMaxDict[resourceGeneratorData] = resourceGeneratorData.timerMax;
        }
    }

    private void Start()
    {
        foreach (ResourceGeneratorData resourceGeneratorData in buildingType.resourceGeneratorDataList)
        {
            Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, resourceGeneratorData.resourceDetectionRadius);

            int nearbyResourceAmount = 0;
            foreach (Collider2D collider2D in collider2DArray)
            {
                ResourceNode resourceNode = collider2D.GetComponent<ResourceNode>();

                if (resourceNode != null)
                {
                    if (resourceNode.resourceType == resourceGeneratorData.resourceType)
                    nearbyResourceAmount++;
                }
            }

            nearbyResourceAmount = Mathf.Clamp(nearbyResourceAmount, 0, resourceGeneratorData.maxResourceAmount);

            if (nearbyResourceAmount == 0)
            {
                enabled = false;
            }
            else
            {
                timerMaxDict[resourceGeneratorData] = (resourceGeneratorData.timerMax / 2f)
                    + resourceGeneratorData.timerMax
                    * (1 - (float)nearbyResourceAmount / resourceGeneratorData.maxResourceAmount);
            }
        }
    }

    private void Update()
    {
        //Faz os timers correrem de acordo com o Time.deltaTime
        foreach (ResourceGeneratorData resourceGeneratorData in buildingType.resourceGeneratorDataList)
        {
            timerDict[resourceGeneratorData] -= Time.deltaTime;
            if (timerDict[resourceGeneratorData] <= 0f)
            {
                timerDict[resourceGeneratorData] += timerMaxDict[resourceGeneratorData];
                ResourceManager.Instance.AddResource(resourceGeneratorData.resourceType, 1);
            }
        }
    }
}