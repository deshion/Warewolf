﻿/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2019 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using Dev2.Communication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;

namespace Dev2.Infrastructure.Tests.Communication
{
    [TestClass]
    public class Dev2JsonSerializerTests
    {
        readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        };
        readonly JsonSerializerSettings _deSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
        };
        const Formatting Formatting = Newtonsoft.Json.Formatting.Indented;

        [TestMethod]
        [Owner("Candice Daniel")]
        [TestCategory(nameof(Dev2JsonSerializer))]
        public void Dev2JsonSerializer_Serialize()
        {
            var theMessage = "testingtesting123";
            var msg = new ExecuteMessage { HasError = false };
            msg.SetMessage(theMessage);
            var buffer = JsonConvert.SerializeObject(msg);

            var json = new Dev2JsonSerializer();
            var resultA = JsonConvert.SerializeObject(buffer, Formatting);
            var resultB = json.Serialize(buffer);

            Assert.AreEqual(resultA.ToString(), resultB.ToString());
        }


        class MyType
        {
            public int Ival { get; set; } = 1234;
            public string Sval { get; set; } = "hello";
        }
        [TestMethod]
        [Owner("Rory McGuire")]
        [TestCategory(nameof(Dev2JsonSerializer))]
        public void Dev2JsonSerializer_Serialize_GivenObject()
        {
            var expected = "{\"$id\":\"1\",\"$type\":\"Dev2.Infrastructure.Tests.Communication.Dev2JsonSerializerTests+MyType, Dev2.Infrastructure.Tests\",\"Ival\":1234,\"Sval\":\"hello\"}";
            var obj = new MyType();
            var json = new Dev2JsonSerializer();
            var stream = new MemoryStream();
            var streamWriter = new StreamWriter(stream, Encoding.UTF8, 4096, true);
            
            // test
            json.Serialize(streamWriter, obj);

            // verify
            stream.Seek(0, SeekOrigin.Begin);
            var streamReader = new StreamReader(stream);

            var value = streamReader.ReadToEnd();

            Assert.AreEqual(expected, value);
        }

        [TestMethod]
        [Owner("Candice Daniel")]
        [TestCategory(nameof(Dev2JsonSerializer))]
        public void Dev2JsonSerializer_Serialize_Formatting()
        {
            var theMessage = "testingtesting123";
            var msg = new ExecuteMessage { HasError = false };
            msg.SetMessage(theMessage);
            var buffer = JsonConvert.SerializeObject(msg);

            var json = new Dev2JsonSerializer();
            var resultA = JsonConvert.SerializeObject(buffer, Formatting.None, _serializerSettings);
            var resultB = json.Serialize(buffer, Formatting.None);

            Assert.AreEqual(resultA.ToString(), resultB.ToString());
        }

        [TestMethod]
        [Owner("Candice Daniel")]
        [TestCategory(nameof(Dev2JsonSerializer))]
        public void Dev2JsonSerializer_Deserialize_String_Type()
        {
            var theMessage = "testingtesting123";
            var js = new Dev2JsonSerializer();
            var msg = new ExecuteMessage { HasError = false };
            msg.SetMessage(theMessage);
            var buffer = JsonConvert.SerializeObject(msg);

            var resultA = JsonConvert.DeserializeObject(buffer.ToString(), typeof(ExecuteMessage), _deSerializerSettings);

            var resultB = js.Deserialize(buffer.ToString(), typeof(ExecuteMessage));
            Assert.AreEqual(resultA.ToString(), resultB.ToString());
        }

        [TestMethod]
        [Owner("Candice Daniel")]
        [TestCategory(nameof(Dev2JsonSerializer))]
        public void Dev2JsonSerializer_Deserialize_String()
        {
            var theMessage = "testingtesting123";
            var js = new Dev2JsonSerializer();
            var msg = new ExecuteMessage { HasError = false };
            msg.SetMessage(theMessage);
            var buffer = JsonConvert.SerializeObject(msg);

            var resultA = JsonConvert.DeserializeObject<ExecuteMessage>(buffer.ToString(), _deSerializerSettings);

            var resultB = js.Deserialize<ExecuteMessage>(buffer.ToString());
            Assert.AreEqual(resultA.Message.ToString(), resultB.Message.ToString());
        }
        [TestMethod]
        [Owner("Candice Daniel")]
        [TestCategory(nameof(Dev2JsonSerializer))]
        public void Dev2JsonSerializer_Deserialize_StringBuilder_SendNull_ReturnNull()
        {
            var js = new Dev2JsonSerializer();
            var sb = new StringBuilder();
            sb = null;
            var result = js.Deserialize<StringBuilder>(sb);
            Assert.IsNull(result);
        }
        [TestMethod]
        [Owner("Candice Daniel")]
        [TestCategory(nameof(Dev2JsonSerializer))]
        public void Dev2JsonSerializer_Deserialize_StringBuilder_SendEmpty_ReturnNull()
        {
            var js = new Dev2JsonSerializer();
            var sb = new StringBuilder("");
            var result = js.Deserialize<StringBuilder>(sb);
            Assert.IsNull(result);
        }
        [TestMethod]
        [Owner("Candice Daniel")]
        [TestCategory(nameof(Dev2JsonSerializer))]
        public void Dev2JsonSerializer_SerializeToBuffer_WhenEsbExecuteRequest_ValidObjectStringBuffer()
        {
            //------------Setup for test--------------------------
            var js = new Dev2JsonSerializer();
            var request = new EsbExecuteRequest { ServiceName = "Foobar" };
            request.AddArgument("key1", new StringBuilder("value1"));
            request.AddArgument("key2", new StringBuilder("value2"));

            //------------Execute Test---------------------------
            var result = js.SerializeToBuilder(request);

            //------------Assert Results-------------------------
            var resultObj = js.Deserialize<EsbExecuteRequest>(result);

            Assert.AreEqual(request.ServiceName, resultObj.ServiceName);
            Assert.AreEqual(request.Args["key1"].ToString(), resultObj.Args["key1"].ToString());
            Assert.AreEqual(request.Args["key2"].ToString(), resultObj.Args["key2"].ToString());
        }

        [TestMethod]
        [Owner("Candice Daniel")]
        [TestCategory(nameof(Dev2JsonSerializer))]
        public void Dev2JsonSerializer_Deserializer_WhenUsingStream_ExpectValidObject()
        {
            //------------Setup for test--------------------------
            const string theMessage = @"Much evil soon high in hope do view. Out may few northward believing attempted. Yet timed being songs marry one defer men our. Although finished blessing do of. Consider speaking me prospect whatever if. Ten nearer rather hunted six parish indeed number. Allowance repulsive sex may contained can set suspected abilities cordially. Do part am he high rest that. So fruit to ready it being views match. 
Knowledge nay estimable questions repulsive daughters boy. Solicitude gay way unaffected expression for. His mistress ladyship required off horrible disposed rejoiced. Unpleasing pianoforte unreserved as oh he unpleasant no inquietude insipidity. Advantages can discretion possession add favourable cultivated admiration far. Why rather assure how esteem end hunted nearer and before. By an truth after heard going early given he. Charmed to it excited females whether at examine. Him abilities suffering may are yet dependent. 
Why end might ask civil again spoil.";

            var msg = new ExecuteMessage { HasError = false };
            msg.SetMessage(theMessage);
            var buffer = new StringBuilder(JsonConvert.SerializeObject(msg));

            //------------Execute Test---------------------------
            var js = new Dev2JsonSerializer();
            var result = js.Deserialize<ExecuteMessage>(buffer);

            //------------Assert Results-------------------------
            Assert.AreEqual(theMessage, result.Message.ToString());
        }

        [TestMethod]
        [Owner("Candice Daniel")]
        [TestCategory(nameof(Dev2JsonSerializer))]
        public void Dev2JsonSerializer_Deserializer_WhenUsingStreamWithLargePayload_ExpectValidObject()
        {
            //------------Setup for test--------------------------
            const string theMessage = @"Much evil soon high in hope do view. Out may few northward believing attempted. Yet timed being songs marry one defer men our. Although finished blessing do of. Consider speaking me prospect whatever if. Ten nearer rather hunted six parish indeed number. Allowance repulsive sex may contained can set suspected abilities cordially. Do part am he high rest that. So fruit to ready it being views match. 

Knowledge nay estimable questions repulsive daughters boy. Solicitude gay way unaffected expression for. His mistress ladyship required off horrible disposed rejoiced. Unpleasing pianoforte unreserved as oh he unpleasant no inquietude insipidity. Advantages can discretion possession add favourable cultivated admiration far. Why rather assure how esteem end hunted nearer and before. By an truth after heard going early given he. Charmed to it excited females whether at examine. Him abilities suffering may are yet dependent. 

Why end might ask civil again spoil. She dinner she our horses depend. Remember at children by reserved to vicinity. In affronting unreserved delightful simplicity ye. Law own advantage furniture continual sweetness bed agreeable perpetual. Oh song well four only head busy it. Afford son she had lively living. Tastes lovers myself too formal season our valley boy. Lived it their their walls might to by young. 

On insensible possession oh particular attachment at excellence in. The books arose but miles happy she. It building contempt or interest children mistress of unlocked no. Offending she contained mrs led listening resembled. Delicate marianne absolute men dashwood landlord and offended. Suppose cottage between and way. Minuter him own clothes but observe country. Agreement far boy otherwise rapturous incommode favourite. 

Inquietude simplicity terminated she compliment remarkably few her nay. The weeks are ham asked jokes. Neglected perceived shy nay concluded. Not mile draw plan snug next all. Houses latter an valley be indeed wished merely in my. Money doubt oh drawn every or an china. Visited out friends for expense message set eat. 

By so delight of showing neither believe he present. Deal sigh up in shew away when. Pursuit express no or prepare replied. Wholly formed old latter future but way she. Day her likewise smallest expenses judgment building man carriage gay. Considered introduced themselves mr to discretion at. Means among saw hopes for. Death mirth in oh learn he equal on. 

Exquisite cordially mr happiness of neglected distrusts. Boisterous impossible unaffected he me everything. Is fine loud deal an rent open give. Find upon and sent spot song son eyes. Do endeavor he differed carriage is learning my graceful. Feel plan know is he like on pure. See burst found sir met think hopes are marry among. Delightful remarkably new assistance saw literature mrs favourable. 

Behind sooner dining so window excuse he summer. Breakfast met certainty and fulfilled propriety led. Waited get either are wooded little her. Contrasted unreserved as mr particular collecting it everything as indulgence. Seems ask meant merry could put. Age old begin had boy noisy table front whole given. 

Boy favourable day can introduced sentiments entreaties. Noisier carried of in warrant because. So mr plate seems cause chief widen first. Two differed husbands met screened his. Bed was form wife out ask draw. Wholly coming at we no enable. Offending sir delivered questions now new met. Acceptance she interested new boisterous day discretion celebrated. 

That know ask case sex ham dear her spot. Weddings followed the all marianne nor whatever settling. Perhaps six prudent several her had offence. Did had way law dinner square tastes. Recommend concealed yet her procuring see consulted depending. Adieus hunted end plenty are his she afraid. Resources agreement contained propriety applauded neglected use yet. 

Far far away, behind the word mountains, far from the countries Vokalia and Consonantia, there live the blind texts. Separated they live in Bookmarksgrove right at the coast of the Semantics, a large language ocean. A small river named Duden flows by their place and supplies it with the necessary regelialia. It is a paradisematic country, in which roasted parts of sentences fly into your mouth. Even the all-powerful 

Pointing has no control about the blind texts it is an almost unorthographic life One day however a small line of blind text by the name of Lorem Ipsum decided to leave for the far World of Grammar. The Big Oxmox advised her not to do so, because there were thousands of bad Commas, wild Question Marks and devious Semikoli, but the Little Blind Text didn't listen. She packed her seven versalia, put her initial into the belt and made herself on the way. When she reached the first hills of the Italic Mountains, she had a last view back on the skyline of her hometown Bookmarksgrove, the headline of ";

            var msg = new ExecuteMessage { HasError = false };
            msg.SetMessage(theMessage);
            var buffer = new StringBuilder(JsonConvert.SerializeObject(msg));

            //------------Execute Test---------------------------

            var js = new Dev2JsonSerializer();
            var result = js.Deserialize<ExecuteMessage>(buffer);

            //------------Assert Results-------------------------
            Assert.AreEqual(theMessage, result.Message.ToString());
        }

        [TestMethod]
        [Owner("Candice Daniel")]
        [TestCategory(nameof(Dev2JsonSerializer))]
        public void Dev2JsonSerializer_Deserialize_StreamReader()
        {
            string json = "{\"ErrorMessage\": \"\",\"ErrorDetails\": {\"ErrorID\": 111,\"Description\":{\"Short\": 0,\"Verbose\": 20},\"ErrorDate\": \"\"}}";
            using (var stream = GenerateStreamFromString(json))
            {
                var sr = new StreamReader(stream);               
                var js = new Dev2JsonSerializer();
                var result = js.Deserialize<object>(sr);
                Assert.AreEqual(json.Replace(" ", ""), result.ToString().Replace("\r\n","").Replace(" ", ""));
            }
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

    }
}
