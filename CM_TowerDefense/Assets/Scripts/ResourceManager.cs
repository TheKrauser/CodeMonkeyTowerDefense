using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResourceManager : MonoBehaviour
{
    //Para acessar essa classe em outros scripts
    public static ResourceManager Instance { get; private set; }

    //EVENTOOOOO!!!
    public event EventHandler OnResourceAmountChanged;

    //Dicionário para armazenar a quantia de cada tipo de recurso
    //ResourceTypeSO é a chave e int é o valor armazenado
    private Dictionary<ResourceTypeSO, int> resourceAmountDict;

    private void Awake()
    {
        Instance = this;

        //Cria o Dictionary
        resourceAmountDict = new Dictionary<ResourceTypeSO, int>();

        //Encontra o ResourceTypeListSO na pasta Resources
        //Sem usar uma string, ao invés deixamos o objeto com mesmo nome e usamos typeof
        ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

        //Seta o valor inicial de cada recurso no dicionário para 0
        foreach (ResourceTypeSO resourceType in resourceTypeList.list)
        {
            resourceAmountDict[resourceType] = 0;
        }
    }

    private void Update()
    {

    }

    /*//Printa no console todos os recursos que há no dicionário
    private void TestLog()
    {
        foreach (ResourceTypeSO resourceType in resourceAmountDict.Keys)
        {
            Debug.Log(resourceType.nameString + ": " + resourceAmountDict[resourceType]);
        }
    }*/

    //Adiciona o recurso na variável
    public void AddResource(ResourceTypeSO resourceType, int amount)
    {
        resourceAmountDict[resourceType] += amount;

        //Dispara esse evento sempre que a função AddResource for chamada
        //Eventos não ligam para se há algum recebedor, ele apenas dispara a ação
        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);

        #region A função acima com Invoke é uma versão simplificada e compacta dessa abaixo
        /*if (OnResourceAmountChanged !=  null)
        {
            OnResourceAmountChanged(this, EventArgs.Empty);
        }*/
        #endregion
    }

    //Função para retornar a quantidade de Recursos
    public int GetResourceAmount(ResourceTypeSO resourceType)
    {
        return resourceAmountDict[resourceType];
    }
}
