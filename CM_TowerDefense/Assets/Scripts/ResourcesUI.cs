using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ResourcesUI : MonoBehaviour
{
    private ResourceTypeListSO resourceTypeListSO;

    //Dictionary para guardar o Transform dos elementos da UI de cada Tipo de Recurso
    //Ex: Um Transform do Sprite da Madeira, um da Pedra, um do Ouro...
    //Foi feito dessa maneira para reaproveitar na função de adicionar o que estava sendo declarado no Awake
    private Dictionary<ResourceTypeSO, Transform> resourceTypeTransformDictionary;
    private void Awake()
    {
        //Procura o objeto de tipo ResourceTypeListSO na pasta Resources do Projeto
        resourceTypeListSO = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

        //Cria o Dicionário de Transform
        resourceTypeTransformDictionary = new Dictionary<ResourceTypeSO, Transform>();

        //Procura o GameObject de nome resourceTemplate dentro do GameObject que contém esse script
        Transform resourceTemplate = transform.Find("resourceTemplate");
        //Desativa ele pois é apenas um Template Base
        resourceTemplate.gameObject.SetActive(false);

        //Index para saber em qual elemento está
        int index = 0;
        foreach (ResourceTypeSO resourceType in resourceTypeListSO.list)
        {
            //Instancia o Template copiado como filho desse Transform
            Transform resourceTransform = Instantiate(resourceTemplate, transform);
            //Ativa ele já que o GameObject parent (pai) estará desativado
            resourceTransform.gameObject.SetActive(true);

            //Float para definir a distância entre cada elemento que será gerado na UI
            float offsetAmount = -200;
            //Ancora o Transform na sua nova posição
            resourceTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);

            //Procura o GameObject image e pega o componente Sprite da Imagem e atribui como
            //ao Sprite que está ligado ao ResourceType
            resourceTransform.Find("image").GetComponent<Image>().sprite = resourceType.sprite;

            //Atribui o Transform de cada um no Dictionary para usar fora do Awake
            resourceTypeTransformDictionary[resourceType] = resourceTransform;

            //Aumenta o index sempre que terminar a função
            index++;
        }
    }

    private void Start()
    {
        //Inscreve para receber o evento do ResourceManager
        ResourceManager.Instance.OnResourceAmountChanged += ResourceManager_OnResourceAmountChanged;

        UpdateResourceAmount();
    }

    //Função criada para funcionar o Evento
    private void ResourceManager_OnResourceAmountChanged(object sender, System.EventArgs e)
    {
        //Sempre que executar o Evento chamará esta função
        UpdateResourceAmount();
    }

    private void UpdateResourceAmount()
    {
        //Atualiza o texto de cada Recurso na tela
        foreach (ResourceTypeSO resourceType in resourceTypeListSO.list)
        {
            //Pega o número de recursos do tipo
            int resourceAmount = ResourceManager.Instance.GetResourceAmount(resourceType);

            //Pega o Transform do Dictionary
            Transform resourceTransform = resourceTypeTransformDictionary[resourceType];
            resourceTransform.Find("text").GetComponent<TextMeshProUGUI>().SetText(resourceAmount.ToString());
        }
    }
}
