using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Lyntia.Utilities
{
	 class TestDataUtils
	{
		private readonly static TestDataUtils _instance = new TestDataUtils();

		private TestDataUtils()
		{
		}

		public static TestDataUtils Instance
		{
			get
			{
				return _instance;
			}
		}

		Dictionary<String, String> database = new Dictionary<String, String>();				
		XmlNodeList testsCase;

		public Dictionary<String, String> GetDatabase()
		{
			return database;
		}

		/**
		 * Read all data of the csv and store as a database.
		 */
		public void testDataReader(string filename, string testCaseName)
		{
			//Create the XmlDocument.
			XmlDocument doc = new XmlDocument();
			doc.Load(filename);

			//Display all the book titles.
			testsCase = doc.GetElementsByTagName(testCaseName);					
		}

		public Dictionary<String, String> generarDiccionario(XmlNodeList datos)
		{
			Dictionary<String, String> diccionario = new Dictionary<String, String>();
			foreach (XmlNode dato in datos)
			{
				diccionario.TryAdd(dato.Name, dato.InnerText);
			}
			return diccionario;
		}

		public String getData(string objectName)
		{
			return database[objectName];
		}

		public Dictionary<string, string> getDatosRepositorio(string nodoRepositorio)
		{								
			return generarDiccionario(testsCase[0][nodoRepositorio].ChildNodes);			
		}
	}
}
