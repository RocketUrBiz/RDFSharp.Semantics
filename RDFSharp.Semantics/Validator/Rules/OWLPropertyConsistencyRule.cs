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

using RDFSharp.Model;
using System.Collections.Generic;
using System.Linq;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// OWL-DL validator rule checking for consistency of object and datatype properties
    /// </summary>
    internal static class OWLPropertyConsistencyRule
    {
        internal static OWLValidatorReport ExecuteRule(OWLOntology ontology)
        {
            OWLValidatorReport validatorRuleReport = new OWLValidatorReport();

            //owl:ObjectProperty
            IEnumerator<RDFResource> objectProperties = ontology.Model.PropertyModel.ObjectPropertiesEnumerator;
            while (objectProperties.MoveNext())
            {
                //Clash with owl:AnnotationProperty
                if (ontology.Model.PropertyModel.CheckHasAnnotationProperty(objectProperties.Current))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Error,
                        nameof(OWLPropertyConsistencyRule),
                        $"Violation of 'rdf:type' definition on object property '{objectProperties.Current}'",
                        $"Revise your property model: it is not allowed to have an object property also declared as annotation property!"));

                //Clash with owl:DatatypeProperty
                if (ontology.Model.PropertyModel.CheckHasDatatypeProperty(objectProperties.Current))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Error,
                        nameof(OWLPropertyConsistencyRule),
                        $"Violation of 'rdf:type' definition on object property '{objectProperties.Current}'",
                        $"Revise your property model: it is not allowed to have an object property also declared as datatype property!"));
            }

            //owl:DatatypeProperty
            IEnumerator<RDFResource> datatypeProperties = ontology.Model.PropertyModel.DatatypePropertiesEnumerator;
            while (datatypeProperties.MoveNext())
            {
                //Clash with owl:AnnotationProperty
                if (ontology.Model.PropertyModel.CheckHasAnnotationProperty(datatypeProperties.Current))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Error,
                        nameof(OWLPropertyConsistencyRule),
                        $"Violation of 'rdf:type' definition on datatype property '{datatypeProperties.Current}'",
                        $"Revise your property model: it is not allowed to have a datatype property also declared as annotation property!"));
            }

            return validatorRuleReport;
        }
    }
}