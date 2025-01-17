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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDFSharp.Model;

namespace RDFSharp.Semantics.Reasoner.Test
{
    [TestClass]
    public class OWLSubPropertyTransitivityRuleTest
    {
        #region Tests
        [TestMethod]
        public void ShouldExecuteSubObjectPropertyTransitivity()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objpropA"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objpropB"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objpropC"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objpropD"));
            ontology.Model.PropertyModel.DeclareSubProperties(new RDFResource("ex:objpropA"), new RDFResource("ex:objpropB"));
            ontology.Model.PropertyModel.DeclareSubProperties(new RDFResource("ex:objpropB"), new RDFResource("ex:objpropC"));
            ontology.Model.PropertyModel.DeclareEquivalentProperties(new RDFResource("ex:objpropC"), new RDFResource("ex:objpropD"));

            OWLReasonerReport reasonerReport = OWLSubPropertyTransitivityRule.ExecuteRule(ontology);

            Assert.IsNotNull(reasonerReport);
            Assert.IsTrue(reasonerReport.EvidencesCount == 3);
        }

        [TestMethod]
        public void ShouldExecuteSubObjectPropertyTransitivityViaReasoner()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objpropA"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objpropB"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objpropC"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objpropD"));
            ontology.Model.PropertyModel.DeclareSubProperties(new RDFResource("ex:objpropA"), new RDFResource("ex:objpropB"));
            ontology.Model.PropertyModel.DeclareSubProperties(new RDFResource("ex:objpropB"), new RDFResource("ex:objpropC"));
            ontology.Model.PropertyModel.DeclareEquivalentProperties(new RDFResource("ex:objpropC"), new RDFResource("ex:objpropD"));

            OWLReasoner reasoner = new OWLReasoner().AddStandardRule(OWLSemanticsEnums.OWLReasonerStandardRules.SubPropertyTransitivity);
            OWLReasonerReport reasonerReport = reasoner.ApplyToOntology(ontology);

            Assert.IsNotNull(reasonerReport);
            Assert.IsTrue(reasonerReport.EvidencesCount == 3);
        }

        [TestMethod]
        public void ShouldExecuteSubDatatypePropertyTransitivity()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.PropertyModel.DeclareDatatypeProperty(new RDFResource("ex:dtpropA"));
            ontology.Model.PropertyModel.DeclareDatatypeProperty(new RDFResource("ex:dtpropB"));
            ontology.Model.PropertyModel.DeclareDatatypeProperty(new RDFResource("ex:dtpropC"));
            ontology.Model.PropertyModel.DeclareDatatypeProperty(new RDFResource("ex:dtpropD"));
            ontology.Model.PropertyModel.DeclareSubProperties(new RDFResource("ex:dtpropA"), new RDFResource("ex:dtpropB"));
            ontology.Model.PropertyModel.DeclareSubProperties(new RDFResource("ex:dtpropB"), new RDFResource("ex:dtpropC"));
            ontology.Model.PropertyModel.DeclareEquivalentProperties(new RDFResource("ex:dtpropC"), new RDFResource("ex:dtpropD"));

            OWLReasonerReport reasonerReport = OWLSubPropertyTransitivityRule.ExecuteRule(ontology);

            Assert.IsNotNull(reasonerReport);
            Assert.IsTrue(reasonerReport.EvidencesCount == 3);
        }

        [TestMethod]
        public void ShouldExecuteSubDatatypePropertyTransitivityViaReasoner()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.PropertyModel.DeclareDatatypeProperty(new RDFResource("ex:dtpropA"));
            ontology.Model.PropertyModel.DeclareDatatypeProperty(new RDFResource("ex:dtpropB"));
            ontology.Model.PropertyModel.DeclareDatatypeProperty(new RDFResource("ex:dtpropC"));
            ontology.Model.PropertyModel.DeclareDatatypeProperty(new RDFResource("ex:dtpropD"));
            ontology.Model.PropertyModel.DeclareSubProperties(new RDFResource("ex:dtpropA"), new RDFResource("ex:dtpropB"));
            ontology.Model.PropertyModel.DeclareSubProperties(new RDFResource("ex:dtpropB"), new RDFResource("ex:dtpropC"));
            ontology.Model.PropertyModel.DeclareEquivalentProperties(new RDFResource("ex:dtpropC"), new RDFResource("ex:dtpropD"));

            OWLReasoner reasoner = new OWLReasoner().AddStandardRule(OWLSemanticsEnums.OWLReasonerStandardRules.SubPropertyTransitivity);
            OWLReasonerReport reasonerReport = reasoner.ApplyToOntology(ontology);

            Assert.IsNotNull(reasonerReport);
            Assert.IsTrue(reasonerReport.EvidencesCount == 3);
        }
        #endregion
    }
}