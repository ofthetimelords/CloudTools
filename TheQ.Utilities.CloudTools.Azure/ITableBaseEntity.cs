using System;

namespace TheQ.Utilities.CloudTools.Storage.Models.ObjectModel
{
	public interface ITableBaseEntity
	{
		/// <summary>
		/// Gets or sets the entity's partition key.
		/// 
		/// </summary>
		/// 
		/// <value>
		/// The entity's partition key.
		/// </value>
		string PartitionKey { get; set; }

		/// <summary>
		/// Gets or sets the entity's row key.
		/// 
		/// </summary>
		/// 
		/// <value>
		/// The entity's row key.
		/// </value>
		string RowKey { get; set; }

		/// <summary>
		/// Gets or sets the entity's timestamp.
		/// 
		/// </summary>
		/// 
		/// <value>
		/// The entity's timestamp. The property is populated by the Windows Azure Table Service.
		/// </value>
		DateTimeOffset Timestamp { get; set; }

		/// <summary>
		/// Gets or sets the entity's current ETag.  Set this value to '*'
		///             in order to blindly overwrite an entity as part of an update
		///             operation.
		/// 
		/// </summary>
		/// 
		/// <value>
		/// The entity's timestamp.
		/// </value>
		string ETag { get; set; }
	}
}