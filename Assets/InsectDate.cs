using UnityEngine;

[CreateAssetMenu(fileName = "Insect", menuName = "Scriptable Objects/Insect")]
public class Insect : ScriptableObject
{
    public string insectName = "";

    public GameObject insectModel;

    public float insectSpeed = 0f;
}
