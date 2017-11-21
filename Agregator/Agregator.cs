using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Http;
using RequestData;

namespace AgregatorNS
{
	public class NewAgregator : Agregator
	{
		string url;
		public string Url
		{
			get => url;
			set => url = value;
		}

		string account;
		public string Account
		{
			get => account;
			set => account = value;
		}

		string secretKey;
		public string SecretKey
		{
			get => secretKey;
			set => secretKey = value;
		}

		string terminalId = null;
		public string TerminalId
		{
			get => terminalId;
			set => terminalId = value;
		}

		string certificateName;
		public string CertificateName
		{
			get => certificateName;
			set => certificateName = value;
		}

		string certificatePassword;
		public string CertificatePassword
		{
			get => certificatePassword;
			set => certificatePassword = value;
		}

		HttpClient httpClient = new HttpClient();

		public NewAgregator()
		{
			Initialize("http://localhost", "User1", "VeryBigSecret", "001", "certificate1", "321");
		}

		public override CheckResponse Check(int providerProductId, string jsonFields)
		{
			CheckResponse checkResponse = new CheckResponse();

			Dictionary<string, string> dictionary = new Dictionary<string, string>();


			dictionary.Add("IdService", providerProductId.ToString());
			//Parse json fields

			//HttpContent content = new FormUrlEncodedContent(dictionary);

			Encoding win1251 = Encoding.GetEncoding(1251);
			HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, Url);
			httpRequestMessage.Content = new StringContent(jsonFields, win1251);

			try
			{
				using (HttpResponseMessage responseMessage = httpClient.SendAsync(httpRequestMessage).Result)
				{
					if (responseMessage.IsSuccessStatusCode)
					{
						var responseTask = responseMessage.Content.ReadAsStringAsync();
						responseTask.Wait();
						checkResponse.JsonValue = responseTask.Result;
						checkResponse.Status = true;
					}
					else
					{
						checkResponse.Status = false;
						checkResponse.ErrorCode = responseMessage.StatusCode.ToString();
						checkResponse.ErrorMessage = responseMessage.ReasonPhrase;

						throw new Exception(responseMessage.StatusCode + ": " + responseMessage.ReasonPhrase);
					}
				}
			}
			catch (WebException e)
			{
				checkResponse.Status = false;
				checkResponse.ErrorCode = null;
				checkResponse.ErrorMessage = e.Message;
			}
			catch (InvalidOperationException e)
			{
				checkResponse.Status = false;
				checkResponse.ErrorCode = null;
				checkResponse.ErrorMessage = e.Message;
			}
			catch (HttpRequestException e)
			{
				checkResponse.Status = false;
				checkResponse.ErrorCode = null;
				checkResponse.ErrorMessage = e.Message;
			}
			catch (Exception e)
			{
				checkResponse.Status = false;
				checkResponse.ErrorCode = null;
				checkResponse.ErrorMessage = e.Message;
			}

			//content.Dispose();
			return checkResponse;
		}

		public CheckResponse Check_old(int providerProductId, string jsonFields)
		{
			CheckResponse checkResponse = new CheckResponse();

			WebRequest webRequest = WebRequest.Create(Url);
			webRequest.Method = "POST";
			Encoding encoding = Encoding.GetEncoding(1251);
			byte[] dataArray = encoding.GetBytes(jsonFields);
			webRequest.ContentType = "text/XML";
			webRequest.ContentLength = dataArray.Length;



			try
			{
				using (Stream webRequestStream = webRequest.GetRequestStream())
				{
					webRequestStream.Write(dataArray, 0, dataArray.Length);
				}

				WebResponse webResponse = webRequest.GetResponse();

				using (Stream webResponseStream = webResponse.GetResponseStream())
				{
					using (StreamReader sr = new StreamReader(webResponseStream))
					{
						checkResponse.JsonValue = sr.ReadToEnd();
					}
				}
			}
			catch (WebException e)
			{
				checkResponse.Status = false;
				checkResponse.ErrorCode = e.Status.ToString();
				checkResponse.ErrorMessage = e.Message;
			}



			return checkResponse;
		}

		public override CommissionResponse Commission(int providerProductId, string jsonFields)
		{
			throw new NotImplementedException();
		}

		public override string GetFields(int providerProductId)
		{
			throw new NotImplementedException();
		}

		public override object GetServices()
		{
			throw new NotImplementedException();
		}

		public override void Initialize(string url, string account, string secretKey, string terminalId, string certificateName, string certificatePassword)
		{
			Url = url;
			Account = account;
			SecretKey = secretKey;
			TerminalId = terminalId;
			CertificateName = certificateName;
			CertificatePassword = certificatePassword;
		}

		public override PayResponse Pay(int providerProductId, string jsonFields, string idempotentKey)
		{
			throw new NotImplementedException();
		}
	}
}
