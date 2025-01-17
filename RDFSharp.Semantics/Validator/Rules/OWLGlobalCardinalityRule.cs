﻿/*
   Copyright 2012-2023 Marco De Salvo
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

using RDFSharp.Model;
using RDFSharp.Query;
using System.Collections.Generic;
using System.Linq;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// OWL-DL validator rule checking for consistency of global cardinality constraints
    /// </summary>
    internal static class OWLGlobalCardinalityRule
    {
        internal static OWLValidatorReport ExecuteRule(OWLOntology ontology)
        {
            OWLValidatorReport validatorRuleReport = new OWLValidatorReport();

            //owl:FunctionalProperty
            IEnumerator<RDFResource> fpEnumerator = ontology.Model.PropertyModel.FunctionalPropertiesEnumerator;
            while (fpEnumerator.MoveNext())
            {
                //owl:FunctionalProperty can only occur once per subject individual within assertions
                bool fpViolatesRule = ontology.Data.ABoxGraph[null, fpEnumerator.Current, null, null]
                                                   .GroupBy(t => t.Subject)
                                                   .Any(grp => grp.Count() > 1);
                if (fpViolatesRule)
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Error,
                        nameof(OWLGlobalCardinalityRule),
                        $"Violation of OWL-DL integrity caused by functional property '{fpEnumerator.Current}' occurring more than once per subject individual",
                        "Revise your data: it is not allowed multiple usage of a functional property by the same subject individual"));

                //owl:FunctionalProperty cannot be directly or indirectly owl:TransitiveProperty
                bool fpIsTransitiveProperty = ontology.Model.PropertyModel.CheckHasTransitiveProperty(fpEnumerator.Current);
                bool fpHasTransitiveSuperProperties = ontology.Model.PropertyModel.GetSuperPropertiesOf(fpEnumerator.Current)
                                                        .Any(sp => ontology.Model.PropertyModel.CheckHasTransitiveProperty(sp));
                if (fpIsTransitiveProperty || fpHasTransitiveSuperProperties)
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Error,
                        nameof(OWLGlobalCardinalityRule),
                        $"Violation of OWL-DL integrity caused by functional property '{fpEnumerator.Current}' being 'owl:TransitiveProperty', or having a super property being 'owl:TransitiveProperty'",
                        "Revise your property model: it is not allowed for a functional property to be also 'owl:TransitiveProperty', or to have super properties being 'owl:TransitiveProperty'"));
            }

            //owl:InverseFunctionalProperty
            IEnumerator<RDFResource> ifpEnumerator = ontology.Model.PropertyModel.InverseFunctionalPropertiesEnumerator;
            while (ifpEnumerator.MoveNext())
            {
                //owl:InverseFunctionalProperty can only occur once per object individual within assertions
                bool ifpViolatesRule = ontology.Data.ABoxGraph[null, ifpEnumerator.Current, null, null]
                                                    .GroupBy(t => t.Object)
                                                    .Any(grp => grp.Count() > 1);
                if (ifpViolatesRule)
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Error,
                        nameof(OWLGlobalCardinalityRule),
                        $"Violation of OWL-DL integrity caused by inverse functional property '{ifpEnumerator.Current}' occurring more than once per object individual",
                        "Revise your data: it is not allowed multiple usage of an inverse functional property by the same object individual"));

                //owl:InverseFunctionalProperty cannot be directly or indirectly owl:TransitiveProperty
                bool ifpIsTransitiveProperty = ontology.Model.PropertyModel.CheckHasTransitiveProperty(ifpEnumerator.Current);
                bool ifpHasTransitiveSuperProperties = ontology.Model.PropertyModel.GetSuperPropertiesOf(ifpEnumerator.Current)
                                                         .Any(sp => ontology.Model.PropertyModel.CheckHasTransitiveProperty(sp));
                if (ifpIsTransitiveProperty || ifpHasTransitiveSuperProperties)
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Error,
                        nameof(OWLGlobalCardinalityRule),
                        $"Violation of OWL-DL integrity caused by inverse functional property '{ifpEnumerator.Current}' being 'owl:TransitiveProperty', or having a super property being 'owl:TransitiveProperty'",
                        "Revise your property model: it is not allowed for an inverse functional property to be also 'owl:TransitiveProperty', or to have super properties being 'owl:TransitiveProperty'"));
            }

            return validatorRuleReport;
        }
    }
}