using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GB_ServerManager.Helpers
{
    internal static class SteamA2SHelper
    {
        /// <summary>Adopted from Valve description: https://developer.valvesoftware.com/wiki/Server_queries#A2S_INFO </summary>
        public class A2S_INFO
        {
            // \xFF\xFF\xFF\xFFTSource Engine Query\x00 because UTF-8 doesn't like to encode 0xFF
            public static readonly byte[] REQUEST = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0x54, 0x53, 0x6F, 0x75, 0x72, 0x63, 0x65, 0x20, 0x45, 0x6E, 0x67, 0x69, 0x6E, 0x65, 0x20, 0x51, 0x75, 0x65, 0x72, 0x79, 0x00 };
            #region Strong Typing Enumerators
            [Flags]
            public enum ExtraDataFlags : byte
            {
                GameID = 0x01,
                SteamID = 0x10,
                Keywords = 0x20,
                Spectator = 0x40,
                Port = 0x80
            }
            public enum VACFlags : byte
            {
                Unsecured = 0,
                Secured = 1
            }
            public enum VisibilityFlags : byte
            {
                Public = 0,
                Private = 1
            }
            public enum EnvironmentFlags : byte
            {
                Linux = 0x6C,   //l
                Windows = 0x77, //w
                Mac = 0x6D,     //m
                MacOsX = 0x6F   //o
            }
            public enum ServerTypeFlags : byte
            {
                Dedicated = 0x64,     //d
                Nondedicated = 0x6C,   //l
                SourceTV = 0x70   //p
            }
            #endregion
            public class A2S_Information
            {
                public byte Header { get; set; }        // I
                public byte Protocol { get; set; }
                public string Name { get; set; }
                public string Map { get; set; }
                public string Folder { get; set; }
                public string Game { get; set; }
                public short ID { get; set; }
                public byte Players { get; set; }
                public byte MaxPlayers { get; set; }
                public byte Bots { get; set; }
                public ServerTypeFlags ServerType { get; set; }
                public EnvironmentFlags Environment { get; set; }
                public VisibilityFlags Visibility { get; set; }
                public VACFlags VAC { get; set; }
                public string Version { get; set; }
                public ExtraDataFlags ExtraDataFlag { get; set; }
                #region Extra Data Flag Members
                public ulong GameID { get; set; }           //0x01
                public ulong SteamID { get; set; }          //0x10
                public string Keywords { get; set; }        //0x20
                public string Spectator { get; set; }       //0x40
                public short SpectatorPort { get; set; }   //0x40
                public short Port { get; set; }             //0x80
            }
            #endregion            
            public static A2S_Information GetA2SInformation(IPEndPoint ep)
            {
                A2S_Information info = new A2S_Information();
                UdpClient udp = new UdpClient();
                udp.Send(REQUEST, REQUEST.Length, ep);
                MemoryStream ms = new MemoryStream(udp.Receive(ref ep));    // Saves the received data in a memory buffer
                BinaryReader br = new BinaryReader(ms, Encoding.UTF8);      // A binary reader that treats charaters as Unicode 8-bit
                ms.Seek(4, SeekOrigin.Begin);   // skip the 4 0xFFs

                info.Header = br.ReadByte();
                info.Protocol = br.ReadByte();
                info.Name = ReadNullTerminatedString(ref br);
                info.Map = ReadNullTerminatedString(ref br);
                info.Folder = ReadNullTerminatedString(ref br);
                info.Game = ReadNullTerminatedString(ref br);
                info.ID = br.ReadInt16();
                info.Players = br.ReadByte();
                info.MaxPlayers = br.ReadByte();
                info.Bots = br.ReadByte();
                info.ServerType = (ServerTypeFlags)br.ReadByte();
                info.Environment = (EnvironmentFlags)br.ReadByte();
                info.Visibility = (VisibilityFlags)br.ReadByte();
                info.VAC = (VACFlags)br.ReadByte();
                info.Version = ReadNullTerminatedString(ref br);
                info.ExtraDataFlag = (ExtraDataFlags)br.ReadByte();
                #region These EDF readers have to be in this order because that's the way they are reported
                if (info.ExtraDataFlag.HasFlag(ExtraDataFlags.Port))
                    info.Port = br.ReadInt16();
                if (info.ExtraDataFlag.HasFlag(ExtraDataFlags.SteamID))
                    info.SteamID = br.ReadUInt64();
                if (info.ExtraDataFlag.HasFlag(ExtraDataFlags.Spectator))
                {
                    info.SpectatorPort = br.ReadInt16();
                    info.Spectator = ReadNullTerminatedString(ref br);
                }
                if (info.ExtraDataFlag.HasFlag(ExtraDataFlags.Keywords))
                    info.Keywords = ReadNullTerminatedString(ref br);
                if (info.ExtraDataFlag.HasFlag(ExtraDataFlags.GameID))
                    info.GameID = br.ReadUInt64();
                #endregion
                br.Close();
                ms.Close();
                udp.Close();

                return info;
            }
            /// <summary>Reads a null-terminated string into a .NET Framework compatible string.</summary>
            /// <param name="input">Binary reader to pull the null-terminated string from.  Make sure it is correctly positioned in the stream before calling.</param>
            /// <returns>String of the same encoding as the input BinaryReader.</returns>
            public static string ReadNullTerminatedString(ref BinaryReader input)
            {
                StringBuilder sb = new StringBuilder();
                char read = input.ReadChar();
                while (read != '\x00')
                {
                    sb.Append(read);
                    read = input.ReadChar();
                }
                return sb.ToString();
            }
        }
    }
}
