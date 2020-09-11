using Newtonsoft.Json;
using System;

namespace StandardApi.Common.Extentions
{
    public static class ObjectExtension
    {
        /// <summary>
        /// Perform a deep Copy of the object, using Json as a serialization method. NOTE: Private members are not cloned using this method.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T CloneJson<T>(this T source)
        {
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };
            var serializedObject = JsonConvert.SerializeObject(source);

            return JsonConvert.DeserializeObject<T>(serializedObject, deserializeSettings);
        }
    }
}
