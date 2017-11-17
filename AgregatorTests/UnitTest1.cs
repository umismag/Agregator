using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgregatorNS;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

namespace AgregatorTests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestCheckResponseClass()
		{
			bool status = true;
			CheckResponse cr = new CheckResponse() { Status = status };

			Assert.IsNull(cr.ErrorCode, "Порушена залежність CheckResponse.errorCode від CheckResponse.status");
			Assert.IsNull(cr.ErrorMessage, "Порушена залежність CheckResponse.errorMessage від CheckResponse.status");
		}

		[TestMethod]
		//тест не будет пройден, если не возникнет исключения 
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestLoggerAnyText()
		{
			Agregator agr = new NewAgregator();
			agr.AggregatorLogger("debugg", "");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestLoggerEmptyText()
		{
			Agregator agr = new NewAgregator();
			agr.AggregatorLogger("", "");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestLoggerNullText()
		{
			Agregator agr = new NewAgregator();
			agr.AggregatorLogger(null, "");
		}

		[TestMethod]
		public void TestInitializingUrlMethod()
		{
			NewAgregator agr = new NewAgregator();

			Assert.AreEqual(agr.Url, "http://localhost");
		}

		[TestMethod]
		public void TestInitializingAccountMethod()
		{
			NewAgregator agr = new NewAgregator();

			Assert.AreEqual(agr.Account, "User1");
		}

		[TestMethod]
		public void TestInitializingSecretKeyMethod()
		{
			NewAgregator agr = new NewAgregator();

			Assert.AreEqual(agr.SecretKey, "VeryBigSecret");
		}

		[TestMethod]
		public void TestInitializingTerminalIdMethod()
		{
			NewAgregator agr = new NewAgregator();

			Assert.AreEqual(agr.TerminalId, "001");
		}

		[TestMethod]
		public void TestInitializingCertificateNameMethod()
		{
			NewAgregator agr = new NewAgregator();

			Assert.AreEqual(agr.CertificateName, "certificate1");
		}

		[TestMethod]
		public void TestInitializingCertificatePasswordMethod()
		{
			NewAgregator agr = new NewAgregator();

			Assert.AreEqual(agr.CertificatePassword, "321");
		}

		class MockHandler : HttpMessageHandler
		{
			Func<HttpRequestMessage, HttpResponseMessage> responseGenerator;

			public MockHandler(Func<HttpRequestMessage, HttpResponseMessage> responseGenerator)
			{
				this.responseGenerator = responseGenerator;
			}

			protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
			{
				cancellationToken.ThrowIfCancellationRequested();
				var response = responseGenerator(request);
				response.RequestMessage = request;
				return Task.FromResult(response);
			}
		}

		[TestMethod]
		public void TestCheckPost()
		{
			var mocker = new MockHandler(request =>
			  new HttpResponseMessage(HttpStatusCode.OK)
			  { Content = new StringContent("Запит від адреси:" + request.RequestUri, Encoding.GetEncoding(1251)) });

			var client = new HttpClient(mocker);
			var response = client.GetAsync("http://www.linqpad.net").Result;
			string result = response.Content.ReadAsStringAsync().Result;
			Assert.AreEqual(result, "Запит від адреси:http://www.linqpad.net/");
		}

		[TestMethod]
		public void TestCheckPostByEcho()
		{
			Agregator agr = new NewAgregator();
			(agr as NewAgregator).Url = "https://www.albahari.com/EchoPost.aspx";

			CheckResponse checkResponse = agr.Check(123, "Кириличні символи:їЇєЄЮюяЯІі");
			Assert.AreEqual(checkResponse.JsonValue, "You posted: Кириличні символи:їЇєЄЮюяЯІі");
		}
	}
}
