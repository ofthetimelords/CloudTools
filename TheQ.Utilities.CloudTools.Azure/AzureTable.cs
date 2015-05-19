// <copyright file="AzureTable.cs" company="nett">
//      Copyright (c) 2015 All Right Reserved, http://q.nett.gr
//      Please see the License.txt file for more information. All other rights reserved.
// </copyright>
// <author>James Kavakopoulos</author>
// <email>ofthetimelords@gmail.com</email>
// <date>2015/02/06</date>
// <summary>
// 
// </summary>


using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage.Table;

using TheQ.Utilities.CloudTools.Storage.Internal;
using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;

namespace TheQ.Utilities.CloudTools.Azure
{
	public class AzureTable : ITable
	{
		private readonly CloudTable _tableReference;



		/// <summary>
		///     Initializes a new instance of the <see cref="AzureTable" /> class.
		/// </summary>
		/// <param name="queue">The actual <see cref="CloudTable" /> instance.</param>
		public AzureTable(CloudTable table)
		{
			Guard.NotNull(table, "table");

			this._tableReference = table;
		}



		public string Name { get; set; }
		///// <summary>
		///// A factory method that creates a query that can be modified using LINQ. The query may be subsequently executed using one of the execution methods available for <see cref="T:Microsoft.WindowsAzure.Storage.Table.CloudTable"/>,
		/////             such as <see cref="M:Microsoft.WindowsAzure.Storage.Table.CloudTable.ExecuteQuery(Microsoft.WindowsAzure.Storage.Table.TableQuery,Microsoft.WindowsAzure.Storage.Table.TableRequestOptions,Microsoft.WindowsAzure.Storage.OperationContext)"/>, <see cref="M:Microsoft.WindowsAzure.Storage.Table.CloudTable.ExecuteQuerySegmented(Microsoft.WindowsAzure.Storage.Table.TableQuery,Microsoft.WindowsAzure.Storage.Table.TableContinuationToken,Microsoft.WindowsAzure.Storage.Table.TableRequestOptions,Microsoft.WindowsAzure.Storage.OperationContext)"/>, or <see cref="M:Microsoft.WindowsAzure.Storage.Table.CloudTable.ExecuteQuerySegmentedAsync(Microsoft.WindowsAzure.Storage.Table.TableQuery,Microsoft.WindowsAzure.Storage.Table.TableContinuationToken)"/>.
		///// 
		///// </summary>
		///// <typeparam name="TElement">The entity type of the query.</typeparam>
		///// <returns>
		///// A <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableQuery"/> object, specialized for type <c>TElement</c>, that may subsequently be executed.
		///// </returns>
		///// 
		///// <remarks>
		///// The <see cref="N:Microsoft.WindowsAzure.Storage.Table.Queryable"/> namespace includes extension methods for the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableQuery"/> object,
		/////             including <see cref="M:WithOptions"/>, <see cref="M:WithContext"/>, and <see cref="M:AsTableQuery"/>. To use these methods, include a <c>using</c>
		/////             statement that references the <see cref="N:Microsoft.WindowsAzure.Storage.Table.Queryable"/> namespace.
		///// 
		///// </remarks>
		//ITableQuery<TElement> CreateQuery<TElement>() where TElement : ITableBaseEntity, new();


		///// <summary>
		///// Executes a query on a table.
		///// 
		///// </summary>
		///// <typeparam name="TElement">The entity type of the query.</typeparam><param name="query">A TableQuery instance specifying the table to query and the query parameters to use, specialized for a type <c>TElement</c>.</param><param name="requestOptions">A <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableRequestOptions"/> object that specifies additional options for the request.</param><param name="operationContext">An <see cref="T:Microsoft.WindowsAzure.Storage.OperationContext"/> object that represents the context for the current operation.</param>
		///// <returns>
		///// An enumerable collection, specialized for type <c>TElement</c>, of the results of executing the query.
		///// </returns>
		//IEnumerable<TElement> ExecuteQuery<TElement>(TableQuery<TElement> query) where TElement : ITableBaseEntity, new();


		///// <summary>
		///// Executes a query on a table in segmented mode.
		///// 
		///// </summary>
		///// <typeparam name="TElement">The entity type of the query.</typeparam><param name="query">A <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableQuery"/> instance specifying the table to query and the query parameters to use, specialized for a type <c>TElement</c>.</param><param name="token">A <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableContinuationToken"/> object representing a continuation token from the server when the operation returns a partial result.</param><param name="requestOptions">A <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableRequestOptions"/> object that specifies additional options for the request.</param><param name="operationContext">An <see cref="T:Microsoft.WindowsAzure.Storage.OperationContext"/> object that represents the context for the current operation.</param>
		///// <returns>
		///// A <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableQuerySegment`1"/>, specialized for type <c>TElement</c>, containing the results of executing the query.
		///// </returns>
		//TableQuerySegment<TElement> ExecuteQuerySegmented<TElement>(TableQuery<TElement> query, TableContinuationToken token) where TElement : ITableBaseEntity, new();


		///// <summary>
		///// Initiates an asynchronous operation to query a table in segmented mode.
		///// 
		///// </summary>
		///// <typeparam name="TElement">The entity type of the query.</typeparam><param name="query">A <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableQuery"/> instance specifying the table to query and the query parameters to use, specialized for a type <c>TElement</c>.</param><param name="token">A <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableContinuationToken"/> object representing a continuation token from the server when the operation returns a partial result.</param>
		///// <returns>
		///// A <see cref="T:System.Threading.Tasks.Task`1"/> object that represents the asynchronous operation.
		///// </returns>
		//Task<TableQuerySegment<TElement>> ExecuteQuerySegmentedAsync<TElement>(TableQuery<TElement> query, TableContinuationToken token) where TElement : ITableBaseEntity, new();


		///// <summary>
		///// Initiates an asynchronous operation to query a table in segmented mode.
		///// 
		///// </summary>
		///// <typeparam name="TElement">The entity type of the query.</typeparam><param name="query">A <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableQuery"/> instance specifying the table to query and the query parameters to use, specialized for a type <c>TElement</c>.</param><param name="token">A <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableContinuationToken"/> object representing a continuation token from the server when the operation returns a partial result.</param><param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken"/> to observe while waiting for a task to complete.</param>
		///// <returns>
		///// A <see cref="T:System.Threading.Tasks.Task`1"/> object that represents the asynchronous operation.
		///// </returns>
		//Task<TableQuerySegment<TElement>> ExecuteQuerySegmentedAsync<TElement>(TableQuery<TElement> query, TableContinuationToken token, CancellationToken cancellationToken) where TElement : ITableBaseEntity, new();


		/// <summary>
		///     Creates a table.
		/// </summary>
		/// <param name="requestOptions">A <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableRequestOptions" /> object that specifies additional options for the request.</param>
		/// <param name="operationContext">An <see cref="T:Microsoft.WindowsAzure.Storage.OperationContext" /> object that represents the context for the current operation.</param>
		public void Create()
		{
			throw new NotImplementedException();
		}



		/// <summary>
		///     Initiates an asynchronous operation to create a table.
		/// </summary>
		/// <returns>
		///     A <see cref="T:System.Threading.Tasks.Task" /> object that represents the asynchronous operation.
		/// </returns>
		public Task CreateAsync()
		{
			throw new NotImplementedException();
		}



		/// <summary>
		///     Initiates an asynchronous operation to create a table.
		/// </summary>
		/// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for a task to complete.</param>
		/// <returns>
		///     A <see cref="T:System.Threading.Tasks.Task" /> object that represents the asynchronous operation.
		/// </returns>
		public Task CreateAsync(CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}



		/// <summary>
		///     Creates the table if it does not already exist.
		/// </summary>
		/// <param name="requestOptions">A <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableRequestOptions" /> object that specifies additional options for the request.</param>
		/// <param name="operationContext">An <see cref="T:Microsoft.WindowsAzure.Storage.OperationContext" /> object that represents the context for the current operation.</param>
		/// <returns>
		///     <c>true</c> if table was created; otherwise, <c>false</c>.
		/// </returns>
		public bool CreateIfNotExists()
		{
			throw new NotImplementedException();
		}



		/// <summary>
		///     Initiates an asynchronous operation to create a table if it does not already exist.
		/// </summary>
		/// <returns>
		///     A <see cref="T:System.Threading.Tasks.Task`1" /> object of type <c>bool</c> that represents the asynchronous operation.
		/// </returns>
		public Task<bool> CreateIfNotExistsAsync()
		{
			throw new NotImplementedException();
		}



		/// <summary>
		///     Initiates an asynchronous operation to create a table if it does not already exist.
		/// </summary>
		/// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for a task to complete.</param>
		/// <returns>
		///     A <see cref="T:System.Threading.Tasks.Task`1" /> object of type <c>bool</c> that represents the asynchronous operation.
		/// </returns>
		public Task<bool> CreateIfNotExistsAsync(CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}



		/// <summary>
		///     Deletes a table.
		/// </summary>
		/// <param name="requestOptions">A <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableRequestOptions" /> object that specifies additional options for the request.</param>
		/// <param name="operationContext">An <see cref="T:Microsoft.WindowsAzure.Storage.OperationContext" /> object that represents the context for the current operation.</param>
		public void Delete()
		{
			throw new NotImplementedException();
		}



		/// <summary>
		///     Initiates an asynchronous operation to delete a table.
		/// </summary>
		/// <returns>
		///     A <see cref="T:System.Threading.Tasks.Task" /> object that represents the asynchronous operation.
		/// </returns>
		public Task DeleteAsync()
		{
			throw new NotImplementedException();
		}



		/// <summary>
		///     Initiates an asynchronous operation to delete a table.
		/// </summary>
		/// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for a task to complete.</param>
		/// <returns>
		///     A <see cref="T:System.Threading.Tasks.Task" /> object that represents the asynchronous operation.
		/// </returns>
		public Task DeleteAsync(CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}



		/// <summary>
		///     Deletes the table if it exists.
		/// </summary>
		/// <param name="requestOptions">A <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableRequestOptions" /> object that specifies additional options for the request.</param>
		/// <param name="operationContext">An <see cref="T:Microsoft.WindowsAzure.Storage.OperationContext" /> object that represents the context for the current operation.</param>
		/// <returns>
		///     <c>true</c> if the table was deleted; otherwise, <c>false</c>.
		/// </returns>
		public bool DeleteIfExists()
		{
			throw new NotImplementedException();
		}



		/// <summary>
		///     Initiates an asynchronous operation to delete the table if it exists.
		/// </summary>
		/// <returns>
		///     A <see cref="T:System.Threading.Tasks.Task`1" /> object of type <c>bool</c> that represents the asynchronous operation.
		/// </returns>
		public Task<bool> DeleteIfExistsAsync()
		{
			throw new NotImplementedException();
		}



		/// <summary>
		///     Initiates an asynchronous operation to delete the table if it exists.
		/// </summary>
		/// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for a task to complete.</param>
		/// <returns>
		///     A <see cref="T:System.Threading.Tasks.Task`1" /> object of type <c>bool</c> that represents the asynchronous operation.
		/// </returns>
		public Task<bool> DeleteIfExistsAsync(CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}



		/// <summary>
		///     Checks whether the table exists.
		/// </summary>
		/// <param name="requestOptions">A <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableRequestOptions" /> object that specifies additional options for the request.</param>
		/// <param name="operationContext">An <see cref="T:Microsoft.WindowsAzure.Storage.OperationContext" /> object that represents the context for the current operation.</param>
		/// <returns>
		///     <c>true</c> if table exists; otherwise, <c>false</c>.
		/// </returns>
		public bool Exists()
		{
			throw new NotImplementedException();
		}



		/// <summary>
		///     Initiates an asynchronous operation to determine whether a table exists.
		/// </summary>
		/// <returns>
		///     A <see cref="T:System.Threading.Tasks.Task`1" /> object of type <c>bool</c> that represents the asynchronous operation.
		/// </returns>
		public Task<bool> ExistsAsync()
		{
			throw new NotImplementedException();
		}



		/// <summary>
		///     Initiates an asynchronous operation to determine whether a table exists.
		/// </summary>
		/// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for a task to complete.</param>
		/// <returns>
		///     A <see cref="T:System.Threading.Tasks.Task`1" /> object of type <c>bool</c> that represents the asynchronous operation.
		/// </returns>
		public Task<bool> ExistsAsync(CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}



		ITableResult Execute(ITableOperation operation)
		{
			throw new NotImplementedException();
		}



		/// <summary>
		///     Initiates an asynchronous operation that executes an asynchronous table operation.
		/// </summary>
		/// <param name="operation">A <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation" /> object that represents the operation to perform.</param>
		/// <returns>
		///     A <see cref="T:System.Threading.Tasks.Task`1" /> object of type <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableResult" /> that represents the asynchronous operation.
		/// </returns>
		public Task<ITableResult> ExecuteAsync(ITableOperation operation)
		{
			throw new NotImplementedException();
		}



		/// <summary>
		///     Initiates an asynchronous operation that executes an asynchronous table operation.
		/// </summary>
		/// <param name="operation">A <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation" /> object that represents the operation to perform.</param>
		/// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for a task to complete.</param>
		/// <returns>
		///     A <see cref="T:System.Threading.Tasks.Task`1" /> object of type <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableResult" /> that represents the asynchronous operation.
		/// </returns>
		public Task<ITableResult> ExecuteAsync(ITableOperation operation, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}



		/// <summary>
		///     Executes a batch operation on a table as an atomic operation.
		/// </summary>
		/// <param name="batch">The <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation" /> object representing the operations to execute on the table.</param>
		/// <param name="requestOptions">A <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableRequestOptions" /> object that specifies additional options for the request.</param>
		/// <param name="operationContext">An <see cref="T:Microsoft.WindowsAzure.Storage.OperationContext" /> object that represents the context for the current operation.</param>
		/// <returns>
		///     An enumerable collection of <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableResult" /> objects that contains the results, in order, of each operation in the
		///     <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation" /> on the table.
		/// </returns>
		public IList<ITableResult> ExecuteBatch(ITableBatchOperation batch)
		{
			throw new NotImplementedException();
		}



		/// <summary>
		///     Initiates an asynchronous operation to execute a batch of operations on a table.
		/// </summary>
		/// <param name="batch">The <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation" /> object representing the operations to execute on the table.</param>
		/// <returns>
		///     A <see cref="T:System.Threading.Tasks.Task`1" /> object that is list of type <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableResult" /> that represents the asynchronous operation.
		/// </returns>
		public Task<IList<ITableResult>> ExecuteBatchAsync(ITableBatchOperation batch)
		{
			throw new NotImplementedException();
		}



		/// <summary>
		///     Initiates an asynchronous operation to execute a batch of operations on a table.
		/// </summary>
		/// <param name="batch">The <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation" /> object representing the operations to execute on the table.</param>
		/// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for a task to complete.</param>
		/// <returns>
		///     A <see cref="T:System.Threading.Tasks.Task`1" /> object that is list of type <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableResult" /> that represents the asynchronous operation.
		/// </returns>
		public Task<IList<ITableResult>> ExecuteBatchAsync(ITableBatchOperation batch, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}



		/// <summary>
		///     Performs an <see langword="implicit" /> conversion from <see cref="AzureTable" /> to <see cref="CloudTable" /> .
		/// </summary>
		/// <param name="Table">The <see cref="AzureTable" /> instance.</param>
		/// <returns>
		///     The underlying <see cref="CloudTable" /> instance.
		/// </returns>
		public static implicit operator CloudTable(AzureTable Table)
		{
			return Table._tableReference;
		}



		/// <summary>
		///     Performs an <see langword="implicit" /> conversion from <see cref="CloudTable" /> to <see cref="AzureTable" /> .
		/// </summary>
		/// <param name="Table">The <see cref="CloudTable" /> instance.</param>
		/// <returns>
		///     A <see cref="AzureTable" /> wrapper.
		/// </returns>
		public static implicit operator AzureTable(CloudTable table)
		{
			return new AzureTable(table);
		}



		/// <summary>
		///     Creates an <see cref="AzureTable" /> from a <see cref="CloudTable" /> instance.
		/// </summary>
		/// <param name="Table">The <see cref="CloudTable" /> instance.</param>
		/// <returns>
		///     A <see cref="AzureTable" /> wrapper.
		/// </returns>
		public static AzureTable FromCloudTable(CloudTable table)
		{
			return new AzureTable(table);
		}



		/// <summary>
		///     Retrieves the underlying <see cref="CloudTable" /> instance from this <see cref="AzureTable" /> instance.
		/// </summary>
		/// <param name="Table">The underlying <see cref="CloudTable" /> instance.</param>
		/// <returns>
		///     An <see cref="AzureTable" /> wrapper.
		/// </returns>
		public static CloudTable ToCloudTableMessage(AzureTable table)
		{
			return table._tableReference;
		}
	}
}