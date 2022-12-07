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

namespace RDFSharp.Semantics
{
    /// <summary>
    /// OWLSemanticsOptions represents a collector of global options for customizing specific behaviors of the library
    /// </summary>
    public class OWLSemanticsOptions
    {
        #region Properties
        /// <summary>
        /// Represents the level of runtime intelligence given to the ontology engine [default: Advanced]
        /// </summary>
        public static OWLSemanticsEnums.OWLOntologyIntelligenceLevel IntelligenceLevel { get; set; }
        #endregion

        #region Ctors
        /// <summary>
        /// Static-ctor to initialize global options
        /// </summary>
        static OWLSemanticsOptions()
            => IntelligenceLevel = OWLSemanticsEnums.OWLOntologyIntelligenceLevel.Advanced;
        #endregion
    }
}