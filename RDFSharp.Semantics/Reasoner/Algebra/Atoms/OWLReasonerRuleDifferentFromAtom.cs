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
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// OWLReasonerRuleDifferentFromAtom represents a SWRL atom inferring owl:differentFrom relations between individuals
    /// </summary>
    public class OWLReasonerRuleDifferentFromAtom : OWLReasonerRuleObjectPropertyAtom
    {
        #region Ctors
        /// <summary>
        /// Default-ctor to build an owl:differentFrom atom with the given arguments
        /// </summary>
        public OWLReasonerRuleDifferentFromAtom(RDFVariable leftArgument, RDFVariable rightArgument)
            : base(RDFVocabulary.OWL.DIFFERENT_FROM, leftArgument, rightArgument)
        {
            if (rightArgument == null)
                throw new OWLSemanticsException("Cannot create atom because given \"rightArgument\" parameter is null");
        }

        /// <summary>
        /// Default-ctor to build an owl:differentFrom atom with the given arguments
        /// </summary>
        public OWLReasonerRuleDifferentFromAtom(RDFVariable leftArgument, RDFResource rightArgument)
            : base(RDFVocabulary.OWL.DIFFERENT_FROM, leftArgument, rightArgument)
        {
            if (rightArgument == null)
                throw new OWLSemanticsException("Cannot create atom because given \"rightArgument\" parameter is null");
        }
        #endregion

        #region Methods
        /// <summary>
        /// Evaluates the atom in the context of an antecedent
        /// </summary>
        internal override DataTable EvaluateOnAntecedent(OWLOntology ontology)
        {
            string leftArgumentString = LeftArgument.ToString();
            string rightArgumentString = RightArgument.ToString();

            //Initialize the structure of the atom result
            DataTable atomResult = new DataTable();
            RDFQueryEngine.AddColumn(atomResult, leftArgumentString);
            if (RightArgument is RDFVariable)
                RDFQueryEngine.AddColumn(atomResult, rightArgumentString);

            //Extract owl:differentFrom relations of the atom's right argument
            if (RightArgument is RDFResource rightArgumentIndividual)
            {
                List<RDFResource> differentIndividuals = ontology.Data.GetDifferentIndividuals(rightArgumentIndividual);

                //Save them into the atom result
                Dictionary<string, string> atomResultBindings = new Dictionary<string, string>();
                foreach (RDFResource differentIndividual in differentIndividuals)
                {                    
                    atomResultBindings.Add(leftArgumentString, differentIndividual.ToString());

                    RDFQueryEngine.AddRow(atomResult, atomResultBindings);

                    atomResultBindings.Clear();
                }
            }
            else
            {
                foreach (RDFResource owlIndividual in ontology.Data)
                {
                    string owlIndividualString = owlIndividual.ToString();
                    List<RDFResource> differentIndividuals = ontology.Data.GetDifferentIndividuals(owlIndividual);

                    //Save them into the atom result
                    Dictionary<string, string> atomResultBindings = new Dictionary<string, string>();
                    foreach (RDFResource differentIndividual in differentIndividuals)
                    {                        
                        atomResultBindings.Add(leftArgumentString, owlIndividualString);
                        atomResultBindings.Add(rightArgumentString, differentIndividual.ToString());

                        RDFQueryEngine.AddRow(atomResult, atomResultBindings);

                        atomResultBindings.Clear();
                    }
                }
            }

            //Return the atom result
            return atomResult;
        }

        /// <summary>
        /// Evaluates the atom in the context of an consequent
        /// </summary>
        internal override OWLReasonerReport EvaluateOnConsequent(DataTable antecedentResults, OWLOntology ontology)
        {
            OWLReasonerReport report = new OWLReasonerReport();
            string leftArgumentString = LeftArgument.ToString();
            string rightArgumentString = RightArgument.ToString();
            string differentFromAtomString = this.ToString();

            #region Guards
            //The antecedent results table MUST have a column corresponding to the atom's left argument
            if (!antecedentResults.Columns.Contains(leftArgumentString))
                return report;

            //The antecedent results table MUST have a column corresponding to the atom's right argument (if variable)
            if (RightArgument is RDFVariable && !antecedentResults.Columns.Contains(rightArgumentString))
                return report;
            #endregion

            //Iterate the antecedent results table to materialize the atom's reasoner evidences
            IEnumerator rowsEnum = antecedentResults.Rows.GetEnumerator();
            while (rowsEnum.MoveNext())
            {
                DataRow currentRow = (DataRow)rowsEnum.Current;

                #region Guards
                //The current row MUST have a BOUND value in the column corresponding to the atom's left argument
                if (currentRow.IsNull(leftArgumentString))
                    continue;

                //The current row MUST have a BOUND value in the column corresponding to the atom's right argument (if variable)
                if (this.RightArgument is RDFVariable && currentRow.IsNull(rightArgumentString))
                    continue;
                #endregion

                //Parse the value of the column corresponding to the atom's left argument
                RDFPatternMember leftArgumentValue = RDFQueryUtilities.ParseRDFPatternMember(currentRow[leftArgumentString].ToString());

                //Parse the value of the column corresponding to the atom's right argument
                RDFPatternMember rightArgumentValue = RightArgument is RDFVariable ? RDFQueryUtilities.ParseRDFPatternMember(currentRow[rightArgumentString].ToString())
                                                                                   : RightArgument; //Resource

                if (leftArgumentValue is RDFResource leftArgumentValueResource
                        && rightArgumentValue is RDFResource rightArgumentValueResource)
                {
                    //Protect atom's inferences with implicit taxonomy checks
                    if (ontology.Data.CheckDifferentFromCompatibility(leftArgumentValueResource, rightArgumentValueResource))
                    {
                        //Create the inferences
                        RDFTriple atomInference = new RDFTriple(leftArgumentValueResource, Predicate, rightArgumentValueResource);
                        RDFTriple symmetricAtomInference = new RDFTriple(rightArgumentValueResource, Predicate, leftArgumentValueResource);

                        //Add the inferences to the report
                        if (!ontology.Data.ABoxGraph.ContainsTriple(atomInference))
                            report.AddEvidence(new OWLReasonerEvidence(OWLSemanticsEnums.OWLReasonerEvidenceCategory.Data, differentFromAtomString, atomInference));
                        if (!ontology.Data.ABoxGraph.ContainsTriple(symmetricAtomInference))
                            report.AddEvidence(new OWLReasonerEvidence(OWLSemanticsEnums.OWLReasonerEvidenceCategory.Data, differentFromAtomString, symmetricAtomInference));
                    }
                }
            }

            return report;
        }
        #endregion
    }
}