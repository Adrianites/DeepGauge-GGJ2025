using System.Collections.Generic;
using UnityEngine;

public class ValvePuzzleManager : MonoBehaviour
{
    public static ValvePuzzleManager Instance;

    [System.Serializable]
    public class DoorValves
    {
        public Door door;
        public List<Valve> valves;
        [HideInInspector]
        public int activeValvesCount = 0;
    }

    public List<DoorValves> doorValvesList;

    private void Awake()
    {
        Instance = this;
    }

    public void IncrementActiveValves(Valve valve)
    {
        foreach (DoorValves doorValves in doorValvesList)
        {
            if (doorValves.valves.Contains(valve))
            {
                doorValves.activeValvesCount++;
                if (doorValves.activeValvesCount == doorValves.valves.Count)
                {
                    doorValves.door.OpenDoor();
                }
                break;
            }
        }
    }
}