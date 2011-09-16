using System;
using NUnit.Framework;

namespace Options.Fixtures
{
	[TestFixture]
	class EitherExtensionsFixture
	{
		[Test]
		public void ActShouldCallIfFirstWhenEitherHasAFirstValue()
		{
			var called = false;
			var target = new Either<int, string>(1);
			target.Act(_ => called = true, _ => { });
			Assert.That(called);
		}

		[Test]
		public void ActOneArgumentShouldCallIfFirstWhenEitherHasAFirstValue()
		{
			var called = false;
			var target = new Either<int, string>(1);
			target.Act(ifFirst: _ => { called = true; });
			Assert.That(called);
		}

		[Test]
		public void ActShouldCallIfSecondWhenEitherHasASecondValue()
		{
			var called = false;
			var target = new Either<int, string>("1");
			target.Act(_ => { }, _ => called = true);
			Assert.That(called);
		}

		[Test]
		public void ActOneArgumentShouldCallIfSecondWhenEitherHasASecondValue()
		{
			var called = false;
			var target = new Either<int, string>("1");
			target.Act(ifSecond: _ => { called = true; });
			Assert.That(called);
		}

		[Test]
		public void ActShouldNotCallIfSecondWhenEitherHasAFirstValue()
		{
			var called = false;
			var target = new Either<int, string>(1);
			target.Act(_ => { }, _ => called = true);
			Assert.That(!called);
		}

		[Test]
		public void ActOneArgumentShouldNotCallIfSecondWhenEitherHasAFirstValue()
		{
			var called = false;
			var target = new Either<int, string>(1);
			target.Act(ifSecond: _ => { called = true; });
			Assert.That(!called);
		}

		[Test]
		public void ActShouldNotCallIfFirstWhenEitherHasASecondValue()
		{
			var called = false;
			var target = new Either<int, string>("1");
			target.Act(_ => called = true, _ => { });
			Assert.That(!called);
		}

		[Test]
		public void ActOneArgumentShouldNotCallIfFirstWhenEitherHasASecondValue()
		{
			var called = false;
			var target = new Either<int, string>("1");
			target.Act(ifFirst: _ => { called = true; });
			Assert.That(!called);
		}

		[Test]
		public void ActShouldPassThroughFirstValueToIfFirst()
		{
			const uint value = 0x6a09e667;
			var target = new Either<uint, string>(value);
			target.Act(f => Assert.That(f, Is.EqualTo(value)), _ => { });
		}

		[Test]
		public void ActOneArgumentShouldPassThroughFirstValueToIfFirst()
		{
			const uint value = 0x6a09e667;
			var target = new Either<uint, string>(value);
			target.Act(ifFirst: f => Assert.That(f, Is.EqualTo(value)));
		}

		[Test]
		public void ActShouldPassThroughSecondValueToIfSecond()
		{
			const string value = "6a09e667";
			var target = new Either<uint, string>(value);
			target.Act(_ => { }, s => Assert.That(s, Is.EqualTo(value)));
		}

		[Test]
		public void ActOneArgumentShouldPassThroughSecondValueToIfSecond()
		{
			const string value = "6a09e667";
			var target = new Either<uint, string>(value);
			target.Act(ifSecond: s => Assert.That(s, Is.EqualTo(value)));
		}

		[Test]
		public void IsFirstReturnsTrueWhenFirstValueIsPresent()
		{
			const uint value = 0x6a09e667;
			var target = new Either<uint, string>(value);
			Assert.That(target.IsFirst());
		}

		[Test]
		public void IsFirstReturnsFalseWhenFirstValueIsNotPresent()
		{
			const string value = "6a09e667";
			var target = new Either<uint, string>(value);
			Assert.That(!target.IsFirst());
		}

		[Test]
		public void IsSecondReturnsTrueWhenSecondValueIsPresent()
		{
			const string value = "6a09e667";
			var target = new Either<uint, string>(value);
			Assert.That(target.IsSecond());
		}

		[Test]
		public void IsSecondReturnsFalseWhenSecondValueIsNotPresent()
		{
			const uint value = 0x6a09e667;
			var target = new Either<uint, string>(value);
			Assert.That(!target.IsSecond());
		}

		[Test]
		public void SwapShouldMoveSecondValueToFirstValue()
		{
			const string value = "6a09e667";
			var target = new Either<uint, string>(value).Swap();

			Assert.That(target, Is.EqualTo(new Either<string, uint>(value)));
		}

		[Test]
		public void SwapShouldMoveFirstValueToSecondValue()
		{
			const uint value = 0x6a09e667;
			var target = new Either<uint, string>(value).Swap();

			Assert.That(target, Is.EqualTo(new Either<string, uint>(value)));
		}

		[Test]
		public void GetFirstOrThrowShouldReturnFirstValueWhenPresent()
		{
			const uint value = 0x6a09e667;
			var target = new Either<uint, string>(value).GetFirstOrThrow();

			Assert.That(target,Is.EqualTo(value));
		}

		[Test]
		public void GetFirstOrThrowShouldThrowWhenNoFirstValue()
		{
			const string value = "6a09e667";
			Assert.That(() => { new Either<uint, string>(value).GetFirstOrThrow(); }, Throws.InvalidOperationException);
		}

		[Test]
		public void GetSecondOrThrowShouldReturnSecondValueWhenPresent()
		{
			const string value = "6a09e667";
			var target = new Either<uint, string>(value).GetSecondOrThrow();

			Assert.That(target,Is.EqualTo(value));
		}

		[Test]
		public void GetSecondOrThrowShouldThrowWhenNoSecondValue()
		{
			const uint value = 0x6a09e667;
			Assert.That(() => { new Either<uint, string>(value).GetSecondOrThrow(); }, Throws.InvalidOperationException);
		}

		[Test]
		public void GetFirstOrDefaultShouldReturnFirstValueWhenPresent()
		{
			const uint value = 0x6a09e667;
			var target = new Either<uint, string>(value).GetFirstOrDefault((uint)0);

			Assert.That(target, Is.EqualTo(value));
		}

		[Test]
		public void GetFirstOrDefaultShouldReturnParameterWhenNoFirstValue()
		{
			const uint value = 0x6a09e667;
			var target = new Either<uint, string>("0").GetFirstOrDefault(value);

			Assert.That(target, Is.EqualTo(value));
		}

		[Test]
		public void GetSecondOrDefaultShouldReturnSecondValueWhenPresent()
		{
			const string value = "6a09e667";
			var target = new Either<uint, string>(value).GetSecondOrDefault("0");

			Assert.That(target, Is.EqualTo(value));
		}

		[Test]
		public void GetSecondOrDefaultShouldReturnParameterWhenNoSecondValue()
		{
			const string value = "6a09e667";
			var target = new Either<uint, string>(0).GetSecondOrDefault(value);

			Assert.That(target, Is.EqualTo(value));
		}

		[Test]
		public void GetFirstOrDefaultWithFunctionShouldReturnFirstValueWhenPresent()
		{
			const uint value = 0x6a09e667;
			var target = new Either<uint, string>(value).GetFirstOrDefault(() => (uint)0);

			Assert.That(target, Is.EqualTo(value));
		}

		[Test]
		public void GetFirstOrDefaultWithFunctionShouldNotInvokeParameterWhenFirstValuePresent()
		{
			var called = false;
			new Either<uint, string>(0x6a09e667).GetFirstOrDefault(() =>
			                                                       	{
			                                                       		called = true;
			                                                       		return (uint) 0;
			                                                       	});

			Assert.That(!called);
		}

		[Test]
		public void GetFirstOrDefaultWithFunctionShouldReturnParameterResultWhenNoFirstValue()
		{
			const uint value = 0x6a09e667;
			var target = new Either<uint, string>("0").GetFirstOrDefault(() => value);

			Assert.That(target, Is.EqualTo(value));
		}

		[Test]
		public void GetSecondOrDefaultWithFunctionShouldReturnSecondValueWhenPresent()
		{
			const string value = "6a09e667";
			var target = new Either<uint, string>(value).GetSecondOrDefault(() => "0");

			Assert.That(target, Is.EqualTo(value));
		}

		[Test]
		public void GetSecondOrDefaultWithFunctionShouldNotInvokeParameterWhenSecondValuePresent()
		{
			var called = false;
			new Either<uint, string>("6a09e667").GetSecondOrDefault(() =>
			                                                        	{
			                                                        		called = true;
			                                                        		return "0";
			                                                        	});

			Assert.That(!called);
		}

		[Test]
		public void GetSecondOrDefaultWithFunctionShouldReturnParameterResultWhenNoSecondValue()
		{
			const string value = "6a09e667";
			var target = new Either<uint, string>(0).GetSecondOrDefault(() => value);

			Assert.That(target, Is.EqualTo(value));
		}

		[Test]
		public void HandleOneArgumentIfFirstShouldBeCalledIfFirstValuePresent()
		{
			var called = false;
			const uint value = 0x6a09e667;
			new Either<uint, string>(value).Handle(_ =>
			                                       	{
			                                       		called = true;
			                                       		return "";
			                                       	});
			Assert.That(called);
		}

		[Test]
		public void HandleOneArgumentIfFirstShouldNotBeCalledIfFirstValueNotPresent()
		{
			var called = false;
			const string value = "6a09e667";
			new Either<uint, string>(value).Handle(_ =>
			                                       	{
			                                       		called = true;
			                                       		return "";
			                                       	});
			Assert.That(!called);
		}

		[Test]
		public void HandleOneArgumentIfFirstShouldPassFirstValueThroughWhenPresent()
		{
			const uint value = 0x6a09e667;
			new Either<uint, string>(value).Handle(v =>
			{
				Assert.That(v, Is.EqualTo(value));
				return "";
			});
		}

		[Test]
		public void HandleOneArgumentIfFirstShouldReturnInvocationResult()
		{
			const uint value = 0x6a09e667;
			var actual = new Either<uint, string>("").Handle(_ => value);
			Assert.That(actual, Is.EqualTo(value));
		}

		[Test]
		public void HandleOneArgumentIfSecondShouldBeCalledIfSecondValuePresent()
		{
			var called = false;
			const string value = "6a09e667";
			new Either<uint, string>(value).Handle(_ =>
			{
				called = true;
				return (uint)0;
			});
			Assert.That(called);
		}

		[Test]
		public void HandleOneArgumentIfSecondShouldNotBeCalledIfSecondValueNotPresent()
		{
			var called = false;
			const uint value = 0x6a09e667;
			new Either<uint, string>(value).Handle(_ =>
			{
				called = true;
				return (uint)0;
			});
			Assert.That(!called);
		}

		[Test]
		public void HandleOneArgumentIfSecondShouldPassSecondValueThroughWhenPresent()
		{
			const string value = "6a09e667";
			new Either<uint, string>(value).Handle(v =>
			{
				Assert.That(v, Is.EqualTo(value));
				return (uint)0;
			});
		}

		[Test]
		public void HandleOneArgumentIfSecondShouldReturnInvocationResult()
		{
			const uint value = 0x6a09e667;
			var actual = new Either<uint, string>("").Handle(_ => value);
			Assert.That(actual, Is.EqualTo(value));
		}

		[Test]
		public void ToTupleShouldPassFirstValueThroughToResult()
		{
			const uint value = 0x6a09e667;
			var actual = new Either<uint, string>(value).ToTuple(() => (uint)0, () => "");
			Assert.That(actual.Item1, Is.EqualTo(value));
		}

		[Test]
		public void ToTupleShouldPassIfFirstResultThroughWhenFirstValuePresent()
		{
			const string value = "6a09e667";
			var actual = new Either<uint, string>(0).ToTuple(() => (uint)0, () => value);
			Assert.That(actual.Item2, Is.EqualTo(value));
		}

		[Test]
		public void ToTupleShouldPassSecondValueThroughToResult()
		{
			const string value = "6a09e667";
			var actual = new Either<uint, string>(value).ToTuple(() => (uint)0, () => "");
			Assert.That(actual.Item2, Is.EqualTo(value));
		}

		[Test]
		public void ToTupleShouldPassIfSecondResultThroughWhenSecondValuePresent()
		{
			const uint value = 0x6a09e667;
			var actual = new Either<uint, string>("").ToTuple(() => value, () => "");
			Assert.That(actual.Item1, Is.EqualTo(value));
		}

		[Test]
		public void OptionFirstShouldReturnFirstValueWhenPresent()
		{
			const uint value = 0x6a09e667;
			new Either<uint, string>(value).OptionFirst()
				.Act(v => Assert.That(v, Is.EqualTo(value)), Assert.Fail);
		}

		[Test]
		public void OptionFirstShouldReturnNoValueWhenFirstValueNotPresent()
		{
			Assert.That(!new Either<uint, string>("").OptionFirst().IsSome());
		}

		[Test]
		public void OptionSecondShouldReturnSecondValueWhenPresent()
		{
			const string value = "6a09e667";
			new Either<uint, string>(value).OptionSecond()
				.Act(v => Assert.That(v, Is.EqualTo(value)), Assert.Fail);
		}

		[Test]
		public void OptionSecondShouldReturnNoValueWhenSecondValueNotPresent()
		{
			Assert.That(!new Either<uint, string>(0).OptionSecond().IsSome());
		}

		[Test]
		public void SelectShouldPutSelectorResultInFirstWhenFirstValuePresent()
		{
			const uint value = 0x6a09e667;
			var target = new Either<DateTime, string>(DateTime.MinValue).Select(_ => value);

			Assert.That(target, Is.EqualTo(new Either<uint, string>(value)));
		}

		[Test]
		public void SelectShouldPassFirstValueToSelector()
		{
			const uint value = 0x6a09e667;
			uint? actual = null;
			new Either<uint, string>(value).Select(v => actual = v);

			Assert.That(actual, Is.EqualTo(value));
		}

		[Test]
		public void SelectShouldNotCallSelectorWhenFirstValueNotPresent()
		{
			var called = false;
			new Either<uint, string>("").Select(_ => called = true);

			Assert.That(!called);
		}

		[Test]
		public void SelectShouldLeaveResultUnmodifiedIfFirstValueNotPresent()
		{
			const string value = "6a09e667";
			var actual = new Either<uint, string>(value).Select(_ => (uint)0);

			Assert.That(actual, Is.EqualTo(new Either<uint, string>(value)));
		}
		
		[Test]
		public void SelectManyShouldPassFirstValueIntoEitherSelector()
		{
			const uint value = 0x6a09e667;
			uint? actual = null;
			new Either<uint, string>(value).SelectMany(v =>
			                                           	{
			                                           		actual = v;
			                                           		return new Either<DateTime, string>("");
			                                           	},
			                                           (_, __) => TimeSpan.MaxValue);

			Assert.That(actual, Is.EqualTo(value));
		}

		[Test]
		public void SelectManyShouldNotCallEitherSelectorWhenFirstValueNotPresent()
		{
			var called = false;
			new Either<uint, string>("").SelectMany(_ =>
			                                        	{
			                                        		called = true;
			                                        		return new Either<DateTime, string>("");
			                                        	},
			                                        (_, __) => TimeSpan.MaxValue);

			Assert.That(!called);
		}

		[Test]
		public void SelectManyShouldNotCallResultSelectorWhenFirstValueNotPresent()
		{
			var called = false;
			new Either<uint, string>("").SelectMany(_ => new Either<DateTime, string>(DateTime.MinValue),
			                                        (_, __) =>
			                                        	{
																									called = true;
			                                        		return TimeSpan.MaxValue;
			                                        	});

			Assert.That(!called);
		}

		[Test]
		public void SelectManyShouldPassFirstValueIntoResultSelector()
		{
			const uint value = 0x6a09e667;
			uint? actual = null;
			new Either<uint, string>(value)
				.SelectMany(_ => new Either<DateTime, string>(DateTime.MinValue),
										(v, _) =>
										{
											actual = v;
											return TimeSpan.MaxValue;
										});

			Assert.That(actual, Is.EqualTo(value));
		}

		[Test]
		public void SelectManyShouldPassIntermediateFirstValueIntoResultSelector()
		{
			const uint value = 0x6a09e667;
			uint? actual = null;
			new Either<DateTime, string>(DateTime.MinValue)
				.SelectMany(_ => new Either<uint, string>(value),
				            (_, v) =>
				            	{
				            		actual = v;
				            		return TimeSpan.MaxValue;
				            	});

			Assert.That(actual, Is.EqualTo(value));
		}

		[Test]
		public void SelectManyShouldNotInvokeResultSelectorWhenIntermediateFirstValueNotPresent()
		{
			var called = false;
			new Either<DateTime, string>(DateTime.MinValue)
				.SelectMany(_ => new Either<uint, string>(""),
				            (_, v) =>
				            	{
				            		called = true;
				            		return TimeSpan.MaxValue;
				            	});

			Assert.That(!called);
		}

		[Test]
		public void SelectManyShouldReturnSecondValueOfSourceIfPresent()
		{
			const string value = "6a09e667";
			var actual = new Either<uint, string>(value)
				.SelectMany(_ => new Either<DateTime, string>("0"),
				            (_, __) => TimeSpan.MinValue);

			Assert.That(actual, Is.EqualTo(new Either<TimeSpan, string>(value)));
		}

		[Test]
		public void SelectManyShouldReturnSecondValueOfIntermediateIfPresent()
		{
			const string value = "6a09e667";
			var actual = new Either<uint, string>(0)
				.SelectMany(_ => new Either<DateTime, string>(value),
				            (_, __) => TimeSpan.MinValue);

			Assert.That(actual, Is.EqualTo(new Either<TimeSpan, string>(value)));
		}

		[Test]
		public void SelectManyShouldReturnResultIfNoSecondValuesPresent()
		{
			const uint value = 0x6a09e667;
			var actual = new Either<DateTime, string>(DateTime.MinValue)
				.SelectMany(_ => new Either<TimeSpan, string>(TimeSpan.MinValue),
				            (_, __) => value);

			Assert.That(actual, Is.EqualTo(new Either<uint, string>(value)));
		}

		public void QuerySyntaxShouldCompile()
		{
			var youmama =
				from i in new Either<int, string>(0)
				from d in new Either<DateTime, string>(DateTime.MinValue)
				from b in new Either<bool, string>(true)
				select b ? d.Ticks : i;
		}
	}
}