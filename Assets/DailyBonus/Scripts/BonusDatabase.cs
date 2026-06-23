using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BonusDatabase", menuName = "Scriptable Objects/BonusDatabase")]
public class BonusDatabase : ScriptableObject
{
    public List<BonusDefinition> bonuses;

    public BonusDefinition GetRandomBonus()
    {
        return bonuses[Random.Range(0, bonuses.Count)];
    }

    public BonusDefinition GetBonusById(string id)
    {
        return bonuses.Find(x => x.id == id);
    }
}
