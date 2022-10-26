﻿/*
   Copyright 2012-2022 Marco De Salvo
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at
     http://www.apache.org/licenses/LICENSE-2.0
   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System.Collections.Generic;
using System.Threading.Tasks;

namespace RDFSharp.Semantics.Extensions.SKOS
{
    /// <summary>
    /// SKOSValidator analyzes a skos:ConceptScheme in order to discover errors and inconsistencies affecting its taxonomies
    /// </summary>
    public class SKOSValidator
    {
        #region Properties
        /// <summary>
        /// List of standard rules applied by the SKOS validator
        /// </summary>
        internal List<SKOSEnums.SKOSValidatorStandardRules> StandardRules { get; set; }
        #endregion

        #region Ctors
        /// <summary>
        /// Default-ctor to build an empty SKOS validator
        /// </summary>
        public SKOSValidator()
            => StandardRules = new List<SKOSEnums.SKOSValidatorStandardRules>();
        #endregion

        #region Methods
        /// <summary>
        /// Adds the given standard rule to the SKOS validator
        /// </summary>
        public SKOSValidator AddStandardRule(SKOSEnums.SKOSValidatorStandardRules standardRule)
        {
            if (!StandardRules.Contains(standardRule))
                StandardRules.Add(standardRule);
            return this;
        }

        /// <summary>
        /// Applies the SKOS validator on the given skos:ConceptScheme
        /// </summary>
        public OWLValidatorReport ApplyToConceptScheme(SKOSConceptScheme conceptScheme)
        {
            OWLValidatorReport validatorReport = new OWLValidatorReport();

            if (conceptScheme != null)
            {
                OWLSemanticsEvents.RaiseSemanticsInfo($"SKOS Validator is going to be applied on skos:ConceptScheme '{conceptScheme.URI}'");

                //Standard Rules
                Parallel.ForEach(StandardRules,
                    standardRule =>
                    {
                        OWLSemanticsEvents.RaiseSemanticsInfo($"Launching standard SKOS validator rule '{standardRule}'");

                        OWLValidatorReport standardRuleReport = new OWLValidatorReport();
                        switch (standardRule)
                        {
                            case SKOSEnums.SKOSValidatorStandardRules.TopConcept:
                                standardRuleReport.MergeEvidences(SKOSTopConceptRule.ExecuteRule(conceptScheme));
                                break;
                            case SKOSEnums.SKOSValidatorStandardRules.LiteralForm:
                                standardRuleReport.MergeEvidences(SKOSXLLiteralFormRule.ExecuteRule(conceptScheme));
                                break;
                        }
                        validatorReport.MergeEvidences(standardRuleReport);

                        OWLSemanticsEvents.RaiseSemanticsInfo($"Completed standard SKOS validator rule '{standardRule}': found {standardRuleReport.EvidencesCount} evidences");
                    });

                OWLSemanticsEvents.RaiseSemanticsInfo($"SKOS Validator has been applied on skos:ConceptScheme '{conceptScheme.URI}': found {validatorReport.EvidencesCount} evidences");
            }

            return validatorReport;
        }

        /// <summary>
        /// Asynchronously applies the SKOS validator on the given skos:ConceptScheme
        /// </summary>
        public Task<OWLValidatorReport> ApplyToConceptSchemeAsync(SKOSConceptScheme conceptScheme)
            => Task.Run(() => ApplyToConceptScheme(conceptScheme));
        #endregion
    }
}