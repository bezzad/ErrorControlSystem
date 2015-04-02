using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using ErrorControlSystem.DbConnectionManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ErrorControlSystemUnitTest.DbConnectionManager
{
    [TestClass]
    public class UnitTest
    {
        private Connection _conn;
        private Connection _connHost;
        private Connection _connJustTrueServer;
        private Connection _connFalse;



        [TestInitialize]
        public void UnitTestInitializer()
        {
            _conn = new Connection(".", "UsersManagements") { Name = "UM" };
            _connHost = new Connection("localhost", "UsersManagements") { Name = "UM" };
            _connJustTrueServer = new Connection("localhost", "TestNotExistDbName") { Name = "Test" };
            _connFalse = new Connection("TestNotExistServer", "TestNotExistDbName") { Name = "Test" };
        }


        #region Test StringCipher.cs

        [TestMethod, TestCategory("StringCipher.cs")]
        public void RandomPassword()
        {
            for (int i = 0; i < 1000; i++)
            {
                var password = GetHexId(10);

                var plaintext = GetRandomText(10000);

                var encryptedstring = plaintext.Encrypt(password);

                var decryptedstring = encryptedstring.Decrypt(password);

                Assert.AreEqual(plaintext, decryptedstring);
            }
        }

        [TestMethod, TestCategory("StringCipher.cs")]
        public void PublicPassword()
        {
            for (int i = 0; i < 1000; i++)
            {
                var plaintext = GetRandomText(10000);

                var encryptedstring = plaintext.Encrypt();


                var decryptedstring = encryptedstring.Decrypt();

                Assert.AreEqual(plaintext, decryptedstring);
            }
        }

        [TestMethod, TestCategory("StringCipher.cs")]
        public void RandomPasswordUtf8()
        {
            for (int i = 0; i < 1000; i++)
            {
                var password = GetHexId(10);

                var plaintext = GetRandomTextUtf8(10000);

                var encryptedstring = StringCipher.Encrypt(plaintext, password);

                var decryptedstring = StringCipher.Decrypt(encryptedstring, password);

                Assert.AreEqual(plaintext, decryptedstring);
            }
        }

        [TestMethod, TestCategory("StringCipher.cs")]
        public void PublicPasswordUtf8()
        {
            for (int i = 0; i < 1000; i++)
            {
                var plaintext = GetRandomTextUtf8(10000);

                var encryptedstring = plaintext.Encrypt();


                var decryptedstring = encryptedstring.Decrypt();

                Assert.AreEqual(plaintext, decryptedstring);
            }
        }


        protected string GetHexId(int len)
        {
            var chars = "ABCDEF0123456789";

            var random = new System.Random();

            var result = new string(
                Enumerable.Repeat(chars, len)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());


            return result;
        }

        protected string GetRandomText(int len)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            var random = new System.Random();

            var result = new string(
                Enumerable.Repeat(chars, random.Next(1, len))
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return result;
        }

        protected string GetRandomTextUtf8(int len)
        {
            const string chars = @"پضصثقفغعهخحجچگکمنتاآلبیسشظطزرذدئو./ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789~!@#$%^&*()_+-|\";

            var random = new System.Random();

            var result = new string(
                Enumerable.Repeat(chars, random.Next(1, len))
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return result;
        }

        #endregion


        #region Test Connection.cs

        [TestMethod, TestCategory("Connection.cs")]
        public void CreateConnection1Args()
        {
            //
            // --------------- Constructor 1 Arguments ------------------------------------------
            //
            var connFull = new Connection(".", "UsersManagements", "um");
            var conn = new Connection(connFull);

            Assert.AreEqual(conn.ToXml(false).ToString(SaveOptions.None), conn.ToString());
            Assert.AreEqual(conn.ToXml(true).ToString(SaveOptions.None), conn.ToString(true));
            var newConnWithoutId = conn.ToXml(true);
            TestTools.ExceptException<NullReferenceException>(() => newConnWithoutId.Element("add").Attribute("publicKeyToken").Value = "");
            var c = Connection.Parse(newConnWithoutId);
            Assert.AreEqual(c.ConnectionString, conn.ConnectionString); // c.Id != conn.Id but both of them have duplicate connectionString

            var conn2 = new Connection(conn.ConnectionString);

            TestTools.ExpectException<NullReferenceException>(() => conn.ConnectionString = null); // Test throw NullReferenceException

            Assert.AreEqual(conn.ConnectionString, conn2.ConnectionString);
        }

        [TestMethod, TestCategory("Connection.cs")]
        public void CreateConnection2Args()
        {
            //
            // --------------- Constructor 2 Arguments ------------------------------------------
            //
            var connFull = new Connection(".", "UsersManagements", "sa", "123", 3, "Test Connection", "um");
            var conn = new Connection(connFull.ConnectionString);

            Assert.AreEqual(conn.ToXml(false).ToString(SaveOptions.None), conn.ToString());
            Assert.AreEqual(conn.ToXml(true).ToString(SaveOptions.None), conn.ToString(true));
            Assert.AreEqual(
                Connection.Parse(conn.ToXml(true)).ConnectionString,
                conn.ConnectionString);

            var conn2 = new Connection(conn.ConnectionString);

            Assert.AreEqual(conn.ConnectionString, conn2.ConnectionString);
        }

        [TestMethod, TestCategory("Connection.cs")]
        public void CreateConnection3Args()
        {
            //
            // --------------- Constructor 3 Arguments ------------------------------------------
            //
            var conn1 = new Connection(".", "UsersManagements");
            conn1.Name = "UM";
            Assert.AreEqual(conn1.ToXml().ToString(SaveOptions.None), conn1.ToString());
            Assert.AreEqual(conn1.ToXml(true).ToString(SaveOptions.None), conn1.ToString(true));
            Assert.AreEqual(
                Connection.Parse(conn1.ToXml(true)).ConnectionString,
                conn1.ConnectionString);
        }

        [TestMethod, TestCategory("Connection.cs")]
        public void CreateConnection4Args()
        {
            //
            // --------------- Constructor 4 Arguments ------------------------------------------
            //
            var conn = new Connection();

            Assert.AreEqual(conn.ToXml().ToString(SaveOptions.None), conn.ToString());
            Assert.AreEqual(conn.ToXml(true).ToString(SaveOptions.None), conn.ToString(true));
            Assert.AreEqual(
                Connection.Parse(conn.ToXml(true)).ConnectionString,
                conn.ConnectionString);


            var conn1_2 = new Connection()
            {
                ConnectionString = conn.ConnectionString
            };

            Assert.AreEqual(conn.ConnectionString, conn1_2.ConnectionString);
        }

        [TestMethod, TestCategory("Connection.cs")]
        public void CreateConnection6Args()
        {
            //
            // --------------- Constructor 6 Arguments ------------------------------------------
            //
            var conn2 = new Connection(".", "UsersManagements", "sa", "123", -3);

            Assert.AreEqual(conn2.ToXml().ToString(SaveOptions.None), conn2.ToString());
            Assert.AreEqual(conn2.ToXml(true).ToString(SaveOptions.None), conn2.ToString(true));
            Assert.AreEqual(
                Connection.Parse(conn2.ToXml(true)).ConnectionString,
                conn2.ConnectionString);


            var conn2_2 = new Connection()
            {
                ConnectionString = conn2.ConnectionString
            };

            Assert.AreEqual(conn2.ConnectionString, conn2_2.ConnectionString);
        }

        [TestMethod, TestCategory("Connection.cs")]
        public void CreateConnection8Args()
        {
            //
            // --------------- Constructor 8 Arguments ------------------------------------------
            //
            var conn3 = new Connection(".", "UsersManagements", "sa", "123", 3, "Test Connection", "name");

            Assert.AreEqual(conn3.ToXml(false).ToString(SaveOptions.None), conn3.ToString());
            Assert.AreEqual(conn3.ToXml(true).ToString(SaveOptions.None), conn3.ToString(true));
            Assert.AreEqual(
                Connection.Parse(conn3.ToXml(true)).ConnectionString,
                conn3.ConnectionString);

            var conn3_2 = new Connection()
            {
                ConnectionString = conn3.ConnectionString
            };

            Assert.AreEqual(conn3.ConnectionString, conn3_2.ConnectionString);
        }


        [TestMethod, TestCategory("Connection.cs")]
        public void ConnectionExplicitConverting()
        {
            var conn = new Connection("UM", ".", "UsersManagements");

            string connStr = (string)conn;
            XElement connXml = (XElement)conn;

            Assert.AreEqual(connStr, conn.ConnectionString);
            Assert.AreEqual(connXml.ToString(SaveOptions.None), conn.ToXml().ToString(SaveOptions.None));
        }

        [TestMethod, TestCategory("Connection.cs")]
        public void ConnectionImplicitConverting()
        {
            var conn = new Connection("UM", ".", "UsersManagements");

            var connStr = (Connection)conn.ConnectionString;
            var connXml = (Connection)conn.ToXml();

            Assert.AreEqual(connStr.ConnectionString, conn.ConnectionString);
            Assert.AreEqual(connXml.ConnectionString, conn.ConnectionString);
        }

        [TestMethod, TestCategory("Connection.cs")]
        public void ConnectionCloneConverting()
        {
            var conn = new Connection("UM", ".", "UsersManagements");

            var connObj = conn.Clone();
            var connObjThis = new Connection(conn);

            TestTools.ExpectException<ArgumentNullException>(() => Connection.Clone(conn, null)); // throw ArgumentNullException
            TestTools.ExpectException<ArgumentNullException>(() => Connection.Clone(null, connObjThis));  // throw ArgumentNullException

            Connection.Clone(conn, connObjThis);

            Assert.AreEqual(conn.ConnectionString, ((Connection)connObj).ConnectionString);
            Assert.AreEqual(connObjThis.ConnectionString, ((Connection)connObj).ConnectionString);
            Assert.AreEqual(connObjThis.ConnectionString, conn.ConnectionString);
        }

        #endregion


        #region Test ConnectionCollection.cs


        [TestMethod, TestCategory("ConnectionCollection.cs")]
        public void TestConnectionCollection()
        {
            var items = new ConnectionCollection { _conn, _connHost, _connFalse, _connJustTrueServer };

            items["Test"] = _conn;

            Assert.AreEqual(items["Test"].ConnectionString, _conn.ConnectionString);

            foreach (var cm in items.Select(x => new ConnectionManager(x)))
            {
                TestTools.ExceptException<SqlException>(() =>
                {
                    cm.Open();
                    Assert.IsTrue(cm.State == ConnectionState.Open);
                });
            }
        }

        #endregion


        #region Test ConnectionManager.cs


        [TestMethod, TestCategory("ConnectionManager.cs")]
        public void TestMultipleConnectionOpenAsyncByCancellationToken()
        {
            var cm = ConnectionManager.Add(_conn, "um");
            Assert.IsTrue(cm.State == ConnectionState.Closed);

            var counter = 0;
            var n = 100000;
            var ct = new CancellationToken();

            Parallel.For(0, n, async i =>
            {
                //counter += i;
                Interlocked.Add(ref counter, i);

                await cm.OpenAsync(ct);
                Assert.IsTrue(cm.State == ConnectionState.Open);
                cm.Close();
                Assert.IsTrue(cm.State == ConnectionState.Closed);
            });


            var sum = n * (n - 1) / 2;
            Assert.AreEqual(counter, sum, string.Format("Counter:{0} , Sum:{1}", counter, sum));
        }



        [TestMethod, TestCategory("ConnectionManager.cs")]
        public void TestAddFind()
        {
            //
            // Test Add and Find
            //
            var c1 = ConnectionManager.Add(_conn, "um");
            var c2 = ConnectionManager.Find("um");
            Assert.AreEqual(c1.ConnectionString, c2.ConnectionString);
            //
            // Test Search Connection by Ignore Case Name
            //
            Assert.IsNotNull(ConnectionManager.Find("UM"));
            Assert.IsNotNull(ConnectionManager.Find("um"));
            Assert.IsNotNull(ConnectionManager.Find("uM"));
            Assert.IsNotNull(ConnectionManager.Find("Um"));
        }

        [TestMethod, TestCategory("ConnectionManager.cs")]
        public void TestXml()
        {
            //
            // Convert connections to XML
            //
            ConnectionManager.Add(_conn, "um");
            ConnectionManager.Add(_connJustTrueServer, "test");
            var xmlConnections = ConnectionManager.SaveToXml();
            var xmlConnectionsEncrypted = ConnectionManager.SaveToXml(true);

            ConnectionManager.ClearAll(); ;
            Assert.IsTrue(ConnectionManager.Count == 0);

            ConnectionManager.LoadFromXml(xmlConnections);
            Assert.IsTrue(ConnectionManager.Count == 2);

            ConnectionManager.ClearAll();
            Assert.IsTrue(ConnectionManager.Count == 0);

            ConnectionManager.LoadFromXml(xmlConnectionsEncrypted);
            Assert.IsTrue(ConnectionManager.Count == 2);
        }

        #endregion
    }
}