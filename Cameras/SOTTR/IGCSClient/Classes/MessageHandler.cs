﻿////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part of Injectable Generic Camera System
// Copyright(c) 2019, Frans Bouma
// All rights reserved.
// https://github.com/FransBouma/InjectableGenericCameraSystem
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met :
//
//  * Redistributions of source code must retain the above copyright notice, this
//	  list of conditions and the following disclaimer.
//
//  * Redistributions in binary form must reproduce the above copyright notice,
//    this list of conditions and the following disclaimer in the documentation
//    and / or other materials provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED.IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IGCSClient.NamedPipeSubSystem;
using SD.Tools.Algorithmia.GeneralDataStructures.EventArguments;

namespace IGCSClient.Classes
{
	/// <summary>
	/// Singleton which handles all messages either received or to be send. 
	/// </summary>
	public static class MessageHandlerSingleton
	{
		private static MessageHandler _instance = new MessageHandler();

		/// <summary>Dummy static constructor to make sure threadsafe initialization is performed.</summary>
		static MessageHandlerSingleton() {}

		/// <summary>
		/// Gets the single instance in use in this application
		/// </summary>
		/// <returns></returns>
		public static MessageHandler Instance() => _instance;

	}


	public class MessageHandler
	{
		#region Members
		private NamedPipeServer _pipeServer;
		private NamedPipeClient _pipeClient;
		#endregion

		internal MessageHandler()
		{
			_pipeServer = new NamedPipeServer(ConstantsEnums.NamedPipeName);
			_pipeClient = new NamedPipeClient(ConstantsEnums.NamedPipeName);
			_pipeServer.MessageReceived += _pipeServer_MessageReceived;
			_pipeServer.ClientConnectionEstablished += _pipeServer_ClientConnectionEstablished;
			LogHandlerSingleton.Instance().LogLine("Named pipe enabled.", "System");
		}


		/// <summary>
		/// Sends a setting message. Setting messages have as format: MessageType.Setting | ID | payload
		/// </summary>
		/// <param name="id"></param>
		/// <param name="payload"></param>
		public void SendSettingMessage(byte id, byte[] payload)
		{
			_pipeClient.Send(new IGCSMessage(MessageType.Setting, id, payload));
#if DEBUG
			LogHandlerSingleton.Instance().LogLine("Setting message sent with id: {0}. Length: {1}", "[DEBUG] MessageHandler", id, payload.Length);
#endif
		}


		/// <summary>
		/// Sends a keybinding message. Setting messages have as format: MessageType.KeyBinding | ID | payload
		/// </summary>
		/// <param name="id"></param>
		/// <param name="payload"></param>
		public void SendKeyBindingMessage(byte id, byte[] payload)
		{
			_pipeClient.Send(new IGCSMessage(MessageType.KeyBinding, id, payload));
		}


		private void HandleNamedPipeMessageReceived(ContainerEventArgs<byte[]> e)
		{
#warning >>> IMPLEMENT
		}


		private void _pipeServer_ClientConnectionEstablished(object sender, EventArgs e)
		{
			if(this.ClientConnectionReceivedFunc != null)
			{
				this.ClientConnectionReceivedFunc();
			}
		}


		private void _pipeServer_MessageReceived(object sender, ContainerEventArgs<byte[]> e)
		{
			HandleNamedPipeMessageReceived(e);
		}


		#region Properties
		/// <summary>
		/// Func which is called when a client connection has been established
		/// </summary>
		public Action ClientConnectionReceivedFunc { get; set; }
		#endregion
	}
}