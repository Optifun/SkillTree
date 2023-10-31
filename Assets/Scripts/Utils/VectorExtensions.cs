using UnityEngine;

namespace SkillTree.Utils
{
    public static class VectorExtensions
    {
        public static System.Numerics.Vector3 ToVector(this Vector3 v)
        {
            return new System.Numerics.Vector3(v.x, v.y, v.z);
        }

        public static Vector3 ConvertToUnityVector(this System.Numerics.Vector3 v)
        {
            return new Vector3(v.X, v.Y, v.Z);
        }

        public static Vector2 ToVector2(this Vector3 v)
        {
            return new Vector2(v.x, v.y);
        }

        public static System.Numerics.Vector2 ToVector2(this System.Numerics.Vector3 v)
        {
            return new System.Numerics.Vector2(v.X, v.Y);
        }

        public static System.Numerics.Vector3 ToVector3(this System.Numerics.Vector2 v)
        {
            return new System.Numerics.Vector3(v.X, v.Y, 0);
        }
    }
}