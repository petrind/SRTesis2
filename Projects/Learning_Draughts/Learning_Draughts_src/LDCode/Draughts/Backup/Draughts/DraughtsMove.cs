using System;
using BoardControl;
using System.Xml;

namespace Draughts 
{
	/// <summary>
	/// Summary description for DraughtsMove.
	/// </summary>
	public class DraughtsMove : BasicMove
	{
		protected int nSuccessRate;

		public DraughtsMove()
		{
			//
			// _TODO: Add constructor logic here
			// 
			nSuccessRate = 3;
		}

		public DraughtsMove( string identifier ) : base( identifier )
		{
			nSuccessRate = 3;
		}

		public DraughtsMove( string identifier, int successRate )
		{
			nSuccessRate = successRate;
		}

		public new void Save( XmlWriter xmlWriter )
		{
			xmlWriter.WriteStartElement( "DraughtsMove" );
			base.Save( xmlWriter );
			xmlWriter.WriteEndElement();
		}

		public new void Load( XmlReader xmlReader )
		{
			base.Load( xmlReader );
		}

		public int SuccessRate()
		{
			int nRate = TimesUsedInWinningGame - TimesUsedInLosingGame;

			if( nRate <= nSuccessRate )
				return 1;

			return ( int )nRate/nSuccessRate;
		}
	
		public bool WonMore()
		{
			if( TimesUsedInWinningGame >= TimesUsedInLosingGame )
				return true;

			return false;
		}
	}
}
