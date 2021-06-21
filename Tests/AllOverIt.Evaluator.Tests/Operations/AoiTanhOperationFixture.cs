using AllOverIt.Evaluator.Operations;
using AllOverIt.Evaluator.Operators;
using System;

namespace AllOverIt.Evaluator.Tests.Operations
{
    public class AoiTanhOperationFixture : AoiUnaryOperationFixtureBase<AoiTanhOperation>
    {
        protected override Type OperatorType => typeof(AoiTanhOperator);
    }
}