﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace IntroSE.Kanban.Frontend.Model
{

	/// <summary>
	/// This class supports encoding objects into jsons and decoding objects from jsons
	/// </summary>
	public static class JsonEncoder
	{
		private readonly static JsonSerializerOptions options = new()
		{
			WriteIndented = true,
		};

		/// <summary>
		/// Encodes an object into a json
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static string ConvertToJson<T>(T obj)
		{
			return JsonSerializer.Serialize(obj, options);
		}

		/// <summary>
		/// Builds an object from a json
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="json"></param>
		/// <returns></returns>
		public static T BuildFromJson<T>(string json)
		{
			return JsonSerializer.Deserialize<T>(json, options);
		}
	}
}

