using System;

namespace MPGameLib.Extensions
{
    public static class EnumExtensions
    {
        public static int IntValue(this Enum argEnum)
        {
            return (int) (object)argEnum;
        }
    }
}