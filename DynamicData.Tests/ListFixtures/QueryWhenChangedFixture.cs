using DynamicData.Tests.Domain;
using NUnit.Framework;
using System;

namespace DynamicData.Tests.ListFixtures
{
	[TestFixture]
	public class QueryWhenChangedFixture
	{
		private ISourceList<Person> _source;
		private ChangeSetAggregator<Person> _results;

		[SetUp]
		public void Initialise()
		{
			_source = new SourceList<Person>();
			_results = new ChangeSetAggregator<Person>(_source.Connect(p => p.Age > 20));
		}

		[TearDown]
		public void Cleanup()
		{
			_source.Dispose();
			_results.Dispose();
		}

		[Test]
		public void ChangeInvokedOnSubscriptionIfItHasData()
		{
			bool invoked = false;
			_source.Add(new Person("A", 1));
			var subscription = _source.Connect()
				.QueryWhenChanged()
				.Subscribe(x => invoked = true);
			Assert.IsTrue(invoked, "Should have received on next");
			subscription.Dispose();
		}


		[Test]
		public void CanHandleAddsAndUpdates()
		{


			bool invoked = false;
			var subscription = _source.Connect()
				.QueryWhenChanged(q=>q.Count)
				.Subscribe(query => invoked = true);

			var person = new Person("A", 1);
            _source.Add(person);
			_source.Remove(person);

			Assert.IsTrue(invoked, "Should have received on next");
			subscription.Dispose();
		}

		[Test]
		public void ChangeInvokedOnNext()
		{
			bool invoked = false;


			var subscription = _source.Connect()
				.QueryWhenChanged()
				.Subscribe(x => invoked = true);

			Assert.IsFalse(invoked, "Should have received on next");

			_source.Add(new Person("A", 1));
			Assert.IsTrue(invoked, "Should have received on next");

			subscription.Dispose();
		}

		[Test]
		public void ChangeInvokedOnSubscriptionIfItHasData_WithSelector()
		{
			bool invoked = false;
			_source.Add(new Person("A", 1));
			var subscription = _source.Connect()
				.QueryWhenChanged(query => query.Count)
				.Subscribe(x => invoked = true);
			Assert.IsTrue(invoked, "Should have received on next");
			subscription.Dispose();
		}

		[Test]
		public void ChangeInvokedOnNext_WithSelector()
		{
			bool invoked = false;


			var subscription = _source.Connect()
				.QueryWhenChanged(query => query.Count)
				.Subscribe(x => invoked = true);

			Assert.IsFalse(invoked, "Should have received on next");

			_source.Add(new Person("A", 1));
			Assert.IsTrue(invoked, "Should have received on next");

			subscription.Dispose();
		}
	}
}