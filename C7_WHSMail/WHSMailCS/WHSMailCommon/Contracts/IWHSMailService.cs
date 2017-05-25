using System.Collections.Generic;
using System.ServiceModel;
using WHSMailCommon.Entities;

namespace WHSMailCommon.Contracts
{
	// list of methods of the WHSMailService service
	[ServiceContract()]
	public interface IWHSMailService
	{
		[OperationContract]
		List<Folder> GetFolders();

		[OperationContract]
		List<Email> GetMessages(string entryID, int numPerPage, int pageNum);

		[OperationContract]
		Email GetMessage(string entryID);
	}
}
