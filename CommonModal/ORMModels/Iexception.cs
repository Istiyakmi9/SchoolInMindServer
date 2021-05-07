using System;

namespace CommonModal.ORMModels
{
	public class Iexception
	{
		public string ExceptionUniqueCode { set; get; }
		public string StackTrace { set; get; }
		public string MethodFullyQualifiedName { set; get; }
		public DateTime ExceptionTime { set; get; }
		public bool? IsProcedureException { set; get; }
		public bool? IsCodeException { set; get; }
	}
}
