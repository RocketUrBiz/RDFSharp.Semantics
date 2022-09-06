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
    /// RDFSemanticsOptions represents a collector of global options for customizing many aspects of the library behavior
    /// </summary>
    public static class RDFSemanticsOptions
    {
        #region Properties
        /// <summary>
        /// Indicates the policy adopted by the library for real-time OWL-DL integrity and compliance checking (default: Strict)
        /// </summary>
        public static RDFSemanticsEnums.RDFOntologyOWLDLIntegrityPolicy OWLDLIntegrityPolicy { get; set; } 
            = RDFSemanticsEnums.RDFOntologyOWLDLIntegrityPolicy.Strict;
        #endregion

        #region Methods
        /// <summary>
        /// Checks if the global option for real-time OWL-DL integrity and policy compliance checking is active
        /// </summary>
        internal static bool ShouldCheckOWLDLIntegrity 
            => OWLDLIntegrityPolicy == RDFSemanticsEnums.RDFOntologyOWLDLIntegrityPolicy.Strict;
        #endregion
    }
}