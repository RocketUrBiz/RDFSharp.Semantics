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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDFSharp.Model;

namespace RDFSharp.Semantics.Reasoner.Test
{
    [TestClass]
    public class OWLDomainEntailmentRuleTest
    {
        #region Tests
        [TestMethod]
        public void ShouldExecuteDomainEntailmentOnObjectProperties()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:class1"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objpropA"), new OWLOntologyObjectPropertyBehavior() { Domain = new RDFResource("ex:class1") });
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objpropB"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objpropA"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objpropA"), new RDFResource("ex:indiv3"));

            OWLReasonerReport reasonerReport = OWLDomainEntailmentRule.ExecuteRule(ontology, OWLOntologyLoaderOptions.DefaultOptions);

            Assert.IsNotNull(reasonerReport);
            Assert.IsTrue(reasonerReport.EvidencesCount == 2);
        }

        [TestMethod]
        public void ShouldExecuteDomainEntailmentOnObjectPropertiesViaReasoner()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:class1"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objpropA"), new OWLOntologyObjectPropertyBehavior() { Domain = new RDFResource("ex:class1") });
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objpropB"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objpropA"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objpropA"), new RDFResource("ex:indiv3"));

            OWLReasoner reasoner = new OWLReasoner().AddStandardRule(OWLSemanticsEnums.OWLReasonerStandardRules.DomainEntailment);
            OWLReasonerReport reasonerReport = reasoner.ApplyToOntology(ontology);

            Assert.IsNotNull(reasonerReport);
            Assert.IsTrue(reasonerReport.EvidencesCount == 2);
        }

        [TestMethod]
        public void ShouldExecuteDomainEntailmentOnDatatypeProperties()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:class1"));
            ontology.Model.PropertyModel.DeclareDatatypeProperty(new RDFResource("ex:dtpropA"), new OWLOntologyObjectPropertyBehavior() { Domain = new RDFResource("ex:class1") });
            ontology.Model.PropertyModel.DeclareDatatypeProperty(new RDFResource("ex:dtpropB"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:dtpropA"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:dtpropA"), new RDFResource("ex:indiv3"));

            OWLReasonerReport reasonerReport = OWLDomainEntailmentRule.ExecuteRule(ontology, OWLOntologyLoaderOptions.DefaultOptions);

            Assert.IsNotNull(reasonerReport);
            Assert.IsTrue(reasonerReport.EvidencesCount == 2);
        }

        [TestMethod]
        public void ShouldExecuteDomainEntailmentOnDatatypePropertiesViaReasoner()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:class1"));
            ontology.Model.PropertyModel.DeclareDatatypeProperty(new RDFResource("ex:dtpropA"), new OWLOntologyObjectPropertyBehavior() { Domain = new RDFResource("ex:class1") });
            ontology.Model.PropertyModel.DeclareDatatypeProperty(new RDFResource("ex:dtpropB"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:dtpropA"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:dtpropA"), new RDFResource("ex:indiv3"));

            OWLReasoner reasoner = new OWLReasoner().AddStandardRule(OWLSemanticsEnums.OWLReasonerStandardRules.DomainEntailment);
            OWLReasonerReport reasonerReport = reasoner.ApplyToOntology(ontology);

            Assert.IsNotNull(reasonerReport);
            Assert.IsTrue(reasonerReport.EvidencesCount == 2);
        }
        #endregion
    }
}