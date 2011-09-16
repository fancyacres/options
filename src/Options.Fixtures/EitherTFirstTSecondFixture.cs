using NUnit.Framework;

namespace Options.Fixtures
{
	[TestFixture]
	class EitherTFirstTSecondFixture
	{
		[Test]
		public void HandleShouldCallIfFirstWhenEitherHasAFirstValue()
		{
			var called = false;
			var target = new Either<int, string>(1);
			target.Handle(_ => called = true, _ => true);
			Assert.That(called);
		}

		[Test]
		public void HandleShouldCallIfSecondWhenEitherHasASecondValue()
		{
			var called = false;
			var target = new Either<int, string>("1");
			target.Handle(_ => true, _ => called = true);
			Assert.That(called);
		}

		[Test]
		public void HandleShouldNotCallIfSecondWhenEitherHasAFirstValue()
		{
			var called = false;
			var target = new Either<int, string>(1);
			target.Handle(_ => true, _ => called = true);
			Assert.That(!called);
		}

		[Test]
		public void HandleShouldNotCallIfFirstWhenEitherHasASecondValue()
		{
			var called = false;
			var target = new Either<int, string>("1");
			target.Handle(_ => called = true, _ => true);
			Assert.That(!called);
		}

		[Test]
		public void HandleShouldPassThroughFirstValueToIfFirst()
		{
			const uint value = 0x6a09e667;
			var target = new Either<uint, string>(value);
			target.Handle(f =>
			{
				Assert.That(f, Is.EqualTo(value));
				return true;
			},
										_ => true);
		}

		[Test]
		public void HandleShouldPassThroughSecondValueToIfSecond()
		{
			const string value = "6a09e667";
			var target = new Either<uint, string>(value);
			target.Handle(_ => true,
										s =>
										{
											Assert.That(s, Is.EqualTo(value));
											return true;
										});
		}

		[Test]
		public void HandleShouldPassThroughIfFirstResult()
		{
			var target = new Either<uint, string>(0x6a09e667);
			const decimal expected = 5m;
			Assert.That(target.Handle(_ => expected, _ => 0m), Is.EqualTo(expected));
		}

		[Test]
		public void HandleShouldPassThroughIfSecondResult()
		{
			var target = new Either<uint, string>("1");
			const decimal expected = 5m;
			Assert.That(target.Handle(_ => 0m, _ => expected), Is.EqualTo(expected));
		}

		[Test]
		public void IdenticalEitherValuesShouldBeEqual()
		{
			const uint value = 0x6a09e667;
			var target0 = new Either<uint, string>(value);
			var target1 = new Either<uint, string>(value);

			if (ReferenceEquals(target0, target1))
			{
				Assert.Inconclusive();
			}
			Assert.That(target0, Is.EqualTo(target1));
		}

		[Test]
		public void SwappedEitherValuesShouldNotBeEqual()
		{
			const uint value = 0x6a09e667;
			var target0 = new Either<uint, string>(value);
			var target1 = new Either<string, uint>(value);
			Assert.That(target0, Is.Not.EqualTo(target1));
		}

		[Test]
		public void DifferentFirstValuesShouldNotBeEqual()
		{
			const uint value0 = 0x6a09e667;
			const uint value1 = 0xbb67ae85;
			var target0 = new Either<uint, string>(value0);
			var target1 = new Either<uint, string>(value1);
			Assert.That(target0, Is.Not.EqualTo(target1));
		}

		[Test]
		public void DifferentSecondValuesShouldNotBeEqual()
		{
			const string value0 = "6a09e667";
			const string value1 = "bb67ae85";
			var target0 = new Either<uint, string>(value0);
			var target1 = new Either<uint, string>(value1);
			Assert.That(target0, Is.Not.EqualTo(target1));
		}

		[Test]
		public void IdenticalEitherValuesShouldHaveTheSameHashCode()
		{
			const uint value = 0x6a09e667;
			var target0 = new Either<uint, string>(value);
			var target1 = new Either<uint, string>(value);

			if (ReferenceEquals(target0, target1))
			{
				Assert.Inconclusive();
			}
			Assert.That(target0.GetHashCode(), Is.EqualTo(target1.GetHashCode()));
		}
	}
}
