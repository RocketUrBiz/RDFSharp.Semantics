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

namespace RDFSharp.Semantics.Test
{
    [TestClass]
    public class OWLSemanticsEventsTest
    {
        #region Test
        [TestMethod]
        public void ShouldSubscribeInfoEvents()
        {
            string infoMsg = null;
            OWLSemanticsEvents.OnSemanticsInfo += (string msg) => { infoMsg = msg; };
            OWLSemanticsEvents.RaiseSemanticsInfo("Hello Info");

            Assert.IsTrue(infoMsg.IndexOf("Hello Info") > -1);
        }

        [TestMethod]
        public void ShouldSubscribeWarningEvents()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };
            OWLSemanticsEvents.RaiseSemanticsWarning("Hello Warning");

            Assert.IsTrue(warningMsg.IndexOf("Hello Warning") > -1);
        }
        #endregion
    }
}