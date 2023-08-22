using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UNUMSelfPwdReset
{
    public static class SessionExtensions
    {

        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static void ClearSession(this ISession session, string key)
        {
            session.SetString(key, "");
        }
        public static bool CheckSession(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? false : true;
        }
    }

    public static class TempDataExtensions
    {
        public static void SetObjectAsJson(this ITempDataDictionary tempData, string key, object value)
        {
            tempData.Add(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            object value;
            tempData.TryGetValue(key, out value);
            return value == null ? null : JsonConvert.DeserializeObject<T>((string)value);
        }
    }
}
