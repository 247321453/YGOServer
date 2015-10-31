﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using AsyncServer;

namespace YGOCore.Game
{
	public class GameClient
	{
		public bool IsConnected { get; private set; }
		public Game Game { get; private set; }
		public Player Player { get; private set; }
		private GameRoom m_room;
		private Queue<GameClientPacket> m_recvQueue;
		private Queue<byte[]> m_sendQueue;
		private bool m_disconnected;
		private bool m_closePending;
		public long logintime{get;private set;}

		public GameClient()
		{
			IsConnected = true;
			Player = new Player(this);
			m_recvQueue = new Queue<GameClientPacket>();
			m_sendQueue = new Queue<byte[]>();
			logintime =DateTime.Now.Ticks;
		}

		public void Close()
		{
			if (!IsConnected)
				return;
			IsConnected = false;
			if(InGame())
				m_room.RemoveClient(this);
		}

		public bool InGame()
		{
			return Game != null;
		}
		
		public void SetRoom(GameRoom room)
		{
			if (m_room == null)
			{
				m_room = room;
				Game = m_room.Game;
			}
		}
		
		public void LobbyError(string str){
			if(Player!=null){
				Player.LobbyError(str);
			}
		}

		public void CloseDelayed()
		{
			m_closePending = true;
		}

		public void Send(byte[] raw)
		{
			m_sendQueue.Enqueue(raw);
		}

		public void Tick()
		{
			TickTask();
		}
		
		private void TickTask(){
			if (IsConnected)
			{
				try
				{
					CheckDisconnected();
					NetworkSend();
					NetworkReceive();
				}
				catch (Exception)
				{
					m_disconnected = true;
				}
			}
			if (m_closePending)
			{
				m_disconnected = true;
				Close();
				return;
			}
			if (!m_disconnected)
			{
				try
				{
					NetworkParse();
				}
				catch (Exception ex)
				{
					Logger.Error(ex);
					m_disconnected = true;
				}
			}
			if (m_disconnected)
			{
				Close();
				Player.OnDisconnected();
			}
		}

		private void CheckDisconnected()
		{
//			m_disconnected = (m_client.Available == 0);
		}

		public void NetworkReceive()
		{
//			if (m_client.Available >= 2 && m_receivedLen == -1)
//				m_receivedLen = m_reader.ReadUInt16();
//
//			if (m_receivedLen != -1 && m_client.Available >= m_receivedLen)
//			{
//				GameClientPacket packet = new GameClientPacket(m_reader.ReadBytes(m_receivedLen));
//				m_receivedLen = -1;
//				lock (m_recvQueue)
//					m_recvQueue.Enqueue(packet);
//			}
		}

		private void NetworkSend()
		{
			try{
				while (m_sendQueue.Count > 0)
				{
					byte[] raw = m_sendQueue.Dequeue();
					MemoryStream stream = new MemoryStream(raw.Length + 2);
					BinaryWriter writer = new BinaryWriter(stream);
					writer.Write((ushort)raw.Length);
					writer.Write(raw);
			//		m_client.Client.Send(stream.ToArray());
				}
			}catch{}
		}
		
//		public bool IsTimeOut(){
//			long t = (DateTime.Now.Ticks-logintime)/10000;
//			long all = ((long)Program.Config.Timeout * 1000);
//			return  t >= all;
//		}

		private void NetworkParse()
		{
			int count;
			lock (m_recvQueue)
				count = m_recvQueue.Count;
			while (count > 0)
			{
				GameClientPacket packet = null;
				lock (m_recvQueue)
				{
					if (m_recvQueue.Count > 0)
						packet = m_recvQueue.Dequeue();
					count = m_recvQueue.Count;
				}
				if (packet != null)
					Player.Parse(packet);
			}
		}
	}
}