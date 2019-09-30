﻿using FluentAssertions;
using Lifti.Querying;
using Lifti.Querying.QueryParts;
using Xunit;

namespace Lifti.Tests.Querying.QueryParts
{
    public class NearQueryOperatorTests : OperatorTestBase
    {
        [Fact]
        public void ShouldOnlyReturnMatchesForAppropriateField()
        {
            var sut = new NearQueryOperator(
                new FakeQueryPart(
                    new QueryWordMatch(7, FieldMatch(1, 8, 20, 100)),
                    new QueryWordMatch(7, FieldMatch(2, 9, 14)),
                    new QueryWordMatch(8, FieldMatch(1, 11, 101)),
                    new QueryWordMatch(8, FieldMatch(2, 8, 104))),
                new FakeQueryPart(
                    new QueryWordMatch(7, FieldMatch(1, 6, 14, 102)),
                    new QueryWordMatch(8, FieldMatch(1, 5, 106)),
                    new QueryWordMatch(8, FieldMatch(2, 3, 105))));

            var results = sut.Evaluate(() => new FakeIndexNavigator());

            // Item 7 matches:
            // Field 1: (8, 6) (100, 102)
            // Field 2: None
            // Item 8 matches:
            // Field 1: (101, 106)
            // Field 2: (8, 3) (104, 105)
            results.Matches.Should().BeEquivalentTo(
                new QueryWordMatch(
                    7,
                    new FieldMatch(1, CompositeMatch(8, 6), CompositeMatch(100, 102))),
                new QueryWordMatch(
                    8,
                    new FieldMatch(1, CompositeMatch(101, 106)),
                    new FieldMatch(2, CompositeMatch(8, 3), CompositeMatch(104, 105))));
        }
    }
}
