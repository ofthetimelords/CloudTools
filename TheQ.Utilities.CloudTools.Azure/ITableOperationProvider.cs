namespace TheQ.Utilities.CloudTools.Storage.Models.ObjectModel
{
	public interface ITableOperationProvider
	{
		ITableOperation Delete(ITableBaseEntity entity);

		ITableOperation Insert(ITableBaseEntity entity);

		ITableOperation InsertOrMerge(ITableBaseEntity entity);

		ITableOperation InsertOrReplace(ITableBaseEntity entity);

		ITableOperation Merge(ITableBaseEntity entity);

		ITableOperation Replace(ITableBaseEntity entity);

		ITableOperation Retrieve<TElement>(string partitionKey, string rowkey) where TElement : ITableBaseEntity;

		ITableOperation Retrieve(string partitionKey, string rowkey);
	}
}