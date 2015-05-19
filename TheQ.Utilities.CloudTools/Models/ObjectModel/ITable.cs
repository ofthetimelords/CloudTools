// <copyright file="ITable.cs" company="nett">
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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;



namespace TheQ.Utilities.CloudTools.Storage.Models.ObjectModel
{
	/// <summary>
	///     Defines a table, with the minimum required functionality for CloudTools.
	/// </summary>
	public interface ITable
	{
		/// <summary>
		/// Gets the name of the table.
		/// 
		/// </summary>
		/// 
		/// <value>
		/// A string containing the name of the table.
		/// </value>
		string Name { get; }



		/// <summary>
		/// Executes an operation on a table.
		/// 
		/// </summary>
		/// <param name="operation">A <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation"/> object that represents the operation to perform.</param><param name="requestOptions">A <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableRequestOptions"/> object that specifies additional options for the request.</param><param name="operationContext">An <see cref="T:Microsoft.WindowsAzure.Storage.OperationContext"/> object that represents the context for the current operation.</param>
		/// <returns>
		/// A <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableResult"/> object.
		/// </returns>
		ITableResult Execute(ITableOperation operation);



		/// <summary>
		/// Initiates an asynchronous operation that executes an asynchronous table operation.
		/// 
		/// </summary>
		/// <param name="operation">A <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation"/> object that represents the operation to perform.</param>
		/// <returns>
		/// A <see cref="T:System.Threading.Tasks.Task`1"/> object of type <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableResult"/> that represents the asynchronous operation.
		/// </returns>
		Task<ITableResult> ExecuteAsync(ITableOperation operation);



		/// <summary>
		/// Initiates an asynchronous operation that executes an asynchronous table operation.
		/// 
		/// </summary>
		/// <param name="operation">A <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableOperation"/> object that represents the operation to perform.</param><param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken"/> to observe while waiting for a task to complete.</param>
		/// <returns>
		/// A <see cref="T:System.Threading.Tasks.Task`1"/> object of type <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableResult"/> that represents the asynchronous operation.
		/// </returns>
		Task<ITableResult> ExecuteAsync(ITableOperation operation, CancellationToken cancellationToken);



		/// <summary>
		/// Executes a batch operation on a table as an atomic operation.
		/// 
		/// </summary>
		/// <param name="batch">The <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation"/> object representing the operations to execute on the table.</param><param name="requestOptions">A <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableRequestOptions"/> object that specifies additional options for the request.</param><param name="operationContext">An <see cref="T:Microsoft.WindowsAzure.Storage.OperationContext"/> object that represents the context for the current operation.</param>
		/// <returns>
		/// An enumerable collection of <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableResult"/> objects that contains the results, in order, of each operation in the <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation"/> on the table.
		/// </returns>
		IList<ITableResult> ExecuteBatch(ITableBatchOperation batch);




		/// <summary>
		/// Initiates an asynchronous operation to execute a batch of operations on a table.
		/// 
		/// </summary>
		/// <param name="batch">The <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation"/> object representing the operations to execute on the table.</param>
		/// <returns>
		/// A <see cref="T:System.Threading.Tasks.Task`1"/> object that is list of type <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableResult"/> that represents the asynchronous operation.
		/// </returns>
		Task<IList<ITableResult>> ExecuteBatchAsync(ITableBatchOperation batch);



		/// <summary>
		/// Initiates an asynchronous operation to execute a batch of operations on a table.
		/// 
		/// </summary>
		/// <param name="batch">The <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableBatchOperation"/> object representing the operations to execute on the table.</param><param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken"/> to observe while waiting for a task to complete.</param>
		/// <returns>
		/// A <see cref="T:System.Threading.Tasks.Task`1"/> object that is list of type <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableResult"/> that represents the asynchronous operation.
		/// </returns>
		Task<IList<ITableResult>> ExecuteBatchAsync(ITableBatchOperation batch, CancellationToken cancellationToken);




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
		/// Creates a table.
		/// 
		/// </summary>
		/// <param name="requestOptions">A <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableRequestOptions"/> object that specifies additional options for the request.</param><param name="operationContext">An <see cref="T:Microsoft.WindowsAzure.Storage.OperationContext"/> object that represents the context for the current operation.</param>
		void Create();




		/// <summary>
		/// Initiates an asynchronous operation to create a table.
		/// 
		/// </summary>
		/// 
		/// <returns>
		/// A <see cref="T:System.Threading.Tasks.Task"/> object that represents the asynchronous operation.
		/// </returns>
		Task CreateAsync();



		/// <summary>
		/// Initiates an asynchronous operation to create a table.
		/// 
		/// </summary>
		/// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken"/> to observe while waiting for a task to complete.</param>
		/// <returns>
		/// A <see cref="T:System.Threading.Tasks.Task"/> object that represents the asynchronous operation.
		/// </returns>
		Task CreateAsync(CancellationToken cancellationToken);



		/// <summary>
		/// Creates the table if it does not already exist.
		/// 
		/// </summary>
		/// <param name="requestOptions">A <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableRequestOptions"/> object that specifies additional options for the request.</param><param name="operationContext">An <see cref="T:Microsoft.WindowsAzure.Storage.OperationContext"/> object that represents the context for the current operation.</param>
		/// <returns>
		/// <c>true</c> if table was created; otherwise, <c>false</c>.
		/// </returns>
		bool CreateIfNotExists();



		/// <summary>
		/// Initiates an asynchronous operation to create a table if it does not already exist.
		/// 
		/// </summary>
		/// 
		/// <returns>
		/// A <see cref="T:System.Threading.Tasks.Task`1"/> object of type <c>bool</c> that represents the asynchronous operation.
		/// </returns>
		Task<bool> CreateIfNotExistsAsync();



		/// <summary>
		/// Initiates an asynchronous operation to create a table if it does not already exist.
		/// 
		/// </summary>
		/// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken"/> to observe while waiting for a task to complete.</param>
		/// <returns>
		/// A <see cref="T:System.Threading.Tasks.Task`1"/> object of type <c>bool</c> that represents the asynchronous operation.
		/// </returns>
		Task<bool> CreateIfNotExistsAsync(CancellationToken cancellationToken);



		/// <summary>
		/// Deletes a table.
		/// 
		/// </summary>
		/// <param name="requestOptions">A <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableRequestOptions"/> object that specifies additional options for the request.</param><param name="operationContext">An <see cref="T:Microsoft.WindowsAzure.Storage.OperationContext"/> object that represents the context for the current operation.</param>
		void Delete();



		/// <summary>
		/// Initiates an asynchronous operation to delete a table.
		/// 
		/// </summary>
		/// 
		/// <returns>
		/// A <see cref="T:System.Threading.Tasks.Task"/> object that represents the asynchronous operation.
		/// </returns>
		Task DeleteAsync();



		/// <summary>
		/// Initiates an asynchronous operation to delete a table.
		/// 
		/// </summary>
		/// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken"/> to observe while waiting for a task to complete.</param>
		/// <returns>
		/// A <see cref="T:System.Threading.Tasks.Task"/> object that represents the asynchronous operation.
		/// </returns>
		Task DeleteAsync(CancellationToken cancellationToken);



		/// <summary>
		/// Deletes the table if it exists.
		/// 
		/// </summary>
		/// <param name="requestOptions">A <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableRequestOptions"/> object that specifies additional options for the request.</param><param name="operationContext">An <see cref="T:Microsoft.WindowsAzure.Storage.OperationContext"/> object that represents the context for the current operation.</param>
		/// <returns>
		/// <c>true</c> if the table was deleted; otherwise, <c>false</c>.
		/// </returns>
		bool DeleteIfExists();



		/// <summary>
		/// Initiates an asynchronous operation to delete the table if it exists.
		/// 
		/// </summary>
		/// 
		/// <returns>
		/// A <see cref="T:System.Threading.Tasks.Task`1"/> object of type <c>bool</c> that represents the asynchronous operation.
		/// </returns>
		Task<bool> DeleteIfExistsAsync();



		/// <summary>
		/// Initiates an asynchronous operation to delete the table if it exists.
		/// 
		/// </summary>
		/// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken"/> to observe while waiting for a task to complete.</param>
		/// <returns>
		/// A <see cref="T:System.Threading.Tasks.Task`1"/> object of type <c>bool</c> that represents the asynchronous operation.
		/// </returns>
		Task<bool> DeleteIfExistsAsync(CancellationToken cancellationToken);



		/// <summary>
		/// Checks whether the table exists.
		/// 
		/// </summary>
		/// <param name="requestOptions">A <see cref="T:Microsoft.WindowsAzure.Storage.Table.TableRequestOptions"/> object that specifies additional options for the request.</param><param name="operationContext">An <see cref="T:Microsoft.WindowsAzure.Storage.OperationContext"/> object that represents the context for the current operation.</param>
		/// <returns>
		/// <c>true</c> if table exists; otherwise, <c>false</c>.
		/// </returns>
		bool Exists();



		/// <summary>
		/// Initiates an asynchronous operation to determine whether a table exists.
		/// 
		/// </summary>
		/// 
		/// <returns>
		/// A <see cref="T:System.Threading.Tasks.Task`1"/> object of type <c>bool</c> that represents the asynchronous operation.
		/// </returns>
		Task<bool> ExistsAsync();



		/// <summary>
		/// Initiates an asynchronous operation to determine whether a table exists.
		/// 
		/// </summary>
		/// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken"/> to observe while waiting for a task to complete.</param>
		/// <returns>
		/// A <see cref="T:System.Threading.Tasks.Task`1"/> object of type <c>bool</c> that represents the asynchronous operation.
		/// </returns>
		Task<bool> ExistsAsync(CancellationToken cancellationToken);
	}
}