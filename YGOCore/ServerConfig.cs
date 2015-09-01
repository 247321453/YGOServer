using System.IO;
using System;
using System.Globalization;
namespace YGOCore
{
	public class ServerConfig
	{
		/// <summary>
		/// ����˿�
		/// </summary>
		public int ServerPort { get; private set; }
		
	//	public string ApiIp{get;private set;}
		/// <summary>
		/// api�˿�
		/// </summary>
		public int ApiPort{get;private set;}
		/// <summary>
		/// ����Ŀ¼
		/// </summary>
		public string Path { get; private set; }
		/// <summary>
		/// �ű�Ŀ¼
		/// </summary>
		public string ScriptFolder { get; private set; }
		/// <summary>
		/// �������ݿ�
		/// </summary>
		public string CardCDB { get; private set; }
		/// <summary>
		/// �������ļ�
		/// </summary>
		public string BanlistFile { get; private set; }
		/// <summary>
		/// log
		/// </summary>
		public bool Log { get; private set; }
		/// <summary>
		/// ����̨��־
		/// </summary>
		public bool ConsoleLog { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public bool HandShuffle { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public bool AutoEndTurn { get; private set; }
		/// <summary>
		/// �ͻ��˰汾
		/// </summary>
		public int ClientVersion { get; private set; }
		
//		/// <summary>
//		/// �����ظ���½
//		/// </summary>
//		public bool RepeatLogin{get;private set;}
		/// <summary>
		/// ��Ҫ����
		/// </summary>
		public bool isNeedAuth{get;private set;}
		/// <summary>
		/// ���ͻ�������
		/// </summary>
		public int MaxRoomCount{get;private set;}
		/// <summary>
		/// �Զ�¼��
		/// </summary>
		public bool AutoReplay{get;private set;}
		/// <summary>
		/// ¼�񱣴��ļ���
		/// </summary>
		public string replayFolder { get; private set; }
		
		public bool RecordWin{get;private set;}
		
		/// <summary>
		/// ��¼�����ʱ�䣨���ӣ�
		/// </summary>
		public int SaveRecordTime{get;private set;}
		/// <summary>
		/// ��¼
		/// </summary>
		public string WinDbName{get;private set;}
		/// <summary>
		/// ��¼�ӿ�
		/// </summary>
		public string LoginUrl{get;private set;}
		
		/// <summary>
		/// ˽��
		/// </summary>
		public bool PrivateChat{get;private set;}
		
		/// <summary>
		/// ���������
		/// </summary>
		public string ServerName{get;private set;}
		
		public ServerConfig()
		{
			ClientVersion = 0x1335;
			ServerPort = 8911;
			ServerName="YGOserver";
		//	ApiIp="127.0.0.1";
			ApiPort=18911;
			Path = ".";
			ScriptFolder = "script";
			replayFolder="replay";
			CardCDB = "cards.cdb";
			BanlistFile = "lflist.conf";
			Log = true;
			//RepeatLogin=true;
			ConsoleLog = true;
			HandShuffle = false;
			AutoEndTurn = true;
			isNeedAuth=false;
			MaxRoomCount=200;
			WinDbName="wins.db";
			RecordWin=false;
			PrivateChat=false;
			SaveRecordTime=1;//
			LoginUrl="http://127.0.0.1/login.php";
		}

		public bool Load(string file = "config.txt")
		{
			if (File.Exists(file))
			{
				StreamReader reader = null;
				try
				{
					reader = new StreamReader(File.OpenRead(file));
					while (!reader.EndOfStream)
					{
						string line = reader.ReadLine();
						if (line == null) continue;
						line = line.Trim();
						if (line.Equals(string.Empty)) continue;
						if (!line.Contains("=")) continue;
						if (line.StartsWith("#")) continue;

						string[] data = line.Split(new[] { '=' }, 2);
						string variable = data[0].Trim().ToLower();
						string value = data[1].Trim();
						setValue(variable, value);
					}
				}
				catch (Exception ex)
				{
					Logger.WriteError(ex);
					reader.Close();
					return false;
				}
				reader.Close();
				return true;
			}

			return false;
		}
		
		public bool setValue(string variable,string value){
			if(string.IsNullOrEmpty(value)||string.IsNullOrEmpty(variable)){
				return false;
			}
			variable=variable.ToLower();
			switch (variable)
			{
				case "servername":
					ServerName=value;
					break;
				case "serverport":
					ServerPort = Convert.ToInt32(value);
					break;
//				case "apiip":
//					ApiIp=value;
//					break;
				case "apiport":
					ApiPort = Convert.ToInt32(value);
					break;
				case "path":
					Path = value;
					break;
				case "scriptfolder":
					ScriptFolder = value;
					break;
				case "cardcdb":
					CardCDB = value;
					break;
				case "banlist":
					BanlistFile = value;
					break;
				case "privatechat":
					PrivateChat=Convert.ToBoolean(value);
					break;
				case "errorlog":
					Log = Convert.ToBoolean(value);
					break;
//				case "repeatlogin":
//					RepeatLogin = Convert.ToBoolean(value);
//					break;
				case "consolelog":
					ConsoleLog = Convert.ToBoolean(value);
					break;
				case "handshuffle":
					HandShuffle = Convert.ToBoolean(value);
					break;
				case "autoendturn":
					AutoEndTurn = Convert.ToBoolean(value);
					break;
				case "clientversion":
					ClientVersion = Convert.ToInt32(value, 16);
					break;
				case "needauth":
					isNeedAuth = (value.ToLower()=="true"||value=="1");
					break;
				case "maxroom":
					MaxRoomCount=Convert.ToInt32(value, 10);
					break;
				case "autoreplay":
					AutoReplay= (value.ToLower()=="true"||value=="1");
					break;
				case "replayfolder":
					replayFolder=value;
					break;
				case "windbname":
					WinDbName=value;
					break;
				case "recordwin":
					RecordWin=(value.ToLower()=="true"||value=="1");
					break;
				case "saverecordtime":
					SaveRecordTime=Convert.ToInt32(value, 1);
					if(SaveRecordTime<=0){
						SaveRecordTime=1;
					}
					break;
				case "loginurl":
					LoginUrl=value;
					break;
				default:
					return false;
			}
			return true;
		}

	}
}
