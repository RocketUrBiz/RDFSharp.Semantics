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
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace RDFSharp.Semantics.Test
{
    [TestClass]
    public class RDFSemanticsExceptionTest
    {
        #region Tests
        [TestMethod]
        public void ShouldRaiseEmptySemanticsException()
        {
            try
            {
                throw new RDFSemanticsException();
            }
            catch (RDFSemanticsException mex)
            {
                Assert.IsTrue(mex.Message.Contains("RDFSharp.Semantics.RDFSemanticsException", StringComparison.OrdinalIgnoreCase));
            }
        }

        [TestMethod]
        public void ShouldRaiseMessageSemanticsException()
        {
            try
            {
                throw new RDFSemanticsException("This is an exception coming from OWL modeling!");
            }
            catch (RDFSemanticsException mex)
            {
                Assert.IsTrue(mex.Message.Equals("This is an exception coming from OWL modeling!", StringComparison.OrdinalIgnoreCase));
                Assert.IsNull(mex.InnerException);
            }
        }

        [TestMethod]
        public void ShouldRaiseMessageWithInnerSemanticsException()
        {
            try
            {
                throw new RDFSemanticsException("This is an exception coming from OWL modeling!", new Exception("This is the inner exception!"));
            }
            catch (RDFSemanticsException mex)
            {
                Assert.IsTrue(mex.Message.Equals("This is an exception coming from OWL modeling!", StringComparison.OrdinalIgnoreCase));
                Assert.IsNotNull(mex.InnerException);
                Assert.IsTrue(mex.InnerException.Message.Equals("This is the inner exception!"));
            }
        }

        [TestMethod]
        public void ShouldSerializeSemanticsException()
        {
            byte[] SerializeToBytes(RDFSemanticsException e)
            {
                using (MemoryStream stream = new MemoryStream())
                { 
                    new BinaryFormatter().Serialize(stream, e);
                    return stream.GetBuffer();
                }
            }

            RDFSemanticsException DeserializeFromBytes(byte[] data)
            {
                using (MemoryStream stream = new MemoryStream(data))
                    return (RDFSemanticsException)new BinaryFormatter().Deserialize(stream);
            }

            RDFSemanticsException mex = new RDFSemanticsException("RDFSemanticsException is serializable");
            byte[] bytes = SerializeToBytes(mex);
            Assert.IsTrue(bytes.Length > 0);

            RDFSemanticsException result = DeserializeFromBytes(bytes);
            Assert.IsTrue(result.Message.Equals("RDFSemanticsException is serializable"));
            Assert.IsNull(result.InnerException);
        }
        #endregion
    }
}