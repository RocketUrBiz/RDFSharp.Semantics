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

namespace RDFSharp.Semantics.Extensions.TIME
{
    /// <summary>
    /// Represents the definition of a reference system for expressing temporal extents, coordinates and ordinals.
    /// </summary>
    public class TIMEReferenceSystem : RDFResource
    {
        #region Ctors
        /// <summary>
        /// Builds a generic TRS with the given URI
        /// </summary>
        internal TIMEReferenceSystem(RDFResource trsUri)
            : base(trsUri?.ToString()) { }
        #endregion
    }
}