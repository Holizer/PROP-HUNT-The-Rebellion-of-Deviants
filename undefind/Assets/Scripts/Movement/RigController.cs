using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public class RigController : MonoBehaviour
{
    public MultiAimConstraint headAimConstraint;
    public MultiAimConstraint spinAimConstraint;
    public MultiAimConstraint shouldersAimConstraint;

    public void SetAimTargets(Transform target, float weight = 1f)
    {
        if (target == null)
        {
            Debug.LogWarning("Target transform is null. Cannot set aim targets.");
            return;
        }

        WeightedTransformArray headSources = new WeightedTransformArray();
        WeightedTransformArray bodySources = new WeightedTransformArray();
        WeightedTransformArray shouldersSources = new WeightedTransformArray();

        headSources.Add(new WeightedTransform { transform = target, weight = weight });
        bodySources.Add(new WeightedTransform { transform = target, weight = weight });
        shouldersSources.Add(new WeightedTransform { transform = target, weight = weight });

        headAimConstraint.data.sourceObjects = headSources;
        spinAimConstraint.data.sourceObjects = bodySources;
        shouldersAimConstraint.data.sourceObjects = shouldersSources;

        headAimConstraint.weight = weight;
        spinAimConstraint.weight = weight;
        shouldersAimConstraint.weight = weight;
    }

    public void ClearAimTargets()
    {
        headAimConstraint.weight = 0f;
        spinAimConstraint.weight = 0f;
        shouldersAimConstraint.weight = 0f;

        headAimConstraint.data.sourceObjects = new WeightedTransformArray();
        spinAimConstraint.data.sourceObjects = new WeightedTransformArray();
        shouldersAimConstraint.data.sourceObjects = new WeightedTransformArray();
    }
}
