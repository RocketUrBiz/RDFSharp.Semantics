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
using System.Collections.Generic;

namespace RDFSharp.Semantics.Validator.Test
{
    [TestClass]
    public class OWLValidatorRulesetTest
    {
        #region Tests
        [TestMethod]
        public void ShouldValidateVocabularyDisjointness()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:entity"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:entity"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:entity"));

            OWLValidatorReport validatorReport = OWLValidatorRuleset.VocabularyDisjointness(ontology);

            Assert.IsNotNull(validatorReport);
            Assert.IsTrue(validatorReport.EvidencesCount == 3);
            Assert.IsTrue(validatorReport.SelectErrors().Count == 3);
            Assert.IsTrue(validatorReport.SelectWarnings().Count == 0);
        }

        [TestMethod]
        public void ShouldValidateVocabularyDeclaration_SubClassOf()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareSubClasses(new RDFResource("ex:class1"), new RDFResource("ex:class2"));

            OWLValidatorReport validatorReport = OWLValidatorRuleset.VocabularyDeclaration(ontology);

            Assert.IsNotNull(validatorReport);
            Assert.IsTrue(validatorReport.EvidencesCount == 2);
            Assert.IsTrue(validatorReport.SelectErrors().Count == 0);
            Assert.IsTrue(validatorReport.SelectWarnings().Count == 2);
        }

        [TestMethod]
        public void ShouldValidateVocabularyDeclaration_EquivalentClass()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareEquivalentClasses(new RDFResource("ex:class1"), new RDFResource("ex:class2"));

            OWLValidatorReport validatorReport = OWLValidatorRuleset.VocabularyDeclaration(ontology);

            Assert.IsNotNull(validatorReport);
            Assert.IsTrue(validatorReport.EvidencesCount == 4); //Consider also the automatic inferences
            Assert.IsTrue(validatorReport.SelectErrors().Count == 0);
            Assert.IsTrue(validatorReport.SelectWarnings().Count == 4);
        }

        [TestMethod]
        public void ShouldValidateVocabularyDeclaration_DisjointWith()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareDisjointClasses(new RDFResource("ex:class1"), new RDFResource("ex:class2"));

            OWLValidatorReport validatorReport = OWLValidatorRuleset.VocabularyDeclaration(ontology);

            Assert.IsNotNull(validatorReport);
            Assert.IsTrue(validatorReport.EvidencesCount == 4); //Consider also the automatic inferences
            Assert.IsTrue(validatorReport.SelectErrors().Count == 0);
            Assert.IsTrue(validatorReport.SelectWarnings().Count == 4);
        }

        [TestMethod]
        public void ShouldValidateVocabularyDeclaration_OneOf_NoClass()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("ex:enumerateClass"), RDFVocabulary.OWL.ONE_OF, new RDFResource("bnode:representative")));
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("bnode:representative"), RDFVocabulary.RDF.TYPE, RDFVocabulary.RDF.LIST));
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("bnode:representative"), RDFVocabulary.RDF.FIRST, new RDFResource("ex:individual1")));
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("bnode:representative"), RDFVocabulary.RDF.REST, RDFVocabulary.RDF.NIL));

            OWLValidatorReport validatorReport = OWLValidatorRuleset.VocabularyDeclaration(ontology);

            Assert.IsNotNull(validatorReport);
            Assert.IsTrue(validatorReport.EvidencesCount == 1);
            Assert.IsTrue(validatorReport.SelectErrors().Count == 0);
            Assert.IsTrue(validatorReport.SelectWarnings().Count == 1);
        }

        [TestMethod]
        public void ShouldValidateVocabularyDeclaration_OneOf_NoIndividual()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareEnumerateClass(new RDFResource("ex:enumerateClass"), new List<RDFResource>() { new RDFResource("ex:individual1") });

            OWLValidatorReport validatorReport = OWLValidatorRuleset.VocabularyDeclaration(ontology);

            Assert.IsNotNull(validatorReport);
            Assert.IsTrue(validatorReport.EvidencesCount == 1);
            Assert.IsTrue(validatorReport.SelectErrors().Count == 0);
            Assert.IsTrue(validatorReport.SelectWarnings().Count == 1);
        }
        #endregion
    }
}