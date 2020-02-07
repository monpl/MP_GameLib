using UnityEngine;

namespace MPGameLib.Extensions
{
    public static class VectorExtensions
    {
        public static Vector2 WithX(this Vector2 vector, float x)
        {
            return new Vector2(x, vector.y);
        }
        
        public static Vector2 WithY(this Vector2 vector, float y)
        {
            return new Vector2(vector.x, y);
        }
        
        public static Vector2 WithIncX(this Vector2 vector, float xInc)
        {
            return new Vector2(vector.x + xInc, vector.y);
        }

        public static Vector2 WithIncY(this Vector2 vector, float yInc)
        {
            return new Vector2(vector.x, vector.y + yInc);
        }

        public static Vector3 WithIncX(this Vector3 vector, float xInc)
        {
            return new Vector3(vector.x + xInc, vector.y, vector.z);
        }
        
        public static Vector3 WithIncY(this Vector3 vector, float yInc)
        {
            return new Vector3(vector.x, vector.y + yInc, vector.z);
        }
        
        public static Vector3 WithIncZ(this Vector3 vector, float zInc)
        {
            return new Vector3(vector.x, vector.y, vector.z + zInc);
        }

        public static Vector3 WithIncXYZ(this Vector3 vector, float inc)
        {
            return new Vector3(vector.x + inc, vector.y + inc, vector.z + inc);
        }

        public static Vector3 WithIncXY(this Vector3 vector, Vector2 inc)
        {
            return new Vector3(vector.x + inc.x, vector.y + inc.y, vector.z);
        }

        public static Vector2 WithMulX(this Vector2 vector, float xMul)
        {
            return new Vector2(vector.x * xMul, vector.y);
        }
        public static Vector2 WithMulY(this Vector2 vector, float yMul)
        {
            return new Vector2(vector.x, vector.y * yMul);
        }
        public static Vector2 WithMulXY(this Vector2 vector, float mul)
        {
            return new Vector2(vector.x * mul, vector.y * mul);
        }

    }
}