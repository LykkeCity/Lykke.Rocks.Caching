using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Caching.Tests
{
	[TestClass]
	public class CachingParametersTests
	{
		[TestMethod]
		public void Clone_ReturnsDeepClone()
		{
			// arrange
			var source = new CachingParameters(TimeSpan.FromDays(123), true, CachePriority.High);


			// act
			var result = source.Clone ();


			// assert
			result.ShouldBeEquivalentTo (source);
		}
	}
}