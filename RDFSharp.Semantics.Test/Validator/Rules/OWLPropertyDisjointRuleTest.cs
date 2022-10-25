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

namespace RDFSharp.Semantics.Validator.Test
{
    [TestClass]
    public class OWLPropertyDisjointRuleTest
    {
        #region Tests
        [TestMethod]
        public void ShouldValidateObjectPropertyDisjoint()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:loves"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:hates"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:despises"));
            ontology.Model.PropertyModel.DeclareDisjointProperties(new RDFResource("ex:loves"), new RDFResource("ex:hates"));
            ontology.Model.PropertyModel.DeclareEquivalentProperties(new RDFResource("ex:hates"), new RDFResource("ex:despises"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:marco"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:mark"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:valentina"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:valentine"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:rebecca"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:marta"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:marco"), new RDFResource("ex:loves"), new RDFResource("ex:valentina"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:marco"), new RDFResource("ex:loves"), new RDFResource("ex:rebecca"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:marco"), new RDFResource("ex:hates"), new RDFResource("ex:marta"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:marco"), new RDFResource("ex:hates"), new RDFResource("ex:valentina"));   //exact violation
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:mark"), new RDFResource("ex:hates"), new RDFResource("ex:valentina"));    //inferred violation on synonim subject
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:marco"), new RDFResource("ex:hates"), new RDFResource("ex:valentine"));   //inferred violation on synonim object
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:mark"), new RDFResource("ex:hates"), new RDFResource("ex:valentine"));    //inferred violation on synonim subject and synonim object
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:mark"), new RDFResource("ex:despises"), new RDFResource("ex:valentine")); //inferred violation on synonim subject and synonim object and indirectly disjoint property

            OWLValidatorReport validatorReport = OWLPropertyDisjointRule.ExecuteRule(ontology);

            Assert.IsNotNull(validatorReport);
            Assert.IsTrue(validatorReport.EvidencesCount == 2); //we are reporting that 'ex:hates' and 'ex:despises' are clashing with 'ex:loves'
            Assert.IsTrue(validatorReport.SelectErrors().Count == 2);
            Assert.IsTrue(validatorReport.SelectWarnings().Count == 0);
        }

        [TestMethod]
        public void ShouldValidateObjectPropertyDisjointViaValidator()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:loves"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:hates"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:despises"));
            ontology.Model.PropertyModel.DeclareDisjointProperties(new RDFResource("ex:loves"), new RDFResource("ex:hates"));
            ontology.Model.PropertyModel.DeclareEquivalentProperties(new RDFResource("ex:hates"), new RDFResource("ex:despises"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:marco"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:mark"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:valentina"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:valentine"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:marco"), new RDFResource("ex:loves"), new RDFResource("ex:valentina"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:marco"), new RDFResource("ex:hates"), new RDFResource("ex:valentina"));   //exact violation
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:mark"), new RDFResource("ex:hates"), new RDFResource("ex:valentina"));    //inferred violation on synonim subject
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:marco"), new RDFResource("ex:hates"), new RDFResource("ex:valentine"));   //inferred violation on synonim object
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:mark"), new RDFResource("ex:hates"), new RDFResource("ex:valentine"));    //inferred violation on synonim subject and synonim object
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:mark"), new RDFResource("ex:despises"), new RDFResource("ex:valentine")); //inferred violation on synonim subject and synonim object and indirectly disjoint property

            OWLValidator validator = new OWLValidator().AddStandardRule(OWLSemanticsEnums.OWLValidatorStandardRules.PropertyDisjoint);
            OWLValidatorReport validatorReport = validator.ApplyToOntology(ontology);

            Assert.IsNotNull(validatorReport);
            Assert.IsTrue(validatorReport.EvidencesCount == 2); //we are reporting that 'ex:hates' and 'ex:despises' are clashing with 'ex:loves'
            Assert.IsTrue(validatorReport.SelectErrors().Count == 2);
            Assert.IsTrue(validatorReport.SelectWarnings().Count == 0);
        }

        [TestMethod]
        public void ShouldValidateDatatypePropertyDisjoint()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.PropertyModel.DeclareDatatypeProperty(new RDFResource("ex:loves"));
            ontology.Model.PropertyModel.DeclareDatatypeProperty(new RDFResource("ex:hates"));
            ontology.Model.PropertyModel.DeclareDatatypeProperty(new RDFResource("ex:despises"));
            ontology.Model.PropertyModel.DeclareDisjointProperties(new RDFResource("ex:loves"), new RDFResource("ex:hates"));
            ontology.Model.PropertyModel.DeclareEquivalentProperties(new RDFResource("ex:hates"), new RDFResource("ex:despises"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:marco"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:mark"));
            ontology.Data.DeclareDatatypeAssertion(new RDFResource("ex:marco"), new RDFResource("ex:loves"), new RDFPlainLiteral("valentina"));
            ontology.Data.DeclareDatatypeAssertion(new RDFResource("ex:marco"), new RDFResource("ex:loves"), new RDFPlainLiteral("rebecca"));
            ontology.Data.DeclareDatatypeAssertion(new RDFResource("ex:marco"), new RDFResource("ex:hates"), new RDFPlainLiteral("marta"));
            ontology.Data.DeclareDatatypeAssertion(new RDFResource("ex:marco"), new RDFResource("ex:hates"), new RDFPlainLiteral("valentina"));   //exact violation
            ontology.Data.DeclareDatatypeAssertion(new RDFResource("ex:mark"), new RDFResource("ex:hates"), new RDFPlainLiteral("valentina"));    //inferred violation on synonim subject
            ontology.Data.DeclareDatatypeAssertion(new RDFResource("ex:mark"), new RDFResource("ex:despises"), new RDFPlainLiteral("valentine")); //inferred violation on synonim subject and indirectly disjoint property

            OWLValidatorReport validatorReport = OWLPropertyDisjointRule.ExecuteRule(ontology);

            Assert.IsNotNull(validatorReport);
            Assert.IsTrue(validatorReport.EvidencesCount == 2); //we are reporting that 'ex:hates' and 'ex:despises' are clashing with 'ex:loves'
            Assert.IsTrue(validatorReport.SelectErrors().Count == 2);
            Assert.IsTrue(validatorReport.SelectWarnings().Count == 0);
        }

        [TestMethod]
        public void ShouldValidateDatatypePropertyDisjointViaValidator()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.PropertyModel.DeclareDatatypeProperty(new RDFResource("ex:loves"));
            ontology.Model.PropertyModel.DeclareDatatypeProperty(new RDFResource("ex:hates"));
            ontology.Model.PropertyModel.DeclareDatatypeProperty(new RDFResource("ex:despises"));
            ontology.Model.PropertyModel.DeclareDisjointProperties(new RDFResource("ex:loves"), new RDFResource("ex:hates"));
            ontology.Model.PropertyModel.DeclareEquivalentProperties(new RDFResource("ex:hates"), new RDFResource("ex:despises"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:marco"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:mark"));
            ontology.Data.DeclareDatatypeAssertion(new RDFResource("ex:marco"), new RDFResource("ex:loves"), new RDFPlainLiteral("valentina"));
            ontology.Data.DeclareDatatypeAssertion(new RDFResource("ex:marco"), new RDFResource("ex:hates"), new RDFPlainLiteral("valentina"));   //exact violation
            ontology.Data.DeclareDatatypeAssertion(new RDFResource("ex:mark"), new RDFResource("ex:hates"), new RDFPlainLiteral("valentina"));    //inferred violation on synonim subject
            ontology.Data.DeclareDatatypeAssertion(new RDFResource("ex:mark"), new RDFResource("ex:despises"), new RDFPlainLiteral("valentine")); //inferred violation on synonim subject and indirectly disjoint property
            OWLValidator validator = new OWLValidator().AddStandardRule(OWLSemanticsEnums.OWLValidatorStandardRules.PropertyDisjoint);
            OWLValidatorReport validatorReport = validator.ApplyToOntology(ontology);

            Assert.IsNotNull(validatorReport);
            Assert.IsTrue(validatorReport.EvidencesCount == 2); //we are reporting that 'ex:hates' and 'ex:despises' are clashing with 'ex:loves'
            Assert.IsTrue(validatorReport.SelectErrors().Count == 2);
            Assert.IsTrue(validatorReport.SelectWarnings().Count == 0);
        }
        #endregion
    }
}