using System.Collections.Generic;
using UnityEngine;

public class PeasantFactory : NPCFactory
{
    public override NPC CreateNPC(string npcName, List<GameObject> models)
    {
        PeasantNPC peasant = new GameObject(npcName).AddComponent<PeasantNPC>();
        peasant.SetName(npcName);
        peasant.AssignRandomModel(models);
        return peasant;
    }
}
