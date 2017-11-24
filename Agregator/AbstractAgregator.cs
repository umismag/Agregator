using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AgregatorNS
{
	public abstract class Agregator
	{
		//​​Метод ​​инициализации ​​агрегатора
		public abstract void Initialize(string url, string account, string secretKey, string terminalId, string certificateName, string certificatePassword);

		// Метод  выполняющий  проверку  возможности  оплатить  услугу  у агрегатора
		public abstract CheckResponse Check(int providerProductId, string jsonFields);

		//​​Метод ​​выполняющий ​​оплату ​​услуги ​​у ​​агрегатора
		public abstract PayResponse Pay(int providerProductId, string jsonFields, string idempotentKey);

		//Метод выполняющий расчет или получение комиссии по услуге агрегатора
		public abstract CommissionResponse Commission(int providerProductId, string jsonFields);

		//Метод  возвращающий  содержание  сервиса  агрегатора  в  виде полученном ​​от ​​агрегатора
		public abstract string GetFields(int providerProductId);

		//​Метод ​​возвращающий ​​все ​​сервисы ​-​ ​​услуги ​​агрегатора
		public abstract object GetServices();

		//​​Метод ​​выполняющий ​​логирование ​​библиотеки
		public void AggregatorLogger(string TypeMessage, string Message)
		{
			//TypeMessage - тип ​​логируемого ​с​ообщения, может ​​принимать ​з​начения: ​​debug, ​i​nfo, ​​error
			//Message - сообщение ​​для ​​логирования
			if (TypeMessage != "debug" && TypeMessage != "info" && TypeMessage != "error")
				throw new ArgumentOutOfRangeException("Type message not valid.");

			string logFile = "Agregator.log";
			StringBuilder contents = new StringBuilder();
			contents.AppendLine(DateTime.Now.ToString()+
				"\tType="+TypeMessage+
				"\tMessage="+Message);
			try
			{
				File.AppendAllText(logFile,contents.ToString());
			}
			catch (Exception)
			{
				//throw;
			}   
		}
	}

	public class CommissionResponse
	{
		int providerProductId;
		double providerCommission;
	}

	public class PayResponse
	{
		string providerTransactionId;
		int state;
		string errorCode;
		string errorMassage;
		string jsonValue;
		string idempotentKey;
	}

	public class CheckResponse
	{
		bool status;
		public bool Status
		{
			get => status;
			set
			{
				status = value;
				if (value)
				{
					ErrorCode = null;
					ErrorMessage = null;
				}
			}
		}

		string errorCode;
		public string ErrorCode
		{
			get => errorCode;
			set => errorCode = value;
		}

		string errorMessage;
		public string ErrorMessage
		{
			get => errorMessage;
			set => errorMessage = value;
		}

		string jsonValue;
		public string JsonValue
		{
			get => jsonValue;
			set => jsonValue = value;
		}
	}
}
