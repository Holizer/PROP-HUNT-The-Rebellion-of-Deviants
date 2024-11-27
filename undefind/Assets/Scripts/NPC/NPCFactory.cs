using System.Collections.Generic;
using UnityEngine;

public abstract class NPCFactory
{
    public abstract NPC CreateNPC(string npcName, List<GameObject> models);
}