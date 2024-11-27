using UnityEngine;

public class PeasantNPC : NPC
{
    public override void PerformBehavior()
    {
        Debug.Log($"Крестьянин {NpcName} работает на поле.");
    }
}
