using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgregatorNS;

namespace TestClient
{
	class Program
	{
		static void Main(string[] args)
		{
			Agregator agr = new NewAgregator();
			agr.AggregatorLogger("debug", "Test message");
			(agr as NewAgregator).Url = "http://10.1.1.1:8508/"; //"https://httpbin.org/post";// "https://requestb.in/1n26mtx1";// ;"http://www.albahari.com/EchoPost.aspx";// 

			CheckResponse checkResponse = (agr as NewAgregator).Check(123,
				//@""
				//@"<Data><Request><RqHeader><idMsgType>111</idMsgType><IdService>510</IdService><IdSubService>0</IdSubService><IdClient>10044163</IdClient></RqHeader><body></body><RqFooter><MsgDateTime>2016.10.28 01:05:12</MsgDateTime><IdTerminal>625350</IdTerminal><idKey>999900000384</idKey></RqFooter></Request><SignMsg>eAJjM6KjwzDKX/Do0QX7nnnGzYfMdQyPRP7Nn+e12QA96+NQnwvwQyPzPY2elN1xo6EjhBWI2DZbh8z9HUpeHYT74EaS5nwYgNWZ3gmo1+Z3DJcyNjI9n7ZWHRpPSV/iGb70/4SrcwBVVdZLdpe8xZFShPlrqZfZ8uf6ncjeHR8=</SignMsg></Data>"
				@"<Request><RqHeader><idMsgType>111</idMsgType><IdService>115</IdService><IdSubService>0</IdSubService><IdClient>0674040404</IdClient></RqHeader><body></body><RqFooter><MsgDateTime>2016.11.30 13:45:14</MsgDateTime><IdTerminal>345098</IdTerminal><idKey>999900000009</idKey></RqFooter></Request><SignMsg>this is RSA sign from buyer</SignMsg>"
				);
				//"Кириличні символи=їЇєЄЮюяЯІі");
			Console.WriteLine("You posted: Кириличні символи:їЇєЄЮюяЯІі");
			Console.WriteLine(checkResponse.JsonValue);

			Console.WriteLine("All Ok.");
			Console.ReadKey();
		}
	}
}
