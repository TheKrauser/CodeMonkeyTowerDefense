using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]
public class BuildingTypeSO : ScriptableObject
{
    //Scriptable Object para guardar o Nome da Construção, Prefab, Sprite e a lista de Recursos que ela vai gerar
    public string nameString;
    public Transform prefab;
    public List<ResourceGeneratorData> resourceGeneratorDataList;
    public Sprite sprite;
    public float minConstructionRadius;
}
