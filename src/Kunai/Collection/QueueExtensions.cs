using System.Collections.Generic;

namespace Kunai.CollectionExt
{
	public static class QueueExtensions
	{
		public static void EnqueueAll<T>(this Queue<T> queue, IEnumerable<T> items)
		{
			foreach (var item in items)
				queue.Enqueue(item);
		}
		
	}
}
