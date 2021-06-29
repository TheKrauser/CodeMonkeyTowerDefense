using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BuildingTypeList")]
public class BuildingTypeListSO : ScriptableObject
{
    //Scriptable Object para guardar todos os tipos de contrução
    //Ex: Wood Harvester, Stone Harvester, Gold Harvester...
    
    public List<BuildingTypeSO> list;
}
