using UnityEngine;

namespace SkillTree.View
{
    public class SkillConnectionView : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;

        public void DrawLine(Vector3 from, Vector3 to)
        {
            _lineRenderer.SetPositions(new Vector3[] {from, to});
            _lineRenderer.widthMultiplier = 0.1f;
        }
    }
}