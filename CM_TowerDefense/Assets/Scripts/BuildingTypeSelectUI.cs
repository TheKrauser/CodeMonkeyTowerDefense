using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTypeSelectUI : MonoBehaviour
{
    //Sprite do ponteiro do Mouse
    [SerializeField] private Sprite arrowSprite;

    //List de Construções a ser ignoradas na hora de criar os botões da UI
    [SerializeField] private List<BuildingTypeSO> ignoreBuildingTypeList;

    //Dicionário para guardar o Transform da posição dos botões na UI
    private Dictionary<BuildingTypeSO, Transform> btnTransformDictionary;

    //Posição do botão do ponteiro do Mouse
    private Transform arrowBtn;
    private void Awake()
    {
        //Procura o template na Hierarchy (filho do GameObject que está esse Script)
        Transform btnTemplate = transform.Find("btnTemplate");
        //Deixa o template Inativo
        btnTemplate.gameObject.SetActive(false);

        //Procura o ScriptableObject na pasta Resources (sem usar string) 
        BuildingTypeListSO buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);

        //Guarda o Transform do botão no Dictionary
        btnTransformDictionary = new Dictionary<BuildingTypeSO, Transform>();


        //Deixa o index em 0
        int index = 0;

        //Instancia o botão inicial do cursor do Mouse e deixa ativo
        arrowBtn = Instantiate(btnTemplate, transform);
        arrowBtn.gameObject.SetActive(true);

        //Distância entre ele e o próximo botão
        float offsetAmount = 160f;
        //Posiciona e ancora na posição inicial onde o GameObject pai está localizado
        arrowBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);

        //Pega o sprite da imagem e troca pelo sprite do Mouse e diminui o tamanho em 30 unidades
        arrowBtn.Find("image").GetComponent<Image>().sprite = arrowSprite;
        arrowBtn.Find("image").GetComponent<RectTransform>().sizeDelta = new Vector2(0, -30);

        //Adiciona um evento de OnClick no botão
        arrowBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            //Quando clicar a construção ativa será null, ou seja, não posicionará nada ao clicar com o mouse
            BuildingManager.Instance.SetActiveBuilingType(null);
        });

        //Incrementa o index ao posicionar o primeiro botão
        index++;

        //Para cada construção existente na lista
        foreach (BuildingTypeSO buildingType in buildingTypeList.list)
        {
            if (ignoreBuildingTypeList.Contains(buildingType)) continue;

            //Instancia o template e ativa
            Transform btnTransform = Instantiate(btnTemplate, transform);
            btnTransform.gameObject.SetActive(true);

            //Posiciona o botão baseado no index e com distância da variável offsetAmount
            offsetAmount = 160f;
            btnTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);

            //Muda o sprite de acordo com o que está declarado no Scriptable Object
            btnTransform.Find("image").GetComponent<Image>().sprite = buildingType.sprite;

            //Adiciona um evento de OnClick
            btnTransform.GetComponent<Button>().onClick.AddListener(() =>
            {
                //Ao clicar, seta a construção ativa de acordo com o Scriptable Object
                BuildingManager.Instance.SetActiveBuilingType(buildingType);
            });

            //O transform da posição se torna o transform do botão atual
            btnTransformDictionary[buildingType] = btnTransform;

            //Incrementa o index para o próximo botão
            index++;
        }
    }

    private void Start()
    {
        UpdateActiveBuildingTypeButton();
        //Se inscreve no evento de OnActiveBuildingChanged
        BuildingManager.Instance.OnActiveBuildingChanged += BuildingManager_OnActiveBuildingTypeChanged;
    }

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, BuildingManager.OnActiveBuildingTypeChangedEventArgs e)
    {
        //Sempre que receber o evento realiza a ação de atualizar a UI para a construção ativa
        UpdateActiveBuildingTypeButton();
    }

    private void UpdateActiveBuildingTypeButton()
    {
        //Desativa o sprite de construção selecionada (O outline em volta) do Mouse
        arrowBtn.Find("selected").gameObject.SetActive(false);
        //Desativa o sprite de construção selecionada (O outline em volta) das construções na lista
        foreach (BuildingTypeSO buildingType in btnTransformDictionary.Keys)
        {
            Transform btnTransform = btnTransformDictionary[buildingType];
            btnTransform.Find("selected").gameObject.SetActive(false);
        }

        //Ativa de acordo com a construção que está selecionada no momento
        BuildingTypeSO activeBuildingType = BuildingManager.Instance.GetActiveBuildingType();

        //Se nenhuma estiver selecionada então ative o do Mouse
        if (activeBuildingType == null)
        {
            arrowBtn.Find("selected").gameObject.SetActive(true);
        }
        else
        {
            btnTransformDictionary[activeBuildingType].Find("selected").gameObject.SetActive(true);
        }
    }
}
