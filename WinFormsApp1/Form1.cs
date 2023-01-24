using Newtonsoft.Json.Linq;
using System.Collections;
using System.Diagnostics;
using System.Net;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            customdesign();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Load(
                "https://media.comicbook.com/2018/10/fortnite-cube-logo-1140141.jpeg");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            flowLayoutPanel1.Controls.Clear();
            Button button = new Button();
            button.Text = "launch";
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.ForeColor = Color.Gainsboro;
            button.Location = new Point(289, 451);
            button.Click += new EventHandler(button_Click);
            flowLayoutPanel1.Controls.Add(button);
            TextBox textBox = new TextBox();
            textBox.Text = "Enter path";
            textBox.Location = new Point(289, 451);
            flowLayoutPanel1.Controls.Add(textBox);
            textBox.TextChanged += new EventHandler(textBox_TextChanged);
        }

        private void customdesign()
        {
            panel3.Visible = false;
        }

        private void hideSubMenu()
        {
            if (panel3.Visible == true)
                panel3.Visible = false;
        }

        private void showSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                hideSubMenu();
                subMenu.Visible = true;
            }
            else
                subMenu.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (panel3.Visible == false)
            {
                panel3.Visible = true;
            }
            else
            {
                panel3.Visible = false;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e) { }

        private void button3_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            Button button = new Button();
            button.Text = "launch";
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.ForeColor = Color.Gainsboro;
            button.Location = new Point(289, 451);
            button.Click += new EventHandler(button_Click);
            flowLayoutPanel1.Controls.Add(button);
            TextBox textBox = new TextBox();
            textBox.Text = "Enter path";
            textBox.Location = new Point(289, 451);
            flowLayoutPanel1.Controls.Add(textBox);
            textBox.TextChanged += new EventHandler(textBox_TextChanged);
        }

        void start()
        {
            server.StartServer();
        }



        private void button_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string path = "D:\\FortniteBuilds\\Fortnite 7.30.0-CL-4834550\\FortniteGame\\Binaries\\Win64\\";
            string BaseArguments = "-epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -skippatchcheck";
            string arguments = $" {BaseArguments}-noeac -fromfl=be -fltoken=5dh74c635862g575778132fb -frombe";

            var clientProcess = new Process
            {
                StartInfo = new ProcessStartInfo(Path.Combine(path, "FortniteClient-Win64-Shipping.exe"), arguments + $"-AUTH_TYPE=epic -AUTH_LOGIN=\"bmorris@bmorris.dev \\\" -AUTH_PASSWORD=\"bmorris\"")
                {
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            var antiCheatProcess = new Process
            {
                StartInfo = new ProcessStartInfo(Path.Combine(path, "FortniteClient-Win64-Shipping_EAC.exe"), arguments)
                {
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            var launcherProcess = new Process
            {
                StartInfo = new ProcessStartInfo(Path.Combine(path, "FortniteLauncher.exe"), arguments)
                {
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            try
            {
                var existingAntiCheatProcess = Process.GetProcessesByName("FortniteClient-Win64-Shipping_EAC")?.FirstOrDefault();
                existingAntiCheatProcess.Kill();
                Thread.Sleep(200);

            }
            catch { }
            clientProcess.Start();
            antiCheatProcess.Start();
            foreach (ProcessThread thread in (ReadOnlyCollectionBase)antiCheatProcess.Threads)
                Win32.SuspendThread(Win32.OpenThread(2, false, thread.Id));
            launcherProcess.Start();
            foreach (ProcessThread thread in (ReadOnlyCollectionBase)launcherProcess.Threads)
                Win32.SuspendThread(Win32.OpenThread(2, false, thread.Id));

            injector.Inject(clientProcess.Id, "Aurora.Runtime.dll");
            start();
            clientProcess.WaitForExit();

            antiCheatProcess.Kill();
            Task.Delay(200);

        }
        string path;
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            path = textBox.Text;

        }
        private void panel3_Paint(object sender, PaintEventArgs e) { }
        int count = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This feture is in beta\nthere may be some bugs.");
            // https://fortnite-api.com/v2/cosmetics/br
            var request = (HttpWebRequest)WebRequest.Create(
                "https://fortnite-api.com/v2/cosmetics/br/new");
            request.Method = "GET";
            var response = (HttpWebResponse)request.GetResponse();
            var responseString =
                new StreamReader(response.GetResponseStream()).ReadToEnd();
            var json = JObject.Parse(responseString);
            var items = json["data"]["items"];

            foreach (var item in items)
            {
                count++;
                var pb = new PictureBox();
                pb.Size = new Size(100, 100);
                pb.Load(item["images"]["icon"].ToString());
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                flowLayoutPanel1.Controls.Add(pb);
                pb.Name = item["id"].ToString();

                pb.Click += new EventHandler(pb_Click);
            }
        }

        private void pb_Click(object sender, EventArgs e)
        {
            if (((PictureBox)sender).Name.Contains("Backpack"))
            {
                string json =
                    $"\"AthenaBackpack:{((PictureBox)sender).Name}\":{{\r\n\"templateId\":\"AthenaBackpack:{((PictureBox)sender).Name}\",\r\n\"attributes\":{{\r\n\"item_seen\":true,\r\n\"variants\": [],\r\n\"favorite\": false\r\n}},\r\n\"quantity\": 1\r\n}},";
                if (System.IO.File.Exists("athena.json"))
                {

                }
                else
                {
                    string d = "{  \"_id\": \"\",  \"Update\": \"profile by tim\",  \"created\": \"2022-01-08T22:58:06.983Z\",  \"updated\": \"2022-01-08T22:58:57.261Z\",  \"rvn\": 24,  \"wipeNumber\": 1,  \"accountId\": \"\",  \"profileId\": \"athena\",  \"version\": \"neonite_2\",  \"items\": {    \"sandbox_loadout\": {      \"templateId\": \"CosmeticLocker:cosmeticlocker_athena\",      \"attributes\": {        \"locker_slots_data\": {          \"slots\": {            \"Pickaxe\": {              \"items\": [                \"AthenaPickaxe:DefaultPickaxe\"              ],              \"activeVariants\": []            },            \"Dance\": {              \"items\": [                \"AthenaDance:EID_BoogieDown\",                \"AthenaDance:EID_DanceMoves\",                \"\",                \"\",                \"\",                \"\"              ]            },            \"Glider\": {              \"items\": [                \"AthenaGlider:DefaultGlider\"              ]            },            \"Character\": {              \"items\": [                \"AthenaCharacter:CID_001_Athena_Commando_F_Default\"              ],              \"activeVariants\": [                {                  \"variants\": []                }              ]            },            \"Backpack\": {              \"items\": [                \"\"              ],              \"activeVariants\": [                {                  \"variants\": []                }              ]            },            \"ItemWrap\": {              \"items\": [                \"\",                \"\",                \"\",                \"\",                \"\",                \"\",                \"\"              ],              \"activeVariants\": [                null,                null,                null,                null,                null,                null,                null              ]            },            \"LoadingScreen\": {              \"items\": [                \"\"              ],              \"activeVariants\": [                null              ]            },            \"MusicPack\": {              \"items\": [                \"\"              ],              \"activeVariants\": [                null              ]            },            \"SkyDiveContrail\": {              \"items\": [                \"\"              ],              \"activeVariants\": [                null              ]            }          }        },        \"use_count\": 1,        \"banner_icon_template\": \"\",        \"locker_name\": \"\",        \"banner_color_template\": \"\",        \"item_seen\": false,        \"favorite\": false      },      \"quantity\": 1    },    \"neoset0_loadout\": {      \"templateId\": \"CosmeticLocker:cosmeticlocker_athena\",      \"attributes\": {        \"locker_slots_data\": {          \"slots\": {            \"Pickaxe\": {              \"items\": [                \"\"              ],              \"activeVariants\": []            },            \"Dance\": {              \"items\": [                \"\",                \"\",                \"\",                \"\",                \"\",                \"\"              ]            },            \"Glider\": {              \"items\": [                \"\"              ]            },            \"Character\": {              \"items\": [                \"\"              ],              \"activeVariants\": [                {                  \"variants\": []                }              ]            },            \"Backpack\": {              \"items\": [                \"\"              ],              \"activeVariants\": [                {                  \"variants\": []                }              ]            },            \"ItemWrap\": {              \"items\": [                \"\",                \"\",                \"\",                \"\",                \"\",                \"\",                \"\"              ],              \"activeVariants\": [                null,                null,                null,                null,                null,                null,                null              ]            },            \"LoadingScreen\": {              \"items\": [                \"\"              ],              \"activeVariants\": [                null              ]            },            \"MusicPack\": {              \"items\": [                \"\"              ],              \"activeVariants\": [                null              ]            },            \"SkyDiveContrail\": {              \"items\": [                \"\"              ],              \"activeVariants\": [                null              ]            }          }        },        \"use_count\": 1,        \"banner_icon_template\": \"\",        \"locker_name\": \"NEOSET\",        \"banner_color_template\": \"\",        \"item_seen\": false,        \"favorite\": false      },      \"quantity\": 1    },    \"AthenaCharacter:CID_001_Athena_Commando_F_Default\": {      \"attributes\": {        \"favorite\": false,        \"item_seen\": true,        \"level\": 0,        \"max_level_bonus\": 0,        \"rnd_sel_cnt\": 0,        \"variants\": [],        \"xp\": 0      },      \"templateId\": \"AthenaCharacter:CID_001_Athena_Commando_F_Default\"    },    \"AthenaCharacter:CID_002_Athena_Commando_F_Default\": {      \"attributes\": {        \"favorite\": false,        \"item_seen\": true,        \"level\": 0,        \"max_level_bonus\": 0,        \"rnd_sel_cnt\": 0,        \"variants\": [],        \"xp\": 0      },      \"templateId\": \"AthenaCharacter:CID_002_Athena_Commando_F_Default\"    },    \"AthenaCharacter:CID_003_Athena_Commando_F_Default\": {      \"attributes\": {        \"favorite\": false,        \"item_seen\": true,        \"level\": 0,        \"max_level_bonus\": 0,        \"rnd_sel_cnt\": 0,        \"variants\": [],        \"xp\": 0      },      \"templateId\": \"AthenaCharacter:CID_003_Athena_Commando_F_Default\"    },    \"AthenaCharacter:CID_004_Athena_Commando_F_Default\": {      \"attributes\": {        \"favorite\": false,        \"item_seen\": true,        \"level\": 0,        \"max_level_bonus\": 0,        \"rnd_sel_cnt\": 0,        \"variants\": [],        \"xp\": 0      },      \"templateId\": \"AthenaCharacter:CID_004_Athena_Commando_F_Default\"    },    \"AthenaCharacter:CID_005_Athena_Commando_M_Default\": {      \"attributes\": {        \"favorite\": false,        \"item_seen\": true,        \"level\": 0,        \"max_level_bonus\": 0,        \"rnd_sel_cnt\": 0,        \"variants\": [],        \"xp\": 0      },      \"templateId\": \"AthenaCharacter:CID_005_Athena_Commando_M_Default\"    },    \"AthenaCharacter:CID_006_Athena_Commando_M_Default\": {      \"attributes\": {        \"favorite\": false,        \"item_seen\": true,        \"level\": 0,        \"max_level_bonus\": 0,        \"rnd_sel_cnt\": 0,        \"variants\": [],        \"xp\": 0      },      \"templateId\": \"AthenaCharacter:CID_006_Athena_Commando_M_Default\"    },    \"AthenaCharacter:CID_007_Athena_Commando_M_Default\": {      \"attributes\": {        \"favorite\": false,        \"item_seen\": true,        \"level\": 0,        \"max_level_bonus\": 0,        \"rnd_sel_cnt\": 0,        \"variants\": [],        \"xp\": 0      },      \"templateId\": \"AthenaCharacter:CID_007_Athena_Commando_M_Default\"    },    \"AthenaCharacter:CID_008_Athena_Commando_M_Default\": {      \"attributes\": {        \"favorite\": false,        \"item_seen\": true,        \"level\": 0,        \"max_level_bonus\": 0,        \"rnd_sel_cnt\": 0,        \"variants\": [],        \"xp\": 0      },      \"templateId\": \"AthenaCharacter:CID_008_Athena_Commando_M_Default\"    },    \"AthenaCharacter:CID_556_Athena_Commando_F_RebirthDefaultA\": {      \"attributes\": {        \"favorite\": false,        \"item_seen\": true,        \"level\": 0,        \"max_level_bonus\": 0,        \"rnd_sel_cnt\": 0,        \"variants\": [],        \"xp\": 0      },      \"templateId\": \"AthenaCharacter:CID_556_Athena_Commando_F_RebirthDefaultA\"    },    \"AthenaCharacter:CID_557_Athena_Commando_F_RebirthDefaultB\": {      \"attributes\": {        \"favorite\": false,        \"item_seen\": true,        \"level\": 0,        \"max_level_bonus\": 0,        \"rnd_sel_cnt\": 0,        \"variants\": [],        \"xp\": 0      },      \"templateId\": \"AthenaCharacter:CID_557_Athena_Commando_F_RebirthDefaultB\"    },    \"AthenaCharacter:CID_558_Athena_Commando_F_RebirthDefaultC\": {      \"attributes\": {        \"favorite\": false,        \"item_seen\": true,        \"level\": 0,        \"max_level_bonus\": 0,        \"rnd_sel_cnt\": 0,        \"variants\": [],        \"xp\": 0      },      \"templateId\": \"AthenaCharacter:CID_558_Athena_Commando_F_RebirthDefaultC\"    },    \"AthenaCharacter:CID_559_Athena_Commando_F_RebirthDefaultD\": {      \"attributes\": {        \"favorite\": false,        \"item_seen\": true,        \"level\": 0,        \"max_level_bonus\": 0,        \"rnd_sel_cnt\": 0,        \"variants\": [],        \"xp\": 0      },      \"templateId\": \"AthenaCharacter:CID_559_Athena_Commando_F_RebirthDefaultD\"    },    \"AthenaCharacter:CID_560_Athena_Commando_M_RebirthDefaultA\": {      \"attributes\": {        \"favorite\": false,        \"item_seen\": true,        \"level\": 0,        \"max_level_bonus\": 0,        \"rnd_sel_cnt\": 0,        \"variants\": [],        \"xp\": 0      },      \"templateId\": \"AthenaCharacter:CID_560_Athena_Commando_M_RebirthDefaultA\"    },    \"AthenaCharacter:CID_561_Athena_Commando_M_RebirthDefaultB\": {      \"attributes\": {        \"favorite\": false,        \"item_seen\": true,        \"level\": 0,        \"max_level_bonus\": 0,        \"rnd_sel_cnt\": 0,        \"variants\": [],        \"xp\": 0      },      \"templateId\": \"AthenaCharacter:CID_561_Athena_Commando_M_RebirthDefaultB\"    },    \"AthenaCharacter:CID_562_Athena_Commando_M_RebirthDefaultC\": {      \"attributes\": {        \"favorite\": false,        \"item_seen\": true,        \"level\": 0,        \"max_level_bonus\": 0,        \"rnd_sel_cnt\": 0,        \"variants\": [],        \"xp\": 0      },      \"templateId\": \"AthenaCharacter:CID_562_Athena_Commando_M_RebirthDefaultC\"    },    \"AthenaCharacter:CID_563_Athena_Commando_M_RebirthDefaultD\": {      \"attributes\": {        \"favorite\": false,        \"item_seen\": true,        \"level\": 0,        \"max_level_bonus\": 0,        \"rnd_sel_cnt\": 0,        \"variants\": [],        \"xp\": 0      },      \"templateId\": \"AthenaCharacter:CID_563_Athena_Commando_M_RebirthDefaultD\"    },    \"AthenaCharacter:cid_a_272_athena_commando_f_prime\": {      \"attributes\": {        \"favorite\": false,        \"item_seen\": true,        \"level\": 0,        \"max_level_bonus\": 0,        \"rnd_sel_cnt\": 0,        \"variants\": [],        \"xp\": 0      },      \"templateId\": \"AthenaCharacter:cid_a_272_athena_commando_f_prime\"    },    \"AthenaPickaxe:DefaultPickaxe\": {      \"attributes\": {        \"favorite\": false,        \"item_seen\": true,        \"level\": 0,        \"max_level_bonus\": 0,        \"rnd_sel_cnt\": 0,        \"variants\": [],        \"xp\": 0      },      \"templateId\": \"AthenaPickaxe:DefaultPickaxe\"    },    \"AthenaGlider:DefaultGlider\": {      \"attributes\": {        \"favorite\": false,        \"item_seen\": true,        \"level\": 0,        \"max_level_bonus\": 0,        \"rnd_sel_cnt\": 0,        \"variants\": [],        \"xp\": 0      },      \"templateId\": \"AthenaGlider:DefaultGlider\"    },    \"AthenaDance:EID_DanceMoves\": {      \"attributes\": {        \"favorite\": false,        \"item_seen\": true,        \"level\": 0,        \"max_level_bonus\": 0,        \"rnd_sel_cnt\": 0,        \"variants\": [],        \"xp\": 0      },      \"templateId\": \"AthenaDance:EID_DanceMoves\"    }, " + json + "   \"AthenaDance:EID_BoogieDown\": {      \"attributes\": {        \"favorite\": false,        \"item_seen\": true,        \"level\": 0,        \"max_level_bonus\": 0,        \"rnd_sel_cnt\": 0,        \"variants\": [],        \"xp\": 0      },      \"templateId\": \"AthenaDance:EID_BoogieDown\"    }  },  \"stats\": {    \"attributes\": {      \"season_match_boost\": 0,      \"loadouts\": [        \"sandbox_loadout\",        \"neoset0_loadout\"      ],      \"rested_xp_overflow\": 0,      \"mfa_reward_claimed\": true,      \"quest_manager\": {},      \"book_level\": 0,      \"season_num\": 16,      \"season_update\": 1,      \"book_xp\": 1,      \"permissions\": [],      \"book_purchased\": false,      \"lifetime_wins\": 0,      \"party_assist_quest\": \"\",      \"purchased_battle_pass_tier_offers\": [],      \"rested_xp_exchange\": 0.333,      \"level\": 0,      \"xp_overflow\": 0,      \"rested_xp\": 0,      \"rested_xp_mult\": 0,      \"accountLevel\": 0,      \"competitive_identity\": {},      \"inventory_limit_bonus\": 0,      \"last_applied_loadout\": \"sandbox_loadout\",      \"daily_rewards\": {},      \"xp\": 0,      \"season_friend_match_boost\": 1,      \"active_loadout_index\": 1,      \"favorite_musicpack\": \"\",      \"favorite_glider\": \"\",      \"favorite_pickaxe\": \"\",      \"favorite_skydivecontrail\": \"\",      \"favorite_backpack\": \"\",      \"favorite_dance\": [        \"\",        \"\",        \"\",        \"\",        \"\",        \"\"      ],      \"favorite_itemwraps\": [],      \"favorite_character\": \"\",      \"favorite_loadingscreen\": \"\"    }  },  \"commandRevision\": 5}";
                    File.WriteAllText("athena.json", d);
                }

            }
            else if (((PictureBox)sender).Name.Contains("Character"))
            {
                string json =
                    $"\"AthenaCharacter::{((PictureBox)sender).Name}\":{{\r\n\"templateId\":\"AthenaCharacter:{((PictureBox)sender).Name}\",\r\n\"attributes\":{{\r\n\"item_seen\":true,\r\n\"variants\": [],\r\n\"favorite\": false\r\n}},\r\n\"quantity\": 1\r\n}},";
                MessageBox.Show(json);
            }
            else if (((PictureBox)sender).Name.Contains("EID"))
            {
                string json =
                    $"\"AthenaDance:{((PictureBox)sender).Name}\":{{\r\n\"templateId\":\"AthenaDance:{((PictureBox)sender).Name}\",\r\n\"attributes\":{{\r\n\"item_seen\":true,\r\n\"variants\": [],\r\n\"favorite\": false\r\n}},\r\n\"quantity\": 1\r\n}},";
                MessageBox.Show(json);
            }
            else if (((PictureBox)sender).Name.Contains("Emoji"))
            {
                string json =
                    $"\"AthenaDance:{((PictureBox)sender).Name}\":{{\r\n\"templateId\":\"AthenaDance:{((PictureBox)sender).Name}\",\r\n\"attributes\":{{\r\n\"item_seen\":true,\r\n\"variants\": [],\r\n\"favorite\": false\r\n}},\r\n\"quantity\": 1\r\n}},";
                MessageBox.Show(json);
            }
            else if (((PictureBox)sender).Name.Contains("Glider"))
            {
                string json =
                    $"\"AthenaGlider:{((PictureBox)sender).Name}\":{{\r\n\"templateId\":\"AthenaGlider:{((PictureBox)sender).Name}\",\r\n\"attributes\":{{\r\n\"item_seen\":true,\r\n\"variants\": [],\r\n\"favorite\": false\r\n}},\r\n\"quantity\": 1\r\n}},";
                MessageBox.Show(json);
            }
            else if (((PictureBox)sender).Name.Contains("LSID"))
            {
                string json =
                    $"\"AthenaLoadingScreen:{((PictureBox)sender).Name}\":{{\r\n\"templateId\":\"AthenaLoadingScreen:{((PictureBox)sender).Name}\",\r\n\"attributes\":{{\r\n\"item_seen\":true,\r\n\"variants\": [],\r\n\"favorite\": false\r\n}},\r\n\"quantity\": 1\r\n}},";
                MessageBox.Show(json);
            }
            else if (((PictureBox)sender).Name.Contains("MusicPack"))
            {
                string json =
                    $"\"AthenaMusicPack:{((PictureBox)sender).Name}\":{{\r\n\"templateId\":\"AthenaMusicPack:{((PictureBox)sender).Name}\",\r\n\"attributes\":{{\r\n\"item_seen\":true,\r\n\"variants\": [],\r\n\"favorite\": false\r\n}},\r\n\"quantity\": 1\r\n}},";
                MessageBox.Show(json);
            }
            else if (((PictureBox)sender).Name.Contains("Pet"))
            {
                string json =
                    $"\"AthenaPet:{((PictureBox)sender).Name}\":{{\r\n\"templateId\":\"AthenaPet:{((PictureBox)sender).Name}\",\r\n\"attributes\":{{\r\n\"item_seen\":true,\r\n\"variants\": [],\r\n\"favorite\": false\r\n}},\r\n\"quantity\": 1\r\n}},";
                MessageBox.Show(json);
            }
            else if (((PictureBox)sender).Name.Contains("Pickaxe"))
            {
                string json =
                    $"\"AthenaPickaxe:{((PictureBox)sender).Name}\":{{\r\n\"templateId\":\"AthenaPickaxe:{((PictureBox)sender).Name}\",\r\n\"attributes\":{{\r\n\"item_seen\":true,\r\n\"variants\": [],\r\n\"favorite\": false\r\n}},\r\n\"quantity\": 1\r\n}},";
                MessageBox.Show(json);
            }
            else if (((PictureBox)sender).Name.Contains("Spray"))
            {
                string json =
                    $"\"AthenaDance:{((PictureBox)sender).Name}\":{{\r\n\"templateId\":\"AthenaDance:{((PictureBox)sender).Name}\",\r\n\"attributes\":{{\r\n\"item_seen\":true,\r\n\"variants\": [],\r\n\"favorite\": false\r\n}},\r\n\"quantity\": 1\r\n}},";
                MessageBox.Show(json);
            }
            else if (((PictureBox)sender).Name.Contains("Toy"))
            {
                string json =
                    $"\"AthenaDance:{((PictureBox)sender).Name}\":{{\r\n\"templateId\":\"AthenaDance:{((PictureBox)sender).Name}\",\r\n\"attributes\":{{\r\n\"item_seen\":true,\r\n\"variants\": [],\r\n\"favorite\": false\r\n}},\r\n\"quantity\": 1\r\n}},";
                MessageBox.Show(json);
            }
            else if (((PictureBox)sender).Name.Contains("Wrap"))
            {
                string json =
                    $"\"AthenaItemWrap:{((PictureBox)sender).Name}\":{{\r\n\"templateId\":\"AthenaItemWrap:{((PictureBox)sender).Name}\",\r\n\"attributes\":{{\r\n\"item_seen\":true,\r\n\"variants\": [],\r\n\"favorite\": false\r\n}},\r\n\"quantity\": 1\r\n}},";
                MessageBox.Show(json);
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
    }
}
