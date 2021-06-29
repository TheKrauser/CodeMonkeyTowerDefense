using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Mostrar no Inspector da Unity
[System.Serializable]
public class ResourceGeneratorData
{
    //Script para configurar em cada Scriptable Object das Construções, a frequência e o recurso que será gerado
    public float timerMax;
    public ResourceTypeSO resourceType;
    public float resourceDetectionRadius;
    public int maxResourceAmount;
}
