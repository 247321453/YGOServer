﻿/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2015/11/12
 * 时间: 17:36
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;

namespace YGOCore.Common
{
	/// <summary>
	/// Description of RoomMessage.
	/// </summary>
	public enum RoomMessage : byte
	{
		Error,
		RoomList,
		ServerInfo,
		ClientChat,
		RoomCreate,
		RoomStart,
		RoomClose,
	}
}
