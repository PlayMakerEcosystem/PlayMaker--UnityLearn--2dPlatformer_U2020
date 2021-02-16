using System;

using PlayMaker.ConditionalExpression;
using HutongGames.PlayMakerEditor;

using HutongGames.PlayMaker.Ecosystem.Utils;

namespace HutongGames.PlayMaker.Ecosystem.Utils
{

	public class LinkerEditorChecks {


		public static void CheckUsedActions()
		{
			
			if ( HutongGames.PlayMakerEditor.Actions.GetUsageCount(typeof(ConditionalExpression)) >0 )
			{
				LinkerData.RegisterClassDependancy(typeof(ConditionalExpression),typeof(ConditionalExpression).FullName);
			}

		}

	}
}