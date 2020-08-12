using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MPGameLib.Extensions
{
    public static class GenericExtensions
    {
        /// <summary>
        /// 깊은 복사를 한다. (조건: 여기 들어오는 모든 클래스는 Serializable가 되있어야 한다.) 
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T DeepClone<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T) formatter.Deserialize(ms);
            }
        }
    }
}