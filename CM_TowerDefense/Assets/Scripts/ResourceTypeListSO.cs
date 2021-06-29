using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ResourceTypeListSO")]
public class ResourceTypeListSO : ScriptableObject
{
    //Scriptable Object para guardar uma lista de todos os tipos de recurso que as construções podem gerar
    public List<ResourceTypeSO> list;
}
