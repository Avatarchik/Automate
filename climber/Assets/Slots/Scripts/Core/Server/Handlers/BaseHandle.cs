using Core.Server.Request;
using Core.Server.Response;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Core.Server.Handlers
{
	public abstract class BaseHandler
	{
        public abstract void DoRequest ();

	}

}
