using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace RequestData
{
	public class RtHeader
	{
		public int idMsgType { get; set; }
	}

	public class Body
	{
		Dictionary<string, string> Items { get; set; } = new Dictionary<string, string>();
	}

	public class RFooter
	{
		string msgDateTime;
		public string MsgDateTime
		{
			get {return msgDateTime;}
			set {msgDateTime = value;}
		}

		string idKeyField;
		public string idKey
		{
			get { return idKeyField; }
			set { idKeyField = value; }
		}

		public RFooter() { }

		public RFooter(string msgDateTime, string idKey)
		{
			MsgDateTime = msgDateTime;
			this.idKey = idKey;
		}
	}

	public class Request
	{
		public RtHeader RtHeader { get; set; }
		public Body body { get; set; }
		public RFooter RFooter { get; set; }

		public Request() { }

		public Request(RtHeader rtHeader, Body body, RFooter rtFooter)
		{
			RtHeader = rtHeader;
			this.body = body;
			RFooter = rtFooter;
		}
	}
	


	public class Data
    {
		public Request Request { get; set; }
		public string SignMsg { get; set; }

		public Data() { }

		public Data(Request request, string signMsg)
		{
			Request = request;
			SignMsg = signMsg;
		}
    }

	public static class Serialization
	{
		public static string Obj2XMLstring(object obj)
		{
			string res;
			Encoding win1251 = Encoding.GetEncoding(1251);
			XmlSerializer xmlserializer = new XmlSerializer(typeof(Data));
			byte[] bytes;
			char[] chars;

			using (Stream stream=new MemoryStream())
			{
				xmlserializer.Serialize(stream, obj);
				stream.Seek(0, SeekOrigin.Begin);

				bytes = new byte[stream.Length];
				stream.ReadAsync(bytes, 0, (int)stream.Length);

				chars = new char[win1251.GetCharCount(bytes)];
				win1251.GetDecoder().GetChars(bytes, 0, bytes.Length, chars, 0, true);
				res =new string(chars);
			}
			return res;
		}

	}
}
