using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ResourceTypeSO")]
public class ResourceTypeSO : ScriptableObject
{
    //Scriptable Object para guardar o nome do Recurso e seu Sprite
    //Ex: Wood, Stone, Gold...
    public string nameString;
    public Sprite sprite;
}
