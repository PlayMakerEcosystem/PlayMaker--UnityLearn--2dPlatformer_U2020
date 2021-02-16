﻿// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using System;

namespace HutongGames.PlayMaker.Ecosystem.Utils
{

	[AttributeUsage(AttributeTargets.All)]
	public class EventTargetVariable : Attribute
	{
		public string variable;

		public EventTargetVariable(string variable)
		{
			this.variable = variable;
		}
	}

}