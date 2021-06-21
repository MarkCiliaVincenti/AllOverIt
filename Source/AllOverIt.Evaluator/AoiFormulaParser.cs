using AllOverIt.Evaluator.Operations;
using AllOverIt.Evaluator.Variables;
using AllOverIt.Helpers;
using System;
using System.Text.RegularExpressions;

namespace AllOverIt.Evaluator
{
    public sealed class AoiFormulaParser : IAoiFormulaParser
    {
        private readonly IAoiFormulaProcessor _formulaProcessor;
        private readonly Func<string, IAoiFormulaReader> _formulaReaderCreator;
        private IAoiFormulaReader _formulaReader;

        internal static IAoiFormulaParser CreateDefault()
        {
            return Create(
              new AoiArithmeticOperationFactory(),
              new AoiUserDefinedMethodFactory());
        }

        // This factory method can be used to build a custom parser based on extended versions of the arithmetic operation and user-defined
        // method factories. A 'null' formula processor will result in a AoiFormulaProcessor being used.
        public static IAoiFormulaParser Create(IAoiArithmeticOperationFactory arithmeticFactory, IAoiUserDefinedMethodFactory userMethodFactory,
          Func<IAoiArithmeticOperationFactory, IAoiUserDefinedMethodFactory, IAoiFormulaProcessor> processorCreator = null)
        {
            _ = arithmeticFactory.WhenNotNull(nameof(arithmeticFactory));
            _ = userMethodFactory.WhenNotNull(nameof(userMethodFactory));

            processorCreator ??= ((arithmetic, userMethod) => new AoiFormulaProcessor(arithmetic, userMethod));

            var processor = processorCreator.Invoke(arithmeticFactory, userMethodFactory);

            return new AoiFormulaParser(processor);
        }

        public AoiFormulaParser(IAoiFormulaProcessor formulaProcessor)
            : this(formulaProcessor, formula => new AoiFormulaReader(formula))
        {
        }

        internal AoiFormulaParser(IAoiFormulaProcessor formulaProcessor, Func<string, IAoiFormulaReader> formulaReaderCreator)
        {
            _formulaProcessor = formulaProcessor.WhenNotNull(nameof(formulaProcessor));
            _formulaReaderCreator = formulaReaderCreator.WhenNotNull(nameof(formulaReaderCreator));
        }

        public AoiFormulaProcessorResult Parse(string formula, IAoiVariableRegistry variableRegistry)
        {
            _ = formula.WhenNotNullOrEmpty(nameof(formula));
            _ = variableRegistry.WhenNotNull(nameof(variableRegistry));

            // remove any extraneous whitespace
            formula = Regex.Replace(formula, @"\s+", "");

            using (_formulaReader = _formulaReaderCreator.Invoke(formula))
            {
                return _formulaProcessor.Process(_formulaReader, variableRegistry);
            }
        }
    }
}