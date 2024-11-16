using System;
using System.Collections.Concurrent;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Filter;
using NewFilter.CORE;
using NewFilter.WGUI;

namespace NewFilter;

public class MainMenu : Form
{
	internal class Global
	{
		public static MainMenu MainWindow;

		public static Agent AgentGlobal;
	}

	public static ConcurrentDictionary<Socket, UserConnections> MobilinGames = new ConcurrentDictionary<Socket, UserConnections>();

	public static List<users> LisansUser = new List<users>();

	public static bool Lisans = true;

	public static string lisansdate = "12/11/2072";

	public static string LisansIP = "192.168.8.101";

	public static int LisansPort = 8080;

	public static string LisansCharName = "sa";

	public static string LisansPassword = "1234";

	public static bool LisansAuto = true;

	public static string SQL_HOST = "DESKTOP-5MDODIM";

	public static string SQL_USER = "sa";

	public static string SQL_PASS = "1453aE";

	public static string FILTER_DB = "CLEAN_GUARD";

	public static string ACC_DB = "SRO_VT_ACCOUNT";

	public static string SHA_DB = "SRO_VT_SHARD";

	public static string LOG_DB = "SRO_VT_LOG";

	public static iniFile cfg = new iniFile("Config/settings.ini");

	public static bool JobBlockReversed = false;

	public static bool AttendanceWnd = false;

	public static bool AttendanceWndCombobox = false;

	public static bool AttendanceWndCheck = false;

	public static string AttendanceMon = "";

	public static bool EventRegWnd = false;

	public static bool DiscordWnd = false;

	public static bool FacebookWnd = false;

	public static bool ChestWnd = false;

	public static bool NewReverse = false;

	public static bool OldMainPopup = false;

	public static bool ItemComparison = false;

	public static bool PermanenyAlchemy = false;

	public static bool GuildJobMode = false;

	public static string DiscordURL = string.Empty;

	public static string FacebookURL = string.Empty;

	public static long DiscordInstanceID = 0L;

	public static int MasteryLimit = 0;

	public static int MaxPartyLevelLimit = 0;

	public static bool EnableMarket = false;

	public static bool EnableMarketToken = false;

	public static int CustomTitlePrice = 0;

	public static string CustomTitleBirim = string.Empty;

	public static bool EnableMarketSilk = false;

	public static bool EnableMarketGold = false;

	public static string Proxy_IP = "192.168.8.101";

	public static string Server_IP = "192.168.8.101";

	public static int Download_Public_port = 1455;

	public static int Download_Server_port = 15881;

	public static int Gateway_Public_port = 1453;

	public static int Gateway_Server_port = 15779;

	public static int Agent_Public_port = 1454;

	public static int Agent_Server_port = 15884;

	public static string SERVER_NAME = "";

	public static int SHARD_MAX_PLAYER = 0;

	public static string CAPCHA = "0";

	public static int AFKMS = 15884;

	public static bool DISABLECAPCHA = true;

	public static List<string> BanStrUserID = new List<string>();

	public static List<string> BanIPList = new List<string>();

	public static List<string> BanHWID = new List<string>();

	public static bool SCANONLINE = false;

	public static int GATEWAY_PACKET_RESET = 500;

	public static int AGENT_PACKET_RESET = 500;

	public static int DOWNLOAD_PACKET_RESET = 500;

	public static int GW_BPS_VALUE = 0;

	public static int AG_BPS_VALUE = 0;

	public static int GW_PPS_VALUE = 0;

	public static int AG_PPS_VALUE = 0;

	public static int DW_BPS_VALUE = 0;

	public static int DW_PPS_VALUE = 0;

	public static int FLOOD_COUNT = 0;

	public static bool PACKET_CHECK = false;

	public static bool FIREWALLBANCHECK = false;

	public static bool MAINTENANCE = false;

	public static bool OpCodeWhiteList = true;

	public static int IP_LIMIT = 0;

	public static int PC_LIMIT = 0;

	public static int BA_PC_LIMIT = 0;

	public static int CTF_PC_LIMIT = 0;

	public static int FTW_PC_LIMIT = 0;

	public static int JOB_PC_LIMIT = 0;

	public static int HT_PC_LIMIT = 0;

	public static int JOBT_PC_LIMIT = 0;

	public static int FGW_PC_LIMIT = 0;

	public static int JUPITER_PC_LIMIT = 0;

	public static int SURVIVAL_PC_LIMIT = 0;

	public static int CAFE_IP_LIMIT = 0;

	public static int PLUS_LIMIT = 15;

	public static int DEVIL_PLUS_LIMIT = 6;

	public static string IP_LIMIT_NOTICE = "";

	public static string PC_LIMIT_NOTICE = "";

	public static string BA_PC_LIMIT_NOTICE = "";

	public static string CTF_PC_LIMIT_NOTICE = "";

	public static string FTW_PC_LIMIT_NOTICE = "";

	public static string JOB_PC_LIMIT_NOTICE = "";

	public static string HT_PC_LIMIT_NOTICE = "";

	public static string JOBT_PC_LIMIT_NOTICE = "";

	public static string FGW_PC_LIMIT_NOTICE = "";

	public static string JUPITER_PC_LIMIT_NOTICE = "";

	public static string SURVIVAL_PC_LIMIT_NOTICE = "";

	public static string CAFE_IP_LIMIT_NOTICE = "";

	public static string PLUS_LIMIT_NOTICE = "";

	public static string DEVIL_PLUS_LIMIT_NOTICE = "";

	public static int EXCHANGE_DELAY = 0;

	public static int EXIT_DELAY = 0;

	public static int GLOBAL_DELAY = 0;

	public static int GUILD_DELAY = 0;

	public static int RESTART_DELAY = 0;

	public static int STALL_DELAY = 0;

	public static int UNION_DELAY = 0;

	public static int ZERK_DELAY = 0;

	public static int REVERSE_DELAY = 0;

	public static string EXCHANGE_DELAY_NOTICE = "";

	public static string EXIT_DELAY_NOTICE = "";

	public static string GLOBAL_DELAY_NOTICE = "";

	public static string GUILD_DELAY_NOTICE = "";

	public static string RESTART_DELAY_NOTICE = "";

	public static string STALL_DELAY_NOTICE = "";

	public static string UNION_DELAY_NOTICE = "";

	public static string ZERK_DELAY_NOTICE = "";

	public static string REVERSE_DELAY_NOTICE = "";

	public static int BA_REQ_LEVEL = 0;

	public static int CTF_REQ_LEVEL = 0;

	public static int EXCHANGE_LEVEL = 0;

	public static int GLOBAL_LEVEL = 0;

	public static int STALL_LEVEL = 0;

	public static int ZERK_LEVEL = 0;

	public static int DROP_GOLD_LEVEL = 0;

	public static string BA_REQ_LEVEL_NOTICE = "";

	public static string CTF_REQ_LEVEL_NOTICE = "";

	public static string EXCHANGE_LEVEL_NOTICE = "";

	public static string GLOBAL_LEVEL_NOTICE = "";

	public static string STALL_LEVEL_NOTICE = "";

	public static string ZERK_LEVEL_NOTICE = "";

	public static string DROP_GOLD_LEVEL_NOTICE = "";

	public static bool WELCOME_MSG_CHECK = false;

	public static string WELCOME_TEXT_NOTICE = string.Empty;

	public static bool DISABLE_RESTART_BUTTON_CHECK = false;

	public static string DISABLE_RESTART_NOTICE = string.Empty;

	public static bool DISABLE_AVATAR_BLUES_CHECK = false;

	public static string DISABLE_AVATAR_BLUES_NOTICE = string.Empty;

	public static bool DISABLED_ACADEMY_CHECK = false;

	public static string DISABLED_ACADEMY_NOTICE = string.Empty;

	public static bool DISABLE_TAX_RATE_CHANGE_CHECK = false;

	public static string DISABLE_TAX_RATE_CHANGE_NOTICE = string.Empty;

	public static bool TOWN_DROP_ITEM_CHECK = false;

	public static string TOWN_DROP_ITEM_NOTICE = string.Empty;

	public static bool DISCONNECT_NOTICE_CHECK = false;

	public static string DISCONNECT_NOTICE_NOTICE = string.Empty;

	public static bool BAN_NOTICE_CHECK = false;

	public static string BAN_NOTICE_NOTICE = string.Empty;

	public static bool GM_NOTICE_CHECK = false;

	public static string GM_NOTICE_NOTICE = string.Empty;

	public static bool BLOCKSKILL_NOTICE_CHECK = false;

	public static string BLOCKSKILL_NOTICE_NOTICE = string.Empty;

	public static bool BLOCKSKILLPVP_NOTICE_CHECK = false;

	public static string BLOCKSKILLPVP_NOTICE = string.Empty;

	public static bool BLOCKZERKPVP_NOTICE_CHECK = false;

	public static string BLOCKZERKPVP_NOTICE = string.Empty;

	public static bool PULSNOTICE_NOTICE_CHECK = false;

	public static int PULSNOTICE_NOTICE_Start = 0;

	public static string PULSNOTICE_NOTICE = string.Empty;

	public static string SOX_PLUS1 = string.Empty;

	public static string SOX_PLUS2 = string.Empty;

	public static string SOX_DROP1 = string.Empty;

	public static string SOX_DROP2 = string.Empty;

	public static string SOXDROPNOTICE_NOTICE = string.Empty;

	public static bool SOXDROPOTICE_NOTICE_CHECK = false;

	public static List<Clientlesss> ClientlessList = new List<Clientlesss>();

	public static string CLIENT_HOST_IP = string.Empty;

	public static int CLIENT_GW_PORT = 0;

	public static int CLIENT_VERSION = 0;

	public static int CLIENT_LOCALE = 0;

	public static string CLIENT_CAPTCHA_VALUE = string.Empty;

	public static string CLIENT_ID = string.Empty;

	public static string CLIENT_PW = string.Empty;

	public static string CLIENT_CharName = string.Empty;

	public static bool AUTOEVENT_ENABLE = false;

	public static bool AUTOEVENT_ALREADY = false;

	public static bool QNA_ENABLE = false;

	public static int QNA_TIMETOANSWER = 60;

	public static int QNA_ROUNDS = 0;

	public static int QNA_ITEMREWARD = 0;

	public static string QNA_ITEMNAME = string.Empty;

	public static int QNA_ITEMCOUNT = 0;

	public static string QNA_STARTNOTICE = string.Empty;

	public static string QNA_INFONOTICE = string.Empty;

	public static string QNA_INFOCHAR = string.Empty;

	public static string QNA_END = string.Empty;

	public static string QNA_WIN = string.Empty;

	public static string QNA_ROUNDINFO = string.Empty;

	public static bool HNS_ENABLE = false;

	public static int HNS_TIMETOSEARCH = 300;

	public static int HNS_ROUNDS = 0;

	public static int HNS_ITEMREWARD = 0;

	public static int HNS_ITEMCOUNT = 0;

	public static string HNS_ITEMNAME = string.Empty;

	public static string HNS_STARTNOTICE = string.Empty;

	public static string HNS_INFONOTICE = string.Empty;

	public static string HNS_PLACEINFO = string.Empty;

	public static string HNS_END = string.Empty;

	public static string HNS_WIN = string.Empty;

	public static bool GMK_ENABLE = false;

	public static bool GMK_STATUS = false;

	public static int GMK_ROUND = 0;

	public static int GMK_TIMETOWAIT = 0;

	public static int GMK_ITEMID = 0;

	public static int GMK_ITEMCOUNT = 0;

	public static string GMK_INFO_NOTICE = string.Empty;

	public static string GMK_REGIONID = string.Empty;

	public static string GMK_ITEMNAME = string.Empty;

	public static string GMK_POSX = string.Empty;

	public static string GMK_POSY = string.Empty;

	public static string GMK_POSZ = string.Empty;

	public static string GMK_PLACENAME = string.Empty;

	public static string GMK_START_NOTICE = string.Empty;

	public static string GMK_INFORM_NOTICE = string.Empty;

	public static string GMK_WIN_NOTICE = string.Empty;

	public static string GMK_END_NOTICE = string.Empty;

	public static bool SND_ENABLE = false;

	public static bool SND_STATUS = false;

	public static int SND_MOBID = 0;

	public static int SND_TIMETOSEARCH = 0;

	public static int SND_ROUNDS = 0;

	public static int SND_ITEMREWARD = 0;

	public static int SND_ITEMCOUNT = 0;

	public static string SND_ITEMNAME = string.Empty;

	public static string SND_STARTNOTICE = string.Empty;

	public static string SND_INFONOTICE = string.Empty;

	public static string SND_PLACEINFO = string.Empty;

	public static string SND_END = string.Empty;

	public static string SND_WIN = string.Empty;

	public static bool LG_ENABLE = false;

	public static bool LG_STATUS = false;

	public static bool LG_REGISTER_STATUS = false;

	public static int LG_ROUND = 0;

	public static int LG_TIMETOWAIT = 0;

	public static int LG_TICKETPRICE = 0;

	public static string LG_START_NOTICE = string.Empty;

	public static string LG_TICKETPRICE_NOTICE = string.Empty;

	public static string LG_STARTREG_NOTICE = string.Empty;

	public static string LG_STOPREG_NOTICE = string.Empty;

	public static string LG_WIN_NOTICE = string.Empty;

	public static string LG_END_NOTICE = string.Empty;

	public static string LG_ENDD_NOTICE = string.Empty;

	public static string LG_REGISTERSUCCESS_NOTICE = string.Empty;

	public static string LG_REGISTED_NOTICE = string.Empty;

	public static string LG_GOLDREQUIRE_NOTICE = string.Empty;

	public static bool LPN_ENABLE = false;

	public static bool LPN_STATUS = false;

	public static int PT_MINVALUE = 0;

	public static int PT_MAXVALUE = 0;

	public static int LPN_TIMETOWAIT = 120;

	public static int LPN_ROUNDS = 0;

	public static int LPN_ITEMREWARD = 0;

	public static int LPN_ITEMCOUNT = 0;

	public static string LPN_ITEMNAME = string.Empty;

	public static int LPN_TARGETPARTYY = 0;

	public static int PT_NR_RECORD = 0;

	public static bool LPN_LUCKYPT_STATUS = false;

	public static string LPN_START_NOTICE = string.Empty;

	public static string LPN_INFO = string.Empty;

	public static string LPN_WIN_NOTICE = string.Empty;

	public static string LPN_END = string.Empty;

	public static string LPN_NOREFORM_NOTICE = string.Empty;

	public static string LPN_ROUNDINFO = string.Empty;

	public static bool LMS_ENABLE = false;

	public static int LMS_PCLIMIT = 0;

	public static bool LMS_REGISTERSTATUS = false;

	public static bool LMS_GATE_OPENCLOSE = false;

	public static int LMS_GATEID = 0;

	public static int LMS_REGIONID = 0;

	public static int LMS_REGIONID2 = 0;

	public static int LMS_REQUIRELEVEL = 0;

	public static int LMS_ITEMID = 0;

	public static bool LMS_WAR_START = false;

	public static int LMS_MATCHTIME = 0;

	public static int LMS_REGISTERTIME = 0;

	public static int LMS_GATEWAIT_TIME = 0;

	public static int LMS_ITEMREWARD = 0;

	public static int LMS_ITEMCOUNT = 0;

	public static string LMS_ITEMNAME = string.Empty;

	public static string LMS_FIGHTSTART_NOTICE = string.Empty;

	public static string LMS_START_NOTICE = string.Empty;

	public static string LMS_REGISTERTIME_NOTICE = string.Empty;

	public static string LMS_INFORM_NOTICE = string.Empty;

	public static string LMS_INFO2_NOTICE = string.Empty;

	public static string LMS_GATEOPEN_NOTICE = string.Empty;

	public static string LMS_GATECLOSE_NOTICE = string.Empty;

	public static string LMS_WIN_NOTICE = string.Empty;

	public static string LMS_END_NOTICE = string.Empty;

	public static string LMS_REGISTERSUCCESS_NOTICE = string.Empty;

	public static string LMS_REGISTED_NOTICE = string.Empty;

	public static string LMS_REGISTERCLOSE_NOTICE = string.Empty;

	public static string LMS_REQUIRELEVEL_NOTICE = string.Empty;

	public static string LMS_JOB_NOTICE = string.Empty;

	public static string LMS_LIMIT_NOTICE = string.Empty;

	public static string LMS_ELIMINATED_NOTICE = string.Empty;

	public static string LMS_CANCELEVENT_NOTICE = string.Empty;

	public static List<string> LMS_PLAYERSLIST = new List<string>();

	public static bool LMS_ATTACK_ON_OFF = false;

	public static bool SURV_ENABLE = false;

	public static bool SURV_REGISTERSTATUS = false;

	public static bool SURV_GATE_OPENCLOSE = false;

	public static int SURV_GATEID = 0;

	public static int SURV_REGIONID = 0;

	public static int SURV_REGIONID2 = 0;

	public static int SURV_REQUIRELEVEL = 0;

	public static int SURV_ITEMID = 0;

	public static bool SURV_WAR_START = false;

	public static int SURV_MATCHTIME = 0;

	public static int SURV_REGISTERTIME = 0;

	public static int SURV_GATEWAIT_TIME = 0;

	public static int SURV_ITEMREWARD = 0;

	public static int SURV_ITEMCOUNT = 0;

	public static string SURV_ITEMNAME = string.Empty;

	public static bool SURV_ATTACK_ON_OFF = false;

	public static List<string> SURV_PLAYERSLIST = new List<string>();

	public static string SURV_FIGHTSTART_NOTICE = string.Empty;

	public static string SURV_START_NOTICE = string.Empty;

	public static string SURV_REGISTERTIME_NOTICE = string.Empty;

	public static string SURV_INFORM_NOTICE = string.Empty;

	public static string SURV_INFO2_NOTICE = string.Empty;

	public static string SURV_GATEOPEN_NOTICE = string.Empty;

	public static string SURV_GATECLOSE_NOTICE = string.Empty;

	public static string SURV_WIN_NOTICE = string.Empty;

	public static string SURV_END_NOTICE = string.Empty;

	public static string SURV_REGISTERSUCCESS_NOTICE = string.Empty;

	public static string SURV_REGISTED_NOTICE = string.Empty;

	public static string SURV_REGISTERCLOSE_NOTICE = string.Empty;

	public static string SURV_REQUIRELEVEL_NOTICE = string.Empty;

	public static string SURV_JOB_NOTICE = string.Empty;

	public static string SURV_LIMIT_NOTICE = string.Empty;

	public static string SURV_ELIMINATED_NOTICE = string.Empty;

	public static string SURV_CANCELEVENT_NOTICE = string.Empty;

	public static bool ITEM_LOCK = true;

	public static int ITEM_LOCK_MAX_FAIL = 3;

	public static string ITEM_LOCK_NOT_EXIST_NOTICE = "Your account is not protected by item lock.";

	public static string ITEM_LOCK_FIRST_TIME_NOTICE = "You have activated item protection, remember your Code: {code}";

	public static string ITEM_UNLOCK_NOTICE = "Your items has been unlocked.";

	public static string ITEM_LOCK_IS_UNLOCKED_NOTICE = "Your items is already locked. Please use unlock button for unlock items.";

	public static string ITEM_LOCK_WRONG_CODE_NOTICE = "Wrong password. Please use the correct password. {count}/{max}.";

	public static string ITEM_LOCK_DISCONNECT_NOTICE = "You have been disconnected because you entered the wrong password many times !";

	public static string ITEM_LOCK_PASSWORD_LENGTH_NOTICE = "Item lock password is password must contain at least 4 numbers.";

	public static string ITEM_LOCK_INTEGER_NOTICE = "Item lock password is must be integer.";

	public static string ITEM_LOCK_IS_GUILD_NOTICE = "Your items is locked. Please exit this window. You will be get disconnect.";

	public static string ITEM_LOCK_IS_GUILD_SP = "Your Skill Points is locked. You cant donate your Skill Points. Please unlock.";

	public static string ITEM_LOCK_EXCHANGE_NOTICE = "Your character is locked. You cant use exchange function. Please unlock.";

	public static string ITEM_LOCK_ALCHEMY_NOTICE = "Your character is locked. You cant use alchemy function. Please unlock.";

	public static string ITEM_LOCK_STALL_NOTICE = "Your character is locked. You cant use stall function. Please unlock.";

	public static string ITEM_LOCK_DRESS_BLUE_NOTICE = "Your character is locked. You cant add grant magic option on avatar. Please unlock.";

	public static string ITEM_LOCK_DROP_SELL_GOLD = "Your items is locked. You cant drop and sell your items. Please unlock.";

	public static bool FortressStatus = false;

	public static List<int> JANGANFORTRESS = new List<int>();

	public static List<int> HOTANFORTRESS = new List<int>();

	public static List<string> JGFTWINPLAYER = new List<string>();

	public static List<string> HTFTWINPLAYER = new List<string>();

	public static List<int> PartyDisableRegion = new List<int>();

	public static List<int> PartyDisableteleport = new List<int>();

	public static List<int> CosDisableTeleport = new List<int>();

	public static List<int> PvpArenaRegion = new List<int>();

	public static List<int> LMSArenaRegion = new List<int>();

	public static List<string> WhiteList;

	public static List<string> cafeip;

	public static List<string> GM_ACCOUNT_List = new List<string>();

	public static List<string> GM_IP_List = new List<string>();

	public static List<string> AutoCape;

	public static List<string> DisablePartyRegion;

	private static object ListLocker = new object();

	public static string DatabaseBackup = "C:\\RLonline\\Database";

	private IContainer components = null;

	private ListView listView1;

	private ColumnHeader columnHeader1;

	private ColumnHeader columnHeader2;

	private ColumnHeader columnHeader3;

	private Label Agent_Count_Lable;

	private Label Gateway_Count_Lable;

	private Label Download_Count_Lable;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private GroupBox groupBox1;

	private TextBox textBox_Froxy_IP;

	private Label Proxy_label;

	private TextBox textBox_PublicGatewayPort;

	private Label label3;

	private Label label2;

	private TextBox textBox_Server_IP;

	private Label Server_label;

	private GroupBox groupBox4;

	private Label label13;

	private TextBox textBox_FilterDB;

	private TextBox textBox_LogDB;

	private TextBox textBox_ShardDB;

	private TextBox textBox_AccDB;

	private TextBox textBox_SQLPass;

	private TextBox textBox_SQLId;

	private TextBox textBox_SQLHost;

	private Label label12;

	private Label label11;

	private Label label10;

	private Label label14;

	private Label label15;

	private Label label16;

	private Button Open_Directory_Button;

	private Button RestartFilter_Button;

	private TextBox textBox_RealAgentPort;

	private Label label4;

	private TextBox textBox_PublicAgentPort;

	private TextBox textBox_RealGatewayPort;

	private Label label5;

	private TextBox textBox_RealDownloadPort;

	private Label label6;

	private TextBox textBox_PublicDownloadPort;

	private Label label7;

	private Button Save_Settings_Buttun;

	private GroupBox groupBox9;

	private TextBox DOWNLOAD_PACKET_RESET_TextBox;

	private TextBox DW_PPS_VALUE_TextBox;

	private Label label27;

	private Label label28;

	private TextBox DW_BPS_VALUE_TextBox;

	private Label label29;

	private GroupBox groupBox8;

	private TextBox AGENT_PACKET_RESET_TextBox;

	private TextBox AG_PPS_VALUE_TextBox;

	private Label label21;

	private Label label22;

	private TextBox AG_BPS_VALUE_TextBox;

	private Label label23;

	private GroupBox groupBox7;

	private TextBox GATEWAY_PACKET_RESET_TextBox;

	private TextBox GW_PPS_VALUE_TextBox;

	private Label label24;

	private Label label25;

	private TextBox GW_BPS_VALUE_TextBox;

	private Label label26;

	private GroupBox groupBox6;

	private Label label30;

	private TextBox FLOOD_COUNT_TextBox;

	private CheckBox MAINTENANCE_CHECKBOX;

	private CheckBox PACKET_CHECKBOX;

	private CheckBox FIREWALLBANCHECKBOX;

	private CheckBox WhiteList_CheckBox;

	private GroupBox groupBox12;

	private Label label68;

	private Label label62;

	private Label label63;

	private Label label65;

	private Label label66;

	private Label label67;

	private Label label69;

	private Label label61;

	private Label label60;

	private Label label59;

	private Label label58;

	private Label label57;

	private Label label56;

	private Label label55;

	private Label label54;

	private TextBox CAFE_IP_LIMITNoticeTextBox;

	private Label label51;

	private TextBox PLUS_LIMITNoticeTextBox;

	private Label label52;

	private TextBox DEVIL_PLUS_LIMITNoticeTextBox;

	private TextBox JOBT_PC_LIMITNoticeTextBox;

	private Label label8;

	private TextBox FGW_PC_LIMITNoticeTextBox;

	private Label label9;

	private TextBox JUPITER_PC_LIMITNoticeTextBox;

	private Label label32;

	private TextBox SURVIVAL_PC_LIMITNoticeTextBox;

	private Label label33;

	private TextBox HT_PC_LIMITNoticeTextBox;

	private Label label47;

	private TextBox PC_LIMITNoticeTextBox;

	private Label label31;

	private TextBox BA_PC_LIMITNoticeTextBox;

	private Label label20;

	private TextBox CTF_PC_LIMITNoticeTextBox;

	private Label label19;

	private TextBox FTW_PC_LIMITNoticeTextBox;

	private Label label18;

	private TextBox JOB_PC_LIMITNoticeTextBox;

	private Label label17;

	private TextBox IP_LIMITNoticeTextBox;

	private Label label1;

	private Label label49;

	private Label label48;

	private Label label46;

	private Label label44;

	private Label label43;

	private Label label42;

	private Label label41;

	private Label label40;

	private Label label39;

	private Label label37;

	private Label label36;

	private Label label38;

	private Label label35;

	private TextBox JUPITER_PC_LIMITTextBox;

	private TextBox FGW_PC_LIMITTextBox;

	private TextBox JOBT_PC_LIMITTextBox;

	private TextBox SURVIVAL_PC_LIMITTextBox;

	private TextBox HT_PC_LIMITTextBox;

	private TextBox CAFE_IP_LIMITTextBox;

	private TextBox PLUS_LIMITTextBox;

	private TextBox DEVIL_PLUS_LIMITTextBox;

	private TextBox PC_LIMITTextBox;

	private TextBox BA_PC_LIMITTextBox;

	private TextBox CTF_PC_LIMITTextBox;

	private TextBox FTW_PC_LIMITTextBox;

	private TextBox JOB_PC_LIMITTextBox;

	private TextBox IP_LIMITTextBox;

	private Label label34;

	private TabPage tabPage3;

	private GroupBox groupBox3;

	private Label label116;

	private Label label110;

	private Label label117;

	private Label label111;

	private Label label118;

	private Label label112;

	private Label label119;

	private Label label113;

	private Label label120;

	private Label label114;

	private TextBox ZERK_LEVEL_TextBox;

	private TextBox STALL_LEVEL_TextBox;

	private Label label123;

	private TextBox GLOBAL_LEVEL_TextBox;

	private TextBox EXCHANGE_LEVEL_TextBox;

	private TextBox CTF_REQ_LEVEL_TextBox;

	private Label label115;

	private TextBox BA_REQ_LEVEL_TextBox;

	private TextBox CTF_REQ_LEVEL_NOTICE_TextBox;

	private TextBox BA_REQ_LEVEL_NOTICE_TextBox;

	private Label label126;

	private Label label131;

	private TextBox EXCHENGE_LEVEL_NOTICE_TextBox;

	private Label label130;

	private Label label127;

	private TextBox ZERK_LEVEL_NOTICE_TextBox;

	private TextBox GLOBAL_LEVEL_NOTICE_TextBox;

	private Label label129;

	private Label label128;

	private TextBox STALL_LEVEL_NOTICE_TextBox;

	private GroupBox groupBox2;

	private Label label94;

	private Label label95;

	private Label label96;

	private Label label97;

	private Label label98;

	private Label label99;

	private Label label100;

	private Label label101;

	private TextBox ZERK_DELAY_NOTICE_TextBox;

	private Label label102;

	private TextBox UNION_DELAY_NOTICE_TextBox;

	private Label label103;

	private TextBox EXIT_DELAY_NOTICE_TextBox;

	private Label label104;

	private TextBox GLOBAL_DELAY_NOTICE_TextBox;

	private Label label105;

	private TextBox GUILD_DELAY_NOTICE_TextBox;

	private Label label106;

	private TextBox RESTART_DELAY_NOTICE_TextBox;

	private Label label107;

	private TextBox STALL_DELAY_NOTICE_TextBox;

	private Label label108;

	private TextBox EXCHANGE_DELAY_NOTICE_TextBox;

	private Label label109;

	private Label label45;

	private Label label50;

	private Label label53;

	private Label label64;

	private Label label90;

	private Label label91;

	private Label label92;

	private TextBox ZERK_DELAY_TextBox;

	private TextBox UNION_DELAY_TextBox;

	private TextBox STALL_DELAY_TextBox;

	private TextBox RESTART_DELAY_TextBox;

	private TextBox GUILD_DELAY_TextBox;

	private TextBox GLOBAL_DELAY_TextBox;

	private TextBox EXIT_DELAY_TextBox;

	private Label label93;

	private TextBox EXCHANGE_DELAY_TextBox;

	private TabPage tabPage4;

	private GroupBox groupBox30;

	private CheckBox BLOCKSKILL_NOTICE_CHECKBOX;

	private CheckBox GM_NOTICE_CHECKBOX;

	private CheckBox BAN_NOTICE_CHECKBOX;

	private TextBox PULSNOTICE_NOTICE_Start_TextBox;

	private CheckBox DISCONNECT_NOTICE_CHECKBOX;

	private Label label137;

	private Label label136;

	private CheckBox BLOCKZERKPVP_NOTICE_CHECKBOX;

	private CheckBox PULSNOTICE_NOTICE_CHECKBOX;

	private Label label133;

	private Label label132;

	private Label label125;

	private CheckBox DISABLE_TAX_RATE_CHANGE_CHECKBOX;

	private Label label124;

	private Label label122;

	private CheckBox WELCOME_MSG_CHECKBOX;

	private Label label121;

	private CheckBox TOWN_DROP_ITEM_CHECKBOX;

	private Label label86;

	private CheckBox DISABLED_ACADEMY_CHECKBOX;

	private Label label85;

	private CheckBox DISABLE_AVATAR_BLUES_CHECKBOX;

	private Label label84;

	private CheckBox DISABLE_RESTART_BUTTON_CHECKBOX;

	private Label label83;

	private TextBox PULSNOTICE_NOTICE_TextBox;

	private TextBox BLOCKZERKPVP_NOTICE_TextBox;

	private TextBox BLOCKSKILL_NOTICE_TextBox;

	private TextBox GM_NOTICE_TextBox;

	private TextBox TOWN_DROP_ITEM_NOTICE_TextBox;

	private TextBox DISCONNECT_NOTICE_TextBox;

	private TextBox BAN_NOTICE_TextBox;

	private TextBox DISABLE_TAX_RATE_CHANGE_NOTICE_TextBox;

	private TextBox DISABLED_ACADEMY_NOTICE_TextBox;

	private TextBox DISABLE_AVATAR_BLUES_NOTICE_TextBox;

	private TextBox DISABLE_RESTART_NOTICE_TextBox;

	private TextBox WELCOME_TEXT_NOTICE_TextBox;

	private GroupBox groupBox5;

	private GroupBox groupBox16;

	private Label label87;

	private CheckBox afksystem;

	private Label label89;

	private TextBox AFKMS_TEXTBOX;

	private Label label80;

	private Label label79;

	private Label label77;

	private CheckBox DISABLECAPCHA_CHECKBOX;

	private TextBox CAPCHA_TEXTBOX;

	private TextBox SHARD_MAX_PLAYER_TEXTBOX;

	private Label label76;

	private TextBox SERVER_NAME_TEXTBOX;

	private Label label78;

	private Label label71;

	private TextBox DROP_GOLD_LEVEL_TextBox;

	private Label label70;

	private TextBox DROP_LEVEL_NOTICE_TextBox;

	private Label label72;

	private TextBox REVERSE_DELAY_NOTICE_TextBox;

	private Label label73;

	private Label label74;

	private TextBox REVERSE_DELAY_TextBox;

	private Label label75;

	private Button button1;

	private TabControl tabControl1;

	private TabPage tabPage5;

	private GroupBox groupBox37;

	private DataGridView dataGridView1;

	private Button button_ShowEventTime;

	private Button button_RemoveEvent;

	private Button button_AddEvent;

	private ComboBox comboBox_EventTime_EventName;

	private ComboBox comboBox_EventTime_Day;

	private DateTimePicker dateTimePicker_EventTime_Hour;

	private GroupBox groupBox27;

	public CheckBox CL_INV_STATE;

	public Label label81;

	private Label label82;

	private Label label88;

	public TextBox CL_CHARNAME;

	private Label label134;

	public TextBox CL_CAPTCHA;

	private Label label135;

	public TextBox CL_PASSWORD;

	private Label label138;

	public TextBox CL_ID;

	private Label label139;

	public TextBox CL_LOCALE;

	private Label label140;

	public TextBox CL_VER;

	private Label label141;

	public TextBox CL_GT_PORT;

	private Label label142;

	public TextBox CL_GT_IP;

	private TabPage tabPage6;

	private GroupBox groupBox38;

	private TextBox itemnametrivia;

	private Label label157;

	private TextBox questitemcount;

	private Label label158;

	private TextBox itemcodetrivia;

	private TextBox qnarounds;

	private TextBox textBox_QnATimeAnswer;

	private Label label191;

	private Label Rounds;

	private Label label189;

	private CheckBox checkBox_QnAEnable;

	private TabPage tabPage7;

	private GroupBox groupBox29;

	private TextBox HNS_ITEMNAMEBOX;

	private Label label152;

	private TextBox HNS_ITEMCOUNTBOX;

	private Label label153;

	private TextBox HNS_ITEMREWARDBOX;

	private TextBox HNS_ROUNDSBOX;

	private TextBox HNS_TIMETOSEARCHBOX;

	private Label label154;

	private Label label155;

	private Label label156;

	private CheckBox HNS_ENABLEBOX;

	private GroupBox groupBox10;

	private TabPage tabPage8;

	private GroupBox groupBox45;

	private TextBox LG_ROUNDBOX;

	private Label label143;

	private TextBox LG_TICKETPRICEBOX;

	private TextBox LG_TIMETOWAITBOX;

	private Label label223;

	private Label label224;

	private CheckBox LG_ENABLEBOX;

	private TextBox QNA_STARTNOTICEBOX;

	private Label label185;

	private TextBox QNA_WINBOX;

	private Label label184;

	private TextBox QNA_ENDBOX;

	private Label label183;

	private TextBox QNA_INFOCHARBOX;

	private Label label187;

	private TextBox QNA_INFONOTICEBOX;

	private Label label159;

	private Label label186;

	private TextBox QNA_ROUNDINFOBOX;

	private GroupBox groupBox11;

	private Label label190;

	private TextBox HNS_WINBOX;

	private Label label192;

	private TextBox HNS_ENDBOX;

	private Label label193;

	private TextBox HNS_PLACEINFOBOX;

	private Label label194;

	private TextBox HNS_INFONOTICEBOX;

	private Label label195;

	private TextBox HNS_STARTNOTICEBOX;

	private GroupBox groupBox13;

	private TextBox GMK_END_NOTICEBOX;

	private Label label196;

	private TextBox GMK_WIN_NOTICEBOX;

	private Label label197;

	private TextBox GMK_INFORM_NOTICEBOX;

	private Label label198;

	private TextBox GMK_INFO_NOTICEBOX;

	private Label label199;

	private TextBox GMK_START_NOTICEBOX;

	private Label label200;

	private TextBox GMK_PLACENAMEBOX;

	private GroupBox groupBox49;

	private Label label149;

	private TextBox GMK_ITEMCOUNTBOX;

	private Label label150;

	private TextBox GMK_ROUNDBOX;

	private TextBox GMK_ITEMNAMEBOX;

	private Label label151;

	private Label label243;

	private Label label242;

	private Label label221;

	private TextBox GMK_POSZBOX;

	private TextBox GMK_POSYBOX;

	private TextBox GMK_POSXBOX;

	private TextBox GMK_REGIONIDBOX;

	private Label label241;

	private TextBox GMK_ITEMIDBOX;

	private TextBox GMK_TIMETOWAITBOX;

	private Label label238;

	private Label label240;

	private CheckBox GMK_ENABLEBOX;

	private TabPage tabPage9;

	private GroupBox groupBox47;

	private Label label176;

	private TextBox PT_MAXVALUEBOX;

	private TextBox PT_MINVALUEBOX;

	private Label label177;

	private Label label178;

	private TextBox LPN_ITEMCOUNTBOX;

	private Label label179;

	private TextBox LPN_ROUNDSBOX;

	private TextBox LPN_ITEMNAMEBOX;

	private Label label180;

	private TextBox LPN_ITEMREWARDBOX;

	private Label label181;

	private TextBox LPN_TIMETOWAITBOX;

	private Label label233;

	private CheckBox LPN_ENABLEBOX;

	private Label label188;

	private GroupBox groupBox15;

	private Label label207;

	private TextBox LG_REGISTED_NOTICEBOX;

	private Label label216;

	private TextBox LG_REGISTERSUCCESS_NOTICEBOX;

	private Label label213;

	private TextBox LG_GOLDREQUIRE_NOTICEBOX;

	private Label label215;

	private TextBox LG_STOPREG_NOTICEBOX;

	private Label label208;

	private TextBox LG_STARTREG_NOTICEBOX;

	private Label label209;

	private TextBox LG_WIN_NOTICEBOX;

	private Label label210;

	private TextBox LG_END_NOTICEBOX;

	private Label label211;

	private TextBox LG_TICKETPRICE_NOTICEBOX;

	private Label label212;

	private TextBox LG_START_NOTICEBOX;

	private GroupBox groupBox14;

	private TextBox SND_ENDBOX;

	private Label label202;

	private TextBox SND_WINBOX;

	private Label label203;

	private Label label204;

	private TextBox SND_PLACEINFOBOX;

	private Label label205;

	private TextBox SND_INFONOTICEBOX;

	private Label label206;

	private TextBox SND_STARTNOTICEBOX;

	private GroupBox groupBox43;

	private Label label144;

	private TextBox SND_MOBIDBOX;

	private Label label145;

	private TextBox SND_TIMETOSEARCHBOX;

	private Label label214;

	private TextBox SND_ITEMCOUNTBOX;

	private CheckBox SND_ENABLEBOX;

	private Label label146;

	private Label label147;

	private TextBox SND_ITEMNAMEBOX;

	private TextBox SND_ITEMREWARDBOX;

	private Label label148;

	private TextBox SND_ROUNDSBOX;

	private TextBox LG_ENDD_NOTICEBOX;

	private Label label201;

	private GroupBox groupBox17;

	private Label label217;

	private TextBox LPN_WIN_NOTICEBOX;

	private Label label218;

	private TextBox LPN_ENDBOX;

	private Label label219;

	private TextBox LPN_NOREFORM_NOTICEBOX;

	private Label label220;

	private TextBox LPN_INFOBOX;

	private Label label222;

	private TextBox LPN_START_NOTICEBOX;

	private TabPage tabPage10;

	private GroupBox groupBox53;

	private TextBox LMS_PCLIMITBOX;

	private Label label169;

	private TextBox LMS_GATEWAIT_TIMEBOX;

	private Label label170;

	private TextBox LMS_REGIONIDBOX;

	private Label label171;

	private TextBox LMS_GATEIDBOX;

	private Label label172;

	private TextBox LMS_ITEMCOUNTBOX;

	private Label label173;

	private TextBox LMS_ITEMNAMEBOX;

	private Label label174;

	private TextBox LMS_ITEMIDBOX;

	private Label label175;

	private TextBox LMS_REQUIRELEVELBOX;

	private Label label269;

	private TextBox LMS_REGISTERTIMEBOX;

	private TextBox LMS_MATCHTIMEBOX;

	private Label label276;

	private Label label277;

	private CheckBox LMS_ENABLEBOX;

	private Label label225;

	private TextBox LPN_ROUNDINFOBOX;

	private GroupBox groupBox18;

	private TextBox LMS_REQUIRELEVEL_NOTICEBOX;

	private Label label246;

	private TextBox LMS_FIGHTSTART_NOTICEBOX;

	private Label label245;

	private TextBox LMS_ELIMINATED_NOTICEBOX;

	private Label label236;

	private TextBox LMS_JOB_NOTICEBOX;

	private Label label237;

	private TextBox LMS_REGISTERSUCCESS_NOTICEBOX;

	private Label label239;

	private TextBox LMS_REGISTED_NOTICEBOX;

	private Label label244;

	private TextBox LMS_END_NOTICEBOX;

	private Label label182;

	private Label label226;

	private TextBox LMS_WIN_NOTICEBOX;

	private Label label227;

	private TextBox LMS_INFO2_NOTICEBOX;

	private Label label228;

	private TextBox LMS_CANCELEVENT_NOTICEBOX;

	private Label label229;

	private TextBox LMS_GATECLOSE_NOTICEBOX;

	private Label label230;

	private TextBox LMS_GATEOPEN_NOTICEBOX;

	private Label label231;

	private TextBox LMS_INFORM_NOTICEBOX;

	private Label label232;

	private TextBox LMS_REGISTERCLOSE_NOTICEBOX;

	private Label label234;

	private TextBox LMS_REGISTERTIME_NOTICEBOX;

	private Label label235;

	private TextBox LMS_START_NOTICEBOX;

	private TabPage tabPage11;

	private GroupBox groupBox28;

	private TextBox SURV_GATEWAIT_TIMEBOX;

	private Label label160;

	private TextBox SURV_REGIONIDBOX;

	private Label label161;

	private TextBox SURV_GATEIDBOX;

	private Label label162;

	private TextBox SURV_ITEMCOUNTBOX;

	private Label label163;

	private TextBox SURV_ITEMNAMEBOX;

	private Label label164;

	private TextBox SURV_ITEMIDBOX;

	private Label label165;

	private TextBox SURV_REQUIRELEVELBOX;

	private Label label166;

	private TextBox SURV_REGISTERTIMEBOX;

	private TextBox SURV_MATCHTIMEBOX;

	private Label label167;

	private Label label168;

	private CheckBox SURV_ENABLEBOX;

	private GroupBox groupBox19;

	private TextBox SURV_REQUIRELEVEL_NOTICEBOX;

	private Label label247;

	private TextBox SURV_FIGHTSTART_NOTICEBOX;

	private Label label248;

	private TextBox SURV_ELIMINATED_NOTICEBOX;

	private Label label249;

	private TextBox SURV_JOB_NOTICEBOX;

	private Label label250;

	private TextBox SURV_REGISTERSUCCESS_NOTICEBOX;

	private Label label251;

	private TextBox SURV_REGISTED_NOTICEBOX;

	private Label label252;

	private TextBox SURV_END_NOTICEBOX;

	private Label label253;

	private Label label254;

	private TextBox SURV_WIN_NOTICEBOX;

	private Label label255;

	private TextBox SURV_INFO2_NOTICEBOX;

	private Label label256;

	private TextBox SURV_CANCELEVENT_NOTICEBOX;

	private Label label257;

	private TextBox SURV_GATECLOSE_NOTICEBOX;

	private Label label258;

	private TextBox SURV_GATEOPEN_NOTICEBOX;

	private Label label259;

	private TextBox SURV_INFORM_NOTICEBOX;

	private Label label260;

	private TextBox SURV_REGISTERCLOSE_NOTICEBOX;

	private Label label261;

	private TextBox SURV_REGISTERTIME_NOTICEBOX;

	private Label label262;

	private TextBox SURV_START_NOTICEBOX;

	private Label label263;

	private ListBox listBox1;

	private Label label264;

	private ListBox listBox2;

	private GroupBox groupBox20;

	private Button DC_Clientless_Button;

	private Button Start_Clientless_Button;

	private TabPage tabPage12;

	private UserInfo userInfo1;

	private GroupBox groupBox23;

	private ListBox BanUserListBox;

	private Button AddBanUser;

	private Button RemoveBanUser;

	private TextBox BanUserTextBox;

	private GroupBox groupBox22;

	private GroupBox groupBox21;

	private ListBox BanIpListBox;

	private Button RemoveBanIP;

	private TextBox BanIPTextBox;

	private Button AddBanIP;

	private GroupBox groupBox24;

	private ListBox BanHwidListBox;

	private Button AddBanHwid;

	private Button RemoveBanHwid;

	private TextBox BanhwidTextBox;

	private CheckBox ScanOnlineCheckbox;

	private Button button2;

	private Button button3;

	private Label label265;

	private TextBox SOX_PLUS1_TextBox;

	private Label label266;

	private TextBox SOX_PLUS2_TextBox;

	private TextBox SOXDROPNOTICE_NOTICE_TextBox;

	private Label label270;

	private Label label267;

	private TextBox SOX_Drop2_TextBox;

	private Label label268;

	private TextBox SOX_Drop1_TextBox;

	private CheckBox SOXDROPNOTICE_NOTICE_CHECKBOX;

	private TabPage tabPage13;

	private GroupBox groupBox25;

	private CheckBox ENABLE_EVNTREG;

	private CheckBox ENABLE_FB;

	private CheckBox ENABLE_DC;

	private CheckBox ENABLE_ATTENDANCE;

	private CheckBox ENABLE_CHEST;

	private Label label271;

	private TextBox DCID;

	private CheckBox checkBoxNewRew;

	private CheckBox checkBox1oldmain;

	private CheckBox checkBox1itemcomp;

	private Label label272;

	private TextBox DiscordSite;

	private Label label273;

	private TextBox facebooksite;

	private GroupBox groupBox26;

	private ListBox suitlistbox;

	private Button addregionbutton;

	private Button removeregionButton;

	private TextBox SuitRegiontextbox;

	private Label label275;

	private TextBox MaxPtNoLimit;

	private Label label274;

	private TextBox MaxMastery;

	private Label label278;

	private ComboBox ATTENDANCE_comboBox;

	private GroupBox groupBox31;

	private Button Block_Skill_Add_Button;

	private Button Block_Skill_Remove_Button;

	private TextBox Block_Skill_TextBox;

	private ListBox Block_Skill_listBox;

	private GroupBox groupBox32;

	private Label label279;

	private TextBox Block_Skill_TextBoxSkillID;

	private Label label280;

	private TextBox LMS_REGIONIDBOX2;

	private Label label281;

	private TextBox SURV_REGIONIDBOX2;

	private Label label282;

	private CheckBox ENABLE_PALCHEMY;

	private CheckBox GUILDJOB;

	private TabPage tabPage14;

	private Button button4;

	private Button Login;

	private TextBox TextBoxLisansPassword;

	private Label label283;

	private TextBox TextBoxLisansUserName;

	private Label label284;

	private Label LicenseStatus;

	private Label label285;

	private Label label286;

	private Label LisansDateLable;

	private CheckBox LisansAutoCheckBox;

	private Button RefreshGmList;

	private GroupBox groupBox33;

	private CheckBox marketbutton;

	private CheckBox goldboxs;

	private CheckBox silksystembox;

	private Label label287;

	private CheckBox tokenbox;

	private Label label288;

	private TextBox titleprices;

	private TextBox titlepricebirim;

	private CheckBox JobReverseBlock;

	public MainMenu()
	{
		this.InitializeComponent();
	}

	private void MainMenu_Load(object sender, EventArgs e)
	{
		Global.MainWindow = this;
		this.LoadSettings();
		this.StartGame();
	}

	private void StartGame()
	{
		if (this.IsServerConnected("Server=" + MainMenu.SQL_HOST + ";User ID=" + MainMenu.SQL_USER + ";Password=" + MainMenu.SQL_PASS + ";MultipleActiveResultSets=True;"))
		{
			AsyncServer.InitializeSingleEngine(MainMenu.Proxy_IP, MainMenu.Gateway_Public_port, MainMenu.Download_Public_port, MainMenu.Agent_Public_port);
			MainMenu.WriteLine(1, "MSSQL Server Connection is OK.");
			sqlCon.SetConnectiontType();
			this.Initial();
			new Thread(RightContent).Start();
		}
		else
		{
			MainMenu.WriteLine(2, "MSSQL Server Connection Faulty. Please recheck your SQL setting");
			MainMenu.WriteLine(2, "Cannot communicate with SQL server. Filter initialization failed.");
		}
	}

	private bool IsServerConnected(string connectionString)
	{
		using SqlConnection sqlConnection = new SqlConnection(connectionString);
		try
		{
			sqlConnection.Open();
			return true;
		}
		catch (SqlException)
		{
			return false;
		}
	}

	private void Initial()
	{
		MainMenu.WhiteList = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "/Config/WhiteList.txt").ToList();
		MainMenu.cafeip = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "/Config/cafeip.txt").ToList();
		MainMenu.AutoCape = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "/Config/AutoCape.txt").ToList();
		MainMenu.DisablePartyRegion = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "/Config/DisablePartyRegion.txt").ToList();
		this.GET_GMIP_List();
		this.GET_GM_ACCOUNT_List();
		MainMenu.CLIENT_VERSION = Task.Run(async () => await sqlCon.prod_int("select MAX(nVersion) from " + MainMenu.ACC_DB + ".dbo._ModuleVersion")).Result;
		this.CL_VER.Text = MainMenu.CLIENT_VERSION.ToString();
		sqlCon.LoadEveryThing();
		this.GetBanList();
		this.GetSuitRegions();
		this.GetBlockSkill();
		MainMenu.BlockedReverseRegion();
		MainMenu.BlockedScrollRegion();
		MainMenu.LoadZerkBlockArenas();
	}

	public static void BlockedReverseRegion()
	{
		Utils.BlockedReverseRegion.Clear();
		foreach (DataRow row in Task.Run(async () => await sqlCon.GetList("SELECT * FROM CLEAN_GUARD.dbo._BlockedReverseRegions")).Result.Rows)
		{
			string text;
			text = row["RegionID"].ToString();
			if (text != string.Empty && text != null && !text.StartsWith("#") && !Utils.BlockedReverseRegion.Contains(text))
			{
				Utils.BlockedReverseRegion.Add(text);
			}
		}
	}

	public static void LoadZerkBlockArenas()
	{
		Utils.BlockedZerkForRegion.Clear();
		foreach (DataRow row in Task.Run(async () => await sqlCon.GetList("SELECT * FROM CLEAN_GUARD.dbo._BlockedZerkRegions")).Result.Rows)
		{
			string text;
			text = row["RegionID"].ToString();
			if (text != string.Empty && text != null && !text.StartsWith("#") && !Utils.BlockedZerkForRegion.Contains(text))
			{
				Utils.BlockedZerkForRegion.Add(text);
			}
		}
	}

	public static void BlockedScrollRegion()
	{
		Utils.BlockedScrollForRegion.Clear();
		foreach (DataRow row in Task.Run(async () => await sqlCon.GetList("SELECT * FROM CLEAN_GUARD.dbo._BlockedScrollRegions")).Result.Rows)
		{
			string text;
			text = row["RegionID"].ToString();
			if (text != string.Empty && text != null && !text.StartsWith("#"))
			{
				string[] array;
				array = text.Split(':');
				if (!Utils.BlockedScrollForRegion.ContainsKey(array[1]))
				{
					Utils.BlockedScrollForRegion.TryAdd(array[1], array[0]);
				}
			}
		}
	}

	public async void GetSuitRegions()
	{
		await Task.Delay(100);
		Utils.EventSuitRegionIds.Clear();
		this.suitlistbox.Items.Clear();
		DataTable IPList;
		IPList = await sqlCon.GetList("select RegionID from " + MainMenu.FILTER_DB + ".._EventSuitRegions");
		if (IPList.Rows.Count == 0)
		{
			return;
		}
		foreach (DataRow row in IPList.Rows)
		{
			string regionID;
			regionID = Convert.ToString(row["RegionID"]);
			this.suitlistbox.Items.Add(regionID.ToString());
			Utils.EventSuitRegionIds.Add(Convert.ToUInt16(regionID));
		}
	}

	public async void GetBlockSkill()
	{
		await Task.Delay(100);
		Utils.RegionSkillBlock.Clear();
		this.Block_Skill_listBox.Items.Clear();
		DataTable IPList;
		IPList = await sqlCon.GetList("select * from " + MainMenu.FILTER_DB + ".._BlockedSkills");
		if (IPList.Rows.Count == 0)
		{
			return;
		}
		foreach (DataRow asdf in IPList.Rows)
		{
			int regionID;
			regionID = Convert.ToInt32(asdf["RegionID"]);
			int SkillID;
			SkillID = Convert.ToInt32(asdf["SkillID"]);
			Utils.RegionSkillBlock.TryAdd(SkillID.ToString(), regionID.ToString());
			this.Block_Skill_listBox.Items.Add($"{regionID.ToString()} | {SkillID.ToString()}");
		}
	}

	public void GetBanList()
	{
		MainMenu.BanIPList.Clear();
		MainMenu.BanStrUserID.Clear();
		this.BanIpListBox.Items.Clear();
		this.BanUserListBox.Items.Clear();
		this.BanHwidListBox.Items.Clear();
		MainMenu.BanHWID.Clear();
		DataTable result;
		result = Task.Run(async () => await sqlCon.GetList("select IP from " + MainMenu.FILTER_DB + ".._FirewallBlocks")).Result;
		if (result.Rows.Count != 0)
		{
			foreach (DataRow row in result.Rows)
			{
				string item;
				item = Convert.ToString(row["IP"]);
				this.BanIpListBox.Items.Add(item);
				MainMenu.BanIPList.Add(item);
			}
		}
		DataTable result2;
		result2 = Task.Run(async () => await sqlCon.GetList("select StrUserID from " + MainMenu.FILTER_DB + ".._FirewallUserBlocks")).Result;
		if (result2.Rows.Count != 0)
		{
			foreach (DataRow row2 in result2.Rows)
			{
				string item2;
				item2 = Convert.ToString(row2["StrUserID"]);
				this.BanUserListBox.Items.Add(item2);
				MainMenu.BanStrUserID.Add(item2);
			}
		}
		DataTable result3;
		result3 = Task.Run(async () => await sqlCon.GetList("select HWID from " + MainMenu.FILTER_DB + ".._FirewallHwidBlocks")).Result;
		if (result3.Rows.Count == 0)
		{
			return;
		}
		foreach (DataRow row3 in result3.Rows)
		{
			string item3;
			item3 = Convert.ToString(row3["HWID"]);
			this.BanHwidListBox.Items.Add(item3);
			MainMenu.BanHWID.Add(item3);
		}
	}

	private void LoadSettings()
	{
		try
		{
			if (File.Exists(Application.StartupPath + "\\Config/settings.ini"))
			{
				this.GetSetting("LISENSCONFIG", "LisansCharName", ref MainMenu.LisansPassword, ref this.TextBoxLisansUserName);
				this.GetSetting("LISENSCONFIG", "LisansPassword", ref MainMenu.LisansPassword, ref this.TextBoxLisansPassword);
				this.GetSetting("LISENSCONFIG", "LisansAuto", ref MainMenu.LisansAuto, ref this.LisansAutoCheckBox);
				this.GetSetting("SERVERNOTICE", "JOB_REVERSE_BLOCK", ref MainMenu.JobBlockReversed, ref this.JobReverseBlock);
				this.GetSetting("GENERAL", "SQL_HOST", ref MainMenu.SQL_HOST, ref this.textBox_SQLHost);
				this.GetSetting("GENERAL", "SQL_USER", ref MainMenu.SQL_USER, ref this.textBox_SQLId);
				this.GetSetting("GENERAL", "SQL_PASS", ref MainMenu.SQL_PASS, ref this.textBox_SQLPass);
				this.GetSetting("GENERAL", "ACC_DB", ref MainMenu.ACC_DB, ref this.textBox_AccDB);
				this.GetSetting("GENERAL", "SHA_DB", ref MainMenu.SHA_DB, ref this.textBox_ShardDB);
				this.GetSetting("GENERAL", "LOG_DB", ref MainMenu.LOG_DB, ref this.textBox_LogDB);
				this.GetSetting("GENERAL", "FILTER_DB", ref MainMenu.FILTER_DB, ref this.textBox_FilterDB);
				this.GetSetting("SERVERSETTINGS", "Proxy_IP", ref MainMenu.Proxy_IP, ref this.textBox_Froxy_IP);
				this.GetSetting("SERVERSETTINGS", "Server_IP", ref MainMenu.Server_IP, ref this.textBox_Server_IP);
				this.GetSetting("SERVERSETTINGS", "Download_Public_port", ref MainMenu.Download_Public_port, ref this.textBox_PublicDownloadPort);
				this.GetSetting("SERVERSETTINGS", "Download_Server_port", ref MainMenu.Download_Server_port, ref this.textBox_RealDownloadPort);
				this.GetSetting("SERVERSETTINGS", "Gateway_Public_port", ref MainMenu.Gateway_Public_port, ref this.textBox_PublicGatewayPort);
				this.GetSetting("SERVERSETTINGS", "Gateway_Server_port", ref MainMenu.Gateway_Server_port, ref this.textBox_RealGatewayPort);
				this.GetSetting("SERVERSETTINGS", "Agent_Public_port", ref MainMenu.Agent_Public_port, ref this.textBox_PublicAgentPort);
				this.GetSetting("SERVERSETTINGS", "Agent_Server_port", ref MainMenu.Agent_Server_port, ref this.textBox_RealAgentPort);
				this.GetSetting("PROTECTION", "GATEWAY_PACKET_RESET", ref MainMenu.GATEWAY_PACKET_RESET, ref this.GATEWAY_PACKET_RESET_TextBox);
				this.GetSetting("PROTECTION", "AGENT_PACKET_RESET", ref MainMenu.AGENT_PACKET_RESET, ref this.AGENT_PACKET_RESET_TextBox);
				this.GetSetting("PROTECTION", "DOWNLOAD_PACKET_RESET", ref MainMenu.DOWNLOAD_PACKET_RESET, ref this.DOWNLOAD_PACKET_RESET_TextBox);
				this.GetSetting("PROTECTION", "GW_BPS_VALUE", ref MainMenu.GW_BPS_VALUE, ref this.GW_BPS_VALUE_TextBox);
				this.GetSetting("PROTECTION", "AG_BPS_VALUE", ref MainMenu.AG_BPS_VALUE, ref this.AG_BPS_VALUE_TextBox);
				this.GetSetting("PROTECTION", "GW_PPS_VALUE", ref MainMenu.GW_PPS_VALUE, ref this.GW_PPS_VALUE_TextBox);
				this.GetSetting("PROTECTION", "AG_PPS_VALUE", ref MainMenu.AG_PPS_VALUE, ref this.AG_PPS_VALUE_TextBox);
				this.GetSetting("PROTECTION", "DW_BPS_VALUE", ref MainMenu.DW_BPS_VALUE, ref this.DW_BPS_VALUE_TextBox);
				this.GetSetting("PROTECTION", "DW_PPS_VALUE", ref MainMenu.DW_PPS_VALUE, ref this.DW_PPS_VALUE_TextBox);
				this.GetSetting("PROTECTION", "FLOOD_COUNT", ref MainMenu.FLOOD_COUNT, ref this.FLOOD_COUNT_TextBox);
				this.GetSetting("PROTECTION", "PACKET_CHECK", ref MainMenu.PACKET_CHECK, ref this.PACKET_CHECKBOX);
				this.GetSetting("PROTECTION", "FIREWALLBANCHECK", ref MainMenu.FIREWALLBANCHECK, ref this.FIREWALLBANCHECKBOX);
				this.GetSetting("PROTECTION", "MAINTENANCE", ref MainMenu.MAINTENANCE, ref this.MAINTENANCE_CHECKBOX);
				this.GetSetting("PROTECTION", "OpCodeWhiteList", ref MainMenu.OpCodeWhiteList, ref this.WhiteList_CheckBox);
				this.GetSetting("LIMITS", "IP_LIMIT", ref MainMenu.IP_LIMIT, ref this.IP_LIMITTextBox);
				this.GetSetting("LIMITS", "PC_LIMIT", ref MainMenu.PC_LIMIT, ref this.PC_LIMITTextBox);
				this.GetSetting("LIMITS", "BA_PC_LIMIT", ref MainMenu.BA_PC_LIMIT, ref this.BA_PC_LIMITTextBox);
				this.GetSetting("LIMITS", "CTF_PC_LIMIT", ref MainMenu.CTF_PC_LIMIT, ref this.CTF_PC_LIMITTextBox);
				this.GetSetting("LIMITS", "FTW_PC_LIMIT", ref MainMenu.FTW_PC_LIMIT, ref this.FTW_PC_LIMITTextBox);
				this.GetSetting("LIMITS", "JOB_PC_LIMIT", ref MainMenu.JOB_PC_LIMIT, ref this.JOB_PC_LIMITTextBox);
				this.GetSetting("LIMITS", "HT_PC_LIMIT", ref MainMenu.HT_PC_LIMIT, ref this.HT_PC_LIMITTextBox);
				this.GetSetting("LIMITS", "JOBT_PC_LIMIT", ref MainMenu.JOBT_PC_LIMIT, ref this.JOBT_PC_LIMITTextBox);
				this.GetSetting("LIMITS", "FGW_PC_LIMIT", ref MainMenu.FGW_PC_LIMIT, ref this.FGW_PC_LIMITTextBox);
				this.GetSetting("LIMITS", "JUPITER_PC_LIMIT", ref MainMenu.JUPITER_PC_LIMIT, ref this.JUPITER_PC_LIMITTextBox);
				this.GetSetting("LIMITS", "SURVIVAL_PC_LIMIT", ref MainMenu.SURVIVAL_PC_LIMIT, ref this.SURVIVAL_PC_LIMITTextBox);
				this.GetSetting("LIMITS", "CAFE_IP_LIMIT", ref MainMenu.CAFE_IP_LIMIT, ref this.CAFE_IP_LIMITTextBox);
				this.GetSetting("LIMITS", "PLUS_LIMIT", ref MainMenu.PLUS_LIMIT, ref this.PLUS_LIMITTextBox);
				this.GetSetting("LIMITS", "DEVIL_PLUS_LIMIT", ref MainMenu.DEVIL_PLUS_LIMIT, ref this.DEVIL_PLUS_LIMITTextBox);
				this.GetSetting("LIMITS", "IP_LIMIT_NOTICE", ref MainMenu.IP_LIMIT_NOTICE, ref this.IP_LIMITNoticeTextBox);
				this.GetSetting("LIMITS", "PC_LIMIT_NOTICE", ref MainMenu.PC_LIMIT_NOTICE, ref this.PC_LIMITNoticeTextBox);
				this.GetSetting("LIMITS", "BA_PC_LIMIT_NOTICE", ref MainMenu.BA_PC_LIMIT_NOTICE, ref this.BA_PC_LIMITNoticeTextBox);
				this.GetSetting("LIMITS", "CTF_PC_LIMIT_NOTICE", ref MainMenu.CTF_PC_LIMIT_NOTICE, ref this.CTF_PC_LIMITNoticeTextBox);
				this.GetSetting("LIMITS", "FTW_PC_LIMIT_NOTICE", ref MainMenu.FTW_PC_LIMIT_NOTICE, ref this.FTW_PC_LIMITNoticeTextBox);
				this.GetSetting("LIMITS", "JOB_PC_LIMIT_NOTICE", ref MainMenu.JOB_PC_LIMIT_NOTICE, ref this.JOB_PC_LIMITNoticeTextBox);
				this.GetSetting("LIMITS", "HT_PC_LIMIT_NOTICE", ref MainMenu.HT_PC_LIMIT_NOTICE, ref this.HT_PC_LIMITNoticeTextBox);
				this.GetSetting("LIMITS", "JOBT_PC_LIMIT_NOTICE", ref MainMenu.JOBT_PC_LIMIT_NOTICE, ref this.JOBT_PC_LIMITNoticeTextBox);
				this.GetSetting("LIMITS", "FGW_PC_LIMIT_NOTICE", ref MainMenu.FGW_PC_LIMIT_NOTICE, ref this.FGW_PC_LIMITNoticeTextBox);
				this.GetSetting("LIMITS", "JUPITER_PC_LIMIT_NOTICE", ref MainMenu.JUPITER_PC_LIMIT_NOTICE, ref this.JUPITER_PC_LIMITNoticeTextBox);
				this.GetSetting("LIMITS", "SURVIVAL_PC_LIMIT_NOTICE", ref MainMenu.SURVIVAL_PC_LIMIT_NOTICE, ref this.SURVIVAL_PC_LIMITNoticeTextBox);
				this.GetSetting("LIMITS", "CAFE_IP_LIMIT_NOTICE", ref MainMenu.CAFE_IP_LIMIT_NOTICE, ref this.CAFE_IP_LIMITNoticeTextBox);
				this.GetSetting("LIMITS", "PLUS_LIMIT_NOTICE", ref MainMenu.PLUS_LIMIT_NOTICE, ref this.PLUS_LIMITNoticeTextBox);
				this.GetSetting("LIMITS", "DEVIL_PLUS_LIMIT_NOTICE", ref MainMenu.DEVIL_PLUS_LIMIT_NOTICE, ref this.DEVIL_PLUS_LIMITNoticeTextBox);
				this.GetSetting("DELAYS", "EXCHANGE_DELAY", ref MainMenu.EXCHANGE_DELAY, ref this.EXCHANGE_DELAY_TextBox);
				this.GetSetting("DELAYS", "EXIT_DELAY", ref MainMenu.EXIT_DELAY, ref this.EXIT_DELAY_TextBox);
				this.GetSetting("DELAYS", "GLOBAL_DELAY", ref MainMenu.GLOBAL_DELAY, ref this.GLOBAL_DELAY_TextBox);
				this.GetSetting("DELAYS", "GUILD_DELAY", ref MainMenu.GUILD_DELAY, ref this.GUILD_DELAY_TextBox);
				this.GetSetting("DELAYS", "RESTART_DELAY", ref MainMenu.RESTART_DELAY, ref this.RESTART_DELAY_TextBox);
				this.GetSetting("DELAYS", "STALL_DELAY", ref MainMenu.STALL_DELAY, ref this.STALL_DELAY_TextBox);
				this.GetSetting("DELAYS", "UNION_DELAY", ref MainMenu.UNION_DELAY, ref this.UNION_DELAY_TextBox);
				this.GetSetting("DELAYS", "ZERK_DELAY", ref MainMenu.ZERK_DELAY, ref this.ZERK_DELAY_TextBox);
				this.GetSetting("DELAYS", "REVERSE_DELAY", ref MainMenu.REVERSE_DELAY, ref this.REVERSE_DELAY_TextBox);
				this.GetSetting("DELAYS", "EXCHANGE_DELAY_NOTICE", ref MainMenu.EXCHANGE_DELAY_NOTICE, ref this.EXCHANGE_DELAY_NOTICE_TextBox);
				this.GetSetting("DELAYS", "EXIT_DELAY_NOTICE", ref MainMenu.EXIT_DELAY_NOTICE, ref this.EXIT_DELAY_NOTICE_TextBox);
				this.GetSetting("DELAYS", "GLOBAL_DELAY_NOTICE", ref MainMenu.GLOBAL_DELAY_NOTICE, ref this.GLOBAL_DELAY_NOTICE_TextBox);
				this.GetSetting("DELAYS", "GUILD_DELAY_NOTICE", ref MainMenu.GUILD_DELAY_NOTICE, ref this.GUILD_DELAY_NOTICE_TextBox);
				this.GetSetting("DELAYS", "RESTART_DELAY_NOTICE", ref MainMenu.RESTART_DELAY_NOTICE, ref this.RESTART_DELAY_NOTICE_TextBox);
				this.GetSetting("DELAYS", "STALL_DELAY_NOTICE", ref MainMenu.STALL_DELAY_NOTICE, ref this.STALL_DELAY_NOTICE_TextBox);
				this.GetSetting("DELAYS", "UNION_DELAY_NOTICE", ref MainMenu.UNION_DELAY_NOTICE, ref this.UNION_DELAY_NOTICE_TextBox);
				this.GetSetting("DELAYS", "ZERK_DELAY_NOTICE", ref MainMenu.ZERK_DELAY_NOTICE, ref this.ZERK_DELAY_NOTICE_TextBox);
				this.GetSetting("DELAYS", "REVERSE_DELAY_NOTICE", ref MainMenu.REVERSE_DELAY_NOTICE, ref this.REVERSE_DELAY_NOTICE_TextBox);
				this.GetSetting("LEVELS", "BA_REQ_LEVEL", ref MainMenu.BA_REQ_LEVEL, ref this.BA_REQ_LEVEL_TextBox);
				this.GetSetting("LEVELS", "CTF_REQ_LEVEL", ref MainMenu.CTF_REQ_LEVEL, ref this.CTF_REQ_LEVEL_TextBox);
				this.GetSetting("LEVELS", "EXCHANGE_LEVEL", ref MainMenu.EXCHANGE_LEVEL, ref this.EXCHANGE_LEVEL_TextBox);
				this.GetSetting("LEVELS", "GLOBAL_LEVEL", ref MainMenu.GLOBAL_LEVEL, ref this.GLOBAL_LEVEL_TextBox);
				this.GetSetting("LEVELS", "STALL_LEVEL", ref MainMenu.STALL_LEVEL, ref this.STALL_LEVEL_TextBox);
				this.GetSetting("LEVELS", "ZERK_LEVEL", ref MainMenu.ZERK_LEVEL, ref this.ZERK_LEVEL_TextBox);
				this.GetSetting("LEVELS", "DROP_GOLD_LEVEL", ref MainMenu.DROP_GOLD_LEVEL, ref this.DROP_GOLD_LEVEL_TextBox);
				this.GetSetting("LEVELS", "BA_REQ_LEVEL_NOTICE", ref MainMenu.BA_REQ_LEVEL_NOTICE, ref this.BA_REQ_LEVEL_NOTICE_TextBox);
				this.GetSetting("LEVELS", "CTF_REQ_LEVEL_NOTICE", ref MainMenu.CTF_REQ_LEVEL_NOTICE, ref this.CTF_REQ_LEVEL_NOTICE_TextBox);
				this.GetSetting("LEVELS", "EXCHANGE_LEVEL_NOTICE", ref MainMenu.EXCHANGE_LEVEL_NOTICE, ref this.EXCHENGE_LEVEL_NOTICE_TextBox);
				this.GetSetting("LEVELS", "GLOBAL_LEVEL_NOTICE", ref MainMenu.GLOBAL_LEVEL_NOTICE, ref this.GLOBAL_LEVEL_NOTICE_TextBox);
				this.GetSetting("LEVELS", "STALL_LEVEL_NOTICE", ref MainMenu.STALL_LEVEL_NOTICE, ref this.STALL_LEVEL_NOTICE_TextBox);
				this.GetSetting("LEVELS", "ZERK_LEVEL_NOTICE", ref MainMenu.ZERK_LEVEL_NOTICE, ref this.ZERK_LEVEL_NOTICE_TextBox);
				this.GetSetting("LEVELS", "DROP_GOLD_LEVEL_NOTICE", ref MainMenu.DROP_GOLD_LEVEL_NOTICE, ref this.DROP_LEVEL_NOTICE_TextBox);
				this.GetSetting("SERVERNOTICE", "WELCOME_MSG_CHECK", ref MainMenu.WELCOME_MSG_CHECK, ref this.WELCOME_MSG_CHECKBOX);
				this.GetSetting("SERVERNOTICE", "DISABLE_RESTART_BUTTON_CHECK", ref MainMenu.DISABLE_RESTART_BUTTON_CHECK, ref this.DISABLE_RESTART_BUTTON_CHECKBOX);
				this.GetSetting("SERVERNOTICE", "DISABLE_AVATAR_BLUES_CHECK", ref MainMenu.DISABLE_AVATAR_BLUES_CHECK, ref this.DISABLE_AVATAR_BLUES_CHECKBOX);
				this.GetSetting("SERVERNOTICE", "DISABLED_ACADEMY_CHECK", ref MainMenu.DISABLED_ACADEMY_CHECK, ref this.DISABLED_ACADEMY_CHECKBOX);
				this.GetSetting("SERVERNOTICE", "DISABLE_TAX_RATE_CHANGE_CHECK", ref MainMenu.DISABLE_TAX_RATE_CHANGE_CHECK, ref this.DISABLE_TAX_RATE_CHANGE_CHECKBOX);
				this.GetSetting("SERVERNOTICE", "TOWN_DROP_ITEM_CHECK", ref MainMenu.TOWN_DROP_ITEM_CHECK, ref this.TOWN_DROP_ITEM_CHECKBOX);
				this.GetSetting("SERVERNOTICE", "DISCONNECT_NOTICE_CHECK", ref MainMenu.DISCONNECT_NOTICE_CHECK, ref this.DISCONNECT_NOTICE_CHECKBOX);
				this.GetSetting("SERVERNOTICE", "BAN_NOTICE_CHECK", ref MainMenu.BAN_NOTICE_CHECK, ref this.BAN_NOTICE_CHECKBOX);
				this.GetSetting("SERVERNOTICE", "GM_NOTICE_CHECK", ref MainMenu.GM_NOTICE_CHECK, ref this.GM_NOTICE_CHECKBOX);
				this.GetSetting("SERVERNOTICE", "BLOCKSKILL_NOTICE_CHECK", ref MainMenu.BLOCKSKILL_NOTICE_CHECK, ref this.BLOCKSKILL_NOTICE_CHECKBOX);
				this.GetSetting("SERVERNOTICE", "BLOCKSKILLPVP_NOTICE_CHECK", ref MainMenu.BLOCKZERKPVP_NOTICE_CHECK, ref this.BLOCKZERKPVP_NOTICE_CHECKBOX);
				this.GetSetting("SERVERNOTICE", "BLOCKZERKPVP_NOTICE_CHECK", ref MainMenu.PULSNOTICE_NOTICE_CHECK, ref this.PULSNOTICE_NOTICE_CHECKBOX);
				this.GetSetting("SERVERNOTICE", "SOX_PLUS1", ref MainMenu.SOX_PLUS1, ref this.SOX_PLUS1_TextBox);
				this.GetSetting("SERVERNOTICE", "SOX_PLUS2", ref MainMenu.SOX_PLUS2, ref this.SOX_PLUS2_TextBox);
				this.GetSetting("SERVERNOTICE", "SOX_DROP1", ref MainMenu.SOX_DROP1, ref this.SOX_Drop1_TextBox);
				this.GetSetting("SERVERNOTICE", "SOX_DROP2", ref MainMenu.SOX_DROP2, ref this.SOX_Drop2_TextBox);
				this.GetSetting("SERVERNOTICE", "SOXDROPOTICE_NOTICE_CHECK", ref MainMenu.SOXDROPOTICE_NOTICE_CHECK, ref this.SOXDROPNOTICE_NOTICE_CHECKBOX);
				this.GetSetting("SERVERNOTICE", "SOXDROPNOTICE_NOTICE", ref MainMenu.SOXDROPNOTICE_NOTICE, ref this.SOXDROPNOTICE_NOTICE_TextBox);
				this.GetSetting("SERVERNOTICE", "WELCOME_TEXT_NOTICE", ref MainMenu.WELCOME_TEXT_NOTICE, ref this.WELCOME_TEXT_NOTICE_TextBox);
				this.GetSetting("SERVERNOTICE", "DISABLE_RESTART_NOTICE", ref MainMenu.DISABLE_RESTART_NOTICE, ref this.DISABLE_RESTART_NOTICE_TextBox);
				this.GetSetting("SERVERNOTICE", "DISABLE_AVATAR_BLUES_NOTICE", ref MainMenu.DISABLE_AVATAR_BLUES_NOTICE, ref this.DISABLE_AVATAR_BLUES_NOTICE_TextBox);
				this.GetSetting("SERVERNOTICE", "DISABLED_ACADEMY_NOTICE", ref MainMenu.DISABLED_ACADEMY_NOTICE, ref this.DISABLED_ACADEMY_NOTICE_TextBox);
				this.GetSetting("SERVERNOTICE", "DISABLE_TAX_RATE_CHANGE_NOTICE", ref MainMenu.DISABLE_TAX_RATE_CHANGE_NOTICE, ref this.DISABLE_TAX_RATE_CHANGE_NOTICE_TextBox);
				this.GetSetting("SERVERNOTICE", "TOWN_DROP_ITEM_NOTICE", ref MainMenu.TOWN_DROP_ITEM_NOTICE, ref this.TOWN_DROP_ITEM_NOTICE_TextBox);
				this.GetSetting("SERVERNOTICE", "DISCONNECT_NOTICE_NOTICE", ref MainMenu.DISCONNECT_NOTICE_NOTICE, ref this.DISCONNECT_NOTICE_TextBox);
				this.GetSetting("SERVERNOTICE", "BAN_NOTICE_NOTICE", ref MainMenu.BAN_NOTICE_NOTICE, ref this.BAN_NOTICE_TextBox);
				this.GetSetting("SERVERNOTICE", "GM_NOTICE_NOTICE", ref MainMenu.GM_NOTICE_NOTICE, ref this.GM_NOTICE_TextBox);
				this.GetSetting("SERVERNOTICE", "BLOCKSKILL_NOTICE_NOTICE", ref MainMenu.BLOCKSKILL_NOTICE_NOTICE, ref this.BLOCKSKILL_NOTICE_TextBox);
				this.GetSetting("SERVERNOTICE", "BLOCKZERKPVP_NOTICE", ref MainMenu.BLOCKZERKPVP_NOTICE, ref this.BLOCKZERKPVP_NOTICE_TextBox);
				this.GetSetting("SERVERNOTICE", "PULSNOTICE_NOTICE", ref MainMenu.PULSNOTICE_NOTICE, ref this.PULSNOTICE_NOTICE_TextBox);
				this.GetSetting("SERVERNOTICE", "PULSNOTICE_NOTICE_Start", ref MainMenu.PULSNOTICE_NOTICE_Start, ref this.PULSNOTICE_NOTICE_Start_TextBox);
				this.GetSetting("SERVERINFO", "SERVER_NAME", ref MainMenu.SERVER_NAME, ref this.SERVER_NAME_TEXTBOX);
				this.GetSetting("SERVERINFO", "SHARD_MAX_PLAYER", ref MainMenu.SHARD_MAX_PLAYER, ref this.SHARD_MAX_PLAYER_TEXTBOX);
				this.GetSetting("SERVERINFO", "CAPCHA", ref MainMenu.CAPCHA, ref this.CAPCHA_TEXTBOX);
				this.GetSetting("SERVERINFO", "AFKMS", ref MainMenu.AFKMS, ref this.AFKMS_TEXTBOX);
				this.GetSetting("SERVERINFO", "DISABLECAPCHA", ref MainMenu.DISABLECAPCHA, ref this.DISABLECAPCHA_CHECKBOX);
				this.GetSetting("CLIENTLESS", "CL_GT_IP", ref MainMenu.CLIENT_HOST_IP, ref this.CL_GT_IP);
				this.GetSetting("CLIENTLESS", "CL_GT_PORT", ref MainMenu.CLIENT_GW_PORT, ref this.CL_GT_PORT);
				this.GetSetting("CLIENTLESS", "CL_VER", ref MainMenu.CLIENT_VERSION, ref this.CL_VER);
				this.GetSetting("CLIENTLESS", "CL_LOCALE", ref MainMenu.CLIENT_LOCALE, ref this.CL_LOCALE);
				this.GetSetting("CLIENTLESS", "CL_ID", ref MainMenu.CLIENT_ID, ref this.CL_ID);
				this.GetSetting("CLIENTLESS", "CL_PASSWORD", ref MainMenu.CLIENT_PW, ref this.CL_PASSWORD);
				this.GetSetting("CLIENTLESS", "CL_CAPTCHA", ref MainMenu.CLIENT_CAPTCHA_VALUE, ref this.CL_CAPTCHA);
				this.GetSetting("CLIENTLESS", "CL_CHARNAME", ref MainMenu.CLIENT_CharName, ref this.CL_CHARNAME);
				this.GetSetting("EVENTS", "QNA_ENABLE", ref MainMenu.QNA_ENABLE, ref this.checkBox_QnAEnable);
				this.GetSetting("EVENTS", "QNA_TIMETOANSWER", ref MainMenu.QNA_TIMETOANSWER, ref this.textBox_QnATimeAnswer);
				this.GetSetting("EVENTS", "QNA_ROUNDS", ref MainMenu.QNA_ROUNDS, ref this.qnarounds);
				this.GetSetting("EVENTS", "QNA_ITEMREWARD", ref MainMenu.QNA_ITEMREWARD, ref this.itemcodetrivia);
				this.GetSetting("EVENTS", "QNA_ITEMCOUNT", ref MainMenu.QNA_ITEMCOUNT, ref this.questitemcount);
				this.GetSetting("EVENTS", "QNA_ITEMNAME", ref MainMenu.QNA_ITEMNAME, ref this.itemnametrivia);
				this.GetSetting("EVENTS", "QNA_ENABLE", ref MainMenu.QNA_ENABLE, ref this.checkBox_QnAEnable);
				this.GetSetting("EVENTS", "QNA_TIMETOANSWER", ref MainMenu.QNA_TIMETOANSWER, ref this.textBox_QnATimeAnswer);
				this.GetSetting("EVENTS", "QNA_ROUNDS", ref MainMenu.QNA_ROUNDS, ref this.qnarounds);
				this.GetSetting("EVENTS", "QNA_ITEMREWARD", ref MainMenu.QNA_ITEMREWARD, ref this.itemcodetrivia);
				this.GetSetting("EVENTS", "QNA_ITEMCOUNT", ref MainMenu.QNA_ITEMCOUNT, ref this.questitemcount);
				this.GetSetting("EVENTS", "QNA_ITEMNAME", ref MainMenu.QNA_ITEMNAME, ref this.itemnametrivia);
				this.GetSetting("EVENTS", "QNA_ENABLE", ref MainMenu.QNA_ENABLE, ref this.checkBox_QnAEnable);
				this.GetSetting("EVENTS", "QNA_TIMETOANSWER", ref MainMenu.QNA_TIMETOANSWER, ref this.textBox_QnATimeAnswer);
				this.GetSetting("EVENTS", "QNA_ROUNDS", ref MainMenu.QNA_ROUNDS, ref this.qnarounds);
				this.GetSetting("EVENTS", "QNA_ITEMREWARD", ref MainMenu.QNA_ITEMREWARD, ref this.itemcodetrivia);
				this.GetSetting("EVENTS", "QNA_ITEMCOUNT", ref MainMenu.QNA_ITEMCOUNT, ref this.questitemcount);
				this.GetSetting("EVENTS", "QNA_ITEMNAME", ref MainMenu.QNA_ITEMNAME, ref this.itemnametrivia);
				this.GetSetting("EVENTS", "HNS_ENABLE", ref MainMenu.HNS_ENABLE, ref this.HNS_ENABLEBOX);
				this.GetSetting("EVENTS", "HNS_TIMETOSEARCH", ref MainMenu.HNS_TIMETOSEARCH, ref this.HNS_TIMETOSEARCHBOX);
				this.GetSetting("EVENTS", "HNS_ROUNDS", ref MainMenu.HNS_ROUNDS, ref this.HNS_ROUNDSBOX);
				this.GetSetting("EVENTS", "HNS_ITEMREWARD", ref MainMenu.HNS_ITEMREWARD, ref this.HNS_ITEMREWARDBOX);
				this.GetSetting("EVENTS", "HNS_ITEMCOUNT", ref MainMenu.HNS_ITEMCOUNT, ref this.HNS_ITEMCOUNTBOX);
				this.GetSetting("EVENTS", "HNS_ITEMNAME", ref MainMenu.HNS_ITEMNAME, ref this.HNS_ITEMNAMEBOX);
				this.GetSetting("EVENTS", "GMK_ENABLE", ref MainMenu.GMK_ENABLE, ref this.GMK_ENABLEBOX);
				this.GetSetting("EVENTS", "GMK_TIMETOWAIT", ref MainMenu.GMK_TIMETOWAIT, ref this.GMK_TIMETOWAITBOX);
				this.GetSetting("EVENTS", "GMK_ROUND", ref MainMenu.GMK_ROUND, ref this.GMK_ROUNDBOX);
				this.GetSetting("EVENTS", "GMK_ITEMID", ref MainMenu.GMK_ITEMID, ref this.GMK_ITEMIDBOX);
				this.GetSetting("EVENTS", "GMK_ITEMCOUNT", ref MainMenu.GMK_ITEMCOUNT, ref this.GMK_ITEMCOUNTBOX);
				this.GetSetting("EVENTS", "GMK_ITEMNAME", ref MainMenu.GMK_ITEMNAME, ref this.GMK_ITEMNAMEBOX);
				this.GetSetting("EVENTS", "GMK_REGIONID", ref MainMenu.GMK_REGIONID, ref this.GMK_REGIONIDBOX);
				this.GetSetting("EVENTS", "GMK_POSX", ref MainMenu.GMK_POSX, ref this.GMK_POSXBOX);
				this.GetSetting("EVENTS", "GMK_POSY", ref MainMenu.GMK_POSY, ref this.GMK_POSYBOX);
				this.GetSetting("EVENTS", "GMK_POSZ", ref MainMenu.GMK_POSZ, ref this.GMK_POSZBOX);
				this.GetSetting("EVENTS", "SND_ENABLE", ref MainMenu.SND_ENABLE, ref this.SND_ENABLEBOX);
				this.GetSetting("EVENTS", "SND_TIMETOSEARCH", ref MainMenu.SND_TIMETOSEARCH, ref this.SND_TIMETOSEARCHBOX);
				this.GetSetting("EVENTS", "SND_ROUNDS", ref MainMenu.SND_ROUNDS, ref this.SND_ROUNDSBOX);
				this.GetSetting("EVENTS", "SND_ITEMREWARD", ref MainMenu.SND_ITEMREWARD, ref this.SND_ITEMREWARDBOX);
				this.GetSetting("EVENTS", "SND_ITEMCOUNT", ref MainMenu.SND_ITEMCOUNT, ref this.SND_ITEMCOUNTBOX);
				this.GetSetting("EVENTS", "SND_ITEMNAME", ref MainMenu.SND_ITEMNAME, ref this.SND_ITEMNAMEBOX);
				this.GetSetting("EVENTS", "SND_MOBID", ref MainMenu.SND_MOBID, ref this.SND_MOBIDBOX);
				this.GetSetting("EVENTS", "LG_ENABLE", ref MainMenu.LG_ENABLE, ref this.LG_ENABLEBOX);
				this.GetSetting("EVENTS", "LG_TIMETOWAIT", ref MainMenu.LG_TIMETOWAIT, ref this.LG_TIMETOWAITBOX);
				this.GetSetting("EVENTS", "LG_TICKETPRICE", ref MainMenu.LG_TICKETPRICE, ref this.LG_TICKETPRICEBOX);
				this.GetSetting("EVENTS", "LG_ROUND", ref MainMenu.LG_ROUND, ref this.LG_ROUNDBOX);
				this.GetSetting("EVENTS", "LPN_ENABLE", ref MainMenu.LPN_ENABLE, ref this.LPN_ENABLEBOX);
				this.GetSetting("EVENTS", "LPN_TIMETOWAIT", ref MainMenu.LPN_TIMETOWAIT, ref this.LPN_TIMETOWAITBOX);
				this.GetSetting("EVENTS", "LPN_ROUNDS", ref MainMenu.LPN_ROUNDS, ref this.LPN_ROUNDSBOX);
				this.GetSetting("EVENTS", "LPN_ITEMREWARD", ref MainMenu.LPN_ITEMREWARD, ref this.LPN_ITEMREWARDBOX);
				this.GetSetting("EVENTS", "LPN_ITEMCOUNT", ref MainMenu.LPN_ITEMCOUNT, ref this.LPN_ITEMCOUNTBOX);
				this.GetSetting("EVENTS", "LPN_ITEMNAME", ref MainMenu.LPN_ITEMNAME, ref this.LPN_ITEMNAMEBOX);
				this.GetSetting("EVENTS", "PT_MINVALUE", ref MainMenu.PT_MINVALUE, ref this.PT_MINVALUEBOX);
				this.GetSetting("EVENTS", "PT_MAXVALUE", ref MainMenu.PT_MAXVALUE, ref this.PT_MAXVALUEBOX);
				this.GetSetting("EVENTS", "LMS_ENABLE", ref MainMenu.LMS_ENABLE, ref this.LMS_ENABLEBOX);
				this.GetSetting("EVENTS", "LMS_PCLIMIT", ref MainMenu.LMS_PCLIMIT, ref this.LMS_PCLIMITBOX);
				this.GetSetting("EVENTS", "LMS_MATCHTIME", ref MainMenu.LMS_MATCHTIME, ref this.LMS_MATCHTIMEBOX);
				this.GetSetting("EVENTS", "LMS_REGISTERTIME", ref MainMenu.LMS_REGISTERTIME, ref this.LMS_REGISTERTIMEBOX);
				this.GetSetting("EVENTS", "LMS_REQUIRELEVEL", ref MainMenu.LMS_REQUIRELEVEL, ref this.LMS_REQUIRELEVELBOX);
				this.GetSetting("EVENTS", "LMS_ITEMID", ref MainMenu.LMS_ITEMID, ref this.LMS_ITEMIDBOX);
				this.GetSetting("EVENTS", "LMS_ITEMCOUNT", ref MainMenu.LMS_ITEMCOUNT, ref this.LMS_ITEMCOUNTBOX);
				this.GetSetting("EVENTS", "LMS_ITEMNAME", ref MainMenu.LMS_ITEMNAME, ref this.LMS_ITEMNAMEBOX);
				this.GetSetting("EVENTS", "LMS_GATEID", ref MainMenu.LMS_GATEID, ref this.LMS_GATEIDBOX);
				this.GetSetting("EVENTS", "LMS_REGIONID", ref MainMenu.LMS_REGIONID, ref this.LMS_REGIONIDBOX);
				this.GetSetting("EVENTS", "LMS_REGIONID2", ref MainMenu.LMS_REGIONID2, ref this.LMS_REGIONIDBOX2);
				this.GetSetting("EVENTS", "LMS_GATEWAIT_TIME", ref MainMenu.LMS_GATEWAIT_TIME, ref this.LMS_GATEWAIT_TIMEBOX);
				this.GetSetting("EVENTS", "SURV_ENABLE", ref MainMenu.SURV_ENABLE, ref this.SURV_ENABLEBOX);
				this.GetSetting("EVENTS", "SURV_MATCHTIME", ref MainMenu.SURV_MATCHTIME, ref this.SURV_MATCHTIMEBOX);
				this.GetSetting("EVENTS", "SURV_REGISTERTIME", ref MainMenu.SURV_REGISTERTIME, ref this.SURV_REGISTERTIMEBOX);
				this.GetSetting("EVENTS", "SURV_REQUIRELEVEL", ref MainMenu.SURV_REQUIRELEVEL, ref this.SURV_REQUIRELEVELBOX);
				this.GetSetting("EVENTS", "SURV_ITEMID", ref MainMenu.SURV_ITEMID, ref this.SURV_ITEMIDBOX);
				this.GetSetting("EVENTS", "SURV_ITEMCOUNT", ref MainMenu.SURV_ITEMCOUNT, ref this.SURV_ITEMCOUNTBOX);
				this.GetSetting("EVENTS", "SURV_ITEMNAME", ref MainMenu.SURV_ITEMNAME, ref this.SURV_ITEMNAMEBOX);
				this.GetSetting("EVENTS", "SURV_GATEID", ref MainMenu.SURV_GATEID, ref this.SURV_GATEIDBOX);
				this.GetSetting("EVENTS", "SURV_REGIONID", ref MainMenu.SURV_REGIONID, ref this.SURV_REGIONIDBOX);
				this.GetSetting("EVENTS", "SURV_REGIONID2", ref MainMenu.SURV_REGIONID2, ref this.SURV_REGIONIDBOX2);
				this.GetSetting("EVENTS", "SURV_GATEWAIT_TIME", ref MainMenu.SURV_GATEWAIT_TIME, ref this.SURV_GATEWAIT_TIMEBOX);
				this.GetSetting("EVENTSNOTICE", "QNA_STARTNOTICE", ref MainMenu.QNA_STARTNOTICE, ref this.QNA_STARTNOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "QNA_INFONOTICE", ref MainMenu.QNA_INFONOTICE, ref this.QNA_INFONOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "QNA_INFOCHAR", ref MainMenu.QNA_INFOCHAR, ref this.QNA_INFOCHARBOX);
				this.GetSetting("EVENTSNOTICE", "QNA_END", ref MainMenu.QNA_END, ref this.QNA_ENDBOX);
				this.GetSetting("EVENTSNOTICE", "QNA_WIN", ref MainMenu.QNA_WIN, ref this.QNA_WINBOX);
				this.GetSetting("EVENTSNOTICE", "QNA_ROUNDINFO", ref MainMenu.QNA_ROUNDINFO, ref this.QNA_ROUNDINFOBOX);
				this.GetSetting("EVENTSNOTICE", "HNS_STARTNOTICE", ref MainMenu.HNS_STARTNOTICE, ref this.HNS_STARTNOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "HNS_INFONOTICE", ref MainMenu.HNS_INFONOTICE, ref this.HNS_INFONOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "HNS_PLACEINFO", ref MainMenu.HNS_PLACEINFO, ref this.HNS_PLACEINFOBOX);
				this.GetSetting("EVENTSNOTICE", "HNS_END", ref MainMenu.HNS_END, ref this.HNS_ENDBOX);
				this.GetSetting("EVENTSNOTICE", "HNS_WIN", ref MainMenu.HNS_WIN, ref this.HNS_WINBOX);
				this.GetSetting("EVENTSNOTICE", "GMK_PLACENAME", ref MainMenu.GMK_PLACENAME, ref this.GMK_PLACENAMEBOX);
				this.GetSetting("EVENTSNOTICE", "GMK_START_NOTICE", ref MainMenu.GMK_START_NOTICE, ref this.GMK_START_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "GMK_INFO_NOTICE", ref MainMenu.GMK_INFO_NOTICE, ref this.GMK_INFO_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "GMK_INFORM_NOTICE", ref MainMenu.GMK_INFORM_NOTICE, ref this.GMK_INFORM_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "GMK_WIN_NOTICE", ref MainMenu.GMK_WIN_NOTICE, ref this.GMK_WIN_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "GMK_END_NOTICE", ref MainMenu.GMK_END_NOTICE, ref this.GMK_END_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "SND_STARTNOTICE", ref MainMenu.SND_STARTNOTICE, ref this.SND_STARTNOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "SND_INFONOTICE", ref MainMenu.SND_INFONOTICE, ref this.SND_INFONOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "SND_PLACEINFO", ref MainMenu.SND_PLACEINFO, ref this.SND_PLACEINFOBOX);
				this.GetSetting("EVENTSNOTICE", "SND_END", ref MainMenu.SND_END, ref this.SND_ENDBOX);
				this.GetSetting("EVENTSNOTICE", "SND_WIN", ref MainMenu.SND_WIN, ref this.SND_WINBOX);
				this.GetSetting("EVENTSNOTICE", "LG_START_NOTICE", ref MainMenu.LG_START_NOTICE, ref this.LG_START_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "LG_TICKETPRICE_NOTICE", ref MainMenu.LG_TICKETPRICE_NOTICE, ref this.LG_TICKETPRICE_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "LG_END_NOTICE", ref MainMenu.LG_END_NOTICE, ref this.LG_END_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "LG_WIN_NOTICE", ref MainMenu.LG_WIN_NOTICE, ref this.LG_WIN_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "LG_STARTREG_NOTICE", ref MainMenu.LG_STARTREG_NOTICE, ref this.LG_STARTREG_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "LG_STOPREG_NOTICE", ref MainMenu.LG_STOPREG_NOTICE, ref this.LG_STOPREG_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "LG_GOLDREQUIRE_NOTICE", ref MainMenu.LG_GOLDREQUIRE_NOTICE, ref this.LG_GOLDREQUIRE_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "LG_REGISTERSUCCESS_NOTICE", ref MainMenu.LG_REGISTERSUCCESS_NOTICE, ref this.LG_REGISTERSUCCESS_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "LG_REGISTED_NOTICE", ref MainMenu.LG_REGISTED_NOTICE, ref this.LG_REGISTED_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "LG_ENDD_NOTICE", ref MainMenu.LG_ENDD_NOTICE, ref this.LG_ENDD_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "LPN_START_NOTICE", ref MainMenu.LPN_START_NOTICE, ref this.LPN_START_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "LPN_INFO", ref MainMenu.LPN_INFO, ref this.LPN_INFOBOX);
				this.GetSetting("EVENTSNOTICE", "LPN_WIN_NOTICE", ref MainMenu.LPN_WIN_NOTICE, ref this.LPN_WIN_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "LPN_NOREFORM_NOTICE", ref MainMenu.LPN_NOREFORM_NOTICE, ref this.LPN_NOREFORM_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "LPN_END", ref MainMenu.LPN_END, ref this.LPN_ENDBOX);
				this.GetSetting("EVENTSNOTICE", "LPN_ROUNDINFO", ref MainMenu.LPN_ROUNDINFO, ref this.LPN_ROUNDINFOBOX);
				this.GetSetting("EVENTSNOTICE", "LMS_START_NOTICE", ref MainMenu.LMS_START_NOTICE, ref this.LMS_START_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "LMS_REGISTERTIME_NOTICE", ref MainMenu.LMS_REGISTERTIME_NOTICE, ref this.LMS_REGISTERTIME_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "LMS_REGISTERCLOSE_NOTICE", ref MainMenu.LMS_REGISTERCLOSE_NOTICE, ref this.LMS_REGISTERCLOSE_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "LMS_INFORM_NOTICE", ref MainMenu.LMS_INFORM_NOTICE, ref this.LMS_INFORM_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "LMS_GATEOPEN_NOTICE", ref MainMenu.LMS_GATEOPEN_NOTICE, ref this.LMS_GATEOPEN_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "LMS_GATECLOSE_NOTICE", ref MainMenu.LMS_GATECLOSE_NOTICE, ref this.LMS_GATECLOSE_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "LMS_CANCELEVENT_NOTICE", ref MainMenu.LMS_CANCELEVENT_NOTICE, ref this.LMS_CANCELEVENT_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "LMS_INFO2_NOTICE", ref MainMenu.LMS_INFO2_NOTICE, ref this.LMS_INFO2_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "LMS_WIN_NOTICE", ref MainMenu.LMS_WIN_NOTICE, ref this.LMS_WIN_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "LMS_END_NOTICE", ref MainMenu.LMS_END_NOTICE, ref this.LMS_END_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "LMS_REGISTED_NOTICE", ref MainMenu.LMS_REGISTED_NOTICE, ref this.LMS_REGISTED_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "LMS_REGISTERSUCCESS_NOTICE", ref MainMenu.LMS_REGISTERSUCCESS_NOTICE, ref this.LMS_REGISTERSUCCESS_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "LMS_REQUIRELEVEL_NOTICE", ref MainMenu.LMS_REQUIRELEVEL_NOTICE, ref this.LMS_REQUIRELEVEL_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "LMS_JOB_NOTICE", ref MainMenu.LMS_JOB_NOTICE, ref this.LMS_JOB_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "LMS_ELIMINATED_NOTICE", ref MainMenu.LMS_ELIMINATED_NOTICE, ref this.LMS_ELIMINATED_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "LMS_FIGHTSTART_NOTICE", ref MainMenu.LMS_FIGHTSTART_NOTICE, ref this.LMS_FIGHTSTART_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "SURV_START_NOTICE", ref MainMenu.SURV_START_NOTICE, ref this.SURV_START_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "SURV_REGISTERTIME_NOTICE", ref MainMenu.SURV_REGISTERTIME_NOTICE, ref this.SURV_REGISTERTIME_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "SURV_REGISTERCLOSE_NOTICE", ref MainMenu.SURV_REGISTERCLOSE_NOTICE, ref this.SURV_REGISTERCLOSE_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "SURV_INFORM_NOTICE", ref MainMenu.SURV_INFORM_NOTICE, ref this.SURV_INFORM_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "SURV_GATEOPEN_NOTICE", ref MainMenu.SURV_GATEOPEN_NOTICE, ref this.SURV_GATEOPEN_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "SURV_GATECLOSE_NOTICE", ref MainMenu.SURV_GATECLOSE_NOTICE, ref this.SURV_GATECLOSE_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "SURV_CANCELEVENT_NOTICE", ref MainMenu.SURV_CANCELEVENT_NOTICE, ref this.SURV_CANCELEVENT_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "SURV_INFO2_NOTICE", ref MainMenu.SURV_INFO2_NOTICE, ref this.SURV_INFO2_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "SURV_WIN_NOTICE", ref MainMenu.SURV_WIN_NOTICE, ref this.SURV_WIN_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "SURV_END_NOTICE", ref MainMenu.SURV_END_NOTICE, ref this.SURV_END_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "SURV_REGISTED_NOTICE", ref MainMenu.SURV_REGISTED_NOTICE, ref this.SURV_REGISTED_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "SURV_REGISTERSUCCESS_NOTICE", ref MainMenu.SURV_REGISTERSUCCESS_NOTICE, ref this.SURV_REGISTERSUCCESS_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "SURV_REQUIRELEVEL_NOTICE", ref MainMenu.SURV_REQUIRELEVEL_NOTICE, ref this.SURV_REQUIRELEVEL_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "SURV_JOB_NOTICE", ref MainMenu.SURV_JOB_NOTICE, ref this.SURV_JOB_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "SURV_ELIMINATED_NOTICE", ref MainMenu.SURV_ELIMINATED_NOTICE, ref this.SURV_ELIMINATED_NOTICEBOX);
				this.GetSetting("EVENTSNOTICE", "SURV_FIGHTSTART_NOTICE", ref MainMenu.SURV_FIGHTSTART_NOTICE, ref this.SURV_FIGHTSTART_NOTICEBOX);
				this.GetSetting("GUISETTINGS", "ENABLE_CHEST", ref MainMenu.ChestWnd, ref this.ENABLE_CHEST);
				this.GetSetting("GUISETTINGS", "ENABLE_ATTENDANCE", ref MainMenu.AttendanceWndCheck, ref this.ENABLE_ATTENDANCE);
				this.GetSetting("GUISETTINGS", "ENABLE_ATTENDANCE_MON", ref MainMenu.AttendanceMon, ref this.ATTENDANCE_comboBox);
				this.GetSetting("GUISETTINGS", "ENABLE_EVENTREGISTER", ref MainMenu.EventRegWnd, ref this.ENABLE_EVNTREG);
				this.GetSetting("GUISETTINGS", "ENABLE_FACEBOOK", ref MainMenu.FacebookWnd, ref this.ENABLE_FB);
				this.GetSetting("GUISETTINGS", "ENABLE_DISCORD", ref MainMenu.DiscordWnd, ref this.ENABLE_DC);
				this.GetSetting("GUISETTINGS", "DISCORD_ID", ref MainMenu.DiscordInstanceID, ref this.DCID);
				this.GetSetting("GUISETTINGS", "ENABLE_NEWREVERSE", ref MainMenu.NewReverse, ref this.checkBoxNewRew);
				this.GetSetting("GUISETTINGS", "ENABLE_OLDMAINPOPUP", ref MainMenu.OldMainPopup, ref this.checkBox1oldmain);
				this.GetSetting("GUISETTINGS", "ENABLE_ITEMCOMPARISON", ref MainMenu.ItemComparison, ref this.checkBox1itemcomp);
				this.GetSetting("GUISETTINGS", "DISCORD_URL", ref MainMenu.DiscordURL, ref this.DiscordSite);
				this.GetSetting("GUISETTINGS", "FACEBOOK_URL", ref MainMenu.FacebookURL, ref this.facebooksite);
				this.GetSetting("GUISETTINGS", "MASTERY_LIMIT", ref MainMenu.MasteryLimit, ref this.MaxMastery);
				this.GetSetting("GUISETTINGS", "PT_MAX_LIMIT", ref MainMenu.MaxPartyLevelLimit, ref this.MaxPtNoLimit);
				this.GetSetting("GUISETTINGS", "ENABLE_PERMANENTALCHEMY", ref MainMenu.PermanenyAlchemy, ref this.ENABLE_PALCHEMY);
				this.GetSetting("GUISETTINGS", "ENABLE_GUILD_JOBMODE", ref MainMenu.GuildJobMode, ref this.GUILDJOB);
				this.GetSetting("GUISETTINGS", "ENABLE_MARKET", ref MainMenu.EnableMarket, ref this.marketbutton);
				this.GetSetting("GUISETTINGS", "ENABLE_TOKEN", ref MainMenu.EnableMarketToken, ref this.tokenbox);
				this.GetSetting("GUISETTINGS", "ENABLE_SILK", ref MainMenu.EnableMarketSilk, ref this.silksystembox);
				this.GetSetting("GUISETTINGS", "ENABLE_GOLD", ref MainMenu.EnableMarketGold, ref this.goldboxs);
				this.GetSetting("GUISETTINGS", "CUSTOMTITLEPRICE", ref MainMenu.CustomTitlePrice, ref this.titleprices);
				this.GetSetting("GUISETTINGS", "CUSTOMTITLEPRICEBRM", ref MainMenu.CustomTitleBirim, ref this.titlepricebirim);
				MainMenu.AttendanceWnd = true;
			}
		}
		catch
		{
		}
	}

	private void SaveSettings()
	{
		try
		{
			if (File.Exists(Application.StartupPath + "\\Config/settings.ini"))
			{
				this.UpdateSetting("LISENSCONFIG", "LisansCharName", ref MainMenu.LisansPassword, ref this.TextBoxLisansUserName);
				this.UpdateSetting("LISENSCONFIG", "LisansPassword", ref MainMenu.LisansPassword, ref this.TextBoxLisansPassword);
				this.UpdateSetting("LISENSCONFIG", "LisansAuto", ref MainMenu.LisansAuto, ref this.LisansAutoCheckBox);
				this.UpdateSetting("SERVERNOTICE", "JOB_REVERSE_BLOCK", ref MainMenu.JobBlockReversed, ref this.JobReverseBlock);
				this.UpdateSetting("GENERAL", "SQL_HOST", ref MainMenu.SQL_HOST, ref this.textBox_SQLHost);
				this.UpdateSetting("GENERAL", "SQL_USER", ref MainMenu.SQL_USER, ref this.textBox_SQLId);
				this.UpdateSetting("GENERAL", "SQL_PASS", ref MainMenu.SQL_PASS, ref this.textBox_SQLPass);
				this.UpdateSetting("GENERAL", "ACC_DB", ref MainMenu.ACC_DB, ref this.textBox_AccDB);
				this.UpdateSetting("GENERAL", "SHA_DB", ref MainMenu.SHA_DB, ref this.textBox_ShardDB);
				this.UpdateSetting("GENERAL", "LOG_DB", ref MainMenu.LOG_DB, ref this.textBox_LogDB);
				this.UpdateSetting("GENERAL", "FILTER_DB", ref MainMenu.FILTER_DB, ref this.textBox_FilterDB);
				this.UpdateSetting("SERVERSETTINGS", "Proxy_IP", ref MainMenu.Proxy_IP, ref this.textBox_Froxy_IP);
				this.UpdateSetting("SERVERSETTINGS", "Server_IP", ref MainMenu.Server_IP, ref this.textBox_Server_IP);
				this.UpdateSetting("SERVERSETTINGS", "Download_Public_port", ref MainMenu.Download_Public_port, ref this.textBox_PublicDownloadPort);
				this.UpdateSetting("SERVERSETTINGS", "Download_Server_port", ref MainMenu.Download_Server_port, ref this.textBox_RealDownloadPort);
				this.UpdateSetting("SERVERSETTINGS", "Gateway_Public_port", ref MainMenu.Gateway_Public_port, ref this.textBox_PublicGatewayPort);
				this.UpdateSetting("SERVERSETTINGS", "Gateway_Server_port", ref MainMenu.Gateway_Server_port, ref this.textBox_RealGatewayPort);
				this.UpdateSetting("SERVERSETTINGS", "Agent_Public_port", ref MainMenu.Agent_Public_port, ref this.textBox_PublicAgentPort);
				this.UpdateSetting("SERVERSETTINGS", "Agent_Server_port", ref MainMenu.Agent_Server_port, ref this.textBox_RealAgentPort);
				this.UpdateSetting("PROTECTION", "GATEWAY_PACKET_RESET", ref MainMenu.GATEWAY_PACKET_RESET, ref this.GATEWAY_PACKET_RESET_TextBox);
				this.UpdateSetting("PROTECTION", "AGENT_PACKET_RESET", ref MainMenu.AGENT_PACKET_RESET, ref this.AGENT_PACKET_RESET_TextBox);
				this.UpdateSetting("PROTECTION", "DOWNLOAD_PACKET_RESET", ref MainMenu.DOWNLOAD_PACKET_RESET, ref this.DOWNLOAD_PACKET_RESET_TextBox);
				this.UpdateSetting("PROTECTION", "GW_BPS_VALUE", ref MainMenu.GW_BPS_VALUE, ref this.GW_BPS_VALUE_TextBox);
				this.UpdateSetting("PROTECTION", "AG_BPS_VALUE", ref MainMenu.AG_BPS_VALUE, ref this.AG_BPS_VALUE_TextBox);
				this.UpdateSetting("PROTECTION", "GW_PPS_VALUE", ref MainMenu.GW_PPS_VALUE, ref this.GW_PPS_VALUE_TextBox);
				this.UpdateSetting("PROTECTION", "AG_PPS_VALUE", ref MainMenu.AG_PPS_VALUE, ref this.AG_PPS_VALUE_TextBox);
				this.UpdateSetting("PROTECTION", "DW_BPS_VALUE", ref MainMenu.DW_BPS_VALUE, ref this.DW_BPS_VALUE_TextBox);
				this.UpdateSetting("PROTECTION", "DW_PPS_VALUE", ref MainMenu.DW_PPS_VALUE, ref this.DW_PPS_VALUE_TextBox);
				this.UpdateSetting("PROTECTION", "FLOOD_COUNT", ref MainMenu.FLOOD_COUNT, ref this.FLOOD_COUNT_TextBox);
				this.UpdateSetting("PROTECTION", "PACKET_CHECK", ref MainMenu.PACKET_CHECK, ref this.PACKET_CHECKBOX);
				this.UpdateSetting("PROTECTION", "FIREWALLBANCHECK", ref MainMenu.FIREWALLBANCHECK, ref this.FIREWALLBANCHECKBOX);
				this.UpdateSetting("PROTECTION", "MAINTENANCE", ref MainMenu.MAINTENANCE, ref this.MAINTENANCE_CHECKBOX);
				this.UpdateSetting("PROTECTION", "OpCodeWhiteList", ref MainMenu.OpCodeWhiteList, ref this.WhiteList_CheckBox);
				this.UpdateSetting("LIMITS", "IP_LIMIT", ref MainMenu.IP_LIMIT, ref this.IP_LIMITTextBox);
				this.UpdateSetting("LIMITS", "PC_LIMIT", ref MainMenu.PC_LIMIT, ref this.PC_LIMITTextBox);
				this.UpdateSetting("LIMITS", "BA_PC_LIMIT", ref MainMenu.BA_PC_LIMIT, ref this.BA_PC_LIMITTextBox);
				this.UpdateSetting("LIMITS", "CTF_PC_LIMIT", ref MainMenu.CTF_PC_LIMIT, ref this.CTF_PC_LIMITTextBox);
				this.UpdateSetting("LIMITS", "FTW_PC_LIMIT", ref MainMenu.FTW_PC_LIMIT, ref this.FTW_PC_LIMITTextBox);
				this.UpdateSetting("LIMITS", "JOB_PC_LIMIT", ref MainMenu.JOB_PC_LIMIT, ref this.JOB_PC_LIMITTextBox);
				this.UpdateSetting("LIMITS", "HT_PC_LIMIT", ref MainMenu.HT_PC_LIMIT, ref this.HT_PC_LIMITTextBox);
				this.UpdateSetting("LIMITS", "JOBT_PC_LIMIT", ref MainMenu.JOBT_PC_LIMIT, ref this.JOBT_PC_LIMITTextBox);
				this.UpdateSetting("LIMITS", "FGW_PC_LIMIT", ref MainMenu.FGW_PC_LIMIT, ref this.FGW_PC_LIMITTextBox);
				this.UpdateSetting("LIMITS", "JUPITER_PC_LIMIT", ref MainMenu.JUPITER_PC_LIMIT, ref this.JUPITER_PC_LIMITTextBox);
				this.UpdateSetting("LIMITS", "SURVIVAL_PC_LIMIT", ref MainMenu.SURVIVAL_PC_LIMIT, ref this.SURVIVAL_PC_LIMITTextBox);
				this.UpdateSetting("LIMITS", "CAFE_IP_LIMIT", ref MainMenu.CAFE_IP_LIMIT, ref this.CAFE_IP_LIMITTextBox);
				this.UpdateSetting("LIMITS", "PLUS_LIMIT", ref MainMenu.PLUS_LIMIT, ref this.PLUS_LIMITTextBox);
				this.UpdateSetting("LIMITS", "DEVIL_PLUS_LIMIT", ref MainMenu.DEVIL_PLUS_LIMIT, ref this.DEVIL_PLUS_LIMITTextBox);
				this.UpdateSetting("LIMITS", "IP_LIMIT_NOTICE", ref MainMenu.IP_LIMIT_NOTICE, ref this.CAFE_IP_LIMITNoticeTextBox);
				this.UpdateSetting("LIMITS", "PC_LIMIT_NOTICE", ref MainMenu.PC_LIMIT_NOTICE, ref this.PC_LIMITNoticeTextBox);
				this.UpdateSetting("LIMITS", "BA_PC_LIMIT_NOTICE", ref MainMenu.BA_PC_LIMIT_NOTICE, ref this.BA_PC_LIMITNoticeTextBox);
				this.UpdateSetting("LIMITS", "CTF_PC_LIMIT_NOTICE", ref MainMenu.CTF_PC_LIMIT_NOTICE, ref this.CTF_PC_LIMITNoticeTextBox);
				this.UpdateSetting("LIMITS", "FTW_PC_LIMIT_NOTICE", ref MainMenu.FTW_PC_LIMIT_NOTICE, ref this.FTW_PC_LIMITNoticeTextBox);
				this.UpdateSetting("LIMITS", "JOB_PC_LIMIT_NOTICE", ref MainMenu.JOB_PC_LIMIT_NOTICE, ref this.JOB_PC_LIMITNoticeTextBox);
				this.UpdateSetting("LIMITS", "HT_PC_LIMIT_NOTICE", ref MainMenu.HT_PC_LIMIT_NOTICE, ref this.HT_PC_LIMITNoticeTextBox);
				this.UpdateSetting("LIMITS", "JOBT_PC_LIMIT_NOTICE", ref MainMenu.JOBT_PC_LIMIT_NOTICE, ref this.JOBT_PC_LIMITNoticeTextBox);
				this.UpdateSetting("LIMITS", "FGW_PC_LIMIT_NOTICE", ref MainMenu.FGW_PC_LIMIT_NOTICE, ref this.FGW_PC_LIMITNoticeTextBox);
				this.UpdateSetting("LIMITS", "JUPITER_PC_LIMIT_NOTICE", ref MainMenu.JUPITER_PC_LIMIT_NOTICE, ref this.JUPITER_PC_LIMITNoticeTextBox);
				this.UpdateSetting("LIMITS", "SURVIVAL_PC_LIMIT_NOTICE", ref MainMenu.SURVIVAL_PC_LIMIT_NOTICE, ref this.SURVIVAL_PC_LIMITNoticeTextBox);
				this.UpdateSetting("LIMITS", "CAFE_IP_LIMI_NOTICET", ref MainMenu.CAFE_IP_LIMIT_NOTICE, ref this.CAFE_IP_LIMITNoticeTextBox);
				this.UpdateSetting("LIMITS", "PLUS_LIMIT_NOTICE", ref MainMenu.PLUS_LIMIT_NOTICE, ref this.PLUS_LIMITNoticeTextBox);
				this.UpdateSetting("LIMITS", "DEVIL_PLUS_LIMIT_NOTICE", ref MainMenu.DEVIL_PLUS_LIMIT_NOTICE, ref this.DEVIL_PLUS_LIMITNoticeTextBox);
				this.UpdateSetting("DELAYS", "EXCHANGE_DELAY", ref MainMenu.EXCHANGE_DELAY, ref this.EXCHANGE_DELAY_TextBox);
				this.UpdateSetting("DELAYS", "EXIT_DELAY", ref MainMenu.EXIT_DELAY, ref this.EXIT_DELAY_TextBox);
				this.UpdateSetting("DELAYS", "GLOBAL_DELAY", ref MainMenu.GLOBAL_DELAY, ref this.GLOBAL_DELAY_TextBox);
				this.UpdateSetting("DELAYS", "GUILD_DELAY", ref MainMenu.GUILD_DELAY, ref this.GUILD_DELAY_TextBox);
				this.UpdateSetting("DELAYS", "RESTART_DELAY", ref MainMenu.RESTART_DELAY, ref this.RESTART_DELAY_TextBox);
				this.UpdateSetting("DELAYS", "STALL_DELAY", ref MainMenu.STALL_DELAY, ref this.STALL_DELAY_TextBox);
				this.UpdateSetting("DELAYS", "UNION_DELAY", ref MainMenu.UNION_DELAY, ref this.UNION_DELAY_TextBox);
				this.UpdateSetting("DELAYS", "ZERK_DELAY", ref MainMenu.ZERK_DELAY, ref this.ZERK_DELAY_TextBox);
				this.UpdateSetting("DELAYS", "REVERSE_DELAY", ref MainMenu.REVERSE_DELAY, ref this.REVERSE_DELAY_TextBox);
				this.UpdateSetting("DELAYS", "EXCHANGE_DELAY_NOTICE", ref MainMenu.EXCHANGE_DELAY_NOTICE, ref this.EXCHANGE_DELAY_NOTICE_TextBox);
				this.UpdateSetting("DELAYS", "EXIT_DELAY_NOTICE", ref MainMenu.EXIT_DELAY_NOTICE, ref this.EXIT_DELAY_NOTICE_TextBox);
				this.UpdateSetting("DELAYS", "GLOBAL_DELAY_NOTICE", ref MainMenu.GLOBAL_DELAY_NOTICE, ref this.GLOBAL_DELAY_NOTICE_TextBox);
				this.UpdateSetting("DELAYS", "GUILD_DELAY_NOTICE", ref MainMenu.GUILD_DELAY_NOTICE, ref this.GUILD_DELAY_NOTICE_TextBox);
				this.UpdateSetting("DELAYS", "RESTART_DELAY_NOTICE", ref MainMenu.RESTART_DELAY_NOTICE, ref this.RESTART_DELAY_NOTICE_TextBox);
				this.UpdateSetting("DELAYS", "STALL_DELAY_NOTICE", ref MainMenu.STALL_DELAY_NOTICE, ref this.STALL_DELAY_NOTICE_TextBox);
				this.UpdateSetting("DELAYS", "UNION_DELAY_NOTICE", ref MainMenu.UNION_DELAY_NOTICE, ref this.UNION_DELAY_NOTICE_TextBox);
				this.UpdateSetting("DELAYS", "ZERK_DELAY_NOTICE", ref MainMenu.ZERK_DELAY_NOTICE, ref this.ZERK_DELAY_NOTICE_TextBox);
				this.UpdateSetting("DELAYS", "REVERSE_DELAY_NOTICE", ref MainMenu.REVERSE_DELAY_NOTICE, ref this.REVERSE_DELAY_NOTICE_TextBox);
				this.UpdateSetting("LEVELS", "BA_REQ_LEVEL", ref MainMenu.BA_REQ_LEVEL, ref this.BA_REQ_LEVEL_TextBox);
				this.UpdateSetting("LEVELS", "CTF_REQ_LEVEL", ref MainMenu.CTF_REQ_LEVEL, ref this.CTF_REQ_LEVEL_TextBox);
				this.UpdateSetting("LEVELS", "EXCHANGE_LEVEL", ref MainMenu.EXCHANGE_LEVEL, ref this.EXCHANGE_LEVEL_TextBox);
				this.UpdateSetting("LEVELS", "GLOBAL_LEVEL", ref MainMenu.GLOBAL_LEVEL, ref this.GLOBAL_LEVEL_TextBox);
				this.UpdateSetting("LEVELS", "STALL_LEVEL", ref MainMenu.STALL_LEVEL, ref this.STALL_LEVEL_TextBox);
				this.UpdateSetting("LEVELS", "ZERK_LEVEL", ref MainMenu.ZERK_LEVEL, ref this.ZERK_LEVEL_TextBox);
				this.UpdateSetting("LEVELS", "DROP_GOLD_LEVEL", ref MainMenu.DROP_GOLD_LEVEL, ref this.DROP_GOLD_LEVEL_TextBox);
				this.UpdateSetting("LEVELS", "BA_REQ_LEVEL_NOTICE", ref MainMenu.BA_REQ_LEVEL_NOTICE, ref this.BA_REQ_LEVEL_NOTICE_TextBox);
				this.UpdateSetting("LEVELS", "CTF_REQ_LEVEL_NOTICE", ref MainMenu.CTF_REQ_LEVEL_NOTICE, ref this.CTF_REQ_LEVEL_NOTICE_TextBox);
				this.UpdateSetting("LEVELS", "EXCHANGE_LEVEL_NOTICE", ref MainMenu.EXCHANGE_LEVEL_NOTICE, ref this.EXCHENGE_LEVEL_NOTICE_TextBox);
				this.UpdateSetting("LEVELS", "GLOBAL_LEVEL_NOTICE", ref MainMenu.GLOBAL_LEVEL_NOTICE, ref this.GLOBAL_LEVEL_NOTICE_TextBox);
				this.UpdateSetting("LEVELS", "STALL_LEVEL_NOTICE", ref MainMenu.STALL_LEVEL_NOTICE, ref this.STALL_LEVEL_NOTICE_TextBox);
				this.UpdateSetting("LEVELS", "ZERK_LEVEL_NOTICE", ref MainMenu.ZERK_LEVEL_NOTICE, ref this.ZERK_LEVEL_NOTICE_TextBox);
				this.UpdateSetting("LEVELS", "DROP_GOLD_LEVEL_NOTICE", ref MainMenu.DROP_GOLD_LEVEL_NOTICE, ref this.DROP_LEVEL_NOTICE_TextBox);
				this.UpdateSetting("SERVERNOTICE", "WELCOME_MSG_CHECK", ref MainMenu.WELCOME_MSG_CHECK, ref this.WELCOME_MSG_CHECKBOX);
				this.UpdateSetting("SERVERNOTICE", "DISABLE_RESTART_BUTTON_CHECK", ref MainMenu.DISABLE_RESTART_BUTTON_CHECK, ref this.DISABLE_RESTART_BUTTON_CHECKBOX);
				this.UpdateSetting("SERVERNOTICE", "DISABLE_AVATAR_BLUES_CHECK", ref MainMenu.DISABLE_AVATAR_BLUES_CHECK, ref this.DISABLE_AVATAR_BLUES_CHECKBOX);
				this.UpdateSetting("SERVERNOTICE", "DISABLED_ACADEMY_CHECK", ref MainMenu.DISABLED_ACADEMY_CHECK, ref this.DISABLED_ACADEMY_CHECKBOX);
				this.UpdateSetting("SERVERNOTICE", "DISABLE_TAX_RATE_CHANGE_CHECK", ref MainMenu.DISABLE_TAX_RATE_CHANGE_CHECK, ref this.DISABLE_TAX_RATE_CHANGE_CHECKBOX);
				this.UpdateSetting("SERVERNOTICE", "TOWN_DROP_ITEM_CHECK", ref MainMenu.TOWN_DROP_ITEM_CHECK, ref this.TOWN_DROP_ITEM_CHECKBOX);
				this.UpdateSetting("SERVERNOTICE", "DISCONNECT_NOTICE_CHECK", ref MainMenu.DISCONNECT_NOTICE_CHECK, ref this.DISCONNECT_NOTICE_CHECKBOX);
				this.UpdateSetting("SERVERNOTICE", "BAN_NOTICE_CHECK", ref MainMenu.BAN_NOTICE_CHECK, ref this.BAN_NOTICE_CHECKBOX);
				this.UpdateSetting("SERVERNOTICE", "GM_NOTICE_CHECK", ref MainMenu.GM_NOTICE_CHECK, ref this.GM_NOTICE_CHECKBOX);
				this.UpdateSetting("SERVERNOTICE", "BLOCKSKILL_NOTICE_CHECK", ref MainMenu.BLOCKSKILL_NOTICE_CHECK, ref this.BLOCKSKILL_NOTICE_CHECKBOX);
				this.UpdateSetting("SERVERNOTICE", "BLOCKSKILLPVP_NOTICE_CHECK", ref MainMenu.BLOCKZERKPVP_NOTICE_CHECK, ref this.BLOCKZERKPVP_NOTICE_CHECKBOX);
				this.UpdateSetting("SERVERNOTICE", "BLOCKZERKPVP_NOTICE_CHECK", ref MainMenu.PULSNOTICE_NOTICE_CHECK, ref this.PULSNOTICE_NOTICE_CHECKBOX);
				this.UpdateSetting("SERVERNOTICE", "SOX_PLUS1", ref MainMenu.SOX_PLUS1, ref this.SOX_PLUS1_TextBox);
				this.UpdateSetting("SERVERNOTICE", "SOX_PLUS2", ref MainMenu.SOX_PLUS2, ref this.SOX_PLUS2_TextBox);
				this.UpdateSetting("SERVERNOTICE", "SOX_DROP1", ref MainMenu.SOX_DROP1, ref this.SOX_Drop1_TextBox);
				this.UpdateSetting("SERVERNOTICE", "SOX_DROP2", ref MainMenu.SOX_DROP2, ref this.SOX_Drop2_TextBox);
				this.UpdateSetting("SERVERNOTICE", "SOXDROPOTICE_NOTICE_CHECK", ref MainMenu.SOXDROPOTICE_NOTICE_CHECK, ref this.SOXDROPNOTICE_NOTICE_CHECKBOX);
				this.UpdateSetting("SERVERNOTICE", "SOXDROPNOTICE_NOTICE", ref MainMenu.SOXDROPNOTICE_NOTICE, ref this.SOXDROPNOTICE_NOTICE_TextBox);
				this.UpdateSetting("SERVERNOTICE", "WELCOME_TEXT_NOTICE", ref MainMenu.WELCOME_TEXT_NOTICE, ref this.WELCOME_TEXT_NOTICE_TextBox);
				this.UpdateSetting("SERVERNOTICE", "DISABLE_RESTART_NOTICE", ref MainMenu.DISABLE_RESTART_NOTICE, ref this.DISABLE_RESTART_NOTICE_TextBox);
				this.UpdateSetting("SERVERNOTICE", "DISABLE_AVATAR_BLUES_NOTICE", ref MainMenu.DISABLE_AVATAR_BLUES_NOTICE, ref this.DISABLE_AVATAR_BLUES_NOTICE_TextBox);
				this.UpdateSetting("SERVERNOTICE", "DISABLED_ACADEMY_NOTICE", ref MainMenu.DISABLED_ACADEMY_NOTICE, ref this.DISABLED_ACADEMY_NOTICE_TextBox);
				this.UpdateSetting("SERVERNOTICE", "DISABLE_TAX_RATE_CHANGE_NOTICE", ref MainMenu.DISABLE_TAX_RATE_CHANGE_NOTICE, ref this.DISABLE_TAX_RATE_CHANGE_NOTICE_TextBox);
				this.UpdateSetting("SERVERNOTICE", "TOWN_DROP_ITEM_NOTICE", ref MainMenu.TOWN_DROP_ITEM_NOTICE, ref this.TOWN_DROP_ITEM_NOTICE_TextBox);
				this.UpdateSetting("SERVERNOTICE", "DISCONNECT_NOTICE_NOTICE", ref MainMenu.DISCONNECT_NOTICE_NOTICE, ref this.DISCONNECT_NOTICE_TextBox);
				this.UpdateSetting("SERVERNOTICE", "BAN_NOTICE_NOTICE", ref MainMenu.BAN_NOTICE_NOTICE, ref this.BAN_NOTICE_TextBox);
				this.UpdateSetting("SERVERNOTICE", "GM_NOTICE_NOTICE", ref MainMenu.GM_NOTICE_NOTICE, ref this.GM_NOTICE_TextBox);
				this.UpdateSetting("SERVERNOTICE", "BLOCKSKILL_NOTICE_NOTICE", ref MainMenu.BLOCKSKILL_NOTICE_NOTICE, ref this.BLOCKSKILL_NOTICE_TextBox);
				this.UpdateSetting("SERVERNOTICE", "BLOCKZERKPVP_NOTICE", ref MainMenu.BLOCKZERKPVP_NOTICE, ref this.BLOCKZERKPVP_NOTICE_TextBox);
				this.UpdateSetting("SERVERNOTICE", "PULSNOTICE_NOTICE", ref MainMenu.PULSNOTICE_NOTICE, ref this.PULSNOTICE_NOTICE_TextBox);
				this.UpdateSetting("SERVERNOTICE", "PULSNOTICE_NOTICE_Start", ref MainMenu.PULSNOTICE_NOTICE_Start, ref this.PULSNOTICE_NOTICE_Start_TextBox);
				this.UpdateSetting("SERVERINFO", "SERVER_NAME", ref MainMenu.SERVER_NAME, ref this.SERVER_NAME_TEXTBOX);
				this.UpdateSetting("SERVERINFO", "SHARD_MAX_PLAYER", ref MainMenu.SHARD_MAX_PLAYER, ref this.SHARD_MAX_PLAYER_TEXTBOX);
				this.UpdateSetting("SERVERINFO", "CAPCHA", ref MainMenu.CAPCHA, ref this.CAPCHA_TEXTBOX);
				this.UpdateSetting("SERVERINFO", "AFKMS", ref MainMenu.AFKMS, ref this.AFKMS_TEXTBOX);
				this.UpdateSetting("SERVERINFO", "DISABLECAPCHA", ref MainMenu.DISABLECAPCHA, ref this.DISABLECAPCHA_CHECKBOX);
				this.UpdateSetting("CLIENTLESS", "CL_GT_IP", ref MainMenu.CLIENT_HOST_IP, ref this.CL_GT_IP);
				this.UpdateSetting("CLIENTLESS", "CL_GT_PORT", ref MainMenu.CLIENT_GW_PORT, ref this.CL_GT_PORT);
				this.UpdateSetting("CLIENTLESS", "CL_VER", ref MainMenu.CLIENT_VERSION, ref this.CL_VER);
				this.UpdateSetting("CLIENTLESS", "CL_LOCALE", ref MainMenu.CLIENT_LOCALE, ref this.CL_LOCALE);
				this.UpdateSetting("CLIENTLESS", "CL_ID", ref MainMenu.CLIENT_ID, ref this.CL_ID);
				this.UpdateSetting("CLIENTLESS", "CL_PASSWORD", ref MainMenu.CLIENT_PW, ref this.CL_PASSWORD);
				this.UpdateSetting("CLIENTLESS", "CL_CAPTCHA", ref MainMenu.CLIENT_CAPTCHA_VALUE, ref this.CL_CAPTCHA);
				this.UpdateSetting("CLIENTLESS", "CL_CHARNAME", ref MainMenu.CLIENT_CharName, ref this.CL_CHARNAME);
				this.UpdateSetting("EVENTS", "QNA_ENABLE", ref MainMenu.QNA_ENABLE, ref this.checkBox_QnAEnable);
				this.UpdateSetting("EVENTS", "QNA_TIMETOANSWER", ref MainMenu.QNA_TIMETOANSWER, ref this.textBox_QnATimeAnswer);
				this.UpdateSetting("EVENTS", "QNA_ROUNDS", ref MainMenu.QNA_ROUNDS, ref this.qnarounds);
				this.UpdateSetting("EVENTS", "QNA_ITEMREWARD", ref MainMenu.QNA_ITEMREWARD, ref this.itemcodetrivia);
				this.UpdateSetting("EVENTS", "QNA_ITEMCOUNT", ref MainMenu.QNA_ITEMCOUNT, ref this.questitemcount);
				this.UpdateSetting("EVENTS", "QNA_ITEMNAME", ref MainMenu.QNA_ITEMNAME, ref this.itemnametrivia);
				this.UpdateSetting("EVENTS", "QNA_ENABLE", ref MainMenu.QNA_ENABLE, ref this.checkBox_QnAEnable);
				this.UpdateSetting("EVENTS", "QNA_TIMETOANSWER", ref MainMenu.QNA_TIMETOANSWER, ref this.textBox_QnATimeAnswer);
				this.UpdateSetting("EVENTS", "QNA_ROUNDS", ref MainMenu.QNA_ROUNDS, ref this.qnarounds);
				this.UpdateSetting("EVENTS", "QNA_ITEMREWARD", ref MainMenu.QNA_ITEMREWARD, ref this.itemcodetrivia);
				this.UpdateSetting("EVENTS", "QNA_ITEMCOUNT", ref MainMenu.QNA_ITEMCOUNT, ref this.questitemcount);
				this.UpdateSetting("EVENTS", "QNA_ITEMNAME", ref MainMenu.QNA_ITEMNAME, ref this.itemnametrivia);
				this.UpdateSetting("EVENTS", "QNA_ENABLE", ref MainMenu.QNA_ENABLE, ref this.checkBox_QnAEnable);
				this.UpdateSetting("EVENTS", "QNA_TIMETOANSWER", ref MainMenu.QNA_TIMETOANSWER, ref this.textBox_QnATimeAnswer);
				this.UpdateSetting("EVENTS", "QNA_ROUNDS", ref MainMenu.QNA_ROUNDS, ref this.qnarounds);
				this.UpdateSetting("EVENTS", "QNA_ITEMREWARD", ref MainMenu.QNA_ITEMREWARD, ref this.itemcodetrivia);
				this.UpdateSetting("EVENTS", "QNA_ITEMCOUNT", ref MainMenu.QNA_ITEMCOUNT, ref this.questitemcount);
				this.UpdateSetting("EVENTS", "QNA_ITEMNAME", ref MainMenu.QNA_ITEMNAME, ref this.itemnametrivia);
				this.UpdateSetting("EVENTS", "HNS_ENABLE", ref MainMenu.HNS_ENABLE, ref this.HNS_ENABLEBOX);
				this.UpdateSetting("EVENTS", "HNS_TIMETOSEARCH", ref MainMenu.HNS_TIMETOSEARCH, ref this.HNS_TIMETOSEARCHBOX);
				this.UpdateSetting("EVENTS", "HNS_ROUNDS", ref MainMenu.HNS_ROUNDS, ref this.HNS_ROUNDSBOX);
				this.UpdateSetting("EVENTS", "HNS_ITEMREWARD", ref MainMenu.HNS_ITEMREWARD, ref this.HNS_ITEMREWARDBOX);
				this.UpdateSetting("EVENTS", "HNS_ITEMCOUNT", ref MainMenu.HNS_ITEMCOUNT, ref this.HNS_ITEMCOUNTBOX);
				this.UpdateSetting("EVENTS", "HNS_ITEMNAME", ref MainMenu.HNS_ITEMNAME, ref this.HNS_ITEMNAMEBOX);
				this.UpdateSetting("EVENTS", "GMK_ENABLE", ref MainMenu.GMK_ENABLE, ref this.GMK_ENABLEBOX);
				this.UpdateSetting("EVENTS", "GMK_TIMETOWAIT", ref MainMenu.GMK_TIMETOWAIT, ref this.GMK_TIMETOWAITBOX);
				this.UpdateSetting("EVENTS", "GMK_ROUND", ref MainMenu.GMK_ROUND, ref this.GMK_ROUNDBOX);
				this.UpdateSetting("EVENTS", "GMK_ITEMID", ref MainMenu.GMK_ITEMID, ref this.GMK_ITEMIDBOX);
				this.UpdateSetting("EVENTS", "GMK_ITEMCOUNT", ref MainMenu.GMK_ITEMCOUNT, ref this.GMK_ITEMCOUNTBOX);
				this.UpdateSetting("EVENTS", "GMK_ITEMNAME", ref MainMenu.GMK_ITEMNAME, ref this.GMK_ITEMNAMEBOX);
				this.UpdateSetting("EVENTS", "GMK_REGIONID", ref MainMenu.GMK_REGIONID, ref this.GMK_REGIONIDBOX);
				this.UpdateSetting("EVENTS", "GMK_POSX", ref MainMenu.GMK_POSX, ref this.GMK_POSXBOX);
				this.UpdateSetting("EVENTS", "GMK_POSY", ref MainMenu.GMK_POSY, ref this.GMK_POSYBOX);
				this.UpdateSetting("EVENTS", "GMK_POSZ", ref MainMenu.GMK_POSZ, ref this.GMK_POSZBOX);
				this.UpdateSetting("EVENTS", "SND_ENABLE", ref MainMenu.SND_ENABLE, ref this.SND_ENABLEBOX);
				this.UpdateSetting("EVENTS", "SND_TIMETOSEARCH", ref MainMenu.SND_TIMETOSEARCH, ref this.SND_TIMETOSEARCHBOX);
				this.UpdateSetting("EVENTS", "SND_ROUNDS", ref MainMenu.SND_ROUNDS, ref this.SND_ROUNDSBOX);
				this.UpdateSetting("EVENTS", "SND_ITEMREWARD", ref MainMenu.SND_ITEMREWARD, ref this.SND_ITEMREWARDBOX);
				this.UpdateSetting("EVENTS", "SND_ITEMCOUNT", ref MainMenu.SND_ITEMCOUNT, ref this.SND_ITEMCOUNTBOX);
				this.UpdateSetting("EVENTS", "SND_ITEMNAME", ref MainMenu.SND_ITEMNAME, ref this.SND_ITEMNAMEBOX);
				this.UpdateSetting("EVENTS", "SND_MOBID", ref MainMenu.SND_MOBID, ref this.SND_MOBIDBOX);
				this.UpdateSetting("EVENTS", "LG_ENABLE", ref MainMenu.LG_ENABLE, ref this.LG_ENABLEBOX);
				this.UpdateSetting("EVENTS", "LG_TIMETOWAIT", ref MainMenu.LG_TIMETOWAIT, ref this.LG_TIMETOWAITBOX);
				this.UpdateSetting("EVENTS", "LG_TICKETPRICE", ref MainMenu.LG_TICKETPRICE, ref this.LG_TICKETPRICEBOX);
				this.UpdateSetting("EVENTS", "LG_ROUND", ref MainMenu.LG_ROUND, ref this.LG_ROUNDBOX);
				this.UpdateSetting("EVENTS", "LPN_ENABLE", ref MainMenu.LPN_ENABLE, ref this.LPN_ENABLEBOX);
				this.UpdateSetting("EVENTS", "LPN_TIMETOWAIT", ref MainMenu.LPN_TIMETOWAIT, ref this.LPN_TIMETOWAITBOX);
				this.UpdateSetting("EVENTS", "LPN_ROUNDS", ref MainMenu.LPN_ROUNDS, ref this.LPN_ROUNDSBOX);
				this.UpdateSetting("EVENTS", "LPN_ITEMREWARD", ref MainMenu.LPN_ITEMREWARD, ref this.LPN_ITEMREWARDBOX);
				this.UpdateSetting("EVENTS", "LPN_ITEMCOUNT", ref MainMenu.LPN_ITEMCOUNT, ref this.LPN_ITEMCOUNTBOX);
				this.UpdateSetting("EVENTS", "LPN_ITEMNAME", ref MainMenu.LPN_ITEMNAME, ref this.LPN_ITEMNAMEBOX);
				this.UpdateSetting("EVENTS", "PT_MINVALUE", ref MainMenu.PT_MINVALUE, ref this.PT_MINVALUEBOX);
				this.UpdateSetting("EVENTS", "PT_MAXVALUE", ref MainMenu.PT_MAXVALUE, ref this.PT_MAXVALUEBOX);
				this.UpdateSetting("EVENTS", "LMS_ENABLE", ref MainMenu.LMS_ENABLE, ref this.LMS_ENABLEBOX);
				this.UpdateSetting("EVENTS", "LMS_PCLIMIT", ref MainMenu.LMS_PCLIMIT, ref this.LMS_PCLIMITBOX);
				this.UpdateSetting("EVENTS", "LMS_MATCHTIME", ref MainMenu.LMS_MATCHTIME, ref this.LMS_MATCHTIMEBOX);
				this.UpdateSetting("EVENTS", "LMS_REGISTERTIME", ref MainMenu.LMS_REGISTERTIME, ref this.LMS_REGISTERTIMEBOX);
				this.UpdateSetting("EVENTS", "LMS_REQUIRELEVEL", ref MainMenu.LMS_REQUIRELEVEL, ref this.LMS_REQUIRELEVELBOX);
				this.UpdateSetting("EVENTS", "LMS_ITEMID", ref MainMenu.LMS_ITEMID, ref this.LMS_ITEMIDBOX);
				this.UpdateSetting("EVENTS", "LMS_ITEMCOUNT", ref MainMenu.LMS_ITEMCOUNT, ref this.LMS_ITEMCOUNTBOX);
				this.UpdateSetting("EVENTS", "LMS_ITEMNAME", ref MainMenu.LMS_ITEMNAME, ref this.LMS_ITEMNAMEBOX);
				this.UpdateSetting("EVENTS", "LMS_GATEID", ref MainMenu.LMS_GATEID, ref this.LMS_GATEIDBOX);
				this.UpdateSetting("EVENTS", "LMS_REGIONID", ref MainMenu.LMS_REGIONID, ref this.LMS_REGIONIDBOX);
				this.UpdateSetting("EVENTS", "LMS_REGIONID2", ref MainMenu.LMS_REGIONID2, ref this.LMS_REGIONIDBOX2);
				this.UpdateSetting("EVENTS", "LMS_GATEWAIT_TIME", ref MainMenu.LMS_GATEWAIT_TIME, ref this.LMS_GATEWAIT_TIMEBOX);
				this.UpdateSetting("EVENTS", "SURV_ENABLE", ref MainMenu.SURV_ENABLE, ref this.SURV_ENABLEBOX);
				this.UpdateSetting("EVENTS", "SURV_MATCHTIME", ref MainMenu.SURV_MATCHTIME, ref this.SURV_MATCHTIMEBOX);
				this.UpdateSetting("EVENTS", "SURV_REGISTERTIME", ref MainMenu.SURV_REGISTERTIME, ref this.SURV_REGISTERTIMEBOX);
				this.UpdateSetting("EVENTS", "SURV_REQUIRELEVEL", ref MainMenu.SURV_REQUIRELEVEL, ref this.SURV_REQUIRELEVELBOX);
				this.UpdateSetting("EVENTS", "SURV_ITEMID", ref MainMenu.SURV_ITEMID, ref this.SURV_ITEMIDBOX);
				this.UpdateSetting("EVENTS", "SURV_ITEMCOUNT", ref MainMenu.SURV_ITEMCOUNT, ref this.SURV_ITEMCOUNTBOX);
				this.UpdateSetting("EVENTS", "SURV_ITEMNAME", ref MainMenu.SURV_ITEMNAME, ref this.SURV_ITEMNAMEBOX);
				this.UpdateSetting("EVENTS", "SURV_GATEID", ref MainMenu.SURV_GATEID, ref this.SURV_GATEIDBOX);
				this.UpdateSetting("EVENTS", "SURV_REGIONID2", ref MainMenu.SURV_REGIONID2, ref this.SURV_REGIONIDBOX2);
				this.UpdateSetting("EVENTS", "SURV_REGIONID", ref MainMenu.SURV_REGIONID, ref this.SURV_REGIONIDBOX);
				this.UpdateSetting("EVENTS", "SURV_GATEWAIT_TIME", ref MainMenu.SURV_GATEWAIT_TIME, ref this.SURV_GATEWAIT_TIMEBOX);
				this.UpdateSetting("EVENTSNOTICE", "QNA_STARTNOTICE", ref MainMenu.QNA_STARTNOTICE, ref this.QNA_STARTNOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "QNA_INFONOTICE", ref MainMenu.QNA_INFONOTICE, ref this.QNA_INFONOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "QNA_INFOCHAR", ref MainMenu.QNA_INFOCHAR, ref this.QNA_INFOCHARBOX);
				this.UpdateSetting("EVENTSNOTICE", "QNA_END", ref MainMenu.QNA_END, ref this.QNA_ENDBOX);
				this.UpdateSetting("EVENTSNOTICE", "QNA_WIN", ref MainMenu.QNA_WIN, ref this.QNA_WINBOX);
				this.UpdateSetting("EVENTSNOTICE", "QNA_ROUNDINFO", ref MainMenu.QNA_ROUNDINFO, ref this.QNA_ROUNDINFOBOX);
				this.UpdateSetting("EVENTSNOTICE", "HNS_STARTNOTICE", ref MainMenu.HNS_STARTNOTICE, ref this.HNS_STARTNOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "HNS_INFONOTICE", ref MainMenu.HNS_INFONOTICE, ref this.HNS_INFONOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "HNS_PLACEINFO", ref MainMenu.HNS_PLACEINFO, ref this.HNS_PLACEINFOBOX);
				this.UpdateSetting("EVENTSNOTICE", "HNS_END", ref MainMenu.HNS_END, ref this.HNS_ENDBOX);
				this.UpdateSetting("EVENTSNOTICE", "HNS_WIN", ref MainMenu.HNS_WIN, ref this.HNS_WINBOX);
				this.UpdateSetting("EVENTSNOTICE", "GMK_PLACENAME", ref MainMenu.GMK_PLACENAME, ref this.GMK_PLACENAMEBOX);
				this.UpdateSetting("EVENTSNOTICE", "GMK_START_NOTICE", ref MainMenu.GMK_START_NOTICE, ref this.GMK_START_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "GMK_INFO_NOTICE", ref MainMenu.GMK_INFO_NOTICE, ref this.GMK_INFO_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "GMK_INFORM_NOTICE", ref MainMenu.GMK_INFORM_NOTICE, ref this.GMK_INFORM_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "GMK_WIN_NOTICE", ref MainMenu.GMK_WIN_NOTICE, ref this.GMK_WIN_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "GMK_END_NOTICE", ref MainMenu.GMK_END_NOTICE, ref this.GMK_END_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "SND_STARTNOTICE", ref MainMenu.SND_STARTNOTICE, ref this.SND_STARTNOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "SND_INFONOTICE", ref MainMenu.SND_INFONOTICE, ref this.SND_INFONOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "SND_PLACEINFO", ref MainMenu.SND_PLACEINFO, ref this.SND_PLACEINFOBOX);
				this.UpdateSetting("EVENTSNOTICE", "SND_END", ref MainMenu.SND_END, ref this.SND_ENDBOX);
				this.UpdateSetting("EVENTSNOTICE", "SND_WIN", ref MainMenu.SND_WIN, ref this.SND_WINBOX);
				this.UpdateSetting("EVENTSNOTICE", "LG_START_NOTICE", ref MainMenu.LG_START_NOTICE, ref this.LG_START_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "LG_TICKETPRICE_NOTICE", ref MainMenu.LG_TICKETPRICE_NOTICE, ref this.LG_TICKETPRICE_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "LG_END_NOTICE", ref MainMenu.LG_END_NOTICE, ref this.LG_END_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "LG_WIN_NOTICE", ref MainMenu.LG_WIN_NOTICE, ref this.LG_WIN_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "LG_STARTREG_NOTICE", ref MainMenu.LG_STARTREG_NOTICE, ref this.LG_STARTREG_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "LG_STOPREG_NOTICE", ref MainMenu.LG_STOPREG_NOTICE, ref this.LG_STOPREG_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "LG_GOLDREQUIRE_NOTICE", ref MainMenu.LG_GOLDREQUIRE_NOTICE, ref this.LG_GOLDREQUIRE_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "LG_REGISTERSUCCESS_NOTICE", ref MainMenu.LG_REGISTERSUCCESS_NOTICE, ref this.LG_REGISTERSUCCESS_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "LG_REGISTED_NOTICE", ref MainMenu.LG_REGISTED_NOTICE, ref this.LG_REGISTED_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "LG_ENDD_NOTICE", ref MainMenu.LG_ENDD_NOTICE, ref this.LG_ENDD_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "LPN_START_NOTICE", ref MainMenu.LPN_START_NOTICE, ref this.LPN_START_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "LPN_INFO", ref MainMenu.LPN_INFO, ref this.LPN_INFOBOX);
				this.UpdateSetting("EVENTSNOTICE", "LPN_WIN_NOTICE", ref MainMenu.LPN_WIN_NOTICE, ref this.LPN_WIN_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "LPN_NOREFORM_NOTICE", ref MainMenu.LPN_NOREFORM_NOTICE, ref this.LPN_NOREFORM_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "LPN_END", ref MainMenu.LPN_END, ref this.LPN_ENDBOX);
				this.UpdateSetting("EVENTSNOTICE", "LPN_ROUNDINFO", ref MainMenu.LPN_ROUNDINFO, ref this.LPN_ROUNDINFOBOX);
				this.UpdateSetting("EVENTSNOTICE", "LMS_START_NOTICE", ref MainMenu.LMS_START_NOTICE, ref this.LMS_START_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "LMS_REGISTERTIME_NOTICE", ref MainMenu.LMS_REGISTERTIME_NOTICE, ref this.LMS_REGISTERTIME_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "LMS_REGISTERCLOSE_NOTICE", ref MainMenu.LMS_REGISTERCLOSE_NOTICE, ref this.LMS_REGISTERCLOSE_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "LMS_INFORM_NOTICE", ref MainMenu.LMS_INFORM_NOTICE, ref this.LMS_INFORM_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "LMS_GATEOPEN_NOTICE", ref MainMenu.LMS_GATEOPEN_NOTICE, ref this.LMS_GATEOPEN_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "LMS_GATECLOSE_NOTICE", ref MainMenu.LMS_GATECLOSE_NOTICE, ref this.LMS_GATECLOSE_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "LMS_CANCELEVENT_NOTICE", ref MainMenu.LMS_CANCELEVENT_NOTICE, ref this.LMS_CANCELEVENT_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "LMS_INFO2_NOTICE", ref MainMenu.LMS_INFO2_NOTICE, ref this.LMS_INFO2_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "LMS_WIN_NOTICE", ref MainMenu.LMS_WIN_NOTICE, ref this.LMS_WIN_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "LMS_END_NOTICE", ref MainMenu.LMS_END_NOTICE, ref this.LMS_END_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "LMS_REGISTED_NOTICE", ref MainMenu.LMS_REGISTED_NOTICE, ref this.LMS_REGISTED_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "LMS_REGISTERSUCCESS_NOTICE", ref MainMenu.LMS_REGISTERSUCCESS_NOTICE, ref this.LMS_REGISTERSUCCESS_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "LMS_REQUIRELEVEL_NOTICE", ref MainMenu.LMS_REQUIRELEVEL_NOTICE, ref this.LMS_REQUIRELEVEL_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "LMS_JOB_NOTICE", ref MainMenu.LMS_JOB_NOTICE, ref this.LMS_JOB_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "LMS_ELIMINATED_NOTICE", ref MainMenu.LMS_ELIMINATED_NOTICE, ref this.LMS_ELIMINATED_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "LMS_FIGHTSTART_NOTICE", ref MainMenu.LMS_FIGHTSTART_NOTICE, ref this.LMS_FIGHTSTART_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "SURV_START_NOTICE", ref MainMenu.SURV_START_NOTICE, ref this.SURV_START_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "SURV_REGISTERTIME_NOTICE", ref MainMenu.SURV_REGISTERTIME_NOTICE, ref this.SURV_REGISTERTIME_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "SURV_REGISTERCLOSE_NOTICE", ref MainMenu.SURV_REGISTERCLOSE_NOTICE, ref this.SURV_REGISTERCLOSE_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "SURV_INFORM_NOTICE", ref MainMenu.SURV_INFORM_NOTICE, ref this.SURV_INFORM_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "SURV_GATEOPEN_NOTICE", ref MainMenu.SURV_GATEOPEN_NOTICE, ref this.SURV_GATEOPEN_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "SURV_GATECLOSE_NOTICE", ref MainMenu.SURV_GATECLOSE_NOTICE, ref this.SURV_GATECLOSE_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "SURV_CANCELEVENT_NOTICE", ref MainMenu.SURV_CANCELEVENT_NOTICE, ref this.SURV_CANCELEVENT_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "SURV_INFO2_NOTICE", ref MainMenu.SURV_INFO2_NOTICE, ref this.SURV_INFO2_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "SURV_WIN_NOTICE", ref MainMenu.SURV_WIN_NOTICE, ref this.SURV_WIN_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "SURV_END_NOTICE", ref MainMenu.SURV_END_NOTICE, ref this.SURV_END_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "SURV_REGISTED_NOTICE", ref MainMenu.SURV_REGISTED_NOTICE, ref this.SURV_REGISTED_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "SURV_REGISTERSUCCESS_NOTICE", ref MainMenu.SURV_REGISTERSUCCESS_NOTICE, ref this.SURV_REGISTERSUCCESS_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "SURV_REQUIRELEVEL_NOTICE", ref MainMenu.SURV_REQUIRELEVEL_NOTICE, ref this.SURV_REQUIRELEVEL_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "SURV_JOB_NOTICE", ref MainMenu.SURV_JOB_NOTICE, ref this.SURV_JOB_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "SURV_ELIMINATED_NOTICE", ref MainMenu.SURV_ELIMINATED_NOTICE, ref this.SURV_ELIMINATED_NOTICEBOX);
				this.UpdateSetting("EVENTSNOTICE", "SURV_FIGHTSTART_NOTICE", ref MainMenu.SURV_FIGHTSTART_NOTICE, ref this.SURV_FIGHTSTART_NOTICEBOX);
				this.UpdateSetting("GUISETTINGS", "ENABLE_CHEST", ref MainMenu.ChestWnd, ref this.ENABLE_CHEST);
				this.UpdateSetting("GUISETTINGS", "ENABLE_ATTENDANCE", ref MainMenu.AttendanceWnd, ref this.ENABLE_ATTENDANCE);
				this.UpdateSetting("GUISETTINGS", "ENABLE_ATTENDANCE_MON", ref MainMenu.AttendanceMon, ref this.ATTENDANCE_comboBox);
				this.UpdateSetting("GUISETTINGS", "ENABLE_EVENTREGISTER", ref MainMenu.EventRegWnd, ref this.ENABLE_EVNTREG);
				this.UpdateSetting("GUISETTINGS", "ENABLE_FACEBOOK", ref MainMenu.FacebookWnd, ref this.ENABLE_FB);
				this.UpdateSetting("GUISETTINGS", "ENABLE_DISCORD", ref MainMenu.DiscordWnd, ref this.ENABLE_DC);
				this.UpdateSetting("GUISETTINGS", "DISCORD_ID", ref MainMenu.DiscordInstanceID, ref this.DCID);
				this.UpdateSetting("GUISETTINGS", "ENABLE_NEWREVERSE", ref MainMenu.NewReverse, ref this.checkBoxNewRew);
				this.UpdateSetting("GUISETTINGS", "ENABLE_OLDMAINPOPUP", ref MainMenu.OldMainPopup, ref this.checkBox1oldmain);
				this.UpdateSetting("GUISETTINGS", "ENABLE_ITEMCOMPARISON", ref MainMenu.ItemComparison, ref this.checkBox1itemcomp);
				this.UpdateSetting("GUISETTINGS", "DISCORD_URL", ref MainMenu.DiscordURL, ref this.DiscordSite);
				this.UpdateSetting("GUISETTINGS", "FACEBOOK_URL", ref MainMenu.FacebookURL, ref this.facebooksite);
				this.UpdateSetting("GUISETTINGS", "MASTERY_LIMIT", ref MainMenu.MasteryLimit, ref this.MaxMastery);
				this.UpdateSetting("GUISETTINGS", "PT_MAX_LIMIT", ref MainMenu.MaxPartyLevelLimit, ref this.MaxPtNoLimit);
				this.UpdateSetting("GUISETTINGS", "ENABLE_PERMANENTALCHEMY", ref MainMenu.PermanenyAlchemy, ref this.ENABLE_PALCHEMY);
				this.UpdateSetting("GUISETTINGS", "ENABLE_GUILD_JOBMODE", ref MainMenu.GuildJobMode, ref this.GUILDJOB);
				this.UpdateSetting("GUISETTINGS", "ENABLE_MARKET", ref MainMenu.EnableMarket, ref this.marketbutton);
				this.UpdateSetting("GUISETTINGS", "ENABLE_TOKEN", ref MainMenu.EnableMarketToken, ref this.tokenbox);
				this.UpdateSetting("GUISETTINGS", "ENABLE_SILK", ref MainMenu.EnableMarketSilk, ref this.silksystembox);
				this.UpdateSetting("GUISETTINGS", "ENABLE_GOLD", ref MainMenu.EnableMarketGold, ref this.goldboxs);
				this.UpdateSetting("GUISETTINGS", "CUSTOMTITLEPRICE", ref MainMenu.CustomTitlePrice, ref this.titleprices);
				this.UpdateSetting("GUISETTINGS", "CUSTOMTITLEPRICEBRM", ref MainMenu.CustomTitleBirim, ref this.titlepricebirim);
			}
		}
		catch
		{
		}
	}

	private void GetSetting(string section, string type, ref string rfr, ref TextBox txtbx)
	{
		try
		{
			rfr = MainMenu.cfg.IniReadValue(section, type);
			txtbx.Text = rfr;
		}
		catch (Exception ex)
		{
			MainMenu.WriteLine(2, "GetSetting('" + section + "', '" + type + "', '" + rfr + "', '" + txtbx.ToString() + "')");
			Program.WriteError(ex.ToString(), "GetSetting('" + section + "', '" + type + "', '" + rfr + "', '" + txtbx.ToString() + "')");
		}
	}

	private void GetSetting(string section, string type, ref int rfr, ref TextBox txtbx)
	{
		try
		{
			txtbx.Text = MainMenu.cfg.IniReadValue(section, type);
			rfr = int.Parse(txtbx.Text);
		}
		catch (Exception ex)
		{
			MainMenu.WriteLine(2, $"GetSetting('{section}', '{type}', '{rfr}', '{txtbx.ToString()}')");
			Program.WriteError(ex.ToString(), $"GetSetting('{section}', '{type}', '{rfr}', '{txtbx.ToString()}')");
		}
	}

	private void GetSetting(string section, string type, ref long rfr, ref TextBox txtbx)
	{
		try
		{
			txtbx.Text = MainMenu.cfg.IniReadValue(section, type);
			rfr = long.Parse(txtbx.Text);
		}
		catch (Exception ex)
		{
			MainMenu.WriteLine(2, $"GetSetting('{section}', '{type}', '{rfr}', '{txtbx.ToString()}')");
			Program.WriteError(ex.ToString(), $"GetSetting('{section}', '{type}', '{rfr}', '{txtbx.ToString()}')");
		}
	}

	private void GetSetting(string section, string type, ref bool rfr, ref CheckBox cckbx)
	{
		try
		{
			rfr = bool.Parse(MainMenu.cfg.IniReadValue(section, type));
			cckbx.Checked = rfr;
		}
		catch (Exception ex)
		{
			MainMenu.WriteLine(2, $"GetSetting('{section}', '{type}', '{rfr}', '{cckbx.ToString()}')");
			Program.WriteError(ex.ToString(), $"GetSetting('{section}', '{type}', '{rfr}', '{cckbx.ToString()}')");
		}
	}

	private void GetSetting(string section, string type, ref string rfr, ref ComboBox txtbx)
	{
		try
		{
			rfr = MainMenu.cfg.IniReadValue(section, type);
			txtbx.Text = rfr;
		}
		catch (Exception ex)
		{
			MainMenu.WriteLine(2, "GetSetting('" + section + "', '" + type + "', '" + rfr + "', '" + txtbx.ToString() + "')");
			Program.WriteError(ex.ToString(), "GetSetting('" + section + "', '" + type + "', '" + rfr + "', '" + txtbx.ToString() + "')");
		}
	}

	private void UpdateSetting(string section, string type, ref string rfr, ref TextBox txtbx)
	{
		try
		{
			rfr = txtbx.Text;
			MainMenu.cfg.IniWriteValue(section, type, rfr);
		}
		catch (Exception ex)
		{
			MainMenu.WriteLine(2, "UpdateSetting('" + section + "', '" + type + "', '" + rfr + "', '" + txtbx.Text + "')");
			Program.WriteError(ex.ToString(), "UpdateSetting('" + section + "', '" + type + "', '" + rfr + "', '" + txtbx.Text + "')");
		}
	}

	private void UpdateSetting(string section, string type, ref string rfr, ref ComboBox txtbx)
	{
		try
		{
			rfr = txtbx.Text;
			MainMenu.cfg.IniWriteValue(section, type, rfr);
		}
		catch (Exception ex)
		{
			MainMenu.WriteLine(2, "UpdateSetting('" + section + "', '" + type + "', '" + rfr + "', '" + txtbx.Text + "')");
			Program.WriteError(ex.ToString(), "UpdateSetting('" + section + "', '" + type + "', '" + rfr + "', '" + txtbx.Text + "')");
		}
	}

	private void UpdateSetting(string section, string type, ref int rfr, ref TextBox txtbx)
	{
		try
		{
			rfr = int.Parse(txtbx.Text.ToString());
			MainMenu.cfg.IniWriteValue(section, type, txtbx.Text.ToString());
		}
		catch (Exception ex)
		{
			MainMenu.WriteLine(2, $"UpdateSetting('{section}', '{type}', '{rfr}', '{txtbx.Text}')");
			Program.WriteError(ex.ToString(), $"UpdateSetting('{section}', '{type}', '{rfr}', '{txtbx.Text}')");
		}
	}

	private void UpdateSetting(string section, string type, ref long rfr, ref TextBox txtbx)
	{
		try
		{
			rfr = long.Parse(txtbx.Text.ToString());
			MainMenu.cfg.IniWriteValue(section, type, txtbx.Text.ToString());
		}
		catch (Exception ex)
		{
			MainMenu.WriteLine(2, $"UpdateSetting('{section}', '{type}', '{rfr}', '{txtbx.Text}')");
			Program.WriteError(ex.ToString(), $"UpdateSetting('{section}', '{type}', '{rfr}', '{txtbx.Text}')");
		}
	}

	private void UpdateSetting(string section, string type, ref bool rfr, ref CheckBox cckbx)
	{
		try
		{
			rfr = cckbx.Checked;
			MainMenu.cfg.IniWriteValue(section, type, rfr.ToString());
		}
		catch (Exception ex)
		{
			MainMenu.WriteLine(2, $"UpdateSetting('{section}', '{type}', '{rfr}', '{cckbx.Checked.ToString()}')");
			Program.WriteError(ex.ToString(), $"UpdateSetting('{section}', '{type}', '{rfr}', '{cckbx.Checked.ToString()}')");
		}
	}

	private void RightContent()
	{
		while (true)
		{
			try
			{
				this.Agent_Count_Lable.Invoke((MethodInvoker)delegate
				{
					this.Agent_Count_Lable.Text = "Agent Count: " + AsyncServer.AgentConnections.Count;
				});
				this.Gateway_Count_Lable.Invoke((MethodInvoker)delegate
				{
					this.Gateway_Count_Lable.Text = "Gateway Count: " + AsyncServer.GatewayConnections.Count;
				});
				this.Download_Count_Lable.Invoke((MethodInvoker)delegate
				{
					this.Download_Count_Lable.Text = "Downloand Count: " + AsyncServer.DownloadConnections.Count;
				});
				if (MainMenu.AUTOEVENT_ALREADY)
				{
					this.label81.Invoke((MethodInvoker)delegate
					{
						this.label81.Text = "Online";
						this.label81.ForeColor = Color.Green;
					});
				}
				else
				{
					this.label81.Invoke((MethodInvoker)delegate
					{
						this.label81.Text = "Offline";
						this.label81.ForeColor = Color.Red;
					});
				}
				Thread.Sleep(3000);
			}
			catch (Exception ex)
			{
				Program.WriteError(ex.ToString(), "RightContent");
			}
		}
	}

	public static void WriteLine(int type, string msg)
	{
		try
		{
			if (Global.MainWindow.listView1.InvokeRequired)
			{
				Global.MainWindow.listView1.Invoke((MethodInvoker)delegate
				{
					MainMenu.WriteLine(type, msg);
				});
				return;
			}
			lock (MainMenu.ListLocker)
			{
				Color foreColor;
				string text;
				switch (type)
				{
				case 1:
					foreColor = Color.Green;
					text = "[NOTIFY]";
					break;
				case 2:
					foreColor = Color.Red;
					text = "[FATAL]";
					break;
				default:
					foreColor = Color.Blue;
					text = "[WARNNING]";
					break;
				}
				ListViewItem listViewItem;
				listViewItem = new ListViewItem(new string[3]
				{
					DateTime.Now.ToLongTimeString(),
					text,
					msg
				});
				listViewItem.ForeColor = foreColor;
				Global.MainWindow.listView1.Items.Add(listViewItem);
				Global.MainWindow.listView1.Items[Global.MainWindow.listView1.Items.Count - 1].EnsureVisible();
				Global.MainWindow.listView1.Columns[Global.MainWindow.listView1.Columns.Count - 1].Width = -2;
			}
		}
		catch (Exception ex)
		{
			MainMenu.WriteLine(1, ex.ToString());
		}
	}

	private void MainMenu_FormClosing(object sender, FormClosingEventArgs e)
	{
		if (MessageBox.Show("Are you sure you want to exit?", "Confirm exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
		{
			Process.GetCurrentProcess().Kill();
		}
		else
		{
			e.Cancel = true;
		}
	}

	private void Open_Directory_Button_Click(object sender, EventArgs e)
	{
		Process.Start(Application.ExecutablePath.ToString().Replace("New Filter.exe", ""));
	}

	private void RestartFilter_Button_Click(object sender, EventArgs e)
	{
		if (MessageBox.Show("Are you sure you want to restart?", "Confirm restart", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
		{
			this.RestartApp();
		}
	}

	private void RestartApp()
	{
		ProcessStartInfo processStartInfo;
		processStartInfo = new ProcessStartInfo();
		processStartInfo.Arguments = "/C ping 127.0.0.1 -n 2 && \"" + Application.ExecutablePath + "\"";
		processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
		processStartInfo.CreateNoWindow = true;
		processStartInfo.FileName = "cmd.exe";
		Process.Start(processStartInfo);
		Process.GetCurrentProcess().Kill();
	}

	private void Save_Settings_Buttun_Click(object sender, EventArgs e)
	{
		this.SaveSettings();
	}

	private void button1_Click(object sender, EventArgs e)
	{
		DateTime now;
		now = DateTime.Now;
		string today;
		today = MainMenu.DatabaseBackup + "\\" + (now.Day + "." + now.Month + "." + now.Year + " " + now.Hour + "." + now.Minute + "." + now.Second).ToString();
		Directory.CreateDirectory(today);
		Task.Run(async delegate
		{
			await sqlCon.EXEC_QUERY("BACKUP DATABASE " + MainMenu.ACC_DB + " TO  DISK = N'" + today + "\\" + MainMenu.ACC_DB.ToString() + ".bak' WITH NOFORMAT, NOINIT,  NAME = N'" + MainMenu.ACC_DB + "-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10 ");
		});
		Task.Run(async delegate
		{
			await sqlCon.EXEC_QUERY("BACKUP DATABASE " + MainMenu.FILTER_DB + " TO  DISK = N'" + today + "\\" + MainMenu.FILTER_DB.ToString() + ".bak' WITH NOFORMAT, NOINIT,  NAME = N'" + MainMenu.FILTER_DB + "-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10 ");
		});
		Task.Run(async delegate
		{
			await sqlCon.EXEC_QUERY("BACKUP DATABASE " + MainMenu.LOG_DB + " TO  DISK = N'" + today + "\\" + MainMenu.LOG_DB.ToString() + ".bak' WITH NOFORMAT, NOINIT,  NAME = N'" + MainMenu.LOG_DB + "-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10 ");
		});
		Task.Run(async delegate
		{
			await sqlCon.EXEC_QUERY("BACKUP DATABASE " + MainMenu.SHA_DB + " TO  DISK = N'" + today + "\\" + MainMenu.SHA_DB.ToString() + ".bak' WITH NOFORMAT, NOINIT,  NAME = N'" + MainMenu.SHA_DB + "-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10 ");
		});
		MessageBox.Show("Yedekleme başarıyla Tamamlandı");
		Process.Start(MainMenu.DatabaseBackup);
	}

	private void button10_Click(object sender, EventArgs e)
	{
		MainMenu.AUTOEVENT_ENABLE = true;
		try
		{
			MainMenu.ClientlessList.Add(new Clientlesss(MainMenu.CLIENT_ID, MainMenu.CLIENT_PW, MainMenu.CLIENT_CharName));
		}
		catch
		{
			MainMenu.WriteLine(3, "[AutoEvent]: Error Starting!");
		}
	}

	private void LoadDataGridView()
	{
		if (this.dataGridView1.InvokeRequired)
		{
			this.dataGridView1.Invoke((MethodInvoker)delegate
			{
				this.LoadDataGridView();
			});
			return;
		}
		DataTable result;
		result = Task.Run(async () => await sqlCon.GetList("SELECT * FROM " + MainMenu.FILTER_DB + ".._EventTime")).Result;
		this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
		this.dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
		this.dataGridView1.DataSource = result;
		this.dataGridView1.Columns[0].FillWeight = 60f;
		this.dataGridView1.Columns[1].FillWeight = 60f;
		this.dataGridView1.Columns[3].FillWeight = 60f;
		this.dataGridView1.Columns[4].FillWeight = 60f;
		this.dataGridView1.Columns[5].FillWeight = 60f;
	}

	private void button_AddEvent_Click(object sender, EventArgs e)
	{
		bool num;
		num = (this.comboBox_EventTime_EventName.SelectedItem != null) & (this.comboBox_EventTime_Day.SelectedItem != null);
		_ = this.dateTimePicker_EventTime_Hour.Value;
		if (num && true)
		{
			string eventname;
			eventname = "";
			string eventday;
			eventday = "";
			string eventhour;
			eventhour = "";
			eventname = this.comboBox_EventTime_EventName.SelectedItem.ToString();
			eventday = this.comboBox_EventTime_Day.SelectedItem.ToString();
			eventhour = this.dateTimePicker_EventTime_Hour.Value.ToString("HH:mm");
			Task.Run(async delegate
			{
				await sqlCon.EXEC_QUERY("INSERT INTO " + MainMenu.FILTER_DB + ".._EventTime VALUES (1,'" + eventname + "', '" + eventday + "', '" + eventhour + "',0)");
			});
			this.dataGridView1.DataSource = null;
			this.LoadDataGridView();
		}
		else
		{
			MainMenu.WriteLine(3, "Saat ve gün seçimi yapmadınız.");
		}
	}

	private void button_RemoveEvent_Click(object sender, EventArgs e)
	{
		if (this.dataGridView1.SelectedRows.Count > 0)
		{
			foreach (DataGridViewRow selectedRow in this.dataGridView1.SelectedRows)
			{
				string id;
				id = this.dataGridView1.Rows[selectedRow.Index].Cells["ID"].Value.ToString();
				this.dataGridView1.Rows.RemoveAt(selectedRow.Index);
				Task.Run(async delegate
				{
					await sqlCon.EXEC_QUERY($"DELETE {MainMenu.FILTER_DB}.._EventTime WHERE ID = {int.Parse(id)}");
				});
			}
			return;
		}
		MainMenu.WriteLine(3, "You must choice event you want delete in Event Time Setting List");
	}

	private void button_ShowEventTime_Click(object sender, EventArgs e)
	{
		this.dataGridView1.DataSource = null;
		this.LoadDataGridView();
	}

	private void MAINTENANCE_CHECKBOX_CheckedChanged(object sender, EventArgs e)
	{
		this.UpdateSetting("PROTECTION", "MAINTENANCE", ref MainMenu.MAINTENANCE, ref this.MAINTENANCE_CHECKBOX);
	}

	private void GET_GMIP_List()
	{
		DataTable result;
		result = Task.Run(async () => await sqlCon.GetList("select *from " + MainMenu.ACC_DB + ".._PrivilegedIP")).Result;
		if (result.Rows.Count == 0)
		{
			return;
		}
		foreach (DataRow row in result.Rows)
		{
			int num;
			num = Convert.ToInt32(row["IP1"]);
			int num2;
			num2 = Convert.ToInt32(row["IP2"]);
			int num3;
			num3 = Convert.ToInt32(row["IP3"]);
			int num4;
			num4 = Convert.ToInt32(row["IP4"]);
			Convert.ToInt32(row["IP5"]);
			Convert.ToInt32(row["IP6"]);
			Convert.ToInt32(row["IP7"]);
			Convert.ToInt32(row["IP8"]);
			this.listBox1.Items.Add(num + "." + num2 + "." + num3 + "." + num4);
			MainMenu.GM_IP_List.Add(num + "." + num2 + "." + num3 + "." + num4);
		}
	}

	private void GET_GM_ACCOUNT_List()
	{
		DataTable result;
		result = Task.Run(async () => await sqlCon.GetList("select StrUserID from " + MainMenu.ACC_DB + "..TB_User where sec_content=1 and sec_primary=1")).Result;
		if (result.Rows.Count == 0)
		{
			return;
		}
		foreach (DataRow row in result.Rows)
		{
			string item;
			item = Convert.ToString(row["StrUserID"]);
			this.listBox2.Items.Add(item);
			MainMenu.GM_ACCOUNT_List.Add(item);
		}
	}

	private async void Start_Clientless_Button_Click(object sender, EventArgs e)
	{
		if (!MainMenu.AUTOEVENT_ENABLE)
		{
			MainMenu.AUTOEVENT_ENABLE = true;
			try
			{
				MainMenu.ClientlessList.Add(new Clientlesss(MainMenu.CLIENT_ID, MainMenu.CLIENT_PW, MainMenu.CLIENT_CharName));
				await Task.Delay(10000);
				this.Start_Clientless_Button.Enabled = false;
				this.DC_Clientless_Button.Enabled = true;
			}
			catch
			{
				MainMenu.WriteLine(3, "[AutoEvent]: Error Starting!");
			}
		}
	}

	private async void DC_Clientless_Button_Click(object sender, EventArgs e)
	{
		try
		{
			if (MainMenu.AUTOEVENT_ALREADY)
			{
				this.DC_Clientless_Button.Enabled = false;
				new Thread(Global.AgentGlobal.ExitAutoEvent).Start();
				await Task.Delay(10000);
				this.Start_Clientless_Button.Enabled = true;
			}
			else
			{
				MainMenu.WriteLine(3, "[AutoEvent]: Character is not online!");
			}
		}
		catch
		{
			MainMenu.WriteLine(3, "Clientless cant closed. Try Again.");
		}
	}

	private void button2_Click(object sender, EventArgs e)
	{
		this.dateTimePicker_EventTime_Hour.Value = DateTime.Now;
		this.dateTimePicker_EventTime_Hour.Value = this.dateTimePicker_EventTime_Hour.Value.AddMinutes(1.0);
	}

	private void AddBanIP_Click(object sender, EventArgs e)
	{
		Task.Run(async delegate
		{
			await sqlCon.EXEC_QUERY("INSERT INTO " + MainMenu.FILTER_DB + ".[dbo].[_FirewallBlocks] VALUES ( '" + this.BanIPTextBox.Text + "', GETDATE())");
		});
		MessageBox.Show("IP: " + this.BanIPTextBox.Text + " Ban.");
		this.BanIPTextBox.Text = "";
		this.GetBanList();
	}

	private void AddBanUser_Click(object sender, EventArgs e)
	{
		Task.Run(async delegate
		{
			await sqlCon.EXEC_QUERY("INSERT INTO " + MainMenu.FILTER_DB + ".[dbo].[_FirewallUserBlocks] VALUES ( '" + this.BanUserTextBox.Text + "', GETDATE())");
		});
		MessageBox.Show("User: " + this.BanUserTextBox.Text + " Ban.");
		this.BanUserTextBox.Text = "";
		this.GetBanList();
	}

	private void RemoveBanIP_Click(object sender, EventArgs e)
	{
		string IP;
		IP = this.BanIpListBox.SelectedItem.ToString();
		Task.Run(async delegate
		{
			await sqlCon.EXEC_QUERY("DELETE " + MainMenu.FILTER_DB + ".[dbo].[_FirewallBlocks] WHERE IP = '" + IP + "'");
		});
		MessageBox.Show("User: " + this.BanIpListBox.SelectedItem.ToString() + " Unban.");
		this.GetBanList();
		this.RemoveBanIP.Enabled = false;
	}

	private void RemoveBanUser_Click(object sender, EventArgs e)
	{
		string user;
		user = this.BanUserListBox.SelectedItem.ToString();
		Task.Run(async delegate
		{
			await sqlCon.EXEC_QUERY("DELETE " + MainMenu.FILTER_DB + ".[dbo].[_FirewallUserBlocks] WHERE StrUserID = '" + user + "'");
		});
		MessageBox.Show("User: " + this.BanUserListBox.SelectedItem.ToString() + " Unban.");
		this.GetBanList();
		this.RemoveBanUser.Enabled = false;
	}

	private void BanIpListBox_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (this.BanIpListBox.SelectedIndex != -1 && this.BanIpListBox.SelectedItem.ToString() != "")
		{
			this.RemoveBanIP.Enabled = true;
		}
	}

	private void BanUserListBox_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (this.BanUserListBox.SelectedIndex != -1 && this.BanUserListBox.SelectedItem.ToString() != "")
		{
			this.RemoveBanUser.Enabled = true;
		}
	}

	private void BanIPTextBox_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.';
	}

	private void BanUserTextBox_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsSeparator(e.KeyChar);
	}

	private void AddBanHwid_Click(object sender, EventArgs e)
	{
		Task.Run(async delegate
		{
			await sqlCon.EXEC_QUERY("INSERT INTO " + MainMenu.FILTER_DB + ".[dbo].[_FirewallHwidBlocks] VALUES ( '" + this.BanhwidTextBox.Text + "', GETDATE())");
		});
		MessageBox.Show("HWID: " + this.BanhwidTextBox.Text + " Ban.");
		this.BanhwidTextBox.Text = "";
		this.GetBanList();
	}

	private void RemoveBanHwid_Click(object sender, EventArgs e)
	{
		string HWID;
		HWID = this.BanHwidListBox.SelectedItem.ToString();
		Task.Run(async delegate
		{
			await sqlCon.EXEC_QUERY("DELETE " + MainMenu.FILTER_DB + ".[dbo].[_FirewallHwidBlocks] WHERE HWID = '" + HWID + "'");
		});
		MessageBox.Show("HWID: " + this.BanHwidListBox.SelectedItem.ToString() + " Unban.");
		this.GetBanList();
		this.RemoveBanHwid.Enabled = false;
	}

	private void BanHwidListBox_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (this.BanHwidListBox.SelectedIndex != -1 && this.BanHwidListBox.SelectedItem.ToString() != "")
		{
			this.RemoveBanHwid.Enabled = true;
		}
	}

	private void checkBox1_CheckedChanged(object sender, EventArgs e)
	{
	}

	private void ScanOnlineCheckbox_CheckedChanged(object sender, EventArgs e)
	{
		if (this.ScanOnlineCheckbox.Checked)
		{
			MainMenu.SCANONLINE = true;
		}
		else
		{
			MainMenu.SCANONLINE = false;
		}
	}

	private void button2_Click_1(object sender, EventArgs e)
	{
		new GlobalColor().Show();
	}

	private void button3_Click(object sender, EventArgs e)
	{
		this.listView1.Items.Clear();
	}

	private void button4_Click(object sender, EventArgs e)
	{
		string RegionID;
		RegionID = this.SuitRegiontextbox.Text;
		Task.Run(async delegate
		{
			await sqlCon.EXEC_QUERY($"INSERT INTO {MainMenu.FILTER_DB}.[dbo].[_EventSuitRegions] VALUES ('{Convert.ToInt32(RegionID)}')");
		});
		this.SuitRegiontextbox.Text = "";
		this.GetSuitRegions();
	}

	private void button5_Click(object sender, EventArgs e)
	{
		string RegionID;
		RegionID = this.suitlistbox.SelectedItem.ToString();
		Task.Run(async delegate
		{
			await sqlCon.EXEC_QUERY("DELETE " + MainMenu.FILTER_DB + ".[dbo].[_EventSuitRegions] WHERE RegionID = '" + RegionID + "'");
		});
		this.GetSuitRegions();
		this.removeregionButton.Enabled = false;
	}

	private void suitlistbox_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (this.suitlistbox.SelectedIndex != -1 && this.suitlistbox.SelectedItem.ToString() != "")
		{
			this.removeregionButton.Enabled = true;
		}
	}

	private void ATTENDANCE_comboBox_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (this.ATTENDANCE_comboBox.SelectedIndex + 1 == DateTime.Now.Month)
		{
			MainMenu.AttendanceWndCombobox = true;
			if (MainMenu.AttendanceWndCheck)
			{
				this.label278.Text = "True";
				this.label278.ForeColor = Color.Green;
			}
		}
		else
		{
			MainMenu.AttendanceWndCombobox = false;
			if (!MainMenu.AttendanceWndCheck)
			{
				this.label278.Text = "false";
				this.label278.ForeColor = Color.Green;
			}
			else
			{
				this.label278.ForeColor = Color.Red;
			}
		}
	}

	private void ENABLE_ATTENDANCE_CheckedChanged(object sender, EventArgs e)
	{
		if (this.ENABLE_ATTENDANCE.Checked)
		{
			MainMenu.AttendanceWndCheck = true;
			if (MainMenu.AttendanceWndCombobox)
			{
				this.label278.Text = "True";
				this.label278.ForeColor = Color.Green;
			}
			else
			{
				this.label278.Text = "false";
				this.label278.ForeColor = Color.Red;
			}
		}
		else
		{
			MainMenu.AttendanceWndCheck = false;
			if (!MainMenu.AttendanceWndCombobox)
			{
				this.label278.Text = "false";
				this.label278.ForeColor = Color.Red;
			}
			else
			{
				this.label278.Text = "false";
				this.label278.ForeColor = Color.Red;
			}
		}
	}

	private void Block_Skill_Add_Button_Click(object sender, EventArgs e)
	{
		if (this.Block_Skill_TextBox.Text.Length > 0 && this.Block_Skill_TextBoxSkillID.Text.Length > 0)
		{
			string RegionID;
			RegionID = this.Block_Skill_TextBox.Text;
			string SkillID;
			SkillID = this.Block_Skill_TextBoxSkillID.Text;
			Task.Run(async delegate
			{
				await sqlCon.EXEC_QUERY($"INSERT INTO {MainMenu.FILTER_DB}.[dbo].[_BlockedSkills] VALUES ('{Convert.ToInt32(RegionID)}', '{Convert.ToInt32(SkillID)}')");
			});
			this.Block_Skill_TextBox.Text = "";
			this.Block_Skill_TextBoxSkillID.Text = "";
			this.GetBlockSkill();
		}
	}

	private void Block_Skill_Remove_Button_Click(object sender, EventArgs e)
	{
		string[] result;
		result = this.Block_Skill_listBox.SelectedItem.ToString().Split('|');
		Task.Run(async delegate
		{
			await sqlCon.EXEC_QUERY("DELETE " + MainMenu.FILTER_DB + ".[dbo].[_BlockedSkills] WHERE RegionID = '" + result[0] + "' and SkillID = '" + result[1] + "'");
		});
		this.GetBlockSkill();
		this.Block_Skill_Remove_Button.Enabled = false;
	}

	private void Block_Skill_listBox_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (this.Block_Skill_listBox.SelectedIndex != -1 && this.Block_Skill_listBox.SelectedItem.ToString() != "")
		{
			this.Block_Skill_Remove_Button.Enabled = true;
		}
	}

	private void Login_Click(object sender, EventArgs e)
	{
		if (!MainMenu.Lisans && MainMenu.MobilinGames.Count == 0 && MainMenu.LisansUser.Count == 0)
		{
			MainMenu.Lisans = true;
			MainMenu.LisansUser.Add(new users("sa", "1234", "sa"));
			Thread.Sleep(2000);
		}
	}

	private void button4_Click_1(object sender, EventArgs e)
	{
		if (!MainMenu.Lisans)
		{
			return;
		}
		foreach (KeyValuePair<Socket, UserConnections> mobilinGame in MainMenu.MobilinGames)
		{
			MainMenu.Lisans = true;
			mobilinGame.Value.Exit = true;
			mobilinGame.Value.Disconnect();
			MainMenu.LisansUser.Clear();
		}
	}

	private void RefreshGmList_Click(object sender, EventArgs e)
	{
		this.listBox1.Items.Clear();
		MainMenu.GM_IP_List.Clear();
		this.listBox2.Items.Clear();
		MainMenu.GM_ACCOUNT_List.Clear();
		this.GET_GMIP_List();
		this.GET_GM_ACCOUNT_List();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && this.components != null)
		{
			this.components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.listView1 = new System.Windows.Forms.ListView();
		this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
		this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
		this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
		this.Agent_Count_Lable = new System.Windows.Forms.Label();
		this.Gateway_Count_Lable = new System.Windows.Forms.Label();
		this.Download_Count_Lable = new System.Windows.Forms.Label();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.button3 = new System.Windows.Forms.Button();
		this.groupBox20 = new System.Windows.Forms.GroupBox();
		this.RefreshGmList = new System.Windows.Forms.Button();
		this.listBox2 = new System.Windows.Forms.ListBox();
		this.label264 = new System.Windows.Forms.Label();
		this.listBox1 = new System.Windows.Forms.ListBox();
		this.label263 = new System.Windows.Forms.Label();
		this.MAINTENANCE_CHECKBOX = new System.Windows.Forms.CheckBox();
		this.groupBox6 = new System.Windows.Forms.GroupBox();
		this.WhiteList_CheckBox = new System.Windows.Forms.CheckBox();
		this.label30 = new System.Windows.Forms.Label();
		this.FLOOD_COUNT_TextBox = new System.Windows.Forms.TextBox();
		this.PACKET_CHECKBOX = new System.Windows.Forms.CheckBox();
		this.FIREWALLBANCHECKBOX = new System.Windows.Forms.CheckBox();
		this.groupBox9 = new System.Windows.Forms.GroupBox();
		this.DOWNLOAD_PACKET_RESET_TextBox = new System.Windows.Forms.TextBox();
		this.DW_PPS_VALUE_TextBox = new System.Windows.Forms.TextBox();
		this.label27 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.DW_BPS_VALUE_TextBox = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.groupBox8 = new System.Windows.Forms.GroupBox();
		this.AGENT_PACKET_RESET_TextBox = new System.Windows.Forms.TextBox();
		this.AG_PPS_VALUE_TextBox = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.AG_BPS_VALUE_TextBox = new System.Windows.Forms.TextBox();
		this.label23 = new System.Windows.Forms.Label();
		this.groupBox7 = new System.Windows.Forms.GroupBox();
		this.GATEWAY_PACKET_RESET_TextBox = new System.Windows.Forms.TextBox();
		this.GW_PPS_VALUE_TextBox = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.GW_BPS_VALUE_TextBox = new System.Windows.Forms.TextBox();
		this.label26 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.textBox_RealDownloadPort = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.textBox_PublicDownloadPort = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.textBox_RealAgentPort = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.textBox_PublicAgentPort = new System.Windows.Forms.TextBox();
		this.textBox_RealGatewayPort = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.textBox_Froxy_IP = new System.Windows.Forms.TextBox();
		this.Proxy_label = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.textBox_PublicGatewayPort = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.textBox_Server_IP = new System.Windows.Forms.TextBox();
		this.Server_label = new System.Windows.Forms.Label();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.label13 = new System.Windows.Forms.Label();
		this.textBox_FilterDB = new System.Windows.Forms.TextBox();
		this.textBox_LogDB = new System.Windows.Forms.TextBox();
		this.textBox_ShardDB = new System.Windows.Forms.TextBox();
		this.textBox_AccDB = new System.Windows.Forms.TextBox();
		this.textBox_SQLPass = new System.Windows.Forms.TextBox();
		this.textBox_SQLId = new System.Windows.Forms.TextBox();
		this.textBox_SQLHost = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.groupBox12 = new System.Windows.Forms.GroupBox();
		this.label68 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.label65 = new System.Windows.Forms.Label();
		this.label66 = new System.Windows.Forms.Label();
		this.label67 = new System.Windows.Forms.Label();
		this.label69 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.label59 = new System.Windows.Forms.Label();
		this.label58 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.CAFE_IP_LIMITNoticeTextBox = new System.Windows.Forms.TextBox();
		this.label51 = new System.Windows.Forms.Label();
		this.PLUS_LIMITNoticeTextBox = new System.Windows.Forms.TextBox();
		this.label52 = new System.Windows.Forms.Label();
		this.DEVIL_PLUS_LIMITNoticeTextBox = new System.Windows.Forms.TextBox();
		this.JOBT_PC_LIMITNoticeTextBox = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.FGW_PC_LIMITNoticeTextBox = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.JUPITER_PC_LIMITNoticeTextBox = new System.Windows.Forms.TextBox();
		this.label32 = new System.Windows.Forms.Label();
		this.SURVIVAL_PC_LIMITNoticeTextBox = new System.Windows.Forms.TextBox();
		this.label33 = new System.Windows.Forms.Label();
		this.HT_PC_LIMITNoticeTextBox = new System.Windows.Forms.TextBox();
		this.label47 = new System.Windows.Forms.Label();
		this.PC_LIMITNoticeTextBox = new System.Windows.Forms.TextBox();
		this.label31 = new System.Windows.Forms.Label();
		this.BA_PC_LIMITNoticeTextBox = new System.Windows.Forms.TextBox();
		this.label20 = new System.Windows.Forms.Label();
		this.CTF_PC_LIMITNoticeTextBox = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.FTW_PC_LIMITNoticeTextBox = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.JOB_PC_LIMITNoticeTextBox = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.IP_LIMITNoticeTextBox = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.JUPITER_PC_LIMITTextBox = new System.Windows.Forms.TextBox();
		this.FGW_PC_LIMITTextBox = new System.Windows.Forms.TextBox();
		this.JOBT_PC_LIMITTextBox = new System.Windows.Forms.TextBox();
		this.SURVIVAL_PC_LIMITTextBox = new System.Windows.Forms.TextBox();
		this.HT_PC_LIMITTextBox = new System.Windows.Forms.TextBox();
		this.CAFE_IP_LIMITTextBox = new System.Windows.Forms.TextBox();
		this.PLUS_LIMITTextBox = new System.Windows.Forms.TextBox();
		this.DEVIL_PLUS_LIMITTextBox = new System.Windows.Forms.TextBox();
		this.PC_LIMITTextBox = new System.Windows.Forms.TextBox();
		this.BA_PC_LIMITTextBox = new System.Windows.Forms.TextBox();
		this.CTF_PC_LIMITTextBox = new System.Windows.Forms.TextBox();
		this.FTW_PC_LIMITTextBox = new System.Windows.Forms.TextBox();
		this.JOB_PC_LIMITTextBox = new System.Windows.Forms.TextBox();
		this.IP_LIMITTextBox = new System.Windows.Forms.TextBox();
		this.label34 = new System.Windows.Forms.Label();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.label75 = new System.Windows.Forms.Label();
		this.label71 = new System.Windows.Forms.Label();
		this.DROP_GOLD_LEVEL_TextBox = new System.Windows.Forms.TextBox();
		this.label70 = new System.Windows.Forms.Label();
		this.DROP_LEVEL_NOTICE_TextBox = new System.Windows.Forms.TextBox();
		this.label116 = new System.Windows.Forms.Label();
		this.label110 = new System.Windows.Forms.Label();
		this.label117 = new System.Windows.Forms.Label();
		this.label111 = new System.Windows.Forms.Label();
		this.label118 = new System.Windows.Forms.Label();
		this.label112 = new System.Windows.Forms.Label();
		this.label119 = new System.Windows.Forms.Label();
		this.label113 = new System.Windows.Forms.Label();
		this.label120 = new System.Windows.Forms.Label();
		this.label114 = new System.Windows.Forms.Label();
		this.ZERK_LEVEL_TextBox = new System.Windows.Forms.TextBox();
		this.STALL_LEVEL_TextBox = new System.Windows.Forms.TextBox();
		this.label123 = new System.Windows.Forms.Label();
		this.GLOBAL_LEVEL_TextBox = new System.Windows.Forms.TextBox();
		this.EXCHANGE_LEVEL_TextBox = new System.Windows.Forms.TextBox();
		this.CTF_REQ_LEVEL_TextBox = new System.Windows.Forms.TextBox();
		this.label115 = new System.Windows.Forms.Label();
		this.BA_REQ_LEVEL_TextBox = new System.Windows.Forms.TextBox();
		this.CTF_REQ_LEVEL_NOTICE_TextBox = new System.Windows.Forms.TextBox();
		this.BA_REQ_LEVEL_NOTICE_TextBox = new System.Windows.Forms.TextBox();
		this.label126 = new System.Windows.Forms.Label();
		this.label131 = new System.Windows.Forms.Label();
		this.EXCHENGE_LEVEL_NOTICE_TextBox = new System.Windows.Forms.TextBox();
		this.label130 = new System.Windows.Forms.Label();
		this.label127 = new System.Windows.Forms.Label();
		this.ZERK_LEVEL_NOTICE_TextBox = new System.Windows.Forms.TextBox();
		this.GLOBAL_LEVEL_NOTICE_TextBox = new System.Windows.Forms.TextBox();
		this.label129 = new System.Windows.Forms.Label();
		this.label128 = new System.Windows.Forms.Label();
		this.STALL_LEVEL_NOTICE_TextBox = new System.Windows.Forms.TextBox();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label72 = new System.Windows.Forms.Label();
		this.REVERSE_DELAY_NOTICE_TextBox = new System.Windows.Forms.TextBox();
		this.label73 = new System.Windows.Forms.Label();
		this.label74 = new System.Windows.Forms.Label();
		this.REVERSE_DELAY_TextBox = new System.Windows.Forms.TextBox();
		this.label94 = new System.Windows.Forms.Label();
		this.label95 = new System.Windows.Forms.Label();
		this.label96 = new System.Windows.Forms.Label();
		this.label97 = new System.Windows.Forms.Label();
		this.label98 = new System.Windows.Forms.Label();
		this.label99 = new System.Windows.Forms.Label();
		this.label100 = new System.Windows.Forms.Label();
		this.label101 = new System.Windows.Forms.Label();
		this.ZERK_DELAY_NOTICE_TextBox = new System.Windows.Forms.TextBox();
		this.label102 = new System.Windows.Forms.Label();
		this.UNION_DELAY_NOTICE_TextBox = new System.Windows.Forms.TextBox();
		this.label103 = new System.Windows.Forms.Label();
		this.EXIT_DELAY_NOTICE_TextBox = new System.Windows.Forms.TextBox();
		this.label104 = new System.Windows.Forms.Label();
		this.GLOBAL_DELAY_NOTICE_TextBox = new System.Windows.Forms.TextBox();
		this.label105 = new System.Windows.Forms.Label();
		this.GUILD_DELAY_NOTICE_TextBox = new System.Windows.Forms.TextBox();
		this.label106 = new System.Windows.Forms.Label();
		this.RESTART_DELAY_NOTICE_TextBox = new System.Windows.Forms.TextBox();
		this.label107 = new System.Windows.Forms.Label();
		this.STALL_DELAY_NOTICE_TextBox = new System.Windows.Forms.TextBox();
		this.label108 = new System.Windows.Forms.Label();
		this.EXCHANGE_DELAY_NOTICE_TextBox = new System.Windows.Forms.TextBox();
		this.label109 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.label64 = new System.Windows.Forms.Label();
		this.label90 = new System.Windows.Forms.Label();
		this.label91 = new System.Windows.Forms.Label();
		this.label92 = new System.Windows.Forms.Label();
		this.ZERK_DELAY_TextBox = new System.Windows.Forms.TextBox();
		this.UNION_DELAY_TextBox = new System.Windows.Forms.TextBox();
		this.STALL_DELAY_TextBox = new System.Windows.Forms.TextBox();
		this.RESTART_DELAY_TextBox = new System.Windows.Forms.TextBox();
		this.GUILD_DELAY_TextBox = new System.Windows.Forms.TextBox();
		this.GLOBAL_DELAY_TextBox = new System.Windows.Forms.TextBox();
		this.EXIT_DELAY_TextBox = new System.Windows.Forms.TextBox();
		this.label93 = new System.Windows.Forms.Label();
		this.EXCHANGE_DELAY_TextBox = new System.Windows.Forms.TextBox();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.groupBox30 = new System.Windows.Forms.GroupBox();
		this.SOXDROPNOTICE_NOTICE_TextBox = new System.Windows.Forms.TextBox();
		this.label270 = new System.Windows.Forms.Label();
		this.label267 = new System.Windows.Forms.Label();
		this.SOX_Drop2_TextBox = new System.Windows.Forms.TextBox();
		this.label268 = new System.Windows.Forms.Label();
		this.SOX_Drop1_TextBox = new System.Windows.Forms.TextBox();
		this.SOXDROPNOTICE_NOTICE_CHECKBOX = new System.Windows.Forms.CheckBox();
		this.label266 = new System.Windows.Forms.Label();
		this.SOX_PLUS2_TextBox = new System.Windows.Forms.TextBox();
		this.label265 = new System.Windows.Forms.Label();
		this.SOX_PLUS1_TextBox = new System.Windows.Forms.TextBox();
		this.BLOCKSKILL_NOTICE_CHECKBOX = new System.Windows.Forms.CheckBox();
		this.GM_NOTICE_CHECKBOX = new System.Windows.Forms.CheckBox();
		this.BAN_NOTICE_CHECKBOX = new System.Windows.Forms.CheckBox();
		this.PULSNOTICE_NOTICE_Start_TextBox = new System.Windows.Forms.TextBox();
		this.DISCONNECT_NOTICE_CHECKBOX = new System.Windows.Forms.CheckBox();
		this.label137 = new System.Windows.Forms.Label();
		this.label136 = new System.Windows.Forms.Label();
		this.BLOCKZERKPVP_NOTICE_CHECKBOX = new System.Windows.Forms.CheckBox();
		this.PULSNOTICE_NOTICE_CHECKBOX = new System.Windows.Forms.CheckBox();
		this.label133 = new System.Windows.Forms.Label();
		this.label132 = new System.Windows.Forms.Label();
		this.label125 = new System.Windows.Forms.Label();
		this.DISABLE_TAX_RATE_CHANGE_CHECKBOX = new System.Windows.Forms.CheckBox();
		this.label124 = new System.Windows.Forms.Label();
		this.label122 = new System.Windows.Forms.Label();
		this.WELCOME_MSG_CHECKBOX = new System.Windows.Forms.CheckBox();
		this.label121 = new System.Windows.Forms.Label();
		this.TOWN_DROP_ITEM_CHECKBOX = new System.Windows.Forms.CheckBox();
		this.label86 = new System.Windows.Forms.Label();
		this.DISABLED_ACADEMY_CHECKBOX = new System.Windows.Forms.CheckBox();
		this.label85 = new System.Windows.Forms.Label();
		this.DISABLE_AVATAR_BLUES_CHECKBOX = new System.Windows.Forms.CheckBox();
		this.label84 = new System.Windows.Forms.Label();
		this.DISABLE_RESTART_BUTTON_CHECKBOX = new System.Windows.Forms.CheckBox();
		this.label83 = new System.Windows.Forms.Label();
		this.PULSNOTICE_NOTICE_TextBox = new System.Windows.Forms.TextBox();
		this.BLOCKZERKPVP_NOTICE_TextBox = new System.Windows.Forms.TextBox();
		this.BLOCKSKILL_NOTICE_TextBox = new System.Windows.Forms.TextBox();
		this.GM_NOTICE_TextBox = new System.Windows.Forms.TextBox();
		this.TOWN_DROP_ITEM_NOTICE_TextBox = new System.Windows.Forms.TextBox();
		this.DISCONNECT_NOTICE_TextBox = new System.Windows.Forms.TextBox();
		this.BAN_NOTICE_TextBox = new System.Windows.Forms.TextBox();
		this.DISABLE_TAX_RATE_CHANGE_NOTICE_TextBox = new System.Windows.Forms.TextBox();
		this.DISABLED_ACADEMY_NOTICE_TextBox = new System.Windows.Forms.TextBox();
		this.DISABLE_AVATAR_BLUES_NOTICE_TextBox = new System.Windows.Forms.TextBox();
		this.DISABLE_RESTART_NOTICE_TextBox = new System.Windows.Forms.TextBox();
		this.WELCOME_TEXT_NOTICE_TextBox = new System.Windows.Forms.TextBox();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.groupBox16 = new System.Windows.Forms.GroupBox();
		this.label87 = new System.Windows.Forms.Label();
		this.afksystem = new System.Windows.Forms.CheckBox();
		this.label89 = new System.Windows.Forms.Label();
		this.AFKMS_TEXTBOX = new System.Windows.Forms.TextBox();
		this.label80 = new System.Windows.Forms.Label();
		this.label79 = new System.Windows.Forms.Label();
		this.label77 = new System.Windows.Forms.Label();
		this.DISABLECAPCHA_CHECKBOX = new System.Windows.Forms.CheckBox();
		this.CAPCHA_TEXTBOX = new System.Windows.Forms.TextBox();
		this.SHARD_MAX_PLAYER_TEXTBOX = new System.Windows.Forms.TextBox();
		this.label76 = new System.Windows.Forms.Label();
		this.SERVER_NAME_TEXTBOX = new System.Windows.Forms.TextBox();
		this.label78 = new System.Windows.Forms.Label();
		this.tabPage5 = new System.Windows.Forms.TabPage();
		this.groupBox37 = new System.Windows.Forms.GroupBox();
		this.dataGridView1 = new System.Windows.Forms.DataGridView();
		this.button_ShowEventTime = new System.Windows.Forms.Button();
		this.button_RemoveEvent = new System.Windows.Forms.Button();
		this.button_AddEvent = new System.Windows.Forms.Button();
		this.comboBox_EventTime_EventName = new System.Windows.Forms.ComboBox();
		this.comboBox_EventTime_Day = new System.Windows.Forms.ComboBox();
		this.dateTimePicker_EventTime_Hour = new System.Windows.Forms.DateTimePicker();
		this.groupBox27 = new System.Windows.Forms.GroupBox();
		this.Start_Clientless_Button = new System.Windows.Forms.Button();
		this.DC_Clientless_Button = new System.Windows.Forms.Button();
		this.CL_INV_STATE = new System.Windows.Forms.CheckBox();
		this.label81 = new System.Windows.Forms.Label();
		this.label82 = new System.Windows.Forms.Label();
		this.label88 = new System.Windows.Forms.Label();
		this.CL_CHARNAME = new System.Windows.Forms.TextBox();
		this.label134 = new System.Windows.Forms.Label();
		this.CL_CAPTCHA = new System.Windows.Forms.TextBox();
		this.label135 = new System.Windows.Forms.Label();
		this.CL_PASSWORD = new System.Windows.Forms.TextBox();
		this.label138 = new System.Windows.Forms.Label();
		this.CL_ID = new System.Windows.Forms.TextBox();
		this.label139 = new System.Windows.Forms.Label();
		this.CL_LOCALE = new System.Windows.Forms.TextBox();
		this.label140 = new System.Windows.Forms.Label();
		this.CL_VER = new System.Windows.Forms.TextBox();
		this.label141 = new System.Windows.Forms.Label();
		this.CL_GT_PORT = new System.Windows.Forms.TextBox();
		this.label142 = new System.Windows.Forms.Label();
		this.CL_GT_IP = new System.Windows.Forms.TextBox();
		this.tabPage6 = new System.Windows.Forms.TabPage();
		this.groupBox11 = new System.Windows.Forms.GroupBox();
		this.label190 = new System.Windows.Forms.Label();
		this.HNS_WINBOX = new System.Windows.Forms.TextBox();
		this.label192 = new System.Windows.Forms.Label();
		this.HNS_ENDBOX = new System.Windows.Forms.TextBox();
		this.label193 = new System.Windows.Forms.Label();
		this.HNS_PLACEINFOBOX = new System.Windows.Forms.TextBox();
		this.label194 = new System.Windows.Forms.Label();
		this.HNS_INFONOTICEBOX = new System.Windows.Forms.TextBox();
		this.label195 = new System.Windows.Forms.Label();
		this.HNS_STARTNOTICEBOX = new System.Windows.Forms.TextBox();
		this.groupBox10 = new System.Windows.Forms.GroupBox();
		this.label186 = new System.Windows.Forms.Label();
		this.QNA_ROUNDINFOBOX = new System.Windows.Forms.TextBox();
		this.label185 = new System.Windows.Forms.Label();
		this.QNA_WINBOX = new System.Windows.Forms.TextBox();
		this.label184 = new System.Windows.Forms.Label();
		this.QNA_ENDBOX = new System.Windows.Forms.TextBox();
		this.label183 = new System.Windows.Forms.Label();
		this.QNA_INFOCHARBOX = new System.Windows.Forms.TextBox();
		this.label187 = new System.Windows.Forms.Label();
		this.QNA_INFONOTICEBOX = new System.Windows.Forms.TextBox();
		this.label159 = new System.Windows.Forms.Label();
		this.QNA_STARTNOTICEBOX = new System.Windows.Forms.TextBox();
		this.groupBox29 = new System.Windows.Forms.GroupBox();
		this.HNS_ITEMNAMEBOX = new System.Windows.Forms.TextBox();
		this.label152 = new System.Windows.Forms.Label();
		this.HNS_ITEMCOUNTBOX = new System.Windows.Forms.TextBox();
		this.label153 = new System.Windows.Forms.Label();
		this.HNS_ITEMREWARDBOX = new System.Windows.Forms.TextBox();
		this.HNS_ROUNDSBOX = new System.Windows.Forms.TextBox();
		this.HNS_TIMETOSEARCHBOX = new System.Windows.Forms.TextBox();
		this.label154 = new System.Windows.Forms.Label();
		this.label155 = new System.Windows.Forms.Label();
		this.label156 = new System.Windows.Forms.Label();
		this.HNS_ENABLEBOX = new System.Windows.Forms.CheckBox();
		this.groupBox38 = new System.Windows.Forms.GroupBox();
		this.itemnametrivia = new System.Windows.Forms.TextBox();
		this.label157 = new System.Windows.Forms.Label();
		this.questitemcount = new System.Windows.Forms.TextBox();
		this.label158 = new System.Windows.Forms.Label();
		this.itemcodetrivia = new System.Windows.Forms.TextBox();
		this.qnarounds = new System.Windows.Forms.TextBox();
		this.textBox_QnATimeAnswer = new System.Windows.Forms.TextBox();
		this.label191 = new System.Windows.Forms.Label();
		this.Rounds = new System.Windows.Forms.Label();
		this.label189 = new System.Windows.Forms.Label();
		this.checkBox_QnAEnable = new System.Windows.Forms.CheckBox();
		this.tabPage7 = new System.Windows.Forms.TabPage();
		this.groupBox13 = new System.Windows.Forms.GroupBox();
		this.label188 = new System.Windows.Forms.Label();
		this.GMK_END_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label196 = new System.Windows.Forms.Label();
		this.GMK_WIN_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label197 = new System.Windows.Forms.Label();
		this.GMK_INFORM_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label198 = new System.Windows.Forms.Label();
		this.GMK_INFO_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label199 = new System.Windows.Forms.Label();
		this.GMK_START_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label200 = new System.Windows.Forms.Label();
		this.GMK_PLACENAMEBOX = new System.Windows.Forms.TextBox();
		this.groupBox49 = new System.Windows.Forms.GroupBox();
		this.label149 = new System.Windows.Forms.Label();
		this.GMK_ITEMCOUNTBOX = new System.Windows.Forms.TextBox();
		this.label150 = new System.Windows.Forms.Label();
		this.GMK_ROUNDBOX = new System.Windows.Forms.TextBox();
		this.GMK_ITEMNAMEBOX = new System.Windows.Forms.TextBox();
		this.label151 = new System.Windows.Forms.Label();
		this.label243 = new System.Windows.Forms.Label();
		this.label242 = new System.Windows.Forms.Label();
		this.label221 = new System.Windows.Forms.Label();
		this.GMK_POSZBOX = new System.Windows.Forms.TextBox();
		this.GMK_POSYBOX = new System.Windows.Forms.TextBox();
		this.GMK_POSXBOX = new System.Windows.Forms.TextBox();
		this.GMK_REGIONIDBOX = new System.Windows.Forms.TextBox();
		this.label241 = new System.Windows.Forms.Label();
		this.GMK_ITEMIDBOX = new System.Windows.Forms.TextBox();
		this.GMK_TIMETOWAITBOX = new System.Windows.Forms.TextBox();
		this.label238 = new System.Windows.Forms.Label();
		this.label240 = new System.Windows.Forms.Label();
		this.GMK_ENABLEBOX = new System.Windows.Forms.CheckBox();
		this.tabPage8 = new System.Windows.Forms.TabPage();
		this.groupBox15 = new System.Windows.Forms.GroupBox();
		this.LG_ENDD_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label201 = new System.Windows.Forms.Label();
		this.label207 = new System.Windows.Forms.Label();
		this.LG_REGISTED_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label216 = new System.Windows.Forms.Label();
		this.LG_REGISTERSUCCESS_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label213 = new System.Windows.Forms.Label();
		this.LG_GOLDREQUIRE_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label215 = new System.Windows.Forms.Label();
		this.LG_STOPREG_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label208 = new System.Windows.Forms.Label();
		this.LG_STARTREG_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label209 = new System.Windows.Forms.Label();
		this.LG_WIN_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label210 = new System.Windows.Forms.Label();
		this.LG_END_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label211 = new System.Windows.Forms.Label();
		this.LG_TICKETPRICE_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label212 = new System.Windows.Forms.Label();
		this.LG_START_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.groupBox14 = new System.Windows.Forms.GroupBox();
		this.SND_ENDBOX = new System.Windows.Forms.TextBox();
		this.label202 = new System.Windows.Forms.Label();
		this.SND_WINBOX = new System.Windows.Forms.TextBox();
		this.label203 = new System.Windows.Forms.Label();
		this.label204 = new System.Windows.Forms.Label();
		this.SND_PLACEINFOBOX = new System.Windows.Forms.TextBox();
		this.label205 = new System.Windows.Forms.Label();
		this.SND_INFONOTICEBOX = new System.Windows.Forms.TextBox();
		this.label206 = new System.Windows.Forms.Label();
		this.SND_STARTNOTICEBOX = new System.Windows.Forms.TextBox();
		this.groupBox45 = new System.Windows.Forms.GroupBox();
		this.LG_ROUNDBOX = new System.Windows.Forms.TextBox();
		this.label143 = new System.Windows.Forms.Label();
		this.LG_TICKETPRICEBOX = new System.Windows.Forms.TextBox();
		this.LG_TIMETOWAITBOX = new System.Windows.Forms.TextBox();
		this.label223 = new System.Windows.Forms.Label();
		this.label224 = new System.Windows.Forms.Label();
		this.LG_ENABLEBOX = new System.Windows.Forms.CheckBox();
		this.groupBox43 = new System.Windows.Forms.GroupBox();
		this.label144 = new System.Windows.Forms.Label();
		this.SND_MOBIDBOX = new System.Windows.Forms.TextBox();
		this.label145 = new System.Windows.Forms.Label();
		this.SND_TIMETOSEARCHBOX = new System.Windows.Forms.TextBox();
		this.label214 = new System.Windows.Forms.Label();
		this.SND_ITEMCOUNTBOX = new System.Windows.Forms.TextBox();
		this.SND_ENABLEBOX = new System.Windows.Forms.CheckBox();
		this.label146 = new System.Windows.Forms.Label();
		this.label147 = new System.Windows.Forms.Label();
		this.SND_ITEMNAMEBOX = new System.Windows.Forms.TextBox();
		this.SND_ITEMREWARDBOX = new System.Windows.Forms.TextBox();
		this.label148 = new System.Windows.Forms.Label();
		this.SND_ROUNDSBOX = new System.Windows.Forms.TextBox();
		this.tabPage9 = new System.Windows.Forms.TabPage();
		this.groupBox17 = new System.Windows.Forms.GroupBox();
		this.label225 = new System.Windows.Forms.Label();
		this.LPN_ROUNDINFOBOX = new System.Windows.Forms.TextBox();
		this.label217 = new System.Windows.Forms.Label();
		this.LPN_WIN_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label218 = new System.Windows.Forms.Label();
		this.LPN_ENDBOX = new System.Windows.Forms.TextBox();
		this.label219 = new System.Windows.Forms.Label();
		this.LPN_NOREFORM_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label220 = new System.Windows.Forms.Label();
		this.LPN_INFOBOX = new System.Windows.Forms.TextBox();
		this.label222 = new System.Windows.Forms.Label();
		this.LPN_START_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.groupBox47 = new System.Windows.Forms.GroupBox();
		this.label176 = new System.Windows.Forms.Label();
		this.PT_MAXVALUEBOX = new System.Windows.Forms.TextBox();
		this.PT_MINVALUEBOX = new System.Windows.Forms.TextBox();
		this.label177 = new System.Windows.Forms.Label();
		this.label178 = new System.Windows.Forms.Label();
		this.LPN_ITEMCOUNTBOX = new System.Windows.Forms.TextBox();
		this.label179 = new System.Windows.Forms.Label();
		this.LPN_ROUNDSBOX = new System.Windows.Forms.TextBox();
		this.LPN_ITEMNAMEBOX = new System.Windows.Forms.TextBox();
		this.label180 = new System.Windows.Forms.Label();
		this.LPN_ITEMREWARDBOX = new System.Windows.Forms.TextBox();
		this.label181 = new System.Windows.Forms.Label();
		this.LPN_TIMETOWAITBOX = new System.Windows.Forms.TextBox();
		this.label233 = new System.Windows.Forms.Label();
		this.LPN_ENABLEBOX = new System.Windows.Forms.CheckBox();
		this.tabPage10 = new System.Windows.Forms.TabPage();
		this.groupBox18 = new System.Windows.Forms.GroupBox();
		this.LMS_REQUIRELEVEL_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label246 = new System.Windows.Forms.Label();
		this.LMS_FIGHTSTART_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label245 = new System.Windows.Forms.Label();
		this.LMS_ELIMINATED_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label236 = new System.Windows.Forms.Label();
		this.LMS_JOB_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label237 = new System.Windows.Forms.Label();
		this.LMS_REGISTERSUCCESS_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label239 = new System.Windows.Forms.Label();
		this.LMS_REGISTED_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label244 = new System.Windows.Forms.Label();
		this.LMS_END_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label182 = new System.Windows.Forms.Label();
		this.label226 = new System.Windows.Forms.Label();
		this.LMS_WIN_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label227 = new System.Windows.Forms.Label();
		this.LMS_INFO2_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label228 = new System.Windows.Forms.Label();
		this.LMS_CANCELEVENT_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label229 = new System.Windows.Forms.Label();
		this.LMS_GATECLOSE_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label230 = new System.Windows.Forms.Label();
		this.LMS_GATEOPEN_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label231 = new System.Windows.Forms.Label();
		this.LMS_INFORM_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label232 = new System.Windows.Forms.Label();
		this.LMS_REGISTERCLOSE_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label234 = new System.Windows.Forms.Label();
		this.LMS_REGISTERTIME_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label235 = new System.Windows.Forms.Label();
		this.LMS_START_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.groupBox53 = new System.Windows.Forms.GroupBox();
		this.LMS_REGIONIDBOX2 = new System.Windows.Forms.TextBox();
		this.label281 = new System.Windows.Forms.Label();
		this.LMS_PCLIMITBOX = new System.Windows.Forms.TextBox();
		this.label169 = new System.Windows.Forms.Label();
		this.LMS_GATEWAIT_TIMEBOX = new System.Windows.Forms.TextBox();
		this.label170 = new System.Windows.Forms.Label();
		this.LMS_REGIONIDBOX = new System.Windows.Forms.TextBox();
		this.label171 = new System.Windows.Forms.Label();
		this.LMS_GATEIDBOX = new System.Windows.Forms.TextBox();
		this.label172 = new System.Windows.Forms.Label();
		this.LMS_ITEMCOUNTBOX = new System.Windows.Forms.TextBox();
		this.label173 = new System.Windows.Forms.Label();
		this.LMS_ITEMNAMEBOX = new System.Windows.Forms.TextBox();
		this.label174 = new System.Windows.Forms.Label();
		this.LMS_ITEMIDBOX = new System.Windows.Forms.TextBox();
		this.label175 = new System.Windows.Forms.Label();
		this.LMS_REQUIRELEVELBOX = new System.Windows.Forms.TextBox();
		this.label269 = new System.Windows.Forms.Label();
		this.LMS_REGISTERTIMEBOX = new System.Windows.Forms.TextBox();
		this.LMS_MATCHTIMEBOX = new System.Windows.Forms.TextBox();
		this.label276 = new System.Windows.Forms.Label();
		this.label277 = new System.Windows.Forms.Label();
		this.LMS_ENABLEBOX = new System.Windows.Forms.CheckBox();
		this.tabPage11 = new System.Windows.Forms.TabPage();
		this.groupBox19 = new System.Windows.Forms.GroupBox();
		this.SURV_REQUIRELEVEL_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label247 = new System.Windows.Forms.Label();
		this.SURV_FIGHTSTART_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label248 = new System.Windows.Forms.Label();
		this.SURV_ELIMINATED_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label249 = new System.Windows.Forms.Label();
		this.SURV_JOB_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label250 = new System.Windows.Forms.Label();
		this.SURV_REGISTERSUCCESS_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label251 = new System.Windows.Forms.Label();
		this.SURV_REGISTED_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label252 = new System.Windows.Forms.Label();
		this.SURV_END_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label253 = new System.Windows.Forms.Label();
		this.label254 = new System.Windows.Forms.Label();
		this.SURV_WIN_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label255 = new System.Windows.Forms.Label();
		this.SURV_INFO2_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label256 = new System.Windows.Forms.Label();
		this.SURV_CANCELEVENT_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label257 = new System.Windows.Forms.Label();
		this.SURV_GATECLOSE_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label258 = new System.Windows.Forms.Label();
		this.SURV_GATEOPEN_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label259 = new System.Windows.Forms.Label();
		this.SURV_INFORM_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label260 = new System.Windows.Forms.Label();
		this.SURV_REGISTERCLOSE_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label261 = new System.Windows.Forms.Label();
		this.SURV_REGISTERTIME_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.label262 = new System.Windows.Forms.Label();
		this.SURV_START_NOTICEBOX = new System.Windows.Forms.TextBox();
		this.groupBox28 = new System.Windows.Forms.GroupBox();
		this.SURV_REGIONIDBOX2 = new System.Windows.Forms.TextBox();
		this.label282 = new System.Windows.Forms.Label();
		this.SURV_GATEWAIT_TIMEBOX = new System.Windows.Forms.TextBox();
		this.label160 = new System.Windows.Forms.Label();
		this.SURV_REGIONIDBOX = new System.Windows.Forms.TextBox();
		this.label161 = new System.Windows.Forms.Label();
		this.SURV_GATEIDBOX = new System.Windows.Forms.TextBox();
		this.label162 = new System.Windows.Forms.Label();
		this.SURV_ITEMCOUNTBOX = new System.Windows.Forms.TextBox();
		this.label163 = new System.Windows.Forms.Label();
		this.SURV_ITEMNAMEBOX = new System.Windows.Forms.TextBox();
		this.label164 = new System.Windows.Forms.Label();
		this.SURV_ITEMIDBOX = new System.Windows.Forms.TextBox();
		this.label165 = new System.Windows.Forms.Label();
		this.SURV_REQUIRELEVELBOX = new System.Windows.Forms.TextBox();
		this.label166 = new System.Windows.Forms.Label();
		this.SURV_REGISTERTIMEBOX = new System.Windows.Forms.TextBox();
		this.SURV_MATCHTIMEBOX = new System.Windows.Forms.TextBox();
		this.label167 = new System.Windows.Forms.Label();
		this.label168 = new System.Windows.Forms.Label();
		this.SURV_ENABLEBOX = new System.Windows.Forms.CheckBox();
		this.tabPage12 = new System.Windows.Forms.TabPage();
		this.groupBox24 = new System.Windows.Forms.GroupBox();
		this.BanHwidListBox = new System.Windows.Forms.ListBox();
		this.AddBanHwid = new System.Windows.Forms.Button();
		this.RemoveBanHwid = new System.Windows.Forms.Button();
		this.BanhwidTextBox = new System.Windows.Forms.TextBox();
		this.groupBox23 = new System.Windows.Forms.GroupBox();
		this.BanUserListBox = new System.Windows.Forms.ListBox();
		this.AddBanUser = new System.Windows.Forms.Button();
		this.RemoveBanUser = new System.Windows.Forms.Button();
		this.BanUserTextBox = new System.Windows.Forms.TextBox();
		this.groupBox22 = new System.Windows.Forms.GroupBox();
		this.ScanOnlineCheckbox = new System.Windows.Forms.CheckBox();
		this.userInfo1 = new NewFilter.WGUI.UserInfo();
		this.groupBox21 = new System.Windows.Forms.GroupBox();
		this.BanIpListBox = new System.Windows.Forms.ListBox();
		this.RemoveBanIP = new System.Windows.Forms.Button();
		this.BanIPTextBox = new System.Windows.Forms.TextBox();
		this.AddBanIP = new System.Windows.Forms.Button();
		this.tabPage13 = new System.Windows.Forms.TabPage();
		this.groupBox33 = new System.Windows.Forms.GroupBox();
		this.titlepricebirim = new System.Windows.Forms.TextBox();
		this.label288 = new System.Windows.Forms.Label();
		this.titleprices = new System.Windows.Forms.TextBox();
		this.marketbutton = new System.Windows.Forms.CheckBox();
		this.goldboxs = new System.Windows.Forms.CheckBox();
		this.silksystembox = new System.Windows.Forms.CheckBox();
		this.label287 = new System.Windows.Forms.Label();
		this.tokenbox = new System.Windows.Forms.CheckBox();
		this.groupBox31 = new System.Windows.Forms.GroupBox();
		this.groupBox32 = new System.Windows.Forms.GroupBox();
		this.label280 = new System.Windows.Forms.Label();
		this.Block_Skill_TextBoxSkillID = new System.Windows.Forms.TextBox();
		this.label279 = new System.Windows.Forms.Label();
		this.Block_Skill_listBox = new System.Windows.Forms.ListBox();
		this.Block_Skill_Add_Button = new System.Windows.Forms.Button();
		this.Block_Skill_Remove_Button = new System.Windows.Forms.Button();
		this.Block_Skill_TextBox = new System.Windows.Forms.TextBox();
		this.groupBox26 = new System.Windows.Forms.GroupBox();
		this.addregionbutton = new System.Windows.Forms.Button();
		this.removeregionButton = new System.Windows.Forms.Button();
		this.SuitRegiontextbox = new System.Windows.Forms.TextBox();
		this.suitlistbox = new System.Windows.Forms.ListBox();
		this.groupBox25 = new System.Windows.Forms.GroupBox();
		this.GUILDJOB = new System.Windows.Forms.CheckBox();
		this.ENABLE_PALCHEMY = new System.Windows.Forms.CheckBox();
		this.label278 = new System.Windows.Forms.Label();
		this.ATTENDANCE_comboBox = new System.Windows.Forms.ComboBox();
		this.label275 = new System.Windows.Forms.Label();
		this.MaxPtNoLimit = new System.Windows.Forms.TextBox();
		this.label274 = new System.Windows.Forms.Label();
		this.MaxMastery = new System.Windows.Forms.TextBox();
		this.label273 = new System.Windows.Forms.Label();
		this.facebooksite = new System.Windows.Forms.TextBox();
		this.label272 = new System.Windows.Forms.Label();
		this.DiscordSite = new System.Windows.Forms.TextBox();
		this.checkBox1itemcomp = new System.Windows.Forms.CheckBox();
		this.checkBox1oldmain = new System.Windows.Forms.CheckBox();
		this.checkBoxNewRew = new System.Windows.Forms.CheckBox();
		this.label271 = new System.Windows.Forms.Label();
		this.DCID = new System.Windows.Forms.TextBox();
		this.ENABLE_CHEST = new System.Windows.Forms.CheckBox();
		this.ENABLE_EVNTREG = new System.Windows.Forms.CheckBox();
		this.ENABLE_FB = new System.Windows.Forms.CheckBox();
		this.ENABLE_DC = new System.Windows.Forms.CheckBox();
		this.ENABLE_ATTENDANCE = new System.Windows.Forms.CheckBox();
		this.tabPage14 = new System.Windows.Forms.TabPage();
		this.LisansAutoCheckBox = new System.Windows.Forms.CheckBox();
		this.button4 = new System.Windows.Forms.Button();
		this.Login = new System.Windows.Forms.Button();
		this.TextBoxLisansPassword = new System.Windows.Forms.TextBox();
		this.label283 = new System.Windows.Forms.Label();
		this.TextBoxLisansUserName = new System.Windows.Forms.TextBox();
		this.label284 = new System.Windows.Forms.Label();
		this.Open_Directory_Button = new System.Windows.Forms.Button();
		this.RestartFilter_Button = new System.Windows.Forms.Button();
		this.Save_Settings_Buttun = new System.Windows.Forms.Button();
		this.button1 = new System.Windows.Forms.Button();
		this.button2 = new System.Windows.Forms.Button();
		this.LicenseStatus = new System.Windows.Forms.Label();
		this.label285 = new System.Windows.Forms.Label();
		this.label286 = new System.Windows.Forms.Label();
		this.LisansDateLable = new System.Windows.Forms.Label();
		this.JobReverseBlock = new System.Windows.Forms.CheckBox();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.groupBox20.SuspendLayout();
		this.groupBox6.SuspendLayout();
		this.groupBox9.SuspendLayout();
		this.groupBox8.SuspendLayout();
		this.groupBox7.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.groupBox4.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.groupBox12.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.tabPage4.SuspendLayout();
		this.groupBox30.SuspendLayout();
		this.groupBox5.SuspendLayout();
		this.groupBox16.SuspendLayout();
		this.tabPage5.SuspendLayout();
		this.groupBox37.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dataGridView1).BeginInit();
		this.groupBox27.SuspendLayout();
		this.tabPage6.SuspendLayout();
		this.groupBox11.SuspendLayout();
		this.groupBox10.SuspendLayout();
		this.groupBox29.SuspendLayout();
		this.groupBox38.SuspendLayout();
		this.tabPage7.SuspendLayout();
		this.groupBox13.SuspendLayout();
		this.groupBox49.SuspendLayout();
		this.tabPage8.SuspendLayout();
		this.groupBox15.SuspendLayout();
		this.groupBox14.SuspendLayout();
		this.groupBox45.SuspendLayout();
		this.groupBox43.SuspendLayout();
		this.tabPage9.SuspendLayout();
		this.groupBox17.SuspendLayout();
		this.groupBox47.SuspendLayout();
		this.tabPage10.SuspendLayout();
		this.groupBox18.SuspendLayout();
		this.groupBox53.SuspendLayout();
		this.tabPage11.SuspendLayout();
		this.groupBox19.SuspendLayout();
		this.groupBox28.SuspendLayout();
		this.tabPage12.SuspendLayout();
		this.groupBox24.SuspendLayout();
		this.groupBox23.SuspendLayout();
		this.groupBox22.SuspendLayout();
		this.groupBox21.SuspendLayout();
		this.tabPage13.SuspendLayout();
		this.groupBox33.SuspendLayout();
		this.groupBox31.SuspendLayout();
		this.groupBox32.SuspendLayout();
		this.groupBox26.SuspendLayout();
		this.groupBox25.SuspendLayout();
		this.tabPage14.SuspendLayout();
		base.SuspendLayout();
		this.listView1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.listView1.BackColor = System.Drawing.SystemColors.InactiveBorder;
		this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[3] { this.columnHeader1, this.columnHeader2, this.columnHeader3 });
		this.listView1.FullRowSelect = true;
		this.listView1.GridLines = true;
		this.listView1.HideSelection = false;
		this.listView1.Location = new System.Drawing.Point(0, 478);
		this.listView1.MultiSelect = false;
		this.listView1.Name = "listView1";
		this.listView1.Size = new System.Drawing.Size(1182, 166);
		this.listView1.TabIndex = 3;
		this.listView1.UseCompatibleStateImageBehavior = false;
		this.listView1.View = System.Windows.Forms.View.Details;
		this.columnHeader1.Text = "Time";
		this.columnHeader1.Width = 75;
		this.columnHeader2.Text = "Type";
		this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.columnHeader2.Width = 86;
		this.columnHeader3.Text = "Message";
		this.columnHeader3.Width = 787;
		this.Agent_Count_Lable.AutoSize = true;
		this.Agent_Count_Lable.Location = new System.Drawing.Point(1070, 21);
		this.Agent_Count_Lable.Name = "Agent_Count_Lable";
		this.Agent_Count_Lable.Size = new System.Drawing.Size(69, 13);
		this.Agent_Count_Lable.TabIndex = 4;
		this.Agent_Count_Lable.Text = "Agent Count:";
		this.Gateway_Count_Lable.AutoSize = true;
		this.Gateway_Count_Lable.Location = new System.Drawing.Point(1056, 44);
		this.Gateway_Count_Lable.Name = "Gateway_Count_Lable";
		this.Gateway_Count_Lable.Size = new System.Drawing.Size(83, 13);
		this.Gateway_Count_Lable.TabIndex = 5;
		this.Gateway_Count_Lable.Text = "Gateway Count:";
		this.Download_Count_Lable.AutoSize = true;
		this.Download_Count_Lable.Location = new System.Drawing.Point(1044, 66);
		this.Download_Count_Lable.Name = "Download_Count_Lable";
		this.Download_Count_Lable.Size = new System.Drawing.Size(95, 13);
		this.Download_Count_Lable.TabIndex = 6;
		this.Download_Count_Lable.Text = "Downloand Count:";
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Controls.Add(this.tabPage4);
		this.tabControl1.Controls.Add(this.tabPage5);
		this.tabControl1.Controls.Add(this.tabPage6);
		this.tabControl1.Controls.Add(this.tabPage7);
		this.tabControl1.Controls.Add(this.tabPage8);
		this.tabControl1.Controls.Add(this.tabPage9);
		this.tabControl1.Controls.Add(this.tabPage10);
		this.tabControl1.Controls.Add(this.tabPage11);
		this.tabControl1.Controls.Add(this.tabPage12);
		this.tabControl1.Controls.Add(this.tabPage13);
		this.tabControl1.Controls.Add(this.tabPage14);
		this.tabControl1.Location = new System.Drawing.Point(0, 1);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1041, 479);
		this.tabControl1.TabIndex = 7;
		this.tabPage1.BackColor = System.Drawing.Color.WhiteSmoke;
		this.tabPage1.Controls.Add(this.button3);
		this.tabPage1.Controls.Add(this.groupBox20);
		this.tabPage1.Controls.Add(this.groupBox6);
		this.tabPage1.Controls.Add(this.groupBox9);
		this.tabPage1.Controls.Add(this.groupBox8);
		this.tabPage1.Controls.Add(this.groupBox7);
		this.tabPage1.Controls.Add(this.groupBox1);
		this.tabPage1.Controls.Add(this.groupBox4);
		this.tabPage1.Location = new System.Drawing.Point(4, 22);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1033, 453);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "General Settings";
		this.button3.Location = new System.Drawing.Point(45, 421);
		this.button3.Name = "button3";
		this.button3.Size = new System.Drawing.Size(75, 23);
		this.button3.TabIndex = 71;
		this.button3.Text = "button3";
		this.button3.UseVisualStyleBackColor = true;
		this.button3.Click += new System.EventHandler(button3_Click);
		this.groupBox20.Controls.Add(this.RefreshGmList);
		this.groupBox20.Controls.Add(this.listBox2);
		this.groupBox20.Controls.Add(this.label264);
		this.groupBox20.Controls.Add(this.listBox1);
		this.groupBox20.Controls.Add(this.label263);
		this.groupBox20.Controls.Add(this.MAINTENANCE_CHECKBOX);
		this.groupBox20.Location = new System.Drawing.Point(789, 236);
		this.groupBox20.Name = "groupBox20";
		this.groupBox20.Size = new System.Drawing.Size(229, 191);
		this.groupBox20.TabIndex = 70;
		this.groupBox20.TabStop = false;
		this.groupBox20.Text = "GM Settings";
		this.RefreshGmList.Location = new System.Drawing.Point(153, 159);
		this.RefreshGmList.Name = "RefreshGmList";
		this.RefreshGmList.Size = new System.Drawing.Size(75, 23);
		this.RefreshGmList.TabIndex = 60;
		this.RefreshGmList.Text = "Refresh";
		this.RefreshGmList.UseVisualStyleBackColor = true;
		this.RefreshGmList.Click += new System.EventHandler(RefreshGmList_Click);
		this.listBox2.FormattingEnabled = true;
		this.listBox2.Location = new System.Drawing.Point(128, 35);
		this.listBox2.Name = "listBox2";
		this.listBox2.Size = new System.Drawing.Size(97, 108);
		this.listBox2.TabIndex = 58;
		this.label264.AutoSize = true;
		this.label264.Location = new System.Drawing.Point(125, 19);
		this.label264.Name = "label264";
		this.label264.Size = new System.Drawing.Size(79, 13);
		this.label264.TabIndex = 59;
		this.label264.Text = "GM ACCOUNT";
		this.listBox1.FormattingEnabled = true;
		this.listBox1.Location = new System.Drawing.Point(6, 35);
		this.listBox1.Name = "listBox1";
		this.listBox1.Size = new System.Drawing.Size(110, 108);
		this.listBox1.TabIndex = 54;
		this.label263.AutoSize = true;
		this.label263.Location = new System.Drawing.Point(12, 19);
		this.label263.Name = "label263";
		this.label263.Size = new System.Drawing.Size(37, 13);
		this.label263.TabIndex = 57;
		this.label263.Text = "GM IP";
		this.MAINTENANCE_CHECKBOX.AutoSize = true;
		this.MAINTENANCE_CHECKBOX.Location = new System.Drawing.Point(9, 159);
		this.MAINTENANCE_CHECKBOX.Name = "MAINTENANCE_CHECKBOX";
		this.MAINTENANCE_CHECKBOX.Size = new System.Drawing.Size(128, 17);
		this.MAINTENANCE_CHECKBOX.TabIndex = 11;
		this.MAINTENANCE_CHECKBOX.Text = "Maintenance (Check)";
		this.MAINTENANCE_CHECKBOX.UseVisualStyleBackColor = true;
		this.MAINTENANCE_CHECKBOX.CheckedChanged += new System.EventHandler(MAINTENANCE_CHECKBOX_CheckedChanged);
		this.groupBox6.Controls.Add(this.WhiteList_CheckBox);
		this.groupBox6.Controls.Add(this.label30);
		this.groupBox6.Controls.Add(this.FLOOD_COUNT_TextBox);
		this.groupBox6.Controls.Add(this.PACKET_CHECKBOX);
		this.groupBox6.Controls.Add(this.FIREWALLBANCHECKBOX);
		this.groupBox6.Location = new System.Drawing.Point(789, 8);
		this.groupBox6.Name = "groupBox6";
		this.groupBox6.Size = new System.Drawing.Size(229, 221);
		this.groupBox6.TabIndex = 69;
		this.groupBox6.TabStop = false;
		this.groupBox6.Text = "Security";
		this.WhiteList_CheckBox.AutoSize = true;
		this.WhiteList_CheckBox.Location = new System.Drawing.Point(13, 109);
		this.WhiteList_CheckBox.Name = "WhiteList_CheckBox";
		this.WhiteList_CheckBox.Size = new System.Drawing.Size(115, 17);
		this.WhiteList_CheckBox.TabIndex = 53;
		this.WhiteList_CheckBox.Text = "OpCode White List";
		this.WhiteList_CheckBox.UseVisualStyleBackColor = true;
		this.label30.AutoSize = true;
		this.label30.Location = new System.Drawing.Point(9, 25);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(64, 13);
		this.label30.TabIndex = 52;
		this.label30.Text = "Flood Count";
		this.FLOOD_COUNT_TextBox.Location = new System.Drawing.Point(79, 22);
		this.FLOOD_COUNT_TextBox.Name = "FLOOD_COUNT_TextBox";
		this.FLOOD_COUNT_TextBox.Size = new System.Drawing.Size(114, 20);
		this.FLOOD_COUNT_TextBox.TabIndex = 51;
		this.FLOOD_COUNT_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.PACKET_CHECKBOX.AutoSize = true;
		this.PACKET_CHECKBOX.Location = new System.Drawing.Point(12, 57);
		this.PACKET_CHECKBOX.Name = "PACKET_CHECKBOX";
		this.PACKET_CHECKBOX.Size = new System.Drawing.Size(114, 17);
		this.PACKET_CHECKBOX.TabIndex = 9;
		this.PACKET_CHECKBOX.Text = "Unknown Packets";
		this.PACKET_CHECKBOX.UseVisualStyleBackColor = true;
		this.FIREWALLBANCHECKBOX.AutoSize = true;
		this.FIREWALLBANCHECKBOX.Location = new System.Drawing.Point(12, 83);
		this.FIREWALLBANCHECKBOX.Name = "FIREWALLBANCHECKBOX";
		this.FIREWALLBANCHECKBOX.Size = new System.Drawing.Size(83, 17);
		this.FIREWALLBANCHECKBOX.TabIndex = 10;
		this.FIREWALLBANCHECKBOX.Text = "Firewall Ban";
		this.FIREWALLBANCHECKBOX.UseVisualStyleBackColor = true;
		this.groupBox9.Controls.Add(this.DOWNLOAD_PACKET_RESET_TextBox);
		this.groupBox9.Controls.Add(this.DW_PPS_VALUE_TextBox);
		this.groupBox9.Controls.Add(this.label27);
		this.groupBox9.Controls.Add(this.label28);
		this.groupBox9.Controls.Add(this.DW_BPS_VALUE_TextBox);
		this.groupBox9.Controls.Add(this.label29);
		this.groupBox9.Location = new System.Drawing.Point(530, 235);
		this.groupBox9.Name = "groupBox9";
		this.groupBox9.Size = new System.Drawing.Size(237, 130);
		this.groupBox9.TabIndex = 68;
		this.groupBox9.TabStop = false;
		this.groupBox9.Text = "Download Packet Settings";
		this.DOWNLOAD_PACKET_RESET_TextBox.Location = new System.Drawing.Point(111, 85);
		this.DOWNLOAD_PACKET_RESET_TextBox.Name = "DOWNLOAD_PACKET_RESET_TextBox";
		this.DOWNLOAD_PACKET_RESET_TextBox.Size = new System.Drawing.Size(114, 20);
		this.DOWNLOAD_PACKET_RESET_TextBox.TabIndex = 50;
		this.DOWNLOAD_PACKET_RESET_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.DW_PPS_VALUE_TextBox.Location = new System.Drawing.Point(111, 55);
		this.DW_PPS_VALUE_TextBox.Name = "DW_PPS_VALUE_TextBox";
		this.DW_PPS_VALUE_TextBox.Size = new System.Drawing.Size(114, 20);
		this.DW_PPS_VALUE_TextBox.TabIndex = 49;
		this.DW_PPS_VALUE_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label27.AutoSize = true;
		this.label27.Location = new System.Drawing.Point(28, 88);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(77, 13);
		this.label27.TabIndex = 48;
		this.label27.Text = "Packets Reset";
		this.label28.AutoSize = true;
		this.label28.Location = new System.Drawing.Point(59, 58);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(46, 13);
		this.label28.TabIndex = 47;
		this.label28.Text = "Packets";
		this.DW_BPS_VALUE_TextBox.Location = new System.Drawing.Point(111, 25);
		this.DW_BPS_VALUE_TextBox.Name = "DW_BPS_VALUE_TextBox";
		this.DW_BPS_VALUE_TextBox.Size = new System.Drawing.Size(114, 20);
		this.DW_BPS_VALUE_TextBox.TabIndex = 45;
		this.DW_BPS_VALUE_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label29.AutoSize = true;
		this.label29.Location = new System.Drawing.Point(72, 28);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(33, 13);
		this.label29.TabIndex = 46;
		this.label29.Text = "Bytes";
		this.groupBox8.Controls.Add(this.AGENT_PACKET_RESET_TextBox);
		this.groupBox8.Controls.Add(this.AG_PPS_VALUE_TextBox);
		this.groupBox8.Controls.Add(this.label21);
		this.groupBox8.Controls.Add(this.label22);
		this.groupBox8.Controls.Add(this.AG_BPS_VALUE_TextBox);
		this.groupBox8.Controls.Add(this.label23);
		this.groupBox8.Location = new System.Drawing.Point(268, 235);
		this.groupBox8.Name = "groupBox8";
		this.groupBox8.Size = new System.Drawing.Size(237, 130);
		this.groupBox8.TabIndex = 67;
		this.groupBox8.TabStop = false;
		this.groupBox8.Text = "Agent Packet Settings";
		this.AGENT_PACKET_RESET_TextBox.Location = new System.Drawing.Point(111, 85);
		this.AGENT_PACKET_RESET_TextBox.Name = "AGENT_PACKET_RESET_TextBox";
		this.AGENT_PACKET_RESET_TextBox.Size = new System.Drawing.Size(114, 20);
		this.AGENT_PACKET_RESET_TextBox.TabIndex = 50;
		this.AGENT_PACKET_RESET_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.AG_PPS_VALUE_TextBox.Location = new System.Drawing.Point(111, 55);
		this.AG_PPS_VALUE_TextBox.Name = "AG_PPS_VALUE_TextBox";
		this.AG_PPS_VALUE_TextBox.Size = new System.Drawing.Size(114, 20);
		this.AG_PPS_VALUE_TextBox.TabIndex = 49;
		this.AG_PPS_VALUE_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label21.AutoSize = true;
		this.label21.Location = new System.Drawing.Point(28, 88);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(77, 13);
		this.label21.TabIndex = 48;
		this.label21.Text = "Packets Reset";
		this.label22.AutoSize = true;
		this.label22.Location = new System.Drawing.Point(59, 58);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(46, 13);
		this.label22.TabIndex = 47;
		this.label22.Text = "Packets";
		this.AG_BPS_VALUE_TextBox.Location = new System.Drawing.Point(111, 25);
		this.AG_BPS_VALUE_TextBox.Name = "AG_BPS_VALUE_TextBox";
		this.AG_BPS_VALUE_TextBox.Size = new System.Drawing.Size(114, 20);
		this.AG_BPS_VALUE_TextBox.TabIndex = 45;
		this.AG_BPS_VALUE_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label23.AutoSize = true;
		this.label23.Location = new System.Drawing.Point(72, 28);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(33, 13);
		this.label23.TabIndex = 46;
		this.label23.Text = "Bytes";
		this.groupBox7.Controls.Add(this.GATEWAY_PACKET_RESET_TextBox);
		this.groupBox7.Controls.Add(this.GW_PPS_VALUE_TextBox);
		this.groupBox7.Controls.Add(this.label24);
		this.groupBox7.Controls.Add(this.label25);
		this.groupBox7.Controls.Add(this.GW_BPS_VALUE_TextBox);
		this.groupBox7.Controls.Add(this.label26);
		this.groupBox7.Location = new System.Drawing.Point(8, 235);
		this.groupBox7.Name = "groupBox7";
		this.groupBox7.Size = new System.Drawing.Size(237, 130);
		this.groupBox7.TabIndex = 66;
		this.groupBox7.TabStop = false;
		this.groupBox7.Text = "Gateway Packet Settings";
		this.GATEWAY_PACKET_RESET_TextBox.Location = new System.Drawing.Point(111, 85);
		this.GATEWAY_PACKET_RESET_TextBox.Name = "GATEWAY_PACKET_RESET_TextBox";
		this.GATEWAY_PACKET_RESET_TextBox.Size = new System.Drawing.Size(114, 20);
		this.GATEWAY_PACKET_RESET_TextBox.TabIndex = 50;
		this.GATEWAY_PACKET_RESET_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.GW_PPS_VALUE_TextBox.Location = new System.Drawing.Point(111, 55);
		this.GW_PPS_VALUE_TextBox.Name = "GW_PPS_VALUE_TextBox";
		this.GW_PPS_VALUE_TextBox.Size = new System.Drawing.Size(114, 20);
		this.GW_PPS_VALUE_TextBox.TabIndex = 49;
		this.GW_PPS_VALUE_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label24.AutoSize = true;
		this.label24.Location = new System.Drawing.Point(28, 88);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(77, 13);
		this.label24.TabIndex = 48;
		this.label24.Text = "Packets Reset";
		this.label25.AutoSize = true;
		this.label25.Location = new System.Drawing.Point(59, 58);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(46, 13);
		this.label25.TabIndex = 47;
		this.label25.Text = "Packets";
		this.GW_BPS_VALUE_TextBox.Location = new System.Drawing.Point(111, 25);
		this.GW_BPS_VALUE_TextBox.Name = "GW_BPS_VALUE_TextBox";
		this.GW_BPS_VALUE_TextBox.Size = new System.Drawing.Size(114, 20);
		this.GW_BPS_VALUE_TextBox.TabIndex = 45;
		this.GW_BPS_VALUE_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label26.AutoSize = true;
		this.label26.Location = new System.Drawing.Point(72, 28);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(33, 13);
		this.label26.TabIndex = 46;
		this.label26.Text = "Bytes";
		this.groupBox1.Controls.Add(this.textBox_RealDownloadPort);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.textBox_PublicDownloadPort);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.textBox_RealAgentPort);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.textBox_PublicAgentPort);
		this.groupBox1.Controls.Add(this.textBox_RealGatewayPort);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.textBox_Froxy_IP);
		this.groupBox1.Controls.Add(this.Proxy_label);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.textBox_PublicGatewayPort);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.textBox_Server_IP);
		this.groupBox1.Controls.Add(this.Server_label);
		this.groupBox1.Location = new System.Drawing.Point(407, 6);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(376, 223);
		this.groupBox1.TabIndex = 65;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Default Gateway Server";
		this.textBox_RealDownloadPort.Location = new System.Drawing.Point(303, 173);
		this.textBox_RealDownloadPort.MaxLength = 5;
		this.textBox_RealDownloadPort.Name = "textBox_RealDownloadPort";
		this.textBox_RealDownloadPort.Size = new System.Drawing.Size(57, 20);
		this.textBox_RealDownloadPort.TabIndex = 64;
		this.textBox_RealDownloadPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label6.AutoSize = true;
		this.label6.Location = new System.Drawing.Point(201, 176);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(102, 13);
		this.label6.TabIndex = 63;
		this.label6.Text = "Real Download Port";
		this.textBox_PublicDownloadPort.Location = new System.Drawing.Point(120, 173);
		this.textBox_PublicDownloadPort.MaxLength = 5;
		this.textBox_PublicDownloadPort.Name = "textBox_PublicDownloadPort";
		this.textBox_PublicDownloadPort.Size = new System.Drawing.Size(57, 20);
		this.textBox_PublicDownloadPort.TabIndex = 62;
		this.textBox_PublicDownloadPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label7.AutoSize = true;
		this.label7.Location = new System.Drawing.Point(13, 176);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(109, 13);
		this.label7.TabIndex = 61;
		this.label7.Text = "Public Download Port";
		this.textBox_RealAgentPort.Location = new System.Drawing.Point(303, 141);
		this.textBox_RealAgentPort.MaxLength = 5;
		this.textBox_RealAgentPort.Name = "textBox_RealAgentPort";
		this.textBox_RealAgentPort.Size = new System.Drawing.Size(57, 20);
		this.textBox_RealAgentPort.TabIndex = 60;
		this.textBox_RealAgentPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(201, 144);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(82, 13);
		this.label4.TabIndex = 59;
		this.label4.Text = "Real Agent Port";
		this.textBox_PublicAgentPort.Location = new System.Drawing.Point(120, 141);
		this.textBox_PublicAgentPort.MaxLength = 5;
		this.textBox_PublicAgentPort.Name = "textBox_PublicAgentPort";
		this.textBox_PublicAgentPort.Size = new System.Drawing.Size(57, 20);
		this.textBox_PublicAgentPort.TabIndex = 58;
		this.textBox_PublicAgentPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.textBox_RealGatewayPort.Location = new System.Drawing.Point(303, 112);
		this.textBox_RealGatewayPort.MaxLength = 5;
		this.textBox_RealGatewayPort.Name = "textBox_RealGatewayPort";
		this.textBox_RealGatewayPort.Size = new System.Drawing.Size(57, 20);
		this.textBox_RealGatewayPort.TabIndex = 57;
		this.textBox_RealGatewayPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(13, 144);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(89, 13);
		this.label5.TabIndex = 55;
		this.label5.Text = "Public Agent Port";
		this.textBox_Froxy_IP.Location = new System.Drawing.Point(120, 30);
		this.textBox_Froxy_IP.MaxLength = 15;
		this.textBox_Froxy_IP.Name = "textBox_Froxy_IP";
		this.textBox_Froxy_IP.Size = new System.Drawing.Size(114, 20);
		this.textBox_Froxy_IP.TabIndex = 52;
		this.textBox_Froxy_IP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.Proxy_label.AutoSize = true;
		this.Proxy_label.Location = new System.Drawing.Point(11, 33);
		this.Proxy_label.Name = "Proxy_label";
		this.Proxy_label.Size = new System.Drawing.Size(100, 13);
		this.Proxy_label.TabIndex = 5;
		this.Proxy_label.Text = "Froxy Framework IP";
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(201, 115);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(96, 13);
		this.label3.TabIndex = 48;
		this.label3.Text = "Real Gateway Port";
		this.textBox_PublicGatewayPort.Location = new System.Drawing.Point(120, 112);
		this.textBox_PublicGatewayPort.MaxLength = 5;
		this.textBox_PublicGatewayPort.Name = "textBox_PublicGatewayPort";
		this.textBox_PublicGatewayPort.Size = new System.Drawing.Size(57, 20);
		this.textBox_PublicGatewayPort.TabIndex = 49;
		this.textBox_PublicGatewayPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(11, 115);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(103, 13);
		this.label2.TabIndex = 47;
		this.label2.Text = "Public Gateway Port";
		this.textBox_Server_IP.Location = new System.Drawing.Point(120, 61);
		this.textBox_Server_IP.MaxLength = 15;
		this.textBox_Server_IP.Name = "textBox_Server_IP";
		this.textBox_Server_IP.Size = new System.Drawing.Size(114, 20);
		this.textBox_Server_IP.TabIndex = 45;
		this.textBox_Server_IP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.Server_label.AutoSize = true;
		this.Server_label.Location = new System.Drawing.Point(11, 65);
		this.Server_label.Name = "Server_label";
		this.Server_label.Size = new System.Drawing.Size(51, 13);
		this.Server_label.TabIndex = 46;
		this.Server_label.Text = "Server IP";
		this.groupBox4.Controls.Add(this.label13);
		this.groupBox4.Controls.Add(this.textBox_FilterDB);
		this.groupBox4.Controls.Add(this.textBox_LogDB);
		this.groupBox4.Controls.Add(this.textBox_ShardDB);
		this.groupBox4.Controls.Add(this.textBox_AccDB);
		this.groupBox4.Controls.Add(this.textBox_SQLPass);
		this.groupBox4.Controls.Add(this.textBox_SQLId);
		this.groupBox4.Controls.Add(this.textBox_SQLHost);
		this.groupBox4.Controls.Add(this.label12);
		this.groupBox4.Controls.Add(this.label11);
		this.groupBox4.Controls.Add(this.label10);
		this.groupBox4.Controls.Add(this.label14);
		this.groupBox4.Controls.Add(this.label15);
		this.groupBox4.Controls.Add(this.label16);
		this.groupBox4.Location = new System.Drawing.Point(8, 6);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(389, 223);
		this.groupBox4.TabIndex = 4;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "MSSQL Server";
		this.label13.AutoSize = true;
		this.label13.Location = new System.Drawing.Point(20, 198);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(93, 13);
		this.label13.TabIndex = 64;
		this.label13.Text = "FILTER Database";
		this.textBox_FilterDB.Location = new System.Drawing.Point(139, 195);
		this.textBox_FilterDB.Name = "textBox_FilterDB";
		this.textBox_FilterDB.Size = new System.Drawing.Size(224, 20);
		this.textBox_FilterDB.TabIndex = 63;
		this.textBox_FilterDB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.textBox_LogDB.Location = new System.Drawing.Point(139, 166);
		this.textBox_LogDB.Name = "textBox_LogDB";
		this.textBox_LogDB.Size = new System.Drawing.Size(224, 20);
		this.textBox_LogDB.TabIndex = 62;
		this.textBox_LogDB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.textBox_ShardDB.Location = new System.Drawing.Point(139, 138);
		this.textBox_ShardDB.Name = "textBox_ShardDB";
		this.textBox_ShardDB.Size = new System.Drawing.Size(224, 20);
		this.textBox_ShardDB.TabIndex = 61;
		this.textBox_ShardDB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.textBox_AccDB.Location = new System.Drawing.Point(139, 110);
		this.textBox_AccDB.Name = "textBox_AccDB";
		this.textBox_AccDB.Size = new System.Drawing.Size(224, 20);
		this.textBox_AccDB.TabIndex = 60;
		this.textBox_AccDB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.textBox_SQLPass.Location = new System.Drawing.Point(139, 82);
		this.textBox_SQLPass.Name = "textBox_SQLPass";
		this.textBox_SQLPass.Size = new System.Drawing.Size(224, 20);
		this.textBox_SQLPass.TabIndex = 59;
		this.textBox_SQLPass.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.textBox_SQLPass.UseSystemPasswordChar = true;
		this.textBox_SQLId.Location = new System.Drawing.Point(139, 54);
		this.textBox_SQLId.Name = "textBox_SQLId";
		this.textBox_SQLId.Size = new System.Drawing.Size(224, 20);
		this.textBox_SQLId.TabIndex = 58;
		this.textBox_SQLId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.textBox_SQLHost.Location = new System.Drawing.Point(139, 26);
		this.textBox_SQLHost.Name = "textBox_SQLHost";
		this.textBox_SQLHost.Size = new System.Drawing.Size(224, 20);
		this.textBox_SQLHost.TabIndex = 57;
		this.textBox_SQLHost.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label12.AutoSize = true;
		this.label12.Location = new System.Drawing.Point(20, 169);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(78, 13);
		this.label12.TabIndex = 56;
		this.label12.Text = "LOG Database";
		this.label11.AutoSize = true;
		this.label11.Location = new System.Drawing.Point(20, 141);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(94, 13);
		this.label11.TabIndex = 55;
		this.label11.Text = "SHARD Database";
		this.label10.AutoSize = true;
		this.label10.Location = new System.Drawing.Point(20, 113);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(108, 13);
		this.label10.TabIndex = 54;
		this.label10.Text = "ACCOUNT Database";
		this.label14.AutoSize = true;
		this.label14.Location = new System.Drawing.Point(20, 85);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(74, 13);
		this.label14.TabIndex = 53;
		this.label14.Text = "Instance Pass";
		this.label15.AutoSize = true;
		this.label15.Location = new System.Drawing.Point(20, 57);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(62, 13);
		this.label15.TabIndex = 52;
		this.label15.Text = "Instance ID";
		this.label16.AutoSize = true;
		this.label16.Location = new System.Drawing.Point(20, 29);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(79, 13);
		this.label16.TabIndex = 51;
		this.label16.Text = "Instance Name";
		this.tabPage2.BackColor = System.Drawing.Color.WhiteSmoke;
		this.tabPage2.Controls.Add(this.groupBox12);
		this.tabPage2.Location = new System.Drawing.Point(4, 22);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1033, 453);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Limit Settings";
		this.groupBox12.BackColor = System.Drawing.Color.WhiteSmoke;
		this.groupBox12.Controls.Add(this.label68);
		this.groupBox12.Controls.Add(this.label62);
		this.groupBox12.Controls.Add(this.label63);
		this.groupBox12.Controls.Add(this.label65);
		this.groupBox12.Controls.Add(this.label66);
		this.groupBox12.Controls.Add(this.label67);
		this.groupBox12.Controls.Add(this.label69);
		this.groupBox12.Controls.Add(this.label61);
		this.groupBox12.Controls.Add(this.label60);
		this.groupBox12.Controls.Add(this.label59);
		this.groupBox12.Controls.Add(this.label58);
		this.groupBox12.Controls.Add(this.label57);
		this.groupBox12.Controls.Add(this.label56);
		this.groupBox12.Controls.Add(this.label55);
		this.groupBox12.Controls.Add(this.label54);
		this.groupBox12.Controls.Add(this.CAFE_IP_LIMITNoticeTextBox);
		this.groupBox12.Controls.Add(this.label51);
		this.groupBox12.Controls.Add(this.PLUS_LIMITNoticeTextBox);
		this.groupBox12.Controls.Add(this.label52);
		this.groupBox12.Controls.Add(this.DEVIL_PLUS_LIMITNoticeTextBox);
		this.groupBox12.Controls.Add(this.JOBT_PC_LIMITNoticeTextBox);
		this.groupBox12.Controls.Add(this.label8);
		this.groupBox12.Controls.Add(this.FGW_PC_LIMITNoticeTextBox);
		this.groupBox12.Controls.Add(this.label9);
		this.groupBox12.Controls.Add(this.JUPITER_PC_LIMITNoticeTextBox);
		this.groupBox12.Controls.Add(this.label32);
		this.groupBox12.Controls.Add(this.SURVIVAL_PC_LIMITNoticeTextBox);
		this.groupBox12.Controls.Add(this.label33);
		this.groupBox12.Controls.Add(this.HT_PC_LIMITNoticeTextBox);
		this.groupBox12.Controls.Add(this.label47);
		this.groupBox12.Controls.Add(this.PC_LIMITNoticeTextBox);
		this.groupBox12.Controls.Add(this.label31);
		this.groupBox12.Controls.Add(this.BA_PC_LIMITNoticeTextBox);
		this.groupBox12.Controls.Add(this.label20);
		this.groupBox12.Controls.Add(this.CTF_PC_LIMITNoticeTextBox);
		this.groupBox12.Controls.Add(this.label19);
		this.groupBox12.Controls.Add(this.FTW_PC_LIMITNoticeTextBox);
		this.groupBox12.Controls.Add(this.label18);
		this.groupBox12.Controls.Add(this.JOB_PC_LIMITNoticeTextBox);
		this.groupBox12.Controls.Add(this.label17);
		this.groupBox12.Controls.Add(this.IP_LIMITNoticeTextBox);
		this.groupBox12.Controls.Add(this.label1);
		this.groupBox12.Controls.Add(this.label49);
		this.groupBox12.Controls.Add(this.label48);
		this.groupBox12.Controls.Add(this.label46);
		this.groupBox12.Controls.Add(this.label44);
		this.groupBox12.Controls.Add(this.label43);
		this.groupBox12.Controls.Add(this.label42);
		this.groupBox12.Controls.Add(this.label41);
		this.groupBox12.Controls.Add(this.label40);
		this.groupBox12.Controls.Add(this.label39);
		this.groupBox12.Controls.Add(this.label37);
		this.groupBox12.Controls.Add(this.label36);
		this.groupBox12.Controls.Add(this.label38);
		this.groupBox12.Controls.Add(this.label35);
		this.groupBox12.Controls.Add(this.JUPITER_PC_LIMITTextBox);
		this.groupBox12.Controls.Add(this.FGW_PC_LIMITTextBox);
		this.groupBox12.Controls.Add(this.JOBT_PC_LIMITTextBox);
		this.groupBox12.Controls.Add(this.SURVIVAL_PC_LIMITTextBox);
		this.groupBox12.Controls.Add(this.HT_PC_LIMITTextBox);
		this.groupBox12.Controls.Add(this.CAFE_IP_LIMITTextBox);
		this.groupBox12.Controls.Add(this.PLUS_LIMITTextBox);
		this.groupBox12.Controls.Add(this.DEVIL_PLUS_LIMITTextBox);
		this.groupBox12.Controls.Add(this.PC_LIMITTextBox);
		this.groupBox12.Controls.Add(this.BA_PC_LIMITTextBox);
		this.groupBox12.Controls.Add(this.CTF_PC_LIMITTextBox);
		this.groupBox12.Controls.Add(this.FTW_PC_LIMITTextBox);
		this.groupBox12.Controls.Add(this.JOB_PC_LIMITTextBox);
		this.groupBox12.Controls.Add(this.IP_LIMITTextBox);
		this.groupBox12.Controls.Add(this.label34);
		this.groupBox12.Location = new System.Drawing.Point(7, 6);
		this.groupBox12.Name = "groupBox12";
		this.groupBox12.Size = new System.Drawing.Size(1019, 412);
		this.groupBox12.TabIndex = 4;
		this.groupBox12.TabStop = false;
		this.groupBox12.Text = "Limit Settings";
		this.label68.AutoSize = true;
		this.label68.Location = new System.Drawing.Point(275, 372);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(38, 13);
		this.label68.TabIndex = 139;
		this.label68.Text = "Notice";
		this.label62.AutoSize = true;
		this.label62.Location = new System.Drawing.Point(250, 267);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(19, 13);
		this.label62.TabIndex = 137;
		this.label62.Text = ">>";
		this.label63.AutoSize = true;
		this.label63.Location = new System.Drawing.Point(250, 293);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(19, 13);
		this.label63.TabIndex = 136;
		this.label63.Text = ">>";
		this.label65.AutoSize = true;
		this.label65.Location = new System.Drawing.Point(250, 319);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(19, 13);
		this.label65.TabIndex = 134;
		this.label65.Text = ">>";
		this.label66.AutoSize = true;
		this.label66.Location = new System.Drawing.Point(250, 342);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(19, 13);
		this.label66.TabIndex = 133;
		this.label66.Text = ">>";
		this.label67.AutoSize = true;
		this.label67.Location = new System.Drawing.Point(250, 372);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(19, 13);
		this.label67.TabIndex = 132;
		this.label67.Text = ">>";
		this.label69.AutoSize = true;
		this.label69.Location = new System.Drawing.Point(250, 241);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(19, 13);
		this.label69.TabIndex = 130;
		this.label69.Text = ">>";
		this.label61.AutoSize = true;
		this.label61.Location = new System.Drawing.Point(250, 59);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(19, 13);
		this.label61.TabIndex = 129;
		this.label61.Text = ">>";
		this.label60.AutoSize = true;
		this.label60.Location = new System.Drawing.Point(250, 85);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(19, 13);
		this.label60.TabIndex = 128;
		this.label60.Text = ">>";
		this.label59.AutoSize = true;
		this.label59.Location = new System.Drawing.Point(250, 111);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(19, 13);
		this.label59.TabIndex = 127;
		this.label59.Text = ">>";
		this.label58.AutoSize = true;
		this.label58.Location = new System.Drawing.Point(250, 137);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(19, 13);
		this.label58.TabIndex = 126;
		this.label58.Text = ">>";
		this.label57.AutoSize = true;
		this.label57.Location = new System.Drawing.Point(250, 163);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(19, 13);
		this.label57.TabIndex = 125;
		this.label57.Text = ">>";
		this.label56.AutoSize = true;
		this.label56.Location = new System.Drawing.Point(250, 189);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(19, 13);
		this.label56.TabIndex = 124;
		this.label56.Text = ">>";
		this.label55.AutoSize = true;
		this.label55.Location = new System.Drawing.Point(250, 215);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(19, 13);
		this.label55.TabIndex = 123;
		this.label55.Text = ">>";
		this.label54.AutoSize = true;
		this.label54.Location = new System.Drawing.Point(250, 33);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(19, 13);
		this.label54.TabIndex = 122;
		this.label54.Text = ">>";
		this.CAFE_IP_LIMITNoticeTextBox.Location = new System.Drawing.Point(319, 316);
		this.CAFE_IP_LIMITNoticeTextBox.Name = "CAFE_IP_LIMITNoticeTextBox";
		this.CAFE_IP_LIMITNoticeTextBox.Size = new System.Drawing.Size(657, 20);
		this.CAFE_IP_LIMITNoticeTextBox.TabIndex = 121;
		this.label51.AutoSize = true;
		this.label51.Location = new System.Drawing.Point(275, 320);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(38, 13);
		this.label51.TabIndex = 120;
		this.label51.Text = "Notice";
		this.PLUS_LIMITNoticeTextBox.Location = new System.Drawing.Point(319, 342);
		this.PLUS_LIMITNoticeTextBox.Name = "PLUS_LIMITNoticeTextBox";
		this.PLUS_LIMITNoticeTextBox.Size = new System.Drawing.Size(657, 20);
		this.PLUS_LIMITNoticeTextBox.TabIndex = 119;
		this.label52.AutoSize = true;
		this.label52.Location = new System.Drawing.Point(275, 345);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(38, 13);
		this.label52.TabIndex = 118;
		this.label52.Text = "Notice";
		this.DEVIL_PLUS_LIMITNoticeTextBox.Location = new System.Drawing.Point(319, 368);
		this.DEVIL_PLUS_LIMITNoticeTextBox.Name = "DEVIL_PLUS_LIMITNoticeTextBox";
		this.DEVIL_PLUS_LIMITNoticeTextBox.Size = new System.Drawing.Size(657, 20);
		this.DEVIL_PLUS_LIMITNoticeTextBox.TabIndex = 117;
		this.JOBT_PC_LIMITNoticeTextBox.Location = new System.Drawing.Point(319, 212);
		this.JOBT_PC_LIMITNoticeTextBox.Name = "JOBT_PC_LIMITNoticeTextBox";
		this.JOBT_PC_LIMITNoticeTextBox.Size = new System.Drawing.Size(657, 20);
		this.JOBT_PC_LIMITNoticeTextBox.TabIndex = 115;
		this.label8.AutoSize = true;
		this.label8.Location = new System.Drawing.Point(275, 215);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(38, 13);
		this.label8.TabIndex = 114;
		this.label8.Text = "Notice";
		this.FGW_PC_LIMITNoticeTextBox.Location = new System.Drawing.Point(319, 238);
		this.FGW_PC_LIMITNoticeTextBox.Name = "FGW_PC_LIMITNoticeTextBox";
		this.FGW_PC_LIMITNoticeTextBox.Size = new System.Drawing.Size(657, 20);
		this.FGW_PC_LIMITNoticeTextBox.TabIndex = 113;
		this.label9.AutoSize = true;
		this.label9.Location = new System.Drawing.Point(275, 241);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(38, 13);
		this.label9.TabIndex = 112;
		this.label9.Text = "Notice";
		this.JUPITER_PC_LIMITNoticeTextBox.Location = new System.Drawing.Point(319, 264);
		this.JUPITER_PC_LIMITNoticeTextBox.Name = "JUPITER_PC_LIMITNoticeTextBox";
		this.JUPITER_PC_LIMITNoticeTextBox.Size = new System.Drawing.Size(657, 20);
		this.JUPITER_PC_LIMITNoticeTextBox.TabIndex = 111;
		this.label32.AutoSize = true;
		this.label32.Location = new System.Drawing.Point(275, 267);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(38, 13);
		this.label32.TabIndex = 110;
		this.label32.Text = "Notice";
		this.SURVIVAL_PC_LIMITNoticeTextBox.Location = new System.Drawing.Point(319, 290);
		this.SURVIVAL_PC_LIMITNoticeTextBox.Name = "SURVIVAL_PC_LIMITNoticeTextBox";
		this.SURVIVAL_PC_LIMITNoticeTextBox.Size = new System.Drawing.Size(657, 20);
		this.SURVIVAL_PC_LIMITNoticeTextBox.TabIndex = 109;
		this.label33.AutoSize = true;
		this.label33.Location = new System.Drawing.Point(275, 294);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(38, 13);
		this.label33.TabIndex = 108;
		this.label33.Text = "Notice";
		this.HT_PC_LIMITNoticeTextBox.Location = new System.Drawing.Point(319, 186);
		this.HT_PC_LIMITNoticeTextBox.Name = "HT_PC_LIMITNoticeTextBox";
		this.HT_PC_LIMITNoticeTextBox.Size = new System.Drawing.Size(657, 20);
		this.HT_PC_LIMITNoticeTextBox.TabIndex = 105;
		this.label47.AutoSize = true;
		this.label47.Location = new System.Drawing.Point(275, 189);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(38, 13);
		this.label47.TabIndex = 104;
		this.label47.Text = "Notice";
		this.PC_LIMITNoticeTextBox.Location = new System.Drawing.Point(319, 56);
		this.PC_LIMITNoticeTextBox.Name = "PC_LIMITNoticeTextBox";
		this.PC_LIMITNoticeTextBox.Size = new System.Drawing.Size(657, 20);
		this.PC_LIMITNoticeTextBox.TabIndex = 103;
		this.label31.AutoSize = true;
		this.label31.Location = new System.Drawing.Point(275, 59);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(38, 13);
		this.label31.TabIndex = 102;
		this.label31.Text = "Notice";
		this.BA_PC_LIMITNoticeTextBox.Location = new System.Drawing.Point(319, 82);
		this.BA_PC_LIMITNoticeTextBox.Name = "BA_PC_LIMITNoticeTextBox";
		this.BA_PC_LIMITNoticeTextBox.Size = new System.Drawing.Size(657, 20);
		this.BA_PC_LIMITNoticeTextBox.TabIndex = 101;
		this.label20.AutoSize = true;
		this.label20.Location = new System.Drawing.Point(275, 85);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(38, 13);
		this.label20.TabIndex = 100;
		this.label20.Text = "Notice";
		this.CTF_PC_LIMITNoticeTextBox.Location = new System.Drawing.Point(319, 108);
		this.CTF_PC_LIMITNoticeTextBox.Name = "CTF_PC_LIMITNoticeTextBox";
		this.CTF_PC_LIMITNoticeTextBox.Size = new System.Drawing.Size(657, 20);
		this.CTF_PC_LIMITNoticeTextBox.TabIndex = 99;
		this.label19.AutoSize = true;
		this.label19.Location = new System.Drawing.Point(275, 111);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(38, 13);
		this.label19.TabIndex = 98;
		this.label19.Text = "Notice";
		this.FTW_PC_LIMITNoticeTextBox.Location = new System.Drawing.Point(319, 134);
		this.FTW_PC_LIMITNoticeTextBox.Name = "FTW_PC_LIMITNoticeTextBox";
		this.FTW_PC_LIMITNoticeTextBox.Size = new System.Drawing.Size(657, 20);
		this.FTW_PC_LIMITNoticeTextBox.TabIndex = 97;
		this.label18.AutoSize = true;
		this.label18.Location = new System.Drawing.Point(275, 137);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(38, 13);
		this.label18.TabIndex = 96;
		this.label18.Text = "Notice";
		this.JOB_PC_LIMITNoticeTextBox.Location = new System.Drawing.Point(319, 160);
		this.JOB_PC_LIMITNoticeTextBox.Name = "JOB_PC_LIMITNoticeTextBox";
		this.JOB_PC_LIMITNoticeTextBox.Size = new System.Drawing.Size(657, 20);
		this.JOB_PC_LIMITNoticeTextBox.TabIndex = 95;
		this.label17.AutoSize = true;
		this.label17.Location = new System.Drawing.Point(275, 163);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(38, 13);
		this.label17.TabIndex = 94;
		this.label17.Text = "Notice";
		this.IP_LIMITNoticeTextBox.Location = new System.Drawing.Point(319, 30);
		this.IP_LIMITNoticeTextBox.Name = "IP_LIMITNoticeTextBox";
		this.IP_LIMITNoticeTextBox.Size = new System.Drawing.Size(657, 20);
		this.IP_LIMITNoticeTextBox.TabIndex = 89;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(275, 33);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(38, 13);
		this.label1.TabIndex = 88;
		this.label1.Text = "Notice";
		this.label49.AutoSize = true;
		this.label49.Location = new System.Drawing.Point(70, 320);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(53, 13);
		this.label49.TabIndex = 86;
		this.label49.Text = "Cafe Limit";
		this.label48.AutoSize = true;
		this.label48.Location = new System.Drawing.Point(74, 346);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(51, 13);
		this.label48.TabIndex = 85;
		this.label48.Text = "Plus Limit";
		this.label46.AutoSize = true;
		this.label46.Location = new System.Drawing.Point(47, 372);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(78, 13);
		this.label46.TabIndex = 83;
		this.label46.Text = "Devil Plus Limit";
		this.label44.AutoSize = true;
		this.label44.Location = new System.Drawing.Point(22, 85);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(106, 13);
		this.label44.TabIndex = 81;
		this.label44.Text = "Battle Arena PC Limit";
		this.label43.AutoSize = true;
		this.label43.Location = new System.Drawing.Point(39, 137);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(85, 13);
		this.label43.TabIndex = 80;
		this.label43.Text = "Fortress PC Limit";
		this.label42.AutoSize = true;
		this.label42.Location = new System.Drawing.Point(57, 163);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(65, 13);
		this.label42.TabIndex = 79;
		this.label42.Text = "Job PC Limit";
		this.label41.AutoSize = true;
		this.label41.Location = new System.Drawing.Point(57, 241);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(68, 13);
		this.label41.TabIndex = 78;
		this.label41.Text = "Fgw PC Limit";
		this.label40.AutoSize = true;
		this.label40.Location = new System.Drawing.Point(46, 267);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(79, 13);
		this.label40.TabIndex = 77;
		this.label40.Text = "Jupiter PC Limit";
		this.label39.AutoSize = true;
		this.label39.Location = new System.Drawing.Point(18, 189);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(107, 13);
		this.label39.TabIndex = 76;
		this.label39.Text = "Holy Temple PC Limit";
		this.label37.AutoSize = true;
		this.label37.Location = new System.Drawing.Point(22, 215);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(103, 13);
		this.label37.TabIndex = 74;
		this.label37.Text = "Job Temple PC Limit";
		this.label36.AutoSize = true;
		this.label36.Location = new System.Drawing.Point(57, 111);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(68, 13);
		this.label36.TabIndex = 73;
		this.label36.Text = "CTF PC Limit";
		this.label38.AutoSize = true;
		this.label38.Location = new System.Drawing.Point(39, 294);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(86, 13);
		this.label38.TabIndex = 75;
		this.label38.Text = "Survival PC Limit";
		this.label35.AutoSize = true;
		this.label35.Location = new System.Drawing.Point(80, 59);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(45, 13);
		this.label35.TabIndex = 72;
		this.label35.Text = "PC Limit";
		this.JUPITER_PC_LIMITTextBox.Location = new System.Drawing.Point(131, 264);
		this.JUPITER_PC_LIMITTextBox.Name = "JUPITER_PC_LIMITTextBox";
		this.JUPITER_PC_LIMITTextBox.Size = new System.Drawing.Size(105, 20);
		this.JUPITER_PC_LIMITTextBox.TabIndex = 56;
		this.JUPITER_PC_LIMITTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.FGW_PC_LIMITTextBox.Location = new System.Drawing.Point(131, 238);
		this.FGW_PC_LIMITTextBox.Name = "FGW_PC_LIMITTextBox";
		this.FGW_PC_LIMITTextBox.Size = new System.Drawing.Size(105, 20);
		this.FGW_PC_LIMITTextBox.TabIndex = 57;
		this.FGW_PC_LIMITTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.JOBT_PC_LIMITTextBox.Location = new System.Drawing.Point(131, 212);
		this.JOBT_PC_LIMITTextBox.Name = "JOBT_PC_LIMITTextBox";
		this.JOBT_PC_LIMITTextBox.Size = new System.Drawing.Size(105, 20);
		this.JOBT_PC_LIMITTextBox.TabIndex = 70;
		this.JOBT_PC_LIMITTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.SURVIVAL_PC_LIMITTextBox.Location = new System.Drawing.Point(131, 291);
		this.SURVIVAL_PC_LIMITTextBox.Name = "SURVIVAL_PC_LIMITTextBox";
		this.SURVIVAL_PC_LIMITTextBox.Size = new System.Drawing.Size(105, 20);
		this.SURVIVAL_PC_LIMITTextBox.TabIndex = 63;
		this.SURVIVAL_PC_LIMITTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.HT_PC_LIMITTextBox.Location = new System.Drawing.Point(131, 186);
		this.HT_PC_LIMITTextBox.Name = "HT_PC_LIMITTextBox";
		this.HT_PC_LIMITTextBox.Size = new System.Drawing.Size(105, 20);
		this.HT_PC_LIMITTextBox.TabIndex = 69;
		this.HT_PC_LIMITTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.CAFE_IP_LIMITTextBox.Location = new System.Drawing.Point(131, 317);
		this.CAFE_IP_LIMITTextBox.Name = "CAFE_IP_LIMITTextBox";
		this.CAFE_IP_LIMITTextBox.Size = new System.Drawing.Size(105, 20);
		this.CAFE_IP_LIMITTextBox.TabIndex = 68;
		this.CAFE_IP_LIMITTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.PLUS_LIMITTextBox.Location = new System.Drawing.Point(131, 343);
		this.PLUS_LIMITTextBox.Name = "PLUS_LIMITTextBox";
		this.PLUS_LIMITTextBox.Size = new System.Drawing.Size(105, 20);
		this.PLUS_LIMITTextBox.TabIndex = 67;
		this.PLUS_LIMITTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.DEVIL_PLUS_LIMITTextBox.Location = new System.Drawing.Point(131, 369);
		this.DEVIL_PLUS_LIMITTextBox.Name = "DEVIL_PLUS_LIMITTextBox";
		this.DEVIL_PLUS_LIMITTextBox.Size = new System.Drawing.Size(105, 20);
		this.DEVIL_PLUS_LIMITTextBox.TabIndex = 64;
		this.DEVIL_PLUS_LIMITTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.PC_LIMITTextBox.Location = new System.Drawing.Point(131, 56);
		this.PC_LIMITTextBox.Name = "PC_LIMITTextBox";
		this.PC_LIMITTextBox.Size = new System.Drawing.Size(105, 20);
		this.PC_LIMITTextBox.TabIndex = 62;
		this.PC_LIMITTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.BA_PC_LIMITTextBox.Location = new System.Drawing.Point(131, 82);
		this.BA_PC_LIMITTextBox.Name = "BA_PC_LIMITTextBox";
		this.BA_PC_LIMITTextBox.Size = new System.Drawing.Size(105, 20);
		this.BA_PC_LIMITTextBox.TabIndex = 61;
		this.BA_PC_LIMITTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.CTF_PC_LIMITTextBox.Location = new System.Drawing.Point(131, 108);
		this.CTF_PC_LIMITTextBox.Name = "CTF_PC_LIMITTextBox";
		this.CTF_PC_LIMITTextBox.Size = new System.Drawing.Size(105, 20);
		this.CTF_PC_LIMITTextBox.TabIndex = 60;
		this.CTF_PC_LIMITTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.FTW_PC_LIMITTextBox.Location = new System.Drawing.Point(131, 134);
		this.FTW_PC_LIMITTextBox.Name = "FTW_PC_LIMITTextBox";
		this.FTW_PC_LIMITTextBox.Size = new System.Drawing.Size(105, 20);
		this.FTW_PC_LIMITTextBox.TabIndex = 59;
		this.FTW_PC_LIMITTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.JOB_PC_LIMITTextBox.Location = new System.Drawing.Point(131, 160);
		this.JOB_PC_LIMITTextBox.Name = "JOB_PC_LIMITTextBox";
		this.JOB_PC_LIMITTextBox.Size = new System.Drawing.Size(105, 20);
		this.JOB_PC_LIMITTextBox.TabIndex = 58;
		this.JOB_PC_LIMITTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.IP_LIMITTextBox.Location = new System.Drawing.Point(131, 30);
		this.IP_LIMITTextBox.Name = "IP_LIMITTextBox";
		this.IP_LIMITTextBox.Size = new System.Drawing.Size(105, 20);
		this.IP_LIMITTextBox.TabIndex = 55;
		this.IP_LIMITTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label34.AutoSize = true;
		this.label34.Location = new System.Drawing.Point(84, 33);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(41, 13);
		this.label34.TabIndex = 54;
		this.label34.Text = "IP Limit";
		this.tabPage3.BackColor = System.Drawing.Color.WhiteSmoke;
		this.tabPage3.Controls.Add(this.groupBox3);
		this.tabPage3.Controls.Add(this.groupBox2);
		this.tabPage3.Location = new System.Drawing.Point(4, 22);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1033, 453);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "level & Delay";
		this.groupBox3.Controls.Add(this.label75);
		this.groupBox3.Controls.Add(this.label71);
		this.groupBox3.Controls.Add(this.DROP_GOLD_LEVEL_TextBox);
		this.groupBox3.Controls.Add(this.label70);
		this.groupBox3.Controls.Add(this.DROP_LEVEL_NOTICE_TextBox);
		this.groupBox3.Controls.Add(this.label116);
		this.groupBox3.Controls.Add(this.label110);
		this.groupBox3.Controls.Add(this.label117);
		this.groupBox3.Controls.Add(this.label111);
		this.groupBox3.Controls.Add(this.label118);
		this.groupBox3.Controls.Add(this.label112);
		this.groupBox3.Controls.Add(this.label119);
		this.groupBox3.Controls.Add(this.label113);
		this.groupBox3.Controls.Add(this.label120);
		this.groupBox3.Controls.Add(this.label114);
		this.groupBox3.Controls.Add(this.ZERK_LEVEL_TextBox);
		this.groupBox3.Controls.Add(this.STALL_LEVEL_TextBox);
		this.groupBox3.Controls.Add(this.label123);
		this.groupBox3.Controls.Add(this.GLOBAL_LEVEL_TextBox);
		this.groupBox3.Controls.Add(this.EXCHANGE_LEVEL_TextBox);
		this.groupBox3.Controls.Add(this.CTF_REQ_LEVEL_TextBox);
		this.groupBox3.Controls.Add(this.label115);
		this.groupBox3.Controls.Add(this.BA_REQ_LEVEL_TextBox);
		this.groupBox3.Controls.Add(this.CTF_REQ_LEVEL_NOTICE_TextBox);
		this.groupBox3.Controls.Add(this.BA_REQ_LEVEL_NOTICE_TextBox);
		this.groupBox3.Controls.Add(this.label126);
		this.groupBox3.Controls.Add(this.label131);
		this.groupBox3.Controls.Add(this.EXCHENGE_LEVEL_NOTICE_TextBox);
		this.groupBox3.Controls.Add(this.label130);
		this.groupBox3.Controls.Add(this.label127);
		this.groupBox3.Controls.Add(this.ZERK_LEVEL_NOTICE_TextBox);
		this.groupBox3.Controls.Add(this.GLOBAL_LEVEL_NOTICE_TextBox);
		this.groupBox3.Controls.Add(this.label129);
		this.groupBox3.Controls.Add(this.label128);
		this.groupBox3.Controls.Add(this.STALL_LEVEL_NOTICE_TextBox);
		this.groupBox3.Location = new System.Drawing.Point(6, 248);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1021, 198);
		this.groupBox3.TabIndex = 87;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Level Settings";
		this.label75.AutoSize = true;
		this.label75.Location = new System.Drawing.Point(4, 173);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(84, 13);
		this.label75.TabIndex = 183;
		this.label75.Text = "Drop Gold Level";
		this.label71.AutoSize = true;
		this.label71.Location = new System.Drawing.Point(244, 173);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(38, 13);
		this.label71.TabIndex = 182;
		this.label71.Text = "Notice";
		this.DROP_GOLD_LEVEL_TextBox.Location = new System.Drawing.Point(97, 169);
		this.DROP_GOLD_LEVEL_TextBox.MaxLength = 3;
		this.DROP_GOLD_LEVEL_TextBox.Name = "DROP_GOLD_LEVEL_TextBox";
		this.DROP_GOLD_LEVEL_TextBox.Size = new System.Drawing.Size(97, 20);
		this.DROP_GOLD_LEVEL_TextBox.TabIndex = 178;
		this.DROP_GOLD_LEVEL_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label70.AutoSize = true;
		this.label70.Location = new System.Drawing.Point(210, 172);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(19, 13);
		this.label70.TabIndex = 180;
		this.label70.Text = ">>";
		this.DROP_LEVEL_NOTICE_TextBox.Location = new System.Drawing.Point(288, 169);
		this.DROP_LEVEL_NOTICE_TextBox.Name = "DROP_LEVEL_NOTICE_TextBox";
		this.DROP_LEVEL_NOTICE_TextBox.Size = new System.Drawing.Size(707, 20);
		this.DROP_LEVEL_NOTICE_TextBox.TabIndex = 179;
		this.label116.AutoSize = true;
		this.label116.Location = new System.Drawing.Point(210, 44);
		this.label116.Name = "label116";
		this.label116.Size = new System.Drawing.Size(19, 13);
		this.label116.TabIndex = 177;
		this.label116.Text = ">>";
		this.label110.AutoSize = true;
		this.label110.Location = new System.Drawing.Point(33, 148);
		this.label110.Name = "label110";
		this.label110.Size = new System.Drawing.Size(58, 13);
		this.label110.TabIndex = 63;
		this.label110.Text = "Zerk Level";
		this.label117.AutoSize = true;
		this.label117.Location = new System.Drawing.Point(210, 70);
		this.label117.Name = "label117";
		this.label117.Size = new System.Drawing.Size(19, 13);
		this.label117.TabIndex = 176;
		this.label117.Text = ">>";
		this.label111.AutoSize = true;
		this.label111.Location = new System.Drawing.Point(35, 122);
		this.label111.Name = "label111";
		this.label111.Size = new System.Drawing.Size(56, 13);
		this.label111.TabIndex = 64;
		this.label111.Text = "Stall Level";
		this.label118.AutoSize = true;
		this.label118.Location = new System.Drawing.Point(210, 96);
		this.label118.Name = "label118";
		this.label118.Size = new System.Drawing.Size(19, 13);
		this.label118.TabIndex = 175;
		this.label118.Text = ">>";
		this.label112.AutoSize = true;
		this.label112.Location = new System.Drawing.Point(25, 96);
		this.label112.Name = "label112";
		this.label112.Size = new System.Drawing.Size(66, 13);
		this.label112.TabIndex = 65;
		this.label112.Text = "Global Level";
		this.label119.AutoSize = true;
		this.label119.Location = new System.Drawing.Point(210, 122);
		this.label119.Name = "label119";
		this.label119.Size = new System.Drawing.Size(19, 13);
		this.label119.TabIndex = 174;
		this.label119.Text = ">>";
		this.label113.AutoSize = true;
		this.label113.Location = new System.Drawing.Point(7, 70);
		this.label113.Name = "label113";
		this.label113.Size = new System.Drawing.Size(84, 13);
		this.label113.TabIndex = 66;
		this.label113.Text = "Exchange Level";
		this.label120.AutoSize = true;
		this.label120.Location = new System.Drawing.Point(210, 148);
		this.label120.Name = "label120";
		this.label120.Size = new System.Drawing.Size(19, 13);
		this.label120.TabIndex = 173;
		this.label120.Text = ">>";
		this.label114.AutoSize = true;
		this.label114.Location = new System.Drawing.Point(12, 44);
		this.label114.Name = "label114";
		this.label114.Size = new System.Drawing.Size(79, 13);
		this.label114.TabIndex = 62;
		this.label114.Text = "CTF Req Level";
		this.ZERK_LEVEL_TextBox.Location = new System.Drawing.Point(97, 145);
		this.ZERK_LEVEL_TextBox.Name = "ZERK_LEVEL_TextBox";
		this.ZERK_LEVEL_TextBox.Size = new System.Drawing.Size(97, 20);
		this.ZERK_LEVEL_TextBox.TabIndex = 59;
		this.ZERK_LEVEL_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.STALL_LEVEL_TextBox.Location = new System.Drawing.Point(97, 119);
		this.STALL_LEVEL_TextBox.Name = "STALL_LEVEL_TextBox";
		this.STALL_LEVEL_TextBox.Size = new System.Drawing.Size(97, 20);
		this.STALL_LEVEL_TextBox.TabIndex = 58;
		this.STALL_LEVEL_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label123.AutoSize = true;
		this.label123.Location = new System.Drawing.Point(210, 18);
		this.label123.Name = "label123";
		this.label123.Size = new System.Drawing.Size(19, 13);
		this.label123.TabIndex = 170;
		this.label123.Text = ">>";
		this.GLOBAL_LEVEL_TextBox.Location = new System.Drawing.Point(97, 93);
		this.GLOBAL_LEVEL_TextBox.Name = "GLOBAL_LEVEL_TextBox";
		this.GLOBAL_LEVEL_TextBox.Size = new System.Drawing.Size(97, 20);
		this.GLOBAL_LEVEL_TextBox.TabIndex = 57;
		this.GLOBAL_LEVEL_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.EXCHANGE_LEVEL_TextBox.Location = new System.Drawing.Point(97, 67);
		this.EXCHANGE_LEVEL_TextBox.Name = "EXCHANGE_LEVEL_TextBox";
		this.EXCHANGE_LEVEL_TextBox.Size = new System.Drawing.Size(97, 20);
		this.EXCHANGE_LEVEL_TextBox.TabIndex = 56;
		this.EXCHANGE_LEVEL_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.CTF_REQ_LEVEL_TextBox.Location = new System.Drawing.Point(97, 41);
		this.CTF_REQ_LEVEL_TextBox.Name = "CTF_REQ_LEVEL_TextBox";
		this.CTF_REQ_LEVEL_TextBox.Size = new System.Drawing.Size(97, 20);
		this.CTF_REQ_LEVEL_TextBox.TabIndex = 55;
		this.CTF_REQ_LEVEL_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label115.AutoSize = true;
		this.label115.Location = new System.Drawing.Point(4, 18);
		this.label115.Name = "label115";
		this.label115.Size = new System.Drawing.Size(87, 13);
		this.label115.TabIndex = 54;
		this.label115.Text = "Arena Req Level";
		this.BA_REQ_LEVEL_TextBox.Location = new System.Drawing.Point(97, 15);
		this.BA_REQ_LEVEL_TextBox.Name = "BA_REQ_LEVEL_TextBox";
		this.BA_REQ_LEVEL_TextBox.Size = new System.Drawing.Size(97, 20);
		this.BA_REQ_LEVEL_TextBox.TabIndex = 3;
		this.BA_REQ_LEVEL_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.CTF_REQ_LEVEL_NOTICE_TextBox.Location = new System.Drawing.Point(288, 41);
		this.CTF_REQ_LEVEL_NOTICE_TextBox.Name = "CTF_REQ_LEVEL_NOTICE_TextBox";
		this.CTF_REQ_LEVEL_NOTICE_TextBox.Size = new System.Drawing.Size(707, 20);
		this.CTF_REQ_LEVEL_NOTICE_TextBox.TabIndex = 165;
		this.BA_REQ_LEVEL_NOTICE_TextBox.Location = new System.Drawing.Point(288, 15);
		this.BA_REQ_LEVEL_NOTICE_TextBox.Name = "BA_REQ_LEVEL_NOTICE_TextBox";
		this.BA_REQ_LEVEL_NOTICE_TextBox.Size = new System.Drawing.Size(707, 20);
		this.BA_REQ_LEVEL_NOTICE_TextBox.TabIndex = 155;
		this.label126.AutoSize = true;
		this.label126.Location = new System.Drawing.Point(244, 44);
		this.label126.Name = "label126";
		this.label126.Size = new System.Drawing.Size(38, 13);
		this.label126.TabIndex = 164;
		this.label126.Text = "Notice";
		this.label131.AutoSize = true;
		this.label131.Location = new System.Drawing.Point(244, 18);
		this.label131.Name = "label131";
		this.label131.Size = new System.Drawing.Size(38, 13);
		this.label131.TabIndex = 154;
		this.label131.Text = "Notice";
		this.EXCHENGE_LEVEL_NOTICE_TextBox.Location = new System.Drawing.Point(288, 67);
		this.EXCHENGE_LEVEL_NOTICE_TextBox.Name = "EXCHENGE_LEVEL_NOTICE_TextBox";
		this.EXCHENGE_LEVEL_NOTICE_TextBox.Size = new System.Drawing.Size(707, 20);
		this.EXCHENGE_LEVEL_NOTICE_TextBox.TabIndex = 163;
		this.label130.AutoSize = true;
		this.label130.Location = new System.Drawing.Point(244, 148);
		this.label130.Name = "label130";
		this.label130.Size = new System.Drawing.Size(38, 13);
		this.label130.TabIndex = 156;
		this.label130.Text = "Notice";
		this.label127.AutoSize = true;
		this.label127.Location = new System.Drawing.Point(244, 70);
		this.label127.Name = "label127";
		this.label127.Size = new System.Drawing.Size(38, 13);
		this.label127.TabIndex = 162;
		this.label127.Text = "Notice";
		this.ZERK_LEVEL_NOTICE_TextBox.Location = new System.Drawing.Point(288, 145);
		this.ZERK_LEVEL_NOTICE_TextBox.Name = "ZERK_LEVEL_NOTICE_TextBox";
		this.ZERK_LEVEL_NOTICE_TextBox.Size = new System.Drawing.Size(707, 20);
		this.ZERK_LEVEL_NOTICE_TextBox.TabIndex = 157;
		this.GLOBAL_LEVEL_NOTICE_TextBox.Location = new System.Drawing.Point(288, 93);
		this.GLOBAL_LEVEL_NOTICE_TextBox.Name = "GLOBAL_LEVEL_NOTICE_TextBox";
		this.GLOBAL_LEVEL_NOTICE_TextBox.Size = new System.Drawing.Size(707, 20);
		this.GLOBAL_LEVEL_NOTICE_TextBox.TabIndex = 161;
		this.label129.AutoSize = true;
		this.label129.Location = new System.Drawing.Point(244, 122);
		this.label129.Name = "label129";
		this.label129.Size = new System.Drawing.Size(38, 13);
		this.label129.TabIndex = 158;
		this.label129.Text = "Notice";
		this.label128.AutoSize = true;
		this.label128.Location = new System.Drawing.Point(244, 96);
		this.label128.Name = "label128";
		this.label128.Size = new System.Drawing.Size(38, 13);
		this.label128.TabIndex = 160;
		this.label128.Text = "Notice";
		this.STALL_LEVEL_NOTICE_TextBox.Location = new System.Drawing.Point(288, 119);
		this.STALL_LEVEL_NOTICE_TextBox.Name = "STALL_LEVEL_NOTICE_TextBox";
		this.STALL_LEVEL_NOTICE_TextBox.Size = new System.Drawing.Size(707, 20);
		this.STALL_LEVEL_NOTICE_TextBox.TabIndex = 159;
		this.groupBox2.Controls.Add(this.label72);
		this.groupBox2.Controls.Add(this.REVERSE_DELAY_NOTICE_TextBox);
		this.groupBox2.Controls.Add(this.label73);
		this.groupBox2.Controls.Add(this.label74);
		this.groupBox2.Controls.Add(this.REVERSE_DELAY_TextBox);
		this.groupBox2.Controls.Add(this.label94);
		this.groupBox2.Controls.Add(this.label95);
		this.groupBox2.Controls.Add(this.label96);
		this.groupBox2.Controls.Add(this.label97);
		this.groupBox2.Controls.Add(this.label98);
		this.groupBox2.Controls.Add(this.label99);
		this.groupBox2.Controls.Add(this.label100);
		this.groupBox2.Controls.Add(this.label101);
		this.groupBox2.Controls.Add(this.ZERK_DELAY_NOTICE_TextBox);
		this.groupBox2.Controls.Add(this.label102);
		this.groupBox2.Controls.Add(this.UNION_DELAY_NOTICE_TextBox);
		this.groupBox2.Controls.Add(this.label103);
		this.groupBox2.Controls.Add(this.EXIT_DELAY_NOTICE_TextBox);
		this.groupBox2.Controls.Add(this.label104);
		this.groupBox2.Controls.Add(this.GLOBAL_DELAY_NOTICE_TextBox);
		this.groupBox2.Controls.Add(this.label105);
		this.groupBox2.Controls.Add(this.GUILD_DELAY_NOTICE_TextBox);
		this.groupBox2.Controls.Add(this.label106);
		this.groupBox2.Controls.Add(this.RESTART_DELAY_NOTICE_TextBox);
		this.groupBox2.Controls.Add(this.label107);
		this.groupBox2.Controls.Add(this.STALL_DELAY_NOTICE_TextBox);
		this.groupBox2.Controls.Add(this.label108);
		this.groupBox2.Controls.Add(this.EXCHANGE_DELAY_NOTICE_TextBox);
		this.groupBox2.Controls.Add(this.label109);
		this.groupBox2.Controls.Add(this.label45);
		this.groupBox2.Controls.Add(this.label50);
		this.groupBox2.Controls.Add(this.label53);
		this.groupBox2.Controls.Add(this.label64);
		this.groupBox2.Controls.Add(this.label90);
		this.groupBox2.Controls.Add(this.label91);
		this.groupBox2.Controls.Add(this.label92);
		this.groupBox2.Controls.Add(this.ZERK_DELAY_TextBox);
		this.groupBox2.Controls.Add(this.UNION_DELAY_TextBox);
		this.groupBox2.Controls.Add(this.STALL_DELAY_TextBox);
		this.groupBox2.Controls.Add(this.RESTART_DELAY_TextBox);
		this.groupBox2.Controls.Add(this.GUILD_DELAY_TextBox);
		this.groupBox2.Controls.Add(this.GLOBAL_DELAY_TextBox);
		this.groupBox2.Controls.Add(this.EXIT_DELAY_TextBox);
		this.groupBox2.Controls.Add(this.label93);
		this.groupBox2.Controls.Add(this.EXCHANGE_DELAY_TextBox);
		this.groupBox2.Location = new System.Drawing.Point(8, 0);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1019, 252);
		this.groupBox2.TabIndex = 86;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Delay Settings";
		this.label72.AutoSize = true;
		this.label72.Location = new System.Drawing.Point(208, 226);
		this.label72.Name = "label72";
		this.label72.Size = new System.Drawing.Size(19, 13);
		this.label72.TabIndex = 158;
		this.label72.Text = ">>";
		this.REVERSE_DELAY_NOTICE_TextBox.Location = new System.Drawing.Point(286, 223);
		this.REVERSE_DELAY_NOTICE_TextBox.Name = "REVERSE_DELAY_NOTICE_TextBox";
		this.REVERSE_DELAY_NOTICE_TextBox.Size = new System.Drawing.Size(707, 20);
		this.REVERSE_DELAY_NOTICE_TextBox.TabIndex = 157;
		this.label73.AutoSize = true;
		this.label73.Location = new System.Drawing.Point(242, 226);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(38, 13);
		this.label73.TabIndex = 156;
		this.label73.Text = "Notice";
		this.label74.AutoSize = true;
		this.label74.Location = new System.Drawing.Point(12, 226);
		this.label74.Name = "label74";
		this.label74.Size = new System.Drawing.Size(77, 13);
		this.label74.TabIndex = 155;
		this.label74.Text = "Reverse Delay";
		this.REVERSE_DELAY_TextBox.Location = new System.Drawing.Point(97, 223);
		this.REVERSE_DELAY_TextBox.Name = "REVERSE_DELAY_TextBox";
		this.REVERSE_DELAY_TextBox.Size = new System.Drawing.Size(97, 20);
		this.REVERSE_DELAY_TextBox.TabIndex = 154;
		this.REVERSE_DELAY_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label94.AutoSize = true;
		this.label94.Location = new System.Drawing.Point(210, 44);
		this.label94.Name = "label94";
		this.label94.Size = new System.Drawing.Size(19, 13);
		this.label94.TabIndex = 153;
		this.label94.Text = ">>";
		this.label95.AutoSize = true;
		this.label95.Location = new System.Drawing.Point(210, 70);
		this.label95.Name = "label95";
		this.label95.Size = new System.Drawing.Size(19, 13);
		this.label95.TabIndex = 152;
		this.label95.Text = ">>";
		this.label96.AutoSize = true;
		this.label96.Location = new System.Drawing.Point(210, 96);
		this.label96.Name = "label96";
		this.label96.Size = new System.Drawing.Size(19, 13);
		this.label96.TabIndex = 151;
		this.label96.Text = ">>";
		this.label97.AutoSize = true;
		this.label97.Location = new System.Drawing.Point(210, 122);
		this.label97.Name = "label97";
		this.label97.Size = new System.Drawing.Size(19, 13);
		this.label97.TabIndex = 150;
		this.label97.Text = ">>";
		this.label98.AutoSize = true;
		this.label98.Location = new System.Drawing.Point(210, 148);
		this.label98.Name = "label98";
		this.label98.Size = new System.Drawing.Size(19, 13);
		this.label98.TabIndex = 149;
		this.label98.Text = ">>";
		this.label99.AutoSize = true;
		this.label99.Location = new System.Drawing.Point(210, 174);
		this.label99.Name = "label99";
		this.label99.Size = new System.Drawing.Size(19, 13);
		this.label99.TabIndex = 148;
		this.label99.Text = ">>";
		this.label100.AutoSize = true;
		this.label100.Location = new System.Drawing.Point(210, 200);
		this.label100.Name = "label100";
		this.label100.Size = new System.Drawing.Size(19, 13);
		this.label100.TabIndex = 147;
		this.label100.Text = ">>";
		this.label101.AutoSize = true;
		this.label101.Location = new System.Drawing.Point(210, 18);
		this.label101.Name = "label101";
		this.label101.Size = new System.Drawing.Size(19, 13);
		this.label101.TabIndex = 146;
		this.label101.Text = ">>";
		this.ZERK_DELAY_NOTICE_TextBox.Location = new System.Drawing.Point(288, 197);
		this.ZERK_DELAY_NOTICE_TextBox.Name = "ZERK_DELAY_NOTICE_TextBox";
		this.ZERK_DELAY_NOTICE_TextBox.Size = new System.Drawing.Size(707, 20);
		this.ZERK_DELAY_NOTICE_TextBox.TabIndex = 145;
		this.label102.AutoSize = true;
		this.label102.Location = new System.Drawing.Point(244, 200);
		this.label102.Name = "label102";
		this.label102.Size = new System.Drawing.Size(38, 13);
		this.label102.TabIndex = 144;
		this.label102.Text = "Notice";
		this.UNION_DELAY_NOTICE_TextBox.Location = new System.Drawing.Point(288, 171);
		this.UNION_DELAY_NOTICE_TextBox.Name = "UNION_DELAY_NOTICE_TextBox";
		this.UNION_DELAY_NOTICE_TextBox.Size = new System.Drawing.Size(707, 20);
		this.UNION_DELAY_NOTICE_TextBox.TabIndex = 143;
		this.label103.AutoSize = true;
		this.label103.Location = new System.Drawing.Point(244, 174);
		this.label103.Name = "label103";
		this.label103.Size = new System.Drawing.Size(38, 13);
		this.label103.TabIndex = 142;
		this.label103.Text = "Notice";
		this.EXIT_DELAY_NOTICE_TextBox.Location = new System.Drawing.Point(288, 41);
		this.EXIT_DELAY_NOTICE_TextBox.Name = "EXIT_DELAY_NOTICE_TextBox";
		this.EXIT_DELAY_NOTICE_TextBox.Size = new System.Drawing.Size(707, 20);
		this.EXIT_DELAY_NOTICE_TextBox.TabIndex = 141;
		this.label104.AutoSize = true;
		this.label104.Location = new System.Drawing.Point(244, 44);
		this.label104.Name = "label104";
		this.label104.Size = new System.Drawing.Size(38, 13);
		this.label104.TabIndex = 140;
		this.label104.Text = "Notice";
		this.GLOBAL_DELAY_NOTICE_TextBox.Location = new System.Drawing.Point(288, 67);
		this.GLOBAL_DELAY_NOTICE_TextBox.Name = "GLOBAL_DELAY_NOTICE_TextBox";
		this.GLOBAL_DELAY_NOTICE_TextBox.Size = new System.Drawing.Size(707, 20);
		this.GLOBAL_DELAY_NOTICE_TextBox.TabIndex = 139;
		this.label105.AutoSize = true;
		this.label105.Location = new System.Drawing.Point(244, 70);
		this.label105.Name = "label105";
		this.label105.Size = new System.Drawing.Size(38, 13);
		this.label105.TabIndex = 138;
		this.label105.Text = "Notice";
		this.GUILD_DELAY_NOTICE_TextBox.Location = new System.Drawing.Point(288, 93);
		this.GUILD_DELAY_NOTICE_TextBox.Name = "GUILD_DELAY_NOTICE_TextBox";
		this.GUILD_DELAY_NOTICE_TextBox.Size = new System.Drawing.Size(707, 20);
		this.GUILD_DELAY_NOTICE_TextBox.TabIndex = 137;
		this.label106.AutoSize = true;
		this.label106.Location = new System.Drawing.Point(244, 96);
		this.label106.Name = "label106";
		this.label106.Size = new System.Drawing.Size(38, 13);
		this.label106.TabIndex = 136;
		this.label106.Text = "Notice";
		this.RESTART_DELAY_NOTICE_TextBox.Location = new System.Drawing.Point(288, 119);
		this.RESTART_DELAY_NOTICE_TextBox.Name = "RESTART_DELAY_NOTICE_TextBox";
		this.RESTART_DELAY_NOTICE_TextBox.Size = new System.Drawing.Size(707, 20);
		this.RESTART_DELAY_NOTICE_TextBox.TabIndex = 135;
		this.label107.AutoSize = true;
		this.label107.Location = new System.Drawing.Point(244, 122);
		this.label107.Name = "label107";
		this.label107.Size = new System.Drawing.Size(38, 13);
		this.label107.TabIndex = 134;
		this.label107.Text = "Notice";
		this.STALL_DELAY_NOTICE_TextBox.Location = new System.Drawing.Point(288, 145);
		this.STALL_DELAY_NOTICE_TextBox.Name = "STALL_DELAY_NOTICE_TextBox";
		this.STALL_DELAY_NOTICE_TextBox.Size = new System.Drawing.Size(707, 20);
		this.STALL_DELAY_NOTICE_TextBox.TabIndex = 133;
		this.label108.AutoSize = true;
		this.label108.Location = new System.Drawing.Point(244, 148);
		this.label108.Name = "label108";
		this.label108.Size = new System.Drawing.Size(38, 13);
		this.label108.TabIndex = 132;
		this.label108.Text = "Notice";
		this.EXCHANGE_DELAY_NOTICE_TextBox.Location = new System.Drawing.Point(288, 15);
		this.EXCHANGE_DELAY_NOTICE_TextBox.Name = "EXCHANGE_DELAY_NOTICE_TextBox";
		this.EXCHANGE_DELAY_NOTICE_TextBox.Size = new System.Drawing.Size(707, 20);
		this.EXCHANGE_DELAY_NOTICE_TextBox.TabIndex = 131;
		this.label109.AutoSize = true;
		this.label109.Location = new System.Drawing.Point(244, 18);
		this.label109.Name = "label109";
		this.label109.Size = new System.Drawing.Size(38, 13);
		this.label109.TabIndex = 130;
		this.label109.Text = "Notice";
		this.label45.AutoSize = true;
		this.label45.Location = new System.Drawing.Point(32, 200);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(59, 13);
		this.label45.TabIndex = 64;
		this.label45.Text = "Zerk Delay";
		this.label50.AutoSize = true;
		this.label50.Location = new System.Drawing.Point(26, 174);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(65, 13);
		this.label50.TabIndex = 65;
		this.label50.Text = "Union Delay";
		this.label53.AutoSize = true;
		this.label53.Location = new System.Drawing.Point(34, 148);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(57, 13);
		this.label53.TabIndex = 63;
		this.label53.Text = "Stall Delay";
		this.label64.AutoSize = true;
		this.label64.Location = new System.Drawing.Point(20, 122);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(71, 13);
		this.label64.TabIndex = 64;
		this.label64.Text = "Restart Delay";
		this.label90.AutoSize = true;
		this.label90.Location = new System.Drawing.Point(30, 96);
		this.label90.Name = "label90";
		this.label90.Size = new System.Drawing.Size(61, 13);
		this.label90.TabIndex = 65;
		this.label90.Text = "Guild Delay";
		this.label91.AutoSize = true;
		this.label91.Location = new System.Drawing.Point(24, 70);
		this.label91.Name = "label91";
		this.label91.Size = new System.Drawing.Size(67, 13);
		this.label91.TabIndex = 66;
		this.label91.Text = "Global Delay";
		this.label92.AutoSize = true;
		this.label92.Location = new System.Drawing.Point(37, 44);
		this.label92.Name = "label92";
		this.label92.Size = new System.Drawing.Size(54, 13);
		this.label92.TabIndex = 62;
		this.label92.Text = "Exit Delay";
		this.ZERK_DELAY_TextBox.Location = new System.Drawing.Point(97, 197);
		this.ZERK_DELAY_TextBox.Name = "ZERK_DELAY_TextBox";
		this.ZERK_DELAY_TextBox.Size = new System.Drawing.Size(97, 20);
		this.ZERK_DELAY_TextBox.TabIndex = 61;
		this.ZERK_DELAY_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.UNION_DELAY_TextBox.Location = new System.Drawing.Point(97, 171);
		this.UNION_DELAY_TextBox.Name = "UNION_DELAY_TextBox";
		this.UNION_DELAY_TextBox.Size = new System.Drawing.Size(97, 20);
		this.UNION_DELAY_TextBox.TabIndex = 60;
		this.UNION_DELAY_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.STALL_DELAY_TextBox.Location = new System.Drawing.Point(97, 145);
		this.STALL_DELAY_TextBox.Name = "STALL_DELAY_TextBox";
		this.STALL_DELAY_TextBox.Size = new System.Drawing.Size(97, 20);
		this.STALL_DELAY_TextBox.TabIndex = 59;
		this.STALL_DELAY_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.RESTART_DELAY_TextBox.Location = new System.Drawing.Point(97, 119);
		this.RESTART_DELAY_TextBox.Name = "RESTART_DELAY_TextBox";
		this.RESTART_DELAY_TextBox.Size = new System.Drawing.Size(97, 20);
		this.RESTART_DELAY_TextBox.TabIndex = 58;
		this.RESTART_DELAY_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.GUILD_DELAY_TextBox.Location = new System.Drawing.Point(97, 93);
		this.GUILD_DELAY_TextBox.Name = "GUILD_DELAY_TextBox";
		this.GUILD_DELAY_TextBox.Size = new System.Drawing.Size(97, 20);
		this.GUILD_DELAY_TextBox.TabIndex = 57;
		this.GUILD_DELAY_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.GLOBAL_DELAY_TextBox.Location = new System.Drawing.Point(97, 67);
		this.GLOBAL_DELAY_TextBox.Name = "GLOBAL_DELAY_TextBox";
		this.GLOBAL_DELAY_TextBox.Size = new System.Drawing.Size(97, 20);
		this.GLOBAL_DELAY_TextBox.TabIndex = 56;
		this.GLOBAL_DELAY_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.EXIT_DELAY_TextBox.Location = new System.Drawing.Point(97, 41);
		this.EXIT_DELAY_TextBox.Name = "EXIT_DELAY_TextBox";
		this.EXIT_DELAY_TextBox.Size = new System.Drawing.Size(97, 20);
		this.EXIT_DELAY_TextBox.TabIndex = 55;
		this.EXIT_DELAY_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label93.AutoSize = true;
		this.label93.Location = new System.Drawing.Point(6, 18);
		this.label93.Name = "label93";
		this.label93.Size = new System.Drawing.Size(85, 13);
		this.label93.TabIndex = 54;
		this.label93.Text = "Exchange Delay";
		this.EXCHANGE_DELAY_TextBox.Location = new System.Drawing.Point(97, 15);
		this.EXCHANGE_DELAY_TextBox.Name = "EXCHANGE_DELAY_TextBox";
		this.EXCHANGE_DELAY_TextBox.Size = new System.Drawing.Size(97, 20);
		this.EXCHANGE_DELAY_TextBox.TabIndex = 3;
		this.EXCHANGE_DELAY_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.tabPage4.BackColor = System.Drawing.Color.WhiteSmoke;
		this.tabPage4.Controls.Add(this.groupBox30);
		this.tabPage4.Controls.Add(this.groupBox5);
		this.tabPage4.Location = new System.Drawing.Point(4, 22);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(1033, 453);
		this.tabPage4.TabIndex = 3;
		this.tabPage4.Text = "Server Settings";
		this.groupBox30.Controls.Add(this.SOXDROPNOTICE_NOTICE_TextBox);
		this.groupBox30.Controls.Add(this.label270);
		this.groupBox30.Controls.Add(this.label267);
		this.groupBox30.Controls.Add(this.SOX_Drop2_TextBox);
		this.groupBox30.Controls.Add(this.label268);
		this.groupBox30.Controls.Add(this.SOX_Drop1_TextBox);
		this.groupBox30.Controls.Add(this.SOXDROPNOTICE_NOTICE_CHECKBOX);
		this.groupBox30.Controls.Add(this.label266);
		this.groupBox30.Controls.Add(this.SOX_PLUS2_TextBox);
		this.groupBox30.Controls.Add(this.label265);
		this.groupBox30.Controls.Add(this.SOX_PLUS1_TextBox);
		this.groupBox30.Controls.Add(this.BLOCKSKILL_NOTICE_CHECKBOX);
		this.groupBox30.Controls.Add(this.GM_NOTICE_CHECKBOX);
		this.groupBox30.Controls.Add(this.BAN_NOTICE_CHECKBOX);
		this.groupBox30.Controls.Add(this.PULSNOTICE_NOTICE_Start_TextBox);
		this.groupBox30.Controls.Add(this.DISCONNECT_NOTICE_CHECKBOX);
		this.groupBox30.Controls.Add(this.label137);
		this.groupBox30.Controls.Add(this.label136);
		this.groupBox30.Controls.Add(this.BLOCKZERKPVP_NOTICE_CHECKBOX);
		this.groupBox30.Controls.Add(this.PULSNOTICE_NOTICE_CHECKBOX);
		this.groupBox30.Controls.Add(this.label133);
		this.groupBox30.Controls.Add(this.label132);
		this.groupBox30.Controls.Add(this.label125);
		this.groupBox30.Controls.Add(this.DISABLE_TAX_RATE_CHANGE_CHECKBOX);
		this.groupBox30.Controls.Add(this.label124);
		this.groupBox30.Controls.Add(this.label122);
		this.groupBox30.Controls.Add(this.WELCOME_MSG_CHECKBOX);
		this.groupBox30.Controls.Add(this.label121);
		this.groupBox30.Controls.Add(this.TOWN_DROP_ITEM_CHECKBOX);
		this.groupBox30.Controls.Add(this.label86);
		this.groupBox30.Controls.Add(this.DISABLED_ACADEMY_CHECKBOX);
		this.groupBox30.Controls.Add(this.label85);
		this.groupBox30.Controls.Add(this.DISABLE_AVATAR_BLUES_CHECKBOX);
		this.groupBox30.Controls.Add(this.label84);
		this.groupBox30.Controls.Add(this.DISABLE_RESTART_BUTTON_CHECKBOX);
		this.groupBox30.Controls.Add(this.label83);
		this.groupBox30.Controls.Add(this.PULSNOTICE_NOTICE_TextBox);
		this.groupBox30.Controls.Add(this.BLOCKZERKPVP_NOTICE_TextBox);
		this.groupBox30.Controls.Add(this.BLOCKSKILL_NOTICE_TextBox);
		this.groupBox30.Controls.Add(this.GM_NOTICE_TextBox);
		this.groupBox30.Controls.Add(this.TOWN_DROP_ITEM_NOTICE_TextBox);
		this.groupBox30.Controls.Add(this.DISCONNECT_NOTICE_TextBox);
		this.groupBox30.Controls.Add(this.BAN_NOTICE_TextBox);
		this.groupBox30.Controls.Add(this.DISABLE_TAX_RATE_CHANGE_NOTICE_TextBox);
		this.groupBox30.Controls.Add(this.DISABLED_ACADEMY_NOTICE_TextBox);
		this.groupBox30.Controls.Add(this.DISABLE_AVATAR_BLUES_NOTICE_TextBox);
		this.groupBox30.Controls.Add(this.DISABLE_RESTART_NOTICE_TextBox);
		this.groupBox30.Controls.Add(this.WELCOME_TEXT_NOTICE_TextBox);
		this.groupBox30.Location = new System.Drawing.Point(331, 7);
		this.groupBox30.Name = "groupBox30";
		this.groupBox30.Size = new System.Drawing.Size(695, 411);
		this.groupBox30.TabIndex = 5;
		this.groupBox30.TabStop = false;
		this.groupBox30.Text = "Server Notice";
		this.SOXDROPNOTICE_NOTICE_TextBox.Location = new System.Drawing.Point(352, 329);
		this.SOXDROPNOTICE_NOTICE_TextBox.Name = "SOXDROPNOTICE_NOTICE_TextBox";
		this.SOXDROPNOTICE_NOTICE_TextBox.Size = new System.Drawing.Size(334, 20);
		this.SOXDROPNOTICE_NOTICE_TextBox.TabIndex = 179;
		this.label270.AutoSize = true;
		this.label270.Location = new System.Drawing.Point(184, 332);
		this.label270.Name = "label270";
		this.label270.Size = new System.Drawing.Size(19, 13);
		this.label270.TabIndex = 178;
		this.label270.Text = ">>";
		this.label267.AutoSize = true;
		this.label267.Location = new System.Drawing.Point(284, 332);
		this.label267.Name = "label267";
		this.label267.Size = new System.Drawing.Size(11, 13);
		this.label267.TabIndex = 177;
		this.label267.Text = "||";
		this.SOX_Drop2_TextBox.Location = new System.Drawing.Point(298, 329);
		this.SOX_Drop2_TextBox.MaxLength = 10;
		this.SOX_Drop2_TextBox.Name = "SOX_Drop2_TextBox";
		this.SOX_Drop2_TextBox.Size = new System.Drawing.Size(48, 20);
		this.SOX_Drop2_TextBox.TabIndex = 176;
		this.SOX_Drop2_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label268.AutoSize = true;
		this.label268.Location = new System.Drawing.Point(203, 332);
		this.label268.Name = "label268";
		this.label268.Size = new System.Drawing.Size(28, 13);
		this.label268.TabIndex = 175;
		this.label268.Text = "Sox:";
		this.SOX_Drop1_TextBox.Location = new System.Drawing.Point(232, 329);
		this.SOX_Drop1_TextBox.MaxLength = 10;
		this.SOX_Drop1_TextBox.Name = "SOX_Drop1_TextBox";
		this.SOX_Drop1_TextBox.Size = new System.Drawing.Size(48, 20);
		this.SOX_Drop1_TextBox.TabIndex = 174;
		this.SOX_Drop1_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.SOXDROPNOTICE_NOTICE_CHECKBOX.AutoSize = true;
		this.SOXDROPNOTICE_NOTICE_CHECKBOX.Location = new System.Drawing.Point(7, 328);
		this.SOXDROPNOTICE_NOTICE_CHECKBOX.Name = "SOXDROPNOTICE_NOTICE_CHECKBOX";
		this.SOXDROPNOTICE_NOTICE_CHECKBOX.Size = new System.Drawing.Size(70, 17);
		this.SOXDROPNOTICE_NOTICE_CHECKBOX.TabIndex = 173;
		this.SOXDROPNOTICE_NOTICE_CHECKBOX.Text = "Sox Drop";
		this.SOXDROPNOTICE_NOTICE_CHECKBOX.UseVisualStyleBackColor = true;
		this.label266.AutoSize = true;
		this.label266.Location = new System.Drawing.Point(284, 306);
		this.label266.Name = "label266";
		this.label266.Size = new System.Drawing.Size(11, 13);
		this.label266.TabIndex = 172;
		this.label266.Text = "||";
		this.SOX_PLUS2_TextBox.Location = new System.Drawing.Point(298, 303);
		this.SOX_PLUS2_TextBox.MaxLength = 10;
		this.SOX_PLUS2_TextBox.Name = "SOX_PLUS2_TextBox";
		this.SOX_PLUS2_TextBox.Size = new System.Drawing.Size(48, 20);
		this.SOX_PLUS2_TextBox.TabIndex = 171;
		this.SOX_PLUS2_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label265.AutoSize = true;
		this.label265.Location = new System.Drawing.Point(203, 306);
		this.label265.Name = "label265";
		this.label265.Size = new System.Drawing.Size(28, 13);
		this.label265.TabIndex = 170;
		this.label265.Text = "Sox:";
		this.SOX_PLUS1_TextBox.Location = new System.Drawing.Point(232, 303);
		this.SOX_PLUS1_TextBox.MaxLength = 10;
		this.SOX_PLUS1_TextBox.Name = "SOX_PLUS1_TextBox";
		this.SOX_PLUS1_TextBox.Size = new System.Drawing.Size(48, 20);
		this.SOX_PLUS1_TextBox.TabIndex = 169;
		this.SOX_PLUS1_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.BLOCKSKILL_NOTICE_CHECKBOX.AutoSize = true;
		this.BLOCKSKILL_NOTICE_CHECKBOX.Location = new System.Drawing.Point(7, 255);
		this.BLOCKSKILL_NOTICE_CHECKBOX.Name = "BLOCKSKILL_NOTICE_CHECKBOX";
		this.BLOCKSKILL_NOTICE_CHECKBOX.Size = new System.Drawing.Size(121, 17);
		this.BLOCKSKILL_NOTICE_CHECKBOX.TabIndex = 168;
		this.BLOCKSKILL_NOTICE_CHECKBOX.Text = "Blocked Skill Notice";
		this.BLOCKSKILL_NOTICE_CHECKBOX.UseVisualStyleBackColor = true;
		this.GM_NOTICE_CHECKBOX.AutoSize = true;
		this.GM_NOTICE_CHECKBOX.Location = new System.Drawing.Point(7, 230);
		this.GM_NOTICE_CHECKBOX.Name = "GM_NOTICE_CHECKBOX";
		this.GM_NOTICE_CHECKBOX.Size = new System.Drawing.Size(77, 17);
		this.GM_NOTICE_CHECKBOX.TabIndex = 167;
		this.GM_NOTICE_CHECKBOX.Text = "GM Notice";
		this.GM_NOTICE_CHECKBOX.UseVisualStyleBackColor = true;
		this.BAN_NOTICE_CHECKBOX.AutoSize = true;
		this.BAN_NOTICE_CHECKBOX.Location = new System.Drawing.Point(7, 204);
		this.BAN_NOTICE_CHECKBOX.Name = "BAN_NOTICE_CHECKBOX";
		this.BAN_NOTICE_CHECKBOX.Size = new System.Drawing.Size(79, 17);
		this.BAN_NOTICE_CHECKBOX.TabIndex = 166;
		this.BAN_NOTICE_CHECKBOX.Text = "Ban Notice";
		this.BAN_NOTICE_CHECKBOX.UseVisualStyleBackColor = true;
		this.PULSNOTICE_NOTICE_Start_TextBox.Location = new System.Drawing.Point(130, 303);
		this.PULSNOTICE_NOTICE_Start_TextBox.MaxLength = 3;
		this.PULSNOTICE_NOTICE_Start_TextBox.Name = "PULSNOTICE_NOTICE_Start_TextBox";
		this.PULSNOTICE_NOTICE_Start_TextBox.Size = new System.Drawing.Size(38, 20);
		this.PULSNOTICE_NOTICE_Start_TextBox.TabIndex = 70;
		this.PULSNOTICE_NOTICE_Start_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.DISCONNECT_NOTICE_CHECKBOX.AutoSize = true;
		this.DISCONNECT_NOTICE_CHECKBOX.Location = new System.Drawing.Point(7, 180);
		this.DISCONNECT_NOTICE_CHECKBOX.Name = "DISCONNECT_NOTICE_CHECKBOX";
		this.DISCONNECT_NOTICE_CHECKBOX.Size = new System.Drawing.Size(114, 17);
		this.DISCONNECT_NOTICE_CHECKBOX.TabIndex = 165;
		this.DISCONNECT_NOTICE_CHECKBOX.Text = "Disconnect Notice";
		this.DISCONNECT_NOTICE_CHECKBOX.UseVisualStyleBackColor = true;
		this.label137.AutoSize = true;
		this.label137.Location = new System.Drawing.Point(184, 306);
		this.label137.Name = "label137";
		this.label137.Size = new System.Drawing.Size(19, 13);
		this.label137.TabIndex = 164;
		this.label137.Text = ">>";
		this.label136.AutoSize = true;
		this.label136.Location = new System.Drawing.Point(184, 281);
		this.label136.Name = "label136";
		this.label136.Size = new System.Drawing.Size(19, 13);
		this.label136.TabIndex = 163;
		this.label136.Text = ">>";
		this.BLOCKZERKPVP_NOTICE_CHECKBOX.AutoSize = true;
		this.BLOCKZERKPVP_NOTICE_CHECKBOX.Location = new System.Drawing.Point(7, 280);
		this.BLOCKZERKPVP_NOTICE_CHECKBOX.Name = "BLOCKZERKPVP_NOTICE_CHECKBOX";
		this.BLOCKZERKPVP_NOTICE_CHECKBOX.Size = new System.Drawing.Size(150, 17);
		this.BLOCKZERKPVP_NOTICE_CHECKBOX.TabIndex = 72;
		this.BLOCKZERKPVP_NOTICE_CHECKBOX.Text = "Disable Zerk in PVP mode";
		this.BLOCKZERKPVP_NOTICE_CHECKBOX.UseVisualStyleBackColor = true;
		this.PULSNOTICE_NOTICE_CHECKBOX.AutoSize = true;
		this.PULSNOTICE_NOTICE_CHECKBOX.Location = new System.Drawing.Point(8, 305);
		this.PULSNOTICE_NOTICE_CHECKBOX.Name = "PULSNOTICE_NOTICE_CHECKBOX";
		this.PULSNOTICE_NOTICE_CHECKBOX.Size = new System.Drawing.Size(116, 17);
		this.PULSNOTICE_NOTICE_CHECKBOX.TabIndex = 63;
		this.PULSNOTICE_NOTICE_CHECKBOX.Text = "Enable Plus Notice";
		this.PULSNOTICE_NOTICE_CHECKBOX.UseVisualStyleBackColor = true;
		this.label133.AutoSize = true;
		this.label133.Location = new System.Drawing.Point(184, 255);
		this.label133.Name = "label133";
		this.label133.Size = new System.Drawing.Size(19, 13);
		this.label133.TabIndex = 160;
		this.label133.Text = ">>";
		this.label132.AutoSize = true;
		this.label132.Location = new System.Drawing.Point(184, 230);
		this.label132.Name = "label132";
		this.label132.Size = new System.Drawing.Size(19, 13);
		this.label132.TabIndex = 159;
		this.label132.Text = ">>";
		this.label125.AutoSize = true;
		this.label125.Location = new System.Drawing.Point(184, 205);
		this.label125.Name = "label125";
		this.label125.Size = new System.Drawing.Size(19, 13);
		this.label125.TabIndex = 158;
		this.label125.Text = ">>";
		this.DISABLE_TAX_RATE_CHANGE_CHECKBOX.AutoSize = true;
		this.DISABLE_TAX_RATE_CHANGE_CHECKBOX.Location = new System.Drawing.Point(7, 129);
		this.DISABLE_TAX_RATE_CHANGE_CHECKBOX.Name = "DISABLE_TAX_RATE_CHANGE_CHECKBOX";
		this.DISABLE_TAX_RATE_CHANGE_CHECKBOX.Size = new System.Drawing.Size(148, 17);
		this.DISABLE_TAX_RATE_CHANGE_CHECKBOX.TabIndex = 71;
		this.DISABLE_TAX_RATE_CHANGE_CHECKBOX.Text = "Disable Tax Rate Change";
		this.DISABLE_TAX_RATE_CHANGE_CHECKBOX.UseVisualStyleBackColor = true;
		this.label124.AutoSize = true;
		this.label124.Location = new System.Drawing.Point(184, 180);
		this.label124.Name = "label124";
		this.label124.Size = new System.Drawing.Size(19, 13);
		this.label124.TabIndex = 157;
		this.label124.Text = ">>";
		this.label122.AutoSize = true;
		this.label122.Location = new System.Drawing.Point(184, 155);
		this.label122.Name = "label122";
		this.label122.Size = new System.Drawing.Size(19, 13);
		this.label122.TabIndex = 156;
		this.label122.Text = ">>";
		this.WELCOME_MSG_CHECKBOX.AutoSize = true;
		this.WELCOME_MSG_CHECKBOX.Location = new System.Drawing.Point(7, 29);
		this.WELCOME_MSG_CHECKBOX.Name = "WELCOME_MSG_CHECKBOX";
		this.WELCOME_MSG_CHECKBOX.Size = new System.Drawing.Size(155, 17);
		this.WELCOME_MSG_CHECKBOX.TabIndex = 66;
		this.WELCOME_MSG_CHECKBOX.Text = "Enable Wellcome Message";
		this.WELCOME_MSG_CHECKBOX.UseVisualStyleBackColor = true;
		this.label121.AutoSize = true;
		this.label121.Location = new System.Drawing.Point(184, 130);
		this.label121.Name = "label121";
		this.label121.Size = new System.Drawing.Size(19, 13);
		this.label121.TabIndex = 155;
		this.label121.Text = ">>";
		this.TOWN_DROP_ITEM_CHECKBOX.AutoSize = true;
		this.TOWN_DROP_ITEM_CHECKBOX.Location = new System.Drawing.Point(7, 154);
		this.TOWN_DROP_ITEM_CHECKBOX.Name = "TOWN_DROP_ITEM_CHECKBOX";
		this.TOWN_DROP_ITEM_CHECKBOX.Size = new System.Drawing.Size(183, 17);
		this.TOWN_DROP_ITEM_CHECKBOX.TabIndex = 62;
		this.TOWN_DROP_ITEM_CHECKBOX.Text = "Disable Player Drop Item in Town";
		this.TOWN_DROP_ITEM_CHECKBOX.UseVisualStyleBackColor = true;
		this.label86.AutoSize = true;
		this.label86.Location = new System.Drawing.Point(184, 105);
		this.label86.Name = "label86";
		this.label86.Size = new System.Drawing.Size(19, 13);
		this.label86.TabIndex = 154;
		this.label86.Text = ">>";
		this.DISABLED_ACADEMY_CHECKBOX.AutoSize = true;
		this.DISABLED_ACADEMY_CHECKBOX.Location = new System.Drawing.Point(7, 104);
		this.DISABLED_ACADEMY_CHECKBOX.Name = "DISABLED_ACADEMY_CHECKBOX";
		this.DISABLED_ACADEMY_CHECKBOX.Size = new System.Drawing.Size(108, 17);
		this.DISABLED_ACADEMY_CHECKBOX.TabIndex = 61;
		this.DISABLED_ACADEMY_CHECKBOX.Text = "Disable Academy";
		this.DISABLED_ACADEMY_CHECKBOX.UseVisualStyleBackColor = true;
		this.label85.AutoSize = true;
		this.label85.Location = new System.Drawing.Point(184, 81);
		this.label85.Name = "label85";
		this.label85.Size = new System.Drawing.Size(19, 13);
		this.label85.TabIndex = 153;
		this.label85.Text = ">>";
		this.DISABLE_AVATAR_BLUES_CHECKBOX.AutoSize = true;
		this.DISABLE_AVATAR_BLUES_CHECKBOX.Location = new System.Drawing.Point(7, 80);
		this.DISABLE_AVATAR_BLUES_CHECKBOX.Name = "DISABLE_AVATAR_BLUES_CHECKBOX";
		this.DISABLE_AVATAR_BLUES_CHECKBOX.Size = new System.Drawing.Size(119, 17);
		this.DISABLE_AVATAR_BLUES_CHECKBOX.TabIndex = 60;
		this.DISABLE_AVATAR_BLUES_CHECKBOX.Text = "Disable Avatar Blue";
		this.DISABLE_AVATAR_BLUES_CHECKBOX.UseVisualStyleBackColor = true;
		this.label84.AutoSize = true;
		this.label84.Location = new System.Drawing.Point(184, 56);
		this.label84.Name = "label84";
		this.label84.Size = new System.Drawing.Size(19, 13);
		this.label84.TabIndex = 152;
		this.label84.Text = ">>";
		this.DISABLE_RESTART_BUTTON_CHECKBOX.AutoSize = true;
		this.DISABLE_RESTART_BUTTON_CHECKBOX.Location = new System.Drawing.Point(7, 55);
		this.DISABLE_RESTART_BUTTON_CHECKBOX.Name = "DISABLE_RESTART_BUTTON_CHECKBOX";
		this.DISABLE_RESTART_BUTTON_CHECKBOX.Size = new System.Drawing.Size(132, 17);
		this.DISABLE_RESTART_BUTTON_CHECKBOX.TabIndex = 49;
		this.DISABLE_RESTART_BUTTON_CHECKBOX.Text = "Disable Restart Button";
		this.DISABLE_RESTART_BUTTON_CHECKBOX.UseVisualStyleBackColor = true;
		this.label83.AutoSize = true;
		this.label83.Location = new System.Drawing.Point(184, 30);
		this.label83.Name = "label83";
		this.label83.Size = new System.Drawing.Size(19, 13);
		this.label83.TabIndex = 151;
		this.label83.Text = ">>";
		this.PULSNOTICE_NOTICE_TextBox.Location = new System.Drawing.Point(352, 303);
		this.PULSNOTICE_NOTICE_TextBox.Name = "PULSNOTICE_NOTICE_TextBox";
		this.PULSNOTICE_NOTICE_TextBox.Size = new System.Drawing.Size(334, 20);
		this.PULSNOTICE_NOTICE_TextBox.TabIndex = 92;
		this.BLOCKZERKPVP_NOTICE_TextBox.Location = new System.Drawing.Point(204, 278);
		this.BLOCKZERKPVP_NOTICE_TextBox.Name = "BLOCKZERKPVP_NOTICE_TextBox";
		this.BLOCKZERKPVP_NOTICE_TextBox.Size = new System.Drawing.Size(482, 20);
		this.BLOCKZERKPVP_NOTICE_TextBox.TabIndex = 90;
		this.BLOCKSKILL_NOTICE_TextBox.Location = new System.Drawing.Point(204, 252);
		this.BLOCKSKILL_NOTICE_TextBox.Name = "BLOCKSKILL_NOTICE_TextBox";
		this.BLOCKSKILL_NOTICE_TextBox.Size = new System.Drawing.Size(482, 20);
		this.BLOCKSKILL_NOTICE_TextBox.TabIndex = 84;
		this.GM_NOTICE_TextBox.Location = new System.Drawing.Point(204, 227);
		this.GM_NOTICE_TextBox.Name = "GM_NOTICE_TextBox";
		this.GM_NOTICE_TextBox.Size = new System.Drawing.Size(482, 20);
		this.GM_NOTICE_TextBox.TabIndex = 82;
		this.TOWN_DROP_ITEM_NOTICE_TextBox.Location = new System.Drawing.Point(204, 152);
		this.TOWN_DROP_ITEM_NOTICE_TextBox.Name = "TOWN_DROP_ITEM_NOTICE_TextBox";
		this.TOWN_DROP_ITEM_NOTICE_TextBox.Size = new System.Drawing.Size(482, 20);
		this.TOWN_DROP_ITEM_NOTICE_TextBox.TabIndex = 80;
		this.DISCONNECT_NOTICE_TextBox.Location = new System.Drawing.Point(204, 177);
		this.DISCONNECT_NOTICE_TextBox.Name = "DISCONNECT_NOTICE_TextBox";
		this.DISCONNECT_NOTICE_TextBox.Size = new System.Drawing.Size(482, 20);
		this.DISCONNECT_NOTICE_TextBox.TabIndex = 78;
		this.BAN_NOTICE_TextBox.Location = new System.Drawing.Point(204, 202);
		this.BAN_NOTICE_TextBox.Name = "BAN_NOTICE_TextBox";
		this.BAN_NOTICE_TextBox.Size = new System.Drawing.Size(482, 20);
		this.BAN_NOTICE_TextBox.TabIndex = 76;
		this.DISABLE_TAX_RATE_CHANGE_NOTICE_TextBox.Location = new System.Drawing.Point(204, 127);
		this.DISABLE_TAX_RATE_CHANGE_NOTICE_TextBox.Name = "DISABLE_TAX_RATE_CHANGE_NOTICE_TextBox";
		this.DISABLE_TAX_RATE_CHANGE_NOTICE_TextBox.Size = new System.Drawing.Size(482, 20);
		this.DISABLE_TAX_RATE_CHANGE_NOTICE_TextBox.TabIndex = 72;
		this.DISABLED_ACADEMY_NOTICE_TextBox.Location = new System.Drawing.Point(204, 102);
		this.DISABLED_ACADEMY_NOTICE_TextBox.Name = "DISABLED_ACADEMY_NOTICE_TextBox";
		this.DISABLED_ACADEMY_NOTICE_TextBox.Size = new System.Drawing.Size(482, 20);
		this.DISABLED_ACADEMY_NOTICE_TextBox.TabIndex = 74;
		this.DISABLE_AVATAR_BLUES_NOTICE_TextBox.Location = new System.Drawing.Point(204, 77);
		this.DISABLE_AVATAR_BLUES_NOTICE_TextBox.Name = "DISABLE_AVATAR_BLUES_NOTICE_TextBox";
		this.DISABLE_AVATAR_BLUES_NOTICE_TextBox.Size = new System.Drawing.Size(482, 20);
		this.DISABLE_AVATAR_BLUES_NOTICE_TextBox.TabIndex = 70;
		this.DISABLE_RESTART_NOTICE_TextBox.Location = new System.Drawing.Point(204, 52);
		this.DISABLE_RESTART_NOTICE_TextBox.Name = "DISABLE_RESTART_NOTICE_TextBox";
		this.DISABLE_RESTART_NOTICE_TextBox.Size = new System.Drawing.Size(482, 20);
		this.DISABLE_RESTART_NOTICE_TextBox.TabIndex = 1;
		this.WELCOME_TEXT_NOTICE_TextBox.Location = new System.Drawing.Point(204, 27);
		this.WELCOME_TEXT_NOTICE_TextBox.Name = "WELCOME_TEXT_NOTICE_TextBox";
		this.WELCOME_TEXT_NOTICE_TextBox.Size = new System.Drawing.Size(482, 20);
		this.WELCOME_TEXT_NOTICE_TextBox.TabIndex = 68;
		this.groupBox5.Controls.Add(this.JobReverseBlock);
		this.groupBox5.Controls.Add(this.groupBox16);
		this.groupBox5.Controls.Add(this.label80);
		this.groupBox5.Controls.Add(this.label79);
		this.groupBox5.Controls.Add(this.label77);
		this.groupBox5.Controls.Add(this.DISABLECAPCHA_CHECKBOX);
		this.groupBox5.Controls.Add(this.CAPCHA_TEXTBOX);
		this.groupBox5.Controls.Add(this.SHARD_MAX_PLAYER_TEXTBOX);
		this.groupBox5.Controls.Add(this.label76);
		this.groupBox5.Controls.Add(this.SERVER_NAME_TEXTBOX);
		this.groupBox5.Controls.Add(this.label78);
		this.groupBox5.Location = new System.Drawing.Point(7, 7);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(318, 440);
		this.groupBox5.TabIndex = 4;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Server Info";
		this.groupBox16.Controls.Add(this.label87);
		this.groupBox16.Controls.Add(this.afksystem);
		this.groupBox16.Controls.Add(this.label89);
		this.groupBox16.Controls.Add(this.AFKMS_TEXTBOX);
		this.groupBox16.Location = new System.Drawing.Point(10, 106);
		this.groupBox16.Name = "groupBox16";
		this.groupBox16.Size = new System.Drawing.Size(264, 112);
		this.groupBox16.TabIndex = 150;
		this.groupBox16.TabStop = false;
		this.groupBox16.Text = "AFK System Settings";
		this.label87.AutoSize = true;
		this.label87.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 162);
		this.label87.Location = new System.Drawing.Point(7, 71);
		this.label87.Name = "label87";
		this.label87.Size = new System.Drawing.Size(194, 13);
		this.label87.TabIndex = 71;
		this.label87.Text = "AFK Süresini milisaniye cinsinden giriniz.";
		this.label87.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.afksystem.AutoSize = true;
		this.afksystem.Location = new System.Drawing.Point(6, 19);
		this.afksystem.Name = "afksystem";
		this.afksystem.Size = new System.Drawing.Size(83, 17);
		this.afksystem.TabIndex = 67;
		this.afksystem.Text = "AFK System";
		this.afksystem.UseVisualStyleBackColor = true;
		this.label89.AutoSize = true;
		this.label89.Location = new System.Drawing.Point(7, 45);
		this.label89.Name = "label89";
		this.label89.Size = new System.Drawing.Size(53, 13);
		this.label89.TabIndex = 64;
		this.label89.Text = "AFK Time";
		this.AFKMS_TEXTBOX.Location = new System.Drawing.Point(66, 42);
		this.AFKMS_TEXTBOX.Name = "AFKMS_TEXTBOX";
		this.AFKMS_TEXTBOX.Size = new System.Drawing.Size(129, 20);
		this.AFKMS_TEXTBOX.TabIndex = 58;
		this.label80.AutoSize = true;
		this.label80.Location = new System.Drawing.Point(135, 81);
		this.label80.Name = "label80";
		this.label80.Size = new System.Drawing.Size(19, 13);
		this.label80.TabIndex = 149;
		this.label80.Text = ">>";
		this.label79.AutoSize = true;
		this.label79.Location = new System.Drawing.Point(135, 56);
		this.label79.Name = "label79";
		this.label79.Size = new System.Drawing.Size(19, 13);
		this.label79.TabIndex = 148;
		this.label79.Text = ">>";
		this.label77.AutoSize = true;
		this.label77.Location = new System.Drawing.Point(135, 30);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(19, 13);
		this.label77.TabIndex = 147;
		this.label77.Text = ">>";
		this.DISABLECAPCHA_CHECKBOX.AutoSize = true;
		this.DISABLECAPCHA_CHECKBOX.Location = new System.Drawing.Point(10, 80);
		this.DISABLECAPCHA_CHECKBOX.Name = "DISABLECAPCHA_CHECKBOX";
		this.DISABLECAPCHA_CHECKBOX.Size = new System.Drawing.Size(132, 17);
		this.DISABLECAPCHA_CHECKBOX.TabIndex = 1;
		this.DISABLECAPCHA_CHECKBOX.Text = "Disable Captcha Code";
		this.DISABLECAPCHA_CHECKBOX.UseVisualStyleBackColor = true;
		this.CAPCHA_TEXTBOX.Location = new System.Drawing.Point(174, 78);
		this.CAPCHA_TEXTBOX.MaxLength = 3;
		this.CAPCHA_TEXTBOX.Name = "CAPCHA_TEXTBOX";
		this.CAPCHA_TEXTBOX.Size = new System.Drawing.Size(100, 20);
		this.CAPCHA_TEXTBOX.TabIndex = 47;
		this.CAPCHA_TEXTBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.SHARD_MAX_PLAYER_TEXTBOX.Location = new System.Drawing.Point(174, 53);
		this.SHARD_MAX_PLAYER_TEXTBOX.MaxLength = 4;
		this.SHARD_MAX_PLAYER_TEXTBOX.Name = "SHARD_MAX_PLAYER_TEXTBOX";
		this.SHARD_MAX_PLAYER_TEXTBOX.Size = new System.Drawing.Size(100, 20);
		this.SHARD_MAX_PLAYER_TEXTBOX.TabIndex = 46;
		this.SHARD_MAX_PLAYER_TEXTBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label76.AutoSize = true;
		this.label76.Location = new System.Drawing.Point(27, 56);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(90, 13);
		this.label76.TabIndex = 4;
		this.label76.Text = "Shard Max Player";
		this.SERVER_NAME_TEXTBOX.Location = new System.Drawing.Point(174, 27);
		this.SERVER_NAME_TEXTBOX.Name = "SERVER_NAME_TEXTBOX";
		this.SERVER_NAME_TEXTBOX.Size = new System.Drawing.Size(100, 20);
		this.SERVER_NAME_TEXTBOX.TabIndex = 2;
		this.SERVER_NAME_TEXTBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label78.AutoSize = true;
		this.label78.Location = new System.Drawing.Point(27, 30);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(102, 13);
		this.label78.TabIndex = 1;
		this.label78.Text = "Server Name/Shard";
		this.tabPage5.BackColor = System.Drawing.Color.WhiteSmoke;
		this.tabPage5.Controls.Add(this.groupBox37);
		this.tabPage5.Controls.Add(this.groupBox27);
		this.tabPage5.Location = new System.Drawing.Point(4, 22);
		this.tabPage5.Name = "tabPage5";
		this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage5.Size = new System.Drawing.Size(1033, 453);
		this.tabPage5.TabIndex = 4;
		this.tabPage5.Text = "Event Settings";
		this.groupBox37.Controls.Add(this.dataGridView1);
		this.groupBox37.Controls.Add(this.button_ShowEventTime);
		this.groupBox37.Controls.Add(this.button_RemoveEvent);
		this.groupBox37.Controls.Add(this.button_AddEvent);
		this.groupBox37.Controls.Add(this.comboBox_EventTime_EventName);
		this.groupBox37.Controls.Add(this.comboBox_EventTime_Day);
		this.groupBox37.Controls.Add(this.dateTimePicker_EventTime_Hour);
		this.groupBox37.Location = new System.Drawing.Point(259, 6);
		this.groupBox37.Name = "groupBox37";
		this.groupBox37.Size = new System.Drawing.Size(582, 417);
		this.groupBox37.TabIndex = 5;
		this.groupBox37.TabStop = false;
		this.groupBox37.Text = "All Event Time Setting";
		this.dataGridView1.AllowUserToAddRows = false;
		this.dataGridView1.AllowUserToResizeColumns = false;
		this.dataGridView1.AllowUserToResizeRows = false;
		this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		this.dataGridView1.Location = new System.Drawing.Point(23, 20);
		this.dataGridView1.Name = "dataGridView1";
		this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
		this.dataGridView1.Size = new System.Drawing.Size(542, 296);
		this.dataGridView1.TabIndex = 7;
		this.button_ShowEventTime.Location = new System.Drawing.Point(23, 322);
		this.button_ShowEventTime.Name = "button_ShowEventTime";
		this.button_ShowEventTime.Size = new System.Drawing.Size(542, 23);
		this.button_ShowEventTime.TabIndex = 6;
		this.button_ShowEventTime.Text = "Show Event Time Setting";
		this.button_ShowEventTime.UseVisualStyleBackColor = true;
		this.button_ShowEventTime.Click += new System.EventHandler(button_ShowEventTime_Click);
		this.button_RemoveEvent.Location = new System.Drawing.Point(490, 360);
		this.button_RemoveEvent.Name = "button_RemoveEvent";
		this.button_RemoveEvent.Size = new System.Drawing.Size(75, 21);
		this.button_RemoveEvent.TabIndex = 5;
		this.button_RemoveEvent.Text = "Remove";
		this.button_RemoveEvent.UseVisualStyleBackColor = true;
		this.button_RemoveEvent.Click += new System.EventHandler(button_RemoveEvent_Click);
		this.button_AddEvent.Location = new System.Drawing.Point(396, 360);
		this.button_AddEvent.Name = "button_AddEvent";
		this.button_AddEvent.Size = new System.Drawing.Size(75, 21);
		this.button_AddEvent.TabIndex = 4;
		this.button_AddEvent.Text = "Add";
		this.button_AddEvent.UseVisualStyleBackColor = true;
		this.button_AddEvent.Click += new System.EventHandler(button_AddEvent_Click);
		this.comboBox_EventTime_EventName.FormattingEnabled = true;
		this.comboBox_EventTime_EventName.Items.AddRange(new object[8] { "Question And Answer", "Hide And Seek", "Search And Destroy", "Lucky Party Number", "Lottery Gold", "GM Killer", "Last Man Standing", "Survival Event" });
		this.comboBox_EventTime_EventName.Location = new System.Drawing.Point(240, 360);
		this.comboBox_EventTime_EventName.Name = "comboBox_EventTime_EventName";
		this.comboBox_EventTime_EventName.Size = new System.Drawing.Size(134, 21);
		this.comboBox_EventTime_EventName.TabIndex = 3;
		this.comboBox_EventTime_Day.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBox_EventTime_Day.FormattingEnabled = true;
		this.comboBox_EventTime_Day.Items.AddRange(new object[8] { "Allday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" });
		this.comboBox_EventTime_Day.Location = new System.Drawing.Point(23, 360);
		this.comboBox_EventTime_Day.Name = "comboBox_EventTime_Day";
		this.comboBox_EventTime_Day.Size = new System.Drawing.Size(85, 21);
		this.comboBox_EventTime_Day.TabIndex = 2;
		this.dateTimePicker_EventTime_Hour.Format = System.Windows.Forms.DateTimePickerFormat.Time;
		this.dateTimePicker_EventTime_Hour.Location = new System.Drawing.Point(131, 360);
		this.dateTimePicker_EventTime_Hour.Name = "dateTimePicker_EventTime_Hour";
		this.dateTimePicker_EventTime_Hour.ShowUpDown = true;
		this.dateTimePicker_EventTime_Hour.Size = new System.Drawing.Size(90, 20);
		this.dateTimePicker_EventTime_Hour.TabIndex = 0;
		this.groupBox27.Controls.Add(this.Start_Clientless_Button);
		this.groupBox27.Controls.Add(this.DC_Clientless_Button);
		this.groupBox27.Controls.Add(this.CL_INV_STATE);
		this.groupBox27.Controls.Add(this.label81);
		this.groupBox27.Controls.Add(this.label82);
		this.groupBox27.Controls.Add(this.label88);
		this.groupBox27.Controls.Add(this.CL_CHARNAME);
		this.groupBox27.Controls.Add(this.label134);
		this.groupBox27.Controls.Add(this.CL_CAPTCHA);
		this.groupBox27.Controls.Add(this.label135);
		this.groupBox27.Controls.Add(this.CL_PASSWORD);
		this.groupBox27.Controls.Add(this.label138);
		this.groupBox27.Controls.Add(this.CL_ID);
		this.groupBox27.Controls.Add(this.label139);
		this.groupBox27.Controls.Add(this.CL_LOCALE);
		this.groupBox27.Controls.Add(this.label140);
		this.groupBox27.Controls.Add(this.CL_VER);
		this.groupBox27.Controls.Add(this.label141);
		this.groupBox27.Controls.Add(this.CL_GT_PORT);
		this.groupBox27.Controls.Add(this.label142);
		this.groupBox27.Controls.Add(this.CL_GT_IP);
		this.groupBox27.Location = new System.Drawing.Point(8, 6);
		this.groupBox27.Name = "groupBox27";
		this.groupBox27.Size = new System.Drawing.Size(245, 417);
		this.groupBox27.TabIndex = 4;
		this.groupBox27.TabStop = false;
		this.groupBox27.Text = "Clientless";
		this.Start_Clientless_Button.Location = new System.Drawing.Point(51, 269);
		this.Start_Clientless_Button.Name = "Start_Clientless_Button";
		this.Start_Clientless_Button.Size = new System.Drawing.Size(125, 25);
		this.Start_Clientless_Button.TabIndex = 182;
		this.Start_Clientless_Button.Text = "Connect to clientless";
		this.Start_Clientless_Button.UseVisualStyleBackColor = true;
		this.Start_Clientless_Button.Click += new System.EventHandler(Start_Clientless_Button_Click);
		this.DC_Clientless_Button.Enabled = false;
		this.DC_Clientless_Button.Location = new System.Drawing.Point(51, 309);
		this.DC_Clientless_Button.Name = "DC_Clientless_Button";
		this.DC_Clientless_Button.Size = new System.Drawing.Size(125, 25);
		this.DC_Clientless_Button.TabIndex = 181;
		this.DC_Clientless_Button.Text = "Disconnect";
		this.DC_Clientless_Button.UseVisualStyleBackColor = true;
		this.DC_Clientless_Button.Click += new System.EventHandler(DC_Clientless_Button_Click);
		this.CL_INV_STATE.AutoSize = true;
		this.CL_INV_STATE.Location = new System.Drawing.Point(6, 393);
		this.CL_INV_STATE.Name = "CL_INV_STATE";
		this.CL_INV_STATE.Size = new System.Drawing.Size(139, 17);
		this.CL_INV_STATE.TabIndex = 180;
		this.CL_INV_STATE.Text = "Change in/visible status";
		this.CL_INV_STATE.UseVisualStyleBackColor = true;
		this.label81.AutoSize = true;
		this.label81.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label81.ForeColor = System.Drawing.Color.Black;
		this.label81.Location = new System.Drawing.Point(84, 235);
		this.label81.Name = "label81";
		this.label81.Size = new System.Drawing.Size(59, 13);
		this.label81.TabIndex = 177;
		this.label81.Text = "Unknown";
		this.label82.AutoSize = true;
		this.label82.ForeColor = System.Drawing.Color.Black;
		this.label82.Location = new System.Drawing.Point(6, 235);
		this.label82.Name = "label82";
		this.label82.Size = new System.Drawing.Size(69, 13);
		this.label82.TabIndex = 176;
		this.label82.Text = "Client Status:";
		this.label88.AutoSize = true;
		this.label88.ForeColor = System.Drawing.Color.Black;
		this.label88.Location = new System.Drawing.Point(6, 210);
		this.label88.Name = "label88";
		this.label88.Size = new System.Drawing.Size(58, 13);
		this.label88.TabIndex = 175;
		this.label88.Text = "Charname:";
		this.CL_CHARNAME.BackColor = System.Drawing.SystemColors.Window;
		this.CL_CHARNAME.Location = new System.Drawing.Point(124, 207);
		this.CL_CHARNAME.Name = "CL_CHARNAME";
		this.CL_CHARNAME.Size = new System.Drawing.Size(100, 20);
		this.CL_CHARNAME.TabIndex = 174;
		this.label134.AutoSize = true;
		this.label134.ForeColor = System.Drawing.Color.Black;
		this.label134.Location = new System.Drawing.Point(6, 184);
		this.label134.Name = "label134";
		this.label134.Size = new System.Drawing.Size(107, 13);
		this.label134.TabIndex = 173;
		this.label134.Text = "Captcha image input:";
		this.CL_CAPTCHA.BackColor = System.Drawing.SystemColors.Window;
		this.CL_CAPTCHA.Location = new System.Drawing.Point(124, 181);
		this.CL_CAPTCHA.Name = "CL_CAPTCHA";
		this.CL_CAPTCHA.Size = new System.Drawing.Size(100, 20);
		this.CL_CAPTCHA.TabIndex = 172;
		this.label135.AutoSize = true;
		this.label135.ForeColor = System.Drawing.Color.Black;
		this.label135.Location = new System.Drawing.Point(6, 158);
		this.label135.Name = "label135";
		this.label135.Size = new System.Drawing.Size(77, 13);
		this.label135.TabIndex = 171;
		this.label135.Text = "Character PW:";
		this.CL_PASSWORD.BackColor = System.Drawing.SystemColors.Window;
		this.CL_PASSWORD.Location = new System.Drawing.Point(124, 155);
		this.CL_PASSWORD.Name = "CL_PASSWORD";
		this.CL_PASSWORD.Size = new System.Drawing.Size(100, 20);
		this.CL_PASSWORD.TabIndex = 170;
		this.CL_PASSWORD.UseSystemPasswordChar = true;
		this.label138.AutoSize = true;
		this.label138.ForeColor = System.Drawing.Color.Black;
		this.label138.Location = new System.Drawing.Point(6, 132);
		this.label138.Name = "label138";
		this.label138.Size = new System.Drawing.Size(70, 13);
		this.label138.TabIndex = 169;
		this.label138.Text = "Character ID:";
		this.CL_ID.BackColor = System.Drawing.SystemColors.Window;
		this.CL_ID.Location = new System.Drawing.Point(124, 129);
		this.CL_ID.Name = "CL_ID";
		this.CL_ID.Size = new System.Drawing.Size(100, 20);
		this.CL_ID.TabIndex = 168;
		this.label139.AutoSize = true;
		this.label139.ForeColor = System.Drawing.Color.Black;
		this.label139.Location = new System.Drawing.Point(6, 106);
		this.label139.Name = "label139";
		this.label139.Size = new System.Drawing.Size(76, 13);
		this.label139.TabIndex = 167;
		this.label139.Text = "Server Locale:";
		this.CL_LOCALE.BackColor = System.Drawing.SystemColors.Window;
		this.CL_LOCALE.Location = new System.Drawing.Point(124, 103);
		this.CL_LOCALE.Name = "CL_LOCALE";
		this.CL_LOCALE.Size = new System.Drawing.Size(100, 20);
		this.CL_LOCALE.TabIndex = 166;
		this.label140.AutoSize = true;
		this.label140.ForeColor = System.Drawing.Color.Black;
		this.label140.Location = new System.Drawing.Point(6, 80);
		this.label140.Name = "label140";
		this.label140.Size = new System.Drawing.Size(79, 13);
		this.label140.TabIndex = 165;
		this.label140.Text = "Server Version:";
		this.CL_VER.BackColor = System.Drawing.SystemColors.Window;
		this.CL_VER.Location = new System.Drawing.Point(124, 77);
		this.CL_VER.Name = "CL_VER";
		this.CL_VER.Size = new System.Drawing.Size(100, 20);
		this.CL_VER.TabIndex = 164;
		this.label141.AutoSize = true;
		this.label141.ForeColor = System.Drawing.Color.Black;
		this.label141.Location = new System.Drawing.Point(8, 54);
		this.label141.Name = "label141";
		this.label141.Size = new System.Drawing.Size(105, 13);
		this.label141.TabIndex = 163;
		this.label141.Text = "GatewayServer Port:";
		this.CL_GT_PORT.BackColor = System.Drawing.SystemColors.Window;
		this.CL_GT_PORT.Location = new System.Drawing.Point(124, 51);
		this.CL_GT_PORT.Name = "CL_GT_PORT";
		this.CL_GT_PORT.Size = new System.Drawing.Size(100, 20);
		this.CL_GT_PORT.TabIndex = 162;
		this.label142.AutoSize = true;
		this.label142.ForeColor = System.Drawing.Color.Black;
		this.label142.Location = new System.Drawing.Point(6, 28);
		this.label142.Name = "label142";
		this.label142.Size = new System.Drawing.Size(96, 13);
		this.label142.TabIndex = 161;
		this.label142.Text = "GatewayServer IP:";
		this.CL_GT_IP.BackColor = System.Drawing.SystemColors.Window;
		this.CL_GT_IP.Location = new System.Drawing.Point(124, 25);
		this.CL_GT_IP.Name = "CL_GT_IP";
		this.CL_GT_IP.Size = new System.Drawing.Size(100, 20);
		this.CL_GT_IP.TabIndex = 160;
		this.tabPage6.BackColor = System.Drawing.Color.WhiteSmoke;
		this.tabPage6.Controls.Add(this.groupBox11);
		this.tabPage6.Controls.Add(this.groupBox10);
		this.tabPage6.Controls.Add(this.groupBox29);
		this.tabPage6.Controls.Add(this.groupBox38);
		this.tabPage6.Location = new System.Drawing.Point(4, 22);
		this.tabPage6.Name = "tabPage6";
		this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage6.Size = new System.Drawing.Size(1033, 453);
		this.tabPage6.TabIndex = 5;
		this.tabPage6.Text = "QNA & HNS";
		this.groupBox11.Controls.Add(this.label190);
		this.groupBox11.Controls.Add(this.HNS_WINBOX);
		this.groupBox11.Controls.Add(this.label192);
		this.groupBox11.Controls.Add(this.HNS_ENDBOX);
		this.groupBox11.Controls.Add(this.label193);
		this.groupBox11.Controls.Add(this.HNS_PLACEINFOBOX);
		this.groupBox11.Controls.Add(this.label194);
		this.groupBox11.Controls.Add(this.HNS_INFONOTICEBOX);
		this.groupBox11.Controls.Add(this.label195);
		this.groupBox11.Controls.Add(this.HNS_STARTNOTICEBOX);
		this.groupBox11.Location = new System.Drawing.Point(328, 229);
		this.groupBox11.Name = "groupBox11";
		this.groupBox11.Size = new System.Drawing.Size(699, 200);
		this.groupBox11.TabIndex = 19;
		this.groupBox11.TabStop = false;
		this.groupBox11.Text = "Notices";
		this.label190.AutoSize = true;
		this.label190.Location = new System.Drawing.Point(37, 127);
		this.label190.Name = "label190";
		this.label190.Size = new System.Drawing.Size(41, 13);
		this.label190.TabIndex = 165;
		this.label190.Text = "Winner";
		this.HNS_WINBOX.Location = new System.Drawing.Point(84, 124);
		this.HNS_WINBOX.Name = "HNS_WINBOX";
		this.HNS_WINBOX.Size = new System.Drawing.Size(609, 20);
		this.HNS_WINBOX.TabIndex = 164;
		this.label192.AutoSize = true;
		this.label192.Location = new System.Drawing.Point(52, 101);
		this.label192.Name = "label192";
		this.label192.Size = new System.Drawing.Size(26, 13);
		this.label192.TabIndex = 163;
		this.label192.Text = "End";
		this.HNS_ENDBOX.Location = new System.Drawing.Point(84, 98);
		this.HNS_ENDBOX.Name = "HNS_ENDBOX";
		this.HNS_ENDBOX.Size = new System.Drawing.Size(609, 20);
		this.HNS_ENDBOX.TabIndex = 162;
		this.label193.AutoSize = true;
		this.label193.Location = new System.Drawing.Point(23, 75);
		this.label193.Name = "label193";
		this.label193.Size = new System.Drawing.Size(55, 13);
		this.label193.TabIndex = 161;
		this.label193.Text = "Place Info";
		this.HNS_PLACEINFOBOX.Location = new System.Drawing.Point(84, 72);
		this.HNS_PLACEINFOBOX.Name = "HNS_PLACEINFOBOX";
		this.HNS_PLACEINFOBOX.Size = new System.Drawing.Size(609, 20);
		this.HNS_PLACEINFOBOX.TabIndex = 160;
		this.label194.AutoSize = true;
		this.label194.Location = new System.Drawing.Point(19, 48);
		this.label194.Name = "label194";
		this.label194.Size = new System.Drawing.Size(59, 13);
		this.label194.TabIndex = 159;
		this.label194.Text = "Info Notice";
		this.HNS_INFONOTICEBOX.Location = new System.Drawing.Point(84, 45);
		this.HNS_INFONOTICEBOX.Name = "HNS_INFONOTICEBOX";
		this.HNS_INFONOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.HNS_INFONOTICEBOX.TabIndex = 158;
		this.label195.AutoSize = true;
		this.label195.Location = new System.Drawing.Point(15, 22);
		this.label195.Name = "label195";
		this.label195.Size = new System.Drawing.Size(63, 13);
		this.label195.TabIndex = 149;
		this.label195.Text = "Start Notice";
		this.HNS_STARTNOTICEBOX.Location = new System.Drawing.Point(84, 19);
		this.HNS_STARTNOTICEBOX.Name = "HNS_STARTNOTICEBOX";
		this.HNS_STARTNOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.HNS_STARTNOTICEBOX.TabIndex = 148;
		this.groupBox10.Controls.Add(this.label186);
		this.groupBox10.Controls.Add(this.QNA_ROUNDINFOBOX);
		this.groupBox10.Controls.Add(this.label185);
		this.groupBox10.Controls.Add(this.QNA_WINBOX);
		this.groupBox10.Controls.Add(this.label184);
		this.groupBox10.Controls.Add(this.QNA_ENDBOX);
		this.groupBox10.Controls.Add(this.label183);
		this.groupBox10.Controls.Add(this.QNA_INFOCHARBOX);
		this.groupBox10.Controls.Add(this.label187);
		this.groupBox10.Controls.Add(this.QNA_INFONOTICEBOX);
		this.groupBox10.Controls.Add(this.label159);
		this.groupBox10.Controls.Add(this.QNA_STARTNOTICEBOX);
		this.groupBox10.Location = new System.Drawing.Point(328, 17);
		this.groupBox10.Name = "groupBox10";
		this.groupBox10.Size = new System.Drawing.Size(699, 200);
		this.groupBox10.TabIndex = 18;
		this.groupBox10.TabStop = false;
		this.groupBox10.Text = "Notices";
		this.label186.AutoSize = true;
		this.label186.Location = new System.Drawing.Point(18, 153);
		this.label186.Name = "label186";
		this.label186.Size = new System.Drawing.Size(60, 13);
		this.label186.TabIndex = 167;
		this.label186.Text = "Round Info";
		this.QNA_ROUNDINFOBOX.Location = new System.Drawing.Point(84, 150);
		this.QNA_ROUNDINFOBOX.Name = "QNA_ROUNDINFOBOX";
		this.QNA_ROUNDINFOBOX.Size = new System.Drawing.Size(609, 20);
		this.QNA_ROUNDINFOBOX.TabIndex = 166;
		this.label185.AutoSize = true;
		this.label185.Location = new System.Drawing.Point(37, 127);
		this.label185.Name = "label185";
		this.label185.Size = new System.Drawing.Size(41, 13);
		this.label185.TabIndex = 165;
		this.label185.Text = "Winner";
		this.QNA_WINBOX.Location = new System.Drawing.Point(84, 124);
		this.QNA_WINBOX.Name = "QNA_WINBOX";
		this.QNA_WINBOX.Size = new System.Drawing.Size(609, 20);
		this.QNA_WINBOX.TabIndex = 164;
		this.label184.AutoSize = true;
		this.label184.Location = new System.Drawing.Point(52, 101);
		this.label184.Name = "label184";
		this.label184.Size = new System.Drawing.Size(26, 13);
		this.label184.TabIndex = 163;
		this.label184.Text = "End";
		this.QNA_ENDBOX.Location = new System.Drawing.Point(84, 98);
		this.QNA_ENDBOX.Name = "QNA_ENDBOX";
		this.QNA_ENDBOX.Size = new System.Drawing.Size(609, 20);
		this.QNA_ENDBOX.TabIndex = 162;
		this.label183.AutoSize = true;
		this.label183.Location = new System.Drawing.Point(13, 75);
		this.label183.Name = "label183";
		this.label183.Size = new System.Drawing.Size(65, 13);
		this.label183.TabIndex = 161;
		this.label183.Text = "Info Notice2";
		this.QNA_INFOCHARBOX.Location = new System.Drawing.Point(84, 72);
		this.QNA_INFOCHARBOX.Name = "QNA_INFOCHARBOX";
		this.QNA_INFOCHARBOX.Size = new System.Drawing.Size(609, 20);
		this.QNA_INFOCHARBOX.TabIndex = 160;
		this.label187.AutoSize = true;
		this.label187.Location = new System.Drawing.Point(19, 48);
		this.label187.Name = "label187";
		this.label187.Size = new System.Drawing.Size(59, 13);
		this.label187.TabIndex = 159;
		this.label187.Text = "Info Notice";
		this.QNA_INFONOTICEBOX.Location = new System.Drawing.Point(84, 45);
		this.QNA_INFONOTICEBOX.Name = "QNA_INFONOTICEBOX";
		this.QNA_INFONOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.QNA_INFONOTICEBOX.TabIndex = 158;
		this.label159.AutoSize = true;
		this.label159.Location = new System.Drawing.Point(15, 22);
		this.label159.Name = "label159";
		this.label159.Size = new System.Drawing.Size(63, 13);
		this.label159.TabIndex = 149;
		this.label159.Text = "Start Notice";
		this.QNA_STARTNOTICEBOX.Location = new System.Drawing.Point(84, 19);
		this.QNA_STARTNOTICEBOX.Name = "QNA_STARTNOTICEBOX";
		this.QNA_STARTNOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.QNA_STARTNOTICEBOX.TabIndex = 148;
		this.groupBox29.Controls.Add(this.HNS_ITEMNAMEBOX);
		this.groupBox29.Controls.Add(this.label152);
		this.groupBox29.Controls.Add(this.HNS_ITEMCOUNTBOX);
		this.groupBox29.Controls.Add(this.label153);
		this.groupBox29.Controls.Add(this.HNS_ITEMREWARDBOX);
		this.groupBox29.Controls.Add(this.HNS_ROUNDSBOX);
		this.groupBox29.Controls.Add(this.HNS_TIMETOSEARCHBOX);
		this.groupBox29.Controls.Add(this.label154);
		this.groupBox29.Controls.Add(this.label155);
		this.groupBox29.Controls.Add(this.label156);
		this.groupBox29.Controls.Add(this.HNS_ENABLEBOX);
		this.groupBox29.Location = new System.Drawing.Point(8, 229);
		this.groupBox29.Name = "groupBox29";
		this.groupBox29.Size = new System.Drawing.Size(314, 200);
		this.groupBox29.TabIndex = 17;
		this.groupBox29.TabStop = false;
		this.groupBox29.Text = "Hide And Seek";
		this.HNS_ITEMNAMEBOX.Location = new System.Drawing.Point(173, 159);
		this.HNS_ITEMNAMEBOX.MaxLength = 5;
		this.HNS_ITEMNAMEBOX.Name = "HNS_ITEMNAMEBOX";
		this.HNS_ITEMNAMEBOX.Size = new System.Drawing.Size(122, 20);
		this.HNS_ITEMNAMEBOX.TabIndex = 43;
		this.HNS_ITEMNAMEBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label152.AutoSize = true;
		this.label152.Location = new System.Drawing.Point(17, 163);
		this.label152.Name = "label152";
		this.label152.Size = new System.Drawing.Size(98, 13);
		this.label152.TabIndex = 42;
		this.label152.Text = "Reward Item Name";
		this.HNS_ITEMCOUNTBOX.Location = new System.Drawing.Point(211, 131);
		this.HNS_ITEMCOUNTBOX.MaxLength = 5;
		this.HNS_ITEMCOUNTBOX.Name = "HNS_ITEMCOUNTBOX";
		this.HNS_ITEMCOUNTBOX.Size = new System.Drawing.Size(84, 20);
		this.HNS_ITEMCOUNTBOX.TabIndex = 41;
		this.HNS_ITEMCOUNTBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label153.AutoSize = true;
		this.label153.Location = new System.Drawing.Point(17, 135);
		this.label153.Name = "label153";
		this.label153.Size = new System.Drawing.Size(98, 13);
		this.label153.TabIndex = 40;
		this.label153.Text = "Reward Item Count";
		this.HNS_ITEMREWARDBOX.Location = new System.Drawing.Point(211, 105);
		this.HNS_ITEMREWARDBOX.MaxLength = 5;
		this.HNS_ITEMREWARDBOX.Name = "HNS_ITEMREWARDBOX";
		this.HNS_ITEMREWARDBOX.Size = new System.Drawing.Size(84, 20);
		this.HNS_ITEMREWARDBOX.TabIndex = 39;
		this.HNS_ITEMREWARDBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.HNS_ROUNDSBOX.Location = new System.Drawing.Point(211, 79);
		this.HNS_ROUNDSBOX.MaxLength = 3;
		this.HNS_ROUNDSBOX.Name = "HNS_ROUNDSBOX";
		this.HNS_ROUNDSBOX.Size = new System.Drawing.Size(84, 20);
		this.HNS_ROUNDSBOX.TabIndex = 38;
		this.HNS_ROUNDSBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.HNS_TIMETOSEARCHBOX.Location = new System.Drawing.Point(211, 53);
		this.HNS_TIMETOSEARCHBOX.MaxLength = 3;
		this.HNS_TIMETOSEARCHBOX.Name = "HNS_TIMETOSEARCHBOX";
		this.HNS_TIMETOSEARCHBOX.Size = new System.Drawing.Size(84, 20);
		this.HNS_TIMETOSEARCHBOX.TabIndex = 37;
		this.HNS_TIMETOSEARCHBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label154.AutoSize = true;
		this.label154.Location = new System.Drawing.Point(17, 109);
		this.label154.Name = "label154";
		this.label154.Size = new System.Drawing.Size(81, 13);
		this.label154.TabIndex = 3;
		this.label154.Text = "Reward Item ID";
		this.label155.AutoSize = true;
		this.label155.Location = new System.Drawing.Point(17, 81);
		this.label155.Name = "label155";
		this.label155.Size = new System.Drawing.Size(39, 13);
		this.label155.TabIndex = 2;
		this.label155.Text = "Round";
		this.label156.AutoSize = true;
		this.label156.Location = new System.Drawing.Point(17, 56);
		this.label156.Name = "label156";
		this.label156.Size = new System.Drawing.Size(83, 13);
		this.label156.TabIndex = 1;
		this.label156.Text = "Time To Search";
		this.HNS_ENABLEBOX.AutoSize = true;
		this.HNS_ENABLEBOX.Location = new System.Drawing.Point(20, 30);
		this.HNS_ENABLEBOX.Name = "HNS_ENABLEBOX";
		this.HNS_ENABLEBOX.Size = new System.Drawing.Size(134, 17);
		this.HNS_ENABLEBOX.TabIndex = 0;
		this.HNS_ENABLEBOX.Text = "Enable Hide And Seek";
		this.HNS_ENABLEBOX.UseVisualStyleBackColor = true;
		this.groupBox38.Controls.Add(this.itemnametrivia);
		this.groupBox38.Controls.Add(this.label157);
		this.groupBox38.Controls.Add(this.questitemcount);
		this.groupBox38.Controls.Add(this.label158);
		this.groupBox38.Controls.Add(this.itemcodetrivia);
		this.groupBox38.Controls.Add(this.qnarounds);
		this.groupBox38.Controls.Add(this.textBox_QnATimeAnswer);
		this.groupBox38.Controls.Add(this.label191);
		this.groupBox38.Controls.Add(this.Rounds);
		this.groupBox38.Controls.Add(this.label189);
		this.groupBox38.Controls.Add(this.checkBox_QnAEnable);
		this.groupBox38.Location = new System.Drawing.Point(8, 17);
		this.groupBox38.Name = "groupBox38";
		this.groupBox38.Size = new System.Drawing.Size(314, 200);
		this.groupBox38.TabIndex = 13;
		this.groupBox38.TabStop = false;
		this.groupBox38.Text = "Question And Answer";
		this.itemnametrivia.Location = new System.Drawing.Point(173, 159);
		this.itemnametrivia.MaxLength = 5;
		this.itemnametrivia.Name = "itemnametrivia";
		this.itemnametrivia.Size = new System.Drawing.Size(122, 20);
		this.itemnametrivia.TabIndex = 43;
		this.itemnametrivia.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label157.AutoSize = true;
		this.label157.Location = new System.Drawing.Point(17, 163);
		this.label157.Name = "label157";
		this.label157.Size = new System.Drawing.Size(98, 13);
		this.label157.TabIndex = 42;
		this.label157.Text = "Reward Item Name";
		this.questitemcount.Location = new System.Drawing.Point(211, 131);
		this.questitemcount.MaxLength = 5;
		this.questitemcount.Name = "questitemcount";
		this.questitemcount.Size = new System.Drawing.Size(84, 20);
		this.questitemcount.TabIndex = 41;
		this.questitemcount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label158.AutoSize = true;
		this.label158.Location = new System.Drawing.Point(17, 135);
		this.label158.Name = "label158";
		this.label158.Size = new System.Drawing.Size(98, 13);
		this.label158.TabIndex = 40;
		this.label158.Text = "Reward Item Count";
		this.itemcodetrivia.Location = new System.Drawing.Point(211, 105);
		this.itemcodetrivia.MaxLength = 5;
		this.itemcodetrivia.Name = "itemcodetrivia";
		this.itemcodetrivia.Size = new System.Drawing.Size(84, 20);
		this.itemcodetrivia.TabIndex = 39;
		this.itemcodetrivia.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.qnarounds.Location = new System.Drawing.Point(211, 79);
		this.qnarounds.MaxLength = 3;
		this.qnarounds.Name = "qnarounds";
		this.qnarounds.Size = new System.Drawing.Size(84, 20);
		this.qnarounds.TabIndex = 38;
		this.qnarounds.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.textBox_QnATimeAnswer.Location = new System.Drawing.Point(211, 53);
		this.textBox_QnATimeAnswer.MaxLength = 3;
		this.textBox_QnATimeAnswer.Name = "textBox_QnATimeAnswer";
		this.textBox_QnATimeAnswer.Size = new System.Drawing.Size(84, 20);
		this.textBox_QnATimeAnswer.TabIndex = 37;
		this.textBox_QnATimeAnswer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label191.AutoSize = true;
		this.label191.Location = new System.Drawing.Point(17, 109);
		this.label191.Name = "label191";
		this.label191.Size = new System.Drawing.Size(81, 13);
		this.label191.TabIndex = 3;
		this.label191.Text = "Reward Item ID";
		this.Rounds.AutoSize = true;
		this.Rounds.Location = new System.Drawing.Point(17, 81);
		this.Rounds.Name = "Rounds";
		this.Rounds.Size = new System.Drawing.Size(39, 13);
		this.Rounds.TabIndex = 2;
		this.Rounds.Text = "Round";
		this.label189.AutoSize = true;
		this.label189.Location = new System.Drawing.Point(17, 56);
		this.label189.Name = "label189";
		this.label189.Size = new System.Drawing.Size(84, 13);
		this.label189.TabIndex = 1;
		this.label189.Text = "Time To Answer";
		this.checkBox_QnAEnable.AutoSize = true;
		this.checkBox_QnAEnable.Location = new System.Drawing.Point(20, 30);
		this.checkBox_QnAEnable.Name = "checkBox_QnAEnable";
		this.checkBox_QnAEnable.Size = new System.Drawing.Size(199, 17);
		this.checkBox_QnAEnable.TabIndex = 0;
		this.checkBox_QnAEnable.Text = "Enable Questions and Answer Event";
		this.checkBox_QnAEnable.UseVisualStyleBackColor = true;
		this.tabPage7.BackColor = System.Drawing.Color.WhiteSmoke;
		this.tabPage7.Controls.Add(this.groupBox13);
		this.tabPage7.Controls.Add(this.groupBox49);
		this.tabPage7.Location = new System.Drawing.Point(4, 22);
		this.tabPage7.Name = "tabPage7";
		this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage7.Size = new System.Drawing.Size(1033, 453);
		this.tabPage7.TabIndex = 6;
		this.tabPage7.Text = "GM Killer";
		this.groupBox13.Controls.Add(this.label188);
		this.groupBox13.Controls.Add(this.GMK_END_NOTICEBOX);
		this.groupBox13.Controls.Add(this.label196);
		this.groupBox13.Controls.Add(this.GMK_WIN_NOTICEBOX);
		this.groupBox13.Controls.Add(this.label197);
		this.groupBox13.Controls.Add(this.GMK_INFORM_NOTICEBOX);
		this.groupBox13.Controls.Add(this.label198);
		this.groupBox13.Controls.Add(this.GMK_INFO_NOTICEBOX);
		this.groupBox13.Controls.Add(this.label199);
		this.groupBox13.Controls.Add(this.GMK_START_NOTICEBOX);
		this.groupBox13.Controls.Add(this.label200);
		this.groupBox13.Controls.Add(this.GMK_PLACENAMEBOX);
		this.groupBox13.Location = new System.Drawing.Point(323, 9);
		this.groupBox13.Name = "groupBox13";
		this.groupBox13.Size = new System.Drawing.Size(699, 200);
		this.groupBox13.TabIndex = 19;
		this.groupBox13.TabStop = false;
		this.groupBox13.Text = "Notices";
		this.label188.AutoSize = true;
		this.label188.Location = new System.Drawing.Point(19, 102);
		this.label188.Name = "label188";
		this.label188.Size = new System.Drawing.Size(59, 13);
		this.label188.TabIndex = 167;
		this.label188.Text = "Info Notice";
		this.GMK_END_NOTICEBOX.Location = new System.Drawing.Point(84, 150);
		this.GMK_END_NOTICEBOX.Name = "GMK_END_NOTICEBOX";
		this.GMK_END_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.GMK_END_NOTICEBOX.TabIndex = 166;
		this.label196.AutoSize = true;
		this.label196.Location = new System.Drawing.Point(37, 127);
		this.label196.Name = "label196";
		this.label196.Size = new System.Drawing.Size(41, 13);
		this.label196.TabIndex = 165;
		this.label196.Text = "Winner";
		this.GMK_WIN_NOTICEBOX.Location = new System.Drawing.Point(84, 124);
		this.GMK_WIN_NOTICEBOX.Name = "GMK_WIN_NOTICEBOX";
		this.GMK_WIN_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.GMK_WIN_NOTICEBOX.TabIndex = 164;
		this.label197.AutoSize = true;
		this.label197.Location = new System.Drawing.Point(52, 153);
		this.label197.Name = "label197";
		this.label197.Size = new System.Drawing.Size(26, 13);
		this.label197.TabIndex = 163;
		this.label197.Text = "End";
		this.GMK_INFORM_NOTICEBOX.Location = new System.Drawing.Point(84, 98);
		this.GMK_INFORM_NOTICEBOX.Name = "GMK_INFORM_NOTICEBOX";
		this.GMK_INFORM_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.GMK_INFORM_NOTICEBOX.TabIndex = 162;
		this.label198.AutoSize = true;
		this.label198.Location = new System.Drawing.Point(19, 75);
		this.label198.Name = "label198";
		this.label198.Size = new System.Drawing.Size(59, 13);
		this.label198.TabIndex = 161;
		this.label198.Text = "Info Notice";
		this.GMK_INFO_NOTICEBOX.Location = new System.Drawing.Point(84, 72);
		this.GMK_INFO_NOTICEBOX.Name = "GMK_INFO_NOTICEBOX";
		this.GMK_INFO_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.GMK_INFO_NOTICEBOX.TabIndex = 160;
		this.label199.AutoSize = true;
		this.label199.Location = new System.Drawing.Point(15, 48);
		this.label199.Name = "label199";
		this.label199.Size = new System.Drawing.Size(63, 13);
		this.label199.TabIndex = 159;
		this.label199.Text = "Start Notice";
		this.GMK_START_NOTICEBOX.Location = new System.Drawing.Point(84, 45);
		this.GMK_START_NOTICEBOX.Name = "GMK_START_NOTICEBOX";
		this.GMK_START_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.GMK_START_NOTICEBOX.TabIndex = 158;
		this.label200.AutoSize = true;
		this.label200.Location = new System.Drawing.Point(13, 22);
		this.label200.Name = "label200";
		this.label200.Size = new System.Drawing.Size(65, 13);
		this.label200.TabIndex = 149;
		this.label200.Text = "Place Name";
		this.GMK_PLACENAMEBOX.Location = new System.Drawing.Point(84, 19);
		this.GMK_PLACENAMEBOX.Name = "GMK_PLACENAMEBOX";
		this.GMK_PLACENAMEBOX.Size = new System.Drawing.Size(609, 20);
		this.GMK_PLACENAMEBOX.TabIndex = 148;
		this.groupBox49.Controls.Add(this.label149);
		this.groupBox49.Controls.Add(this.GMK_ITEMCOUNTBOX);
		this.groupBox49.Controls.Add(this.label150);
		this.groupBox49.Controls.Add(this.GMK_ROUNDBOX);
		this.groupBox49.Controls.Add(this.GMK_ITEMNAMEBOX);
		this.groupBox49.Controls.Add(this.label151);
		this.groupBox49.Controls.Add(this.label243);
		this.groupBox49.Controls.Add(this.label242);
		this.groupBox49.Controls.Add(this.label221);
		this.groupBox49.Controls.Add(this.GMK_POSZBOX);
		this.groupBox49.Controls.Add(this.GMK_POSYBOX);
		this.groupBox49.Controls.Add(this.GMK_POSXBOX);
		this.groupBox49.Controls.Add(this.GMK_REGIONIDBOX);
		this.groupBox49.Controls.Add(this.label241);
		this.groupBox49.Controls.Add(this.GMK_ITEMIDBOX);
		this.groupBox49.Controls.Add(this.GMK_TIMETOWAITBOX);
		this.groupBox49.Controls.Add(this.label238);
		this.groupBox49.Controls.Add(this.label240);
		this.groupBox49.Controls.Add(this.GMK_ENABLEBOX);
		this.groupBox49.Location = new System.Drawing.Point(3, 6);
		this.groupBox49.Name = "groupBox49";
		this.groupBox49.Size = new System.Drawing.Size(314, 406);
		this.groupBox49.TabIndex = 18;
		this.groupBox49.TabStop = false;
		this.groupBox49.Text = "GM Killer Event";
		this.label149.AutoSize = true;
		this.label149.Location = new System.Drawing.Point(20, 75);
		this.label149.Name = "label149";
		this.label149.Size = new System.Drawing.Size(39, 13);
		this.label149.TabIndex = 82;
		this.label149.Text = "Round";
		this.GMK_ITEMCOUNTBOX.Location = new System.Drawing.Point(240, 130);
		this.GMK_ITEMCOUNTBOX.MaxLength = 10;
		this.GMK_ITEMCOUNTBOX.Name = "GMK_ITEMCOUNTBOX";
		this.GMK_ITEMCOUNTBOX.Size = new System.Drawing.Size(54, 20);
		this.GMK_ITEMCOUNTBOX.TabIndex = 51;
		this.GMK_ITEMCOUNTBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label150.AutoSize = true;
		this.label150.Location = new System.Drawing.Point(20, 135);
		this.label150.Name = "label150";
		this.label150.Size = new System.Drawing.Size(98, 13);
		this.label150.TabIndex = 50;
		this.label150.Text = "Reward Item Count";
		this.GMK_ROUNDBOX.Location = new System.Drawing.Point(240, 70);
		this.GMK_ROUNDBOX.MaxLength = 10;
		this.GMK_ROUNDBOX.Name = "GMK_ROUNDBOX";
		this.GMK_ROUNDBOX.Size = new System.Drawing.Size(54, 20);
		this.GMK_ROUNDBOX.TabIndex = 81;
		this.GMK_ROUNDBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.GMK_ITEMNAMEBOX.Location = new System.Drawing.Point(161, 156);
		this.GMK_ITEMNAMEBOX.MaxLength = 10;
		this.GMK_ITEMNAMEBOX.Name = "GMK_ITEMNAMEBOX";
		this.GMK_ITEMNAMEBOX.Size = new System.Drawing.Size(133, 20);
		this.GMK_ITEMNAMEBOX.TabIndex = 49;
		this.GMK_ITEMNAMEBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label151.AutoSize = true;
		this.label151.Location = new System.Drawing.Point(20, 161);
		this.label151.Name = "label151";
		this.label151.Size = new System.Drawing.Size(98, 13);
		this.label151.TabIndex = 48;
		this.label151.Text = "Reward Item Name";
		this.label243.AutoSize = true;
		this.label243.Location = new System.Drawing.Point(20, 263);
		this.label243.Name = "label243";
		this.label243.Size = new System.Drawing.Size(39, 13);
		this.label243.TabIndex = 47;
		this.label243.Text = "POS Z";
		this.label242.AutoSize = true;
		this.label242.Location = new System.Drawing.Point(20, 238);
		this.label242.Name = "label242";
		this.label242.Size = new System.Drawing.Size(39, 13);
		this.label242.TabIndex = 46;
		this.label242.Text = "POS Y";
		this.label221.AutoSize = true;
		this.label221.Location = new System.Drawing.Point(20, 213);
		this.label221.Name = "label221";
		this.label221.Size = new System.Drawing.Size(39, 13);
		this.label221.TabIndex = 45;
		this.label221.Text = "POS X";
		this.GMK_POSZBOX.Location = new System.Drawing.Point(240, 258);
		this.GMK_POSZBOX.MaxLength = 10;
		this.GMK_POSZBOX.Name = "GMK_POSZBOX";
		this.GMK_POSZBOX.Size = new System.Drawing.Size(54, 20);
		this.GMK_POSZBOX.TabIndex = 44;
		this.GMK_POSZBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.GMK_POSYBOX.Location = new System.Drawing.Point(240, 233);
		this.GMK_POSYBOX.MaxLength = 10;
		this.GMK_POSYBOX.Name = "GMK_POSYBOX";
		this.GMK_POSYBOX.Size = new System.Drawing.Size(54, 20);
		this.GMK_POSYBOX.TabIndex = 43;
		this.GMK_POSYBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.GMK_POSXBOX.Location = new System.Drawing.Point(240, 208);
		this.GMK_POSXBOX.MaxLength = 10;
		this.GMK_POSXBOX.Name = "GMK_POSXBOX";
		this.GMK_POSXBOX.Size = new System.Drawing.Size(54, 20);
		this.GMK_POSXBOX.TabIndex = 42;
		this.GMK_POSXBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.GMK_REGIONIDBOX.Location = new System.Drawing.Point(240, 183);
		this.GMK_REGIONIDBOX.MaxLength = 10;
		this.GMK_REGIONIDBOX.Name = "GMK_REGIONIDBOX";
		this.GMK_REGIONIDBOX.Size = new System.Drawing.Size(54, 20);
		this.GMK_REGIONIDBOX.TabIndex = 41;
		this.GMK_REGIONIDBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label241.AutoSize = true;
		this.label241.Location = new System.Drawing.Point(20, 188);
		this.label241.Name = "label241";
		this.label241.Size = new System.Drawing.Size(52, 13);
		this.label241.TabIndex = 40;
		this.label241.Text = "RegionID";
		this.GMK_ITEMIDBOX.Location = new System.Drawing.Point(240, 102);
		this.GMK_ITEMIDBOX.MaxLength = 10;
		this.GMK_ITEMIDBOX.Name = "GMK_ITEMIDBOX";
		this.GMK_ITEMIDBOX.Size = new System.Drawing.Size(54, 20);
		this.GMK_ITEMIDBOX.TabIndex = 39;
		this.GMK_ITEMIDBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.GMK_TIMETOWAITBOX.Location = new System.Drawing.Point(240, 41);
		this.GMK_TIMETOWAITBOX.MaxLength = 10;
		this.GMK_TIMETOWAITBOX.Name = "GMK_TIMETOWAITBOX";
		this.GMK_TIMETOWAITBOX.Size = new System.Drawing.Size(54, 20);
		this.GMK_TIMETOWAITBOX.TabIndex = 37;
		this.GMK_TIMETOWAITBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label238.AutoSize = true;
		this.label238.Location = new System.Drawing.Point(20, 107);
		this.label238.Name = "label238";
		this.label238.Size = new System.Drawing.Size(81, 13);
		this.label238.TabIndex = 3;
		this.label238.Text = "Reward Item ID";
		this.label240.AutoSize = true;
		this.label240.Location = new System.Drawing.Point(20, 44);
		this.label240.Name = "label240";
		this.label240.Size = new System.Drawing.Size(62, 13);
		this.label240.TabIndex = 1;
		this.label240.Text = "Time To Kill";
		this.GMK_ENABLEBOX.AutoSize = true;
		this.GMK_ENABLEBOX.Location = new System.Drawing.Point(23, 19);
		this.GMK_ENABLEBOX.Name = "GMK_ENABLEBOX";
		this.GMK_ENABLEBOX.Size = new System.Drawing.Size(135, 17);
		this.GMK_ENABLEBOX.TabIndex = 0;
		this.GMK_ENABLEBOX.Text = "Enable GM Killer Event";
		this.GMK_ENABLEBOX.UseVisualStyleBackColor = true;
		this.tabPage8.BackColor = System.Drawing.Color.WhiteSmoke;
		this.tabPage8.Controls.Add(this.groupBox15);
		this.tabPage8.Controls.Add(this.groupBox14);
		this.tabPage8.Controls.Add(this.groupBox45);
		this.tabPage8.Controls.Add(this.groupBox43);
		this.tabPage8.Location = new System.Drawing.Point(4, 22);
		this.tabPage8.Name = "tabPage8";
		this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage8.Size = new System.Drawing.Size(1033, 453);
		this.tabPage8.TabIndex = 7;
		this.tabPage8.Text = "LG & SND";
		this.groupBox15.Controls.Add(this.LG_ENDD_NOTICEBOX);
		this.groupBox15.Controls.Add(this.label201);
		this.groupBox15.Controls.Add(this.label207);
		this.groupBox15.Controls.Add(this.LG_REGISTED_NOTICEBOX);
		this.groupBox15.Controls.Add(this.label216);
		this.groupBox15.Controls.Add(this.LG_REGISTERSUCCESS_NOTICEBOX);
		this.groupBox15.Controls.Add(this.label213);
		this.groupBox15.Controls.Add(this.LG_GOLDREQUIRE_NOTICEBOX);
		this.groupBox15.Controls.Add(this.label215);
		this.groupBox15.Controls.Add(this.LG_STOPREG_NOTICEBOX);
		this.groupBox15.Controls.Add(this.label208);
		this.groupBox15.Controls.Add(this.LG_STARTREG_NOTICEBOX);
		this.groupBox15.Controls.Add(this.label209);
		this.groupBox15.Controls.Add(this.LG_WIN_NOTICEBOX);
		this.groupBox15.Controls.Add(this.label210);
		this.groupBox15.Controls.Add(this.LG_END_NOTICEBOX);
		this.groupBox15.Controls.Add(this.label211);
		this.groupBox15.Controls.Add(this.LG_TICKETPRICE_NOTICEBOX);
		this.groupBox15.Controls.Add(this.label212);
		this.groupBox15.Controls.Add(this.LG_START_NOTICEBOX);
		this.groupBox15.Location = new System.Drawing.Point(326, 9);
		this.groupBox15.Name = "groupBox15";
		this.groupBox15.Size = new System.Drawing.Size(699, 279);
		this.groupBox15.TabIndex = 21;
		this.groupBox15.TabStop = false;
		this.groupBox15.Text = "Notices";
		this.LG_ENDD_NOTICEBOX.Location = new System.Drawing.Point(84, 253);
		this.LG_ENDD_NOTICEBOX.Name = "LG_ENDD_NOTICEBOX";
		this.LG_ENDD_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.LG_ENDD_NOTICEBOX.TabIndex = 176;
		this.label201.AutoSize = true;
		this.label201.Location = new System.Drawing.Point(44, 256);
		this.label201.Name = "label201";
		this.label201.Size = new System.Drawing.Size(34, 13);
		this.label201.TabIndex = 175;
		this.label201.Text = "Finish";
		this.label207.AutoSize = true;
		this.label207.Location = new System.Drawing.Point(7, 127);
		this.label207.Name = "label207";
		this.label207.Size = new System.Drawing.Size(71, 13);
		this.label207.TabIndex = 174;
		this.label207.Text = "Start Register";
		this.LG_REGISTED_NOTICEBOX.Location = new System.Drawing.Point(84, 228);
		this.LG_REGISTED_NOTICEBOX.Name = "LG_REGISTED_NOTICEBOX";
		this.LG_REGISTED_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.LG_REGISTED_NOTICEBOX.TabIndex = 173;
		this.label216.AutoSize = true;
		this.label216.Location = new System.Drawing.Point(13, 231);
		this.label216.Name = "label216";
		this.label216.Size = new System.Drawing.Size(65, 13);
		this.label216.TabIndex = 172;
		this.label216.Text = "Already Req";
		this.LG_REGISTERSUCCESS_NOTICEBOX.Location = new System.Drawing.Point(84, 203);
		this.LG_REGISTERSUCCESS_NOTICEBOX.Name = "LG_REGISTERSUCCESS_NOTICEBOX";
		this.LG_REGISTERSUCCESS_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.LG_REGISTERSUCCESS_NOTICEBOX.TabIndex = 171;
		this.label213.AutoSize = true;
		this.label213.Location = new System.Drawing.Point(26, 180);
		this.label213.Name = "label213";
		this.label213.Size = new System.Drawing.Size(52, 13);
		this.label213.TabIndex = 170;
		this.label213.Text = "Gold Req";
		this.LG_GOLDREQUIRE_NOTICEBOX.Location = new System.Drawing.Point(84, 177);
		this.LG_GOLDREQUIRE_NOTICEBOX.Name = "LG_GOLDREQUIRE_NOTICEBOX";
		this.LG_GOLDREQUIRE_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.LG_GOLDREQUIRE_NOTICEBOX.TabIndex = 169;
		this.label215.AutoSize = true;
		this.label215.Location = new System.Drawing.Point(12, 206);
		this.label215.Name = "label215";
		this.label215.Size = new System.Drawing.Size(66, 13);
		this.label215.TabIndex = 168;
		this.label215.Text = "Succes Req";
		this.LG_STOPREG_NOTICEBOX.Location = new System.Drawing.Point(84, 150);
		this.LG_STOPREG_NOTICEBOX.Name = "LG_STOPREG_NOTICEBOX";
		this.LG_STOPREG_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.LG_STOPREG_NOTICEBOX.TabIndex = 166;
		this.label208.AutoSize = true;
		this.label208.Location = new System.Drawing.Point(37, 101);
		this.label208.Name = "label208";
		this.label208.Size = new System.Drawing.Size(41, 13);
		this.label208.TabIndex = 165;
		this.label208.Text = "Winner";
		this.LG_STARTREG_NOTICEBOX.Location = new System.Drawing.Point(84, 124);
		this.LG_STARTREG_NOTICEBOX.Name = "LG_STARTREG_NOTICEBOX";
		this.LG_STARTREG_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.LG_STARTREG_NOTICEBOX.TabIndex = 164;
		this.label209.AutoSize = true;
		this.label209.Location = new System.Drawing.Point(2, 153);
		this.label209.Name = "label209";
		this.label209.Size = new System.Drawing.Size(76, 13);
		this.label209.TabIndex = 163;
		this.label209.Text = "Finish Register";
		this.LG_WIN_NOTICEBOX.Location = new System.Drawing.Point(84, 98);
		this.LG_WIN_NOTICEBOX.Name = "LG_WIN_NOTICEBOX";
		this.LG_WIN_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.LG_WIN_NOTICEBOX.TabIndex = 162;
		this.label210.AutoSize = true;
		this.label210.Location = new System.Drawing.Point(20, 75);
		this.label210.Name = "label210";
		this.label210.Size = new System.Drawing.Size(58, 13);
		this.label210.TabIndex = 161;
		this.label210.Text = "No Winner";
		this.LG_END_NOTICEBOX.Location = new System.Drawing.Point(84, 72);
		this.LG_END_NOTICEBOX.Name = "LG_END_NOTICEBOX";
		this.LG_END_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.LG_END_NOTICEBOX.TabIndex = 160;
		this.label211.AutoSize = true;
		this.label211.Location = new System.Drawing.Point(14, 48);
		this.label211.Name = "label211";
		this.label211.Size = new System.Drawing.Size(64, 13);
		this.label211.TabIndex = 159;
		this.label211.Text = "Ticket Price";
		this.LG_TICKETPRICE_NOTICEBOX.Location = new System.Drawing.Point(84, 45);
		this.LG_TICKETPRICE_NOTICEBOX.Name = "LG_TICKETPRICE_NOTICEBOX";
		this.LG_TICKETPRICE_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.LG_TICKETPRICE_NOTICEBOX.TabIndex = 158;
		this.label212.AutoSize = true;
		this.label212.Location = new System.Drawing.Point(15, 22);
		this.label212.Name = "label212";
		this.label212.Size = new System.Drawing.Size(63, 13);
		this.label212.TabIndex = 149;
		this.label212.Text = "Start Notice";
		this.LG_START_NOTICEBOX.Location = new System.Drawing.Point(84, 19);
		this.LG_START_NOTICEBOX.Name = "LG_START_NOTICEBOX";
		this.LG_START_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.LG_START_NOTICEBOX.TabIndex = 148;
		this.groupBox14.Controls.Add(this.SND_ENDBOX);
		this.groupBox14.Controls.Add(this.label202);
		this.groupBox14.Controls.Add(this.SND_WINBOX);
		this.groupBox14.Controls.Add(this.label203);
		this.groupBox14.Controls.Add(this.label204);
		this.groupBox14.Controls.Add(this.SND_PLACEINFOBOX);
		this.groupBox14.Controls.Add(this.label205);
		this.groupBox14.Controls.Add(this.SND_INFONOTICEBOX);
		this.groupBox14.Controls.Add(this.label206);
		this.groupBox14.Controls.Add(this.SND_STARTNOTICEBOX);
		this.groupBox14.Location = new System.Drawing.Point(326, 294);
		this.groupBox14.Name = "groupBox14";
		this.groupBox14.Size = new System.Drawing.Size(699, 153);
		this.groupBox14.TabIndex = 20;
		this.groupBox14.TabStop = false;
		this.groupBox14.Text = "Notices";
		this.SND_ENDBOX.Location = new System.Drawing.Point(77, 123);
		this.SND_ENDBOX.Name = "SND_ENDBOX";
		this.SND_ENDBOX.Size = new System.Drawing.Size(609, 20);
		this.SND_ENDBOX.TabIndex = 166;
		this.label202.AutoSize = true;
		this.label202.Location = new System.Drawing.Point(30, 100);
		this.label202.Name = "label202";
		this.label202.Size = new System.Drawing.Size(41, 13);
		this.label202.TabIndex = 165;
		this.label202.Text = "Winner";
		this.SND_WINBOX.Location = new System.Drawing.Point(77, 97);
		this.SND_WINBOX.Name = "SND_WINBOX";
		this.SND_WINBOX.Size = new System.Drawing.Size(609, 20);
		this.SND_WINBOX.TabIndex = 164;
		this.label203.AutoSize = true;
		this.label203.Location = new System.Drawing.Point(45, 126);
		this.label203.Name = "label203";
		this.label203.Size = new System.Drawing.Size(26, 13);
		this.label203.TabIndex = 163;
		this.label203.Text = "End";
		this.label204.AutoSize = true;
		this.label204.Location = new System.Drawing.Point(12, 47);
		this.label204.Name = "label204";
		this.label204.Size = new System.Drawing.Size(59, 13);
		this.label204.TabIndex = 161;
		this.label204.Text = "Info Notice";
		this.SND_PLACEINFOBOX.Location = new System.Drawing.Point(77, 71);
		this.SND_PLACEINFOBOX.Name = "SND_PLACEINFOBOX";
		this.SND_PLACEINFOBOX.Size = new System.Drawing.Size(609, 20);
		this.SND_PLACEINFOBOX.TabIndex = 160;
		this.label205.AutoSize = true;
		this.label205.Location = new System.Drawing.Point(8, 21);
		this.label205.Name = "label205";
		this.label205.Size = new System.Drawing.Size(63, 13);
		this.label205.TabIndex = 159;
		this.label205.Text = "Start Notice";
		this.SND_INFONOTICEBOX.Location = new System.Drawing.Point(77, 44);
		this.SND_INFONOTICEBOX.Name = "SND_INFONOTICEBOX";
		this.SND_INFONOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.SND_INFONOTICEBOX.TabIndex = 158;
		this.label206.AutoSize = true;
		this.label206.Location = new System.Drawing.Point(6, 74);
		this.label206.Name = "label206";
		this.label206.Size = new System.Drawing.Size(65, 13);
		this.label206.TabIndex = 149;
		this.label206.Text = "Place Name";
		this.SND_STARTNOTICEBOX.Location = new System.Drawing.Point(77, 18);
		this.SND_STARTNOTICEBOX.Name = "SND_STARTNOTICEBOX";
		this.SND_STARTNOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.SND_STARTNOTICEBOX.TabIndex = 148;
		this.groupBox45.Controls.Add(this.LG_ROUNDBOX);
		this.groupBox45.Controls.Add(this.label143);
		this.groupBox45.Controls.Add(this.LG_TICKETPRICEBOX);
		this.groupBox45.Controls.Add(this.LG_TIMETOWAITBOX);
		this.groupBox45.Controls.Add(this.label223);
		this.groupBox45.Controls.Add(this.label224);
		this.groupBox45.Controls.Add(this.LG_ENABLEBOX);
		this.groupBox45.Location = new System.Drawing.Point(6, 12);
		this.groupBox45.Name = "groupBox45";
		this.groupBox45.Size = new System.Drawing.Size(314, 175);
		this.groupBox45.TabIndex = 19;
		this.groupBox45.TabStop = false;
		this.groupBox45.Text = "Lottery Gold Event";
		this.LG_ROUNDBOX.Location = new System.Drawing.Point(138, 102);
		this.LG_ROUNDBOX.MaxLength = 3;
		this.LG_ROUNDBOX.Name = "LG_ROUNDBOX";
		this.LG_ROUNDBOX.Size = new System.Drawing.Size(156, 20);
		this.LG_ROUNDBOX.TabIndex = 40;
		this.LG_ROUNDBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label143.AutoSize = true;
		this.label143.Location = new System.Drawing.Point(20, 105);
		this.label143.Name = "label143";
		this.label143.Size = new System.Drawing.Size(44, 13);
		this.label143.TabIndex = 39;
		this.label143.Text = "Rounds";
		this.LG_TICKETPRICEBOX.Location = new System.Drawing.Point(138, 75);
		this.LG_TICKETPRICEBOX.MaxLength = 300;
		this.LG_TICKETPRICEBOX.Name = "LG_TICKETPRICEBOX";
		this.LG_TICKETPRICEBOX.Size = new System.Drawing.Size(156, 20);
		this.LG_TICKETPRICEBOX.TabIndex = 38;
		this.LG_TICKETPRICEBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.LG_TIMETOWAITBOX.Location = new System.Drawing.Point(138, 50);
		this.LG_TIMETOWAITBOX.MaxLength = 10;
		this.LG_TIMETOWAITBOX.Name = "LG_TIMETOWAITBOX";
		this.LG_TIMETOWAITBOX.Size = new System.Drawing.Size(156, 20);
		this.LG_TIMETOWAITBOX.TabIndex = 37;
		this.LG_TIMETOWAITBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label223.AutoSize = true;
		this.label223.Location = new System.Drawing.Point(20, 80);
		this.label223.Name = "label223";
		this.label223.Size = new System.Drawing.Size(64, 13);
		this.label223.TabIndex = 2;
		this.label223.Text = "Ticket Price";
		this.label224.AutoSize = true;
		this.label224.Location = new System.Drawing.Point(20, 55);
		this.label224.Name = "label224";
		this.label224.Size = new System.Drawing.Size(88, 13);
		this.label224.TabIndex = 1;
		this.label224.Text = "Time To Register";
		this.LG_ENABLEBOX.AutoSize = true;
		this.LG_ENABLEBOX.Location = new System.Drawing.Point(20, 30);
		this.LG_ENABLEBOX.Name = "LG_ENABLEBOX";
		this.LG_ENABLEBOX.Size = new System.Drawing.Size(150, 17);
		this.LG_ENABLEBOX.TabIndex = 0;
		this.LG_ENABLEBOX.Text = "Enable Lottery Gold Event";
		this.LG_ENABLEBOX.UseVisualStyleBackColor = true;
		this.groupBox43.Controls.Add(this.label144);
		this.groupBox43.Controls.Add(this.SND_MOBIDBOX);
		this.groupBox43.Controls.Add(this.label145);
		this.groupBox43.Controls.Add(this.SND_TIMETOSEARCHBOX);
		this.groupBox43.Controls.Add(this.label214);
		this.groupBox43.Controls.Add(this.SND_ITEMCOUNTBOX);
		this.groupBox43.Controls.Add(this.SND_ENABLEBOX);
		this.groupBox43.Controls.Add(this.label146);
		this.groupBox43.Controls.Add(this.label147);
		this.groupBox43.Controls.Add(this.SND_ITEMNAMEBOX);
		this.groupBox43.Controls.Add(this.SND_ITEMREWARDBOX);
		this.groupBox43.Controls.Add(this.label148);
		this.groupBox43.Controls.Add(this.SND_ROUNDSBOX);
		this.groupBox43.Location = new System.Drawing.Point(8, 226);
		this.groupBox43.Name = "groupBox43";
		this.groupBox43.Size = new System.Drawing.Size(312, 221);
		this.groupBox43.TabIndex = 18;
		this.groupBox43.TabStop = false;
		this.groupBox43.Text = "Search And Destroy Event";
		this.label144.AutoSize = true;
		this.label144.Location = new System.Drawing.Point(22, 183);
		this.label144.Name = "label144";
		this.label144.Size = new System.Drawing.Size(39, 13);
		this.label144.TabIndex = 92;
		this.label144.Text = "MobID";
		this.SND_MOBIDBOX.Location = new System.Drawing.Point(242, 178);
		this.SND_MOBIDBOX.MaxLength = 10;
		this.SND_MOBIDBOX.Name = "SND_MOBIDBOX";
		this.SND_MOBIDBOX.Size = new System.Drawing.Size(54, 20);
		this.SND_MOBIDBOX.TabIndex = 91;
		this.SND_MOBIDBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label145.AutoSize = true;
		this.label145.Location = new System.Drawing.Point(22, 71);
		this.label145.Name = "label145";
		this.label145.Size = new System.Drawing.Size(39, 13);
		this.label145.TabIndex = 90;
		this.label145.Text = "Round";
		this.SND_TIMETOSEARCHBOX.Location = new System.Drawing.Point(242, 39);
		this.SND_TIMETOSEARCHBOX.MaxLength = 3;
		this.SND_TIMETOSEARCHBOX.Name = "SND_TIMETOSEARCHBOX";
		this.SND_TIMETOSEARCHBOX.Size = new System.Drawing.Size(54, 20);
		this.SND_TIMETOSEARCHBOX.TabIndex = 37;
		this.SND_TIMETOSEARCHBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label214.AutoSize = true;
		this.label214.Location = new System.Drawing.Point(20, 44);
		this.label214.Name = "label214";
		this.label214.Size = new System.Drawing.Size(62, 13);
		this.label214.TabIndex = 1;
		this.label214.Text = "Time To Kill";
		this.SND_ITEMCOUNTBOX.Location = new System.Drawing.Point(242, 126);
		this.SND_ITEMCOUNTBOX.MaxLength = 10;
		this.SND_ITEMCOUNTBOX.Name = "SND_ITEMCOUNTBOX";
		this.SND_ITEMCOUNTBOX.Size = new System.Drawing.Size(54, 20);
		this.SND_ITEMCOUNTBOX.TabIndex = 88;
		this.SND_ITEMCOUNTBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.SND_ENABLEBOX.AutoSize = true;
		this.SND_ENABLEBOX.Location = new System.Drawing.Point(20, 19);
		this.SND_ENABLEBOX.Name = "SND_ENABLEBOX";
		this.SND_ENABLEBOX.Size = new System.Drawing.Size(188, 17);
		this.SND_ENABLEBOX.TabIndex = 0;
		this.SND_ENABLEBOX.Text = "Enable Search And Destroy Event";
		this.SND_ENABLEBOX.UseVisualStyleBackColor = true;
		this.label146.AutoSize = true;
		this.label146.Location = new System.Drawing.Point(22, 103);
		this.label146.Name = "label146";
		this.label146.Size = new System.Drawing.Size(81, 13);
		this.label146.TabIndex = 83;
		this.label146.Text = "Reward Item ID";
		this.label147.AutoSize = true;
		this.label147.Location = new System.Drawing.Point(22, 131);
		this.label147.Name = "label147";
		this.label147.Size = new System.Drawing.Size(98, 13);
		this.label147.TabIndex = 87;
		this.label147.Text = "Reward Item Count";
		this.SND_ITEMNAMEBOX.Location = new System.Drawing.Point(163, 152);
		this.SND_ITEMNAMEBOX.MaxLength = 10;
		this.SND_ITEMNAMEBOX.Name = "SND_ITEMNAMEBOX";
		this.SND_ITEMNAMEBOX.Size = new System.Drawing.Size(133, 20);
		this.SND_ITEMNAMEBOX.TabIndex = 86;
		this.SND_ITEMNAMEBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.SND_ITEMREWARDBOX.Location = new System.Drawing.Point(242, 98);
		this.SND_ITEMREWARDBOX.MaxLength = 10;
		this.SND_ITEMREWARDBOX.Name = "SND_ITEMREWARDBOX";
		this.SND_ITEMREWARDBOX.Size = new System.Drawing.Size(54, 20);
		this.SND_ITEMREWARDBOX.TabIndex = 84;
		this.SND_ITEMREWARDBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label148.AutoSize = true;
		this.label148.Location = new System.Drawing.Point(22, 157);
		this.label148.Name = "label148";
		this.label148.Size = new System.Drawing.Size(98, 13);
		this.label148.TabIndex = 85;
		this.label148.Text = "Reward Item Name";
		this.SND_ROUNDSBOX.Location = new System.Drawing.Point(242, 66);
		this.SND_ROUNDSBOX.MaxLength = 10;
		this.SND_ROUNDSBOX.Name = "SND_ROUNDSBOX";
		this.SND_ROUNDSBOX.Size = new System.Drawing.Size(54, 20);
		this.SND_ROUNDSBOX.TabIndex = 89;
		this.SND_ROUNDSBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.tabPage9.BackColor = System.Drawing.Color.WhiteSmoke;
		this.tabPage9.Controls.Add(this.groupBox17);
		this.tabPage9.Controls.Add(this.groupBox47);
		this.tabPage9.Location = new System.Drawing.Point(4, 22);
		this.tabPage9.Name = "tabPage9";
		this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage9.Size = new System.Drawing.Size(1033, 453);
		this.tabPage9.TabIndex = 8;
		this.tabPage9.Text = "LPN";
		this.groupBox17.Controls.Add(this.label225);
		this.groupBox17.Controls.Add(this.LPN_ROUNDINFOBOX);
		this.groupBox17.Controls.Add(this.label217);
		this.groupBox17.Controls.Add(this.LPN_WIN_NOTICEBOX);
		this.groupBox17.Controls.Add(this.label218);
		this.groupBox17.Controls.Add(this.LPN_ENDBOX);
		this.groupBox17.Controls.Add(this.label219);
		this.groupBox17.Controls.Add(this.LPN_NOREFORM_NOTICEBOX);
		this.groupBox17.Controls.Add(this.label220);
		this.groupBox17.Controls.Add(this.LPN_INFOBOX);
		this.groupBox17.Controls.Add(this.label222);
		this.groupBox17.Controls.Add(this.LPN_START_NOTICEBOX);
		this.groupBox17.Location = new System.Drawing.Point(326, 7);
		this.groupBox17.Name = "groupBox17";
		this.groupBox17.Size = new System.Drawing.Size(699, 200);
		this.groupBox17.TabIndex = 21;
		this.groupBox17.TabStop = false;
		this.groupBox17.Text = "Notices";
		this.label225.AutoSize = true;
		this.label225.Location = new System.Drawing.Point(18, 152);
		this.label225.Name = "label225";
		this.label225.Size = new System.Drawing.Size(60, 13);
		this.label225.TabIndex = 167;
		this.label225.Text = "Round Info";
		this.LPN_ROUNDINFOBOX.Location = new System.Drawing.Point(84, 149);
		this.LPN_ROUNDINFOBOX.Name = "LPN_ROUNDINFOBOX";
		this.LPN_ROUNDINFOBOX.Size = new System.Drawing.Size(609, 20);
		this.LPN_ROUNDINFOBOX.TabIndex = 166;
		this.label217.AutoSize = true;
		this.label217.Location = new System.Drawing.Point(37, 127);
		this.label217.Name = "label217";
		this.label217.Size = new System.Drawing.Size(41, 13);
		this.label217.TabIndex = 165;
		this.label217.Text = "Winner";
		this.LPN_WIN_NOTICEBOX.Location = new System.Drawing.Point(84, 124);
		this.LPN_WIN_NOTICEBOX.Name = "LPN_WIN_NOTICEBOX";
		this.LPN_WIN_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.LPN_WIN_NOTICEBOX.TabIndex = 164;
		this.label218.AutoSize = true;
		this.label218.Location = new System.Drawing.Point(52, 101);
		this.label218.Name = "label218";
		this.label218.Size = new System.Drawing.Size(26, 13);
		this.label218.TabIndex = 163;
		this.label218.Text = "End";
		this.LPN_ENDBOX.Location = new System.Drawing.Point(84, 98);
		this.LPN_ENDBOX.Name = "LPN_ENDBOX";
		this.LPN_ENDBOX.Size = new System.Drawing.Size(609, 20);
		this.LPN_ENDBOX.TabIndex = 162;
		this.label219.AutoSize = true;
		this.label219.Location = new System.Drawing.Point(20, 75);
		this.label219.Name = "label219";
		this.label219.Size = new System.Drawing.Size(58, 13);
		this.label219.TabIndex = 161;
		this.label219.Text = "No Reform";
		this.LPN_NOREFORM_NOTICEBOX.Location = new System.Drawing.Point(84, 72);
		this.LPN_NOREFORM_NOTICEBOX.Name = "LPN_NOREFORM_NOTICEBOX";
		this.LPN_NOREFORM_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.LPN_NOREFORM_NOTICEBOX.TabIndex = 160;
		this.label220.AutoSize = true;
		this.label220.Location = new System.Drawing.Point(19, 48);
		this.label220.Name = "label220";
		this.label220.Size = new System.Drawing.Size(59, 13);
		this.label220.TabIndex = 159;
		this.label220.Text = "Info Notice";
		this.LPN_INFOBOX.Location = new System.Drawing.Point(84, 45);
		this.LPN_INFOBOX.Name = "LPN_INFOBOX";
		this.LPN_INFOBOX.Size = new System.Drawing.Size(609, 20);
		this.LPN_INFOBOX.TabIndex = 158;
		this.label222.AutoSize = true;
		this.label222.Location = new System.Drawing.Point(15, 22);
		this.label222.Name = "label222";
		this.label222.Size = new System.Drawing.Size(63, 13);
		this.label222.TabIndex = 149;
		this.label222.Text = "Start Notice";
		this.LPN_START_NOTICEBOX.Location = new System.Drawing.Point(84, 19);
		this.LPN_START_NOTICEBOX.Name = "LPN_START_NOTICEBOX";
		this.LPN_START_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.LPN_START_NOTICEBOX.TabIndex = 148;
		this.groupBox47.Controls.Add(this.label176);
		this.groupBox47.Controls.Add(this.PT_MAXVALUEBOX);
		this.groupBox47.Controls.Add(this.PT_MINVALUEBOX);
		this.groupBox47.Controls.Add(this.label177);
		this.groupBox47.Controls.Add(this.label178);
		this.groupBox47.Controls.Add(this.LPN_ITEMCOUNTBOX);
		this.groupBox47.Controls.Add(this.label179);
		this.groupBox47.Controls.Add(this.LPN_ROUNDSBOX);
		this.groupBox47.Controls.Add(this.LPN_ITEMNAMEBOX);
		this.groupBox47.Controls.Add(this.label180);
		this.groupBox47.Controls.Add(this.LPN_ITEMREWARDBOX);
		this.groupBox47.Controls.Add(this.label181);
		this.groupBox47.Controls.Add(this.LPN_TIMETOWAITBOX);
		this.groupBox47.Controls.Add(this.label233);
		this.groupBox47.Controls.Add(this.LPN_ENABLEBOX);
		this.groupBox47.Location = new System.Drawing.Point(6, 7);
		this.groupBox47.Name = "groupBox47";
		this.groupBox47.Size = new System.Drawing.Size(314, 263);
		this.groupBox47.TabIndex = 20;
		this.groupBox47.TabStop = false;
		this.groupBox47.Text = "Lucky Party Number Event";
		this.label176.AutoSize = true;
		this.label176.Location = new System.Drawing.Point(220, 199);
		this.label176.Name = "label176";
		this.label176.Size = new System.Drawing.Size(10, 13);
		this.label176.TabIndex = 92;
		this.label176.Text = "-";
		this.PT_MAXVALUEBOX.Location = new System.Drawing.Point(236, 196);
		this.PT_MAXVALUEBOX.MaxLength = 10;
		this.PT_MAXVALUEBOX.Name = "PT_MAXVALUEBOX";
		this.PT_MAXVALUEBOX.Size = new System.Drawing.Size(53, 20);
		this.PT_MAXVALUEBOX.TabIndex = 93;
		this.PT_MAXVALUEBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.PT_MINVALUEBOX.Location = new System.Drawing.Point(161, 196);
		this.PT_MINVALUEBOX.MaxLength = 10;
		this.PT_MINVALUEBOX.Name = "PT_MINVALUEBOX";
		this.PT_MINVALUEBOX.Size = new System.Drawing.Size(53, 20);
		this.PT_MINVALUEBOX.TabIndex = 92;
		this.PT_MINVALUEBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label177.AutoSize = true;
		this.label177.Location = new System.Drawing.Point(20, 202);
		this.label177.Name = "label177";
		this.label177.Size = new System.Drawing.Size(102, 13);
		this.label177.TabIndex = 91;
		this.label177.Text = "Party NM MIN/MAX";
		this.label178.AutoSize = true;
		this.label178.Location = new System.Drawing.Point(20, 89);
		this.label178.Name = "label178";
		this.label178.Size = new System.Drawing.Size(39, 13);
		this.label178.TabIndex = 90;
		this.label178.Text = "Round";
		this.LPN_ITEMCOUNTBOX.Location = new System.Drawing.Point(240, 144);
		this.LPN_ITEMCOUNTBOX.MaxLength = 10;
		this.LPN_ITEMCOUNTBOX.Name = "LPN_ITEMCOUNTBOX";
		this.LPN_ITEMCOUNTBOX.Size = new System.Drawing.Size(54, 20);
		this.LPN_ITEMCOUNTBOX.TabIndex = 88;
		this.LPN_ITEMCOUNTBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label179.AutoSize = true;
		this.label179.Location = new System.Drawing.Point(20, 149);
		this.label179.Name = "label179";
		this.label179.Size = new System.Drawing.Size(98, 13);
		this.label179.TabIndex = 87;
		this.label179.Text = "Reward Item Count";
		this.LPN_ROUNDSBOX.Location = new System.Drawing.Point(240, 84);
		this.LPN_ROUNDSBOX.MaxLength = 10;
		this.LPN_ROUNDSBOX.Name = "LPN_ROUNDSBOX";
		this.LPN_ROUNDSBOX.Size = new System.Drawing.Size(54, 20);
		this.LPN_ROUNDSBOX.TabIndex = 89;
		this.LPN_ROUNDSBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.LPN_ITEMNAMEBOX.Location = new System.Drawing.Point(161, 170);
		this.LPN_ITEMNAMEBOX.MaxLength = 10;
		this.LPN_ITEMNAMEBOX.Name = "LPN_ITEMNAMEBOX";
		this.LPN_ITEMNAMEBOX.Size = new System.Drawing.Size(133, 20);
		this.LPN_ITEMNAMEBOX.TabIndex = 86;
		this.LPN_ITEMNAMEBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label180.AutoSize = true;
		this.label180.Location = new System.Drawing.Point(20, 175);
		this.label180.Name = "label180";
		this.label180.Size = new System.Drawing.Size(98, 13);
		this.label180.TabIndex = 85;
		this.label180.Text = "Reward Item Name";
		this.LPN_ITEMREWARDBOX.Location = new System.Drawing.Point(240, 116);
		this.LPN_ITEMREWARDBOX.MaxLength = 10;
		this.LPN_ITEMREWARDBOX.Name = "LPN_ITEMREWARDBOX";
		this.LPN_ITEMREWARDBOX.Size = new System.Drawing.Size(54, 20);
		this.LPN_ITEMREWARDBOX.TabIndex = 84;
		this.LPN_ITEMREWARDBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label181.AutoSize = true;
		this.label181.Location = new System.Drawing.Point(20, 121);
		this.label181.Name = "label181";
		this.label181.Size = new System.Drawing.Size(81, 13);
		this.label181.TabIndex = 83;
		this.label181.Text = "Reward Item ID";
		this.LPN_TIMETOWAITBOX.Location = new System.Drawing.Point(240, 50);
		this.LPN_TIMETOWAITBOX.MaxLength = 10;
		this.LPN_TIMETOWAITBOX.Name = "LPN_TIMETOWAITBOX";
		this.LPN_TIMETOWAITBOX.Size = new System.Drawing.Size(54, 20);
		this.LPN_TIMETOWAITBOX.TabIndex = 37;
		this.LPN_TIMETOWAITBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label233.AutoSize = true;
		this.label233.Location = new System.Drawing.Point(20, 55);
		this.label233.Name = "label233";
		this.label233.Size = new System.Drawing.Size(71, 13);
		this.label233.TabIndex = 1;
		this.label233.Text = "Time To Wait";
		this.LPN_ENABLEBOX.AutoSize = true;
		this.LPN_ENABLEBOX.Location = new System.Drawing.Point(20, 30);
		this.LPN_ENABLEBOX.Name = "LPN_ENABLEBOX";
		this.LPN_ENABLEBOX.Size = new System.Drawing.Size(189, 17);
		this.LPN_ENABLEBOX.TabIndex = 0;
		this.LPN_ENABLEBOX.Text = "Enable Lucky Party Number Event";
		this.LPN_ENABLEBOX.UseVisualStyleBackColor = true;
		this.tabPage10.BackColor = System.Drawing.Color.WhiteSmoke;
		this.tabPage10.Controls.Add(this.groupBox18);
		this.tabPage10.Controls.Add(this.groupBox53);
		this.tabPage10.Location = new System.Drawing.Point(4, 22);
		this.tabPage10.Name = "tabPage10";
		this.tabPage10.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage10.Size = new System.Drawing.Size(1033, 453);
		this.tabPage10.TabIndex = 9;
		this.tabPage10.Text = "LMS";
		this.groupBox18.Controls.Add(this.LMS_REQUIRELEVEL_NOTICEBOX);
		this.groupBox18.Controls.Add(this.label246);
		this.groupBox18.Controls.Add(this.LMS_FIGHTSTART_NOTICEBOX);
		this.groupBox18.Controls.Add(this.label245);
		this.groupBox18.Controls.Add(this.LMS_ELIMINATED_NOTICEBOX);
		this.groupBox18.Controls.Add(this.label236);
		this.groupBox18.Controls.Add(this.LMS_JOB_NOTICEBOX);
		this.groupBox18.Controls.Add(this.label237);
		this.groupBox18.Controls.Add(this.LMS_REGISTERSUCCESS_NOTICEBOX);
		this.groupBox18.Controls.Add(this.label239);
		this.groupBox18.Controls.Add(this.LMS_REGISTED_NOTICEBOX);
		this.groupBox18.Controls.Add(this.label244);
		this.groupBox18.Controls.Add(this.LMS_END_NOTICEBOX);
		this.groupBox18.Controls.Add(this.label182);
		this.groupBox18.Controls.Add(this.label226);
		this.groupBox18.Controls.Add(this.LMS_WIN_NOTICEBOX);
		this.groupBox18.Controls.Add(this.label227);
		this.groupBox18.Controls.Add(this.LMS_INFO2_NOTICEBOX);
		this.groupBox18.Controls.Add(this.label228);
		this.groupBox18.Controls.Add(this.LMS_CANCELEVENT_NOTICEBOX);
		this.groupBox18.Controls.Add(this.label229);
		this.groupBox18.Controls.Add(this.LMS_GATECLOSE_NOTICEBOX);
		this.groupBox18.Controls.Add(this.label230);
		this.groupBox18.Controls.Add(this.LMS_GATEOPEN_NOTICEBOX);
		this.groupBox18.Controls.Add(this.label231);
		this.groupBox18.Controls.Add(this.LMS_INFORM_NOTICEBOX);
		this.groupBox18.Controls.Add(this.label232);
		this.groupBox18.Controls.Add(this.LMS_REGISTERCLOSE_NOTICEBOX);
		this.groupBox18.Controls.Add(this.label234);
		this.groupBox18.Controls.Add(this.LMS_REGISTERTIME_NOTICEBOX);
		this.groupBox18.Controls.Add(this.label235);
		this.groupBox18.Controls.Add(this.LMS_START_NOTICEBOX);
		this.groupBox18.Location = new System.Drawing.Point(328, 6);
		this.groupBox18.Name = "groupBox18";
		this.groupBox18.Size = new System.Drawing.Size(699, 447);
		this.groupBox18.TabIndex = 24;
		this.groupBox18.TabStop = false;
		this.groupBox18.Text = "Notices";
		this.LMS_REQUIRELEVEL_NOTICEBOX.Location = new System.Drawing.Point(84, 339);
		this.LMS_REQUIRELEVEL_NOTICEBOX.Name = "LMS_REQUIRELEVEL_NOTICEBOX";
		this.LMS_REQUIRELEVEL_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.LMS_REQUIRELEVEL_NOTICEBOX.TabIndex = 188;
		this.label246.AutoSize = true;
		this.label246.Location = new System.Drawing.Point(22, 342);
		this.label246.Name = "label246";
		this.label246.Size = new System.Drawing.Size(56, 13);
		this.label246.TabIndex = 187;
		this.label246.Text = "Reg Level";
		this.LMS_FIGHTSTART_NOTICEBOX.Location = new System.Drawing.Point(85, 416);
		this.LMS_FIGHTSTART_NOTICEBOX.Name = "LMS_FIGHTSTART_NOTICEBOX";
		this.LMS_FIGHTSTART_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.LMS_FIGHTSTART_NOTICEBOX.TabIndex = 186;
		this.label245.AutoSize = true;
		this.label245.Location = new System.Drawing.Point(24, 419);
		this.label245.Name = "label245";
		this.label245.Size = new System.Drawing.Size(55, 13);
		this.label245.TabIndex = 185;
		this.label245.Text = "Fight Start";
		this.LMS_ELIMINATED_NOTICEBOX.Location = new System.Drawing.Point(85, 390);
		this.LMS_ELIMINATED_NOTICEBOX.Name = "LMS_ELIMINATED_NOTICEBOX";
		this.LMS_ELIMINATED_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.LMS_ELIMINATED_NOTICEBOX.TabIndex = 184;
		this.label236.AutoSize = true;
		this.label236.Location = new System.Drawing.Point(24, 393);
		this.label236.Name = "label236";
		this.label236.Size = new System.Drawing.Size(55, 13);
		this.label236.TabIndex = 183;
		this.label236.Text = "Eliminated";
		this.LMS_JOB_NOTICEBOX.Location = new System.Drawing.Point(85, 365);
		this.LMS_JOB_NOTICEBOX.Name = "LMS_JOB_NOTICEBOX";
		this.LMS_JOB_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.LMS_JOB_NOTICEBOX.TabIndex = 182;
		this.label237.AutoSize = true;
		this.label237.Location = new System.Drawing.Point(25, 368);
		this.label237.Name = "label237";
		this.label237.Size = new System.Drawing.Size(54, 13);
		this.label237.TabIndex = 181;
		this.label237.Text = "Job Block";
		this.LMS_REGISTERSUCCESS_NOTICEBOX.Location = new System.Drawing.Point(84, 308);
		this.LMS_REGISTERSUCCESS_NOTICEBOX.Name = "LMS_REGISTERSUCCESS_NOTICEBOX";
		this.LMS_REGISTERSUCCESS_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.LMS_REGISTERSUCCESS_NOTICEBOX.TabIndex = 180;
		this.label239.AutoSize = true;
		this.label239.Location = new System.Drawing.Point(13, 285);
		this.label239.Name = "label239";
		this.label239.Size = new System.Drawing.Size(65, 13);
		this.label239.TabIndex = 179;
		this.label239.Text = "Already Reg";
		this.LMS_REGISTED_NOTICEBOX.Location = new System.Drawing.Point(84, 282);
		this.LMS_REGISTED_NOTICEBOX.Name = "LMS_REGISTED_NOTICEBOX";
		this.LMS_REGISTED_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.LMS_REGISTED_NOTICEBOX.TabIndex = 178;
		this.label244.AutoSize = true;
		this.label244.Location = new System.Drawing.Point(12, 311);
		this.label244.Name = "label244";
		this.label244.Size = new System.Drawing.Size(66, 13);
		this.label244.TabIndex = 177;
		this.label244.Text = "Succes Reg";
		this.LMS_END_NOTICEBOX.Location = new System.Drawing.Point(84, 253);
		this.LMS_END_NOTICEBOX.Name = "LMS_END_NOTICEBOX";
		this.LMS_END_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.LMS_END_NOTICEBOX.TabIndex = 176;
		this.label182.AutoSize = true;
		this.label182.Location = new System.Drawing.Point(52, 257);
		this.label182.Name = "label182";
		this.label182.Size = new System.Drawing.Size(26, 13);
		this.label182.TabIndex = 175;
		this.label182.Text = "End";
		this.label226.AutoSize = true;
		this.label226.Location = new System.Drawing.Point(19, 127);
		this.label226.Name = "label226";
		this.label226.Size = new System.Drawing.Size(59, 13);
		this.label226.TabIndex = 174;
		this.label226.Text = "Gate Open";
		this.LMS_WIN_NOTICEBOX.Location = new System.Drawing.Point(84, 228);
		this.LMS_WIN_NOTICEBOX.Name = "LMS_WIN_NOTICEBOX";
		this.LMS_WIN_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.LMS_WIN_NOTICEBOX.TabIndex = 173;
		this.label227.AutoSize = true;
		this.label227.Location = new System.Drawing.Point(52, 231);
		this.label227.Name = "label227";
		this.label227.Size = new System.Drawing.Size(26, 13);
		this.label227.TabIndex = 172;
		this.label227.Text = "Win";
		this.LMS_INFO2_NOTICEBOX.Location = new System.Drawing.Point(84, 203);
		this.LMS_INFO2_NOTICEBOX.Name = "LMS_INFO2_NOTICEBOX";
		this.LMS_INFO2_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.LMS_INFO2_NOTICEBOX.TabIndex = 171;
		this.label228.AutoSize = true;
		this.label228.Location = new System.Drawing.Point(24, 180);
		this.label228.Name = "label228";
		this.label228.Size = new System.Drawing.Size(54, 13);
		this.label228.TabIndex = 170;
		this.label228.Text = "Cancelled";
		this.LMS_CANCELEVENT_NOTICEBOX.Location = new System.Drawing.Point(84, 177);
		this.LMS_CANCELEVENT_NOTICEBOX.Name = "LMS_CANCELEVENT_NOTICEBOX";
		this.LMS_CANCELEVENT_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.LMS_CANCELEVENT_NOTICEBOX.TabIndex = 169;
		this.label229.AutoSize = true;
		this.label229.Location = new System.Drawing.Point(44, 206);
		this.label229.Name = "label229";
		this.label229.Size = new System.Drawing.Size(34, 13);
		this.label229.TabIndex = 168;
		this.label229.Text = "Info 2";
		this.LMS_GATECLOSE_NOTICEBOX.Location = new System.Drawing.Point(84, 150);
		this.LMS_GATECLOSE_NOTICEBOX.Name = "LMS_GATECLOSE_NOTICEBOX";
		this.LMS_GATECLOSE_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.LMS_GATECLOSE_NOTICEBOX.TabIndex = 166;
		this.label230.AutoSize = true;
		this.label230.Location = new System.Drawing.Point(53, 101);
		this.label230.Name = "label230";
		this.label230.Size = new System.Drawing.Size(25, 13);
		this.label230.TabIndex = 165;
		this.label230.Text = "Info";
		this.LMS_GATEOPEN_NOTICEBOX.Location = new System.Drawing.Point(84, 124);
		this.LMS_GATEOPEN_NOTICEBOX.Name = "LMS_GATEOPEN_NOTICEBOX";
		this.LMS_GATEOPEN_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.LMS_GATEOPEN_NOTICEBOX.TabIndex = 164;
		this.label231.AutoSize = true;
		this.label231.Location = new System.Drawing.Point(19, 153);
		this.label231.Name = "label231";
		this.label231.Size = new System.Drawing.Size(59, 13);
		this.label231.TabIndex = 163;
		this.label231.Text = "Gate Close";
		this.LMS_INFORM_NOTICEBOX.Location = new System.Drawing.Point(84, 98);
		this.LMS_INFORM_NOTICEBOX.Name = "LMS_INFORM_NOTICEBOX";
		this.LMS_INFORM_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.LMS_INFORM_NOTICEBOX.TabIndex = 162;
		this.label232.AutoSize = true;
		this.label232.Location = new System.Drawing.Point(22, 75);
		this.label232.Name = "label232";
		this.label232.Size = new System.Drawing.Size(56, 13);
		this.label232.TabIndex = 161;
		this.label232.Text = "Reg Close";
		this.LMS_REGISTERCLOSE_NOTICEBOX.Location = new System.Drawing.Point(84, 72);
		this.LMS_REGISTERCLOSE_NOTICEBOX.Name = "LMS_REGISTERCLOSE_NOTICEBOX";
		this.LMS_REGISTERCLOSE_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.LMS_REGISTERCLOSE_NOTICEBOX.TabIndex = 160;
		this.label234.AutoSize = true;
		this.label234.Location = new System.Drawing.Point(25, 48);
		this.label234.Name = "label234";
		this.label234.Size = new System.Drawing.Size(53, 13);
		this.label234.TabIndex = 159;
		this.label234.Text = "Reg Time";
		this.LMS_REGISTERTIME_NOTICEBOX.Location = new System.Drawing.Point(84, 45);
		this.LMS_REGISTERTIME_NOTICEBOX.Name = "LMS_REGISTERTIME_NOTICEBOX";
		this.LMS_REGISTERTIME_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.LMS_REGISTERTIME_NOTICEBOX.TabIndex = 158;
		this.label235.AutoSize = true;
		this.label235.Location = new System.Drawing.Point(15, 22);
		this.label235.Name = "label235";
		this.label235.Size = new System.Drawing.Size(63, 13);
		this.label235.TabIndex = 149;
		this.label235.Text = "Start Notice";
		this.LMS_START_NOTICEBOX.Location = new System.Drawing.Point(84, 19);
		this.LMS_START_NOTICEBOX.Name = "LMS_START_NOTICEBOX";
		this.LMS_START_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.LMS_START_NOTICEBOX.TabIndex = 148;
		this.groupBox53.Controls.Add(this.LMS_REGIONIDBOX2);
		this.groupBox53.Controls.Add(this.label281);
		this.groupBox53.Controls.Add(this.LMS_PCLIMITBOX);
		this.groupBox53.Controls.Add(this.label169);
		this.groupBox53.Controls.Add(this.LMS_GATEWAIT_TIMEBOX);
		this.groupBox53.Controls.Add(this.label170);
		this.groupBox53.Controls.Add(this.LMS_REGIONIDBOX);
		this.groupBox53.Controls.Add(this.label171);
		this.groupBox53.Controls.Add(this.LMS_GATEIDBOX);
		this.groupBox53.Controls.Add(this.label172);
		this.groupBox53.Controls.Add(this.LMS_ITEMCOUNTBOX);
		this.groupBox53.Controls.Add(this.label173);
		this.groupBox53.Controls.Add(this.LMS_ITEMNAMEBOX);
		this.groupBox53.Controls.Add(this.label174);
		this.groupBox53.Controls.Add(this.LMS_ITEMIDBOX);
		this.groupBox53.Controls.Add(this.label175);
		this.groupBox53.Controls.Add(this.LMS_REQUIRELEVELBOX);
		this.groupBox53.Controls.Add(this.label269);
		this.groupBox53.Controls.Add(this.LMS_REGISTERTIMEBOX);
		this.groupBox53.Controls.Add(this.LMS_MATCHTIMEBOX);
		this.groupBox53.Controls.Add(this.label276);
		this.groupBox53.Controls.Add(this.label277);
		this.groupBox53.Controls.Add(this.LMS_ENABLEBOX);
		this.groupBox53.Location = new System.Drawing.Point(8, 6);
		this.groupBox53.Name = "groupBox53";
		this.groupBox53.Size = new System.Drawing.Size(314, 417);
		this.groupBox53.TabIndex = 23;
		this.groupBox53.TabStop = false;
		this.groupBox53.Text = "Last Man Standing";
		this.LMS_REGIONIDBOX2.Location = new System.Drawing.Point(237, 186);
		this.LMS_REGIONIDBOX2.MaxLength = 33333;
		this.LMS_REGIONIDBOX2.Name = "LMS_REGIONIDBOX2";
		this.LMS_REGIONIDBOX2.Size = new System.Drawing.Size(54, 20);
		this.LMS_REGIONIDBOX2.TabIndex = 104;
		this.LMS_REGIONIDBOX2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label281.AutoSize = true;
		this.label281.Location = new System.Drawing.Point(17, 191);
		this.label281.Name = "label281";
		this.label281.Size = new System.Drawing.Size(61, 13);
		this.label281.TabIndex = 103;
		this.label281.Text = "Region ID2";
		this.LMS_PCLIMITBOX.Location = new System.Drawing.Point(237, 242);
		this.LMS_PCLIMITBOX.MaxLength = 33333;
		this.LMS_PCLIMITBOX.Name = "LMS_PCLIMITBOX";
		this.LMS_PCLIMITBOX.Size = new System.Drawing.Size(54, 20);
		this.LMS_PCLIMITBOX.TabIndex = 102;
		this.LMS_PCLIMITBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label169.AutoSize = true;
		this.label169.Location = new System.Drawing.Point(17, 247);
		this.label169.Name = "label169";
		this.label169.Size = new System.Drawing.Size(45, 13);
		this.label169.TabIndex = 101;
		this.label169.Text = "PC Limit";
		this.LMS_GATEWAIT_TIMEBOX.Location = new System.Drawing.Point(237, 214);
		this.LMS_GATEWAIT_TIMEBOX.MaxLength = 33333;
		this.LMS_GATEWAIT_TIMEBOX.Name = "LMS_GATEWAIT_TIMEBOX";
		this.LMS_GATEWAIT_TIMEBOX.Size = new System.Drawing.Size(54, 20);
		this.LMS_GATEWAIT_TIMEBOX.TabIndex = 100;
		this.LMS_GATEWAIT_TIMEBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label170.AutoSize = true;
		this.label170.Location = new System.Drawing.Point(17, 219);
		this.label170.Name = "label170";
		this.label170.Size = new System.Drawing.Size(81, 13);
		this.label170.TabIndex = 99;
		this.label170.Text = "Gate Wait Time";
		this.LMS_REGIONIDBOX.Location = new System.Drawing.Point(237, 157);
		this.LMS_REGIONIDBOX.MaxLength = 33333;
		this.LMS_REGIONIDBOX.Name = "LMS_REGIONIDBOX";
		this.LMS_REGIONIDBOX.Size = new System.Drawing.Size(54, 20);
		this.LMS_REGIONIDBOX.TabIndex = 98;
		this.LMS_REGIONIDBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label171.AutoSize = true;
		this.label171.Location = new System.Drawing.Point(17, 162);
		this.label171.Name = "label171";
		this.label171.Size = new System.Drawing.Size(55, 13);
		this.label171.TabIndex = 97;
		this.label171.Text = "Region ID";
		this.LMS_GATEIDBOX.Location = new System.Drawing.Point(237, 127);
		this.LMS_GATEIDBOX.MaxLength = 3333;
		this.LMS_GATEIDBOX.Name = "LMS_GATEIDBOX";
		this.LMS_GATEIDBOX.Size = new System.Drawing.Size(54, 20);
		this.LMS_GATEIDBOX.TabIndex = 96;
		this.LMS_GATEIDBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label172.AutoSize = true;
		this.label172.Location = new System.Drawing.Point(17, 132);
		this.label172.Name = "label172";
		this.label172.Size = new System.Drawing.Size(60, 13);
		this.label172.TabIndex = 95;
		this.label172.Text = "Teleport ID";
		this.LMS_ITEMCOUNTBOX.Location = new System.Drawing.Point(237, 302);
		this.LMS_ITEMCOUNTBOX.MaxLength = 10;
		this.LMS_ITEMCOUNTBOX.Name = "LMS_ITEMCOUNTBOX";
		this.LMS_ITEMCOUNTBOX.Size = new System.Drawing.Size(54, 20);
		this.LMS_ITEMCOUNTBOX.TabIndex = 94;
		this.LMS_ITEMCOUNTBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label173.AutoSize = true;
		this.label173.Location = new System.Drawing.Point(17, 307);
		this.label173.Name = "label173";
		this.label173.Size = new System.Drawing.Size(98, 13);
		this.label173.TabIndex = 93;
		this.label173.Text = "Reward Item Count";
		this.LMS_ITEMNAMEBOX.Location = new System.Drawing.Point(158, 328);
		this.LMS_ITEMNAMEBOX.MaxLength = 10;
		this.LMS_ITEMNAMEBOX.Name = "LMS_ITEMNAMEBOX";
		this.LMS_ITEMNAMEBOX.Size = new System.Drawing.Size(133, 20);
		this.LMS_ITEMNAMEBOX.TabIndex = 92;
		this.LMS_ITEMNAMEBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label174.AutoSize = true;
		this.label174.Location = new System.Drawing.Point(17, 333);
		this.label174.Name = "label174";
		this.label174.Size = new System.Drawing.Size(98, 13);
		this.label174.TabIndex = 91;
		this.label174.Text = "Reward Item Name";
		this.LMS_ITEMIDBOX.Location = new System.Drawing.Point(237, 274);
		this.LMS_ITEMIDBOX.MaxLength = 10;
		this.LMS_ITEMIDBOX.Name = "LMS_ITEMIDBOX";
		this.LMS_ITEMIDBOX.Size = new System.Drawing.Size(54, 20);
		this.LMS_ITEMIDBOX.TabIndex = 90;
		this.LMS_ITEMIDBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label175.AutoSize = true;
		this.label175.Location = new System.Drawing.Point(17, 279);
		this.label175.Name = "label175";
		this.label175.Size = new System.Drawing.Size(81, 13);
		this.label175.TabIndex = 89;
		this.label175.Text = "Reward Item ID";
		this.LMS_REQUIRELEVELBOX.Location = new System.Drawing.Point(237, 102);
		this.LMS_REQUIRELEVELBOX.MaxLength = 3;
		this.LMS_REQUIRELEVELBOX.Name = "LMS_REQUIRELEVELBOX";
		this.LMS_REQUIRELEVELBOX.Size = new System.Drawing.Size(54, 20);
		this.LMS_REQUIRELEVELBOX.TabIndex = 51;
		this.LMS_REQUIRELEVELBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label269.AutoSize = true;
		this.label269.Location = new System.Drawing.Point(17, 107);
		this.label269.Name = "label269";
		this.label269.Size = new System.Drawing.Size(73, 13);
		this.label269.TabIndex = 49;
		this.label269.Text = "Require Level";
		this.LMS_REGISTERTIMEBOX.Location = new System.Drawing.Point(237, 75);
		this.LMS_REGISTERTIMEBOX.MaxLength = 10;
		this.LMS_REGISTERTIMEBOX.Name = "LMS_REGISTERTIMEBOX";
		this.LMS_REGISTERTIMEBOX.Size = new System.Drawing.Size(54, 20);
		this.LMS_REGISTERTIMEBOX.TabIndex = 38;
		this.LMS_REGISTERTIMEBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.LMS_MATCHTIMEBOX.Location = new System.Drawing.Point(237, 50);
		this.LMS_MATCHTIMEBOX.MaxLength = 10;
		this.LMS_MATCHTIMEBOX.Name = "LMS_MATCHTIMEBOX";
		this.LMS_MATCHTIMEBOX.Size = new System.Drawing.Size(54, 20);
		this.LMS_MATCHTIMEBOX.TabIndex = 37;
		this.LMS_MATCHTIMEBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label276.AutoSize = true;
		this.label276.Location = new System.Drawing.Point(17, 80);
		this.label276.Name = "label276";
		this.label276.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.label276.Size = new System.Drawing.Size(90, 13);
		this.label276.TabIndex = 2;
		this.label276.Text = "Time For Register";
		this.label277.AutoSize = true;
		this.label277.Location = new System.Drawing.Point(17, 55);
		this.label277.Name = "label277";
		this.label277.Size = new System.Drawing.Size(63, 13);
		this.label277.TabIndex = 1;
		this.label277.Text = "Match Time";
		this.LMS_ENABLEBOX.AutoSize = true;
		this.LMS_ENABLEBOX.Location = new System.Drawing.Point(20, 30);
		this.LMS_ENABLEBOX.Name = "LMS_ENABLEBOX";
		this.LMS_ENABLEBOX.Size = new System.Drawing.Size(84, 17);
		this.LMS_ENABLEBOX.TabIndex = 0;
		this.LMS_ENABLEBOX.Text = "Enable LMS";
		this.LMS_ENABLEBOX.UseVisualStyleBackColor = true;
		this.tabPage11.BackColor = System.Drawing.Color.WhiteSmoke;
		this.tabPage11.Controls.Add(this.groupBox19);
		this.tabPage11.Controls.Add(this.groupBox28);
		this.tabPage11.Location = new System.Drawing.Point(4, 22);
		this.tabPage11.Name = "tabPage11";
		this.tabPage11.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage11.Size = new System.Drawing.Size(1033, 453);
		this.tabPage11.TabIndex = 10;
		this.tabPage11.Text = "Survival Arena";
		this.groupBox19.Controls.Add(this.SURV_REQUIRELEVEL_NOTICEBOX);
		this.groupBox19.Controls.Add(this.label247);
		this.groupBox19.Controls.Add(this.SURV_FIGHTSTART_NOTICEBOX);
		this.groupBox19.Controls.Add(this.label248);
		this.groupBox19.Controls.Add(this.SURV_ELIMINATED_NOTICEBOX);
		this.groupBox19.Controls.Add(this.label249);
		this.groupBox19.Controls.Add(this.SURV_JOB_NOTICEBOX);
		this.groupBox19.Controls.Add(this.label250);
		this.groupBox19.Controls.Add(this.SURV_REGISTERSUCCESS_NOTICEBOX);
		this.groupBox19.Controls.Add(this.label251);
		this.groupBox19.Controls.Add(this.SURV_REGISTED_NOTICEBOX);
		this.groupBox19.Controls.Add(this.label252);
		this.groupBox19.Controls.Add(this.SURV_END_NOTICEBOX);
		this.groupBox19.Controls.Add(this.label253);
		this.groupBox19.Controls.Add(this.label254);
		this.groupBox19.Controls.Add(this.SURV_WIN_NOTICEBOX);
		this.groupBox19.Controls.Add(this.label255);
		this.groupBox19.Controls.Add(this.SURV_INFO2_NOTICEBOX);
		this.groupBox19.Controls.Add(this.label256);
		this.groupBox19.Controls.Add(this.SURV_CANCELEVENT_NOTICEBOX);
		this.groupBox19.Controls.Add(this.label257);
		this.groupBox19.Controls.Add(this.SURV_GATECLOSE_NOTICEBOX);
		this.groupBox19.Controls.Add(this.label258);
		this.groupBox19.Controls.Add(this.SURV_GATEOPEN_NOTICEBOX);
		this.groupBox19.Controls.Add(this.label259);
		this.groupBox19.Controls.Add(this.SURV_INFORM_NOTICEBOX);
		this.groupBox19.Controls.Add(this.label260);
		this.groupBox19.Controls.Add(this.SURV_REGISTERCLOSE_NOTICEBOX);
		this.groupBox19.Controls.Add(this.label261);
		this.groupBox19.Controls.Add(this.SURV_REGISTERTIME_NOTICEBOX);
		this.groupBox19.Controls.Add(this.label262);
		this.groupBox19.Controls.Add(this.SURV_START_NOTICEBOX);
		this.groupBox19.Location = new System.Drawing.Point(331, 6);
		this.groupBox19.Name = "groupBox19";
		this.groupBox19.Size = new System.Drawing.Size(699, 447);
		this.groupBox19.TabIndex = 26;
		this.groupBox19.TabStop = false;
		this.groupBox19.Text = "Notices";
		this.SURV_REQUIRELEVEL_NOTICEBOX.Location = new System.Drawing.Point(84, 339);
		this.SURV_REQUIRELEVEL_NOTICEBOX.Name = "SURV_REQUIRELEVEL_NOTICEBOX";
		this.SURV_REQUIRELEVEL_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.SURV_REQUIRELEVEL_NOTICEBOX.TabIndex = 188;
		this.label247.AutoSize = true;
		this.label247.Location = new System.Drawing.Point(22, 342);
		this.label247.Name = "label247";
		this.label247.Size = new System.Drawing.Size(56, 13);
		this.label247.TabIndex = 187;
		this.label247.Text = "Reg Level";
		this.SURV_FIGHTSTART_NOTICEBOX.Location = new System.Drawing.Point(85, 416);
		this.SURV_FIGHTSTART_NOTICEBOX.Name = "SURV_FIGHTSTART_NOTICEBOX";
		this.SURV_FIGHTSTART_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.SURV_FIGHTSTART_NOTICEBOX.TabIndex = 186;
		this.label248.AutoSize = true;
		this.label248.Location = new System.Drawing.Point(24, 419);
		this.label248.Name = "label248";
		this.label248.Size = new System.Drawing.Size(55, 13);
		this.label248.TabIndex = 185;
		this.label248.Text = "Fight Start";
		this.SURV_ELIMINATED_NOTICEBOX.Location = new System.Drawing.Point(85, 390);
		this.SURV_ELIMINATED_NOTICEBOX.Name = "SURV_ELIMINATED_NOTICEBOX";
		this.SURV_ELIMINATED_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.SURV_ELIMINATED_NOTICEBOX.TabIndex = 184;
		this.label249.AutoSize = true;
		this.label249.Location = new System.Drawing.Point(24, 393);
		this.label249.Name = "label249";
		this.label249.Size = new System.Drawing.Size(55, 13);
		this.label249.TabIndex = 183;
		this.label249.Text = "Eliminated";
		this.SURV_JOB_NOTICEBOX.Location = new System.Drawing.Point(85, 365);
		this.SURV_JOB_NOTICEBOX.Name = "SURV_JOB_NOTICEBOX";
		this.SURV_JOB_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.SURV_JOB_NOTICEBOX.TabIndex = 182;
		this.label250.AutoSize = true;
		this.label250.Location = new System.Drawing.Point(25, 368);
		this.label250.Name = "label250";
		this.label250.Size = new System.Drawing.Size(54, 13);
		this.label250.TabIndex = 181;
		this.label250.Text = "Job Block";
		this.SURV_REGISTERSUCCESS_NOTICEBOX.Location = new System.Drawing.Point(84, 308);
		this.SURV_REGISTERSUCCESS_NOTICEBOX.Name = "SURV_REGISTERSUCCESS_NOTICEBOX";
		this.SURV_REGISTERSUCCESS_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.SURV_REGISTERSUCCESS_NOTICEBOX.TabIndex = 180;
		this.label251.AutoSize = true;
		this.label251.Location = new System.Drawing.Point(13, 285);
		this.label251.Name = "label251";
		this.label251.Size = new System.Drawing.Size(65, 13);
		this.label251.TabIndex = 179;
		this.label251.Text = "Already Reg";
		this.SURV_REGISTED_NOTICEBOX.Location = new System.Drawing.Point(84, 282);
		this.SURV_REGISTED_NOTICEBOX.Name = "SURV_REGISTED_NOTICEBOX";
		this.SURV_REGISTED_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.SURV_REGISTED_NOTICEBOX.TabIndex = 178;
		this.label252.AutoSize = true;
		this.label252.Location = new System.Drawing.Point(12, 311);
		this.label252.Name = "label252";
		this.label252.Size = new System.Drawing.Size(66, 13);
		this.label252.TabIndex = 177;
		this.label252.Text = "Succes Reg";
		this.SURV_END_NOTICEBOX.Location = new System.Drawing.Point(84, 253);
		this.SURV_END_NOTICEBOX.Name = "SURV_END_NOTICEBOX";
		this.SURV_END_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.SURV_END_NOTICEBOX.TabIndex = 176;
		this.label253.AutoSize = true;
		this.label253.Location = new System.Drawing.Point(52, 257);
		this.label253.Name = "label253";
		this.label253.Size = new System.Drawing.Size(26, 13);
		this.label253.TabIndex = 175;
		this.label253.Text = "End";
		this.label254.AutoSize = true;
		this.label254.Location = new System.Drawing.Point(19, 127);
		this.label254.Name = "label254";
		this.label254.Size = new System.Drawing.Size(59, 13);
		this.label254.TabIndex = 174;
		this.label254.Text = "Gate Open";
		this.SURV_WIN_NOTICEBOX.Location = new System.Drawing.Point(84, 228);
		this.SURV_WIN_NOTICEBOX.Name = "SURV_WIN_NOTICEBOX";
		this.SURV_WIN_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.SURV_WIN_NOTICEBOX.TabIndex = 173;
		this.label255.AutoSize = true;
		this.label255.Location = new System.Drawing.Point(52, 231);
		this.label255.Name = "label255";
		this.label255.Size = new System.Drawing.Size(26, 13);
		this.label255.TabIndex = 172;
		this.label255.Text = "Win";
		this.SURV_INFO2_NOTICEBOX.Location = new System.Drawing.Point(84, 203);
		this.SURV_INFO2_NOTICEBOX.Name = "SURV_INFO2_NOTICEBOX";
		this.SURV_INFO2_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.SURV_INFO2_NOTICEBOX.TabIndex = 171;
		this.label256.AutoSize = true;
		this.label256.Location = new System.Drawing.Point(24, 180);
		this.label256.Name = "label256";
		this.label256.Size = new System.Drawing.Size(54, 13);
		this.label256.TabIndex = 170;
		this.label256.Text = "Cancelled";
		this.SURV_CANCELEVENT_NOTICEBOX.Location = new System.Drawing.Point(84, 177);
		this.SURV_CANCELEVENT_NOTICEBOX.Name = "SURV_CANCELEVENT_NOTICEBOX";
		this.SURV_CANCELEVENT_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.SURV_CANCELEVENT_NOTICEBOX.TabIndex = 169;
		this.label257.AutoSize = true;
		this.label257.Location = new System.Drawing.Point(44, 206);
		this.label257.Name = "label257";
		this.label257.Size = new System.Drawing.Size(34, 13);
		this.label257.TabIndex = 168;
		this.label257.Text = "Info 2";
		this.SURV_GATECLOSE_NOTICEBOX.Location = new System.Drawing.Point(84, 150);
		this.SURV_GATECLOSE_NOTICEBOX.Name = "SURV_GATECLOSE_NOTICEBOX";
		this.SURV_GATECLOSE_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.SURV_GATECLOSE_NOTICEBOX.TabIndex = 166;
		this.label258.AutoSize = true;
		this.label258.Location = new System.Drawing.Point(53, 101);
		this.label258.Name = "label258";
		this.label258.Size = new System.Drawing.Size(25, 13);
		this.label258.TabIndex = 165;
		this.label258.Text = "Info";
		this.SURV_GATEOPEN_NOTICEBOX.Location = new System.Drawing.Point(84, 124);
		this.SURV_GATEOPEN_NOTICEBOX.Name = "SURV_GATEOPEN_NOTICEBOX";
		this.SURV_GATEOPEN_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.SURV_GATEOPEN_NOTICEBOX.TabIndex = 164;
		this.label259.AutoSize = true;
		this.label259.Location = new System.Drawing.Point(19, 153);
		this.label259.Name = "label259";
		this.label259.Size = new System.Drawing.Size(59, 13);
		this.label259.TabIndex = 163;
		this.label259.Text = "Gate Close";
		this.SURV_INFORM_NOTICEBOX.Location = new System.Drawing.Point(84, 98);
		this.SURV_INFORM_NOTICEBOX.Name = "SURV_INFORM_NOTICEBOX";
		this.SURV_INFORM_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.SURV_INFORM_NOTICEBOX.TabIndex = 162;
		this.label260.AutoSize = true;
		this.label260.Location = new System.Drawing.Point(22, 75);
		this.label260.Name = "label260";
		this.label260.Size = new System.Drawing.Size(56, 13);
		this.label260.TabIndex = 161;
		this.label260.Text = "Reg Close";
		this.SURV_REGISTERCLOSE_NOTICEBOX.Location = new System.Drawing.Point(84, 72);
		this.SURV_REGISTERCLOSE_NOTICEBOX.Name = "SURV_REGISTERCLOSE_NOTICEBOX";
		this.SURV_REGISTERCLOSE_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.SURV_REGISTERCLOSE_NOTICEBOX.TabIndex = 160;
		this.label261.AutoSize = true;
		this.label261.Location = new System.Drawing.Point(25, 48);
		this.label261.Name = "label261";
		this.label261.Size = new System.Drawing.Size(53, 13);
		this.label261.TabIndex = 159;
		this.label261.Text = "Reg Time";
		this.SURV_REGISTERTIME_NOTICEBOX.Location = new System.Drawing.Point(84, 45);
		this.SURV_REGISTERTIME_NOTICEBOX.Name = "SURV_REGISTERTIME_NOTICEBOX";
		this.SURV_REGISTERTIME_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.SURV_REGISTERTIME_NOTICEBOX.TabIndex = 158;
		this.label262.AutoSize = true;
		this.label262.Location = new System.Drawing.Point(15, 22);
		this.label262.Name = "label262";
		this.label262.Size = new System.Drawing.Size(63, 13);
		this.label262.TabIndex = 149;
		this.label262.Text = "Start Notice";
		this.SURV_START_NOTICEBOX.Location = new System.Drawing.Point(84, 19);
		this.SURV_START_NOTICEBOX.Name = "SURV_START_NOTICEBOX";
		this.SURV_START_NOTICEBOX.Size = new System.Drawing.Size(609, 20);
		this.SURV_START_NOTICEBOX.TabIndex = 148;
		this.groupBox28.Controls.Add(this.SURV_REGIONIDBOX2);
		this.groupBox28.Controls.Add(this.label282);
		this.groupBox28.Controls.Add(this.SURV_GATEWAIT_TIMEBOX);
		this.groupBox28.Controls.Add(this.label160);
		this.groupBox28.Controls.Add(this.SURV_REGIONIDBOX);
		this.groupBox28.Controls.Add(this.label161);
		this.groupBox28.Controls.Add(this.SURV_GATEIDBOX);
		this.groupBox28.Controls.Add(this.label162);
		this.groupBox28.Controls.Add(this.SURV_ITEMCOUNTBOX);
		this.groupBox28.Controls.Add(this.label163);
		this.groupBox28.Controls.Add(this.SURV_ITEMNAMEBOX);
		this.groupBox28.Controls.Add(this.label164);
		this.groupBox28.Controls.Add(this.SURV_ITEMIDBOX);
		this.groupBox28.Controls.Add(this.label165);
		this.groupBox28.Controls.Add(this.SURV_REQUIRELEVELBOX);
		this.groupBox28.Controls.Add(this.label166);
		this.groupBox28.Controls.Add(this.SURV_REGISTERTIMEBOX);
		this.groupBox28.Controls.Add(this.SURV_MATCHTIMEBOX);
		this.groupBox28.Controls.Add(this.label167);
		this.groupBox28.Controls.Add(this.label168);
		this.groupBox28.Controls.Add(this.SURV_ENABLEBOX);
		this.groupBox28.Location = new System.Drawing.Point(8, 6);
		this.groupBox28.Name = "groupBox28";
		this.groupBox28.Size = new System.Drawing.Size(314, 417);
		this.groupBox28.TabIndex = 25;
		this.groupBox28.TabStop = false;
		this.groupBox28.Text = "Survival Arena";
		this.SURV_REGIONIDBOX2.Location = new System.Drawing.Point(237, 186);
		this.SURV_REGIONIDBOX2.MaxLength = 33333;
		this.SURV_REGIONIDBOX2.Name = "SURV_REGIONIDBOX2";
		this.SURV_REGIONIDBOX2.Size = new System.Drawing.Size(54, 20);
		this.SURV_REGIONIDBOX2.TabIndex = 102;
		this.SURV_REGIONIDBOX2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label282.AutoSize = true;
		this.label282.Location = new System.Drawing.Point(17, 191);
		this.label282.Name = "label282";
		this.label282.Size = new System.Drawing.Size(61, 13);
		this.label282.TabIndex = 101;
		this.label282.Text = "Region ID2";
		this.SURV_GATEWAIT_TIMEBOX.Location = new System.Drawing.Point(237, 214);
		this.SURV_GATEWAIT_TIMEBOX.MaxLength = 33333;
		this.SURV_GATEWAIT_TIMEBOX.Name = "SURV_GATEWAIT_TIMEBOX";
		this.SURV_GATEWAIT_TIMEBOX.Size = new System.Drawing.Size(54, 20);
		this.SURV_GATEWAIT_TIMEBOX.TabIndex = 100;
		this.SURV_GATEWAIT_TIMEBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label160.AutoSize = true;
		this.label160.Location = new System.Drawing.Point(17, 219);
		this.label160.Name = "label160";
		this.label160.Size = new System.Drawing.Size(81, 13);
		this.label160.TabIndex = 99;
		this.label160.Text = "Gate Wait Time";
		this.SURV_REGIONIDBOX.Location = new System.Drawing.Point(237, 157);
		this.SURV_REGIONIDBOX.MaxLength = 33333;
		this.SURV_REGIONIDBOX.Name = "SURV_REGIONIDBOX";
		this.SURV_REGIONIDBOX.Size = new System.Drawing.Size(54, 20);
		this.SURV_REGIONIDBOX.TabIndex = 98;
		this.SURV_REGIONIDBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label161.AutoSize = true;
		this.label161.Location = new System.Drawing.Point(17, 162);
		this.label161.Name = "label161";
		this.label161.Size = new System.Drawing.Size(55, 13);
		this.label161.TabIndex = 97;
		this.label161.Text = "Region ID";
		this.SURV_GATEIDBOX.Location = new System.Drawing.Point(237, 127);
		this.SURV_GATEIDBOX.MaxLength = 3333;
		this.SURV_GATEIDBOX.Name = "SURV_GATEIDBOX";
		this.SURV_GATEIDBOX.Size = new System.Drawing.Size(54, 20);
		this.SURV_GATEIDBOX.TabIndex = 96;
		this.SURV_GATEIDBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label162.AutoSize = true;
		this.label162.Location = new System.Drawing.Point(17, 132);
		this.label162.Name = "label162";
		this.label162.Size = new System.Drawing.Size(60, 13);
		this.label162.TabIndex = 95;
		this.label162.Text = "Teleport ID";
		this.SURV_ITEMCOUNTBOX.Location = new System.Drawing.Point(237, 272);
		this.SURV_ITEMCOUNTBOX.MaxLength = 10;
		this.SURV_ITEMCOUNTBOX.Name = "SURV_ITEMCOUNTBOX";
		this.SURV_ITEMCOUNTBOX.Size = new System.Drawing.Size(54, 20);
		this.SURV_ITEMCOUNTBOX.TabIndex = 94;
		this.SURV_ITEMCOUNTBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label163.AutoSize = true;
		this.label163.Location = new System.Drawing.Point(17, 277);
		this.label163.Name = "label163";
		this.label163.Size = new System.Drawing.Size(98, 13);
		this.label163.TabIndex = 93;
		this.label163.Text = "Reward Item Count";
		this.SURV_ITEMNAMEBOX.Location = new System.Drawing.Point(158, 298);
		this.SURV_ITEMNAMEBOX.MaxLength = 10;
		this.SURV_ITEMNAMEBOX.Name = "SURV_ITEMNAMEBOX";
		this.SURV_ITEMNAMEBOX.Size = new System.Drawing.Size(133, 20);
		this.SURV_ITEMNAMEBOX.TabIndex = 92;
		this.SURV_ITEMNAMEBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label164.AutoSize = true;
		this.label164.Location = new System.Drawing.Point(17, 303);
		this.label164.Name = "label164";
		this.label164.Size = new System.Drawing.Size(98, 13);
		this.label164.TabIndex = 91;
		this.label164.Text = "Reward Item Name";
		this.SURV_ITEMIDBOX.Location = new System.Drawing.Point(237, 244);
		this.SURV_ITEMIDBOX.MaxLength = 10;
		this.SURV_ITEMIDBOX.Name = "SURV_ITEMIDBOX";
		this.SURV_ITEMIDBOX.Size = new System.Drawing.Size(54, 20);
		this.SURV_ITEMIDBOX.TabIndex = 90;
		this.SURV_ITEMIDBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label165.AutoSize = true;
		this.label165.Location = new System.Drawing.Point(17, 249);
		this.label165.Name = "label165";
		this.label165.Size = new System.Drawing.Size(81, 13);
		this.label165.TabIndex = 89;
		this.label165.Text = "Reward Item ID";
		this.SURV_REQUIRELEVELBOX.Location = new System.Drawing.Point(237, 102);
		this.SURV_REQUIRELEVELBOX.MaxLength = 3;
		this.SURV_REQUIRELEVELBOX.Name = "SURV_REQUIRELEVELBOX";
		this.SURV_REQUIRELEVELBOX.Size = new System.Drawing.Size(54, 20);
		this.SURV_REQUIRELEVELBOX.TabIndex = 51;
		this.SURV_REQUIRELEVELBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label166.AutoSize = true;
		this.label166.Location = new System.Drawing.Point(17, 107);
		this.label166.Name = "label166";
		this.label166.Size = new System.Drawing.Size(73, 13);
		this.label166.TabIndex = 49;
		this.label166.Text = "Require Level";
		this.SURV_REGISTERTIMEBOX.Location = new System.Drawing.Point(237, 75);
		this.SURV_REGISTERTIMEBOX.MaxLength = 10;
		this.SURV_REGISTERTIMEBOX.Name = "SURV_REGISTERTIMEBOX";
		this.SURV_REGISTERTIMEBOX.Size = new System.Drawing.Size(54, 20);
		this.SURV_REGISTERTIMEBOX.TabIndex = 38;
		this.SURV_REGISTERTIMEBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.SURV_MATCHTIMEBOX.Location = new System.Drawing.Point(237, 50);
		this.SURV_MATCHTIMEBOX.MaxLength = 10;
		this.SURV_MATCHTIMEBOX.Name = "SURV_MATCHTIMEBOX";
		this.SURV_MATCHTIMEBOX.Size = new System.Drawing.Size(54, 20);
		this.SURV_MATCHTIMEBOX.TabIndex = 37;
		this.SURV_MATCHTIMEBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label167.AutoSize = true;
		this.label167.Location = new System.Drawing.Point(17, 80);
		this.label167.Name = "label167";
		this.label167.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.label167.Size = new System.Drawing.Size(90, 13);
		this.label167.TabIndex = 2;
		this.label167.Text = "Time For Register";
		this.label168.AutoSize = true;
		this.label168.Location = new System.Drawing.Point(17, 55);
		this.label168.Name = "label168";
		this.label168.Size = new System.Drawing.Size(63, 13);
		this.label168.TabIndex = 1;
		this.label168.Text = "Match Time";
		this.SURV_ENABLEBOX.AutoSize = true;
		this.SURV_ENABLEBOX.Location = new System.Drawing.Point(20, 30);
		this.SURV_ENABLEBOX.Name = "SURV_ENABLEBOX";
		this.SURV_ENABLEBOX.Size = new System.Drawing.Size(100, 17);
		this.SURV_ENABLEBOX.TabIndex = 0;
		this.SURV_ENABLEBOX.Text = "Enable Survival";
		this.SURV_ENABLEBOX.UseVisualStyleBackColor = true;
		this.tabPage12.Controls.Add(this.groupBox24);
		this.tabPage12.Controls.Add(this.groupBox23);
		this.tabPage12.Controls.Add(this.groupBox22);
		this.tabPage12.Controls.Add(this.groupBox21);
		this.tabPage12.Location = new System.Drawing.Point(4, 22);
		this.tabPage12.Name = "tabPage12";
		this.tabPage12.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage12.Size = new System.Drawing.Size(1033, 453);
		this.tabPage12.TabIndex = 11;
		this.tabPage12.Text = "User Setttings";
		this.tabPage12.UseVisualStyleBackColor = true;
		this.groupBox24.Controls.Add(this.BanHwidListBox);
		this.groupBox24.Controls.Add(this.AddBanHwid);
		this.groupBox24.Controls.Add(this.RemoveBanHwid);
		this.groupBox24.Controls.Add(this.BanhwidTextBox);
		this.groupBox24.Location = new System.Drawing.Point(706, 283);
		this.groupBox24.Name = "groupBox24";
		this.groupBox24.Size = new System.Drawing.Size(321, 156);
		this.groupBox24.TabIndex = 161;
		this.groupBox24.TabStop = false;
		this.groupBox24.Text = "HWID Ban Settings";
		this.BanHwidListBox.FormattingEnabled = true;
		this.BanHwidListBox.Location = new System.Drawing.Point(17, 45);
		this.BanHwidListBox.Name = "BanHwidListBox";
		this.BanHwidListBox.Size = new System.Drawing.Size(194, 108);
		this.BanHwidListBox.TabIndex = 151;
		this.BanHwidListBox.SelectedIndexChanged += new System.EventHandler(BanHwidListBox_SelectedIndexChanged);
		this.AddBanHwid.Location = new System.Drawing.Point(217, 44);
		this.AddBanHwid.Name = "AddBanHwid";
		this.AddBanHwid.Size = new System.Drawing.Size(93, 23);
		this.AddBanHwid.TabIndex = 155;
		this.AddBanHwid.Text = "<- Add Ban User";
		this.AddBanHwid.UseVisualStyleBackColor = true;
		this.AddBanHwid.Click += new System.EventHandler(AddBanHwid_Click);
		this.RemoveBanHwid.Enabled = false;
		this.RemoveBanHwid.Location = new System.Drawing.Point(217, 71);
		this.RemoveBanHwid.Name = "RemoveBanHwid";
		this.RemoveBanHwid.Size = new System.Drawing.Size(93, 23);
		this.RemoveBanHwid.TabIndex = 157;
		this.RemoveBanHwid.Text = "Remove Ban";
		this.RemoveBanHwid.UseVisualStyleBackColor = true;
		this.RemoveBanHwid.Click += new System.EventHandler(RemoveBanHwid_Click);
		this.BanhwidTextBox.Location = new System.Drawing.Point(17, 19);
		this.BanhwidTextBox.Name = "BanhwidTextBox";
		this.BanhwidTextBox.Size = new System.Drawing.Size(293, 20);
		this.BanhwidTextBox.TabIndex = 156;
		this.groupBox23.Controls.Add(this.BanUserListBox);
		this.groupBox23.Controls.Add(this.AddBanUser);
		this.groupBox23.Controls.Add(this.RemoveBanUser);
		this.groupBox23.Controls.Add(this.BanUserTextBox);
		this.groupBox23.Location = new System.Drawing.Point(706, 142);
		this.groupBox23.Name = "groupBox23";
		this.groupBox23.Size = new System.Drawing.Size(321, 137);
		this.groupBox23.TabIndex = 160;
		this.groupBox23.TabStop = false;
		this.groupBox23.Text = "User Ban Settings";
		this.BanUserListBox.FormattingEnabled = true;
		this.BanUserListBox.Location = new System.Drawing.Point(17, 19);
		this.BanUserListBox.Name = "BanUserListBox";
		this.BanUserListBox.Size = new System.Drawing.Size(100, 108);
		this.BanUserListBox.TabIndex = 151;
		this.BanUserListBox.SelectedIndexChanged += new System.EventHandler(BanUserListBox_SelectedIndexChanged);
		this.AddBanUser.Location = new System.Drawing.Point(124, 23);
		this.AddBanUser.Name = "AddBanUser";
		this.AddBanUser.Size = new System.Drawing.Size(93, 23);
		this.AddBanUser.TabIndex = 155;
		this.AddBanUser.Text = "<- Add Ban User";
		this.AddBanUser.UseVisualStyleBackColor = true;
		this.AddBanUser.Click += new System.EventHandler(AddBanUser_Click);
		this.RemoveBanUser.Enabled = false;
		this.RemoveBanUser.Location = new System.Drawing.Point(124, 50);
		this.RemoveBanUser.Name = "RemoveBanUser";
		this.RemoveBanUser.Size = new System.Drawing.Size(93, 23);
		this.RemoveBanUser.TabIndex = 157;
		this.RemoveBanUser.Text = "Remove Ban";
		this.RemoveBanUser.UseVisualStyleBackColor = true;
		this.RemoveBanUser.Click += new System.EventHandler(RemoveBanUser_Click);
		this.BanUserTextBox.Location = new System.Drawing.Point(223, 24);
		this.BanUserTextBox.Name = "BanUserTextBox";
		this.BanUserTextBox.Size = new System.Drawing.Size(87, 20);
		this.BanUserTextBox.TabIndex = 156;
		this.BanUserTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(BanUserTextBox_KeyPress);
		this.groupBox22.Controls.Add(this.ScanOnlineCheckbox);
		this.groupBox22.Controls.Add(this.userInfo1);
		this.groupBox22.Location = new System.Drawing.Point(3, 6);
		this.groupBox22.Name = "groupBox22";
		this.groupBox22.Size = new System.Drawing.Size(697, 433);
		this.groupBox22.TabIndex = 159;
		this.groupBox22.TabStop = false;
		this.ScanOnlineCheckbox.AutoSize = true;
		this.ScanOnlineCheckbox.Location = new System.Drawing.Point(532, 26);
		this.ScanOnlineCheckbox.Name = "ScanOnlineCheckbox";
		this.ScanOnlineCheckbox.Size = new System.Drawing.Size(66, 17);
		this.ScanOnlineCheckbox.TabIndex = 1;
		this.ScanOnlineCheckbox.Text = "ONLINE";
		this.ScanOnlineCheckbox.UseVisualStyleBackColor = true;
		this.ScanOnlineCheckbox.CheckedChanged += new System.EventHandler(ScanOnlineCheckbox_CheckedChanged);
		this.userInfo1.Location = new System.Drawing.Point(5, 19);
		this.userInfo1.Name = "userInfo1";
		this.userInfo1.Size = new System.Drawing.Size(690, 405);
		this.userInfo1.TabIndex = 0;
		this.groupBox21.Controls.Add(this.BanIpListBox);
		this.groupBox21.Controls.Add(this.RemoveBanIP);
		this.groupBox21.Controls.Add(this.BanIPTextBox);
		this.groupBox21.Controls.Add(this.AddBanIP);
		this.groupBox21.Location = new System.Drawing.Point(706, 6);
		this.groupBox21.Name = "groupBox21";
		this.groupBox21.Size = new System.Drawing.Size(321, 134);
		this.groupBox21.TabIndex = 158;
		this.groupBox21.TabStop = false;
		this.groupBox21.Text = "IP Ban Settings";
		this.BanIpListBox.FormattingEnabled = true;
		this.BanIpListBox.Location = new System.Drawing.Point(17, 19);
		this.BanIpListBox.Name = "BanIpListBox";
		this.BanIpListBox.Size = new System.Drawing.Size(100, 108);
		this.BanIpListBox.TabIndex = 6;
		this.BanIpListBox.SelectedIndexChanged += new System.EventHandler(BanIpListBox_SelectedIndexChanged);
		this.RemoveBanIP.Enabled = false;
		this.RemoveBanIP.Location = new System.Drawing.Point(124, 47);
		this.RemoveBanIP.Name = "RemoveBanIP";
		this.RemoveBanIP.Size = new System.Drawing.Size(93, 23);
		this.RemoveBanIP.TabIndex = 154;
		this.RemoveBanIP.Text = "Remove Ban";
		this.RemoveBanIP.UseVisualStyleBackColor = true;
		this.RemoveBanIP.Click += new System.EventHandler(RemoveBanIP_Click);
		this.BanIPTextBox.Location = new System.Drawing.Point(223, 22);
		this.BanIPTextBox.Name = "BanIPTextBox";
		this.BanIPTextBox.Size = new System.Drawing.Size(87, 20);
		this.BanIPTextBox.TabIndex = 153;
		this.BanIPTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(BanIPTextBox_KeyPress);
		this.AddBanIP.Location = new System.Drawing.Point(124, 20);
		this.AddBanIP.Name = "AddBanIP";
		this.AddBanIP.Size = new System.Drawing.Size(93, 23);
		this.AddBanIP.TabIndex = 152;
		this.AddBanIP.Text = "<- Add Ban IP";
		this.AddBanIP.UseVisualStyleBackColor = true;
		this.AddBanIP.Click += new System.EventHandler(AddBanIP_Click);
		this.tabPage13.BackColor = System.Drawing.Color.WhiteSmoke;
		this.tabPage13.Controls.Add(this.groupBox33);
		this.tabPage13.Controls.Add(this.groupBox31);
		this.tabPage13.Controls.Add(this.groupBox26);
		this.tabPage13.Controls.Add(this.groupBox25);
		this.tabPage13.Location = new System.Drawing.Point(4, 22);
		this.tabPage13.Name = "tabPage13";
		this.tabPage13.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage13.Size = new System.Drawing.Size(1033, 453);
		this.tabPage13.TabIndex = 12;
		this.tabPage13.Text = "Gui Settings";
		this.groupBox33.Controls.Add(this.titlepricebirim);
		this.groupBox33.Controls.Add(this.label288);
		this.groupBox33.Controls.Add(this.titleprices);
		this.groupBox33.Controls.Add(this.marketbutton);
		this.groupBox33.Controls.Add(this.goldboxs);
		this.groupBox33.Controls.Add(this.silksystembox);
		this.groupBox33.Controls.Add(this.label287);
		this.groupBox33.Controls.Add(this.tokenbox);
		this.groupBox33.Location = new System.Drawing.Point(831, 12);
		this.groupBox33.Name = "groupBox33";
		this.groupBox33.Size = new System.Drawing.Size(196, 416);
		this.groupBox33.TabIndex = 162;
		this.groupBox33.TabStop = false;
		this.groupBox33.Text = "Gui Market Settings";
		this.titlepricebirim.Location = new System.Drawing.Point(6, 174);
		this.titlepricebirim.MaxLength = 5;
		this.titlepricebirim.Name = "titlepricebirim";
		this.titlepricebirim.Size = new System.Drawing.Size(87, 20);
		this.titlepricebirim.TabIndex = 169;
		this.label288.AutoSize = true;
		this.label288.Location = new System.Drawing.Point(3, 122);
		this.label288.Name = "label288";
		this.label288.Size = new System.Drawing.Size(191, 13);
		this.label288.TabIndex = 168;
		this.label288.Text = "Custom Title Price Sadece birim giriniz. ";
		this.titleprices.Location = new System.Drawing.Point(6, 148);
		this.titleprices.MaxLength = 5;
		this.titleprices.Name = "titleprices";
		this.titleprices.Size = new System.Drawing.Size(87, 20);
		this.titleprices.TabIndex = 167;
		this.marketbutton.AutoSize = true;
		this.marketbutton.Location = new System.Drawing.Point(4, 27);
		this.marketbutton.Name = "marketbutton";
		this.marketbutton.Size = new System.Drawing.Size(129, 17);
		this.marketbutton.TabIndex = 166;
		this.marketbutton.Text = "Enable Market Button";
		this.marketbutton.UseVisualStyleBackColor = true;
		this.goldboxs.AutoSize = true;
		this.goldboxs.Location = new System.Drawing.Point(4, 90);
		this.goldboxs.Name = "goldboxs";
		this.goldboxs.Size = new System.Drawing.Size(84, 17);
		this.goldboxs.TabIndex = 165;
		this.goldboxs.Text = "Enable Gold";
		this.goldboxs.UseVisualStyleBackColor = true;
		this.silksystembox.AutoSize = true;
		this.silksystembox.Location = new System.Drawing.Point(4, 67);
		this.silksystembox.Name = "silksystembox";
		this.silksystembox.Size = new System.Drawing.Size(82, 17);
		this.silksystembox.TabIndex = 164;
		this.silksystembox.Text = "Enable Silk ";
		this.silksystembox.UseVisualStyleBackColor = true;
		this.label287.AutoSize = true;
		this.label287.Location = new System.Drawing.Point(274, 43);
		this.label287.Name = "label287";
		this.label287.Size = new System.Drawing.Size(52, 13);
		this.label287.TabIndex = 162;
		this.label287.Text = "DEACTIF";
		this.tokenbox.AutoSize = true;
		this.tokenbox.Location = new System.Drawing.Point(4, 46);
		this.tokenbox.Name = "tokenbox";
		this.tokenbox.Size = new System.Drawing.Size(93, 17);
		this.tokenbox.TabIndex = 4;
		this.tokenbox.Text = "Enable Token";
		this.tokenbox.UseVisualStyleBackColor = true;
		this.groupBox31.Controls.Add(this.groupBox32);
		this.groupBox31.Location = new System.Drawing.Point(589, 12);
		this.groupBox31.Name = "groupBox31";
		this.groupBox31.Size = new System.Drawing.Size(236, 441);
		this.groupBox31.TabIndex = 161;
		this.groupBox31.TabStop = false;
		this.groupBox31.Text = "Block Settings";
		this.groupBox32.Controls.Add(this.label280);
		this.groupBox32.Controls.Add(this.Block_Skill_TextBoxSkillID);
		this.groupBox32.Controls.Add(this.label279);
		this.groupBox32.Controls.Add(this.Block_Skill_listBox);
		this.groupBox32.Controls.Add(this.Block_Skill_Add_Button);
		this.groupBox32.Controls.Add(this.Block_Skill_Remove_Button);
		this.groupBox32.Controls.Add(this.Block_Skill_TextBox);
		this.groupBox32.Location = new System.Drawing.Point(6, 19);
		this.groupBox32.Name = "groupBox32";
		this.groupBox32.Size = new System.Drawing.Size(224, 199);
		this.groupBox32.TabIndex = 162;
		this.groupBox32.TabStop = false;
		this.groupBox32.Text = "Block Skill";
		this.label280.AutoSize = true;
		this.label280.Location = new System.Drawing.Point(147, 26);
		this.label280.Name = "label280";
		this.label280.Size = new System.Drawing.Size(40, 13);
		this.label280.TabIndex = 163;
		this.label280.Text = "Skill ID";
		this.Block_Skill_TextBoxSkillID.Location = new System.Drawing.Point(126, 42);
		this.Block_Skill_TextBoxSkillID.MaxLength = 5;
		this.Block_Skill_TextBoxSkillID.Name = "Block_Skill_TextBoxSkillID";
		this.Block_Skill_TextBoxSkillID.Size = new System.Drawing.Size(87, 20);
		this.Block_Skill_TextBoxSkillID.TabIndex = 162;
		this.label279.AutoSize = true;
		this.label279.Location = new System.Drawing.Point(6, 26);
		this.label279.Name = "label279";
		this.label279.Size = new System.Drawing.Size(58, 13);
		this.label279.TabIndex = 161;
		this.label279.Text = "Region ID ";
		this.Block_Skill_listBox.FormattingEnabled = true;
		this.Block_Skill_listBox.Location = new System.Drawing.Point(6, 65);
		this.Block_Skill_listBox.Name = "Block_Skill_listBox";
		this.Block_Skill_listBox.Size = new System.Drawing.Size(114, 121);
		this.Block_Skill_listBox.TabIndex = 59;
		this.Block_Skill_listBox.SelectedIndexChanged += new System.EventHandler(Block_Skill_listBox_SelectedIndexChanged);
		this.Block_Skill_Add_Button.Location = new System.Drawing.Point(126, 78);
		this.Block_Skill_Add_Button.Name = "Block_Skill_Add_Button";
		this.Block_Skill_Add_Button.Size = new System.Drawing.Size(87, 23);
		this.Block_Skill_Add_Button.TabIndex = 158;
		this.Block_Skill_Add_Button.Text = "<- Add";
		this.Block_Skill_Add_Button.UseVisualStyleBackColor = true;
		this.Block_Skill_Add_Button.Click += new System.EventHandler(Block_Skill_Add_Button_Click);
		this.Block_Skill_Remove_Button.Enabled = false;
		this.Block_Skill_Remove_Button.Location = new System.Drawing.Point(126, 105);
		this.Block_Skill_Remove_Button.Name = "Block_Skill_Remove_Button";
		this.Block_Skill_Remove_Button.Size = new System.Drawing.Size(87, 23);
		this.Block_Skill_Remove_Button.TabIndex = 160;
		this.Block_Skill_Remove_Button.Text = "Remove Region";
		this.Block_Skill_Remove_Button.UseVisualStyleBackColor = true;
		this.Block_Skill_Remove_Button.Click += new System.EventHandler(Block_Skill_Remove_Button_Click);
		this.Block_Skill_TextBox.Location = new System.Drawing.Point(6, 42);
		this.Block_Skill_TextBox.MaxLength = 5;
		this.Block_Skill_TextBox.Name = "Block_Skill_TextBox";
		this.Block_Skill_TextBox.Size = new System.Drawing.Size(87, 20);
		this.Block_Skill_TextBox.TabIndex = 159;
		this.groupBox26.Controls.Add(this.addregionbutton);
		this.groupBox26.Controls.Add(this.removeregionButton);
		this.groupBox26.Controls.Add(this.SuitRegiontextbox);
		this.groupBox26.Controls.Add(this.suitlistbox);
		this.groupBox26.Location = new System.Drawing.Point(347, 6);
		this.groupBox26.Name = "groupBox26";
		this.groupBox26.Size = new System.Drawing.Size(236, 441);
		this.groupBox26.TabIndex = 27;
		this.groupBox26.TabStop = false;
		this.groupBox26.Text = "Event Suit Regions";
		this.addregionbutton.Location = new System.Drawing.Point(132, 46);
		this.addregionbutton.Name = "addregionbutton";
		this.addregionbutton.Size = new System.Drawing.Size(87, 23);
		this.addregionbutton.TabIndex = 158;
		this.addregionbutton.Text = "<- Add Region";
		this.addregionbutton.UseVisualStyleBackColor = true;
		this.addregionbutton.Click += new System.EventHandler(button4_Click);
		this.removeregionButton.Enabled = false;
		this.removeregionButton.Location = new System.Drawing.Point(132, 73);
		this.removeregionButton.Name = "removeregionButton";
		this.removeregionButton.Size = new System.Drawing.Size(87, 23);
		this.removeregionButton.TabIndex = 160;
		this.removeregionButton.Text = "Remove Region";
		this.removeregionButton.UseVisualStyleBackColor = true;
		this.removeregionButton.Click += new System.EventHandler(button5_Click);
		this.SuitRegiontextbox.Location = new System.Drawing.Point(132, 20);
		this.SuitRegiontextbox.MaxLength = 5;
		this.SuitRegiontextbox.Name = "SuitRegiontextbox";
		this.SuitRegiontextbox.Size = new System.Drawing.Size(87, 20);
		this.SuitRegiontextbox.TabIndex = 159;
		this.suitlistbox.FormattingEnabled = true;
		this.suitlistbox.Location = new System.Drawing.Point(6, 20);
		this.suitlistbox.Name = "suitlistbox";
		this.suitlistbox.Size = new System.Drawing.Size(114, 121);
		this.suitlistbox.TabIndex = 59;
		this.suitlistbox.SelectedIndexChanged += new System.EventHandler(suitlistbox_SelectedIndexChanged);
		this.groupBox25.Controls.Add(this.GUILDJOB);
		this.groupBox25.Controls.Add(this.ENABLE_PALCHEMY);
		this.groupBox25.Controls.Add(this.label278);
		this.groupBox25.Controls.Add(this.ATTENDANCE_comboBox);
		this.groupBox25.Controls.Add(this.label275);
		this.groupBox25.Controls.Add(this.MaxPtNoLimit);
		this.groupBox25.Controls.Add(this.label274);
		this.groupBox25.Controls.Add(this.MaxMastery);
		this.groupBox25.Controls.Add(this.label273);
		this.groupBox25.Controls.Add(this.facebooksite);
		this.groupBox25.Controls.Add(this.label272);
		this.groupBox25.Controls.Add(this.DiscordSite);
		this.groupBox25.Controls.Add(this.checkBox1itemcomp);
		this.groupBox25.Controls.Add(this.checkBox1oldmain);
		this.groupBox25.Controls.Add(this.checkBoxNewRew);
		this.groupBox25.Controls.Add(this.label271);
		this.groupBox25.Controls.Add(this.DCID);
		this.groupBox25.Controls.Add(this.ENABLE_CHEST);
		this.groupBox25.Controls.Add(this.ENABLE_EVNTREG);
		this.groupBox25.Controls.Add(this.ENABLE_FB);
		this.groupBox25.Controls.Add(this.ENABLE_DC);
		this.groupBox25.Controls.Add(this.ENABLE_ATTENDANCE);
		this.groupBox25.Location = new System.Drawing.Point(8, 6);
		this.groupBox25.Name = "groupBox25";
		this.groupBox25.Size = new System.Drawing.Size(333, 441);
		this.groupBox25.TabIndex = 26;
		this.groupBox25.TabStop = false;
		this.groupBox25.Text = "Gui Settings";
		this.GUILDJOB.AutoSize = true;
		this.GUILDJOB.Location = new System.Drawing.Point(6, 226);
		this.GUILDJOB.Name = "GUILDJOB";
		this.GUILDJOB.Size = new System.Drawing.Size(130, 17);
		this.GUILDJOB.TabIndex = 164;
		this.GUILDJOB.Text = "Show Guild While Job";
		this.GUILDJOB.UseVisualStyleBackColor = true;
		this.ENABLE_PALCHEMY.AutoSize = true;
		this.ENABLE_PALCHEMY.Location = new System.Drawing.Point(6, 203);
		this.ENABLE_PALCHEMY.Name = "ENABLE_PALCHEMY";
		this.ENABLE_PALCHEMY.Size = new System.Drawing.Size(139, 17);
		this.ENABLE_PALCHEMY.TabIndex = 163;
		this.ENABLE_PALCHEMY.Text = "Enable P. Alchemy Mat.";
		this.ENABLE_PALCHEMY.UseVisualStyleBackColor = true;
		this.label278.AutoSize = true;
		this.label278.Location = new System.Drawing.Point(274, 43);
		this.label278.Name = "label278";
		this.label278.Size = new System.Drawing.Size(52, 13);
		this.label278.TabIndex = 162;
		this.label278.Text = "DEACTIF";
		this.ATTENDANCE_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.ATTENDANCE_comboBox.FormattingEnabled = true;
		this.ATTENDANCE_comboBox.Items.AddRange(new object[12] { "January (Jan): Ocak.", "February (Feb): Şubat.", "March (Mar): Mart.", "April (Apr): Nisan.", "May (May): Mayıs.", "June (Jun): Haziran.", "July (Jul): Temmuz.", "August (Aug): Ağustos.", "September (Sep): Eylül.", "October (Oct): Ekim.", "November (Nov): Kasım.", "December (Dec): Aralık." });
		this.ATTENDANCE_comboBox.Location = new System.Drawing.Point(149, 40);
		this.ATTENDANCE_comboBox.Name = "ATTENDANCE_comboBox";
		this.ATTENDANCE_comboBox.Size = new System.Drawing.Size(121, 21);
		this.ATTENDANCE_comboBox.TabIndex = 161;
		this.ATTENDANCE_comboBox.SelectedIndexChanged += new System.EventHandler(ATTENDANCE_comboBox_SelectedIndexChanged);
		this.label275.AutoSize = true;
		this.label275.Location = new System.Drawing.Point(6, 305);
		this.label275.Name = "label275";
		this.label275.Size = new System.Drawing.Size(116, 13);
		this.label275.TabIndex = 66;
		this.label275.Text = "Party Match Max Level";
		this.MaxPtNoLimit.Location = new System.Drawing.Point(128, 302);
		this.MaxPtNoLimit.Name = "MaxPtNoLimit";
		this.MaxPtNoLimit.Size = new System.Drawing.Size(141, 20);
		this.MaxPtNoLimit.TabIndex = 65;
		this.label274.AutoSize = true;
		this.label274.Location = new System.Drawing.Point(6, 279);
		this.label274.Name = "label274";
		this.label274.Size = new System.Drawing.Size(68, 13);
		this.label274.TabIndex = 64;
		this.label274.Text = "Mastery Limit";
		this.MaxMastery.Location = new System.Drawing.Point(128, 276);
		this.MaxMastery.Name = "MaxMastery";
		this.MaxMastery.Size = new System.Drawing.Size(141, 20);
		this.MaxMastery.TabIndex = 63;
		this.label273.AutoSize = true;
		this.label273.Location = new System.Drawing.Point(16, 333);
		this.label273.Name = "label273";
		this.label273.Size = new System.Drawing.Size(56, 13);
		this.label273.TabIndex = 62;
		this.label273.Text = "Face URL";
		this.facebooksite.Location = new System.Drawing.Point(78, 330);
		this.facebooksite.Name = "facebooksite";
		this.facebooksite.Size = new System.Drawing.Size(201, 20);
		this.facebooksite.TabIndex = 61;
		this.label272.AutoSize = true;
		this.label272.Location = new System.Drawing.Point(4, 362);
		this.label272.Name = "label272";
		this.label272.Size = new System.Drawing.Size(68, 13);
		this.label272.TabIndex = 60;
		this.label272.Text = "Discord URL";
		this.DiscordSite.Location = new System.Drawing.Point(78, 359);
		this.DiscordSite.Name = "DiscordSite";
		this.DiscordSite.Size = new System.Drawing.Size(201, 20);
		this.DiscordSite.TabIndex = 59;
		this.checkBox1itemcomp.AutoSize = true;
		this.checkBox1itemcomp.Location = new System.Drawing.Point(6, 180);
		this.checkBox1itemcomp.Name = "checkBox1itemcomp";
		this.checkBox1itemcomp.Size = new System.Drawing.Size(140, 17);
		this.checkBox1itemcomp.TabIndex = 58;
		this.checkBox1itemcomp.Text = "Enable Item Comparison";
		this.checkBox1itemcomp.UseVisualStyleBackColor = true;
		this.checkBox1oldmain.AutoSize = true;
		this.checkBox1oldmain.Location = new System.Drawing.Point(6, 157);
		this.checkBox1oldmain.Name = "checkBox1oldmain";
		this.checkBox1oldmain.Size = new System.Drawing.Size(135, 17);
		this.checkBox1oldmain.TabIndex = 57;
		this.checkBox1oldmain.Text = "Enable Old MainPopup";
		this.checkBox1oldmain.UseVisualStyleBackColor = true;
		this.checkBoxNewRew.AutoSize = true;
		this.checkBoxNewRew.Location = new System.Drawing.Point(6, 134);
		this.checkBoxNewRew.Name = "checkBoxNewRew";
		this.checkBoxNewRew.Size = new System.Drawing.Size(127, 17);
		this.checkBoxNewRew.TabIndex = 56;
		this.checkBoxNewRew.Text = "Enable New Reverse";
		this.checkBoxNewRew.UseVisualStyleBackColor = true;
		this.label271.AutoSize = true;
		this.label271.Location = new System.Drawing.Point(15, 388);
		this.label271.Name = "label271";
		this.label271.Size = new System.Drawing.Size(57, 13);
		this.label271.TabIndex = 55;
		this.label271.Text = "Discord ID";
		this.DCID.Location = new System.Drawing.Point(78, 385);
		this.DCID.Name = "DCID";
		this.DCID.Size = new System.Drawing.Size(201, 20);
		this.DCID.TabIndex = 5;
		this.ENABLE_CHEST.AutoSize = true;
		this.ENABLE_CHEST.Location = new System.Drawing.Point(6, 19);
		this.ENABLE_CHEST.Name = "ENABLE_CHEST";
		this.ENABLE_CHEST.Size = new System.Drawing.Size(113, 17);
		this.ENABLE_CHEST.TabIndex = 4;
		this.ENABLE_CHEST.Text = "Enable Chest Icon";
		this.ENABLE_CHEST.UseVisualStyleBackColor = true;
		this.ENABLE_EVNTREG.AutoSize = true;
		this.ENABLE_EVNTREG.Location = new System.Drawing.Point(6, 65);
		this.ENABLE_EVNTREG.Name = "ENABLE_EVNTREG";
		this.ENABLE_EVNTREG.Size = new System.Drawing.Size(156, 17);
		this.ENABLE_EVNTREG.TabIndex = 3;
		this.ENABLE_EVNTREG.Text = "Enable Event Register Icon";
		this.ENABLE_EVNTREG.UseVisualStyleBackColor = true;
		this.ENABLE_FB.AutoSize = true;
		this.ENABLE_FB.Location = new System.Drawing.Point(6, 88);
		this.ENABLE_FB.Name = "ENABLE_FB";
		this.ENABLE_FB.Size = new System.Drawing.Size(134, 17);
		this.ENABLE_FB.TabIndex = 2;
		this.ENABLE_FB.Text = "Enable Facebook Icon";
		this.ENABLE_FB.UseVisualStyleBackColor = true;
		this.ENABLE_DC.AutoSize = true;
		this.ENABLE_DC.Location = new System.Drawing.Point(6, 111);
		this.ENABLE_DC.Name = "ENABLE_DC";
		this.ENABLE_DC.Size = new System.Drawing.Size(122, 17);
		this.ENABLE_DC.TabIndex = 1;
		this.ENABLE_DC.Text = "Enable Discord Icon";
		this.ENABLE_DC.UseVisualStyleBackColor = true;
		this.ENABLE_ATTENDANCE.AutoSize = true;
		this.ENABLE_ATTENDANCE.Location = new System.Drawing.Point(6, 42);
		this.ENABLE_ATTENDANCE.Name = "ENABLE_ATTENDANCE";
		this.ENABLE_ATTENDANCE.Size = new System.Drawing.Size(141, 17);
		this.ENABLE_ATTENDANCE.TabIndex = 0;
		this.ENABLE_ATTENDANCE.Text = "Enable Attendance Icon";
		this.ENABLE_ATTENDANCE.UseVisualStyleBackColor = true;
		this.ENABLE_ATTENDANCE.CheckedChanged += new System.EventHandler(ENABLE_ATTENDANCE_CheckedChanged);
		this.tabPage14.Controls.Add(this.LisansAutoCheckBox);
		this.tabPage14.Controls.Add(this.button4);
		this.tabPage14.Controls.Add(this.Login);
		this.tabPage14.Controls.Add(this.TextBoxLisansPassword);
		this.tabPage14.Controls.Add(this.label283);
		this.tabPage14.Controls.Add(this.TextBoxLisansUserName);
		this.tabPage14.Controls.Add(this.label284);
		this.tabPage14.Location = new System.Drawing.Point(4, 22);
		this.tabPage14.Name = "tabPage14";
		this.tabPage14.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage14.Size = new System.Drawing.Size(1033, 453);
		this.tabPage14.TabIndex = 13;
		this.tabPage14.Text = "Lisans Config";
		this.tabPage14.UseVisualStyleBackColor = true;
		this.LisansAutoCheckBox.AutoSize = true;
		this.LisansAutoCheckBox.Location = new System.Drawing.Point(95, 90);
		this.LisansAutoCheckBox.Name = "LisansAutoCheckBox";
		this.LisansAutoCheckBox.Size = new System.Drawing.Size(91, 17);
		this.LisansAutoCheckBox.TabIndex = 18;
		this.LisansAutoCheckBox.Text = "Otomatik Giriş";
		this.LisansAutoCheckBox.UseVisualStyleBackColor = true;
		this.button4.Location = new System.Drawing.Point(192, 86);
		this.button4.Name = "button4";
		this.button4.Size = new System.Drawing.Size(75, 23);
		this.button4.TabIndex = 17;
		this.button4.Text = "Cancel";
		this.button4.UseVisualStyleBackColor = true;
		this.button4.Click += new System.EventHandler(button4_Click_1);
		this.Login.Location = new System.Drawing.Point(273, 86);
		this.Login.Name = "Login";
		this.Login.Size = new System.Drawing.Size(75, 23);
		this.Login.TabIndex = 16;
		this.Login.Text = "Login";
		this.Login.UseVisualStyleBackColor = true;
		this.Login.Click += new System.EventHandler(Login_Click);
		this.TextBoxLisansPassword.Location = new System.Drawing.Point(92, 47);
		this.TextBoxLisansPassword.Name = "TextBoxLisansPassword";
		this.TextBoxLisansPassword.Size = new System.Drawing.Size(100, 20);
		this.TextBoxLisansPassword.TabIndex = 15;
		this.label283.AutoSize = true;
		this.label283.Location = new System.Drawing.Point(22, 50);
		this.label283.Name = "label283";
		this.label283.Size = new System.Drawing.Size(56, 13);
		this.label283.TabIndex = 14;
		this.label283.Text = "Password:";
		this.TextBoxLisansUserName.Location = new System.Drawing.Point(92, 14);
		this.TextBoxLisansUserName.Name = "TextBoxLisansUserName";
		this.TextBoxLisansUserName.Size = new System.Drawing.Size(100, 20);
		this.TextBoxLisansUserName.TabIndex = 13;
		this.label284.AutoSize = true;
		this.label284.Location = new System.Drawing.Point(22, 17);
		this.label284.Name = "label284";
		this.label284.Size = new System.Drawing.Size(66, 13);
		this.label284.TabIndex = 12;
		this.label284.Text = "User Name: ";
		this.Open_Directory_Button.Location = new System.Drawing.Point(1047, 294);
		this.Open_Directory_Button.Name = "Open_Directory_Button";
		this.Open_Directory_Button.Size = new System.Drawing.Size(125, 27);
		this.Open_Directory_Button.TabIndex = 9;
		this.Open_Directory_Button.Text = "Open Directory";
		this.Open_Directory_Button.UseVisualStyleBackColor = true;
		this.Open_Directory_Button.Click += new System.EventHandler(Open_Directory_Button_Click);
		this.RestartFilter_Button.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.RestartFilter_Button.Location = new System.Drawing.Point(1047, 348);
		this.RestartFilter_Button.Name = "RestartFilter_Button";
		this.RestartFilter_Button.Size = new System.Drawing.Size(125, 27);
		this.RestartFilter_Button.TabIndex = 33;
		this.RestartFilter_Button.Text = "Restart Filter";
		this.RestartFilter_Button.UseVisualStyleBackColor = true;
		this.RestartFilter_Button.Click += new System.EventHandler(RestartFilter_Button_Click);
		this.Save_Settings_Buttun.Location = new System.Drawing.Point(1047, 258);
		this.Save_Settings_Buttun.Name = "Save_Settings_Buttun";
		this.Save_Settings_Buttun.Size = new System.Drawing.Size(125, 27);
		this.Save_Settings_Buttun.TabIndex = 34;
		this.Save_Settings_Buttun.Text = "Save Settings";
		this.Save_Settings_Buttun.UseVisualStyleBackColor = true;
		this.Save_Settings_Buttun.Click += new System.EventHandler(Save_Settings_Buttun_Click);
		this.button1.Location = new System.Drawing.Point(1047, 213);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(125, 27);
		this.button1.TabIndex = 35;
		this.button1.Text = "Database Backup";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.button2.Location = new System.Drawing.Point(1047, 133);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(75, 23);
		this.button2.TabIndex = 36;
		this.button2.Text = "Global Color";
		this.button2.UseVisualStyleBackColor = true;
		this.button2.Click += new System.EventHandler(button2_Click_1);
		this.LicenseStatus.AutoSize = true;
		this.LicenseStatus.Location = new System.Drawing.Point(1135, 90);
		this.LicenseStatus.Name = "LicenseStatus";
		this.LicenseStatus.Size = new System.Drawing.Size(13, 13);
		this.LicenseStatus.TabIndex = 37;
		this.LicenseStatus.Text = "1";
		this.label285.AutoSize = true;
		this.label285.Location = new System.Drawing.Point(1056, 90);
		this.label285.Name = "label285";
		this.label285.Size = new System.Drawing.Size(83, 13);
		this.label285.TabIndex = 38;
		this.label285.Text = "License Status: ";
		this.label286.AutoSize = true;
		this.label286.Location = new System.Drawing.Point(1063, 111);
		this.label286.Name = "label286";
		this.label286.Size = new System.Drawing.Size(76, 13);
		this.label286.TabIndex = 40;
		this.label286.Text = "License Date: ";
		this.LisansDateLable.AutoSize = true;
		this.LisansDateLable.Location = new System.Drawing.Point(1145, 111);
		this.LisansDateLable.Name = "LisansDateLable";
		this.LisansDateLable.Size = new System.Drawing.Size(13, 13);
		this.LisansDateLable.TabIndex = 39;
		this.LisansDateLable.Text = "0";
		this.JobReverseBlock.AutoSize = true;
		this.JobReverseBlock.Location = new System.Drawing.Point(34, 238);
		this.JobReverseBlock.Name = "JobReverseBlock";
		this.JobReverseBlock.Size = new System.Drawing.Size(111, 17);
		this.JobReverseBlock.TabIndex = 151;
		this.JobReverseBlock.Text = "Job Block reverse";
		this.JobReverseBlock.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1184, 646);
		base.Controls.Add(this.label286);
		base.Controls.Add(this.LisansDateLable);
		base.Controls.Add(this.label285);
		base.Controls.Add(this.LicenseStatus);
		base.Controls.Add(this.button2);
		base.Controls.Add(this.button1);
		base.Controls.Add(this.Save_Settings_Buttun);
		base.Controls.Add(this.RestartFilter_Button);
		base.Controls.Add(this.Open_Directory_Button);
		base.Controls.Add(this.tabControl1);
		base.Controls.Add(this.Download_Count_Lable);
		base.Controls.Add(this.Gateway_Count_Lable);
		base.Controls.Add(this.Agent_Count_Lable);
		base.Controls.Add(this.listView1);
		base.Name = "MainMenu";
		this.Text = "MainMenu";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(MainMenu_FormClosing);
		base.Load += new System.EventHandler(MainMenu_Load);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.groupBox20.ResumeLayout(false);
		this.groupBox20.PerformLayout();
		this.groupBox6.ResumeLayout(false);
		this.groupBox6.PerformLayout();
		this.groupBox9.ResumeLayout(false);
		this.groupBox9.PerformLayout();
		this.groupBox8.ResumeLayout(false);
		this.groupBox8.PerformLayout();
		this.groupBox7.ResumeLayout(false);
		this.groupBox7.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.groupBox12.ResumeLayout(false);
		this.groupBox12.PerformLayout();
		this.tabPage3.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.tabPage4.ResumeLayout(false);
		this.groupBox30.ResumeLayout(false);
		this.groupBox30.PerformLayout();
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		this.groupBox16.ResumeLayout(false);
		this.groupBox16.PerformLayout();
		this.tabPage5.ResumeLayout(false);
		this.groupBox37.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
		this.groupBox27.ResumeLayout(false);
		this.groupBox27.PerformLayout();
		this.tabPage6.ResumeLayout(false);
		this.groupBox11.ResumeLayout(false);
		this.groupBox11.PerformLayout();
		this.groupBox10.ResumeLayout(false);
		this.groupBox10.PerformLayout();
		this.groupBox29.ResumeLayout(false);
		this.groupBox29.PerformLayout();
		this.groupBox38.ResumeLayout(false);
		this.groupBox38.PerformLayout();
		this.tabPage7.ResumeLayout(false);
		this.groupBox13.ResumeLayout(false);
		this.groupBox13.PerformLayout();
		this.groupBox49.ResumeLayout(false);
		this.groupBox49.PerformLayout();
		this.tabPage8.ResumeLayout(false);
		this.groupBox15.ResumeLayout(false);
		this.groupBox15.PerformLayout();
		this.groupBox14.ResumeLayout(false);
		this.groupBox14.PerformLayout();
		this.groupBox45.ResumeLayout(false);
		this.groupBox45.PerformLayout();
		this.groupBox43.ResumeLayout(false);
		this.groupBox43.PerformLayout();
		this.tabPage9.ResumeLayout(false);
		this.groupBox17.ResumeLayout(false);
		this.groupBox17.PerformLayout();
		this.groupBox47.ResumeLayout(false);
		this.groupBox47.PerformLayout();
		this.tabPage10.ResumeLayout(false);
		this.groupBox18.ResumeLayout(false);
		this.groupBox18.PerformLayout();
		this.groupBox53.ResumeLayout(false);
		this.groupBox53.PerformLayout();
		this.tabPage11.ResumeLayout(false);
		this.groupBox19.ResumeLayout(false);
		this.groupBox19.PerformLayout();
		this.groupBox28.ResumeLayout(false);
		this.groupBox28.PerformLayout();
		this.tabPage12.ResumeLayout(false);
		this.groupBox24.ResumeLayout(false);
		this.groupBox24.PerformLayout();
		this.groupBox23.ResumeLayout(false);
		this.groupBox23.PerformLayout();
		this.groupBox22.ResumeLayout(false);
		this.groupBox22.PerformLayout();
		this.groupBox21.ResumeLayout(false);
		this.groupBox21.PerformLayout();
		this.tabPage13.ResumeLayout(false);
		this.groupBox33.ResumeLayout(false);
		this.groupBox33.PerformLayout();
		this.groupBox31.ResumeLayout(false);
		this.groupBox32.ResumeLayout(false);
		this.groupBox32.PerformLayout();
		this.groupBox26.ResumeLayout(false);
		this.groupBox26.PerformLayout();
		this.groupBox25.ResumeLayout(false);
		this.groupBox25.PerformLayout();
		this.tabPage14.ResumeLayout(false);
		this.tabPage14.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
