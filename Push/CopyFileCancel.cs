using System;
using System.Collections;
using System.IO;
//---
using System.ComponentModel;
using System.Threading;
//---
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Push
{
	class CopyFileCancel
	{

		public static void CopyCancel_BackGround(object sender, DoWorkEventArgs doWorkEventArgs)
		{
			BackgroundWorker bgWorker = sender as BackgroundWorker;

			// Update Progress bar...
			bgWorker.ReportProgress(100);

			doWorkEventArgs.Result = new Tuple<Helper.commandResult, int, int>(Helper.commandResult.Cancel, 0, 0);
		}

	}
}
