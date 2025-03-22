using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrolleyCapacityManager : MonoBehaviour
{
    public enum PartType { Head, Body, Tail }
    public PartType partType;
    private int maxCapacity;
    private int occupiedSpaces = 0;
    private Material trolleyMaterial;

    void Start()
    {
        if (partType == PartType.Head || partType == PartType.Tail)
            maxCapacity = 2;
        else if (partType == PartType.Body)
            maxCapacity = 4;

        Renderer rend = GetComponent<Renderer>();
        if (rend != null && rend.materials.Length > 0)
        {
            trolleyMaterial = rend.materials[0];
        }
    }

    public bool CanBoardPassenger(Material passengerMaterial)
    {
        if (trolleyMaterial == null || passengerMaterial == null)
        {
            return false;
        }

        bool hasSpace = occupiedSpaces < maxCapacity;
        bool materialMatch = MaterialsMatch(trolleyMaterial, passengerMaterial);

        if (hasSpace && materialMatch)
        {
            return true;
        }

        return false;
    }

    private bool MaterialsMatch(Material mat1, Material mat2)
    {
        return mat1.color == mat2.color;
    }

    public void BoardPassenger()
    {
        if (occupiedSpaces < maxCapacity)
        {
            occupiedSpaces++;
        }
    }

    public int GetOccupiedSpaces()
    {
        return occupiedSpaces;
    }
}