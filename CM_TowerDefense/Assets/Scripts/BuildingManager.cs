using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class BuildingManager : MonoBehaviour
{
    //Singleton
    public static BuildingManager Instance { get; private set; }

    //EVENTO
    #region Evento OnActiveBuildingChanged (Ativa ao mudar construção selecionada)
    public event EventHandler<OnActiveBuildingTypeChangedEventArgs> OnActiveBuildingChanged;

    public class OnActiveBuildingTypeChangedEventArgs : EventArgs
    {
        public BuildingTypeSO activeBuildingType;
    }
    #endregion

    //Camera
    private Camera cam;

    //Scriptable Objects
    private BuildingTypeListSO buildingTypeList;
    private BuildingTypeSO activeBuildingType;

    private void Awake()
    {
        Instance = this;

        #region Usando uma String diretamente
        //[Resources.Load] para encontrar um objeto na pasta [/Resources] de nome() e convertê-lo para tipo<>
        //Este método usa Hard Coded Strings, sendo mais propenso a erros, mas igualmente viável
        //buildingTypeList = Resources.Load<BuildingTypeListSO>("BuildingTypeListSO");
        #endregion

        #region Resources.Load sem usar String
        //Usando o parâmetro [typeof] para encontrar o arquivo 'BuildingTypeListSO' e pegar seu nome usando [.Name]
        //Sem usar String para encontrar o nome do 'ScriptableObject' na pasta 'Resources'
        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
        #endregion
    }
    private void Start()
    {
        //Ao começar a cena, pega a câmera principal e atribui na variável
        cam = Camera.main;
    }

    private void Update()
    {
        //Código para checar se a posição do Mouse está correta usando uma imagem e a fazendo seguir o cursor
        //mouseFollowTest.position = GetMouseWorldPosition();

        //Ao clicar com o botão esquerdo e o mouse não estiver em cima de algum elemento da HUD
        if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
        {
            //Se a construção atual não for nula e não estiver próximo de outra igual ou em cima de algum Collider
            if (activeBuildingType != null && CanSpawnBuilding(activeBuildingType, UtilsClass.GetMouseWorldPosition()))
            {
                //Só constroi se tiver os recursos necessários
                if (ResourceManager.Instance.CanAfford(activeBuildingType.constructionResourceCostArray))
                {
                    //Gasta os recursos necessários para criar a construção
                    ResourceManager.Instance.SpendResources(activeBuildingType.constructionResourceCostArray);
                    //Instancia na posição do mouse
                    Instantiate(activeBuildingType.prefab, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
                }
            }
        }
    }

    public void SetActiveBuilingType(BuildingTypeSO buildingType)
    {
        //Seta a construção ativa para ser posicionada
        activeBuildingType = buildingType;
        //Ativa o evento OnActiveBuildingChanged e passa o argumento activeBuildingType
        OnActiveBuildingChanged?.Invoke(this,
            new OnActiveBuildingTypeChangedEventArgs { activeBuildingType = activeBuildingType });
    }

    public BuildingTypeSO GetActiveBuildingType()
    {
        //Retorna a construção ativa no momento
        return activeBuildingType;
    }

    private bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 position)
    {
        //If pra não dar erro ao clicar com o mouse sem construção selecionada
        if (buildingType != null)
        {
            //Pega o Collider2D do prefab da construção
            BoxCollider2D boxCollider2D = buildingType.prefab.GetComponent<BoxCollider2D>();

            //Faz um raycast na área um pouco maior que o Collider2D do prefab para checar se há construções no local
            Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);

            //Se o array for igual a 0 (não há construções) o valor do bool será true, caso contrário será false
            bool isAreaClear = collider2DArray.Length == 0;

            //Se for false já retorna dizendo que não é possível
            if (!isAreaClear) return false;

            //Faz um raycast para checar se há construções iguais por perto
            collider2DArray = Physics2D.OverlapCircleAll(position, buildingType.minConstructionRadius);

            foreach (Collider2D collider2D in collider2DArray)
            {
                //Se um dos Collider2D do array tiver o mesmo BuildingType da construção atual, retornará false
                BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();

                if (buildingTypeHolder != null)
                {
                    if (buildingTypeHolder.buildingType == buildingType)
                    {
                        return false;
                    }
                }
            }
        }

        //Se chegou até aqui, retorna true dizendo que é possível construir
        return true;
    }
}
