using System;
using BoardControl;
using System.Xml;
using System.Collections;

namespace Draughts
{
	public class DraughtsPiece : BasicGamePiece
	{
		private bool bIsLightPiece;
		private bool bIsKing;
		private string strPlayer;
		private ArrayList arrayMoves;

		public bool LightPiece
		{
			get
			{
				return bIsLightPiece;
			}
			set
			{
				bIsLightPiece = value;
			}
		}

		public bool IsKing
		{
			get
			{
				return bIsKing;
			}
			set
			{
				bIsKing = value;
			}
		}

		public string Player
		{
			get
			{
				return strPlayer;
			}
			set
			{
				strPlayer = value;
			}
		}

		public ArrayList Moves
		{
			get
			{
				return arrayMoves;
			}
		}

		public DraughtsPiece()
		{
			LightPiece = false;
			IsKing = false;
			arrayMoves = new ArrayList();
			Player = null;
		}

		public DraughtsPiece( DraughtsPiece piece ) : base( piece )
		{
			LightPiece = piece.LightPiece;
			IsKing = piece.IsKing;
			arrayMoves = new ArrayList();
			Player = piece.Player;
		}


		public DraughtsPiece( string squareIdentifier ) : base( squareIdentifier )
		{
			LightPiece = false;
			arrayMoves = new ArrayList();
			Player = null;
		}

		public DraughtsPiece( string squareIdentifier, string player ) : base( squareIdentifier )
		{
			LightPiece = false;
			arrayMoves = new ArrayList();
			Player = player;
		}

		public DraughtsPiece( string squareIdentifier, bool isLightPiece ) : this( squareIdentifier )
		{
			LightPiece = isLightPiece;
		}

		public DraughtsPiece( string squareIdentifier, string player, bool isLightPiece ) : this( squareIdentifier, player )
		{
			LightPiece = isLightPiece;
		}

		public DraughtsPiece( string squareIdentifier, bool isLightPiece, bool isKing ) : this( squareIdentifier, isLightPiece )
		{
			IsKing = isKing;
		}

		public DraughtsPiece( string squareIdentifier, string player, bool isLightPiece, bool isKing ) : this( squareIdentifier, player, isLightPiece )
		{
			IsKing = isKing;
		}

		public override void Save( XmlWriter xmlWriter )
		{
			xmlWriter.WriteStartElement( "DraughtsPiece" );
			base.Save( xmlWriter );
			xmlWriter.WriteElementString( "Player", Player );
			xmlWriter.WriteElementString( "AvailableMoves", arrayMoves.Count.ToString() );
			for( int i=0; i<arrayMoves.Count; i++ )
			{
				( ( DraughtsMove )arrayMoves[ i ] ).Save( xmlWriter );
			}
			xmlWriter.WriteEndElement();
		}

		public override void Load( XmlReader xmlReader )
		{
			base.Load( xmlReader );
			while( xmlReader.Name != "Player" )
			{
				xmlReader.Read();
			}

			xmlReader.Read();

			Player = xmlReader.Value;

			while( xmlReader.Name != "AvailableMoves" )
			{
				xmlReader.Read();
			}

			xmlReader.Read();

			int nCount = Int32.Parse( xmlReader.Value );

			for( int i=0; i<nCount; i++ )
			{
				DraughtsMove move = new DraughtsMove();

				while( xmlReader.Name != "DraughtsMove" )
				{
					xmlReader.Read();
				}

				move.Load( xmlReader );
				arrayMoves.Add( move );
			}
		}

		public bool IsMoveInList( string move )
		{
			if( arrayMoves == null )
				return false;

			for( int i=0; i<arrayMoves.Count; i++ )
			{
				if( move == ( ( DraughtsMove )arrayMoves[ i ] ).Identifier )
					return true;
			}

			return false;
		}

		public void AddMove( string move )
		{
			if( arrayMoves == null )
				return;

			if( IsMoveInList( move ) == false )
				arrayMoves.Add( new DraughtsMove( move ) );
			else /// move exists so update it
			{
				for( int i=0; i<arrayMoves.Count; i++ )
				{
					if( ( ( DraughtsMove )arrayMoves[ i ] ).Identifier == move )
						( ( DraughtsMove )arrayMoves[ i ] ).TimesUsed++;
				}
			}
		}

		/// Comparison operators Note moves are not compared
		/// 
		


		/// <summary>
		/// 
		/// </summary>
		/// <param name="pieceOne"></param>
		/// <param name="pieceTwo"></param>
		/// <returns></returns>
		public static bool operator == ( DraughtsPiece pieceOne, DraughtsPiece pieceTwo )
		{
			bool bOneIsNull = false;
			bool bBothAreNull = false;

			try
			{
				bool bTest = pieceOne.LightPiece;
			}
			catch( NullReferenceException nullRefExp )
			{
				string strTemp = nullRefExp.Message;

				bOneIsNull = true;
			}

			try
			{
				bool bTest = pieceTwo.LightPiece;
			}
			catch( NullReferenceException nullRefExp )
			{
				string strTemp = nullRefExp.Message;

				if( bOneIsNull == true )
				{
					bBothAreNull = true;
				}
				else 
					bOneIsNull = true;
			}

			if( bOneIsNull == true )
				return false;

			if( bBothAreNull == true )
				return true;

			if( pieceOne.Player != pieceTwo.Player )
				return false;

			if( pieceOne.LightPiece == pieceTwo.LightPiece )
			{
				if( ( BasicGamePiece )pieceOne == ( BasicGamePiece )pieceTwo )
				{
					if( pieceOne.Moves.Count == pieceTwo.Moves.Count )
					{
						for( int i=0; i<pieceOne.Moves.Count; i++ )
						{
							if( ( DraughtsMove )pieceOne.Moves[ i ] != ( DraughtsMove )pieceTwo.Moves[ i ] )
								return false;
						}
					}
					else
						return false;

					return true;
				}
				else
					return false;
			}
		
			return false;

		}

		public static bool operator != ( DraughtsPiece pieceOne, DraughtsPiece pieceTwo )
		{
			bool bOneIsNull = false;
			bool bBothAreNull = false;

			try
			{
				bool bTest = pieceOne.LightPiece;
			}
			catch( NullReferenceException nullRefExp )
			{
				string strTemp = nullRefExp.Message;

				bOneIsNull = true;
			}

			try
			{
				bool bTest = pieceTwo.LightPiece;	
			}
			catch( NullReferenceException nullRefExp )
			{
				string strTemp = nullRefExp.Message;

				if( bOneIsNull == true )
					bBothAreNull = true;
				else
					bOneIsNull = true;
			}

			if( bOneIsNull == true && bBothAreNull == false )
				return true;

			if( bBothAreNull == true )
				return false;

			if( pieceOne.Player == pieceTwo.Player )
				return false;

			if( pieceOne.LightPiece != pieceTwo.LightPiece )
			{
				if( ( BasicGamePiece )pieceOne != ( BasicGamePiece )pieceTwo )
				{
					if( pieceOne.Moves.Count == pieceTwo.Moves.Count )
					{
						for( int i=0; i<pieceOne.Moves.Count; i++ )
						{
							if( ( DraughtsMove )pieceOne.Moves[ i ] == ( DraughtsMove )pieceTwo.Moves[ i ] )
								return false;
						}
					}
					else
						return false;

					return true;
				}
				else
					return false;
			}

			return false;
		}	


		public override bool Equals(object obj)
		{
			if( obj == null && GetType() != obj.GetType() )
				return false;

			DraughtsPiece piece = ( DraughtsPiece )obj;

			return this == piece;
		}

		public override int GetHashCode()
		{
			return LightPiece.GetHashCode() ^ base.GetHashCode ();
		}
	}


	/// <summary>
	/// Summary description for DraughtsPattern.
	/// </summary>
	public class DraughtsPattern : BasicGamePattern
	{
		private int nMoveNumber;

		public int MoveNumber
		{
			get
			{
				return nMoveNumber;
			}
			set
			{
				nMoveNumber = value;
			}
		}


		public DraughtsPattern() : base()
		{
			nMoveNumber = 0;
			//
			// TODO: Add constructor logic here
			//
		}

		public DraughtsPattern( int numberOfTimesSeen ) : base( numberOfTimesSeen )
		{
			nMoveNumber = 0;
		}

		public DraughtsPattern( int numberOfTimesSeen, int weighting ) : base( numberOfTimesSeen, weighting )
		{
			nMoveNumber = 0;
		}

		/// <summary>
		/// do not call base copy constructor from here as it will cast the
		/// internal game pieces to basic game pieces and cock everything up.
		/// </summary>
		/// <param name="patternSet"></param>
		public DraughtsPattern( DraughtsPattern pattern ) : base()
		{
			NumberOfTimesSeen = pattern.NumberOfTimesSeen;
			NumberOfTimesSeenInWinningGame = pattern.NumberOfTimesSeenInWinningGame;
			NumberOfTimesSeenInLosingGame = pattern.NumberOfTimesSeenInLosingGame;
			IsWinningPattern = pattern.IsWinningPattern;
			IsLosingPattern = pattern.IsLosingPattern;
			Weighting = pattern.Weighting;
			Response = pattern.Response;
			PatternID = pattern.PatternID;
			IsEndingPattern = pattern.IsEndingPattern;
			MoveNumber = pattern.MoveNumber;

			for( int i=0; i<pattern.GamePieces.Count; i++ )
			{
				GamePieces.Add( ( DraughtsPiece )pattern.GamePieces[ i ] );
			}
		}

		public void AddGamePiece( DraughtsPiece piece )
		{
			GamePieces.Add( piece );
		}


		public bool IsPieceInPattern( DraughtsPiece piece )
		{
			for( int i=0; i<GamePieces.Count; i++ )
			{
				if( piece == ( DraughtsPiece )GamePieces[ i ] )
					return true;
			}

			return false;
		}

		public bool IsPieceInPattern( string piece, int moveNumber )
		{
			if( moveNumber != nMoveNumber )
				return false;

			for( int i=0; i<GamePieces.Count; i++ )
			{
				if( piece == ( ( DraughtsPiece )GamePieces[ i ] ).SquareIdentifier )
					return true;
			}

			return false;
		}

		public bool AddMoveToPiece( DraughtsPiece piece, string move )
		{
			for( int i=0; i<GamePieces.Count; i++ )
			{
				if( piece.SquareIdentifier == ( ( DraughtsPiece )GamePieces[ i ] ).SquareIdentifier )
				{
					( ( DraughtsPiece )GamePieces[ i ] ).AddMove( move );
					return true;
				}
			}

			return false;
		}

		public DraughtsPiece GetPiece( string identifier )
		{
			for( int i=0; i<GamePieces.Count; i++ )
			{
				if( identifier == ( ( DraughtsPiece )GamePieces[ i ] ).SquareIdentifier )
				{
					return ( DraughtsPiece )GamePieces[ i ];
				}
			}

			return null;
		}

		/// <summary>
		/// rewrite the get starts with piece to return draughts piece
		/// </summary>
		/// <returns></returns>
		private new DraughtsPiece GetStartsWithPiece()
		{
			for( int i=0; i<GamePieces.Count; i++ )
			{
				if( ( ( BasicGamePiece )GamePieces[ i ] ).IsStartForPattern == true )
					return ( DraughtsPiece )GamePieces[ i ];
			}

			return null;
		}

		public DraughtsPiece GetStartsWithPiece( int moveNumber )
		{
			if( moveNumber != nMoveNumber )
				return null;

			for( int i=0; i<GamePieces.Count; i++ )
			{
				if( ( ( BasicGamePiece )GamePieces[ i ] ).IsStartForPattern == true )
					return ( DraughtsPiece )GamePieces[ i ];
			}

			return null;
		}

		public override void Save(XmlWriter xmlWriter)
		{
			if( GamePieces.Count == 0 )
				return;

			xmlWriter.WriteStartElement( "DraughtsPattern" );
			xmlWriter.WriteElementString( "MoveNumber", nMoveNumber.ToString() );
			base.Save (xmlWriter);
			xmlWriter.WriteEndElement();
		}

		public override void Load(XmlReader xmlReader)
		{
			while( xmlReader.Name != "MoveNumber" )
			{
				xmlReader.Read();
			}

			xmlReader.Read();

			nMoveNumber = Int32.Parse( xmlReader.Value );

			while( xmlReader.Name != "PatternID" )
			{
				xmlReader.Read();
				if( xmlReader.EOF == true )
					return;
			}

			xmlReader.Read();
			nPatternID = int.Parse( xmlReader.Value );

			bool bBreak = false;
			for( ;; )
			{
				xmlReader.Read();
				switch( xmlReader.NodeType )
				{
					case XmlNodeType.Element:
					{
						switch( xmlReader.Name )
						{
							case "DraughtsPiece":
							{
								DraughtsPiece temp = new DraughtsPiece();
								temp.Load( xmlReader );
								arrayGamePieces.Add( temp );
								break;
							}
							case "NumberOfTimesSeen": bBreak = true; break;
						}
					} break;
				}

				if( bBreak == true )
					break;
			}

			/// should be on Number of times seen but doesn't hurt to check
			if( xmlReader.Name != "NumberOfTimesSeen" )
				return;

			xmlReader.Read();
			nNumberOfTimesSeen = int.Parse( xmlReader.Value );

			while( xmlReader.Name != "NumberOfTimesSeenInWinningGame" )
			{
				xmlReader.Read();
				if( xmlReader.EOF == true )
					return;
			}

			xmlReader.Read();
			nNumberOfTimesSeenInWinningGame = int.Parse( xmlReader.Value );

			while( xmlReader.Name != "NumberOfTimesSeenInLosingGame" )
			{
				xmlReader.Read();
				if( xmlReader.EOF == true )
					return;
			}

			xmlReader.Read();
			nNumberOfTimesSeenInLosingGame = int.Parse( xmlReader.Value );

			while( xmlReader.Name != "EndingPattern" )
			{
				xmlReader.Read();
				if( xmlReader.EOF == true )
					return;
			}

			xmlReader.Read();
			if( xmlReader.Value == "True" )
				bIsEndingPattern = true;
			else
				bIsEndingPattern = false;

			while( xmlReader.Name != "Weighting" )
			{
				xmlReader.Read();
				if( xmlReader.EOF == true )
					return;
			}

			xmlReader.Read();
			nWeighting = int.Parse( xmlReader.Value );

			while( xmlReader.Name != "Response" )
			{
				xmlReader.Read();
				if( xmlReader.EOF == true )
					return;
			}

			while( xmlReader.Name != "ResponsePresent" )
			{
				xmlReader.Read();
				if( xmlReader.EOF == true )
					return;
			}

			xmlReader.Read();
			int nResponse = int.Parse( xmlReader.Value );

			if( nResponse != 0 )
			{
				bResponsePresent = true;
				bgpResponse = new BasicGamePiece();
				bgpResponse.Load( xmlReader );
			}

		}

		public override bool Equals(object obj)
		{
			return base.Equals (obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode ();
		}
	}

	/// <summary>
	/// Draughts Pattern Collection
	/// </summary>
	public class DraughtsPatternCollection : BasicGamePatternCollection
	{
		public DraughtsPatternCollection() : base()
		{
		}

		public void AddPattern( DraughtsPattern pattern )
		{
			Patterns.Add( new DraughtsPattern( pattern ) );
		}

		/// <summary>
		/// over write the default getpattern to return a draughtspattern
		/// </summary>
		/// <param name="patternID"></param>
		/// <returns></returns>
		public new DraughtsPattern GetPattern( int patternID )
		{
			if( patternID <= Patterns.Count )
			{
				return ( DraughtsPattern )Patterns[ patternID ];
			}

			return null;	
		}

		public DraughtsPattern GetPattern( string square )
		{
			foreach( DraughtsPattern pattern in Patterns )
			{
				if( pattern.GetStartsWithPiece().SquareIdentifier == square )
					return pattern;
			}

			return null;
		}

		public DraughtsPattern GetPattern( string square, int moveNumber )
		{
			foreach( DraughtsPattern pattern in Patterns )
			{
				if( pattern.GetStartsWithPiece().SquareIdentifier == square 
					&& pattern.MoveNumber == moveNumber )
					return pattern;
			}

			return null;
		}

		public bool IsIn( DraughtsPattern pattern )
		{
			for( int i=0; i<Patterns.Count; i++ )
			{
				if( ( ( DraughtsPattern )Patterns[ i ] ) == pattern )
					return true;
			}

			return false;
		}

		public void UpdatePattern( DraughtsPattern pattern )
		{
			string strTemp = pattern.GetStartsWith();

			for( int i=0; i<Patterns.Count; i++ )
			{
				if( ( ( DraughtsPattern )Patterns[ i ] ).StartsWith( strTemp ) == true )
				{
					for( int n=0; n<pattern.GamePieces.Count; n++ )
					{
						DraughtsPiece piece = ( DraughtsPiece )pattern.GamePieces[ n ];

						if( ( ( DraughtsPattern )Patterns[ i ] ).IsPieceInPattern( piece ) == true )
						{
							DraughtsPiece collectionPiece = pattern.GetPiece( piece.SquareIdentifier ); 

							if( collectionPiece != null )
							{
								for( int j=0; j<piece.Moves.Count; j++ )
								{
									/// draughts move will add or update as appropriate
									/// 
									collectionPiece.AddMove( ( ( DraughtsMove )piece.Moves[ j ] ).Identifier );
								}
							}
						}
						else
							( ( DraughtsPattern )Patterns[ i ] ).AddGamePiece( piece );
					}
				}
			}
		}

		public new DraughtsPatternCollection GetAllPatternsWithIdentifer( string identifier )
		{
			DraughtsPatternCollection unit = new DraughtsPatternCollection();

			for( int i=0; i<Patterns.Count; i++ )
			{
				if( ( ( DraughtsPattern )Patterns[ i ] ).StartsWith( identifier ) == true )
				{
					/// note do not use copy constructor here moron.
					/// 

					unit.AddPattern( ( ( DraughtsPattern )Patterns[ i ] ) );
				}
			}

			return unit;
		}

		public override void Save(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement( "DraughtsPatternCollection" );
			base.Save (xmlWriter);
			xmlWriter.WriteEndElement();
		}

		public override void Load(XmlReader xmlReader)
		{
			bool bBreak = false;
			for( ;; )
			{
				xmlReader.Read();
				if( xmlReader.EOF == true )
					return;
				switch( xmlReader.NodeType )
				{
					case XmlNodeType.Element:
					{
						switch( xmlReader.Name )
						{
							case "DraughtsPattern":
							{
								DraughtsPattern temp = new DraughtsPattern();
/*
								while( xmlReader.Name != "MoveNumber" )
								{
									xmlReader.Read();
								}

								xmlReader.Read();

								temp.MoveNumber = Int32.Parse( xmlReader.Value );
*/
								temp.Load( xmlReader );
								Patterns.Add( temp );
								break;
							}
						}
					} break;
					case XmlNodeType.EndElement:
					{
						switch( xmlReader.Name )
						{
							case "BasicGamePatternCollection": bBreak = true; break;
						}
					} break;
				}

				if( bBreak == true )
					break;
			}
		}
	}
	
}
