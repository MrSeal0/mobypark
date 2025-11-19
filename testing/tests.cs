using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Data.Sqlite;
using Dapper;


using API_C_.LogicLayer;    // Encryption
using API_C_.DataAcces;     // UsersAcces, SessionAcces, ParkingSessionsAcces
using API_C_.Controllers;   // RegisterRequest

namespace API_Tests
{
    [TestClass]
    public class LogicTests
    {
        private const string DbFolder = "DataSources";
        private const string DbFile = "DataBase.db";
        private static string _fullPath;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            Directory.CreateDirectory(DbFolder);
            _fullPath = Path.GetFullPath(Path.Combine(DbFolder, DbFile));
            if (File.Exists(_fullPath)) File.Delete(_fullPath);

            using var conn = new SqliteConnection($"Data Source={_fullPath}");
            conn.Open();

            // minimal schema used by tests (adjust column/type names to match your real schema)
            conn.Execute(@"
                CREATE TABLE users (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    username TEXT,
                    password TEXT,
                    full_name TEXT,
                    email TEXT,
                    phone TEXT,
                    role TEXT,
                    creation_date TEXT,
                    birth_year INTEGER,
                    active INTEGER
                );
                CREATE TABLE sessions (
                    SessionID TEXT PRIMARY KEY,
                    UserID INTEGER,
                    CreatedAt TEXT
                );
                CREATE TABLE parking_Sessions (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    parking_lot_id INTEGER,
                    user_id INTEGER,
                    license_plate TEXT,
                    start_time TEXT,
                    end_time TEXT,
                    duration_minutes INTEGER,
                    cost REAL,
                    payment_status TEXT
                );
            ");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            try { if (File.Exists(_fullPath)) File.Delete(_fullPath); } catch { }
        }

        [TestMethod]
        public void EncryptionHash_KnownValue()
        {
            var hash = Encryption.Hash("test");
            Assert.AreEqual("098f6bcd4621d373cade4e832627b4f6", hash);
        }

        [TestMethod]
        public void UsersAcces_IsTaken_FindsExistingUser()
        {
            using var conn = new SqliteConnection($"Data Source={_fullPath}");
            conn.Open();
            conn.Execute("INSERT INTO users (username, password) VALUES (@u, @p)", new { u = "alice", p = "pw" });

            var access = new UsersAcces();
            bool taken = access.IsTaken("alice");
            Assert.IsTrue(taken);
        }

        [TestMethod]
        public void UsersAcces_InsertAccount_InsertsRow()
        {
            var register = new RegisterRequest
            {
                username = "bob",
                password = "hash",
                name = "Bob",
                email = "b@b.com",
                phone = "123",
                byear = 1990
            };

            var access = new UsersAcces();
            access.InsertAccount(register);

            using var conn = new SqliteConnection($"Data Source={_fullPath}");
            conn.Open();
            var row = conn.QuerySingle<int>("SELECT COUNT(1) FROM users WHERE username = @u", new { u = "bob" });
            Assert.AreEqual(1, row);
        }

        [TestMethod]
        public void SessionAcces_AddAndReadSession()
        {
            var saccess = new SessionAcces();
            string sid = Guid.NewGuid().ToString();
            int uid = 42;

            saccess.AddSession(sid, uid);
            int got = saccess.UidFromSession(sid);

            Assert.AreEqual(uid, got);
        }

        [TestMethod]
        public void ParkingSessionsAcces_StartCreatesRow()
        {
            var paccess = new ParkingSessionsAcces();
            string plate = "ABC123";
            int uid = 7;
            int plid = 1;

            paccess.StartParkSession(plate, uid, plid);

            using var conn = new SqliteConnection($"Data Source={_fullPath}");
            conn.Open();
            var exists = conn.QuerySingle<int>("SELECT COUNT(1) FROM parking_Sessions WHERE license_plate = @p AND user_id = @u", new { p = plate, u = uid });

            Assert.AreEqual(1, exists);
        }
    }
}