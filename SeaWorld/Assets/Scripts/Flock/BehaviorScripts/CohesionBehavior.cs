using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Cohesion")]
public class CohesionBehavior : FilterFlockBehavior
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //如果没有邻居，就不用调整
        if (context.Count == 0 || (filter.Filter(agent, context).Count == 0))
        {
            return Vector3.zero;
        }

        //把所有点聚集起来，并且求平均值
        Vector3 cohesionMove = Vector3.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            cohesionMove += (Vector3)item.position;
        }
        cohesionMove /= filteredContext.Count;

        //对agent的位置计算一下偏差
        cohesionMove -= (Vector3)agent.transform.position;
        return cohesionMove;
    }


}
