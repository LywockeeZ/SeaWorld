using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Alignment")]
public class AlignmentBehavior : FilterFlockBehavior
{
    

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //如果没有邻居，维持原样
        if (context.Count == 0 || (filter.Filter(agent, context).Count == 0))
        {
            return Vector3.forward /7;
        }

        //把所有点聚集起来，并且求平均值
        Vector3 alignmentMove = Vector3.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            alignmentMove += (Vector3)item.transform.forward;
        }
        alignmentMove /= filteredContext.Count;

        return alignmentMove;
    }
}
