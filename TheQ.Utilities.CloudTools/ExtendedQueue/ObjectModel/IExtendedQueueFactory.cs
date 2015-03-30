using System.Linq;

using TheQ.Utilities.CloudTools.Storage.Models.ObjectModel;



namespace TheQ.Utilities.CloudTools.Storage.ExtendedQueue.ObjectModel
{
	/// <summary>
	/// <para>Allows the generation of <see cref="IExtendedQueue"/> objects.</para>
	/// <para><see cref="IExtendedQueueFactory"/> instances should initialise and apply any cross-cutting concerns and decorators beforehand.</para>
	/// </summary>
	public interface IExtendedQueueFactory
	{
		/// <summary>
		/// Creates an <see cref="IExtendedQueue"/> instance from a <see cref="IQueue"/> instance.
		/// </summary>
		/// <param name="original">The <see cref="IQueue"/> instance to wrap into an <see cref="IExtendedQueue"/> instance.</param>
		/// <returns>An <see cref="IExtendedQueue"/> instance backed by the <paramref name="original"/> instance.</returns>
		IExtendedQueue Create(IQueue original);
	}
}