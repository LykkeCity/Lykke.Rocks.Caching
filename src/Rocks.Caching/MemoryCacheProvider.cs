using System;
using Microsoft.Extensions.Caching.Memory;

namespace Rocks.Caching
{
	/// <summary>
	///     An implementation of <see cref="ICacheProvider" /> based on <see cref="MemoryCache" /> storage.
	/// </summary>
	public class MemoryCacheProvider : ICacheProvider
	{
	    private static readonly MemoryCache cache = new MemoryCache(new MemoryCacheOptions());

	    /// <summary>
		///     Gets cached object by <paramref name="key" />.
		///     Returns null if object was not found in cache.
		/// </summary>
		/// <param name="key">Cache key. Can not be null.</param>
		public object Get (string key)
		{
			return cache.Get (key);
		}


		/// <summary>
		///     Adds or updates object in the cache.
		/// </summary>
		/// <param name="key">Object key. Can not be null.</param>
		/// <param name="value">Object value.</param>
		/// <param name="parameters">Caching parameters.</param>
		public void Add (string key, object value, CachingParameters parameters)
		{
			if (parameters == null)
			{
			    throw new ArgumentNullException (nameof(parameters));
			}

		    if (value == null)
			{
				this.Remove (key);
				return;
			}

			if (parameters.NoCaching)
			{
			    return;
			}

		    var cacheEntryOptions = new MemoryCacheEntryOptions
		                            {
		                                Priority = GetCacheItemPriority(parameters.Priority)
		                            };

		    if (!parameters.Sliding)
		    {
		        cacheEntryOptions.AbsoluteExpiration = DateTimeOffset.Now + parameters.Expiration;
		    }
		    else
		    {
		        cacheEntryOptions.SlidingExpiration = parameters.Expiration;
		    }

			cache.Set (key, value, cacheEntryOptions);
		}

	    private static CacheItemPriority GetCacheItemPriority(CachePriority? priority)
	    {
	        switch (priority)
	        {
	            case CachePriority.Low:
	                return CacheItemPriority.Low;
	            case null:
	            case CachePriority.Normal:
	                return CacheItemPriority.Normal;
	            case CachePriority.High:
	                return CacheItemPriority.High;
	            case CachePriority.NotRemovable:
	                return CacheItemPriority.NeverRemove;
	            default:
	                throw new ArgumentOutOfRangeException(nameof(priority), priority, null);
	        }
	    }

	    /// <summary>
		///     Removes (almost?) all cached objects.
		/// </summary>
		public void Clear ()
		{
			cache.Compact(1);
		}


		/// <summary>
		///     Remove cached object by it's key.
		/// </summary>
		/// <param name="key">Object key. Can not be null.</param>
		public void Remove (string key)
		{
			cache.Remove (key);
		}
	}
}