using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferri_Emulator.Core;
using System.Reflection;
using Ferri_Emulator.Database;
using Ferri_Emulator.Database.Mappings;
using NHibernate.Criterion;
using NHibernate;
using Ferri_Emulator.Connections;
using System.IO;
using Ferri_Emulator.Messages;
using Ferri_Encryption;
using Ferri_Emulator.Habbo_Hotel;
using Ferri_Emulator.Utilities;
using Ferri_Emulator.Communication;

namespace Ferri_Emulator
{
    public class Engine
    {
        public static string Version;

        public static Configuration Configuration = new Configuration();
        public static Logging Logging = new Logging();
        public static MySQLManager dbManager = new MySQLManager();
        public static ISession dbSession;
        public static ServerSocket Network;
        public static RemoteNetwork Remote;
        public static PacketHandler Packethandler = new PacketHandler();
        public static HashString PublicHash;

        public static Habbo_Hotel.Habbo_Hotel GetHabboHotel = new Habbo_Hotel.Habbo_Hotel();

        internal static BigInteger N = new BigInteger("86851DD364D5C5CECE3C883171CC6DDC5760779B992482BD1E20DD296888DF91B33B936A7B93F06D29E8870F703A216257DEC7C81DE0058FEA4CC5116F75E6EFC4E9113513E45357DC3FD43D4EFAB5963EF178B78BD61E81A14C603B24C8BCCE0A12230B320045498EDC29282FF0603BC7B7DAE8FC1B05B52B2F301A9DC783B7", 16);
        internal static BigInteger E = new BigInteger(2);
        internal static BigInteger D = new BigInteger("59AE13E243392E89DED305764BDD9E92E4EAFA67BB6DAC7E1415E8C645B0950BCCD26246FD0D4AF37145AF5FA026C0EC3A94853013EAAE5FF1888360F4F9449EE023762EC195DFF3F30CA0B08B8C947E3859877B5D7DCED5C8715C58B53740B84E11FBC71349A27C31745FCEFEEEA57CFF291099205E230E0C7C27E8E1C0512B", 16);

        internal static Dictionary<string, string> BannerTokenValues = new Dictionary<string, string>();
        internal static Dictionary<uint, List<Session>> RoomsLoaded = new Dictionary<uint, List<Session>>();

        internal static string ToReadableString(string s)
        {
            foreach (char c in s)
            {
                if (c > 20)
                    continue; 

                s = s.Replace(c.ToString(), "[" + (int)c + "]");
            }

            return s;
        }

        static void Main(string[] args)
        {
            if (!Directory.Exists("Settings") && !File.Exists("Settings/config.ini"))
            {
                Configuration.CreateFolder("Settings");
                Configuration.CreateFile("Settings/config.ini");
                Configuration.SetConfigurationFile("Settings/config.ini");
                Configuration.AppendValues("version", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            }

            Configuration.SetConfigurationFile("Settings/config.ini");
            Configuration.ReadConfigurationFile();
            Configuration.PopValue<string>("version", out Version);

            Logging.SetTitle("FerriEmulator - v{0}", Version);
            Logging.WriteTagLine("Debug", "Initializing Ferri v{0} for {1}", Version, Environment.UserName);

            dbManager.CreateConnectionString();

            GetHabboHotel.LoadHH();

            ServerSocketSettings Settings = new ServerSocketSettings()
            {
                Backlog = 10,
                BufferSize = 512,
                Endpoint = new System.Net.IPEndPoint(System.Net.IPAddress.Any, 30000),
                MaxConnections = 10000,
                MaxSimultaneousAcceptOps = 15,
                NumOfSaeaForRec = 10000,
                NumOfSaeaForSend = 20000
            };

            Network = new ServerSocket(Settings);
            Network.Init();
            Network.StartListen();

            Remote = new RemoteNetwork(30001);
            Logging.WriteTagLine("Ready", "Initialized FerriEmulator, ready for connections!");

            Console.WriteLine();

            while (true)
                Console.ReadLine();
        }
    }
}
