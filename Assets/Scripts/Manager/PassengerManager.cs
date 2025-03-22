using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PassengerManager : MonoBehaviour
{
    private List<Transform> passengers = new List<Transform>();
    private List<Material> passengerMaterials = new List<Material>();
    private List<bool> passengerBoarded = new List<bool>();

    void Start()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            Transform passenger = transform.GetChild(i);
            passengers.Add(passenger);
            passengerBoarded.Add(false);

            Renderer rend = passenger.GetComponent<Renderer>();
            if (rend == null && passenger.childCount > 0)
            {
                rend = passenger.GetChild(0).GetComponent<Renderer>();
            }

            if (rend != null)
            {
                passengerMaterials.Add(rend.material);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trolley"))
        {
            Transform trolley = other.transform.parent;

            TryBoardPassengers(trolley);
        }
    }

    private void TryBoardPassengers(Transform trolley)
    {
        TrolleyCapacityManager[] trolleyParts = trolley.GetComponentsInChildren<TrolleyCapacityManager>();

        for (int i = 0; i < passengers.Count; i++)
        {
            if (!passengerBoarded[i] && passengerMaterials[i] != null)
            {
                foreach (TrolleyCapacityManager part in trolleyParts)
                {
                    if (part.CanBoardPassenger(passengerMaterials[i]))
                    {
                        Vector3 seatPosition = CalculateSeatPosition(part);

                        passengers[i].SetParent(part.transform);
                        passengers[i].DOLocalMove(seatPosition, 1f);

                        passengerBoarded[i] = true;
                        part.BoardPassenger();

                        break;
                    }
                }
            }
        }
    }

    private Vector3 CalculateSeatPosition(TrolleyCapacityManager part)
    {
        int currentOccupied = part.GetOccupiedSpaces();
        Vector3 position = Vector3.zero;

        if (part.partType == TrolleyCapacityManager.PartType.Head)
        {
            if (currentOccupied == 0) position = new Vector3(-0.2f, 0f, -0.2f);
            else position = new Vector3(0.2f, 0f, -0.2f);
        }
        else if (part.partType == TrolleyCapacityManager.PartType.Tail)
        {
            if (currentOccupied == 0) position = new Vector3(-0.2f, 0f, 0.2f);
            else position = new Vector3(0.2f, 0f, 0.2f);
        }
        else
        {
            switch (currentOccupied)
            {
                case 0: position = new Vector3(-0.2f, 0f, 0.2f); break;
                case 1: position = new Vector3(0.2f, 0f, 0.2f); break;
                case 2: position = new Vector3(-0.2f, 0f, -0.2f); break;
                case 3: position = new Vector3(0.2f, 0f, -0.2f); break;
            }
        }

        return position;
    }
}