using System;
using BoardControl;
using System.IO;
using System.Xml;
using Fuzzy_Logic_Library;
using System.Collections;


namespace Draughts
{
	/// <summary>
	/// The Game code for the draughts game
	/// Note in the connect four game class the 
	/// class held information only about pieces on the board
	/// in the draughts game it controls all the game related code
	/// and the information that was stored in the connect four game class
	/// is now kept by the Draughts square class.
	/// </summary>
	public class DraughtsGame : BasicGame
	{
		private string strFileName = "DraughtsPatterns.xml";

		/// <summary>
		/// collection for storing the historical/memory data
		/// </summary>
		private DraughtsPatternCollection historicalPatterns;

		/// <summary>
		/// collection of the patterns used for this game
		/// </summary>
		private DraughtsPatternCollection gamePatterns;

		/// <summary>
		/// collection of currently availalble patterns for the current move.
		/// </summary>
		private DraughtsPatternCollection availablePatterns;

		/// <summary>
		/// collection for holding the fuzzy descisions
		/// </summary>
		private FuzzyDecisionSetCollection collection;

		private DraughtsBoard board;

		private DraughtsProcessMoves processMoves;

		/// values 
		/// 
		private const int nSacrifice = 2;
		/// <summary>
		/// need to ensure that the must take direction wins
		/// </summary>
		private const int nMustTake = 3;

		/// values to set which decision set should be used
		/// 
		private const int nNormalMove = 1;
		private const int nHistoricalMove = 1;
		private const int nTakenMove = 2;
		private const int nSacrificeMove = 1;
		private const int nSupportMove = 1;
		private const int nKingMove = 1;
		private const int nLeavingOpenMove = 2;
		private const int nOutOfWayMove = 1;
		private const int nMustTakeTakenReduction = 1;
		private const int nKingInfluenceMove = 1;

		/// <summary>
		/// what move is this
		/// </summary>
		private int nMoveNumber;

		private bool bCannotMove = false;

		/// <summary>
		/// has the computer taken a piece
		/// </summary>
		private bool bPieceTaken;

		public bool CannotMove
		{
			get
			{
				return bCannotMove;
			}
			set
			{
				bCannotMove = value;
			}
		}

		public bool PieceTaken
		{
			get
			{
				return bPieceTaken;
			}
			set
			{
				bPieceTaken = value;
			}
		}


		public DraughtsBoard Board
		{
			set
			{
				board = value;
			}
		}


		public DraughtsGame()
		{
			//
			// _TODO: Add constructor logic here
			//

			historicalPatterns = new DraughtsPatternCollection();
			gamePatterns = new DraughtsPatternCollection();
			availablePatterns = new DraughtsPatternCollection();
			collection = new FuzzyDecisionSetCollection( "Draughts Decisions" );
			processMoves = new DraughtsProcessMoves();
			nMoveNumber = 0;
			bPieceTaken = false;
			
		}

		/// <summary>
		/// Load the saved patterns from previous games
		/// </summary>
		private void LoadHistoricalPatterns()
		{
			/// will certainly fail the first time run and possibly on occaision thereafter 
			try
			{
				if( File.Exists( strFileName ) == false )
					return;

				StreamReader reader = new StreamReader( strFileName );
				XmlTextReader xmlReader = new XmlTextReader( reader );

				historicalPatterns.Load( xmlReader );

				xmlReader.Close();
			}
			catch( ArgumentNullException argNullExp )
			{
				throw new Exception( argNullExp.Message );
			}
			catch( ArgumentException argExp )
			{
				throw new Exception( argExp.Message );
			}
			catch( FileNotFoundException fileNFExp )
			{
				throw new Exception( fileNFExp.Message );
			}
			catch( DirectoryNotFoundException dirNFExp )
			{
				throw new Exception( dirNFExp.Message );
			}
			catch( IOException ioExp )
			{
				throw new Exception( ioExp.Message );
			}
			catch( XmlException xmlExp )
			{
				throw new Exception( xmlExp.Message );
			}

		}

		/// <summary>
		/// save the patterns to a file. Called whenever the game is won.
		/// </summary>
		private void SavePatterns()
		{
			try
			{
				if( File.Exists( strFileName ) == true )
				{
					File.Delete( strFileName );
				}
			}
			catch( ArgumentNullException argNullExp )
			{
				throw new Exception( "Error removing the file " + strFileName + " Due to " + argNullExp.Message + " If error persists delete the file and start again" );
			}
			catch( ArgumentException argExp )
			{
				throw new Exception( "Error removing the file " + strFileName + " Due to " + argExp.Message + " If error persists delete the file and start again" );
			}
			catch( UnauthorizedAccessException unAccessExp )
			{
				throw new Exception( "Error removing the file " + strFileName + " Due to " + unAccessExp.Message + " If error persists delete the file and start again" );
			}
			catch( PathTooLongException pathExp )
			{
				throw new Exception( "Error removing the file " + strFileName + " Due to " + pathExp.Message + " If error persists delete the file and start again" );
			}
			catch( DirectoryNotFoundException dirNFExp )
			{
				throw new Exception( "Error removing the file " + strFileName + " Due to " + dirNFExp.Message + " If error persists delete the file and start again" );
			}
			catch( IOException ioExp )
			{
				throw new Exception( "Error removing the file " + strFileName + " Due to " + ioExp.Message + " If error persists delete the file and start again" );
			}


			StreamWriter writer = File.CreateText( strFileName );
			XmlTextWriter xmlWriter = new XmlTextWriter( writer );

			historicalPatterns.Save( xmlWriter );

			xmlWriter.Close();
		}

		/// <summary>
		/// Initialise the game setup. ( just prefer doing it this way than through the constructor )
		/// </summary>
		public void InitializeGame()
		{
			try
			{
				LoadHistoricalPatterns();
			}
			catch( Exception exp )
			{
				throw exp;
			}
		}

		public void ResetGame()
		{
			collection.Clear();
			availablePatterns.Clear();
			nMoveNumber = 0;
			bCannotMove = false;
			gamePatterns.Clear();
		}

		/// <summary>
		/// Get the currently available moves.
		/// </summary>
		/// <param name="board"></param>
		/// <param name="light"></param>
		public void GetAvailablePieces( bool light )
		{
			if( availablePatterns == null )
				throw new Exception( "Everything's screwed" );

			availablePatterns.Clear();
			nMoveNumber++;

			/// get all moveable pieces 
			/// 

			foreach( DictionaryEntry dicEnt in board.GetHashTable )
			{
				DraughtsSquare square = ( DraughtsSquare )dicEnt.Value;

				if( square.IsOccupied == true )
				{
					DraughtsPattern pattern = new DraughtsPattern();

					pattern.MoveNumber = nMoveNumber;

					/// is it a computer piece
					/// 
					if( square.PlayerIsOnSquare == false )
					{
						/// work out which direction we are going in
						/// 

						/// Player is playing up the board
						/// computer is playing down
						/// 
						if( board.PlayerIsLight == true )
						{
							DraughtsSquare tempSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierBelowLeft( square.Identifier ) ];

							if( tempSquare != null )
							{
								/// valid square
								/// 
								if( tempSquare.IsOccupied == false )
								{
									DraughtsPiece piece = new DraughtsPiece( square.Identifier, "COMPUTER", false, square.IsKing );

									if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
									{
										if( pattern.GamePieces.Count == 0 )
											piece.IsStartForPattern = true;
										piece.AddMove( tempSquare.Identifier );
										pattern.AddGamePiece( piece );
									}
									else
									{
										pattern.AddMoveToPiece( piece, tempSquare.Identifier );
									}

								}
								else
								{
									/// check if it's a possible take piece
									/// 
									if( tempSquare.IsOccupied == true 
										&& square.OccupyingName != tempSquare.OccupyingName )
									{
										DraughtsSquare checkSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierBelowLeft( tempSquare.Identifier ) ];

										if( checkSquare != null && checkSquare.IsOccupied == false )
										{
											DraughtsPiece piece = new DraughtsPiece( square.Identifier, "COMPUTER", false, square.IsKing );

											if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
											{
												if( pattern.GamePieces.Count == 0 )
													piece.IsStartForPattern = true;
												piece.AddMove( checkSquare.Identifier );
												pattern.AddGamePiece( piece );
											}
											else
											{
												pattern.AddMoveToPiece( piece, tempSquare.Identifier );
											}
										}
									}
								}
							}

							tempSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierBelowRight( square.Identifier ) ];

							if( tempSquare != null )
							{
								if( tempSquare.IsOccupied == false )
								{
									DraughtsPiece piece = new DraughtsPiece( square.Identifier, "COMPUTER", false, square.IsKing );
									if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
									{
										if( pattern.GamePieces.Count == 0 )
											piece.IsStartForPattern = true;
										piece.AddMove( tempSquare.Identifier );
										pattern.AddGamePiece( piece );
									}
									else
									{
										pattern.AddMoveToPiece( piece, tempSquare.Identifier );
									}
								}
								else
								{
									if( tempSquare.IsOccupied == true 
										&& square.OccupyingName != tempSquare.OccupyingName )
									{
										DraughtsSquare checkSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierBelowRight( tempSquare.Identifier ) ];

										if( checkSquare != null && checkSquare.IsOccupied == false )
										{
											DraughtsPiece piece = new DraughtsPiece( square.Identifier, "COMPUTER", false, square.IsKing );
											if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
											{
												if( pattern.GamePieces.Count == 0 )
													piece.IsStartForPattern = true;
												piece.AddMove( checkSquare.Identifier );
												pattern.AddGamePiece( piece );
											}
											else
											{
												pattern.AddMoveToPiece( piece, checkSquare.Identifier );
											}
										}
									}
								}
							}

							if( square.IsKing == true )
							{
								tempSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierAboveLeft( square.Identifier ) ];

								if( tempSquare != null )
								{
									if( tempSquare.IsOccupied == false )
									{
										DraughtsPiece piece = new DraughtsPiece( square.Identifier, "COMPUTER", false, square.IsKing );
										if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
										{
											if( pattern.GamePieces.Count == 0 )
												piece.IsStartForPattern = true;
											piece.AddMove( tempSquare.Identifier );
											pattern.AddGamePiece( piece );
										}
										else
										{
											pattern.AddMoveToPiece( piece, tempSquare.Identifier );
										}
									}
									else
									{
										if( tempSquare.IsOccupied == true 
											&& square.OccupyingName != tempSquare.OccupyingName )
										{
											DraughtsSquare checkSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierAboveLeft( tempSquare.Identifier ) ];

											if( checkSquare != null && checkSquare.IsOccupied == false )
											{
												DraughtsPiece piece = new DraughtsPiece( square.Identifier, "COMPUTER", false, square.IsKing );
												if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
												{
													if( pattern.GamePieces.Count == 0 )
														piece.IsStartForPattern = true;
													piece.AddMove( checkSquare.Identifier );
													pattern.AddGamePiece( piece );
												}
												else
												{
													pattern.AddMoveToPiece( piece, checkSquare.Identifier );
												}
											}
										}
									}
								}

								tempSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierAboveRight( square.Identifier ) ];

								if( tempSquare != null )
								{
									if( tempSquare.IsOccupied == false )
									{
										DraughtsPiece piece = new DraughtsPiece( square.Identifier, "COMPUTER", false, square.IsKing );
										if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
										{
											if( pattern.GamePieces.Count == 0 )
												piece.IsStartForPattern = true;
											piece.AddMove( tempSquare.Identifier );
											pattern.AddGamePiece( piece );
										}
										else
										{
											pattern.AddMoveToPiece( piece, tempSquare.Identifier );
										}
									}
									else
									{
										if( tempSquare.IsOccupied == true 
											&& square.OccupyingName != tempSquare.OccupyingName )
										{
											DraughtsSquare checkSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierAboveRight( tempSquare.Identifier ) ];

											if( checkSquare != null && checkSquare.IsOccupied == false )
											{
												DraughtsPiece piece = new DraughtsPiece( square.Identifier, "COMPUTER", false, square.IsKing );
												if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
												{
													if( pattern.GamePieces.Count == 0 )
														piece.IsStartForPattern = true;
													piece.AddMove( checkSquare.Identifier );
													pattern.AddGamePiece( piece );
												}
												else
												{
													pattern.AddMoveToPiece( piece, checkSquare.Identifier );
												}
											}
										}
									}
								}
							}
						}
						else
						{
							/// player is playing down the board computer is playing up
							/// 

							DraughtsSquare tempSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierAboveLeft( square.Identifier ) ];

							if( tempSquare != null )
							{
								if( tempSquare.IsOccupied == false )
								{
									DraughtsPiece piece = new DraughtsPiece( square.Identifier, "COMPUTER", true, square.IsKing );
									if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
									{
										if( pattern.GamePieces.Count == 0 )
											piece.IsStartForPattern = true;
										piece.AddMove( tempSquare.Identifier );
										pattern.AddGamePiece( piece );
									}
									else
									{
										pattern.AddMoveToPiece( piece, tempSquare.Identifier );
									}
								}
								else
								{
									if( tempSquare.IsOccupied == true 
										&& square.OccupyingName != tempSquare.OccupyingName )
									{
										DraughtsSquare checkSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierAboveLeft( tempSquare.Identifier ) ];

										if( checkSquare != null && checkSquare.IsOccupied == false )
										{
											DraughtsPiece piece = new DraughtsPiece( square.Identifier, "COMPUTER", true, square.IsKing );
											if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
											{
												if( pattern.GamePieces.Count == 0 )
													piece.IsStartForPattern = true;
												piece.AddMove( checkSquare.Identifier );
												pattern.AddGamePiece( piece );
											}
											else
											{
												pattern.AddMoveToPiece( piece, checkSquare.Identifier );
											}
										}
									}
								}
							}

							tempSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierAboveRight( square.Identifier ) ];

							if( tempSquare != null )
							{
								if( tempSquare.IsOccupied == false )
								{
									DraughtsPiece piece = new DraughtsPiece( square.Identifier, "COMPUTER", true, square.IsKing );
									if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
									{
										if( pattern.GamePieces.Count == 0 )
											piece.IsStartForPattern = true;
										piece.AddMove( tempSquare.Identifier );
										pattern.AddGamePiece( piece );
									}
									else
									{
										pattern.AddMoveToPiece( piece, tempSquare.Identifier );
									}
								}
								else
								{
									if( tempSquare.IsOccupied == true 
										&& square.OccupyingName != tempSquare.OccupyingName )
									{
										DraughtsSquare checkSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierAboveRight( tempSquare.Identifier ) ];

										if( checkSquare != null && checkSquare.IsOccupied == false )
										{
											DraughtsPiece piece = new DraughtsPiece( square.Identifier, "COMPUTER", true, square.IsKing );
											if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
											{
												if( pattern.GamePieces.Count == 0 )
													piece.IsStartForPattern = true;
												piece.AddMove( checkSquare.Identifier );
												pattern.AddGamePiece( piece );
											}
											else
											{
												pattern.AddMoveToPiece( piece, checkSquare.Identifier );
											}
										}
									}
								}
							}

							if( square.IsKing == true )
							{
								tempSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierBelowLeft( square.Identifier ) ];

								if( tempSquare != null )
								{
									if( tempSquare.IsOccupied == false )
									{
										DraughtsPiece piece = new DraughtsPiece( square.Identifier, "COMPUTER", true, square.IsKing );
										if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
										{
											if( pattern.GamePieces.Count == 0 )
												piece.IsStartForPattern = true;
											piece.AddMove( tempSquare.Identifier );
											pattern.AddGamePiece( piece );
										}
										else
										{
											pattern.AddMoveToPiece( piece, tempSquare.Identifier );
										}
									}
									else
									{
										if( tempSquare.IsOccupied == true 
											&& square.OccupyingName != tempSquare.OccupyingName )
										{
											DraughtsSquare checkSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierBelowLeft( tempSquare.Identifier ) ];

											if( checkSquare != null && checkSquare.IsOccupied == false )
											{
												DraughtsPiece piece = new DraughtsPiece( square.Identifier, "COMPUTER", true, square.IsKing );
												if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
												{
													if( pattern.GamePieces.Count == 0 )
														piece.IsStartForPattern = true;
													piece.AddMove( checkSquare.Identifier );
													pattern.AddGamePiece( piece );
												}
												else
												{
													pattern.AddMoveToPiece( piece, tempSquare.Identifier );
												}
											}
										}
									}
								}

								tempSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierBelowRight( square.Identifier ) ];

								if( tempSquare != null )
								{
									if( tempSquare.IsOccupied == false )
									{
										DraughtsPiece piece = new DraughtsPiece( square.Identifier, "COMPUTER", true, square.IsKing );
										if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
										{
											if( pattern.GamePieces.Count == 0 )
												piece.IsStartForPattern = true;
											piece.AddMove( tempSquare.Identifier );
											pattern.AddGamePiece( piece );
										}
										else
										{
											pattern.AddMoveToPiece( piece, tempSquare.Identifier );
										}
									}
									else
									{
										if( tempSquare.IsOccupied == true 
											&& square.OccupyingName != tempSquare.OccupyingName )
										{
											DraughtsSquare checkSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierBelowRight( tempSquare.Identifier ) ];

											if( checkSquare != null && checkSquare.IsOccupied == false )
											{
												DraughtsPiece piece = new DraughtsPiece( square.Identifier, "COMPUTER", true, square.IsKing );
												if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
												{
													if( pattern.GamePieces.Count == 0 )
														piece.IsStartForPattern = true;
													piece.AddMove( checkSquare.Identifier );
													pattern.AddGamePiece( piece );
												}
												else
												{
													pattern.AddMoveToPiece( piece, checkSquare.Identifier );
												}
											}
										}
									}
								}
							}
						}

						if( pattern.Count > 0 )
						{
							availablePatterns.AddPattern( pattern );
						}
					}
					else /// this is a player piece ( Player is on square == true )
					{
						/// player is playing up the board
						/// computer is playing down
						/// 
						if( board.PlayerIsLight == true )
						{
							DraughtsSquare tempSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierAboveLeft( square.Identifier ) ];

							if( tempSquare != null )
							{
								/// valid square
								/// 
								if( tempSquare.IsOccupied == false )
								{
									DraughtsPiece piece = new DraughtsPiece( square.Identifier, "PLAYER", true, square.IsKing );

									if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
									{
										if( pattern.GamePieces.Count == 0 )
											piece.IsStartForPattern = true;
										piece.AddMove( tempSquare.Identifier );
										pattern.AddGamePiece( piece );
									}
									else
									{
										pattern.AddMoveToPiece( piece, tempSquare.Identifier );
									}
								}
								else
								{
									/// check if it's a possible take piece
									/// 
									if( tempSquare.IsOccupied == true 
										&& square.OccupyingName != tempSquare.OccupyingName )
									{
										DraughtsSquare checkSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierAboveLeft( tempSquare.Identifier ) ];

										if( checkSquare != null && checkSquare.IsOccupied == false )
										{
											DraughtsPiece piece = new DraughtsPiece( square.Identifier, "PLAYER", true, square.IsKing );

											if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
											{
												if( pattern.GamePieces.Count == 0 )
													piece.IsStartForPattern = true;
												piece.AddMove( checkSquare.Identifier );
												pattern.AddGamePiece( piece );
											}
											else
											{
												pattern.AddMoveToPiece( piece, checkSquare.Identifier );
											}
										}
									}
								}
							}

							tempSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierAboveRight( square.Identifier ) ];

							if( tempSquare != null )
							{
								if( tempSquare.IsOccupied == false )
								{
									DraughtsPiece piece = new DraughtsPiece( square.Identifier, "PLAYER", true, square.IsKing );
									if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
									{
										if( pattern.GamePieces.Count == 0 )
											piece.IsStartForPattern = true;
										piece.AddMove( tempSquare.Identifier );
										pattern.AddGamePiece( piece );
									}
									else
									{
										pattern.AddMoveToPiece( piece, tempSquare.Identifier );
									}
								}
								else
								{
									if( tempSquare.IsOccupied == true 
										&& square.OccupyingName != tempSquare.OccupyingName )
									{
										DraughtsSquare checkSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierAboveRight( tempSquare.Identifier ) ];

										if( checkSquare != null && checkSquare.IsOccupied == false )
										{
											DraughtsPiece piece = new DraughtsPiece( square.Identifier, "PLAYER", true, square.IsKing );
										
											if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
											{
												if( pattern.GamePieces.Count == 0 )
													piece.IsStartForPattern = true;
												piece.AddMove( checkSquare.Identifier );
												pattern.AddGamePiece( piece );
											}
											else
											{
												pattern.AddMoveToPiece( piece, checkSquare.Identifier );
											}
										}
									}
								}
							}

							if( square.IsKing == true )
							{
								tempSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierBelowLeft( square.Identifier ) ];

								if( tempSquare != null )
								{
									if( tempSquare.IsOccupied == false )
									{
										DraughtsPiece piece = new DraughtsPiece( square.Identifier, "PLAYER", true, square.IsKing );
									
										if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
										{
											if( pattern.GamePieces.Count == 0 )
												piece.IsStartForPattern = true;
											piece.AddMove( tempSquare.Identifier );
											pattern.AddGamePiece( piece );
										}
										else
										{
											pattern.AddMoveToPiece( piece, tempSquare.Identifier );
										}
									}
									else
									{
										if( square.OccupyingName != tempSquare.OccupyingName )
										{
											DraughtsSquare checkSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierBelowLeft( tempSquare.Identifier ) ];

											if( checkSquare != null && checkSquare.IsOccupied == false )
											{
												DraughtsPiece piece = new DraughtsPiece( square.Identifier, "PLAYER", true, square.IsKing );
											
												if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
												{
													if( pattern.GamePieces.Count == 0 )
														piece.IsStartForPattern = true;
													piece.AddMove( checkSquare.Identifier );
													pattern.AddGamePiece( piece );
												}
												else
												{
													pattern.AddMoveToPiece( piece, checkSquare.Identifier );
												}
											}
										}
									}
								}

								tempSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierBelowRight( square.Identifier ) ];

								if( tempSquare != null )
								{
									if( tempSquare.IsOccupied == false )
									{
										DraughtsPiece piece = new DraughtsPiece( square.Identifier, "PLAYER", true, square.IsKing );

										if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
										{
											if( pattern.GamePieces.Count == 0 )
												piece.IsStartForPattern = true;
											piece.AddMove( tempSquare.Identifier );
											pattern.AddGamePiece( piece );
										}
										else
										{
											pattern.AddMoveToPiece( piece, tempSquare.Identifier );
										}
									}
									else
									{
										if( square.OccupyingName != tempSquare.OccupyingName )
										{
											DraughtsSquare checkSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierBelowRight( tempSquare.Identifier ) ];

											if( checkSquare != null && checkSquare.IsOccupied == false )
											{
												DraughtsPiece piece = new DraughtsPiece( square.Identifier, "PLAYER", true, square.IsKing );

												if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
												{
													if( pattern.GamePieces.Count == 0 )
														piece.IsStartForPattern = true;
													piece.AddMove( checkSquare.Identifier );
													pattern.AddGamePiece( piece );
												}
												else
												{
													pattern.AddMoveToPiece( piece, checkSquare.Identifier );
												}
											}
										}
									}
								}	
							}			
						}
						else
						{
							/// player is playing down the board
							/// 

							DraughtsSquare tempSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierBelowLeft( square.Identifier ) ];

							if( tempSquare != null )
							{
								if( tempSquare.IsOccupied == false )
								{
									DraughtsPiece piece = new DraughtsPiece( square.Identifier, "PLAYER", false, square.IsKing );

									if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
									{
										if( pattern.GamePieces.Count == 0 )
											piece.IsStartForPattern = true;
										piece.AddMove( tempSquare.Identifier );
										pattern.AddGamePiece( piece );
									}
									else
									{
										pattern.AddMoveToPiece( piece, tempSquare.Identifier );
									}
								}
								else
								{
									DraughtsSquare checkSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierBelowLeft( tempSquare.Identifier ) ];

									if( checkSquare != null && checkSquare.IsOccupied == false )
									{
										DraughtsPiece piece = new DraughtsPiece( square.Identifier, "PLAYER", false, square.IsKing );

										if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
										{
											if( pattern.GamePieces.Count == 0 )
												piece.IsStartForPattern = true;
											piece.AddMove( checkSquare.Identifier );
											pattern.AddGamePiece( piece );
										}
										else
										{
											pattern.AddMoveToPiece( piece, checkSquare.Identifier );
										}
									}
								}
							}

							tempSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierBelowRight( square.Identifier ) ];

							if( tempSquare != null )
							{
								if( tempSquare.IsOccupied == false )
								{
									DraughtsPiece piece = new DraughtsPiece( square.Identifier, "PLAYER", false, square.IsKing );

									if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
									{
										if( pattern.GamePieces.Count == 0 )
											piece.IsStartForPattern = true;
										piece.AddMove( tempSquare.Identifier );
										pattern.AddGamePiece( piece );
									}
									else
									{
										pattern.AddMoveToPiece( piece, tempSquare.Identifier );
									}
								}
								else
								{
									DraughtsSquare checkSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierBelowRight( tempSquare.Identifier ) ];

									if( checkSquare != null && checkSquare.IsOccupied == false )
									{
										DraughtsPiece piece = new DraughtsPiece( square.Identifier, "PLAYER", false, square.IsKing );

										if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
										{
											if( pattern.GamePieces.Count == 0 )
												piece.IsStartForPattern = true;
											piece.AddMove( checkSquare.Identifier );
											pattern.AddGamePiece( piece );
										}
										else
										{
											pattern.AddMoveToPiece( piece, checkSquare.Identifier );
										}
									}
								}
							}

							if( square.IsKing == true )
							{
								tempSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierAboveLeft( square.Identifier ) ];

								if( tempSquare != null )
								{
									if( tempSquare.IsOccupied == false )
									{
										DraughtsPiece piece = new DraughtsPiece( square.Identifier, "PLAYER", false, square.IsKing );

										if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
										{
											if( pattern.GamePieces.Count == 0 )
												piece.IsStartForPattern = true;
											piece.AddMove( tempSquare.Identifier );
											pattern.AddGamePiece( piece );
										}
										else
										{
											pattern.AddMoveToPiece( piece, tempSquare.Identifier );
										}
									}
									else
									{
										DraughtsSquare checkSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierAboveLeft( tempSquare.Identifier ) ];

										if( checkSquare != null && checkSquare.IsOccupied == false )
										{
											DraughtsPiece piece = new DraughtsPiece( square.Identifier, "PLAYER", false, square.IsKing );

											if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
											{
												if( pattern.GamePieces.Count == 0 )
													piece.IsStartForPattern = true;
												piece.AddMove( checkSquare.Identifier );
												pattern.AddGamePiece( piece );
											}
											else
											{
												pattern.AddMoveToPiece( piece, checkSquare.Identifier );
											}
										}
									}
								}

								tempSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierAboveRight( square.Identifier ) ];

								if( tempSquare != null )
								{
									if( tempSquare.IsOccupied == false )
									{
										DraughtsPiece piece = new DraughtsPiece( square.Identifier, "PLAYER", false, square.IsKing );

										if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
										{
											if( pattern.GamePieces.Count == 0 )
												piece.IsStartForPattern = true;
											piece.AddMove( tempSquare.Identifier );
											pattern.AddGamePiece( piece );
										}
										else
										{
											pattern.AddMoveToPiece( piece, tempSquare.Identifier );
										}
									}
									else
									{
										DraughtsSquare checkSquare = ( DraughtsSquare )board.GetHashTable[ board.GetIdentifierAboveRight( tempSquare.Identifier ) ];

										if( checkSquare != null && checkSquare.IsOccupied == false )
										{
											DraughtsPiece piece = new DraughtsPiece( square.Identifier, "PLAYER", false, square.IsKing );

											if( pattern.IsPieceInPattern( piece.SquareIdentifier, nMoveNumber ) == false )
											{
												if( pattern.GamePieces.Count == 0 )
													piece.IsStartForPattern = true;
												piece.AddMove( checkSquare.Identifier );
												pattern.AddGamePiece( piece );
											}
											else
											{
												pattern.AddMoveToPiece( piece, checkSquare.Identifier );
											}
										}
									}
								}
							}
						}

						if( pattern.Count > 0 )
						{
							availablePatterns.AddPattern( pattern );
						}
					}
				}
			}
		}

		/// <summary>
		/// Get the number of pieces belonging to a particular player
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public int GetPiecesCount( string player, bool light )
		{
			int nCount = 0;
			DraughtsPiece piece = null;

			for( int i=0; i<availablePatterns.Count; i++ )
			{
				piece = availablePatterns.GetPattern( i ).GetStartsWithPiece( nMoveNumber );

				if( piece != null )
				{
					if( piece.Player == player )
						nCount++;
				}
			}

			/// if the count == 1 the players final piece could have just been taken
			/// 

			if( nCount == 1 )
			{
				nCount = 0;
				GetAvailablePieces( light );

				for( int i=0; i<availablePatterns.Count; i++ )
				{
					piece = availablePatterns.GetPattern( i ).GetStartsWithPiece( nMoveNumber );

					if( piece != null )
					{
						if( piece.Player == player )
							nCount++;
					}
				}
			}	

			return nCount;
		}

		/// <summary>
		/// keep track of what moves are made.
		/// called after every move so that you can update the history at the end of the game
		/// </summary>
		/// <param name="moveIdentifier"></param>
		public void UpdateGamePatterns( string identifier, string toSquare )
		{
			DraughtsPiece piece = null;
			DraughtsMove move = null;

			DraughtsPattern pattern = null;
			DraughtsPattern gamePattern = null;

			for( int i=0; i<availablePatterns.Count; i++ )
			{
				pattern = availablePatterns.GetPattern( i );

				if( pattern.GetStartsWith() == identifier )
				{
					if( historicalPatterns != null && historicalPatterns.Patterns.Count > 0 )
					{
						gamePattern = historicalPatterns.GetPattern( pattern.GetStartsWith(), pattern.MoveNumber );
					}
					else
						gamePattern = gamePatterns.GetPattern( pattern.GetStartsWith(), pattern.MoveNumber );

					if( gamePattern != null && gamePattern.MoveNumber == nMoveNumber )
					{
						for( int n=0; n<gamePattern.GamePieces.Count; n++ )
						{
							piece = ( DraughtsPiece )gamePattern.GamePieces[ n ];

							if( piece == null )
								continue;

							for( int k=0; k<piece.Moves.Count; k++ )
							{
								move = ( DraughtsMove )piece.Moves[ k ];

								if( move == null )
									continue;

								if( move.Identifier == toSquare )
									move.UsedThisTime = true;
							}

						}

						gamePatterns.AddPattern( gamePattern );
					}
					else
					{
						for( int n=0; n<pattern.GamePieces.Count; n++ )
						{
							piece = ( DraughtsPiece )pattern.GamePieces[ n ];

							if( piece == null )
								continue;
							
							for( int k=0; k<piece.Moves.Count; k++ )
							{
								move = ( DraughtsMove )piece.Moves[ k ];

								if( move == null )
									continue;

								if( move.Identifier == toSquare )
									move.UsedThisTime = true;
							}
						}

						gamePatterns.AddPattern( pattern );
					}
				}
			}
		}

		/// <summary>
		/// Update the history according to who won.
		/// </summary>
		/// <param name="computerWon"></param>
		public void UpdateHistoricalPatterns( bool computerWon )
		{
			DraughtsPattern pattern = null;
			DraughtsPattern historyPattern = null;
			DraughtsPiece patternPiece = null;
			DraughtsPiece historyPiece = null;
			DraughtsMove patternMove = null;
			DraughtsMove historyMove = null;

			for( int i=0; i<gamePatterns.Count; i++ )
			{
				pattern = gamePatterns.GetPattern( i );

				if( pattern == null )
					continue;

				historyPattern = historicalPatterns.GetPattern( pattern.GetStartsWith(), pattern.MoveNumber );

				if( historyPattern != null )
				{
					for( int n=0; n<pattern.GamePieces.Count; n++ )
					{
						patternPiece = ( DraughtsPiece )pattern.GamePieces[ n ];
						historyPiece = ( DraughtsPiece )historyPattern.GamePieces[ n ];

						if( patternPiece == null || historyPiece == null )
							continue;

						for( int k=0; k<patternPiece.Moves.Count; k++ )
						{
							patternMove = ( DraughtsMove )patternPiece.Moves[ k ];
							historyMove = ( DraughtsMove )historyPiece.Moves[ k ];

							if( patternMove == null || historyMove == null )
								continue;

							if( patternMove.UsedThisTime == true )
							{
								/// update the winning and losing
								/// 

								if( computerWon == true )
								{
									historyMove.TimesUsedInWinningGame++;
								}
								else
									historyMove.TimesUsedInLosingGame++;

								historyMove.TimesUsed++;
							}
						}
					}

					historyPattern.NumberOfTimesSeen++;

					if( computerWon == true )
					{
						historyPattern.NumberOfTimesSeenInWinningGame++;
					}
					else
						historyPattern.NumberOfTimesSeenInLosingGame++;
				}
				else
				{
					for( int n=0; n<pattern.GamePieces.Count; n++ )
					{
						patternPiece = ( DraughtsPiece )pattern.GamePieces[ n ];

						if( patternPiece == null )
							continue;

						for( int k=0; k<patternPiece.Moves.Count; k++ )
						{
							patternMove = ( DraughtsMove )patternPiece.Moves[ k ];

							if( patternMove == null )
								continue;

							if( patternMove.UsedThisTime == true )
							{
								if( computerWon == true )
									patternMove.TimesUsedInWinningGame++;
								else
									patternMove.TimesUsedInLosingGame++;

								patternMove.TimesUsed++;
							}
						}
					}

					pattern.NumberOfTimesSeen++;
					if( computerWon == true )
					{
						pattern.NumberOfTimesSeenInWinningGame++;
					}
					else
						pattern.NumberOfTimesSeenInLosingGame++;

					historicalPatterns.AddPattern( pattern );
				}
			}

			SavePatterns();
		}

		public bool CheckForMustTake( bool player )
		{
			collection.Clear();

			if( availablePatterns.Count == 0 )
				return false;

			/// create the decisions
			/// 
			for( int i=0; i<availablePatterns.Count; i++ )
			{
				FuzzyDecisionSet fuzzyDecisionSet = new FuzzyDecisionSet( availablePatterns.GetPattern( i ).GetStartsWith() );
				fuzzyDecisionSet.AddDecision( new FuzzyDecision( "MustTake", true ) );
				collection.Insert( i, fuzzyDecisionSet );
			}

			FuzzyDecisionSet decisionSet = null;
			DraughtsPattern pattern = null;
			DraughtsPiece piece = null;
		
			/// process the initial decisions
			/// 
			for( int i=0; i<collection.Count; i++ )
			{
				/// so much for writing the collection class as an array list if using foreach
				/// it returns a read only variable which I can't use here anyway.
				/// 
				decisionSet = ( FuzzyDecisionSet )collection[ i ];

				/// make sure that we are dealling with patterns for this move.
				/// 
				pattern = ( DraughtsPattern )availablePatterns.GetPattern( i );

				if( pattern.MoveNumber != nMoveNumber )
					continue;
			
				piece = pattern.GetStartsWithPiece( nMoveNumber );

				DraughtsSquare square = ( DraughtsSquare )board.GetSquare( piece.SquareIdentifier );

				if( square.OccupyingName != piece.Player )
					continue;

				/// don't care at this point if it is a computer or a player piece
				/// 
				if( piece.LightPiece == true )
				{
					/// check for a must take situation
					/// 
					if( processMoves.CanTakeAboveLeft( board, piece.SquareIdentifier, piece.Player ) == true )
					{
						if( player == true && piece.Player == "PLAYER" )
							return true;
						if( player == false && piece.Player == "COMPUTER" )
							return true;
					}
					if( processMoves.CanTakeAboveRight( board, piece.SquareIdentifier, piece.Player ) == true )
					{
						if( player == true && piece.Player == "PLAYER" )
							return true;
						if( player == false && piece.Player == "COMPUTER" )
							return true;
					}
					
					if( piece.IsKing == true )
					{
						if( processMoves.CanTakeBelowLeft( board, piece.SquareIdentifier, piece.Player ) == true )
						{
							if( player == true && piece.Player == "PLAYER" )
								return true;
							if( player == false && piece.Player == "COMPUTER" )
								return true;
						}
						if( processMoves.CanTakeBelowRight( board, piece.SquareIdentifier, piece.Player ) == true )
						{
							if( player == true && piece.Player == "PLAYER" )
								return true;
							if( player == false && piece.Player == "COMPUTER" )
								return true;
						}
					}
				}
				else
				{
					if( processMoves.CanTakeBelowLeft( board, piece.SquareIdentifier, piece.Player ) == true )
					{
						if( player == true && piece.Player == "PLAYER" )
							return true;
						if( player == false && piece.Player == "COMPUTER" )
							return true;
					}
					if( processMoves.CanTakeBelowRight( board, piece.SquareIdentifier, piece.Player ) == true )
					{
						if( player == true && piece.Player == "PLAYER" )
							return true;
						if( player == false && piece.Player == "COMPUTER" )
							return true;
					}
					
					if( piece.IsKing == true )
					{
						if( processMoves.CanTakeAboveLeft( board, piece.SquareIdentifier, piece.Player ) == true )
						{
							if( player == true && piece.Player == "PLAYER" )
								return true;
							if( player == false && piece.Player == "COMPUTER" )
								return true;
						}
						if( processMoves.CanTakeAboveRight( board, piece.SquareIdentifier, piece.Player ) == true )
						{
							if( player == true && piece.Player == "PLAYER" )
								return true;
							if( player == false && piece.Player == "COMPUTER" )
								return true;
						}
					}
				}
			}

			return false;
		}

		public void MovePiece( bool light )
		{
			/// initialise the fuzzy logic stuff for the decision
			/// 
			/// this is where rules and stuff will be worked out
			/// Must Take is when a opposing piece is directly infront and so a piece has to be taken
			///  

			collection.Clear();
			bCannotMove = false;
			bPieceTaken = false;

			/// create the decisions
			/// 
			for( int i=0; i<availablePatterns.Count; i++ )
			{
				FuzzyDecisionSet fuzzyDecisionSet = new FuzzyDecisionSet( availablePatterns.GetPattern( i ).GetStartsWith() );
				fuzzyDecisionSet.Decision.IsValid = true;
				fuzzyDecisionSet.Decision.IncrementDecision();
				fuzzyDecisionSet.IsValid = true;
				fuzzyDecisionSet.AddDecision( new FuzzyDecision( "MustTake", true ) );
				fuzzyDecisionSet.AddDecision( new FuzzyDecision( "MoveAboveLeft", true ) );
				fuzzyDecisionSet.IncrementByName( "MoveAboveLeft" );
				fuzzyDecisionSet.AddDecision( new FuzzyDecision( "MoveAboveRight", true ) );
				fuzzyDecisionSet.IncrementByName( "MoveAboveRight" );
				fuzzyDecisionSet.AddDecision( new FuzzyDecision( "MoveBelowLeft", true ) );
				fuzzyDecisionSet.IncrementByName( "MoveBelowLeft" );
				fuzzyDecisionSet.AddDecision( new FuzzyDecision( "MoveBelowRight", true ) );
				fuzzyDecisionSet.IncrementByName( "MoveBelowRight" );

				collection.Insert( i, fuzzyDecisionSet );
			}

			FuzzyDecisionSet decisionSet = null;
			DraughtsPattern pattern = null;
			DraughtsPiece piece = null;
			DraughtsMove move = null;
			DraughtsSquare moveSquare = null;

			string strToSquare = null;

			/// quick check on the must take
			/// 
			bool bMustTake = false;
			
			/// process the initial decisions
			/// 
			for( int i=0; i<collection.Count; i++ )
			{
				/// so much for writing the collection class as an array list if using foreach
				/// it returns a read only variable which I can't use here anyway.
				/// 
				decisionSet = ( FuzzyDecisionSet )collection[ i ];
				
				/// make sure that we are dealling with patterns for this move.
				/// 
				pattern = ( DraughtsPattern )availablePatterns.GetPattern( i );

				if( pattern.MoveNumber != nMoveNumber )
					continue;
			
				piece = availablePatterns.GetPattern( i ).GetStartsWithPiece( nMoveNumber );

				/// don't care at this point if it is a computer or a player piece
				/// but if it's a light piece it is moving up the board
				/// 
				if( piece.LightPiece == true && piece.LightPiece == light )
				{
					/// check for a must take situation
					/// 
					if( processMoves.CanTakeAboveLeft( board, piece.SquareIdentifier, piece.Player ) == true )
					{
						decisionSet.IncrementByName( "MustTake" );
						decisionSet.AddByName( "MoveAboveLeft", nMustTake );
						decisionSet.Decision.AddToDecision( nMustTake );
						bMustTake = true;

						/// reduce the must take value if the taking piece will be taken in turn
						/// 
						if( processMoves.IsEnemyAboveRight( board, board.GetIdentifierAboveLeft( board.GetIdentifierAboveLeft( piece.SquareIdentifier ) ), piece.Player ) == true )
						{
							decisionSet.SubtractByName( "MoveAboveLeft", nMustTakeTakenReduction );
							decisionSet.Decision.DecrementDecision();
						}

						if( processMoves.IsEnemyAboveLeft( board, board.GetIdentifierAboveLeft( board.GetIdentifierAboveLeft( piece.SquareIdentifier ) ), piece.Player ) == true )
						{
							decisionSet.SubtractByName( "MoveAboveLeft", nMustTakeTakenReduction );
							decisionSet.Decision.DecrementDecision();
						}
					}
					if( processMoves.CanTakeAboveRight( board, piece.SquareIdentifier, piece.Player ) == true )
					{
						decisionSet.IncrementByName( "MustTake" );
						decisionSet.AddByName( "MoveAboveRight", nMustTake );
						decisionSet.Decision.AddToDecision( nMustTake );
						bMustTake = true;

						if( processMoves.IsEnemyAboveRight( board, board.GetIdentifierAboveRight( board.GetIdentifierAboveRight( piece.SquareIdentifier ) ), piece.Player ) == true )
						{
							decisionSet.SubtractByName( "MoveAboveRight", nMustTakeTakenReduction );
							decisionSet.Decision.DecrementDecision();
						}

						if( processMoves.IsEnemyAboveLeft( board, board.GetIdentifierAboveRight( board.GetIdentifierAboveRight( piece.SquareIdentifier ) ), piece.Player ) == true )
						{
							decisionSet.SubtractByName( "MoveAboveRight", nMustTakeTakenReduction );
							decisionSet.Decision.DecrementDecision();
						}
					}
					
					if( piece.IsKing == true )
					{
						if( processMoves.CanTakeBelowLeft( board, piece.SquareIdentifier, piece.Player ) == true )
						{
							decisionSet.IncrementByName( "MustTake" );
							decisionSet.AddByName( "MoveBelowLeft", nMustTake );
							decisionSet.Decision.AddToDecision( nMustTake );
							bMustTake = true;

							if( processMoves.IsEnemyBelowLeft( board, board.GetIdentifierBelowLeft( board.GetIdentifierBelowLeft( piece.SquareIdentifier ) ), piece.Player ) == true )
							{
								decisionSet.SubtractByName( "MoveBelowLeft", nMustTakeTakenReduction );
								decisionSet.Decision.DecrementDecision();
							}

							if( processMoves.IsEnemyBelowRight( board, board.GetIdentifierBelowLeft( board.GetIdentifierBelowLeft( piece.SquareIdentifier ) ), piece.Player ) == true )
							{
								decisionSet.SubtractByName( "MoveBelowLeft", nMustTakeTakenReduction );
								decisionSet.Decision.DecrementDecision();
							}
						}
						if( processMoves.CanTakeBelowRight( board, piece.SquareIdentifier, piece.Player ) == true )
						{
							decisionSet.IncrementByName( "MustTake" );
							decisionSet.AddByName( "MoveBelowRight", nMustTake );
							decisionSet.Decision.AddToDecision( nMustTake );
							bMustTake = true;

							if( processMoves.IsEnemyBelowLeft( board, board.GetIdentifierBelowRight( board.GetIdentifierBelowRight( piece.SquareIdentifier ) ), piece.Player ) == true )
							{
								decisionSet.SubtractByName( "MoveBelowRight", nMustTakeTakenReduction );
								decisionSet.Decision.DecrementDecision();
							}

							if( processMoves.IsEnemyBelowRight( board, board.GetIdentifierBelowRight( board.GetIdentifierBelowRight( piece.SquareIdentifier ) ), piece.Player ) == true )
							{
								decisionSet.SubtractByName( "MoveBelowRight", nMustTakeTakenReduction );
								decisionSet.Decision.DecrementDecision();
							}
						}
					}
					else
					{
						decisionSet.SetIsValidByName( "MoveBelowLeft", false );
						decisionSet.SetIsValidByName( "MoveBelowRight", false );
					}

				}
				
				if( piece.LightPiece == false && piece.LightPiece == light )
				{
					if( processMoves.CanTakeBelowLeft( board, piece.SquareIdentifier, piece.Player ) == true )
					{
						decisionSet.IncrementByName( "MustTake" );
						decisionSet.AddByName( "MoveBelowLeft", nMustTake );
						decisionSet.Decision.AddToDecision( nMustTake );
						bMustTake = true;

						if( processMoves.IsEnemyBelowLeft( board, board.GetIdentifierBelowLeft( board.GetIdentifierBelowLeft( piece.SquareIdentifier ) ), piece.Player ) == true )
						{
							decisionSet.SubtractByName( "MoveBelowLeft", nMustTakeTakenReduction );
							decisionSet.Decision.DecrementDecision();
						}

						if( processMoves.IsEnemyBelowRight( board, board.GetIdentifierBelowLeft( board.GetIdentifierBelowLeft( piece.SquareIdentifier ) ), piece.Player ) == true )
						{
							decisionSet.SubtractByName( "MoveBelowLeft", nMustTakeTakenReduction );
							decisionSet.Decision.DecrementDecision();
						}
					}
					if( processMoves.CanTakeBelowRight( board, piece.SquareIdentifier, piece.Player ) == true )
					{
						decisionSet.IncrementByName( "MustTake" );
						decisionSet.AddByName( "MoveBelowRight", nMustTake );
						decisionSet.Decision.AddToDecision( nMustTake );
						bMustTake = true;

						if( processMoves.IsEnemyBelowLeft( board, board.GetIdentifierBelowRight( board.GetIdentifierBelowRight( piece.SquareIdentifier ) ), piece.Player ) == true )
						{
							decisionSet.SubtractByName( "MoveBelowRight", nMustTakeTakenReduction );
							decisionSet.Decision.DecrementDecision();
						}

						if( processMoves.IsEnemyBelowRight( board, board.GetIdentifierBelowRight( board.GetIdentifierBelowRight( piece.SquareIdentifier ) ), piece.Player ) == true )
						{
							decisionSet.SubtractByName( "MoveBelowRight", nMustTakeTakenReduction );
							decisionSet.Decision.DecrementDecision();
						}
					}
					
					if( piece.IsKing == true )
					{
						if( processMoves.CanTakeAboveLeft( board, piece.SquareIdentifier, piece.Player ) == true )
						{
							decisionSet.IncrementByName( "MustTake" );
							decisionSet.AddByName( "MoveAboveLeft", nMustTake );
							decisionSet.Decision.AddToDecision( nMustTake );
							bMustTake = true;

							if( processMoves.IsEnemyAboveLeft( board, board.GetIdentifierAboveLeft( board.GetIdentifierAboveLeft( piece.SquareIdentifier ) ), piece.Player ) == true )
							{
								decisionSet.SubtractByName( "MoveAboveLeft", nMustTakeTakenReduction );
								decisionSet.Decision.DecrementDecision();
							}

							if( processMoves.IsEnemyAboveRight( board, board.GetIdentifierAboveLeft( board.GetIdentifierAboveLeft( piece.SquareIdentifier ) ), piece.Player ) == true )
							{
								decisionSet.SubtractByName( "MoveAboveLeft", nMustTakeTakenReduction );
								decisionSet.Decision.DecrementDecision();
							}
						}
						if( processMoves.CanTakeAboveRight( board, piece.SquareIdentifier, piece.Player ) == true )
						{
							decisionSet.IncrementByName( "MustTake" );
							decisionSet.AddByName( "MoveAboveRight", nMustTake );
							decisionSet.Decision.AddToDecision( nMustTake );
							bMustTake = true;

							if( processMoves.IsEnemyAboveLeft( board, board.GetIdentifierAboveRight( board.GetIdentifierAboveRight( piece.SquareIdentifier ) ), piece.Player ) == true )
							{
								decisionSet.SubtractByName( "MoveAboveRight", nMustTakeTakenReduction );
								decisionSet.Decision.DecrementDecision();
							}

							if( processMoves.IsEnemyAboveRight( board, board.GetIdentifierAboveRight( board.GetIdentifierAboveRight( piece.SquareIdentifier ) ), piece.Player ) == true )
							{
								decisionSet.SubtractByName( "MoveAboveRight", nMustTakeTakenReduction );
								decisionSet.Decision.DecrementDecision();
							}
						}
					}
					else
					{
						decisionSet.SetIsValidByName( "MoveAboveLeft", false );
						decisionSet.SetIsValidByName( "MoveAboveRight", false );
					}
				}
			}


			if( bMustTake == false )
			{
				bool bDecisionIncremented = false;
				bool bDecisionDecremented = false;

				/// validate the moves
				/// 

				for( int i=0; i<collection.Count; i++ )
				{
					pattern = ( DraughtsPattern )availablePatterns.GetPattern( i );

					if( pattern.MoveNumber != nMoveNumber )
						return;

					decisionSet = ( FuzzyDecisionSet )collection[ i ];
					piece = ( DraughtsPiece )pattern.GetStartsWithPiece();


					/// check not null
					/// 
					if( board.GetSquareAboveLeft( piece.SquareIdentifier ) == null )
					{
						decisionSet.SetIsValidByName( "MoveAboveLeft", false );
					}
					else
					{
						/// if not clear and is friendly can't move
						/// 
						if( processMoves.IsAboveLeftClear( board, piece.SquareIdentifier ) == false 
							&& processMoves.IsFriendlyAboveLeft( board, piece.SquareIdentifier, piece.Player ) == true )
						{
							decisionSet.SetIsValidByName( "MoveAboveLeft", false );
						}

						/// if not clear and contains enemy but is not a take move can't move
						/// 
						if( processMoves.IsAboveLeftClear( board, piece.SquareIdentifier ) == false
							&& processMoves.IsEnemyAboveLeft( board, piece.SquareIdentifier, piece.Player ) == true
							&& processMoves.IsAboveLeftClear( board, board.GetIdentifierAboveLeft( piece.SquareIdentifier ) ) == false )
						{
							decisionSet.SetIsValidByName( "MoveAboveLeft", false );
						}

					}


					if( board.GetSquareAboveRight( piece.SquareIdentifier ) == null )
					{
						decisionSet.SetIsValidByName( "MoveAboveRight", false );
					}
					else
					{
						if( processMoves.IsAboveRightClear( board, piece.SquareIdentifier ) == false 
							&& processMoves.IsFriendlyAboveRight( board, piece.SquareIdentifier, piece.Player ) == true )
						{
							decisionSet.SetIsValidByName( "MoveAboveRight", false );
						}

						if( processMoves.IsAboveRightClear( board, piece.SquareIdentifier ) == false
							&& processMoves.IsEnemyAboveRight( board, piece.SquareIdentifier, piece.Player ) == true 
							&& processMoves.IsAboveRightClear( board, board.GetIdentifierAboveRight( piece.SquareIdentifier ) ) == true )
						{
							decisionSet.SetIsValidByName( "MoveAboveRight", false );
						}
					}

					if( board.GetSquareBelowRight( piece.SquareIdentifier ) == null )
					{
						decisionSet.SetIsValidByName( "MoveBelowRight", false );
					}
					else
					{
						if( processMoves.IsBelowRightClear( board, piece.SquareIdentifier ) == false 
							&& processMoves.IsFriendlyBelowRight( board, piece.SquareIdentifier, piece.Player ) == true )
						{
							decisionSet.SetIsValidByName( "MoveBelowRight", false );
						}

						if( processMoves.IsBelowRightClear( board, piece.SquareIdentifier ) == false 
							&& processMoves.IsEnemyBelowRight( board, piece.SquareIdentifier, piece.Player ) == true 
							&& processMoves.IsBelowRightClear( board, board.GetIdentifierBelowLeft( piece.SquareIdentifier ) ) == true )
						{
							decisionSet.SetIsValidByName( "MoveBelowRight", false );
						}

					}

					if( board.GetSquareBelowLeft( piece.SquareIdentifier ) == null )
					{
						decisionSet.SetIsValidByName( "MoveBelowLeft", false );
					}
					else
					{
						if( processMoves.IsBelowLeftClear( board, piece.SquareIdentifier ) == false 
							&& processMoves.IsFriendlyBelowLeft( board, piece.SquareIdentifier, piece.Player ) == true )
						{
							decisionSet.SetIsValidByName( "MoveBelowLeft", false );
						}

						if( processMoves.IsBelowLeftClear( board, piece.SquareIdentifier ) == false
							&& processMoves.IsEnemyBelowLeft( board, piece.SquareIdentifier, piece.Player ) == true 
							&& processMoves.IsBelowLeftClear( board, board.GetIdentifierBelowLeft( piece.SquareIdentifier ) ) == true )
						{
							decisionSet.SetIsValidByName( "MoveBelowLeft", false );
						}

					}
				}

				
				/// check normal moves
				/// 

				for( int i=0; i<collection.Count; i++ )
				{
					bDecisionIncremented = false;
					bDecisionDecremented = false;

					/// make sure that we are dealling with patterns for this move.
					/// 
					pattern = ( DraughtsPattern )availablePatterns.GetPattern( i );

					if( pattern.MoveNumber != nMoveNumber )
						continue;

					decisionSet = ( FuzzyDecisionSet )collection[ i ];
					piece = ( DraughtsPiece )pattern.GetStartsWithPiece();

					if( piece.LightPiece == true && piece.IsKing == false )
					{
						if( processMoves.IsAboveLeftClear( board, piece.SquareIdentifier ) == true )
						{
							decisionSet.AddByName( "MoveAboveLeft", nNormalMove );
							bDecisionIncremented = true;

						}
						else 
						{
							/// Also invalidate moves where the way is blocked but don't decrement the decision
							/// as it could be that moving another way is a really good move.
							/// 
							if( processMoves.IsEnemyAboveLeft( board, piece.SquareIdentifier, piece.Player ) == true 
								&& processMoves.CanTakeAboveLeft( board, piece.SquareIdentifier, piece.Player ) == false )
							{
								decisionSet.SetIsValidByName( "MoveAboveLeft", false );
							}
						}

						if( processMoves.IsAboveRightClear( board, piece.SquareIdentifier ) == true )
						{
							decisionSet.AddByName( "MoveAboveRight", nNormalMove );
							bDecisionIncremented = true;

						}
						else
						{
							if( processMoves.IsEnemyAboveRight( board, piece.SquareIdentifier, piece.Player ) == true 
								&& processMoves.CanTakeAboveRight( board, piece.SquareIdentifier, piece.Player ) == false )
							{
								decisionSet.SetIsValidByName( "MoveAboveRight", false );
							}
						}
					}
					else if( piece.LightPiece == false && piece.IsKing == false )
					{
						if( processMoves.IsBelowLeftClear( board, piece.SquareIdentifier ) == true )
						{
							decisionSet.AddByName( "MoveBelowLeft", nNormalMove );
							bDecisionIncremented = true;

						}
						else
						{
							if( processMoves.IsEnemyBelowLeft( board, piece.SquareIdentifier, piece.Player ) == true 
								&& processMoves.CanTakeBelowLeft( board, piece.SquareIdentifier, piece.Player ) == false )
							{
								decisionSet.SetIsValidByName( "MoveBelowLeft", false );
							}
						}

						if( processMoves.IsBelowRightClear( board, piece.SquareIdentifier ) == true )
						{
							decisionSet.AddByName( "MoveBelowRight", nNormalMove );
							bDecisionIncremented = true;

						}
						else
						{
							if( processMoves.IsEnemyBelowRight( board, piece.SquareIdentifier, piece.Player ) == true 
								&& processMoves.CanTakeBelowRight( board, piece.SquareIdentifier, piece.Player ) == false )
							{
								decisionSet.SetIsValidByName( "MoveBelowRight", false );
							}
						}
					}
					else if( piece.IsKing == true )
					{
						if( processMoves.IsBelowLeftClear( board, piece.SquareIdentifier ) == true )
						{
							decisionSet.AddByName( "MoveBelowLeft", nNormalMove );
							bDecisionIncremented = true;

						}
						else
						{
							if( processMoves.IsEnemyBelowLeft( board, piece.SquareIdentifier, piece.Player ) == true 
								&& processMoves.CanTakeBelowLeft( board, piece.SquareIdentifier, piece.Player ) == false )
							{
								decisionSet.SetIsValidByName( "MoveBelowLeft", false );
							}
						}

						if( processMoves.IsBelowRightClear( board, piece.SquareIdentifier ) == true )
						{
							decisionSet.AddByName( "MoveBelowRight", nNormalMove );
							bDecisionIncremented = true;
						}
						else
						{
							if( processMoves.IsEnemyBelowRight( board, piece.SquareIdentifier, piece.Player ) == true 
								&& processMoves.CanTakeBelowRight( board, piece.SquareIdentifier, piece.Player ) == false )
							{
								decisionSet.SetIsValidByName( "MoveBelowRight", false );
							}
						}

						if( processMoves.IsAboveLeftClear( board, piece.SquareIdentifier ) == true )
						{
							decisionSet.AddByName( "MoveAboveLeft", nNormalMove );
							bDecisionIncremented = true;

						}
						else
						{
							if( processMoves.IsEnemyAboveLeft( board, piece.SquareIdentifier, piece.Player ) == true 
								&& processMoves.CanTakeAboveLeft( board, piece.SquareIdentifier, piece.Player ) == false )
							{
								decisionSet.SetIsValidByName( "MoveAboveLeft", false );
							}
						}

						if( processMoves.IsAboveRightClear( board, piece.SquareIdentifier ) == true )
						{
							decisionSet.AddByName( "MoveAboveRight", nNormalMove );
							bDecisionIncremented = true;
						}
						else
						{
							if( processMoves.IsEnemyAboveRight( board, piece.SquareIdentifier, piece.Player ) == true 
								&& processMoves.CanTakeAboveRight( board, piece.SquareIdentifier, piece.Player ) == false )
							{
								decisionSet.SetIsValidByName( "MoveAboveRight", false );
							}
						}
					}

					if( bDecisionIncremented == true )
					{
						decisionSet.Decision.AddToDecision( nNormalMove );
					}

					if( bDecisionDecremented == true )
					{
						decisionSet.Decision.AddToDecision( nNormalMove );
					}
				}


				/// Influence the decisions according to the historical memory
				///

 
				/// influence the decisions according to the pattern matching
				/// 

				for( int i=0; i<collection.Count; i++ )
				{
					/// make sure that we are dealling with patterns for this move.
					/// 
					pattern = ( DraughtsPattern )availablePatterns.GetPattern( i );

					if( pattern.MoveNumber != nMoveNumber )
						continue;

					decisionSet = ( FuzzyDecisionSet )collection[ i ];

					/// note that the move still needs to be a valid move for the current turn
					/// 

					if( historicalPatterns.IsIn( pattern ) == true )
					{
						bDecisionIncremented = false;
						bDecisionDecremented = false;

						pattern = ( DraughtsPattern )historicalPatterns.GetPattern( pattern );

						piece = ( DraughtsPiece )pattern.GetStartsWithPiece();

						for( int n=0; n<piece.Moves.Count; n++ )
						{
							move = ( DraughtsMove )piece.Moves[ n ];

							/// check above left
							/// 
							if( board.GetIdentifierAboveLeft( piece.SquareIdentifier ) == move.Identifier 
								&& processMoves.IsAboveLeftClear( board, piece.SquareIdentifier ) == true )
							{
								if( move.WonMore() == true )
								{
									decisionSet.AddByName( "MoveAboveLeft", nHistoricalMove );
									bDecisionIncremented = true;
								}
								else
								{
									decisionSet.SubtractByName( "MoveAboveLeft", nHistoricalMove );
									bDecisionDecremented = true;
								}
							}

							if( board.GetIdentifierAboveRight( piece.SquareIdentifier ) == move.Identifier 
								&& processMoves.IsAboveRightClear( board, piece.SquareIdentifier ) == true )
							{
								if( move.WonMore() == true )
								{
									decisionSet.AddByName( "MoveAboveRight", nHistoricalMove );
									bDecisionIncremented = true;
								}
								else
								{
									decisionSet.SubtractByName( "MoveAboveRight", nHistoricalMove );
									bDecisionDecremented = true;
								}
							}

							if( board.GetIdentifierBelowRight( piece.SquareIdentifier ) == move.Identifier 
								&& processMoves.IsBelowRightClear( board, piece.SquareIdentifier ) == true )
							{
								if( move.WonMore() == true )
								{
									decisionSet.AddByName( "MoveBelowRight", nHistoricalMove );
									bDecisionIncremented = true;
								}
								else
								{
									decisionSet.SubtractByName( "MoveBelowRight", nHistoricalMove );
									bDecisionDecremented = true;
								}
							}

							if( board.GetIdentifierBelowLeft( piece.SquareIdentifier ) == move.Identifier 
								&& processMoves.IsBelowLeftClear( board, piece.SquareIdentifier ) == true )
							{
								if( move.WonMore() == true )
								{
									decisionSet.AddByName( "MoveBelowLeft", nHistoricalMove );
									bDecisionIncremented = true;
								}
								else
								{
									decisionSet.SubtractByName( "MoveBelowLeft", nHistoricalMove );
									bDecisionDecremented = true;
								}
							}
						}

						if( bDecisionIncremented == true )
						{
							decisionSet.Decision.AddToDecision( nHistoricalMove );
						}

						if( bDecisionDecremented == true )
						{
							decisionSet.Decision.SubtractFromDecision( nHistoricalMove );
						}
					}
				}

				/// check to see if a given move will result in a piece being taken
				/// 

				for( int i=0; i<collection.Count; i++ )
				{
					bDecisionDecremented = false;
					/// can take a shortcut here in that moves available is already shown in the 
					/// fuzzydecision stuff so if a value is greater than 0 it is available
					///
 
					decisionSet = ( FuzzyDecisionSet )collection[ i ];

					/// make sure that we are dealling with patterns for this move.
					/// 
					pattern = ( DraughtsPattern )availablePatterns.GetPattern( i );

					if( pattern.MoveNumber != nMoveNumber )
						continue;

					piece = pattern.GetStartsWithPiece( nMoveNumber );

					for( int n=0; n<piece.Moves.Count; n++ )
					{
						move = ( DraughtsMove )piece.Moves[ n ];

						if( decisionSet.GetNumberByName( "MoveAboveLeft" ) > 1 
							&& decisionSet.GetValidByName( "MoveAboveLeft" ) == true
							&& board.GetIdentifierAboveLeft( piece.SquareIdentifier ) == move.Identifier )
						{
							/// check above left of that 
							/// 
			
							if( processMoves.IsEnemyAboveLeft( board, move.Identifier, piece.Player ) == true )
							{
								/// if a piece is present it can make the take move because
								/// it will be moving to the square vacated by this piece.
								/// 
								decisionSet.SubtractByName( "MoveAboveLeft", nTakenMove );
								bDecisionDecremented = true;
							}

							if( processMoves.IsEnemyAboveRight( board, move.Identifier, piece.Player ) == true )
							{
								/// make sure that it can make the take move
								/// 
								if( processMoves.IsBelowLeftClear( board, move.Identifier ) == true )
								{
									decisionSet.SubtractByName( "MoveAboveLeft", nTakenMove );
									bDecisionDecremented = true;
								}
							}

							/// also need to check for king pieces sneaking up behind
							/// 

							if( processMoves.IsEnemyKingBelowLeft( board, move.Identifier, piece.Player ) == true ) 
							{
								if( processMoves.IsAboveLeftClear( board, move.Identifier ) == true )
								{
									decisionSet.SubtractByName( "MoveAboveLeft", nTakenMove );
									bDecisionDecremented = true;
								}
							}

							/// this is piece is moving from the below right position
							/// 
						}

						if( decisionSet.GetNumberByName( "MoveAboveRight" ) > 1 
							&& decisionSet.GetValidByName( "MoveAboveRight" ) == true 
							&& board.GetIdentifierAboveRight( piece.SquareIdentifier ) == move.Identifier )
						{
							if( processMoves.IsEnemyAboveRight( board, move.Identifier, piece.Player ) == true )
							{
								decisionSet.SubtractByName( "MoveAboveRight", nTakenMove );
								bDecisionDecremented = true;
							}
					
							if( processMoves.IsEnemyAboveLeft( board, move.Identifier, piece.Player ) == true )
							{
								if( processMoves.IsBelowLeftClear( board, move.Identifier ) == true )
								{
									decisionSet.SubtractByName( "MoveAboveRight", nTakenMove );
									bDecisionDecremented = true;
								}
							}

							if( processMoves.IsEnemyKingBelowRight( board, move.Identifier, piece.Player ) == true ) 
							{
								if( processMoves.IsAboveLeftClear( board, move.Identifier ) == true )
								{
									decisionSet.SubtractByName( "MoveAboveRight", nTakenMove );
									bDecisionDecremented = true;
								}
							}
						}

						/// do the checks for moving below as at this point only care if a move is valid
						/// not what type or player it is.
						/// 

						if( decisionSet.GetNumberByName( "MoveBelowLeft" ) > 1
							&& decisionSet.GetValidByName( "MoveBelowLeft" ) == true 
							&& board.GetIdentifierBelowLeft( piece.SquareIdentifier ) == move.Identifier )
						{
							if( processMoves.IsEnemyBelowLeft( board, move.Identifier, piece.Player ) == true ) 
							{
								decisionSet.SubtractByName( "MoveBelowLeft", nTakenMove );
								bDecisionDecremented = true;
							}

							if( processMoves.IsEnemyBelowRight( board, move.Identifier, piece.Player ) == true )
							{
								if( processMoves.IsAboveLeftClear( board, move.Identifier ) == true )
								{
									decisionSet.SubtractByName( "MoveBelowLeft", nTakenMove );
									bDecisionDecremented = true; 
								}
							}

							if( processMoves.IsEnemyKingAboveRight( board, move.Identifier, piece.Player ) == true )
							{
								if( processMoves.IsBelowLeftClear( board, move.Identifier ) == true )
								{
									decisionSet.SubtractByName( "MoveBelowLeft", nTakenMove );
									bDecisionDecremented = true;
								}
							}
						}

						if( decisionSet.GetNumberByName( "MoveBelowRight" ) > 1
							&& decisionSet.GetValidByName( "MoveBelowRight" ) == true 
							&& board.GetIdentifierBelowRight( piece.SquareIdentifier ) == move.Identifier )
						{
							if( processMoves.IsEnemyBelowRight( board, move.Identifier, piece.Player ) == true )
							{
								decisionSet.SubtractByName( "MoveBelowRight", nTakenMove );
								bDecisionDecremented = true;
							}
						
							if( processMoves.IsEnemyBelowLeft( board, move.Identifier, piece.Player ) == true )
							{
								if( processMoves.IsAboveRightClear( board, move.Identifier ) == true )
								{
									decisionSet.SubtractByName( "MoveBelowRight", nTakenMove );
									bDecisionDecremented = true;
								}
							}

							if( processMoves.IsEnemyKingAboveLeft( board, move.Identifier, piece.Player ) == true )
							{
								if( processMoves.IsBelowRightClear( board, move.Identifier ) == true )
								{
									decisionSet.SubtractByName( "MoveBelowRight", nTakenMove );
									bDecisionDecremented = true;
								}
							}
						}
					}

					if( bDecisionDecremented == true )
					{
						decisionSet.Decision.SubtractFromDecision( nTakenMove );
					}

				}

				/// Encourage Safe moves
				/// 

				bool bCanBeTaken = false;

				for( int i=0; i<collection.Count; i++ )
				{
					bDecisionIncremented = false;
					bDecisionDecremented = false;

					/// make sure that we are dealling with patterns for this move.
					/// 
					pattern = ( DraughtsPattern )availablePatterns.GetPattern( i );

					if( pattern.MoveNumber != nMoveNumber )
						continue;

					decisionSet = ( FuzzyDecisionSet )collection[ i ];
					piece = ( DraughtsPiece )pattern.GetStartsWithPiece();

					if( decisionSet.IsValidByName( "MoveAboveLeft" ) == true )
					{
						if( piece.Player == "COMPUTER" )
						{
							if( processMoves.IsAboveLeftClear( board, piece.SquareIdentifier ) == true )
							{
								if( processMoves.IsEnemyAboveRight( board, board.GetIdentifierAboveLeft( piece.SquareIdentifier ), piece.Player ) == true 
									&& processMoves.CanTakeBelowLeft( board, board.GetIdentifierAboveRight( board.GetIdentifierAboveLeft( piece.SquareIdentifier ) ), "PLAYER" ) == true )
								{
									decisionSet.SubtractByName( "MoveAboveLeft", nNormalMove );
									bCanBeTaken = true;
								}

								if( processMoves.IsEnemyAboveLeft( board, board.GetIdentifierAboveLeft( piece.SquareIdentifier ), piece.Player ) == true 
									&& processMoves.CanTakeBelowRight( board, board.GetIdentifierAboveLeft( board.GetIdentifierAboveLeft( piece.SquareIdentifier ) ), "PLAYER" ) == true )
								{
									decisionSet.SubtractByName( "MoveAboveLeft", nNormalMove );
									bCanBeTaken = true;
								}

								if( processMoves.IsEnemyBelowRight( board, board.GetIdentifierAboveLeft( piece.SquareIdentifier ), piece.Player ) == true 
									&& processMoves.CanTakeAboveRight( board, board.GetIdentifierBelowRight( board.GetIdentifierAboveLeft( piece.SquareIdentifier ) ), "PLAYER" ) == true )
								{
									decisionSet.SubtractByName( "MoveAboveLeft", nNormalMove );
									bCanBeTaken = true;
								}

								if( bCanBeTaken == false )
								{
									decisionSet.AddByName( "MoveAboveLeft", nNormalMove );
									bDecisionIncremented = true;
								}
								else
									bDecisionDecremented = true;
							}
						}
						else
						{
							if( processMoves.IsAboveLeftClear( board, piece.SquareIdentifier ) == true )
							{
								if( processMoves.IsEnemyAboveRight( board, board.GetIdentifierAboveLeft( piece.SquareIdentifier ), piece.Player ) == true 
									&& processMoves.CanTakeBelowLeft( board, board.GetIdentifierAboveRight( board.GetIdentifierAboveLeft( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
								{
									decisionSet.SubtractByName( "MoveAboveLeft", nNormalMove );
									bCanBeTaken = true;
								}

								if( processMoves.IsEnemyAboveLeft( board, board.GetIdentifierAboveLeft( piece.SquareIdentifier ), piece.Player ) == true 
									&& processMoves.CanTakeBelowRight( board, board.GetIdentifierAboveLeft( board.GetIdentifierAboveLeft( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
								{
									decisionSet.SubtractByName( "MoveAboveLeft", nNormalMove );
									bCanBeTaken = true;
								}

								if( processMoves.IsEnemyBelowRight( board, board.GetIdentifierAboveLeft( piece.SquareIdentifier ), piece.Player ) == true 
									&& processMoves.CanTakeAboveRight( board, board.GetIdentifierBelowRight( board.GetIdentifierAboveLeft( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
								{
									decisionSet.SubtractByName( "MoveAboveLeft", nNormalMove );
									bCanBeTaken = true;
								}

								if( bCanBeTaken == false )
								{
									decisionSet.AddByName( "MoveAboveLeft", nNormalMove );
									bDecisionIncremented = true;
								}
								else
									bDecisionDecremented = true;
							}
						}
					}

					bCanBeTaken = false;

					if( decisionSet.IsValidByName( "MoveAboveRight" ) == true )
					{
						if( piece.Player == "COMPUTER" )
						{
							if( processMoves.IsEnemyAboveRight( board, board.GetIdentifierAboveRight( piece.SquareIdentifier ), piece.Player ) == true
								&& processMoves.CanTakeBelowLeft( board, board.GetIdentifierAboveRight( board.GetIdentifierAboveRight( piece.SquareIdentifier ) ), "PLAYER" ) == true )
							{
								decisionSet.SubtractByName( "MoveAboveRight", nNormalMove );
								bCanBeTaken = true;
							}

							if( processMoves.IsEnemyAboveLeft( board, board.GetIdentifierAboveRight( piece.SquareIdentifier ), piece.Player ) == true
								&& processMoves.CanTakeBelowRight( board, board.GetIdentifierAboveLeft( board.GetIdentifierAboveRight( piece.SquareIdentifier ) ), "PLAYER" ) == true )
							{
								decisionSet.SubtractByName( "MoveAboveRight", nNormalMove );
								bCanBeTaken = true;
							}

							if( processMoves.IsEnemyBelowLeft( board, board.GetIdentifierAboveRight( piece.SquareIdentifier ), piece.Player  ) == true 
								&& processMoves.CanTakeAboveLeft( board, board.GetIdentifierBelowLeft( board.GetIdentifierAboveRight( piece.SquareIdentifier ) ), "PLAYER" ) == true )
							{
								decisionSet.SubtractByName( "MoveAboveRight", nNormalMove );
								bCanBeTaken = true;
							}

							if( bCanBeTaken == false )
							{
								decisionSet.AddByName( "MoveAboveRight", nNormalMove );
								bDecisionIncremented = true;
							}
							else
								bDecisionDecremented = true;
						}
						else
						{
							if( processMoves.IsEnemyAboveRight( board, board.GetIdentifierAboveRight( piece.SquareIdentifier ), piece.Player ) == true
								&& processMoves.CanTakeBelowLeft( board, board.GetIdentifierAboveRight( board.GetIdentifierAboveRight( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
							{
								decisionSet.SubtractByName( "MoveAboveRight", nNormalMove );
								bCanBeTaken = true;
							}

							if( processMoves.IsEnemyAboveLeft( board, board.GetIdentifierAboveRight( piece.SquareIdentifier ), piece.Player ) == true
								&& processMoves.CanTakeBelowRight( board, board.GetIdentifierAboveLeft( board.GetIdentifierAboveRight( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
							{
								decisionSet.SubtractByName( "MoveAboveRight", nNormalMove );
								bCanBeTaken = true;
							}

							if( processMoves.IsEnemyBelowLeft( board, board.GetIdentifierAboveRight( piece.SquareIdentifier ), piece.Player  ) == true 
								&& processMoves.CanTakeAboveLeft( board, board.GetIdentifierBelowLeft( board.GetIdentifierAboveRight( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
							{
								decisionSet.SubtractByName( "MoveAboveRight", nNormalMove );
								bCanBeTaken = true;
							}

							if( bCanBeTaken == false )
							{
								decisionSet.AddByName( "MoveAboveRight", nNormalMove );
								bDecisionIncremented = true;
							}
							else
								bDecisionDecremented = true;
						}
					}

					bCanBeTaken = false;

					if( decisionSet.IsValidByName( "MoveBelowLeft" ) == true )
					{
						if( piece.Player == "COMPUTER" )
						{
							if( processMoves.IsEnemyBelowLeft( board, board.GetIdentifierBelowLeft( piece.SquareIdentifier ), piece.Player ) == true 
								&& processMoves.CanTakeAboveLeft( board, board.GetIdentifierBelowLeft( board.GetIdentifierBelowLeft( piece.SquareIdentifier ) ), "PLAYER" ) == true )
							{
								decisionSet.SubtractByName( "MoveBelowLeft", nNormalMove );
								bCanBeTaken = true;
							}
							
							if( processMoves.IsEnemyBelowRight( board, board.GetIdentifierBelowLeft( piece.SquareIdentifier ), piece.Player ) == true
								&& processMoves.CanTakeAboveRight( board, board.GetIdentifierBelowRight( board.GetIdentifierBelowLeft( piece.SquareIdentifier ) ), "PLAYER" ) == true )
							{
								decisionSet.SubtractByName( "MoveBelowLeft", nNormalMove );
								bCanBeTaken = true;
							}

							if( processMoves.IsEnemyAboveRight( board, board.GetIdentifierBelowLeft( piece.SquareIdentifier ), piece.Player ) == true 
								&& processMoves.CanTakeBelowLeft( board, board.GetIdentifierAboveRight( board.GetIdentifierBelowLeft( piece.SquareIdentifier ) ), "PLAYER" ) == true )
							{
								decisionSet.SubtractByName( "MoveBelowLeft", nNormalMove );
								bCanBeTaken = true;
							}

							if( bCanBeTaken == false )
							{
								decisionSet.AddByName( "MoveBelowLeft", nNormalMove );
								bDecisionIncremented = true;
							}
							else
								bDecisionDecremented = true;
						}
						else
						{
							if( processMoves.IsEnemyBelowLeft( board, board.GetIdentifierBelowLeft( piece.SquareIdentifier ), piece.Player ) == true 
								&& processMoves.CanTakeAboveLeft( board, board.GetIdentifierBelowLeft( board.GetIdentifierBelowLeft( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
							{
								decisionSet.SubtractByName( "MoveBelowLeft", nNormalMove );
								bCanBeTaken = true;
							}
							
							if( processMoves.IsEnemyBelowRight( board, board.GetIdentifierBelowLeft( piece.SquareIdentifier ), piece.Player ) == true
								&& processMoves.CanTakeAboveRight( board, board.GetIdentifierBelowRight( board.GetIdentifierBelowLeft( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
							{
								decisionSet.SubtractByName( "MoveBelowLeft", nNormalMove );
								bCanBeTaken = true;
							}

							if( processMoves.IsEnemyAboveRight( board, board.GetIdentifierBelowLeft( piece.SquareIdentifier ), piece.Player ) == true 
								&& processMoves.CanTakeBelowLeft( board, board.GetIdentifierAboveRight( board.GetIdentifierBelowLeft( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
							{
								decisionSet.SubtractByName( "MoveBelowLeft", nNormalMove );
								bCanBeTaken = true;
							}

							if( bCanBeTaken == false )
							{
								decisionSet.AddByName( "MoveBelowLeft", nNormalMove );
								bDecisionIncremented = true;
							}
							else
								bDecisionDecremented = true;
						}
					}

					bCanBeTaken = false;

					if( decisionSet.IsValidByName( "MoveBelowRight" ) == true )
					{
						if( piece.Player == "COMPUTER" )
						{
							if( processMoves.IsEnemyBelowLeft( board, board.GetIdentifierBelowRight( piece.SquareIdentifier ), piece.Player ) == true
								&& processMoves.CanTakeAboveRight( board, board.GetIdentifierBelowLeft( board.GetIdentifierBelowRight( piece.SquareIdentifier ) ), "PLAYER" ) == true )
							{
								decisionSet.SubtractByName( "MoveBelowRight", nNormalMove );
								bCanBeTaken = true;
							}

							if( processMoves.IsEnemyBelowRight( board, board.GetIdentifierBelowRight( piece.SquareIdentifier ), piece.Player ) == true
								&& processMoves.CanTakeAboveLeft( board, board.GetIdentifierBelowRight( board.GetIdentifierBelowRight( piece.SquareIdentifier ) ), "PLAYER" ) == true )
							{
								decisionSet.SubtractByName( "MoveBelowRight", nNormalMove );
								bCanBeTaken = true;
							}

							if( processMoves.IsEnemyAboveLeft( board, board.GetIdentifierBelowRight( piece.SquareIdentifier ), piece.Player ) == true 
								&& processMoves.CanTakeBelowLeft( board, board.GetIdentifierAboveLeft( board.GetIdentifierBelowRight( piece.SquareIdentifier ) ), "PLAYER" ) == true )
							{
								decisionSet.SubtractByName( "MoveBelowRight", nNormalMove );
								bCanBeTaken = true;
							}

							if( bCanBeTaken == false )
							{
								decisionSet.AddByName( "MoveBelowRight", nNormalMove );
								bDecisionIncremented = true;
							}
							else
								bDecisionDecremented = true;
						}
						else
						{

							if( processMoves.IsEnemyBelowLeft( board, board.GetIdentifierBelowRight( piece.SquareIdentifier ), piece.Player ) == true
								&& processMoves.CanTakeAboveRight( board, board.GetIdentifierBelowLeft( board.GetIdentifierBelowRight( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
							{
								decisionSet.SubtractByName( "MoveBelowRight", nNormalMove );
								bCanBeTaken = true;
							}

							if( processMoves.IsEnemyBelowRight( board, board.GetIdentifierBelowRight( piece.SquareIdentifier ), piece.Player ) == true
								&& processMoves.CanTakeAboveLeft( board, board.GetIdentifierBelowRight( board.GetIdentifierBelowRight( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
							{
								decisionSet.SubtractByName( "MoveBelowRight", nNormalMove );
								bCanBeTaken = true;
							}

							if( processMoves.IsEnemyAboveLeft( board, board.GetIdentifierBelowRight( piece.SquareIdentifier ), piece.Player ) == true 
								&& processMoves.CanTakeBelowLeft( board, board.GetIdentifierAboveLeft( board.GetIdentifierBelowRight( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
							{
								decisionSet.SubtractByName( "MoveBelowRight", nNormalMove );
								bCanBeTaken = true;
							}

							if( bCanBeTaken == false )
							{
								decisionSet.AddByName( "MoveBelowRight", nNormalMove );
								bDecisionIncremented = true;
							}
							else
								bDecisionDecremented = true;
						}									
					}


					if( bDecisionIncremented == true )
					{
						decisionSet.Decision.AddToDecision( nNormalMove );
					}

					if( bDecisionDecremented == true )
					{
						decisionSet.Decision.SubtractFromDecision( nNormalMove );
					}
				}


				/// check to see if sacrificing a piece will result in gains for move
				/// 

				bool bCanSacrifice = false;

				for( int i=0; i<collection.Count; i++ )
				{
					bDecisionIncremented = false;
					decisionSet = ( FuzzyDecisionSet )collection[ i ];

					/// make sure that we are dealling with patterns for this move.
					/// 
					pattern = ( DraughtsPattern )availablePatterns.GetPattern( i );

					if( pattern.MoveNumber != nMoveNumber )
						continue;

					piece = pattern.GetStartsWithPiece( nMoveNumber );

					for( int n=0; n<piece.Moves.Count; n++ )
					{
						move = ( DraughtsMove )piece.Moves[ n ];

						if( decisionSet.GetNumberByName( "MoveAboveLeft" ) > 0 
							&& decisionSet.GetValidByName( "MoveAboveLeft" ) == true 
							&& board.GetIdentifierAboveLeft( piece.SquareIdentifier ) == move.Identifier )
						{
							if( processMoves.IsEnemyAboveLeft( board, move.Identifier, piece.Player ) == true )
							{
								if( processMoves.IsFriendlyBelowRight( board, piece.SquareIdentifier, piece.Player ) == true )
								{
									if( board.GetIdentifierBelowRight( board.GetIdentifierBelowRight( piece.SquareIdentifier ) ) == null 
										|| processMoves.IsFriendlyBelowRight( board, board.GetIdentifierBelowRight( piece.SquareIdentifier ), piece.Player ) == true )
									{
										bCanSacrifice = true;
									}
								}
							}

							if( processMoves.IsEnemyAboveRight( board, move.Identifier, piece.Player ) == true )
							{
								if( processMoves.IsBelowLeftClear( board, move.Identifier ) == true )
								{
									if( processMoves.IsFriendlyBelowLeft( board, board.GetIdentifierBelowLeft( move.Identifier ), piece.Player ) == true )
									{
										if( board.GetIdentifierBelowLeft( board.GetIdentifierBelowLeft( board.GetIdentifierBelowLeft( move.Identifier ) ) ) == null 
											|| processMoves.IsFriendlyBelowLeft( board, board.GetIdentifierBelowLeft( board.GetIdentifierBelowLeft( move.Identifier ) ), piece.Player ) == true )
										{
											bCanSacrifice = true;
										}
									}
								}
							}

							/// only want to increment once for both of the above.
							/// 
							if( bCanSacrifice == true )
							{
								decisionSet.AddByName( "MoveAboveLeft", nSacrificeMove );
								bCanSacrifice = false;
							}

							/// now see if we can sacrifice for a king piece
							/// 

							if( processMoves.IsEnemyKingBelowLeft( board, move.Identifier, piece.Player ) == true )
							{
								if( processMoves.IsAboveRightClear( board, move.Identifier ) == true )
								{
									if( processMoves.IsFriendlyAboveRight( board, board.GetIdentifierAboveRight( move.Identifier ), piece.Player ) == true )
									{
										if( board.GetIdentifierAboveRight( board.GetIdentifierAboveRight( board.GetIdentifierAboveRight( move.Identifier ) ) ) == null 
											|| processMoves.IsFriendlyAboveRight( board, board.GetIdentifierAboveRight( board.GetIdentifierAboveRight( move.Identifier ) ), piece.Player ) == true )
										{
											bCanSacrifice = true;
										}
									}
								}
							}

							if( bCanSacrifice == true )
							{
								decisionSet.AddByName( "MoveAboveLeft", nSacrificeMove );
								bCanSacrifice = false;
								bDecisionIncremented = true;
							}
						}

						/// Above Right
						/// 

						if( decisionSet.GetNumberByName( "MoveAboveRight" ) > 0
							&& decisionSet.GetValidByName( "MoveAboveRight" ) == true 
							&& board.GetIdentifierAboveRight( piece.SquareIdentifier ) == move.Identifier )
						{
							if( processMoves.IsEnemyAboveRight( board, move.Identifier, piece.Player ) == true )
							{
								if( processMoves.IsFriendlyBelowLeft( board, piece.SquareIdentifier, piece.Player ) == true )
								{
									if( board.GetIdentifierBelowLeft( board.GetIdentifierBelowLeft( board.GetIdentifierBelowLeft( piece.SquareIdentifier ) ) ) == null 
										|| processMoves.IsFriendlyBelowLeft( board, board.GetIdentifierBelowLeft( board.GetIdentifierBelowLeft( piece.SquareIdentifier ) ), piece.Player ) == true )
									{
										bCanSacrifice = true;
									}
								}
							}

							if( processMoves.IsEnemyAboveLeft( board, move.Identifier, piece.Player ) == true )
							{
								if( processMoves.IsBelowRightClear( board, move.Identifier ) == true )
								{
									if( processMoves.IsFriendlyBelowRight( board, board.GetIdentifierBelowRight( move.Identifier ), piece.Player ) == true )
									{
										if( board.GetIdentifierBelowRight( board.GetIdentifierBelowRight( board.GetIdentifierBelowRight( move.Identifier ) ) ) == null
											|| processMoves.IsFriendlyBelowRight( board, board.GetIdentifierBelowRight( board.GetIdentifierBelowRight( move.Identifier ) ), piece.Player ) == true )
										{
											bCanSacrifice = true;
										}
									}
								}
							}

							/// only want to increment once for both of the above.
							/// 
							if( bCanSacrifice == true )
							{
								decisionSet.AddByName( "MoveAboveRight", nSacrificeMove );
								bCanSacrifice = false;
							}

							/// now see if we can sacrifice for a king piece
							/// 

							if( processMoves.IsEnemyKingBelowRight( board, move.Identifier, piece.Player ) == true )
							{
								if( processMoves.IsAboveLeftClear( board, move.Identifier ) == true )
								{
									if( processMoves.IsFriendlyAboveLeft( board, board.GetIdentifierAboveLeft( move.Identifier ), piece.Player ) == true )
									{
										if( board.GetIdentifierAboveLeft( board.GetIdentifierAboveLeft( board.GetIdentifierAboveLeft( move.Identifier ) ) ) == null 
											|| processMoves.IsFriendlyAboveLeft( board, board.GetIdentifierAboveLeft( board.GetIdentifierAboveLeft( move.Identifier ) ), piece.Player ) == true )
										{
											bCanSacrifice = true;
										}
									}
								}
							}

							if( bCanSacrifice == true )
							{
								/// do want to take kings if poss
								/// 
								decisionSet.AddByName( "MoveAboveRight", nSacrificeMove + 1 );
								bCanSacrifice = false;
								bDecisionIncremented = true;
							}
						}

						/// Below Left 
						/// 

						if( decisionSet.GetNumberByName( "MoveBelowLeft" ) > 0 
							&& decisionSet.GetValidByName( "MoveBelowLeft" ) == true 
							&& board.GetIdentifierBelowLeft( piece.SquareIdentifier ) == move.Identifier )
						{
							if( processMoves.IsEnemyBelowLeft( board, move.Identifier, piece.Player ) == true )
							{
								if( processMoves.IsFriendlyAboveRight( board, piece.SquareIdentifier, piece.Player ) == true )
								{
									if( board.GetIdentifierAboveRight( board.GetIdentifierAboveRight( piece.SquareIdentifier ) ) == null
										|| processMoves.IsFriendlyAboveRight( board, board.GetIdentifierAboveRight( piece.SquareIdentifier ), piece.Player ) == true )
									{
										bCanSacrifice = true;
									}
								}
							}

							if( processMoves.IsEnemyBelowRight( board, move.Identifier, piece.Player ) == true )
							{
								if( processMoves.IsAboveLeftClear( board, move.Identifier ) == true )
								{
									if( processMoves.IsFriendlyAboveLeft( board, board.GetIdentifierAboveLeft( move.Identifier ), piece.Player ) == true )
									{
										if( board.GetIdentifierAboveLeft( board.GetIdentifierAboveLeft( board.GetIdentifierAboveLeft( move.Identifier ) ) ) == null 
											|| processMoves.IsFriendlyAboveLeft( board, board.GetIdentifierAboveLeft( board.GetIdentifierAboveLeft( move.Identifier ) ), piece.Player ) == true )
										{
											bCanSacrifice = true;
										}
									}
								}
							}

							/// only want to increment once for both of the above.
							/// 
							if( bCanSacrifice == true )
							{
								decisionSet.AddByName( "MoveBelowLeft", nSacrificeMove );
								bCanSacrifice = false;
								bDecisionIncremented = true;
							}

							/// now see if we can sacrifice for a king piece
							/// 

							if( processMoves.IsEnemyKingAboveLeft( board, move.Identifier, piece.Player ) == true )
							{
								if( processMoves.IsBelowRightClear( board, move.Identifier ) == true )
								{
									if( processMoves.IsFriendlyBelowRight( board, board.GetIdentifierBelowRight( move.Identifier ), piece.Player ) == true )
									{
										if( board.GetIdentifierBelowRight( board.GetIdentifierBelowRight( board.GetIdentifierBelowRight( move.Identifier ) ) ) == null 
											|| processMoves.IsFriendlyBelowRight( board, board.GetIdentifierBelowRight( board.GetIdentifierBelowRight( move.Identifier ) ), piece.Player ) == true )
										{
											bCanSacrifice = true;
										}
									}
								}
							}

							if( bCanSacrifice == true )
							{
								decisionSet.AddByName( "MoveBelowLeft", nSacrificeMove + 1 );
								bCanSacrifice = false;
								bDecisionIncremented = true;
							}
						}

						/// Below Right
						/// 

						if( decisionSet.GetNumberByName( "MoveBelowRight" ) > 0
							&& decisionSet.GetValidByName( "MoveBelowRight" ) == true 
							&& board.GetIdentifierBelowRight( piece.SquareIdentifier ) == move.Identifier )
						{
							if( processMoves.IsEnemyBelowRight( board, move.Identifier, piece.Player ) == true )
							{
								if( processMoves.IsFriendlyAboveLeft( board, piece.SquareIdentifier, piece.Player ) == true )
								{
									if( board.GetIdentifierAboveLeft( board.GetIdentifierAboveLeft( piece.SquareIdentifier ) ) == null 
										|| processMoves.IsFriendlyAboveLeft( board, board.GetIdentifierAboveLeft( piece.SquareIdentifier ), piece.Player ) == true )
									{
										bCanSacrifice = true;
									}
								}
							}

							if( processMoves.IsEnemyBelowLeft( board, move.Identifier, piece.Player ) == true )
							{
								if( processMoves.IsAboveRightClear( board, move.Identifier ) == true )
								{
									if( processMoves.IsFriendlyAboveRight( board, board.GetIdentifierAboveRight( move.Identifier ), piece.Player ) == true )
									{
										if( board.GetIdentifierAboveRight( board.GetIdentifierAboveRight( board.GetIdentifierAboveRight( move.Identifier ) ) ) == null 
											|| processMoves.IsFriendlyAboveRight( board, board.GetIdentifierAboveRight( board.GetIdentifierAboveRight( move.Identifier ) ), piece.Player ) == true )
										{
											bCanSacrifice = true;
										}
									}
								}
							}

							/// only want to increment once for both of the above.
							/// 
							if( bCanSacrifice == true )
							{
								decisionSet.AddByName( "MoveBelowRight", nSacrificeMove );
								bCanSacrifice = false;
							}

							/// now see if we can sacrifice for a king piece
							/// 

							if( processMoves.IsEnemyKingAboveRight( board, move.Identifier, piece.Player ) == true )
							{
								if( processMoves.IsBelowLeftClear( board, move.Identifier ) == true )
								{
									if( processMoves.IsFriendlyBelowLeft( board, board.GetIdentifierBelowLeft( move.Identifier ), piece.Player ) == true )
									{
										if( board.GetIdentifierBelowLeft( board.GetIdentifierBelowLeft( board.GetIdentifierBelowLeft( move.Identifier ) ) ) == null 
											|| processMoves.IsFriendlyBelowLeft( board, board.GetIdentifierBelowLeft( board.GetIdentifierBelowLeft( move.Identifier ) ), piece.Player ) == true ) 
										{
											bCanSacrifice = true;
										}
									}
								}
							}

							if( bCanSacrifice == true )
							{
								decisionSet.AddByName( "MoveBelowRight", nSacrificeMove + 1 );
								bCanSacrifice = false;
								bDecisionIncremented = true;
							}
						}
					}

					if( bDecisionIncremented == true )
					{
						decisionSet.Decision.AddToDecision( nSacrificeMove );
					}
				}

				/// check to see if you can add a support move to prevent a piece being taken
				///  

				bool bIncrement = false;

				for( int i=0; i<collection.Count; i++ )
				{
					bDecisionIncremented = false;
					decisionSet = ( FuzzyDecisionSet )collection[ i ];

					/// make sure that we are dealling with patterns for this move.
					/// 
					pattern = ( DraughtsPattern )availablePatterns.GetPattern( i );

					if( pattern.MoveNumber != nMoveNumber )
						continue;

					piece = pattern.GetStartsWithPiece( nMoveNumber );
					for( int n=0; n<piece.Moves.Count; n++ )
					{
						move = ( DraughtsMove )piece.Moves[ n ];
					
						if( decisionSet.GetNumberByName( "MoveAboveLeft" ) > 0 
							&& decisionSet.GetValidByName( "MoveAboveLeft" ) == true 
							&& board.GetIdentifierAboveLeft( piece.SquareIdentifier ) == move.Identifier )
						{
							if( processMoves.IsFriendlyAboveLeft( board, move.Identifier, piece.Player ) == true )
								bIncrement = true;

							if( processMoves.IsFriendlyAboveRight( board, move.Identifier, piece.Player ) == true )
								bIncrement = true;

							if( bIncrement == true )
							{
								bIncrement = false;
								decisionSet.AddByName( "MoveAboveLeft", nSupportMove );
								bDecisionIncremented = true;

								/// extra influence if one is more immeadiately threatened
								/// 

								if( processMoves.IsFriendlyAboveLeft( board, move.Identifier, piece.Player ) == true )
								{
									if( processMoves.IsEnemyAboveLeft( board, board.GetIdentifierAboveLeft( move.Identifier ), piece.Player ) == true )
										bIncrement = true;
								}

								if( bIncrement == true )
								{
									bIncrement = false;
									decisionSet.AddByName( "MoveAboveLeft", nSupportMove );
								}
							}
						}

						if( decisionSet.GetNumberByName( "MoveAboveRight" ) > 0 
							&& decisionSet.GetValidByName( "MoveAboveRight" ) == true 
							&& board.GetIdentifierAboveRight( piece.SquareIdentifier ) == move.Identifier )
						{
							if( processMoves.IsFriendlyAboveLeft( board, move.Identifier, piece.Player ) == true )
								bIncrement = true;

							if( processMoves.IsFriendlyAboveRight( board, move.Identifier, piece.Player ) == true )
								bIncrement = true;

							if( bIncrement == true ) 
							{
								bIncrement = false;
								decisionSet.AddByName( "MoveAboveRight", nSupportMove );
								bDecisionIncremented = true;

								if( processMoves.IsFriendlyAboveRight( board, move.Identifier, piece.Player ) == true )
								{
									if( processMoves.IsEnemyAboveRight( board, board.GetIdentifierAboveRight( move.Identifier ), piece.Player ) == true )
										bIncrement = true;
								}

								if( bIncrement == true )
								{
									bIncrement = false;
									decisionSet.AddByName( "MoveAboveRight", nSupportMove );
								}
							}
						}

						if( decisionSet.GetNumberByName( "MoveBelowLeft" ) > 0
							&& decisionSet.GetValidByName( "MoveBelowLeft" ) == true 
							&& board.GetIdentifierBelowLeft( piece.SquareIdentifier ) == move.Identifier )
						{
							if( processMoves.IsFriendlyBelowLeft( board, move.Identifier, piece.Player ) == true )
								bIncrement = true;

							if( processMoves.IsFriendlyBelowRight( board, move.Identifier, piece.Player ) == true )
								bIncrement = true;

							if( bIncrement == true )
							{
								bIncrement = false;
								decisionSet.AddByName( "MoveBelowLeft", nSupportMove );
								bDecisionIncremented = true;

								if( processMoves.IsFriendlyBelowLeft( board, move.Identifier, piece.Player ) == true )
								{
									if( processMoves.IsEnemyBelowLeft( board, board.GetIdentifierBelowLeft( move.Identifier ), piece.Player ) == true )
										bIncrement = true;
								}

								if( bIncrement == true )
								{
									bIncrement = false;
									decisionSet.AddByName( "MoveBelowLeft", nSupportMove );
								}
							}
						}

						if( decisionSet.GetNumberByName( "MoveBelowRight" ) > 0 
							&& decisionSet.GetValidByName( "MoveBelowRight" ) == true 
							&& board.GetIdentifierBelowRight( piece.SquareIdentifier ) == move.Identifier )
						{
							if( processMoves.IsFriendlyBelowLeft( board, move.Identifier, piece.Player ) == true )
								bIncrement = true;

							if( processMoves.IsFriendlyBelowRight( board, move.Identifier, piece.Player ) == true )
								bIncrement = true;

							if( bIncrement == true )
							{
								bIncrement = false;
								decisionSet.AddByName( "MoveBelowRight", nSupportMove );
								bDecisionIncremented = true;

								if( processMoves.IsFriendlyBelowRight( board, move.Identifier, piece.Player ) == true )
								{
									if( processMoves.IsEnemyBelowRight( board, board.GetIdentifierBelowRight( move.Identifier ), piece.Player ) == true )
										bIncrement = true;
								}

								if( bIncrement == true )
								{
									bIncrement = false;
									decisionSet.AddByName( "MoveBelowRight", nSupportMove );
								}
							}
						}
					}

					if( bDecisionIncremented == true )
					{
						decisionSet.Decision.AddToDecision( nSupportMove );
					}
				}

				/// check to see if we a move is exposing one of our pieces to being taken
				/// 

				for( int i=0; i<collection.Count; i++ )
				{
					bDecisionDecremented = false;
					bool bDecrement = false;
					
					decisionSet = ( FuzzyDecisionSet )collection[ i ];

					/// make sure that we are dealling with patterns for this move.
					/// 
					pattern = ( DraughtsPattern )availablePatterns.GetPattern( i );

					if( pattern.MoveNumber != nMoveNumber )
						continue;

					piece = pattern.GetStartsWithPiece( nMoveNumber );
					
					if( decisionSet.GetValidByName( "MoveAboveLeft" ) == true )
					{
						if( piece.Player == "COMPUTER" )
						{
							if( processMoves.IsEnemyAboveLeft( board, board.GetIdentifierAboveLeft( piece.SquareIdentifier ), piece.Player ) == true
								&& processMoves.CanTakeBelowRight( board, board.GetIdentifierAboveLeft( board.GetIdentifierAboveLeft( piece.SquareIdentifier ) ), "PLAYER" ) == true )
							{
								bDecrement = true;
							}

							if( processMoves.IsEnemyAboveRight( board, board.GetIdentifierAboveLeft( piece.SquareIdentifier ), piece.Player ) == true
								&& processMoves.CanTakeBelowLeft( board, board.GetIdentifierAboveRight( board.GetIdentifierAboveLeft( piece.SquareIdentifier ) ), "PLAYER" ) == true )
							{
								bDecrement = true;
							}

							if( processMoves.IsEnemyBelowLeft( board, board.GetIdentifierAboveLeft( piece.SquareIdentifier ), piece.Player ) == true 
								&& processMoves.CanTakeAboveRight( board, board.GetIdentifierBelowLeft( board.GetIdentifierAboveLeft( piece.SquareIdentifier ) ), "PLAYER" ) == true )
							{
								bDecrement = true;
							}
						}
						else
						{
							if( processMoves.IsEnemyAboveLeft( board, board.GetIdentifierAboveLeft( piece.SquareIdentifier ), piece.Player ) == true
								&& processMoves.CanTakeBelowRight( board, board.GetIdentifierAboveLeft( board.GetIdentifierAboveLeft( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
							{
								bDecrement = true;
							}

							if( processMoves.IsEnemyAboveRight( board, board.GetIdentifierAboveLeft( piece.SquareIdentifier ), piece.Player ) == true
								&& processMoves.CanTakeBelowLeft( board, board.GetIdentifierAboveRight( board.GetIdentifierAboveLeft( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
							{
								bDecrement = true;
							}

							if( processMoves.IsEnemyBelowLeft( board, board.GetIdentifierAboveLeft( piece.SquareIdentifier ), piece.Player ) == true 
								&& processMoves.CanTakeAboveRight( board, board.GetIdentifierBelowLeft( board.GetIdentifierAboveLeft( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
							{
								bDecrement = true;
							}
						}

						if( bDecrement == true )
						{
							bDecrement = false;
							decisionSet.SubtractByName( "MoveAboveLeft", nLeavingOpenMove );
							bDecisionDecremented = true;
						}
					}

					if( decisionSet.GetValidByName( "MoveAboveRight" ) == true )
					{
						if( piece.Player == "COMPUTER" )
						{
							if( processMoves.IsEnemyAboveLeft( board, board.GetIdentifierAboveRight( piece.SquareIdentifier ), piece.Player ) == true 
								&& processMoves.CanTakeBelowLeft( board, board.GetIdentifierAboveLeft( board.GetIdentifierAboveRight( piece.SquareIdentifier ) ), "PLAYER" ) == true )
							{
								bDecrement = true;
							}

							if( processMoves.IsEnemyAboveRight( board, board.GetIdentifierAboveRight( piece.SquareIdentifier ), piece.Player ) == true
								&& processMoves.CanTakeBelowLeft( board, board.GetIdentifierAboveRight( board.GetIdentifierAboveRight( piece.SquareIdentifier ) ), "PLAYER" ) == true )
							{
								bDecrement = true;
							}

							if( processMoves.IsEnemyBelowRight( board, board.GetIdentifierAboveRight( piece.SquareIdentifier ), piece.Player ) == true
								&& processMoves.CanTakeAboveLeft( board, board.GetIdentifierBelowRight( board.GetIdentifierAboveRight( piece.SquareIdentifier ) ), "PLAYER" ) == true )
							{
								bDecrement = true;
							}
						}
						else
						{
							if( processMoves.IsEnemyAboveLeft( board, board.GetIdentifierAboveRight( piece.SquareIdentifier ), piece.Player ) == true 
								&& processMoves.CanTakeBelowLeft( board, board.GetIdentifierAboveLeft( board.GetIdentifierAboveRight( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
							{
								bDecrement = true;
							}

							if( processMoves.IsEnemyAboveRight( board, board.GetIdentifierAboveRight( piece.SquareIdentifier ), piece.Player ) == true
								&& processMoves.CanTakeBelowLeft( board, board.GetIdentifierAboveRight( board.GetIdentifierAboveRight( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
							{
								bDecrement = true;
							}

							if( processMoves.IsEnemyBelowRight( board, board.GetIdentifierAboveRight( piece.SquareIdentifier ), piece.Player ) == true
								&& processMoves.CanTakeAboveLeft( board, board.GetIdentifierBelowRight( board.GetIdentifierAboveRight( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
							{
								bDecrement = true;
							}
						}
																																													 
						if( bDecrement == true )
						{
							bDecrement = false;
							decisionSet.SubtractByName( "MoveAboveRight", nLeavingOpenMove );
							bDecisionDecremented = true;
						}
					}

					if( decisionSet.GetValidByName( "MoveBelowLeft" ) == true )
					{
						if( piece.Player == "COMPUTER" )
						{
							if( processMoves.IsEnemyBelowLeft( board, board.GetIdentifierBelowLeft( piece.SquareIdentifier ), piece.Player ) == true
								&& processMoves.CanTakeAboveRight( board, board.GetIdentifierBelowLeft( board.GetIdentifierBelowLeft( piece.SquareIdentifier ) ), "PLAYER" ) == true )
							{
								bDecrement = true;
							}

							if( processMoves.IsEnemyBelowRight( board, board.GetIdentifierBelowLeft( piece.SquareIdentifier ), piece.Player ) == true
								&& processMoves.CanTakeAboveLeft( board, board.GetIdentifierBelowRight( board.GetIdentifierBelowLeft( piece.SquareIdentifier ) ), "PLAYER" ) == true )
							{
								bDecrement = true;
							}

							if( processMoves.IsEnemyAboveRight( board, board.GetIdentifierBelowLeft( piece.SquareIdentifier ), piece.Player ) == true 
								&& processMoves.CanTakeBelowRight( board, board.GetIdentifierAboveRight( board.GetIdentifierBelowLeft( piece.SquareIdentifier ) ), "PLAYER" ) == true )
							{
								bDecrement = true;
							}
						}
						else
						{
							if( processMoves.IsEnemyBelowLeft( board, board.GetIdentifierBelowLeft( piece.SquareIdentifier ), piece.Player ) == true
								&& processMoves.CanTakeAboveRight( board, board.GetIdentifierBelowLeft( board.GetIdentifierBelowLeft( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
							{
								bDecrement = true;
							}

							if( processMoves.IsEnemyBelowRight( board, board.GetIdentifierBelowLeft( piece.SquareIdentifier ), piece.Player ) == true
								&& processMoves.CanTakeAboveLeft( board, board.GetIdentifierBelowRight( board.GetIdentifierBelowLeft( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
							{
								bDecrement = true;
							}

							if( processMoves.IsEnemyAboveRight( board, board.GetIdentifierBelowLeft( piece.SquareIdentifier ), piece.Player ) == true 
								&& processMoves.CanTakeBelowRight( board, board.GetIdentifierAboveRight( board.GetIdentifierBelowLeft( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
							{
								bDecrement = true;
							}
						}

						if( bDecrement == true )
						{
							bDecrement = false;
							decisionSet.SubtractByName( "MoveBelowLeft", nLeavingOpenMove );
							bDecisionDecremented = true;
						}
					}

					if( decisionSet.GetValidByName( "MoveBelowRight" ) == true )
					{
						if( piece.Player == "COMPUTER" )
						{
							if( processMoves.IsEnemyBelowLeft( board, board.GetIdentifierBelowRight( piece.SquareIdentifier ), piece.Player ) == true
								&& processMoves.CanTakeAboveRight( board, board.GetIdentifierBelowLeft( board.GetIdentifierBelowRight( piece.SquareIdentifier ) ), "PLAYER" ) == true )
							{
								bDecrement = true;
							}

							if( processMoves.IsEnemyBelowRight( board, board.GetIdentifierBelowRight( piece.SquareIdentifier ), piece.Player ) == true 
								&& processMoves.CanTakeAboveLeft( board, board.GetIdentifierBelowRight( board.GetIdentifierBelowRight( piece.SquareIdentifier ) ), "PLAYER" ) == true )
							{
								bDecrement = true;
							}

							if( processMoves.IsEnemyAboveLeft( board, board.GetIdentifierBelowRight( piece.SquareIdentifier ), piece.Player ) == true 
								&& processMoves.CanTakeBelowRight( board, board.GetIdentifierAboveLeft( board.GetIdentifierBelowRight( piece.SquareIdentifier ) ), "PLAYER" ) == true )
							{
								bDecrement = true;
							}
						}
						else
						{
							if( processMoves.IsEnemyBelowLeft( board, board.GetIdentifierBelowRight( piece.SquareIdentifier ), piece.Player ) == true
								&& processMoves.CanTakeAboveRight( board, board.GetIdentifierBelowLeft( board.GetIdentifierBelowRight( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
							{
								bDecrement = true;
							}

							if( processMoves.IsEnemyBelowRight( board, board.GetIdentifierBelowRight( piece.SquareIdentifier ), piece.Player ) == true 
								&& processMoves.CanTakeAboveLeft( board, board.GetIdentifierBelowRight( board.GetIdentifierBelowRight( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
							{
								bDecrement = true;
							}

							if( processMoves.IsEnemyAboveLeft( board, board.GetIdentifierBelowRight( piece.SquareIdentifier ), piece.Player ) == true 
								&& processMoves.CanTakeBelowRight( board, board.GetIdentifierAboveLeft( board.GetIdentifierBelowRight( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
							{
								bDecrement = true;
							}
						}

						if( bDecrement == true )
						{
							bDecrement = false;
							decisionSet.SubtractByName( "MoveBelowRight", nLeavingOpenMove );
							bDecisionDecremented = true;
						}
					}

					if( bDecisionDecremented == true )
					{
						decisionSet.Decision.SubtractFromDecision( nLeavingOpenMove );
					}
				}

				/// check to see if a piece will be taken if it isn't moved
				/// 

				for( int i=0; i<collection.Count; i++ )
				{
					bDecisionIncremented = false;
					bIncrement = false;
					
					decisionSet = ( FuzzyDecisionSet )collection[ i ];

					/// make sure that we are dealling with patterns for this move.
					/// 
					pattern = ( DraughtsPattern )availablePatterns.GetPattern( i );

					if( pattern.MoveNumber != nMoveNumber )
						continue;

					piece = pattern.GetStartsWithPiece( nMoveNumber );

					if( piece.LightPiece == true && piece.LightPiece == light )
					{
						if( processMoves.IsEnemyBelowLeft( board, piece.SquareIdentifier, piece.Player ) == true 
							|| processMoves.IsEnemyBelowRight( board, piece.SquareIdentifier, piece.Player ) == true )
						{
							if( processMoves.IsEnemyBelowRight( board, board.GetIdentifierAboveRight( piece.SquareIdentifier ), piece.Player ) == true 
								|| processMoves.IsEnemyKingAboveLeft( board, board.GetIdentifierAboveRight( piece.SquareIdentifier ), piece.Player ) == true 
								|| processMoves.IsEnemyKingAboveRight( board, board.GetIdentifierAboveRight( piece.SquareIdentifier ), piece.Player ) == true )
							{
								decisionSet.SubtractByName( "MoveAboveRight", nOutOfWayMove );
							}
							else
							{
								decisionSet.AddByName( "MoveAboveRight", nOutOfWayMove );
								bIncrement = true;
							}

							if( processMoves.IsEnemyBelowLeft( board, board.GetIdentifierAboveLeft( piece.SquareIdentifier ), piece.Player ) == true 
								|| processMoves.IsEnemyKingAboveLeft( board, board.GetIdentifierAboveLeft( piece.SquareIdentifier ), piece.Player ) == true 
								|| processMoves.IsEnemyKingAboveRight( board, board.GetIdentifierAboveLeft( piece.SquareIdentifier ), piece.Player ) == true )
							{
								decisionSet.SubtractByName( "MoveAboveLeft", nOutOfWayMove );
							}
							else
							{
								decisionSet.AddByName( "MoveAboveLeft", nOutOfWayMove );
								bIncrement = true;
							}
						}

						if( bIncrement == true )
						{
							bIncrement = false;
							bDecisionIncremented = true;
						}
					}

					if( piece.LightPiece == false && piece.LightPiece == light )
					{
						if( processMoves.IsEnemyAboveLeft( board, piece.SquareIdentifier, piece.Player ) == true 
							|| processMoves.IsEnemyAboveRight( board, piece.SquareIdentifier, piece.Player ) == true )
						{
							if( processMoves.IsEnemyAboveLeft( board, board.GetIdentifierBelowLeft( piece.SquareIdentifier ), piece.Player ) == true
								|| processMoves.IsEnemyKingBelowLeft( board, board.GetIdentifierBelowLeft( piece.SquareIdentifier ), piece.Player ) == true 
								|| processMoves.IsEnemyKingBelowRight( board, board.GetIdentifierBelowLeft( piece.SquareIdentifier ), piece.Player ) == true )
							{
								decisionSet.SubtractByName( "MoveBelowLeft", nOutOfWayMove );
							}
							else
							{
								decisionSet.AddByName( "MoveBelowLeft", nOutOfWayMove );
								bIncrement = true;
							}

							if( processMoves.IsEnemyAboveRight( board, board.GetIdentifierBelowRight( piece.SquareIdentifier ), piece.Player ) == true 
								|| processMoves.IsEnemyKingBelowLeft( board, board.GetIdentifierBelowRight( piece.SquareIdentifier ), piece.Player ) == true 
								|| processMoves.IsEnemyKingBelowRight( board, board.GetIdentifierBelowRight( piece.SquareIdentifier ), piece.Player ) == true )
							{
								decisionSet.SubtractByName( "MoveBelowLeft", nOutOfWayMove );
							}
							else
							{
								decisionSet.AddByName( "MoveBelowLeft", nOutOfWayMove );
								bIncrement = true;
							}
						}

						if( bIncrement == true )
						{
							bIncrement = false;
							bDecisionIncremented = true;
						}
					}

					if( bDecisionIncremented == true )
					{
						decisionSet.Decision.AddToDecision( nOutOfWayMove );
					}
				}

				/// check to see if a move will result in getting a king piece.
				/// 

				for( int i=0; i<collection.Count; i++ )
				{
					bDecisionIncremented = false;

					decisionSet = ( FuzzyDecisionSet )collection[ i ];

					/// make sure that we are dealling with patterns for this move.
					/// 
					pattern = ( DraughtsPattern )availablePatterns.GetPattern( i );

					if( pattern.MoveNumber != nMoveNumber )
						continue;

					piece = pattern.GetStartsWithPiece( nMoveNumber );

					if( piece.IsKing == true )
						continue;

					for( int n=0; n<piece.Moves.Count; n++ )
					{
						move = ( DraughtsMove )piece.Moves[ n ];

						if( decisionSet.GetNumberByName( "MoveAboveLeft" ) > 0
							&& decisionSet.GetValidByName( "MoveBelowLeft" ) == true )
						{
							if( move != null && move.Identifier[ 0 ] == 'A' )
							{
								decisionSet.AddByName( "MoveAboveLeft", nKingMove );
								bDecisionIncremented = true;
							}
						}

						if( decisionSet.GetNumberByName( "MoveAboveRight" ) > 0 
							&& decisionSet.GetValidByName( "MoveAboveRight" ) == true )
						{
							if( move != null && move.Identifier[ 0 ] == 'A' )
							{
								decisionSet.AddByName( "MoveAboveRight", nKingMove );
								bDecisionIncremented = true;
							}
						}

						if( decisionSet.GetNumberByName( "MoveBelowLeft" ) > 0 
							&& decisionSet.GetValidByName( "MoveBelowLeft" ) == true )
						{
							if( move != null && move.Identifier[ 0 ] == 'H' )
							{
								decisionSet.AddByName( "MoveAboveLeft", nKingMove );
								bDecisionIncremented = true;
							}
						}

						if( decisionSet.GetNumberByName( "MoveBelowRight" ) > 0 
							&& decisionSet.GetValidByName( "MoveBelowRight" ) == true )
						{
							if( move != null && move.Identifier[ 0 ] == 'H' )
							{
								decisionSet.AddByName( "MoveBelowRight", nKingMove );
								bDecisionIncremented = true;
							}
						}
					}

					if( bDecisionIncremented == true )
					{
						decisionSet.Decision.AddToDecision( nKingMove );
					}
				}

				/// if moving a king piece encourage it to move towards the enemy
				/// 

				for( int i=0; i<collection.Count; i++ )
				{
					bDecisionIncremented = false;
					bIncrement = false;
					decisionSet = ( FuzzyDecisionSet )collection[ i ];
					bCanBeTaken = false;

					/// make sure that we are dealling with patterns for this move.
					/// 
					pattern = ( DraughtsPattern )availablePatterns.GetPattern( i );

					if( pattern.MoveNumber != nMoveNumber )
						continue;

					piece = pattern.GetStartsWithPiece( nMoveNumber );
					move = null;

					if( piece.IsKing == true )
					{
						/// move above left
						/// 

						if( decisionSet.GetValidByName( "MoveAboveLeft" ) == true )
						{
							if( processMoves.IsEnemyGenerallyAboveLeft( board, piece.SquareIdentifier, piece.Player ) == true )
							{
								/// make sure the king isn't going to be taken
								/// 

								bCanBeTaken = false;

								if( piece.Player == "COMPUTER" )
								{
									if( processMoves.IsEnemyAboveRight( board, board.GetIdentifierAboveLeft( piece.SquareIdentifier ), piece.Player ) == true 
										&& processMoves.CanTakeBelowLeft( board, board.GetIdentifierAboveRight( board.GetIdentifierAboveLeft( piece.SquareIdentifier ) ), "PLAYER" ) == true )
									{
										bCanBeTaken = true;
									}
					
									if( processMoves.IsEnemyAboveLeft( board, board.GetIdentifierAboveLeft( piece.SquareIdentifier ), piece.Player ) == true 
										&& processMoves.CanTakeBelowRight( board, board.GetIdentifierAboveLeft( board.GetIdentifierAboveLeft( piece.SquareIdentifier ) ), "PLAYER" ) == true )
									{
										bCanBeTaken = true;
									}

									if( processMoves.IsEnemyBelowLeft( board, board.GetIdentifierAboveLeft( piece.SquareIdentifier ), piece.Player ) == true 
										&& processMoves.CanTakeAboveRight( board, board.GetIdentifierBelowLeft( board.GetIdentifierAboveLeft( piece.SquareIdentifier ) ), "PLAYER" ) == true )
									{
										bCanBeTaken = true;
									}

									if( bCanBeTaken == false )
									{
										decisionSet.AddByName( "MoveAboveLeft", nKingInfluenceMove );
										bDecisionIncremented = true;
									}
								}
								else
								{
									if( processMoves.IsEnemyAboveRight( board, board.GetIdentifierAboveLeft( piece.SquareIdentifier ), piece.Player ) == true 
										&& processMoves.CanTakeBelowLeft( board, board.GetIdentifierAboveRight( board.GetIdentifierAboveLeft( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
									{
										bCanBeTaken = true;
									}
					
									if( processMoves.IsEnemyAboveLeft( board, board.GetIdentifierAboveLeft( piece.SquareIdentifier ), piece.Player ) == true 
										&& processMoves.CanTakeBelowRight( board, board.GetIdentifierAboveLeft( board.GetIdentifierAboveLeft( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
									{
										bCanBeTaken = true;
									}

									if( processMoves.IsEnemyBelowLeft( board, board.GetIdentifierAboveLeft( piece.SquareIdentifier ), piece.Player ) == true 
										&& processMoves.CanTakeAboveRight( board, board.GetIdentifierBelowLeft( board.GetIdentifierAboveLeft( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
									{
										bCanBeTaken = true;
									}

									if( bCanBeTaken == false )
									{
										decisionSet.AddByName( "MoveAboveLeft", nKingInfluenceMove );
										bDecisionIncremented = true;
									}
								}
							}
						}

						if( decisionSet.GetValidByName( "MoveAboveRight" ) == true )
						{
							if( processMoves.IsEnemyGenerallyAboveRight( board, piece.SquareIdentifier, piece.Player ) == true )
							{
								bCanBeTaken = false;

								if( piece.Player ==	"COMPUTER" )
								{
									if( processMoves.IsEnemyAboveLeft( board, board.GetIdentifierAboveRight( piece.SquareIdentifier ), piece.Player ) == true 
										&& processMoves.CanTakeBelowRight( board, board.GetIdentifierAboveLeft( board.GetIdentifierAboveRight( piece.SquareIdentifier ) ), "PLAYER" ) == true )
									{
										bCanBeTaken = true;
									}

									if( processMoves.IsEnemyAboveRight( board, board.GetIdentifierAboveRight( piece.SquareIdentifier ), piece.Player ) == true 
										&& processMoves.CanTakeBelowLeft( board, board.GetIdentifierAboveRight( board.GetIdentifierAboveRight( piece.SquareIdentifier ) ), "PLAYER" ) == true )
									{
										bCanBeTaken = true;
									}

									if( processMoves.IsEnemyBelowRight( board, board.GetIdentifierAboveRight( piece.SquareIdentifier ), piece.Player ) == true 
										&& processMoves.CanTakeAboveLeft( board, board.GetIdentifierBelowRight( board.GetIdentifierAboveRight( piece.SquareIdentifier ) ), "PLAYER" ) == true )
									{
										bCanBeTaken = true;
									}

									if( bCanBeTaken == false )
									{
										decisionSet.AddByName( "MoveAboveRight", nKingInfluenceMove );
										bDecisionIncremented = true;
									}
								}
								else
								{
									if( processMoves.IsEnemyAboveLeft( board, board.GetIdentifierAboveRight( piece.SquareIdentifier ), piece.Player ) == true 
										&& processMoves.CanTakeBelowRight( board, board.GetIdentifierAboveLeft( board.GetIdentifierAboveRight( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
									{
										bCanBeTaken = true;
									}

									if( processMoves.IsEnemyAboveRight( board, board.GetIdentifierAboveRight( piece.SquareIdentifier ), piece.Player ) == true 
										&& processMoves.CanTakeBelowLeft( board, board.GetIdentifierAboveRight( board.GetIdentifierAboveRight( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
									{
										bCanBeTaken = true;
									}

									if( processMoves.IsEnemyBelowRight( board, board.GetIdentifierAboveRight( piece.SquareIdentifier ), piece.Player ) == true 
										&& processMoves.CanTakeAboveLeft( board, board.GetIdentifierBelowRight( board.GetIdentifierAboveRight( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
									{
										bCanBeTaken = true;
									}

									if( bCanBeTaken == false )
									{
										decisionSet.AddByName( "MoveAboveRight", nKingInfluenceMove );
										bDecisionIncremented = true;
									}
								}
							}
						}

						if( decisionSet.GetValidByName( "MoveBelowLeft" ) == true )
						{
							if( processMoves.IsEnemyGenerallyBelowLeft( board, piece.SquareIdentifier, piece.Player ) == true )
							{
								bCanBeTaken = false;

								if( piece.Player == "COMPUTER" )
								{
									if( processMoves.IsEnemyAboveRight( board, board.GetIdentifierBelowLeft( piece.SquareIdentifier ), piece.Player ) == true
										&& processMoves.CanTakeBelowLeft( board, board.GetIdentifierAboveRight( board.GetIdentifierBelowLeft( piece.SquareIdentifier ) ), "PLAYER" ) == true )
									{
										bCanBeTaken = true;
									}

									if( processMoves.IsEnemyBelowLeft( board, board.GetIdentifierBelowLeft( piece.SquareIdentifier ), piece.Player ) == true
										&& processMoves.CanTakeAboveRight( board, board.GetIdentifierBelowLeft( board.GetIdentifierBelowLeft( piece.SquareIdentifier ) ), "PLAYER" ) == true )
									{
										bCanBeTaken = true;
									}

									if( processMoves.IsEnemyBelowRight( board, board.GetIdentifierBelowLeft( piece.SquareIdentifier ), piece.Player ) == true
										&& processMoves.CanTakeAboveLeft( board, board.GetIdentifierBelowRight( board.GetIdentifierBelowLeft( piece.SquareIdentifier ) ), "PLAYER" ) == true )
									{
										bCanBeTaken = true;
									}

									if( bCanBeTaken == false )
									{
										decisionSet.AddByName( "MoveBelowLeft", nKingInfluenceMove );
										bDecisionIncremented = true;
									}
								}
								else
								{
									if( processMoves.IsEnemyAboveRight( board, board.GetIdentifierBelowLeft( piece.SquareIdentifier ), piece.Player ) == true
										&& processMoves.CanTakeBelowLeft( board, board.GetIdentifierAboveRight( board.GetIdentifierBelowLeft( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
									{
										bCanBeTaken = true;
									}

									if( processMoves.IsEnemyBelowLeft( board, board.GetIdentifierBelowLeft( piece.SquareIdentifier ), piece.Player ) == true
										&& processMoves.CanTakeAboveRight( board, board.GetIdentifierBelowLeft( board.GetIdentifierBelowLeft( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
									{
										bCanBeTaken = true;
									}

									if( processMoves.IsEnemyBelowRight( board, board.GetIdentifierBelowLeft( piece.SquareIdentifier ), piece.Player ) == true
										&& processMoves.CanTakeAboveLeft( board, board.GetIdentifierBelowRight( board.GetIdentifierBelowLeft( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
									{
										bCanBeTaken = true;
									}

									if( bCanBeTaken == false )
									{
										decisionSet.AddByName( "MoveBelowLeft", nKingInfluenceMove );
										bDecisionIncremented = true;
									}
								}
							}
						}

						if( decisionSet.GetValidByName( "MoveBelowRight" ) == true )
						{
							if( processMoves.IsEnemyGenerallyBelowRight( board, piece.SquareIdentifier, piece.Player ) == true )
							{
								bCanBeTaken = false;

								if( piece.Player == "COMPUTER" )
								{
									if( processMoves.IsEnemyAboveLeft( board, board.GetIdentifierBelowRight( piece.SquareIdentifier ), piece.Player ) == true 
										&& processMoves.CanTakeAboveRight( board, board.GetIdentifierAboveLeft( board.GetIdentifierBelowRight( piece.SquareIdentifier ) ), "PLAYER" ) == true )
									{
										bCanBeTaken = true;
									}

									if( processMoves.IsEnemyBelowLeft( board, board.GetIdentifierBelowRight( piece.SquareIdentifier ), piece.Player ) == true 
										&& processMoves.CanTakeAboveRight( board, board.GetIdentifierBelowLeft( board.GetIdentifierBelowRight( piece.SquareIdentifier ) ), "PLAYER" ) == true )
									{
										bCanBeTaken = true;
									}

									if( processMoves.IsEnemyBelowRight( board, board.GetIdentifierBelowRight( piece.SquareIdentifier ), piece.Player ) == true 
										&& processMoves.CanTakeAboveLeft( board, board.GetIdentifierBelowRight( board.GetIdentifierBelowRight( piece.SquareIdentifier ) ), "PLAYER" ) == true )
									{
										bCanBeTaken = true;
									}


									if( bCanBeTaken == false )
									{
										decisionSet.AddByName( "MoveBelowRight", nKingInfluenceMove );
										bDecisionIncremented = true;
									}
								}
								else
								{
									if( processMoves.IsEnemyAboveLeft( board, board.GetIdentifierBelowRight( piece.SquareIdentifier ), piece.Player ) == true 
										&& processMoves.CanTakeAboveRight( board, board.GetIdentifierAboveLeft( board.GetIdentifierBelowRight( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
									{
										bCanBeTaken = true;
									}

									if( processMoves.IsEnemyBelowLeft( board, board.GetIdentifierBelowRight( piece.SquareIdentifier ), piece.Player ) == true 
										&& processMoves.CanTakeAboveRight( board, board.GetIdentifierBelowLeft( board.GetIdentifierBelowRight( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
									{
										bCanBeTaken = true;
									}

									if( processMoves.IsEnemyBelowRight( board, board.GetIdentifierBelowRight( piece.SquareIdentifier ), piece.Player ) == true 
										&& processMoves.CanTakeAboveLeft( board, board.GetIdentifierBelowRight( board.GetIdentifierBelowRight( piece.SquareIdentifier ) ), "COMPUTER" ) == true )
									{
										bCanBeTaken = true;
									}


									if( bCanBeTaken == false )
									{
										decisionSet.AddByName( "MoveBelowRight", nKingInfluenceMove );
										bDecisionIncremented = true;
									}
								}
							}
						}
					}

					if( bDecisionIncremented == true )
					{
						decisionSet.Decision.AddToDecision( nNormalMove );
					}
				}

			}

			/// invalidate all patterns that are not for the current mover
			/// 

			for( int i=0; i<collection.Count; i++ )
			{
				decisionSet = ( FuzzyDecisionSet )collection[ i ];
			
				/// make sure that we are dealling with patterns for this move.
				/// 
				pattern = ( DraughtsPattern )availablePatterns.GetPattern( i );

				if( pattern.MoveNumber != nMoveNumber )
				{
					decisionSet.IsValid = false;
					decisionSet.Decision.IsValid = false;
					continue;
				}

				piece = pattern.GetStartsWithPiece( nMoveNumber );

				if( piece.LightPiece != light )
				{
					decisionSet.IsValid = false;
					decisionSet.Decision.IsValid = false;
				}
			}

			/// Decide which piece is going to be moved
			/// 

			bool bMoveMade = false;

			if( bMustTake == true )
			{
				int nMustTakeCount = 0;

				for( int i=0; i<collection.Count; i++ )
				{
					/// make sure that we are dealling with patterns for this move.
					/// 
					pattern = ( DraughtsPattern )availablePatterns.GetPattern( i );

					if( pattern.MoveNumber != nMoveNumber )
						continue;

					decisionSet = ( FuzzyDecisionSet )collection[ i ];

					if( decisionSet.GetNumberByName( "MustTake" ) > 0 && decisionSet.IsValid == true )
					{
						nMustTakeCount += decisionSet.GetNumberByName( "MustTake" );
					}
				}

				/// choose a must take move
				/// 
				if( nMustTakeCount > 1 )
				{
					for( int i=0; i<collection.Count; i++ )
					{
						if( ( ( FuzzyDecisionSet )collection[ i ] ).GetNumberByName( "MustTake" ) == 0 ||
							( ( FuzzyDecisionSet )collection[ i ] ).IsValid == false )
						{
							collection.RemoveAt( i );
							i=-1;
						}
					}

					/// Check that are actually moves available
					/// 
					if( collection.Count == 0 )
					{
						bCannotMove = true;
						return;
					}

					FuzzyDecisionSet decisionSetTemp = collection.Compare();
					decisionSetTemp.ZeroNumberByName( "MustTake" );
					FuzzyDecision decision = decisionSetTemp.Compare();
					moveSquare = ( DraughtsSquare )board.GetSquare( decisionSetTemp.Name );
			
					switch( decision.Name )
					{
						case "MoveAboveLeft":
						{
							strToSquare = board.GetIdentifierAboveLeft( board.GetIdentifierAboveLeft( moveSquare.Identifier ) );
							if( moveSquare.PlayerIsOnSquare == true )
							{
								if( MoveToMake( moveSquare.Identifier, strToSquare, "PLAYER", board.GetIdentifierAboveLeft( moveSquare.Identifier ) ) == true )
									bMoveMade = true;
							}
							else
							{
								if( MoveToMake( moveSquare.Identifier, strToSquare, "COMPUTER", board.GetIdentifierAboveLeft( moveSquare.Identifier ) ) == true )
									bMoveMade = true;
							}
						}break;
						case "MoveAboveRight":
						{
							strToSquare = board.GetIdentifierAboveRight( board.GetIdentifierAboveRight( moveSquare.Identifier ) );
							if( moveSquare.PlayerIsOnSquare == true )
							{
								if( MoveToMake( moveSquare.Identifier, strToSquare, "PLAYER", board.GetIdentifierAboveRight( moveSquare.Identifier ) ) == true )
									bMoveMade = true;
							}
							else
							{
								if( MoveToMake( moveSquare.Identifier, strToSquare, "COMPUTER", board.GetIdentifierAboveRight( moveSquare.Identifier ) ) == true )
									bMoveMade = true;
							}
						}break;
						case "MoveBelowLeft":
						{
							strToSquare = board.GetIdentifierBelowLeft( board.GetIdentifierBelowLeft( moveSquare.Identifier ) );
							if( moveSquare.PlayerIsOnSquare == true )
							{
								if( MoveToMake( moveSquare.Identifier, strToSquare, "PLAYER", board.GetIdentifierBelowLeft( moveSquare.Identifier ) ) == true )
									bMoveMade = true;
							}
							else
							{
								if( MoveToMake( moveSquare.Identifier, strToSquare, "COMPUTER", board.GetIdentifierBelowLeft( moveSquare.Identifier ) ) == true )
									bMoveMade = true;
							}
						} break;
						case "MoveBelowRight":
						{
							strToSquare = board.GetIdentifierBelowRight( board.GetIdentifierBelowRight( moveSquare.Identifier ) );
							if( moveSquare.PlayerIsOnSquare == true )
							{
								if( MoveToMake( moveSquare.Identifier, strToSquare, "PLAYER", board.GetIdentifierBelowRight( moveSquare.Identifier ) ) == true )
									bMoveMade = true;
							}
							else
							{
								if( MoveToMake( moveSquare.Identifier, strToSquare, "COMPUTER", board.GetIdentifierBelowRight( moveSquare.Identifier ) ) == true )
									bMoveMade = true;
							}
						} break;
					}
				}
				else
				{
					for( int i=0; i<availablePatterns.Count; i++ )
					{
						/// make sure that we are dealling with patterns for this move.
						/// 
						pattern = ( DraughtsPattern )availablePatterns.GetPattern( i );

						if( pattern.MoveNumber != nMoveNumber )
							continue;

						decisionSet = ( FuzzyDecisionSet )collection[ i ];

						if( decisionSet.GetNumberByName( "MustTake" ) > 0 
							&& decisionSet.IsValid == true )
						{
							/// zero out the must move value so it doesn't interfere
							/// 

							decisionSet.ZeroNumberByName( "MustTake" );

							FuzzyDecision decision = decisionSet.Compare();

							piece = pattern.GetStartsWithPiece( nMoveNumber );
							
							switch( decision.Name )
							{
								case "MoveAboveLeft": 
								{
									strToSquare = board.GetIdentifierAboveLeft( board.GetIdentifierAboveLeft( piece.SquareIdentifier ) );
									if( MoveToMake( piece.SquareIdentifier, strToSquare, piece.Player, board.GetIdentifierAboveLeft( piece.SquareIdentifier ) ) == true )
										bMoveMade = true;
								}break;
								case "MoveAboveRight":
								{
									strToSquare = board.GetIdentifierAboveRight( board.GetIdentifierAboveRight( piece.SquareIdentifier ) );
									if( MoveToMake( piece.SquareIdentifier, strToSquare, piece.Player, board.GetIdentifierAboveRight( piece.SquareIdentifier ) ) == true )
										bMoveMade = true;
								}break;
								case "MoveBelowLeft":
								{
									strToSquare = board.GetIdentifierBelowLeft( board.GetIdentifierBelowLeft( piece.SquareIdentifier ) );
									if( MoveToMake( piece.SquareIdentifier, strToSquare, piece.Player, board.GetIdentifierBelowLeft( piece.SquareIdentifier ) ) == true )
										bMoveMade = true;
								}break;
								case "MoveBelowRight":
								{
									strToSquare = board.GetIdentifierBelowRight( board.GetIdentifierBelowRight( piece.SquareIdentifier ) );
									if( MoveToMake( piece.SquareIdentifier, strToSquare, piece.Player, board.GetIdentifierBelowRight( piece.SquareIdentifier ) ) == true )
										bMoveMade = true;
								}break;
							}

							break;
						}
					}
				}

				bPieceTaken = true;
			}
			else
			{
				/// make sure that the must take option is invalid
				/// 

				for( int i=0; i<collection.Count; i++ )
				{
					FuzzyDecisionSet tempDecisionSet = ( FuzzyDecisionSet )collection[ i ];
					tempDecisionSet.SetIsValidByName( "MustTake", false );
				}

				/// right now remove all invalid decisions
				/// 

				for( int i=0; i<collection.Count; i++ )
				{
					if( ( ( FuzzyDecisionSet )collection[ i ] ).Decision.IsValid == false )
					{
						collection.RemoveAt( i );
						i=-1;
					}
				}

#if DEBUG
				bool bSeriousError = false;
#endif

				/// Check that are actually moves available
				/// 
				if( collection.Count == 0 )
				{
#if DEBUG
					bSeriousError = true;
#else
					bCannotMove = true;
					return;
#endif
				}
			
				FuzzyDecisionSet decisionSetTemp = collection.Compare();
				if( decisionSetTemp == null )
				{
#if DEBUG
					bSeriousError = false;
#else
					bCannotMove = true;
					return;
#endif
				}

				FuzzyDecision decision = decisionSetTemp.Compare();
				if( decision == null )
				{
#if DEBUG
					bSeriousError = true;
#else
					bCannotMove = true;
					return;
#endif
				}

				moveSquare = ( DraughtsSquare )board.GetSquare( decisionSetTemp.Name );

#if DEBUG
				if( bSeriousError == true )
				{
					MovePiece( light );
				}
#endif
				
				switch( decision.Name )
				{
					case "MoveAboveLeft":
					{
						strToSquare = board.GetIdentifierAboveLeft( moveSquare.Identifier );
						if( moveSquare.PlayerIsOnSquare == true )
						{
							if( MakeMove( moveSquare.Identifier, strToSquare, "PLAYER", false, null ) == true )
								bMoveMade = true;
						}
						else
						{
							if( MakeMove( moveSquare.Identifier, strToSquare, "COMPUTER", false, null ) == true )
								bMoveMade = true;
						}
					}break;
					case "MoveAboveRight":
					{
						strToSquare = board.GetIdentifierAboveRight( moveSquare.Identifier );
						if( moveSquare.PlayerIsOnSquare == true )
						{
							if( MakeMove( moveSquare.Identifier, strToSquare, "PLAYER", false, null ) == true )
								bMoveMade = true;
						}
						else
						{
							if( MakeMove( moveSquare.Identifier, strToSquare, "COMPUTER", false, null ) == true )
								bMoveMade = true;
						}
					}break;
					case "MoveBelowLeft":
					{
						strToSquare = board.GetIdentifierBelowLeft( moveSquare.Identifier );
						if( moveSquare.PlayerIsOnSquare == true )
						{
							if( MakeMove( moveSquare.Identifier, strToSquare, "PLAYER", false, null ) == true )
								bMoveMade = true;
						}
						else
						{
							if( MakeMove( moveSquare.Identifier, strToSquare, "COMPUTER", false, null ) == true )
								bMoveMade = true;
						}
					} break;
					case "MoveBelowRight":
					{
						strToSquare = board.GetIdentifierBelowRight( moveSquare.Identifier );
						if( moveSquare.PlayerIsOnSquare == true )
						{
							if( MakeMove( moveSquare.Identifier, strToSquare, "PLAYER", false, null ) == true )
								bMoveMade = true;
						}
						else
						{
							if( MakeMove( moveSquare.Identifier, strToSquare, "COMPUTER", false, null ) == true )
								bMoveMade = true;
						}
					} break;

				}
			}

			/// debug function if for any reason a move cannot be made 
			/// break here and debug the fucker
			/// 
			if( bMoveMade == false )
			{
				MovePiece( light );
			}

			/// Update the game array
			/// 
			if( moveSquare == null )
				UpdateGamePatterns( piece.SquareIdentifier, strToSquare );
			else
				UpdateGamePatterns( moveSquare.Identifier, strToSquare );
		}

		/// <summary>
		/// move to make takes the move that is to be made and checks for double jumps
		/// </summary>
		/// <param name="fromSquare"></param>
		/// <param name="toSquare"></param>
		/// <param name="occupier"></param>
		/// <returns></returns>
		public bool MoveToMake( string fromSquare, string toSquare, string occupier, string takenSquare )
		{
			bool bDone = false;
			bool bMove = false;

			if( MakeMove( fromSquare, toSquare, occupier, true, takenSquare ) == false )
				return false;

			DraughtsSquare square = null;

			while( bDone == false )
			{
				fromSquare = toSquare;
				square = ( DraughtsSquare )board.GetSquare( fromSquare );
				bMove = false;

				if( square != null )
				{

					/// if player and moving up the board
					/// 
					if( board.PlayerIsLight == true && occupier == "PLAYER" 
						|| board.PlayerIsLight == false && occupier == "COMPUTER" )
					{
						if( processMoves.IsEnemyAboveLeft( board, fromSquare, occupier ) == true )
						{
							if( processMoves.IsAboveLeftClear( board, board.GetIdentifierAboveLeft( fromSquare ) ) == true )
							{
								toSquare = board.GetIdentifierAboveLeft( board.GetIdentifierAboveLeft( fromSquare ) );
								if( MakeMove( fromSquare, toSquare, occupier, true, board.GetIdentifierAboveLeft( fromSquare ) ) == true )
									bMove = true;
								else
									return false;
							}
						}

						if( processMoves.IsEnemyAboveRight( board, fromSquare, occupier ) == true 
							&& bMove == false )
						{
							if( processMoves.IsAboveRightClear( board, board.GetIdentifierAboveRight( fromSquare ) ) == true )
							{
								toSquare = board.GetIdentifierAboveRight( board.GetIdentifierAboveRight( fromSquare ) );
								if( MakeMove( fromSquare, toSquare, occupier, true, board.GetIdentifierAboveRight( fromSquare ) ) == true )
									bMove = true;
								else
									return false;
							}
						}


						if( square.IsKing == true )
						{
							if( processMoves.IsEnemyBelowLeft( board, fromSquare, occupier ) == true 
								&& bMove == false )
							{
								if( processMoves.IsBelowLeftClear( board, board.GetIdentifierBelowLeft( fromSquare ) ) == true )
								{
									toSquare = board.GetIdentifierBelowLeft( board.GetIdentifierBelowLeft( fromSquare ) );
									if( MakeMove( fromSquare, toSquare, occupier, true, board.GetIdentifierBelowLeft( fromSquare ) ) == true )
										bMove = true;
									else
										return false;
								}
							}

							if( processMoves.IsEnemyBelowRight( board, fromSquare, occupier ) == true 
								&& bMove == false )
							{
								if( processMoves.IsBelowRightClear( board, board.GetIdentifierBelowRight( fromSquare ) ) == true )
								{
									toSquare = board.GetIdentifierBelowRight( board.GetIdentifierBelowRight( fromSquare ) );
									if( MakeMove( fromSquare, toSquare, occupier, true, board.GetIdentifierBelowRight( fromSquare ) ) == true )
										bMove = true;
									else
										return false;
								}
							}
						}

						if( bMove == false )
							bDone = true;
					}
					else 
					{
						if( processMoves.IsEnemyBelowLeft( board, fromSquare, occupier ) == true )
						{
							if( processMoves.IsBelowLeftClear( board, board.GetIdentifierBelowLeft( fromSquare ) ) == true )
							{
								toSquare = board.GetIdentifierBelowLeft( board.GetIdentifierBelowLeft( fromSquare ) );
								if( MakeMove( fromSquare, toSquare, occupier, true, board.GetIdentifierBelowLeft( fromSquare ) ) == true )
									bMove = true;
								else
									return false;
							}
						}

						if( processMoves.IsEnemyBelowRight( board, fromSquare, occupier ) == true 
							&& bMove == false )
						{
							if( processMoves.IsBelowRightClear( board, board.GetIdentifierBelowRight( fromSquare ) ) == true )
							{
								toSquare = board.GetIdentifierBelowRight( board.GetIdentifierBelowRight( fromSquare ) );
								if( MakeMove( fromSquare, toSquare, occupier, true, board.GetIdentifierBelowRight( fromSquare ) ) == true )
									bMove = true;
								else
									return false;
							}
						}

						if( square.IsKing == true )
						{
							if( processMoves.IsEnemyAboveLeft( board, fromSquare, occupier ) == true 
								&& bMove == false )
							{
								if( processMoves.IsAboveLeftClear( board, board.GetIdentifierAboveLeft( fromSquare ) ) == true )
								{
									toSquare = board.GetIdentifierAboveLeft( board.GetIdentifierAboveLeft( fromSquare ) );
									if( MakeMove( fromSquare, toSquare, occupier, true, board.GetIdentifierAboveLeft( fromSquare ) ) == true )
										bMove = true;
									else
										return false;
								}
							}

							if( processMoves.IsEnemyAboveRight( board, fromSquare, occupier ) == true 
								&& bMove == false )
							{
								if( processMoves.IsAboveRightClear( board, board.GetIdentifierAboveRight( fromSquare ) ) == true )
								{
									toSquare = board.GetIdentifierAboveRight( board.GetIdentifierAboveRight( fromSquare ) );
									if( MakeMove( fromSquare, toSquare, occupier, true, board.GetIdentifierAboveRight( fromSquare ) ) == true )
										bMove = true;
									else
										return false;
								}
							}
						}

						if( bMove == false )
							bDone = true;
					}
				}
			}

			return true;
		}

		public bool MakeMove( string fromSquare, string toSquare, string occupier, bool take, string takenSquare )
		{
			/// Note the returning false after the first square has changed
			/// could be "unwise" leave for now but keep an eye out for it
			/// 

			DraughtsSquare square = ( DraughtsSquare )board.GetSquare( fromSquare );
			DraughtsSquare squareTo = ( DraughtsSquare )board.GetSquare( toSquare );
			DraughtsSquare squareTaken = ( DraughtsSquare )board.GetSquare( takenSquare );

			if( square == null || squareTo == null )
				return false;

			if( take == true && squareTaken == null )
				return false;

			if( take == true && squareTo.IsOccupied == true )
				return false;

			square.IsOccupied = false;
			square.OccupyingName = "EMPTY";
			square.IsValid = false;
			square.PlayerIsOnSquare = false;

			bool bIsKing = square.IsKing;
			square.IsKing = false;

			if( take == true && takenSquare != null )
			{
				squareTaken.IsOccupied = false;
				squareTaken.OccupyingName = "EMPTY";
				squareTaken.IsKing = false;
				squareTaken.IsValid = false;
				squareTaken.PlayerIsOnSquare = false;
			}


			squareTo.IsOccupied = true;
			squareTo.OccupyingName = occupier;
			squareTo.IsValid = false;
			squareTo.IsKing = bIsKing;

			if( squareTo.Identifier[ 1 ] == 'A' )
				squareTo.IsKing = true;

			if( squareTo.Identifier[ 1 ] == 'H' )
				squareTo.IsKing = true;

			if( occupier == "PLAYER" )
				squareTo.PlayerIsOnSquare = true;
			else
				squareTo.PlayerIsOnSquare = false;

			squareTo.IsValid = false;

			board.Invalidate();

			return true;
		}
	}
}
