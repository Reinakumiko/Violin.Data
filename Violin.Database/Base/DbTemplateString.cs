using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Violin.Data.Database.Base
{
	public abstract class DbTemplateString
	{
		public abstract string GetTemplate(DbConnectString connString);
	}
}
