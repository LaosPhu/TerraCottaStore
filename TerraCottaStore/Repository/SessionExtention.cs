using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TerraCottaStore.Repository
{
	public static class SessionExtention
	{
		public static void Setjson(this ISession session,String key,object value)
		{
			session.SetString(key, JsonConvert.SerializeObject(value));

		}
		public static T Getjson<T>(this ISession session, String key)
		{
			var sesiondata = session.GetString(key);
			return sesiondata == null ? default (T) : JsonConvert.DeserializeObject<T>(sesiondata);

		}
	}

}
