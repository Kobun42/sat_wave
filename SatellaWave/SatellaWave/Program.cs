﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;

namespace SatellaWave
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>

        public static MainWindow mainWindow;
        public static string lastSavedXMLFile = "";

        public static readonly string[] buildingList = {
            "Robot Tower",
            "News Wall",
            "Broadcast Station",
            "Burger Shop",
            "Police Box",
            "Calculator Building",
            "Beach House (Shop)",
            "Stadium",
            "Convenience Center (Shop)",
            "Girls School",
            "Game Factory",
            "Department Store",
            "Game Museum",
            "Abacus Building",
            "Tofu Hall",
            "Event Plaza",
            "Bagpotamia Temple",
            "Celebrity House",
            "Private House",
            "Telephone Booth",
            "Sewerage (Shop)"
        };

        public static readonly string[] peopleList =
        {
            "[Red Ball at Beach]",
            "Dr. Hiroshi",
            "Dororin",
            "Temple Guardian Left",
            "Temple Guardian Right",
            "Ghost",
            "Otakuman",
            "Gorou",
            "Samson",
            "Gozen Reiji",
            "Tamotsu Sekishita",
            "Mr. Arai",
            "Rinzo Charikawa",
            "Star Rarawo",
            "Manbei",
            "Kenichi",
            "Youta",
            "MIO",
            "MIO (School Uniform)",
            "Reiko",
            "Marina",
            "Akane",
            "Mako",
            "Midori",
            "Suzu Charikawa",
            "Ms. Sera",
            "Secretary Akiko",
            "Tomoko Shirase",
            "Yuka Tsutsumi",
            "Ina Sanda",
            "Fortuneteller Miki",
            "Asaji Kayo",
            "Kimono Girl",
            "Ikebe",
            "Ms. Ochiyo",
            "Old Woman",
            "Tell",
            "Sachiko",
            "Akiko",
            "Rocky (Dog)",
            "Jitsu Hyoue (Cat)",
            "Quack (Duck)",
            "TeeVee",
            "Wide TeeVee",
            "[Custom Script 1]",
            "[Custom Script 2]"
        };

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            mainWindow = new MainWindow();

            Application.Run(mainWindow);
        }

        public static void NewRepository()
        {
            //New repository needs Town Status and Directory (TODO) at least
            TownStatus _town = new TownStatus(0x0101, 0x0005, "Town Status", 0x0123);

            mainWindow.treeViewChn.Nodes.Clear();
            AddChannel(_town);

            lastSavedXMLFile = "";
        }

        public static void AddChannel(Channel _chn)
        {
            TreeNode _node = new TreeNode(_chn.name + " (" + _chn.GetChannelNumberString() + ")");
            _node.Tag = _chn;

            if (_chn.GetType() != typeof(Directory))
                _node.ContextMenuStrip = mainWindow.contextMenuStripChannelMenu;
            else
                _node.ContextMenuStrip = mainWindow.contextMenuStripDirectoryMenu;

            mainWindow.treeViewChn.Nodes.Add(_node);
            mainWindow.treeViewChn.SelectedNode = _node;
        }

        public static void AddChannel(int type)
        {
            if (type == 0)
            {
                //BS-X - Welcome Message (1.1.0.4)
                //Check if already present
                if (CheckUsedChannel("1.1.0.4"))
                {
                    MessageBox.Show("There is already a BS-X Message Channel.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageChannel _msg = new MessageChannel(0x0101, 0x0004, "Welcome Message", GetNextLCI(), "");
                AddChannel(_msg);
            }
            else if(type == 1)
            {
                //BS-X - Town Status (1.1.0.5)
                //Check if already present
                if (CheckUsedChannel("1.1.0.5"))
                {
                    MessageBox.Show("There is already a BS-X Town Status Channel.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                TownStatus _town = new TownStatus(0x0101, 0x0005, "Town Status", GetNextLCI());
                AddChannel(_town);
            }
            else if (type == 2)
            {
                //BS-X - Directory (1.1.0.6)
                //Check if already present
                if (CheckUsedChannel("1.1.0.6"))
                {
                    MessageBox.Show("There is already a BS-X Directory Channel.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Directory _dir = new Directory(0x0101, 0x0006, "Directory", GetNextLCI());
                AddChannel(_dir);
            }
            else if (type == 3)
            {
                //BS-X - Time Channel (1.1.0.8)
                if (CheckUsedChannel("1.1.0.8"))
                {
                    MessageBox.Show("There is already a BS-X Time Channel.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Channel _time = new Channel(0x0101, 0x0008, "Time Channel [BS-X]", 0x0000);
                AddChannel(_time);
            }
            else if (type == 4)
            {
                //Game - Time Channel (1.2.0.48)
                if (CheckUsedChannel("1.2.0.48"))
                {
                    MessageBox.Show("There is already a Game Time Channel.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Channel _time = new Channel(0x0102, 0x0030, "Time Channel [Game]", 0x0000);
                AddChannel(_time);
            }
            else if (type == 5)
            {
                //Itoi Shigesato no Bass Tsuri No. 1 - Contest 1 (1.2.130.0)
                if (CheckUsedChannel("1.2.130.0"))
                {
                    MessageBox.Show("There is already a Itoi Shigesato no Bass Tsuri No. 1 - Contest 1 Channel.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Channel _contest = new Channel(0x0102, 0x8200, "Itoi Shigesato no Bass Tsuri No. 1 - Contest 1", 0x0000);
                AddChannel(_contest);
            }
            else if (type == 6)
            {
                //Itoi Shigesato no Bass Tsuri No. 1 - Contest 2 (1.2.130.16)
                if (CheckUsedChannel("1.2.130.16"))
                {
                    MessageBox.Show("There is already a Itoi Shigesato no Bass Tsuri No. 1 - Contest 2 Channel.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Channel _contest = new Channel(0x0102, 0x8210, "Itoi Shigesato no Bass Tsuri No. 1 - Contest 2", 0x0000);
                AddChannel(_contest);
            }
            else if (type == 7)
            {
                //Itoi Shigesato no Bass Tsuri No. 1 - Contest 3 (1.2.130.32)
                if (CheckUsedChannel("1.2.130.32"))
                {
                    MessageBox.Show("There is already a Itoi Shigesato no Bass Tsuri No. 1 - Contest 3 Channel.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Channel _contest = new Channel(0x0102, 0x8220, "Itoi Shigesato no Bass Tsuri No. 1 - Contest 3", 0x0000);
                AddChannel(_contest);
            }
            else if (type == 8)
            {
                //Itoi Shigesato no Bass Tsuri No. 1 - Contest 4 (1.2.130.48)
                if (CheckUsedChannel("1.2.130.48"))
                {
                    MessageBox.Show("There is already a Itoi Shigesato no Bass Tsuri No. 1 - Contest 4 Channel.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Channel _contest = new Channel(0x0102, 0x8230, "Itoi Shigesato no Bass Tsuri No. 1 - Contest 4", 0x0000);
                AddChannel(_contest);
            }
        }

        public static void AddFolder(TreeNode _node)
        {
            if (_node.Tag.GetType() == typeof(Directory))
            {
                Folder _folder = new Folder();
                TreeNode _tnode = new TreeNode(_folder.name);
                _tnode.Tag = _folder;
                _tnode.ContextMenuStrip = mainWindow.contextMenuStripFolderMenu;
                _node.Nodes.Add(_tnode);
                mainWindow.treeViewChn.SelectedNode = _tnode;
            }
        }

        public static void AddFile(TreeNode _node)
        {
            if (_node.Tag.GetType() == typeof(Folder))
            {
                DownloadFile _file = new DownloadFile((_node.Tag as Folder).purpose == 1, GetNextFileID());

                _file.lci = GetNextLCI();
                _file.program_number = GetNextProgramNumber();
                _file.service_broadcast = 0x0103; //Dedicated to content, more than enough

                TreeNode _tnode = new TreeNode(_file.name);
                _tnode.Tag = _file;
                _tnode.ContextMenuStrip = mainWindow.contextMenuStripFileMenu;
                _node.Nodes.Add(_tnode);
                mainWindow.treeViewChn.SelectedNode = _tnode;
            }
            else if (_node.Tag.GetType() == typeof(DownloadFile))
            {
                DownloadFile _file = new DownloadFile((_node.Tag as DownloadFile).isItem, (_node.Tag as DownloadFile).fileID);

                _file.lci = GetNextLCI();

                _file.program_number = (_node.Tag as DownloadFile).program_number;
                _file.service_broadcast = (_node.Tag as DownloadFile).service_broadcast; //Dedicated to content, more than enough

                while (CheckUsedChannel(_file.GetChannelNumberString()))
                {
                    _file.program_number += 1;
                }

                TreeNode _tnode = new TreeNode(_file.name);
                _tnode.Tag = _file;
                _tnode.ContextMenuStrip = mainWindow.contextMenuStripChannelMenu;
                _node.Nodes.Add(_tnode);
                mainWindow.treeViewChn.SelectedNode = _tnode;
            }
        }

        public static bool CheckUsedChannel(string _chnNumber)
        {
            foreach (TreeNode _node in mainWindow.treeViewChn.Nodes)
            {
                if (_node.Tag.GetType() == typeof(Directory))
                {
                    //Check Folders
                    foreach (TreeNode _nodeChildFolder in _node.Nodes)
                    {
                        if (_nodeChildFolder.Tag.GetType() == typeof(Folder))
                        {
                            //Check Files
                            foreach (TreeNode _nodeFile in _nodeChildFolder.Nodes)
                            {
                                if ((_nodeFile.Tag as DownloadFile).GetChannelNumberString() == _chnNumber)
                                {
                                    return true;
                                }

                                if (_nodeFile.Nodes.Count > 0)
                                {
                                    //Check Include Files if they exist
                                    foreach (TreeNode _nodeIncFile in _nodeFile.Nodes)
                                    {
                                        if ((_nodeIncFile.Tag as DownloadFile).GetChannelNumberString() == _chnNumber)
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if ((_node.Tag as Channel).GetChannelNumberString() == _chnNumber)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool CheckUsedChannel(ushort _service, ushort _program)
        {
            return CheckUsedChannel((_service >> 8).ToString()
                + "." + (_service & 0xFF).ToString()
                + "." + (_program >> 8).ToString()
                + "." + (_program & 0xFF).ToString());
        }

        public static bool CheckUsedLCI(ushort _lci)
        {
            if (_lci == 0x0124)
                return true;

            foreach (TreeNode _node in mainWindow.treeViewChn.Nodes)
            {
                if (_node.Tag.GetType() == typeof(Directory))
                {
                    //Check Folders
                    foreach (TreeNode _nodeChildFolder in _node.Nodes)
                    {
                        if (_nodeChildFolder.Tag.GetType() == typeof(Folder))
                        {
                            //Check Files
                            foreach (TreeNode _nodeFile in _nodeChildFolder.Nodes)
                            {
                                if ((_nodeFile.Tag as DownloadFile).lci == _lci)
                                {
                                    return true;
                                }

                                if (_nodeFile.Nodes.Count > 0)
                                {
                                    //Check Include Files if they exist
                                    foreach (TreeNode _nodeIncFile in _nodeFile.Nodes)
                                    {
                                        if ((_nodeIncFile.Tag as DownloadFile).lci == _lci)
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if ((_node.Tag as Channel).lci == _lci)
                    {
                        return true;
                    }
                }
            }

            //not found
            return false;
        }

        public static bool CheckUsedFileID(byte _fileID)
        {
            foreach (TreeNode _node in mainWindow.treeViewChn.Nodes)
            {
                if (_node.Tag.GetType() == typeof(Directory))
                {
                    //Check Folders
                    foreach (TreeNode _nodeChildFolder in _node.Nodes)
                    {
                        if (_nodeChildFolder.Tag.GetType() == typeof(Folder))
                        {
                            //Check Files (don't need to check include files, it shares the same file ID)
                            foreach (TreeNode _nodeFile in _nodeChildFolder.Nodes)
                            {
                                if ((_nodeFile.Tag as DownloadFile).fileID == _fileID)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            //not found
            return false;
        }

        public static byte GetNextFileID()
        {
            byte nextfileid = 0;
            while (CheckUsedFileID(nextfileid))
            {
                nextfileid++;
            }

            return nextfileid;
        }

        public static ushort GetNextLCI()
        {
            ushort nextlci = 0x0120;
            while (CheckUsedLCI(nextlci))
            {
                nextlci = (ushort)(nextlci & 0x1F | (nextlci >> 3));
                nextlci++;
                nextlci = (ushort)(nextlci | ((nextlci & 0x3E0) << 3) | 0x0020);
            }

            return nextlci;
        }

        public static ushort GetNextProgramNumber()
        {
            ushort nextprgnumber = 0;
            while (CheckUsedChannel(0x0103, nextprgnumber))
                nextprgnumber += 0x0100;
            return nextprgnumber;
        }

        public static void LoadBSXRepository(string xmlPath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);
            List<TreeNode> nodelist = new List<TreeNode>();

            if (xmlDoc.DocumentElement.Name == "bsx")
            {
                foreach (XmlNode nodeChannel in xmlDoc.DocumentElement.ChildNodes)
                {
                    if (nodeChannel.Name == "channel")
                    {
                        //Verify Channel Integrity
                        if (!(new Regex(@"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$").Match(nodeChannel.Attributes["broadcast"].Value).Success))
                        {
                            //Software Channel
                            MessageBox.Show("Software Channel is invalid in channel " + nodeChannel.BaseURI + " (" + nodeChannel.Attributes["name"].Value + ")", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (!(new Regex(@"^[0-9a-fA-F]{4}$").Match(nodeChannel.Attributes["lci"].Value).Success))
                        {
                            //LCI
                            MessageBox.Show("Logical Channel LCI is invalid in channel " + nodeChannel.BaseURI + " (" + nodeChannel.Attributes["name"].Value + ")", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (!(new Regex(@"^[0-9]+$").Match(nodeChannel.Attributes["timeout"].Value).Success))
                        {
                            //Timeout
                            MessageBox.Show("Timeout is invalid in channel " + nodeChannel.BaseURI + " (" + nodeChannel.Attributes["name"].Value + ")", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        ushort _pv = (ushort)((Convert.ToByte(nodeChannel.Attributes["broadcast"].Value.Split('.')[0]) << 8) | Convert.ToByte(nodeChannel.Attributes["broadcast"].Value.Split('.')[1]));
                        ushort _pr = (ushort)((Convert.ToByte(nodeChannel.Attributes["broadcast"].Value.Split('.')[2]) << 8) | Convert.ToByte(nodeChannel.Attributes["broadcast"].Value.Split('.')[3]));
                        string _name = nodeChannel.Attributes["name"].Value;

                        ushort _lci;
                        ushort.TryParse(nodeChannel.Attributes["lci"].Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out _lci);

                        ushort _timeout = Convert.ToUInt16(nodeChannel.Attributes["timeout"].Value);

                        if (nodeChannel.ChildNodes.Count > 0)
                        {
                            if (nodeChannel.ChildNodes[0].Name == "message")
                            {
                                //Message Channel
                                MessageChannel msgchn = new MessageChannel(_pv, _pr, _name, _lci, _timeout, nodeChannel.ChildNodes[0].InnerText);

                                TreeNode msgnode = new TreeNode(msgchn.name + " (" + msgchn.GetChannelNumberString() + ")");
                                msgnode.Tag = msgchn;
                                msgnode.ContextMenuStrip = mainWindow.contextMenuStripChannelMenu;

                                nodelist.Add(msgnode);
                            }
                            else if (nodeChannel.ChildNodes[0].Name == "town")
                            {
                                //Town Status
                                if (!(new Regex(@"^[0-3]$").Match(nodeChannel.ChildNodes[0].Attributes["apu"].Value).Success))
                                {
                                    //apu
                                    MessageBox.Show("APU Setup is invalid in town channel " + nodeChannel.BaseURI + " (" + nodeChannel.Attributes["name"].Value + ")", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                if (!(new Regex(@"^[0-3]$").Match(nodeChannel.ChildNodes[0].Attributes["radio"].Value).Success))
                                {
                                    //radio
                                    MessageBox.Show("Radio Setup is invalid in town channel " + nodeChannel.BaseURI + " (" + nodeChannel.Attributes["name"].Value + ")", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                if (!(new Regex(@"^(?:1[0-2]|[0-9]?)$").Match(nodeChannel.ChildNodes[0].Attributes["fountain"].Value).Success))
                                {
                                    //fountain
                                    MessageBox.Show("Fountain Setup is invalid in town channel " + nodeChannel.BaseURI + " (" + nodeChannel.Attributes["name"].Value + ")", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                if (!(new Regex(@"^[0-4]$").Match(nodeChannel.ChildNodes[0].Attributes["season"].Value).Success))
                                {
                                    //season
                                    MessageBox.Show("Season Setup is invalid in town channel " + nodeChannel.BaseURI + " (" + nodeChannel.Attributes["name"].Value + ")", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                if (!(new Regex(@"^[0-1]{64}$").Match(nodeChannel.ChildNodes[0].Attributes["npc"].Value).Success))
                                {
                                    //npc
                                    MessageBox.Show("NPC Setup is invalid in town channel " + nodeChannel.BaseURI + " (" + nodeChannel.Attributes["name"].Value + ")", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                TownStatus townchn = new TownStatus(_pv, _pr, _name, _lci, _timeout);
                                townchn.apu_setup = Convert.ToByte(nodeChannel.ChildNodes[0].Attributes["apu"].Value);
                                townchn.radio_setup = Convert.ToByte(nodeChannel.ChildNodes[0].Attributes["radio"].Value);
                                townchn.fountain = Convert.ToByte(nodeChannel.ChildNodes[0].Attributes["fountain"].Value);
                                townchn.season = Convert.ToByte(nodeChannel.ChildNodes[0].Attributes["season"].Value);

                                for (int i = 0; i < townchn.npc_flags.Length; i++)
                                {
                                    townchn.npc_flags[i] = (nodeChannel.ChildNodes[0].Attributes["npc"].Value[i] == '1');
                                }

                                TreeNode townnode = new TreeNode(townchn.name + " (" + townchn.GetChannelNumberString() + ")");
                                townnode.Tag = townchn;
                                townnode.ContextMenuStrip = mainWindow.contextMenuStripChannelMenu;

                                nodelist.Add(townnode);
                            }
                            else if (nodeChannel.ChildNodes[0].Name == "directory")
                            {
                                Directory dirchn = new Directory(_pv, _pr, _name, _lci, _timeout);
                                TreeNode dirnode = new TreeNode(dirchn.name + " (" + dirchn.GetChannelNumberString() + ")");
                                dirnode.Tag = dirchn;

                                foreach (XmlNode folderData in nodeChannel.ChildNodes[0].ChildNodes)
                                {
                                    if (folderData.Name == "folder")
                                    {
                                        Folder folder = new Folder(folderData.Attributes["name"].Value, folderData.Attributes["message"].Value, Convert.ToInt32(folderData.Attributes["type"].Value), Convert.ToInt32(folderData.Attributes["purpose"].Value), Convert.ToInt32(folderData.Attributes["id"].Value), Convert.ToInt32(folderData.Attributes["mugshot"].Value));
                                        TreeNode foldernode = new TreeNode(folder.name);
                                        foldernode.Tag = folder;

                                        //File
                                        foreach (XmlNode fileData in folderData.ChildNodes)
                                        {
                                            DownloadFile file = new DownloadFile(folder.purpose == 1, Convert.ToByte(fileData.Attributes["id"].Value));

                                            file.name = fileData.Attributes["name"].Value;
                                            file.service_broadcast = (ushort)((Convert.ToByte(fileData.Attributes["broadcast"].Value.Split('.')[0]) << 8) | Convert.ToByte(fileData.Attributes["broadcast"].Value.Split('.')[1]));
                                            file.program_number = (ushort)((Convert.ToByte(fileData.Attributes["broadcast"].Value.Split('.')[2]) << 8) | Convert.ToByte(fileData.Attributes["broadcast"].Value.Split('.')[3]));
                                            ushort.TryParse(fileData.Attributes["lci"].Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out file.lci);
                                            file.timeout = Convert.ToUInt16(fileData.Attributes["timeout"].Value);

                                            file.filedesc = fileData.Attributes["description"].Value;

                                            file.usage = fileData.Attributes["usage"].Value;
                                            file.price = Convert.ToUInt64(fileData.Attributes["price"].Value);
                                            file.oneuse = Convert.ToBoolean(fileData.Attributes["oneuse"].Value);

                                            file.filepath = fileData.Attributes["path"].Value;
                                            file.autostart = Convert.ToByte(fileData.Attributes["autostart"].Value);
                                            file.dest = Convert.ToByte(fileData.Attributes["destination"].Value);
                                            file.alsoAtHome = Convert.ToBoolean(fileData.Attributes["home"].Value);
                                            file.streamed = Convert.ToBoolean(fileData.Attributes["streamed"].Value);
                                            file.month = Convert.ToByte(fileData.Attributes["month"].Value);
                                            file.day = Convert.ToByte(fileData.Attributes["day"].Value);
                                            file.hour_start = Convert.ToByte(fileData.Attributes["starttime"].Value.Split(':')[0]);
                                            file.min_start = Convert.ToByte(fileData.Attributes["starttime"].Value.Split(':')[1]);
                                            file.hour_end = Convert.ToByte(fileData.Attributes["endtime"].Value.Split(':')[0]);
                                            file.min_end = Convert.ToByte(fileData.Attributes["endtime"].Value.Split(':')[1]);

                                            TreeNode filenode = new TreeNode(file.name);
                                            filenode.Tag = file;

                                            foreach (XmlNode fileInclData in fileData.ChildNodes)
                                            {
                                                //Include Files
                                                DownloadFile fileIncl = new DownloadFile(folder.purpose == 1, Convert.ToByte(fileInclData.Attributes["id"].Value));

                                                fileIncl.name = fileInclData.Attributes["name"].Value;
                                                fileIncl.service_broadcast = (ushort)((Convert.ToByte(fileInclData.Attributes["broadcast"].Value.Split('.')[0]) << 8) | Convert.ToByte(fileInclData.Attributes["broadcast"].Value.Split('.')[1]));
                                                fileIncl.program_number = (ushort)((Convert.ToByte(fileInclData.Attributes["broadcast"].Value.Split('.')[2]) << 8) | Convert.ToByte(fileInclData.Attributes["broadcast"].Value.Split('.')[3]));
                                                ushort.TryParse(fileInclData.Attributes["lci"].Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out fileIncl.lci);
                                                fileIncl.timeout = Convert.ToUInt16(fileInclData.Attributes["timeout"].Value);

                                                fileIncl.filedesc = fileInclData.Attributes["description"].Value;

                                                fileIncl.usage = fileInclData.Attributes["usage"].Value;
                                                fileIncl.price = Convert.ToUInt64(fileInclData.Attributes["price"].Value);
                                                fileIncl.oneuse = Convert.ToBoolean(fileInclData.Attributes["oneuse"].Value);

                                                if (fileInclData.Attributes["path"].Value.Length > 0 && fileInclData.Attributes["path"].Value[0] == '.')
                                                    fileIncl.filepath = Path.GetFullPath(Path.GetDirectoryName(xmlPath) + fileInclData.Attributes["path"].Value);
                                                else
                                                    fileIncl.filepath = fileInclData.Attributes["path"].Value;
                                                fileIncl.autostart = Convert.ToByte(fileInclData.Attributes["autostart"].Value);
                                                fileIncl.dest = Convert.ToByte(fileInclData.Attributes["destination"].Value);
                                                fileIncl.alsoAtHome = Convert.ToBoolean(fileInclData.Attributes["home"].Value);
                                                fileIncl.streamed = Convert.ToBoolean(fileInclData.Attributes["streamed"].Value);
                                                fileIncl.month = Convert.ToByte(fileInclData.Attributes["month"].Value);
                                                fileIncl.day = Convert.ToByte(fileInclData.Attributes["day"].Value);
                                                fileIncl.hour_start = Convert.ToByte(fileInclData.Attributes["starttime"].Value.Split(':')[0]);
                                                fileIncl.min_start = Convert.ToByte(fileInclData.Attributes["starttime"].Value.Split(':')[1]);
                                                fileIncl.hour_end = Convert.ToByte(fileInclData.Attributes["endtime"].Value.Split(':')[0]);
                                                fileIncl.min_end = Convert.ToByte(fileInclData.Attributes["endtime"].Value.Split(':')[1]);

                                                TreeNode fileinclnode = new TreeNode(fileIncl.name);
                                                fileinclnode.Tag = fileIncl;
                                                fileinclnode.ContextMenuStrip = mainWindow.contextMenuStripFileMenu;
                                                filenode.Nodes.Add(fileinclnode);
                                            }
                                            filenode.ContextMenuStrip = mainWindow.contextMenuStripFileMenu;
                                            foldernode.Nodes.Add(filenode);
                                        }
                                        foldernode.ContextMenuStrip = mainWindow.contextMenuStripFolderMenu;
                                        dirnode.Nodes.Add(foldernode);
                                    }
                                }
                                dirnode.ContextMenuStrip = mainWindow.contextMenuStripDirectoryMenu;
                                nodelist.Add(dirnode);
                            }
                        }
                    }
                    else
                    {
                        //error
                        MessageBox.Show("Invalid Channel Node.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            else
            {
                //not proper XML
                MessageBox.Show("This is not a SatellaWave XML file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //add to window
            mainWindow.treeViewChn.Nodes.Clear();
            foreach (TreeNode _node in nodelist)
            {
                Console.WriteLine(_node.Name);
                mainWindow.treeViewChn.Nodes.Add(_node);
            }

            lastSavedXMLFile = xmlPath;
        }

        public static void SaveBSXRepository()
        {
            //Fast save
            SaveBSXRepository(lastSavedXMLFile);
        }

        public static void SaveBSXRepository(string filepath)
        {
            //Save XML file

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t"
            };

            XmlWriter xmlWriter = XmlWriter.Create(filepath, xmlWriterSettings);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("bsx");
            xmlWriter.WriteAttributeString("version", "0.1");

            foreach (TreeNode _node in mainWindow.treeViewChn.Nodes)
            {
                //Channel
                xmlWriter.WriteStartElement("channel");
                xmlWriter.WriteAttributeString("name", (_node.Tag as Channel).name);
                xmlWriter.WriteAttributeString("broadcast", (_node.Tag as Channel).GetChannelNumberString());
                xmlWriter.WriteAttributeString("lci", (_node.Tag as Channel).lci.ToString("X4"));
                xmlWriter.WriteAttributeString("timeout", (_node.Tag as Channel).timeout.ToString());
                xmlWriter.WriteAttributeString("type", (_node.Tag as Channel).type.ToString());

                //Channel Specific Type
                if (_node.Tag.GetType() == typeof(MessageChannel))
                {
                    //Message
                    xmlWriter.WriteStartElement("message");
                    xmlWriter.WriteString((_node.Tag as MessageChannel).message);
                    xmlWriter.WriteEndElement();
                }
                else if (_node.Tag.GetType() == typeof(TownStatus))
                {
                    //Town Status
                    xmlWriter.WriteStartElement("town");
                    xmlWriter.WriteAttributeString("apu", (_node.Tag as TownStatus).apu_setup.ToString());
                    xmlWriter.WriteAttributeString("radio", (_node.Tag as TownStatus).radio_setup.ToString());

                    string npc_list = "";
                    foreach (bool _check in (_node.Tag as TownStatus).npc_flags)
                    {
                        if (_check == true)
                            npc_list += "1";
                        else
                            npc_list += "0";
                    }

                    xmlWriter.WriteAttributeString("npc", npc_list);

                    xmlWriter.WriteAttributeString("fountain", (_node.Tag as TownStatus).fountain.ToString());
                    xmlWriter.WriteAttributeString("season", (_node.Tag as TownStatus).season.ToString());
                    xmlWriter.WriteEndElement();
                }
                else if (_node.Tag.GetType() == typeof(Directory))
                {
                    //Directory
                    xmlWriter.WriteStartElement("directory");

                    //Folders
                    foreach (TreeNode _foldernode in _node.Nodes)
                    {
                        if (_foldernode.Tag.GetType() == typeof(Folder))
                        {
                            //Making sure it's a folder, for future Expansion support
                            xmlWriter.WriteStartElement("folder");

                            xmlWriter.WriteAttributeString("name", (_foldernode.Tag as Folder).name);
                            xmlWriter.WriteAttributeString("message", (_foldernode.Tag as Folder).message);
                            xmlWriter.WriteAttributeString("purpose", (_foldernode.Tag as Folder).purpose.ToString());
                            xmlWriter.WriteAttributeString("type", (_foldernode.Tag as Folder).type.ToString());
                            xmlWriter.WriteAttributeString("id", (_foldernode.Tag as Folder).id.ToString());
                            xmlWriter.WriteAttributeString("mugshot", (_foldernode.Tag as Folder).mugshot.ToString());

                            //Files
                            foreach (TreeNode _filenode in _foldernode.Nodes)
                            {
                                xmlWriter.WriteStartElement("file");

                                xmlWriter.WriteAttributeString("name", (_filenode.Tag as DownloadFile).name);
                                xmlWriter.WriteAttributeString("id", (_filenode.Tag as DownloadFile).fileID.ToString());
                                xmlWriter.WriteAttributeString("broadcast", (_filenode.Tag as DownloadFile).GetChannelNumberString());
                                xmlWriter.WriteAttributeString("lci", (_filenode.Tag as DownloadFile).lci.ToString("X4"));
                                xmlWriter.WriteAttributeString("timeout", (_filenode.Tag as DownloadFile).timeout.ToString());
                                xmlWriter.WriteAttributeString("type", (_filenode.Tag as DownloadFile).type.ToString());
                                xmlWriter.WriteAttributeString("description", (_filenode.Tag as DownloadFile).filedesc);

                                //Item
                                xmlWriter.WriteAttributeString("usage", (_filenode.Tag as DownloadFile).usage);
                                xmlWriter.WriteAttributeString("price", (_filenode.Tag as DownloadFile).price.ToString("D12"));
                                xmlWriter.WriteAttributeString("oneuse", (_filenode.Tag as DownloadFile).oneuse.ToString().ToLowerInvariant());

                                //File
                                xmlWriter.WriteAttributeString("path", (_filenode.Tag as DownloadFile).filepath);
                                xmlWriter.WriteAttributeString("autostart", (_filenode.Tag as DownloadFile).autostart.ToString());
                                xmlWriter.WriteAttributeString("destination", (_filenode.Tag as DownloadFile).dest.ToString());
                                xmlWriter.WriteAttributeString("home", (_filenode.Tag as DownloadFile).alsoAtHome.ToString().ToLowerInvariant());
                                xmlWriter.WriteAttributeString("streamed", (_filenode.Tag as DownloadFile).streamed.ToString().ToLowerInvariant());

                                xmlWriter.WriteAttributeString("month", (_filenode.Tag as DownloadFile).month.ToString());
                                xmlWriter.WriteAttributeString("day", (_filenode.Tag as DownloadFile).day.ToString());

                                xmlWriter.WriteAttributeString("starttime", (_filenode.Tag as DownloadFile).hour_start.ToString("D2") + ":" + (_filenode.Tag as DownloadFile).min_start.ToString("D2"));
                                xmlWriter.WriteAttributeString("endtime", (_filenode.Tag as DownloadFile).hour_end.ToString("D2") + ":" + (_filenode.Tag as DownloadFile).min_end.ToString("D2"));

                                foreach (TreeNode _fileinclnode in _filenode.Nodes)
                                {
                                    //INCLUDE FILES (can only be files)
                                    xmlWriter.WriteStartElement("file");

                                    xmlWriter.WriteAttributeString("name", (_fileinclnode.Tag as DownloadFile).name);
                                    xmlWriter.WriteAttributeString("id", (_fileinclnode.Tag as DownloadFile).fileID.ToString());
                                    xmlWriter.WriteAttributeString("broadcast", (_fileinclnode.Tag as DownloadFile).GetChannelNumberString());
                                    xmlWriter.WriteAttributeString("lci", (_fileinclnode.Tag as DownloadFile).lci.ToString("X4"));
                                    xmlWriter.WriteAttributeString("timeout", (_fileinclnode.Tag as DownloadFile).timeout.ToString());
                                    xmlWriter.WriteAttributeString("type", (_fileinclnode.Tag as DownloadFile).type.ToString());
                                    xmlWriter.WriteAttributeString("description", (_fileinclnode.Tag as DownloadFile).filedesc);

                                    //Item (for compatibility)
                                    xmlWriter.WriteAttributeString("usage", (_filenode.Tag as DownloadFile).usage);
                                    xmlWriter.WriteAttributeString("price", (_filenode.Tag as DownloadFile).price.ToString("D12"));
                                    xmlWriter.WriteAttributeString("oneuse", (_filenode.Tag as DownloadFile).oneuse.ToString().ToLowerInvariant());

                                    //File
                                    xmlWriter.WriteAttributeString("path", (_fileinclnode.Tag as DownloadFile).filepath);
                                    xmlWriter.WriteAttributeString("autostart", (_fileinclnode.Tag as DownloadFile).autostart.ToString());
                                    xmlWriter.WriteAttributeString("destination", (_fileinclnode.Tag as DownloadFile).dest.ToString());
                                    xmlWriter.WriteAttributeString("home", (_fileinclnode.Tag as DownloadFile).alsoAtHome.ToString().ToLowerInvariant());
                                    xmlWriter.WriteAttributeString("streamed", (_fileinclnode.Tag as DownloadFile).streamed.ToString().ToLowerInvariant());

                                    xmlWriter.WriteAttributeString("month", (_fileinclnode.Tag as DownloadFile).month.ToString());
                                    xmlWriter.WriteAttributeString("day", (_fileinclnode.Tag as DownloadFile).day.ToString());

                                    xmlWriter.WriteAttributeString("starttime", (_fileinclnode.Tag as DownloadFile).GetTimeStart());
                                    xmlWriter.WriteAttributeString("endtime", (_fileinclnode.Tag as DownloadFile).GetTimeEnd());

                                    xmlWriter.WriteEndElement();
                                }

                                xmlWriter.WriteEndElement();
                            }

                            xmlWriter.WriteEndElement();
                        }
                    }

                    xmlWriter.WriteEndElement();
                }

                //End Channel
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndDocument();
            xmlWriter.Close();

            lastSavedXMLFile = filepath;
        }

        public static void ExportBSX(string folderPath)
        {
            //2794 bytes max per file
            List<byte> ChannelFile = new List<byte>();
            List<byte> FileIDs = new List<byte>();

            //Find, make the Directory First
            foreach (TreeNode _DirectoryCheck in mainWindow.treeViewChn.Nodes)
            {
                if (_DirectoryCheck.Tag.GetType() == typeof(Directory))
                {
                    ChannelFile.Clear(); //Clear everything, just to make sure

                    bool checkInclude = false;
                    int amountIncludeFiles = 0;
                    foreach (TreeNode _Folder in _DirectoryCheck.Nodes)
                    {
                        foreach (TreeNode _File in _Folder.Nodes)
                        {
                            checkInclude |= (_File.Nodes.Count > 0);
                            amountIncludeFiles += _File.Nodes.Count;
                        }
                    }

                    //Directory Header
                    ChannelFile.Add(1); //Directory ID
                    ChannelFile.Add((byte)(_DirectoryCheck.Nodes.Count + Convert.ToByte(checkInclude))); //Folder Count
                    ChannelFile.Add(0); //Unknown
                    ChannelFile.Add(0);
                    ChannelFile.Add(0);

                    //Folders
                    foreach (TreeNode _Folder in _DirectoryCheck.Nodes)
                    {
                        if (_Folder.Tag.GetType() == typeof(Folder))
                        {
                            ChannelFile.Add(0); //Folder Flags

                            //Count Files (including Include Files)
                            int fileCount = _Folder.Nodes.Count;

                            if (fileCount >= 256)
                            {
                                MessageBox.Show("Error: Folder has more than 255 files.");
                                return;
                            }

                            ChannelFile.Add((byte)fileCount); //File Count

                            //Folder Name
                            for (int i = 0; i < 20; i++)
                            {
                                if (ConvertToBSXStringBytes((_Folder.Tag as Folder).name).Length > i)
                                    ChannelFile.Add(ConvertToBSXStringBytes((_Folder.Tag as Folder).name)[i]);
                                else
                                    ChannelFile.Add(0);
                            }

                            ChannelFile.Add(0); //Name Terminator

                            ChannelFile.Add((byte)(ConvertToBSXStringBytes((_Folder.Tag as Folder).message).Length + 1)); //Length Message
                            foreach (byte _chr in ConvertToBSXStringBytes((_Folder.Tag as Folder).message))
                            {
                                ChannelFile.Add(_chr);
                            }
                            ChannelFile.Add(0); //Message Terminator

                            ChannelFile.Add((byte)((_Folder.Tag as Folder).purpose
                                | ((_Folder.Tag as Folder).type << 1))); //Folder Type

                            ChannelFile.Add((byte)((_Folder.Tag as Folder).id));    //Folder ID
                            ChannelFile.Add(0); //Unknown
                            ChannelFile.Add(0); //Unknown
                            ChannelFile.Add((byte)((_Folder.Tag as Folder).mugshot));    //Folder Mugshot
                            ChannelFile.Add(0); //Unknown
                            ChannelFile.Add(0); //Unknown
                            ChannelFile.Add(0); //Unknown

                            //Files
                            foreach (TreeNode _File in _Folder.Nodes)
                            {
                                ChannelFile.Add((_File.Tag as DownloadFile).fileID); //File ID
                                ChannelFile.Add(0); //Check

                                //File Name
                                for (int i = 0; i < 20; i++)
                                {
                                    if (ConvertToBSXStringBytes((_File.Tag as DownloadFile).name).Length > i)
                                        ChannelFile.Add(ConvertToBSXStringBytes((_File.Tag as DownloadFile).name)[i]);
                                    else
                                        ChannelFile.Add(0);
                                }
                                ChannelFile.Add(0);

                                if ((_File.Tag as DownloadFile).isItem)
                                {
                                    //Item
                                    ChannelFile.Add(0x79); //Item Size

                                    //Description
                                    for (int i = 0; i < 36; i++)
                                    {
                                        if (ConvertToBSXStringBytes((_File.Tag as DownloadFile).filedesc).Length > i)
                                            ChannelFile.Add(ConvertToBSXStringBytes((_File.Tag as DownloadFile).filedesc)[i]);
                                        else
                                            ChannelFile.Add(0);
                                    }
                                    ChannelFile.Add(0);

                                    //Activation Message
                                    for (int i = 0; i < 70; i++)
                                    {
                                        if (ConvertToBSXStringBytes((_File.Tag as DownloadFile).usage).Length > i)
                                            ChannelFile.Add(ConvertToBSXStringBytes((_File.Tag as DownloadFile).usage)[i]);
                                        else
                                            ChannelFile.Add(0);
                                    }
                                    ChannelFile.Add(0);

                                    //Price
                                    foreach (char _chr in (_File.Tag as DownloadFile).price.ToString("D12"))
                                    {
                                        ChannelFile.Add((byte)_chr);
                                    }

                                    //One Use
                                    ChannelFile.Add(Convert.ToByte(!(_File.Tag as DownloadFile).oneuse));

                                    //Useless Bytes
                                    for (int i = 0; i < 26; i++)
                                        ChannelFile.Add(0);
                                }
                                else
                                {
                                    //Export Download File First
                                    FileIDs.Add((_File.Tag as DownloadFile).fileID);

                                    FileStream downloadFile = new FileStream((_File.Tag as DownloadFile).filepath, FileMode.Open);
                                    (_File.Tag as DownloadFile).filesize = (int)downloadFile.Length;

                                    byte[] downloadFileArray = new byte[(_File.Tag as DownloadFile).filesize];
                                    downloadFile.Read(downloadFileArray, 0, (int)downloadFile.Length);

                                    downloadFile.Close();

                                    SaveChannelFile(downloadFileArray, (_File.Tag as DownloadFile).lci, folderPath);

                                    //File
                                    ChannelFile.Add((byte)(ConvertToBSXStringBytes((_File.Tag as DownloadFile).filedesc).Length + 1)); //Description Length

                                    //Description
                                    foreach (byte _chr in ConvertToBSXStringBytes((_File.Tag as DownloadFile).filedesc))
                                    {
                                        ChannelFile.Add(_chr);
                                    }
                                    ChannelFile.Add(0);

                                    //Service Broadcast
                                    ChannelFile.Add((byte)((_File.Tag as DownloadFile).service_broadcast >> 8));
                                    ChannelFile.Add((byte)(_File.Tag as DownloadFile).service_broadcast);
                                    //Program Number
                                    ChannelFile.Add((byte)((_File.Tag as DownloadFile).program_number >> 8));
                                    ChannelFile.Add((byte)(_File.Tag as DownloadFile).program_number);
                                    //File Size
                                    ChannelFile.Add((byte)((_File.Tag as DownloadFile).filesize >> 16));
                                    ChannelFile.Add((byte)((_File.Tag as DownloadFile).filesize >> 8));
                                    ChannelFile.Add((byte)((_File.Tag as DownloadFile).filesize));
                                    //Unknown
                                    ChannelFile.Add(0);
                                    ChannelFile.Add(0);
                                    ChannelFile.Add(0);
                                    //Flags
                                    ChannelFile.Add((byte)(Convert.ToByte(!(_File.Tag as DownloadFile).alsoAtHome) << 2
                                        | Convert.ToByte((_File.Tag as DownloadFile).streamed) << 3));
                                    //Unknown
                                    ChannelFile.Add(0);
                                    //Destination
                                    ChannelFile.Add((byte)((_File.Tag as DownloadFile).autostart
                                        | ((_File.Tag as DownloadFile).dest << 2)));
                                    //Unknown
                                    ChannelFile.Add(0);
                                    ChannelFile.Add(0);
                                    //Date
                                    ChannelFile.Add((byte)((_File.Tag as DownloadFile).month << 4));
                                    ChannelFile.Add((byte)((_File.Tag as DownloadFile).day << 3));
                                    //Time
                                    ChannelFile.Add((byte)(((_File.Tag as DownloadFile).hour_start << 3)
                                        | ((_File.Tag as DownloadFile).min_start >> 3)));
                                    ChannelFile.Add((byte)(((_File.Tag as DownloadFile).min_start << 5)
                                        | (_File.Tag as DownloadFile).hour_end));
                                    ChannelFile.Add((byte)((_File.Tag as DownloadFile).min_end << 2));

                                    if (_File.Nodes.Count > 0)
                                    {
                                        //Include Files

                                        //Service Broadcast
                                        ChannelFile.Add((byte)((_File.Nodes[0].Tag as DownloadFile).service_broadcast >> 8));
                                        ChannelFile.Add((byte)(_File.Nodes[0].Tag as DownloadFile).service_broadcast);
                                        //Program Number
                                        ChannelFile.Add((byte)((_File.Nodes[0].Tag as DownloadFile).program_number >> 8));
                                        ChannelFile.Add((byte)(_File.Nodes[0].Tag as DownloadFile).program_number);
                                    }
                                    else
                                    {
                                        ChannelFile.Add(0);
                                        ChannelFile.Add(0);
                                        ChannelFile.Add(0);
                                        ChannelFile.Add(0);
                                    }
                                    //Unknown
                                    ChannelFile.Add(0);
                                    ChannelFile.Add(0);
                                }
                            }
                        }
                    }

                    //Include File Folder
                    if (checkInclude)
                    {
                        ChannelFile.Add(0); //Folder Flag
                        ChannelFile.Add((byte)amountIncludeFiles);
                        string IncludeFolderName = "Include";

                        for (int i = 0; i < 20; i++)
                        {
                            if (IncludeFolderName.Length > i)
                                ChannelFile.Add((byte)IncludeFolderName[i]);
                            else
                                ChannelFile.Add(0);
                        }
                        ChannelFile.Add(0);

                        string IncludeFolderDesc = "Include Files";
                        ChannelFile.Add((byte)(IncludeFolderDesc.Length + 1)); //Length Message
                        foreach (char _chr in IncludeFolderDesc)
                        {
                            ChannelFile.Add((byte)_chr);
                        }
                        ChannelFile.Add(0); //Message Terminator

                        ChannelFile.Add(0x04); //Folder Purpose & Type: Indoors, Include Files Folder
                        ChannelFile.Add(0x15); //Folder ID: Unused

                        ChannelFile.Add(0); //Unused
                        ChannelFile.Add(0); //Unused
                        ChannelFile.Add(0x10); //BS-X Logo (unseen anyway)
                        ChannelFile.Add(0); //Unused
                        ChannelFile.Add(0); //Unused
                        ChannelFile.Add(0); //Unused

                        foreach (TreeNode _Folder in _DirectoryCheck.Nodes)
                        {
                            foreach (TreeNode _File in _Folder.Nodes)
                            {
                                //Include Files
                                if (_File.Nodes.Count > 0)
                                {
                                    for (int inclCount = 0; inclCount < _File.Nodes.Count; inclCount++)
                                    {
                                        FileStream downloadInclFile = new FileStream((_File.Nodes[inclCount].Tag as DownloadFile).filepath, FileMode.Open);
                                        (_File.Nodes[inclCount].Tag as DownloadFile).filesize = (int)downloadInclFile.Length;

                                        byte[] downloadInclFileArray = new byte[(_File.Nodes[inclCount].Tag as DownloadFile).filesize];
                                        downloadInclFile.Read(downloadInclFileArray, 0, (int)downloadInclFile.Length);

                                        downloadInclFile.Close();

                                        SaveChannelFile(downloadInclFileArray, (_File.Nodes[inclCount].Tag as DownloadFile).lci, folderPath);

                                        ChannelFile.Add(1); //File ID
                                        ChannelFile.Add(0); //Check

                                        //File Name
                                        for (int i = 0; i < 20; i++)
                                        {
                                            if (ConvertToBSXStringBytes((_File.Nodes[inclCount].Tag as DownloadFile).name).Length > i)
                                                ChannelFile.Add(ConvertToBSXStringBytes((_File.Nodes[inclCount].Tag as DownloadFile).name)[i]);
                                            else
                                                ChannelFile.Add(0);
                                        }
                                        ChannelFile.Add(0);

                                        //File
                                        ChannelFile.Add((byte)((_File.Nodes[inclCount].Tag as DownloadFile).filedesc.Length + 1)); //Description Length

                                        //Description
                                        foreach (byte _chr in ConvertToBSXStringBytes((_File.Nodes[inclCount].Tag as DownloadFile).filedesc))
                                        {
                                            ChannelFile.Add(_chr);
                                        }
                                        ChannelFile.Add(0);

                                        //Service Broadcast
                                        ChannelFile.Add((byte)((_File.Nodes[inclCount].Tag as DownloadFile).service_broadcast >> 8));
                                        ChannelFile.Add((byte)(_File.Nodes[inclCount].Tag as DownloadFile).service_broadcast);
                                        //Program Number
                                        ChannelFile.Add((byte)((_File.Nodes[inclCount].Tag as DownloadFile).program_number >> 8));
                                        ChannelFile.Add((byte)(_File.Nodes[inclCount].Tag as DownloadFile).program_number);
                                        //File Size
                                        ChannelFile.Add((byte)((_File.Nodes[inclCount].Tag as DownloadFile).filesize >> 16));
                                        ChannelFile.Add((byte)((_File.Nodes[inclCount].Tag as DownloadFile).filesize >> 8));
                                        ChannelFile.Add((byte)((_File.Nodes[inclCount].Tag as DownloadFile).filesize));
                                        //Unknown
                                        ChannelFile.Add(0);
                                        ChannelFile.Add(0);
                                        ChannelFile.Add(0);
                                        //Flags
                                        ChannelFile.Add((byte)(Convert.ToByte(!(_File.Nodes[inclCount].Tag as DownloadFile).alsoAtHome) << 2
                                            | Convert.ToByte((_File.Nodes[inclCount].Tag as DownloadFile).streamed) << 3));
                                        //Unknown
                                        ChannelFile.Add(0);
                                        //Destination
                                        ChannelFile.Add((byte)((_File.Nodes[inclCount].Tag as DownloadFile).autostart
                                            | ((_File.Nodes[inclCount].Tag as DownloadFile).dest << 2)));
                                        //Unknown
                                        ChannelFile.Add(0);
                                        ChannelFile.Add(0);
                                        //Date
                                        ChannelFile.Add((byte)((_File.Nodes[inclCount].Tag as DownloadFile).month << 4));
                                        ChannelFile.Add((byte)((_File.Nodes[inclCount].Tag as DownloadFile).day << 3));
                                        //Time
                                        ChannelFile.Add((byte)(((_File.Nodes[inclCount].Tag as DownloadFile).hour_start << 3)
                                            | ((_File.Nodes[inclCount].Tag as DownloadFile).min_start >> 3)));
                                        ChannelFile.Add((byte)(((_File.Nodes[inclCount].Tag as DownloadFile).min_start << 5)
                                            | (_File.Nodes[inclCount].Tag as DownloadFile).hour_end));
                                        ChannelFile.Add((byte)((_File.Nodes[inclCount].Tag as DownloadFile).min_end << 2));

                                        if ((_File.Nodes.Count - 1) > inclCount)
                                        {
                                            //Service Broadcast
                                            ChannelFile.Add((byte)((_File.Nodes[inclCount + 1].Tag as DownloadFile).service_broadcast >> 8));
                                            ChannelFile.Add((byte)(_File.Nodes[inclCount + 1].Tag as DownloadFile).service_broadcast);
                                            //Program Number
                                            ChannelFile.Add((byte)((_File.Nodes[inclCount + 1].Tag as DownloadFile).program_number >> 8));
                                            ChannelFile.Add((byte)(_File.Nodes[inclCount + 1].Tag as DownloadFile).program_number);
                                        }
                                        else
                                        {
                                            ChannelFile.Add(0);
                                            ChannelFile.Add(0);
                                            ChannelFile.Add(0);
                                            ChannelFile.Add(0);
                                        }

                                        ChannelFile.Add(0);
                                        ChannelFile.Add(0);
                                    }
                                }
                            }
                        }
                    }

                    //Expansion Packet (TODO)
                    ChannelFile.Add(0); //No Expansion Packet (yet)

                    //Write File
                    SaveChannelFile(ChannelFile.ToArray(), (_DirectoryCheck.Tag as Directory).lci, folderPath);
                }
            }

            foreach (TreeNode _Channel in mainWindow.treeViewChn.Nodes)
            {
                if (_Channel.Tag.GetType() == typeof(MessageChannel))
                {
                    //Message
                    ChannelFile.Clear();

                    foreach (byte _chr in ConvertToBSXStringBytes((_Channel.Tag as MessageChannel).message))
                    {
                        ChannelFile.Add(_chr);
                    }
                    ChannelFile.Add(0);

                    SaveChannelFile(ChannelFile.ToArray(), (_Channel.Tag as MessageChannel).lci, folderPath);
                }
                else if (_Channel.Tag.GetType() == typeof(TownStatus))
                {
                    //Town Status
                    ChannelFile.Clear();

                    ChannelFile.Add(0); //Flag
                    ChannelFile.Add(1); //Town Status ID
                    ChannelFile.Add(1); //Directory ID

                    ChannelFile.Add(0);
                    ChannelFile.Add(0);
                    ChannelFile.Add(0);
                    ChannelFile.Add(0);

                    ChannelFile.Add((byte)(((_Channel.Tag as TownStatus).radio_setup << 6) | ((_Channel.Tag as TownStatus).apu_setup << 4)));
                    ChannelFile.Add(0);

                    //NPC/Event Flags
                    for (int x = 0; x < 8; x++)
                    {
                        byte _flag = 0;
                        for (int y = 0; y < 8; y++)
                        {
                            if ((_Channel.Tag as TownStatus).npc_flags[x * 8 + y] == true)
                                _flag |= (byte)(1 << y);
                        }
                        ChannelFile.Add(_flag);
                    }

                    ushort townsetup = 0;
                    townsetup = (ushort)(1 << ((_Channel.Tag as TownStatus).fountain - 1));
                    townsetup |= (ushort)((1 << ((_Channel.Tag as TownStatus).season - 1) << 12));

                    ChannelFile.Add((byte)townsetup);
                    ChannelFile.Add((byte)(townsetup >> 8));

                    ChannelFile.Add(0);
                    ChannelFile.Add(0);
                    ChannelFile.Add(0);
                    ChannelFile.Add(0);

                    ChannelFile.Add((byte)FileIDs.Count); //Number of file IDs
                    foreach (byte _id in FileIDs)
                        ChannelFile.Add(_id);

                    if (ChannelFile.Count > 256)
                    {
                        MessageBox.Show("Error: Town Status is more than 256 bytes.");
                        return;
                    }

                    SaveChannelFile(ChannelFile.ToArray(), (_Channel.Tag as TownStatus).lci, folderPath);
                }
            }

            //Make a Channel List (for easier sorting)
            List<Channel> ChannelList = new List<Channel>();
            foreach (TreeNode _node in mainWindow.treeViewChn.Nodes)
            {
                if (_node.Tag.GetType() == typeof(Directory))
                {
                    foreach (TreeNode _nodeFolder in _node.Nodes)
                    {
                        if (_nodeFolder.Tag.GetType() == typeof(Folder))
                        {
                            foreach (TreeNode _nodeFile in _nodeFolder.Nodes)
                            {
                                if (_nodeFile.Nodes.Count > 0)
                                {
                                    foreach (TreeNode _nodeFileInc in _nodeFile.Nodes)
                                    {
                                        ChannelList.Add(_nodeFileInc.Tag as Channel);
                                    }
                                }

                                ChannelList.Add(_nodeFile.Tag as Channel);
                            }
                        }
                    }
                }

                ChannelList.Add(_node.Tag as Channel);
            }

            //Make the Service List
            List<ushort> ServiceList = new List<ushort>();
            foreach (Channel _chn in ChannelList)
            {
                if (ServiceList.Contains(_chn.service_broadcast) == false)
                {
                    ServiceList.Add(_chn.service_broadcast);
                }
            }

            //Make BSX0124-0.bin, it is the full channel map
            List<byte> ChannelMapFile = new List<byte>();

            //Header part
            ChannelMapFile.Add((byte)'S');
            ChannelMapFile.Add((byte)'F');
            ChannelMapFile.Add(0);
            ChannelMapFile.Add(0);
            ChannelMapFile.Add(0);
            ChannelMapFile.Add(0);
            ChannelMapFile.Add((byte)ServiceList.Count);
            byte chksum = 0;
            foreach (byte _chk in ChannelMapFile)
            {
                chksum += _chk;
            }
            ChannelMapFile.Add(chksum);

            //Service Broadcast List
            foreach (ushort _cur_service in ServiceList)
            {
                ChannelMapFile.Add((byte)(_cur_service >> 8));
                ChannelMapFile.Add((byte)_cur_service);

                //Counter
                byte _count = 0;
                foreach (TreeNode _node in mainWindow.treeViewChn.Nodes)
                {
                    if ((_node.Tag as Channel).service_broadcast == _cur_service)
                    {
                        _count++;
                    }

                    if (_node.Tag.GetType() == typeof(Directory))
                    {
                        foreach (TreeNode _folder in _node.Nodes)
                        {
                            if (_folder.Tag.GetType() == typeof(Folder))
                            {
                                foreach (TreeNode _file in _folder.Nodes)
                                {
                                    if ((_file.Tag as DownloadFile).service_broadcast == _cur_service)
                                    {
                                        _count++;
                                    }

                                    foreach (TreeNode _fileInc in _file.Nodes)
                                    {
                                        if ((_fileInc.Tag as DownloadFile).service_broadcast == _cur_service)
                                        {
                                            _count++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                ChannelMapFile.Add(_count);

                //Program List
                foreach (Channel _chan in ChannelList)
                {
                    if (_chan.service_broadcast == _cur_service)
                    {
                        ChannelMapFile.Add(_chan.type);
                        ChannelMapFile.Add((byte)(_chan.program_number >> 8));
                        ChannelMapFile.Add((byte)_chan.program_number);

                        ChannelMapFile.Add(0);
                        ChannelMapFile.Add(0);
                        ChannelMapFile.Add(0);
                        ChannelMapFile.Add(0);
                        ChannelMapFile.Add(0);

                        ChannelMapFile.Add((byte)(_chan.timeout >> 8));
                        ChannelMapFile.Add((byte)_chan.timeout);


                        if (_chan.GetType() == typeof(DownloadFile))
                        {
                            ChannelMapFile.Add((byte)((_chan as DownloadFile).autostart
                                                    | ((_chan as DownloadFile).dest << 2)));
                        }
                        else
                        {
                            ChannelMapFile.Add(0); //Autostart stuff
                        }

                        ChannelMapFile.Add((byte)_chan.lci);
                        ChannelMapFile.Add((byte)(_chan.lci >> 8));
                    }
                }
            }

            //Finalize Channel Map File
            int size = ChannelMapFile.Count;
            ChannelMapFile.Insert(0, (byte)size);
            ChannelMapFile.Insert(0, (byte)(size >> 8));
            ChannelMapFile.Insert(0, (byte)(size >> 16));
            ChannelMapFile.Insert(0, 0);
            ChannelMapFile.Insert(0, 0);

            FileStream mapfile = new FileStream(folderPath + "\\BSX0124-0.bin", FileMode.Create);
            mapfile.Write(ChannelMapFile.ToArray(), 0, ChannelMapFile.Count);
            mapfile.Close();

            MessageBox.Show("Export succeeded");
        }

        public static void SaveChannelFile(byte[] filedata, ushort lci, string folderPath)
        {
            int fileAmount = (int)Math.Ceiling(filedata.Length / 8192.0);

            for (int i = 0; i < fileAmount; i++)
            {
                FileStream chnfile = new FileStream(folderPath + "\\BSX" + lci.ToString("X4") + "-" + i.ToString() + ".bin", FileMode.Create);

                chnfile.WriteByte(0); //Data Group ID
                chnfile.WriteByte((byte)i); //Data Group Continuity

                if (i == fileAmount - 1)
                {
                    //Data Group Size
                    chnfile.WriteByte((byte)((filedata.Length - (i * 8192) + 5) >> 16));
                    chnfile.WriteByte((byte)((filedata.Length - (i * 8192) + 5) >> 8));
                    chnfile.WriteByte((byte)(filedata.Length - (i * 8192) + 5));

                    chnfile.WriteByte(1); //Fixed
                    chnfile.WriteByte((byte)fileAmount);    //Amount of fragments

                    chnfile.WriteByte((byte)((i * 8192) >> 16));
                    chnfile.WriteByte((byte)((i * 8192) >> 8));
                    chnfile.WriteByte((byte)(i * 8192));

                    chnfile.Write(filedata, (i * 8192), filedata.Length - (i * 8192));
                }
                else
                {
                    int sizetest = 8192 + 5;
                    chnfile.WriteByte((byte)((sizetest) >> 16));
                    chnfile.WriteByte((byte)((sizetest) >> 8));
                    chnfile.WriteByte((byte)(sizetest));

                    chnfile.WriteByte(1); //Fixed
                    chnfile.WriteByte((byte)fileAmount);    //Amount of fragments

                    chnfile.WriteByte((byte)((i * 8192) >> 16));
                    chnfile.WriteByte((byte)((i * 8192) >> 8));
                    chnfile.WriteByte((byte)(i * 8192));

                    chnfile.Write(filedata, (i * 8192), 8192);
                }
                chnfile.Close();
            }
        }

        public static byte[] ConvertToBSXStringBytes(string _string)
        {
            string _convstring = _string.Replace("\r\n", "\r");
            _convstring = _convstring.Replace("\n", "\r");

            return Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(932), Encoding.UTF8.GetBytes(_convstring));
        }
    }
}
