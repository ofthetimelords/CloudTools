using System.Collections.Generic;

namespace TheQ.Utilities.CloudTools.Storage.Models.ObjectModel
{
	public interface ITableBatchOperation
	{
		/// <summary>
		/// Gets or sets the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation"/> item at the specified index.
		/// 
		/// </summary>
		/// <param name="index">The index at which to get or set the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation"/> item.</param>
		/// <returns>
		/// The <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation"/> item at the specified index.
		/// </returns>
		ITableOperation this[int index] { get; set; }


		/// <summary>
		/// Gets the number of operations in this <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation"/>.
		/// 
		/// </summary>
		/// 
		/// <value>
		/// The number of operations in the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation"/>.
		/// </value>
		int Count { get; }


		/// <summary>
		/// Gets a value indicating whether the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation"/> is read-only.
		/// 
		/// </summary>
		/// 
		/// <value>
		/// <c>true</c> if the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation"/> is read-only; <c>false</c>, otherwise.
		/// </value>
		bool IsReadOnly { get; }



		/// <summary>
		/// Inserts a <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation"/> into the batch that retrieves an entity based on its row key and partition key. The entity will be deserialized into the specified class type which extends <see cref="T:Microsoft.WindowsAzure.Storage.Table.ITableEntity"/>.
		/// 
		/// </summary>
		/// <typeparam name="TElement">The class of type for the entity to retrieve.</typeparam><param name="partitionKey">A string containing the partition key of the entity to retrieve.</param><param name="rowKey">A string containing the row key of the entity to retrieve.</param>
		void Retrieve<TElement>(string partitionKey, string rowKey) where TElement : ITableBaseEntity;



		/// <summary>
		/// Adds a <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation"/> to the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation"/> that deletes the specified entity from a table.
		/// 
		/// </summary>
		/// <param name="entity">The entity to be deleted from the table.</param>
		void Delete(ITableBaseEntity entity);



		/// <summary>
		/// Adds a <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation"/> to the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation"/> that inserts the specified entity into a table.
		/// 
		/// </summary>
		/// <param name="entity">The entity to be inserted into the table.</param>
		void Insert(ITableBaseEntity entity);



		/// <summary>
		/// Adds a <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation"/> object that inserts the specified entity into the table as part of the batch operation.
		/// 
		/// </summary>
		/// <param name="entity">The entity to be inserted into the table.</param><param name="echoContent"><c>true</c> if the message payload should be returned in the response to the insert operation;otherwise, <c>false</c>.</param>
		void Insert(ITableBaseEntity entity, bool echoContent);



		/// <summary>
		/// Adds a <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation"/> to the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation"/> that inserts the specified entity into a table if the entity does not exist; if the entity does exist then its contents are merged with the provided entity.
		/// 
		/// </summary>
		/// <param name="entity">The entity whose contents are being inserted or merged.</param>
		void InsertOrMerge(ITableBaseEntity entity);



		/// <summary>
		/// Adds a <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation"/> to the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation"/> that inserts the specified entity into a table if the entity does not exist; if the entity does exist then its contents are replaced with the provided entity.
		/// 
		/// </summary>
		/// <param name="entity">The entity whose contents are being inserted or replaced.</param>
		void InsertOrReplace(ITableBaseEntity entity);



		/// <summary>
		/// Adds a <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation"/> to the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation"/> that merges the contents of the specified entity with the existing entity in a table.
		/// 
		/// </summary>
		/// <param name="entity">The entity whose contents are being merged.</param>
		void Merge(ITableBaseEntity entity);



		/// <summary>
		/// Adds a <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation"/> to the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation"/> that replaces the contents of the specified entity in a table.
		/// 
		/// </summary>
		/// <param name="entity">The entity whose contents are being replaced.</param>
		void Replace(ITableBaseEntity entity);



		/// <summary>
		/// Adds a <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation"/> to the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation"/> that retrieves an entity with the specified partition key and row key.
		/// 
		/// </summary>
		/// <param name="partitionKey">A string containing the partition key of the entity to retrieve.</param><param name="rowKey">A string containing the row key of the entity to retrieve.</param>
		void Retrieve(string partitionKey, string rowKey);



		/// <summary>
		/// Returns the zero-based index of the first occurrence of the specified <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation"/> item, or -1 if the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation"/> does not contain the item.
		/// 
		/// </summary>
		/// <param name="item">The <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation"/> item to search for.</param>
		/// <returns>
		/// The zero-based index of the first occurrence of item within the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation"/>, if found; otherwise, –1.
		/// </returns>
		int IndexOf(ITableOperation item);



		/// <summary>
		/// Inserts a <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation"/> into the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation"/> at the specified index.
		/// 
		/// </summary>
		/// <param name="index">The index at which to insert the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation"/>.</param><param name="item">The <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation"/> item to insert.</param>
		void Insert(int index, ITableOperation item);



		/// <summary>
		/// Removes the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation"/> at the specified index from the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation"/>.
		/// 
		/// </summary>
		/// <param name="index">The index of the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation"/> to remove from the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation"/>.</param>
		void RemoveAt(int index);



		/// <summary>
		/// Adds the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation"/> to the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation"/>.
		/// 
		/// </summary>
		/// <param name="item">The <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation"/> item to add to the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation"/>.</param>
		void Add(ITableOperation item);



		/// <summary>
		/// Clears all <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation"/> objects from the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation"/>.
		/// 
		/// </summary>
		void Clear();



		/// <summary>
		/// Returns <c>true</c> if this <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation"/> contains the specified element.
		/// 
		/// </summary>
		/// <param name="item">The <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation"/> item to search for.</param>
		/// <returns>
		/// <c>true</c> if the item is contained in the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation"/>; <c>false</c>, otherwise.
		/// </returns>
		bool Contains(ITableOperation item);



		/// <summary>
		/// Copies all the elements of the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation"/> to the specified one-dimensional array starting at the specified destination array index.
		/// 
		/// </summary>
		/// <param name="array">The one-dimensional array that is the destination of the elements copied from the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation"/>.</param><param name="arrayIndex">The index in the destination array at which copying begins.</param>
		void CopyTo(ITableOperation[] array, int arrayIndex);



		/// <summary>
		/// Removes the specified <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation"/> item from the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation"/>.
		/// 
		/// </summary>
		/// <param name="item">The <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation"/> item to remove.</param>
		/// <returns>
		/// <c>true</c> if the item was successfully removed; <c>false</c>, otherwise.
		/// </returns>
		bool Remove(ITableOperation item);



		/// <summary>
		/// Returns an <see cref="T:System.Collections.Generic.IEnumerator`1"/> for the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation"/>.
		/// 
		/// </summary>
		/// 
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> for <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation"/> items.
		/// </returns>
		IEnumerator<ITableOperation> GetEnumerator();
	}
}