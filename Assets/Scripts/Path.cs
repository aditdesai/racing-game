
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    
    private List<Transform> nodes = new List<Transform>();

    private void OnDrawGizmosSelected()
    {
        Transform[] pathTransforms = GetComponentsInChildren<Transform>();//gets all transforms including parent
        nodes = new List<Transform>();
        //filter out parent
        for(int i= 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != transform)
                nodes.Add(pathTransforms[i]);
        }

        for(int i = 0; i < nodes.Count; i++)
        {
            Vector3 currentNode = nodes[i].position;
            Vector3 previousNode = Vector3.zero;
            if (i > 0)
                previousNode = nodes[i - 1].position;
            else
                previousNode = nodes[nodes.Count - 1].position;

            Gizmos.DrawLine(previousNode, currentNode);
            Gizmos.DrawWireSphere(currentNode, 1.5f);
        }
    }
}
