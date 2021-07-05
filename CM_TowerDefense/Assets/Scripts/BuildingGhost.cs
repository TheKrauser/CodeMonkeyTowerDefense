using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    //O GameObject do sprite da construção
    private GameObject spriteGameObject;
    //Script do ResourceNearbyOverlay
    private ResourceNearbyOverlay resourceNearbyOverlay;

    private void Awake()
    {
        //Procura o sprite e o gameObject e atribui
        spriteGameObject = transform.Find("sprite").gameObject;
        resourceNearbyOverlay = transform.Find("pfResourceNearbyOverlay").GetComponent<ResourceNearbyOverlay>();

        //Começa deixando invisível
        Hide();
    }

    private void Start()
    {
        //Inscreve no evento do BuildingManager
        BuildingManager.Instance.OnActiveBuildingChanged += BuildingManager_OnActiveBuildingTypeChanged;
    }

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, BuildingManager.OnActiveBuildingTypeChangedEventArgs e)
    {
        //Quando o evento disparar, se a construção ativa for null
        if (e.activeBuildingType == null)
        {
            //Desativa o sprite
            Hide();
            resourceNearbyOverlay.Hide();
        }
        else
        {
            //Se não, ativa o sprite e deixa o mesmo da construção atual
            Show(e.activeBuildingType.sprite);
            resourceNearbyOverlay.Show(e.activeBuildingType.resourceGeneratorData);
        }
    }

    private void Update()
    {
        //Sempre estará seguindo o ponteiro do mouse
        transform.position = UtilsClass.GetMouseWorldPosition();
    }

    private void Show(Sprite ghostSprite)
    {
        //Faz aparecer o GameObject
        spriteGameObject.SetActive(true);
        //Deixa o sprite o mesmo passado no ghostSprite
        spriteGameObject.GetComponent<SpriteRenderer>().sprite = ghostSprite;
    }

    private void Hide()
    {
        //Faz desaparecer o GameObject
        spriteGameObject.SetActive(false);
    }
}
