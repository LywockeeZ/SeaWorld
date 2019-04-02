using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Avoidance")]
public class AvoidanceBehavior : FilterFlockBehavior
{
    Vector2 currentVelocity = new Vector2(-1, -1);
    public float agentSmoothTime = 0.5f;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //如果没有邻居，就不用调整
        if (context.Count == 0 || (filter.Filter(agent, context).Count == 0))
        {
            return Vector2.zero;
        }

        Ray ray = new Ray(agent.transform.position, agent.transform.forward);
        //Debug.DrawRay(agent.transform.position, agent.transform.forward);
        LayerMask mask = 1 << LayerMask.NameToLayer("Obstacle");
        RaycastHit hitInfo;

        //若需要分离，则执行分离操作
        Vector2 avoidanceMove = Vector2.zero;
        //需要分离的邻居的个数
        int nAvoid = 0;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            if (Physics.Raycast(ray, out hitInfo, flock.neighborRadius, mask))
            {
                avoidanceMove += (Vector2)(agent.transform.position - hitInfo.point);
            }
            else
            {
                if (Vector2.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidanceRadius)
                {
                    nAvoid++;
                    avoidanceMove += (Vector2)(agent.transform.position - item.position);

                    //avoidanceMove = Vector2.Lerp(agent.transform.forward, avoidanceMove, 5 * agentSmoothTime);
                }
            }

           

            avoidanceMove = Vector2.SmoothDamp(agent.transform.forward, avoidanceMove, ref currentVelocity, agentSmoothTime);
        }
        if (nAvoid > 0)
        {
            avoidanceMove /= nAvoid;
        }
        
        return avoidanceMove;
    }
}
