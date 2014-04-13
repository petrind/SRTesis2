using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using BoardControl;

namespace Draughts
{
	public class DraughtsBoard : BoardControl.Board
	{
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// default colour values
		/// </summary>
		private Color lightColor = Color.Beige;
		private Color darkColor = Color.Black;
		private Color draughtsLegendColor = Color.DeepSkyBlue;
		private Color highlightColor = Color.Green;

		/// <summary>
		/// Squares that we are going to play the game on
		/// </summary>
		private bool bPlayOnLightSquares;

		/// <summary>
		/// Testing code variable
		/// </summary>
		private bool bTest;

		/// <summary>
		/// piece bitmaps
		/// </summary>
		private Bitmap bitLightPiece;
		private Bitmap bitDarkPiece;
		private Bitmap bitLightKingPiece;
		private Bitmap bitDarkKingPiece;
		private Bitmap bitLightNoDrop;
		private Bitmap bitDarkNoDrop;
		private Bitmap bitLightDrop;
		private Bitmap bitDarkDrop;
		private Bitmap bitLightKingDrop;
		private Bitmap bitLightKingNoDrop;
		private Bitmap bitDarkKingDrop;
		private Bitmap bitDarkKingNoDrop;

		/// <summary>
		/// colour that the player is using
		/// </summary>
		private bool bPlayerIsLight;


		/// <summary>
		/// is the player allowed to make a move at this 
		/// point in time?
		/// </summary>
		private bool bPlayersMove;


		/// <summary>
		/// Are we running the automatic training
		/// </summary>
		private bool bAutomaticTraining;

		/// <summary>
		/// DragdropHelper values
		/// </summary>
		private bool bHasDropped;
		private bool bDroppedOnOccupiedSquare;
		private bool bInvalidSquareDrop;
		private bool bStartDragDrop;
		private DraughtsSquare bsStartSquare; 
		private bool bAllowDrop;
		private bool bPlayerTakesPiece;
		private string strTakenPieceIdentifier;
		private bool bCanStillTake;
		private string strTakingPieceIdentifier;
		private bool bAllowComputerMove;


		/// <summary>
		/// should the highlights be drawn for a player move
		/// </summary>
		private bool bDrawHighlight;

		/// <summary>
		/// game object for controlling the game code
		/// </summary>
		private DraughtsGame game;


		/// <summary>
		/// If the number of moves none taken goes above the 
		/// the number of moves without take then the game is declared
		/// stalemated.
		/// </summary>
		private int nNumberOfMovesWithoutTake;
		private int nNumberOfMovesNoneTaken;

		public int NumberOfMovesWithoutTake
		{
			get
			{
				return nNumberOfMovesWithoutTake;
			}
			set
			{
				nNumberOfMovesWithoutTake = value;
			}
		}

		public int NumberOfMovesNoneTaken
		{
			get
			{
				return nNumberOfMovesNoneTaken;
			}
			set
			{
				nNumberOfMovesNoneTaken = value;
			}
		}

		private bool Testing
		{
			get
			{
				return bTest;
			}
			set
			{
				bTest = value;
			}
		}


		public Color LightColor
		{
			get
			{
				return lightColor;
			}
			set
			{
				lightColor = value;
			}
		}

		public Color DarkColor
		{
			get
			{
				return darkColor;
			}
			set
			{
				darkColor = value;
			}
		}

		/// <summary>
		/// set the draughts legend colour only used for the loading of the colours xml file.
		/// everything else uses the boardcontrol legend color. This is done just to save the 
		/// color value when the board is initialised after the form has loaded.
		/// </summary>
		public Color DraughtsLegendColor
		{
			get
			{
				return draughtsLegendColor;
			}
			set
			{
				draughtsLegendColor = value;
			}
		}

		public Color HighlightColor
		{
			get
			{
				return highlightColor;
			}
			set
			{
				highlightColor = value;
			}
		}

		public bool PlayOnLightSquares
		{
			get
			{
				return bPlayOnLightSquares;
			}
			set
			{
				bPlayOnLightSquares = value;
			}
		}

		public bool PlayerIsLight
		{
			get
			{
				return bPlayerIsLight;
			}
			set
			{
				bPlayerIsLight = value;
			}
		}

		public bool DrawHighlight
		{
			get
			{
				return bDrawHighlight;
			}
			set
			{
				bDrawHighlight = value;
			}
		}

		private bool PlayersMove
		{
			get
			{
				return bPlayersMove;
			}
			set
			{
				bPlayersMove = value;
			}
		}

		private bool AutomaticTraining
		{
			get
			{
				return bAutomaticTraining;
			}
			set
			{
				bAutomaticTraining = value;
			}
		}

		public DraughtsBoard()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			Testing = false;


			PlayOnLightSquares = true;
			PlayerIsLight = true;
			DrawHighlight = false;
			PlayersMove = false;
			AutomaticTraining = false;

			game = new DraughtsGame();
			game.Board = this;

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


		/// <summary>
		/// Initialize the board
		/// </summary>
		public void InitializeBoard()
		{
			HorizontalSquares = 8;
			VerticalSquares = 8;
			SquareWidth = 75;
			SquareHeight = 75;
			BoardWidth = HorizontalSquares * SquareWidth;
			BoardHeight = VerticalSquares * SquareHeight;

			/// create the board
			Clear();

			GetHashTable.Add( "AA", new DraughtsSquare( SquareWidth, SquareHeight, 0, 0, "AA" ) );
			GetHashTable.Add( "AB", new DraughtsSquare( SquareWidth, SquareHeight, 0, SquareHeight, "AB" ) );
			GetHashTable.Add( "AC", new DraughtsSquare( SquareWidth, SquareHeight, 0, SquareHeight * 2, "AC" ) );
			GetHashTable.Add( "AD", new DraughtsSquare( SquareWidth, SquareHeight, 0, SquareHeight * 3, "AD" ) );
			GetHashTable.Add( "AE", new DraughtsSquare( SquareWidth, SquareHeight, 0, SquareHeight * 4, "AE" ) );
			GetHashTable.Add( "AF", new DraughtsSquare( SquareWidth, SquareHeight, 0, SquareHeight * 5, "AF" ) );
			GetHashTable.Add( "AG", new DraughtsSquare( SquareWidth, SquareHeight, 0, SquareHeight * 6, "AG" ) );
			GetHashTable.Add( "AH", new DraughtsSquare( SquareWidth, SquareHeight, 0, SquareHeight * 7, "AH" ) );
			GetHashTable.Add( "BA", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth, 0, "BA" ) );
			GetHashTable.Add( "BB", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth, SquareHeight, "BB" ) );
			GetHashTable.Add( "BC", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth, SquareHeight * 2, "BC" ) );
			GetHashTable.Add( "BD", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth, SquareHeight * 3, "BD" ) );
			GetHashTable.Add( "BE", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth, SquareHeight * 4, "BE" ) );
			GetHashTable.Add( "BF", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth, SquareHeight * 5, "BF" ) );
			GetHashTable.Add( "BG", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth, SquareHeight * 6, "BG" ) );
			GetHashTable.Add( "BH", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth, SquareHeight * 7, "BH" ) );
			GetHashTable.Add( "CA", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 2, 0, "CA" ) );
			GetHashTable.Add( "CB", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 2, SquareHeight, "CB" ) );
			GetHashTable.Add( "CC", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 2, SquareHeight * 2, "CC" ) );
			GetHashTable.Add( "CD", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 2, SquareHeight * 3, "CD" ) );
			GetHashTable.Add( "CE", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 2, SquareHeight * 4, "CE" ) );
			GetHashTable.Add( "CF", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 2, SquareHeight * 5, "CF" ) );
			GetHashTable.Add( "CG", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 2, SquareHeight * 6, "CG" ) );
			GetHashTable.Add( "CH", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 2, SquareHeight * 7, "CH" ) );
			GetHashTable.Add( "DA", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 3, 0, "DA" ) );
			GetHashTable.Add( "DB", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 3, SquareHeight, "DB" ) );
			GetHashTable.Add( "DC", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 3, SquareHeight * 2, "DC" ) );
			GetHashTable.Add( "DD", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 3, SquareHeight * 3, "DD" ) );
			GetHashTable.Add( "DE", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 3, SquareHeight * 4, "DE" ) );
			GetHashTable.Add( "DF", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 3, SquareHeight * 5, "DF" ) );
			GetHashTable.Add( "DG", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 3, SquareHeight * 6, "DG" ) );
			GetHashTable.Add( "DH", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 3, SquareHeight * 7, "DH" ) );
			GetHashTable.Add( "EA", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 4, 0, "EA" ) );
			GetHashTable.Add( "EB", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 4, SquareHeight, "EB" ) );
			GetHashTable.Add( "EC", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 4, SquareHeight * 2, "EC" ) );
			GetHashTable.Add( "ED", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 4, SquareHeight * 3, "ED" ) );
			GetHashTable.Add( "EE", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 4, SquareHeight * 4, "EE" ) );
			GetHashTable.Add( "EF", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 4, SquareHeight * 5, "EF" ) );
			GetHashTable.Add( "EG", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 4, SquareHeight * 6, "EG" ) );
			GetHashTable.Add( "EH", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 4, SquareHeight * 7, "EH" ) );
			GetHashTable.Add( "FA", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 5, 0, "FA" ) );
			GetHashTable.Add( "FB", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 5, SquareHeight, "FB" ) );
			GetHashTable.Add( "FC", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 5, SquareHeight * 2, "FC" ) );
			GetHashTable.Add( "FD", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 5, SquareHeight * 3, "FD" ) );
			GetHashTable.Add( "FE", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 5, SquareHeight * 4, "FE" ) );
			GetHashTable.Add( "FF", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 5, SquareHeight * 5, "FF" ) );
			GetHashTable.Add( "FG", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 5, SquareHeight * 6, "FG" ) );
			GetHashTable.Add( "FH", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 5, SquareHeight * 7, "FH" ) );
			GetHashTable.Add( "GA", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 6, 0, "GA" ) );
			GetHashTable.Add( "GB", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 6, SquareHeight, "GB" ) );
			GetHashTable.Add( "GC", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 6, SquareHeight * 2, "GC" ) );
			GetHashTable.Add( "GD", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 6, SquareHeight * 3, "GD" ) );
			GetHashTable.Add( "GE", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 6, SquareHeight * 4, "GE" ) );
			GetHashTable.Add( "GF", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 6, SquareHeight * 5, "GF" ) );
			GetHashTable.Add( "GG", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 6, SquareHeight * 6, "GG" ) );
			GetHashTable.Add( "GH", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 6, SquareHeight * 7, "GH" ) );
			GetHashTable.Add( "HA", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 7, 0, "HA" ) );
			GetHashTable.Add( "HB", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 7, SquareHeight, "HB" ) );
			GetHashTable.Add( "HC", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 7, SquareHeight * 2, "HC" ) );
			GetHashTable.Add( "HD", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 7, SquareHeight * 3, "HD" ) );
			GetHashTable.Add( "HE", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 7, SquareHeight * 4, "HE" ) );
			GetHashTable.Add( "HF", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 7, SquareHeight * 5, "HF" ) );
			GetHashTable.Add( "HG", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 7, SquareHeight * 6, "HG" ) );
			GetHashTable.Add( "HH", new DraughtsSquare( SquareWidth, SquareHeight, SquareWidth * 7, SquareHeight * 7, "HH" ) );

			DrawLegend( true );

			LegendColor = DraughtsLegendColor;

			SetBackGroundColor( lightColor, darkColor );
			SetHighlightColor( highlightColor );

			SetDisplayMode( "APPLICATION" );

			if( Designing() == true ) 
				return;

			ImplementDragDrop = true;

			Testing = true;

			bitLightPiece = new Bitmap( "Light.bmp" );
			bitLightKingPiece = new Bitmap( "LightKing.bmp" );
			bitDarkPiece = new Bitmap( "Dark.bmp" );
			bitDarkKingPiece = new Bitmap( "DarkKing.bmp" );
			bitLightNoDrop = new Bitmap( "LightNoDrop.bmp" );
			bitDarkNoDrop = new Bitmap( "DarkNoDrop.bmp" );
			bitLightDrop = new Bitmap( "LightDrop.bmp" );
			bitDarkDrop = new Bitmap( "DarkDrop.bmp" );
			bitLightKingDrop = new Bitmap( "LightKingDrop.bmp" );
			bitLightKingNoDrop = new Bitmap( "LightKingNoDrop.bmp" );
			bitDarkKingDrop = new Bitmap( "DarkKingDrop.bmp" );
			bitDarkKingNoDrop = new Bitmap( "DarkKingNoDrop.bmp" );

			bitLightPiece.MakeTransparent( Color.Black );
			bitLightKingPiece.MakeTransparent( Color.Black );
			bitDarkPiece.MakeTransparent( Color.Black );
			bitDarkKingPiece.MakeTransparent( Color.Black );
			bitLightNoDrop.MakeTransparent( Color.Black );
			bitDarkNoDrop.MakeTransparent( Color.Black );
			bitLightDrop.MakeTransparent( Color.Black );
			bitDarkDrop.MakeTransparent( Color.Black );
			bitLightKingDrop.MakeTransparent( Color.Black );
			bitLightKingNoDrop.MakeTransparent( Color.Black );
			bitDarkKingDrop.MakeTransparent( Color.Black );
			bitDarkKingNoDrop.MakeTransparent( Color.Black );

			DragDropImage = bitLightPiece;

			SetupSquares();

			bHasDropped = true;
			bDroppedOnOccupiedSquare = false;
			bInvalidSquareDrop = false;
			bStartDragDrop = false;
			bsStartSquare = null;
			bAllowDrop = false;
			bPlayerTakesPiece = false;
			strTakenPieceIdentifier = null;
			bCanStillTake = false;
			strTakingPieceIdentifier = null;
			nNumberOfMovesNoneTaken = 0;
			nNumberOfMovesWithoutTake = 20;
			bAllowComputerMove = false;
			game.InitializeGame();
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// DraughtsBoard
			// 
			this.Name = "DraughtsBoard";
			this.Size = new System.Drawing.Size(376, 344);
			this.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.OnGiveFeedback);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
			this.DragOver += new System.Windows.Forms.DragEventHandler(this.OnDragOver);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);

		}
		#endregion

		private void OnPaint(object sender, System.Windows.Forms.PaintEventArgs e)
		{

			Graphics grfx = e.Graphics;

			foreach( DictionaryEntry dicEnt in GetHashTable )
			{
				BasicSquare square = ( BasicSquare )dicEnt.Value;

				square.DrawSquare( grfx );
			}

		}

		/// <summary>
		/// let the sqaures know what they are supposed to draw
		/// </summary>
		private void SetupSquares()
		{
			foreach( DictionaryEntry dicEnt in GetHashTable )
			{
				DraughtsSquare square = ( DraughtsSquare )dicEnt.Value;

				if( PlayerIsLight == true )
				{
					square.PlayerPiece = bitLightPiece;
					square.PlayerKingPiece = bitLightKingPiece;
					square.ComputerPiece = bitDarkPiece;
					square.ComputerKingPiece = bitDarkKingPiece;
				}
				else
				{
					square.PlayerPiece = bitDarkPiece;
					square.PlayerKingPiece = bitDarkKingPiece;
					square.ComputerPiece = bitLightPiece;
					square.ComputerKingPiece = bitLightKingPiece;
				}
			}

			Invalidate();
			Update();
		}

		private void GetTestPieceBitmaps()
		{
			Bitmap bitPiece = new Bitmap( "testpiece.bmp" );
			Bitmap bitDrop = new Bitmap( "testdrop.bmp" );
			Bitmap bitNoDrop = new Bitmap( "testnodrop.bmp" );

			DragDropImage = bitPiece;
			DragOverImage = bitDrop;
			DragNoDropImage = bitNoDrop;
		}

		private void TestDragDrop()
		{
			DraughtsSquare square = ( DraughtsSquare )GetSquare( "AA" );
			Bitmap bitTestPiece = new Bitmap( "testpiece.bmp" );

			if( square != null )
			{
				square.TestPiece = bitTestPiece;
				square.IsValid = false;
				square.IsOccupied = true;
				square.OccupyingName = "TEST";
				square.Piece = new BasicPiece( square );
			}

			DraughtsSquare squareTwo = ( DraughtsSquare )GetSquare( "BB" );

			if( squareTwo != null )
			{
				squareTwo.TestPiece = bitTestPiece;
				squareTwo.IsValid = false;
				squareTwo.IsOccupied = true;
				squareTwo.OccupyingName = "TEST";
				squareTwo.Piece = new BasicPiece( squareTwo );
			}
		}

		private void OnDragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			if( ImplementDragDrop == true )
			{
				DraughtsSquare square = ( DraughtsSquare )GetSquareAt( e.X - this.Parent.Bounds.X - this.Bounds.X - LegendWidth, e.Y - this.Parent.Bounds.Y - this.Bounds.Y - 40 );

				if( PlayOnLightSquares == true && square.BackGroundColor != LightColor ||
					PlayOnLightSquares == false && square.BackGroundColor != DarkColor )
				{
					bInvalidSquareDrop = true;
					return;
				}
				else
					bInvalidSquareDrop = false;

				if( square != null && square.IsOccupied == false )
				{
					if( bAllowDrop == false )
					{
						square.IsValid = false;
						square.DrawDragDropImage = false;

						DragDropEffects effects = DoDragDrop( DragDropImage, DragDropEffects.None );

						Cursor = Cursors.Default;

						foreach( DictionaryEntry dicEnt in GetHashTable )
						{
							BasicSquare squareTemp = ( BasicSquare )dicEnt.Value;

							squareTemp.DrawDragDropImage = false;
						}


						if( square.OccupyingName == "PLAYER" )
							bHasDropped = true;

						bDroppedOnOccupiedSquare = true;
						bInvalidSquareDrop = true;

						Invalidate();
						Update();

						return;
					}

					/// put the piece on the square
					/// 
					square.IsOccupied = true;
					square.IsValid = false;
					square.DrawDragDropImage = false;
					square.OccupyingName = "PLAYER";
					square.PlayerIsOnSquare = true;
					square.IsKing = bsStartSquare.IsKing;

					NumberOfMovesNoneTaken++;

					bsStartSquare.PlayerIsOnSquare = false;
					bsStartSquare.IsOccupied = false;
					bsStartSquare.OccupyingName = "EMPTY";
					bsStartSquare.DrawDragDropImage = false;


					if( square.Identifier[ 1 ] == 'A' )
						square.IsKing = true;

					if( square.Identifier[ 1 ] == 'H' )
						square.IsKing = true;

					DragDropEffects effectsTemp = DoDragDrop( DragDropImage, DragDropEffects.None );

					Cursor = Cursors.Default;

					foreach( DictionaryEntry dicEnt in GetHashTable )
					{
						BasicSquare squareTemp = ( BasicSquare )dicEnt.Value;

						squareTemp.DrawDragDropImage = false;
					} 

					bHasDropped = true;
					bDroppedOnOccupiedSquare = false;
					bInvalidSquareDrop = false;
					bAllowComputerMove = true;

					Invalidate();
					Update();

					return;
				}
				else if( square != null && square.IsOccupied == true )
				{
					square.IsValid = false;
					square.DrawDragDropImage = false;

					DragDropEffects effects = DoDragDrop( DragDropImage, DragDropEffects.None );

					Cursor = Cursors.Default;

					foreach( DictionaryEntry dicEnt in GetHashTable )
					{
						BasicSquare squareTemp = ( BasicSquare )dicEnt.Value;

						squareTemp.DrawDragDropImage = false;
					}


					if( square.OccupyingName == "PLAYER" )
						bHasDropped = true;

					bDroppedOnOccupiedSquare = true;
					bInvalidSquareDrop = true;
					bPlayerTakesPiece = false;

					Invalidate();
					Update();

					return;
				}
				else
					bInvalidSquareDrop = true;
			}		
		}

		private void OnGiveFeedback(object sender, System.Windows.Forms.GiveFeedbackEventArgs e)
		{
			e.UseDefaultCursors = false;
		}

		/// <summary>
		/// handle the player moving a piece with drag n drop
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnDragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
			if( ImplementDragDrop == true )
			{
				DraughtsSquare square = ( DraughtsSquare )GetSquareAt( e.X - this.Parent.Bounds.X - this.Bounds.X - LegendWidth, e.Y - this.Parent.Bounds.Y - this.Bounds.Y - 40 );


				if( bStartDragDrop == false )
				{
					if( square.IsOccupied == false )
						return;

					bStartDragDrop = true;
					bsStartSquare = square;

					if( square.PlayerIsOnSquare == false )
						return;
					
					if( DrawHighlight == true )
					{
						HighlightSquares( square );
					}
				}

				if( bsStartSquare == null )
					return;

				if( square != null && square.IsOccupied == false )
				{
					if( e.Data.GetDataPresent( DataFormats.Bitmap ) == true )
					{
						Cursor = Cursors.Hand;

						e.Effect = DragDropEffects.Copy;

						foreach( DictionaryEntry dicEnt in GetHashTable )
						{
							BasicSquare squareTemp = ( BasicSquare )dicEnt.Value;

							squareTemp.DrawDragDropImage = false;
						}

						square.DrawDragDropImage = true;

						
						if( PlayOnLightSquares == true )
						{
							if( PlayerIsLight == true )
							{
								PlayerLightMove( square );
							}
							else
							{
								PlayerDarkMove( square );
							}
						}
						else
						{
							if( PlayerIsLight == true )
							{
								PlayerLightMove( square );
							}
							else
							{
								PlayerDarkMove( square );
							}
						}


						Invalidate();
						Update();
					}
				}
				else if( square != null && square.IsOccupied == true )
				{
					if( e.Data.GetDataPresent( DataFormats.Bitmap ) == true )
					{
						Cursor = Cursors.Hand;

						e.Effect = DragDropEffects.Copy;

						foreach( DictionaryEntry dicEnt in GetHashTable )
						{
							BasicSquare squareTemp = ( BasicSquare )dicEnt.Value;

							squareTemp.DrawDragDropImage = false;
						}

						square.DrawDragDropImage = true;

						/// if the square contains a computer piece then 
						/// can't drop on it under any circumstances
						/// 

						if( square.OccupyingName == "COMPUTER" )
						{
							if( PlayerIsLight == true )
							{
								if( bsStartSquare.IsKing == true )
									square.DragDropImage = bitLightKingNoDrop;
								else
									square.DragDropImage = bitLightNoDrop;
							}
							else
							{
								if( bsStartSquare.IsKing == true )
									square.DragDropImage = bitDarkKingNoDrop;
								else
									square.DragDropImage = bitDarkNoDrop;
							}

							bAllowDrop = false;

							Invalidate();
							Update();

							return;
						}
						
						if( PlayerIsLight == true )
						{
							if( square.BackGroundColor == LightColor 
								&& square == bsStartSquare )
							{
								if( bsStartSquare.IsKing == true )
									square.DragDropImage = bitLightKingDrop;
								else
									square.DragDropImage = bitLightDrop;

								bAllowDrop = true;
							}
							else
							{
								if( bsStartSquare.IsKing == true )
									square.DragDropImage = bitLightKingNoDrop;
								else
									square.DragDropImage = bitLightNoDrop;

								bAllowDrop = false;
							}	
						}
						else 
						{
							if( square.BackGroundColor == DarkColor 
								&& square == bsStartSquare )
							{
								if( bsStartSquare.IsKing == true )
									square.DragDropImage = bitDarkKingDrop;
								else
									square.DragDropImage = bitDarkDrop;

								bAllowDrop = true;
							}
							else
							{
								if( bsStartSquare.IsKing == true )
									square.DragDropImage = bitDarkKingNoDrop;
								else
									square.DragDropImage = bitDarkNoDrop;
								
								bAllowDrop = false;
							}
						}

						square.IsValid = false;


						Invalidate();
						Update();
					}
				}
				else
				{
					if( square != null )
					{
						square.DrawDragDropImage = false;
					}

					bInvalidSquareDrop = true;
				}
			}
			else
			{
				Cursor = Cursors.Default;	
				bInvalidSquareDrop = true;
			}
		}

		private void OnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if( ImplementDragDrop == true )
			{
				DraughtsSquare square = ( DraughtsSquare )GetSquareAt( e.X, e.Y );

				if( square != null && square.IsOccupied == true )
				{
					if( square.OccupyingName == "COMPUTER" )
						return;

					if( square.PlayerIsOnSquare == false )
						return;

					if( DragDropImage != null )
					{
						DragDropEffects effects = DoDragDrop( DragDropImage, DragDropEffects.Copy );

						/// this code is triggered after the drop event
						/// and on the initial click
						///

						bStartDragDrop = false;

						if( bHasDropped == true
							|| bInvalidSquareDrop == true )
						{
							DragDropFrom = square;
							bHasDropped = false;
							Cursor = Cursors.Default;
						}

						if( bPlayerTakesPiece == true )
						{
							RemovePieceFromSquare( strTakenPieceIdentifier );
						}

						/// Fixes the incredible disappearing pieces bug.
						/// If a piece was taken by the player and then the computer
						/// moved onto that square then the above code would make the computer
						/// piece disappear later on because the variables below
						/// hadn't been reset
						/// Bastard!!
						/// 
						bPlayerTakesPiece = false;
						strTakenPieceIdentifier = null;

						if( bInvalidSquareDrop == true )
						{
							foreach( DictionaryEntry dicEnt in GetHashTable )
							{
								BasicSquare squareTemp = ( BasicSquare )dicEnt.Value;

								squareTemp.DrawDragDropImage = false;
							}
						}


						/// remove highlights
						/// 

						foreach( DictionaryEntry dicEnt in GetHashTable )
						{
							BasicSquare squareTemp = ( BasicSquare )dicEnt.Value;

							squareTemp.DrawHighlight = false;
						}

						if( bCanStillTake == true )
						{
							effects = DoDragDrop( DragDropImage, DragDropEffects.None );
							return;
						}

						/// do the computers move
						/// 

						if( bInvalidSquareDrop == false
							&& bCanStillTake == false
							&& bAllowComputerMove == true )
						{
							PlayersMove = false;
							if( PlayerIsLight == true )
								ComputersMove( false );
							else
								ComputersMove( true );

							bAllowComputerMove = false;
						}

						Invalidate();
						Update();

					}
					else
					{
						MessageBox.Show( "You've not set the drag drop image dumbass" );
					}
				} 
				else
				{
					DragDropEffects effects = DoDragDrop( DragDropImage, DragDropEffects.None );
					bsStartSquare = null;
				}
			}

		}


		/// <summary>
		/// Do the drag and drop for a player making a move
		/// with a light piece
		/// </summary>
		/// <param name="square"></param>
		private void PlayerLightMove( DraughtsSquare square )
		{
			/// Light or red squares always start at the bottom and move up
			/// 

			bool bMustTake = game.CheckForMustTake( true );

			bCanStillTake = false;

			/// make sure there's no backwards movement
			/// 

			if( bsStartSquare.IsKing == false )
			{
				if( bsStartSquare != null 
					&& GetSquareAboveLeft( bsStartSquare.Identifier ) == square 
					|| GetSquareAboveRight( bsStartSquare.Identifier ) == square )
				{

					if( bMustTake == false )
					{
						square.DragDropImage = bitLightDrop;
						bAllowDrop = true;
					}
					else
					{
						square.DragDropImage = this.bitLightNoDrop;
						bAllowDrop = false;
					}

					return;
				}
			}
			else
			{
				if( bsStartSquare != null 
					&& GetSquareAboveLeft( bsStartSquare.Identifier ) == square 
					|| GetSquareAboveRight( bsStartSquare.Identifier ) == square
					|| GetSquareBelowLeft( bsStartSquare.Identifier ) == square 
					|| GetSquareBelowRight( bsStartSquare.Identifier ) == square )
				{
					if( bMustTake == false )
					{
						square.DragDropImage = bitLightKingDrop;
						bAllowDrop = true;
					}
					else
					{
						square.DragDropImage = bitLightKingNoDrop;
						bAllowDrop = false;
					}

					return;
				}
			}																		

			bPlayerTakesPiece = false;
								
			/// now the tricky bit
			/// Get the square to the upper left of the start square
			/// 
			if( bsStartSquare != null && bsStartSquare.Identifier[ 0 ] > square.Identifier[ 0 ] 
				|| bsStartSquare != null && bsStartSquare.Identifier[ 0 ] < square.Identifier[ 0 ] )
			{
				BasicSquare squareNext = GetSquareAboveLeft( bsStartSquare.Identifier );

				if( squareNext != null )
				{
					/// if the square above it is the current square
					/// 
					if( GetSquareAboveLeft( squareNext.Identifier ) == square )
					{
						/// if the square below right is a computer piece
						/// 
						if( squareNext.IsOccupied == true && squareNext.OccupyingName != bsStartSquare.OccupyingName )
						{
							if( bsStartSquare.IsKing == true )
								square.DragDropImage = bitLightKingDrop;
							else
								square.DragDropImage = bitLightDrop;

							bAllowDrop = true;
							bPlayerTakesPiece = true;
							strTakenPieceIdentifier = squareNext.Identifier;

							/// check to see if there is a continuing take
							/// 

							squareNext = GetSquareAboveLeft( square.Identifier );
 
							if( squareNext != null && squareNext.IsOccupied == true 
								&& squareNext.OccupyingName != bsStartSquare.OccupyingName )
							{
								squareNext = GetSquareAboveLeft( squareNext.Identifier );

								if( squareNext != null && squareNext.IsOccupied == false )
								{
									bCanStillTake = true;
									strTakingPieceIdentifier = squareNext.Identifier;
								}
							}

							squareNext = GetSquareAboveRight( square.Identifier );

							if( squareNext != null && squareNext.IsOccupied == true 
								&& squareNext.OccupyingName != bsStartSquare.OccupyingName )
							{
								squareNext = GetSquareAboveRight( squareNext.Identifier );

								if( squareNext != null && squareNext.IsOccupied == false )
								{
									bCanStillTake = true;
									strTakingPieceIdentifier = squareNext.Identifier;
								}
							}

							return;

						}
						else
						{
							if( bsStartSquare.IsKing == true )
								square.DragDropImage = bitLightKingNoDrop;
							else
								square.DragDropImage = bitLightNoDrop;

							bAllowDrop = false;
						}
					}
				}

				squareNext = GetSquareAboveRight( bsStartSquare.Identifier );

				if( squareNext != null )
				{

					if( GetSquareAboveRight( squareNext.Identifier ) == square )
					{
						if( squareNext.IsOccupied == true && squareNext.OccupyingName != bsStartSquare.OccupyingName )
						{
							if( bsStartSquare.IsKing == true )
								square.DragDropImage = bitLightKingDrop;
							else
								square.DragDropImage = bitLightDrop;

							bAllowDrop = true;
							bPlayerTakesPiece = true;
							strTakenPieceIdentifier = squareNext.Identifier;

							/// check to see if there is a continuing take
							/// 

							squareNext = GetSquareAboveLeft( square.Identifier );
 
							if( squareNext != null && squareNext.IsOccupied == true 
								&& squareNext.OccupyingName != bsStartSquare.OccupyingName )
							{
								squareNext = GetSquareAboveLeft( squareNext.Identifier );

								if( squareNext != null && squareNext.IsOccupied == false )
									bCanStillTake = true;
							}

							squareNext = GetSquareAboveRight( square.Identifier );

							if( squareNext != null && squareNext.IsOccupied == true 
								&& squareNext.OccupyingName != bsStartSquare.OccupyingName )
							{
								squareNext = GetSquareAboveRight( squareNext.Identifier );

								if( squareNext != null && squareNext.IsOccupied == false )
									bCanStillTake = true;
							}

							return;

						}
						else
						{
							if( bsStartSquare.IsKing == true )
								square.DragDropImage = bitLightKingNoDrop;
							else
								square.DragDropImage = bitLightNoDrop;

							bAllowDrop = false;
						}
					}
				}
			}


			/// now do the same for king only moves
			/// 

			if( bsStartSquare != null && bsStartSquare.IsKing == true )
			{
				if( bsStartSquare.Identifier[ 0 ] > square.Identifier[ 0 ] )
				{
					BasicSquare squareNext = GetSquareBelowLeft( bsStartSquare.Identifier );

					if( squareNext != null )
					{
						if( GetSquareBelowLeft( squareNext.Identifier ) == square )
						{
							if( squareNext.IsOccupied == true && squareNext.OccupyingName != bsStartSquare.OccupyingName )
							{
								square.DragDropImage = bitLightKingDrop;

								bAllowDrop = true;
								bPlayerTakesPiece = true;
								strTakenPieceIdentifier = squareNext.Identifier;

								/// check to see if there is a continuing take
								/// 

								squareNext = GetSquareBelowLeft( square.Identifier );
 
								if( squareNext != null && squareNext.IsOccupied == true 
									&& squareNext.OccupyingName != bsStartSquare.OccupyingName )
								{
									squareNext = GetSquareBelowLeft( squareNext.Identifier );

									if( squareNext != null && squareNext.IsOccupied == false )
										bCanStillTake = true;
								}

								squareNext = GetSquareBelowRight( square.Identifier );

								if( squareNext != null && squareNext.IsOccupied == true 
									&& squareNext.OccupyingName != bsStartSquare.OccupyingName )
								{
									squareNext = GetSquareBelowRight( squareNext.Identifier );

									if( squareNext != null && squareNext.IsOccupied == false )
										bCanStillTake = true;
								}

								squareNext = GetSquareAboveLeft( square.Identifier );
 
								if( squareNext != null && squareNext.IsOccupied == true 
									&& squareNext.OccupyingName != bsStartSquare.OccupyingName )
								{
									squareNext = GetSquareAboveLeft( squareNext.Identifier );

									if( squareNext != null && squareNext.IsOccupied == false )
										bCanStillTake = true;
								}

								squareNext = GetSquareAboveRight( square.Identifier );

								if( squareNext != null && squareNext.IsOccupied == true 
									&& squareNext.OccupyingName != bsStartSquare.OccupyingName )
								{
									squareNext = GetSquareAboveRight( squareNext.Identifier );

									if( squareNext != null && squareNext.IsOccupied == false )
										bCanStillTake = true;
								}
							}
							else
							{
								square.DragDropImage = bitLightKingNoDrop;

								bAllowDrop = false;
							}

							Invalidate();
							Update();
							return;
						}
					}
				}

				if( bsStartSquare.Identifier[ 0 ] < square.Identifier[ 0 ] )
				{
					BasicSquare squareNext = GetSquareBelowRight( bsStartSquare.Identifier );

					if( squareNext != null )
					{
						if( GetSquareBelowRight( squareNext.Identifier ) == square )
						{
							if( squareNext.IsOccupied == true && squareNext.OccupyingName != bsStartSquare.OccupyingName )
							{
								square.DragDropImage = bitLightKingDrop;

								bAllowDrop = true;
								bPlayerTakesPiece = true;
								strTakenPieceIdentifier = squareNext.Identifier;

								/// check to see if there is a continuing take
								/// 

								squareNext = GetSquareBelowLeft( square.Identifier );
 
								if( squareNext != null && squareNext.IsOccupied == true 
									&& squareNext.OccupyingName != bsStartSquare.OccupyingName )
								{
									squareNext = GetSquareBelowLeft( squareNext.Identifier );

									if( squareNext != null && squareNext.IsOccupied == false )
										bCanStillTake = true;
								}

								squareNext = GetSquareBelowRight( square.Identifier );

								if( squareNext != null && squareNext.IsOccupied == true 
									&& squareNext.OccupyingName != bsStartSquare.OccupyingName )
								{
									squareNext = GetSquareBelowRight( squareNext.Identifier );

									if( squareNext != null && squareNext.IsOccupied == false )
										bCanStillTake = true;
								}

								squareNext = GetSquareAboveLeft( square.Identifier );
 
								if( squareNext != null && squareNext.IsOccupied == true 
									&& squareNext.OccupyingName != bsStartSquare.OccupyingName )
								{
									squareNext = GetSquareAboveLeft( squareNext.Identifier );

									if( squareNext != null && squareNext.IsOccupied == false )
										bCanStillTake = true;
								}

								squareNext = GetSquareAboveRight( square.Identifier );

								if( squareNext != null && squareNext.IsOccupied == true 
									&& squareNext.OccupyingName != bsStartSquare.OccupyingName )
								{
									squareNext = GetSquareAboveRight( squareNext.Identifier );

									if( squareNext != null && squareNext.IsOccupied == false )
										bCanStillTake = true;
								}
							}
							else
							{
								square.DragDropImage = bitLightKingNoDrop;

								bAllowDrop = false;
							}

							Invalidate();
							Update();
							return;
						}
					}
				}
			}

			if( bsStartSquare.IsKing == true )
				square.DragDropImage = bitLightKingNoDrop;
			else
				square.DragDropImage = bitLightNoDrop;

			bAllowDrop = false;
		}

		/// <summary>
		/// do the drag and drop for a player making a move
		/// with a dark piece
		/// </summary>
		/// <param name="square"></param>
		private void PlayerDarkMove( DraughtsSquare square )
		{

			bool bMustTake = game.CheckForMustTake( true );

			bCanStillTake = false;

			/// make sure there's no backwards movement
			/// 

			if( bsStartSquare.IsKing == false )
			{
				if( bsStartSquare != null 
					&& GetSquareBelowLeft( bsStartSquare.Identifier ) == square 
					|| GetSquareBelowRight( bsStartSquare.Identifier ) == square )
				{

					if( bMustTake == false )
					{
						square.DragDropImage = bitDarkDrop;
						bAllowDrop = true;
					}
					else
					{
						square.DragDropImage = this.bitDarkNoDrop;
						bAllowDrop = false;
					}

					return;
				}
			}
			else
			{
				if( bsStartSquare != null 
					&& GetSquareAboveLeft( bsStartSquare.Identifier ) == square 
					|| GetSquareAboveRight( bsStartSquare.Identifier ) == square
					|| GetSquareBelowLeft( bsStartSquare.Identifier ) == square 
					|| GetSquareBelowRight( bsStartSquare.Identifier ) == square )
				{
					if( bMustTake == false )
					{
						square.DragDropImage = bitDarkKingDrop;
						bAllowDrop = true;
					}
					else
					{
						square.DragDropImage = bitDarkKingNoDrop;
						bAllowDrop = false;
					}

					return;
				}
			}																		

			bPlayerTakesPiece = false;

								
			/// now the tricky bit
			/// Get the square to the lower left of the start square
			/// 
			if( bsStartSquare != null && bsStartSquare.Identifier[ 0 ] > square.Identifier[ 0 ] 
				|| bsStartSquare != null && bsStartSquare.Identifier[ 0 ] < square.Identifier[ 0 ] )
			{
				BasicSquare squareNext = GetSquareBelowLeft( bsStartSquare.Identifier );

				if( squareNext != null )
				{
					/// if the square above it is the current square
					/// 
					if( GetSquareBelowLeft( squareNext.Identifier ) == square )
					{
						/// if the square below right is a computer piece
						/// 
						if( squareNext.IsOccupied == true && squareNext.OccupyingName != bsStartSquare.OccupyingName )
						{
							if( bsStartSquare.IsKing == true )
								square.DragDropImage = bitDarkKingDrop;
							else
								square.DragDropImage = bitDarkDrop;

							bAllowDrop = true;
							bPlayerTakesPiece = true;
							strTakenPieceIdentifier = squareNext.Identifier;

							/// check to see if there is a continuing take
							/// 

							squareNext = GetSquareBelowLeft( square.Identifier );
 
							if( squareNext != null && squareNext.IsOccupied == true 
								&& squareNext.OccupyingName != bsStartSquare.OccupyingName )
							{
								squareNext = GetSquareBelowLeft( squareNext.Identifier );

								if( squareNext != null && squareNext.IsOccupied == false )
								{
									bCanStillTake = true;
									strTakingPieceIdentifier = squareNext.Identifier;
								}
							}

							squareNext = GetSquareBelowRight( square.Identifier );

							if( squareNext != null && squareNext.IsOccupied == true 
								&& squareNext.OccupyingName != bsStartSquare.OccupyingName )
							{
								squareNext = GetSquareBelowRight( squareNext.Identifier );

								if( squareNext != null && squareNext.IsOccupied == false )
								{
									bCanStillTake = true;
									strTakingPieceIdentifier = squareNext.Identifier;
								}
							}

							return;

						}
						else
						{
							if( bsStartSquare.IsKing == true )
								square.DragDropImage = bitDarkKingNoDrop;
							else
								square.DragDropImage = bitDarkNoDrop;

							bAllowDrop = false;
						}
					}
				}

				squareNext = GetSquareBelowRight( bsStartSquare.Identifier );

				if( squareNext != null )
				{

					if( GetSquareBelowRight( squareNext.Identifier ) == square )
					{
						if( squareNext.IsOccupied == true && squareNext.OccupyingName != bsStartSquare.OccupyingName )
						{
							if( bsStartSquare.IsKing == true )
								square.DragDropImage = bitDarkKingDrop;
							else
								square.DragDropImage = bitDarkDrop;

							bAllowDrop = true;
							bPlayerTakesPiece = true;
							strTakenPieceIdentifier = squareNext.Identifier;

							/// check to see if there is a continuing take
							/// 

							squareNext = GetSquareBelowLeft( square.Identifier );
 
							if( squareNext != null && squareNext.IsOccupied == true 
								&& squareNext.OccupyingName != bsStartSquare.OccupyingName )
							{
								squareNext = GetSquareBelowLeft( squareNext.Identifier );

								if( squareNext != null && squareNext.IsOccupied == false )
									bCanStillTake = true;
							}

							squareNext = GetSquareBelowRight( square.Identifier );

							if( squareNext != null && squareNext.IsOccupied == true 
								&& squareNext.OccupyingName != bsStartSquare.OccupyingName )
							{
								squareNext = GetSquareBelowRight( squareNext.Identifier );

								if( squareNext != null && squareNext.IsOccupied == false )
									bCanStillTake = true;
							}

							return;

						}
						else
						{
							if( bsStartSquare.IsKing == true )
								square.DragDropImage = bitDarkKingNoDrop;
							else
								square.DragDropImage = bitDarkNoDrop;

							bAllowDrop = false;
						}
					}
				}
			}

			/// no do the same for king only moves
			/// 

			if( bsStartSquare != null && bsStartSquare.IsKing == true )
			{
				if( bsStartSquare.Identifier[ 0 ] > square.Identifier[ 0 ] )
				{
					BasicSquare squareNext = GetSquareAboveLeft( bsStartSquare.Identifier );

					if( squareNext != null )
					{
						if( GetSquareAboveLeft( squareNext.Identifier ) == square )
						{
							if( squareNext.IsOccupied == true && squareNext.OccupyingName != bsStartSquare.OccupyingName )
							{
								square.DragDropImage = bitDarkKingDrop;

								bAllowDrop = true;
								bPlayerTakesPiece = true;
								strTakenPieceIdentifier = squareNext.Identifier;

								/// check to see if there is a continuing take
								/// 

								squareNext = GetSquareAboveLeft( square.Identifier );
 
								if( squareNext != null && squareNext.IsOccupied == true 
									&& squareNext.OccupyingName != bsStartSquare.OccupyingName )
								{
									squareNext = GetSquareAboveLeft( squareNext.Identifier );

									if( squareNext != null && squareNext.IsOccupied == false )
										bCanStillTake = true;
								}

								squareNext = GetSquareAboveRight( square.Identifier );

								if( squareNext != null && squareNext.IsOccupied == true 
									&& squareNext.OccupyingName != bsStartSquare.OccupyingName )
								{
									squareNext = GetSquareAboveRight( squareNext.Identifier );

									if( squareNext != null && squareNext.IsOccupied == false )
										bCanStillTake = true;
								}

								squareNext = GetSquareBelowLeft( square.Identifier );
 
								if( squareNext != null && squareNext.IsOccupied == true 
									&& squareNext.OccupyingName != bsStartSquare.OccupyingName )
								{
									squareNext = GetSquareBelowLeft( squareNext.Identifier );

									if( squareNext != null && squareNext.IsOccupied == false )
										bCanStillTake = true;
								}

								squareNext = GetSquareBelowRight( square.Identifier );

								if( squareNext != null && squareNext.IsOccupied == true 
									&& squareNext.OccupyingName != bsStartSquare.OccupyingName )
								{
									squareNext = GetSquareBelowRight( squareNext.Identifier );

									if( squareNext != null && squareNext.IsOccupied == false )
										bCanStillTake = true;
								}
							}
							else
							{
								square.DragDropImage = bitDarkKingNoDrop;

								bAllowDrop = false;
							}

							return;
						}
					}
				}

				if( bsStartSquare.Identifier[ 0 ] < square.Identifier[ 0 ] )
				{
					BasicSquare squareNext = GetSquareAboveRight( bsStartSquare.Identifier );

					if( squareNext != null )
					{
						if( GetSquareAboveRight( squareNext.Identifier ) == square )
						{
							if( squareNext.IsOccupied == true && squareNext.OccupyingName != bsStartSquare.OccupyingName )
							{
								square.DragDropImage = bitLightKingDrop;

								bAllowDrop = true;
								bPlayerTakesPiece = true;
								strTakenPieceIdentifier = squareNext.Identifier;

								/// check to see if there is a continuing take
								/// 

								squareNext = GetSquareAboveLeft( square.Identifier );
 
								if( squareNext != null && squareNext.IsOccupied == true 
									&& squareNext.OccupyingName != bsStartSquare.OccupyingName )
								{
									squareNext = GetSquareAboveLeft( squareNext.Identifier );

									if( squareNext != null && squareNext.IsOccupied == false )
										bCanStillTake = true;
								}

								squareNext = GetSquareAboveRight( square.Identifier );

								if( squareNext != null && squareNext.IsOccupied == true 
									&& squareNext.OccupyingName != bsStartSquare.OccupyingName )
								{
									squareNext = GetSquareAboveRight( squareNext.Identifier );

									if( squareNext != null && squareNext.IsOccupied == false )
										bCanStillTake = true;
								}

								squareNext = GetSquareBelowLeft( square.Identifier );
 
								if( squareNext != null && squareNext.IsOccupied == true 
									&& squareNext.OccupyingName != bsStartSquare.OccupyingName )
								{
									squareNext = GetSquareBelowLeft( squareNext.Identifier );

									if( squareNext != null && squareNext.IsOccupied == false )
										bCanStillTake = true;
								}

								squareNext = GetSquareBelowRight( square.Identifier );

								if( squareNext != null && squareNext.IsOccupied == true 
									&& squareNext.OccupyingName != bsStartSquare.OccupyingName )
								{
									squareNext = GetSquareBelowRight( squareNext.Identifier );

									if( squareNext != null && squareNext.IsOccupied == false )
										bCanStillTake = true;
								}
							}
							else
							{
								square.DragDropImage = bitDarkKingNoDrop;

								bAllowDrop = false;
							}

							return;
						}
					}
				}
			}

			if( bsStartSquare.IsKing == true )
				square.DragDropImage = bitDarkKingNoDrop;
			else
				square.DragDropImage = bitDarkNoDrop;

		}

		public void Start( bool lightSquares, bool playerIsLight )
		{
			CleanUp();
			ClearBoard();

			PlayerIsLight = playerIsLight;

			PlayOnLightSquares = lightSquares;

			StartPieces();
		}

		private void PlacePieceOnSquare( string strSquareIdentifier, bool bLightPiece )
		{
			DraughtsSquare square = ( DraughtsSquare )GetSquare( strSquareIdentifier );

			if( square != null )
			{
				square.IsOccupied = true;
				square.IsValid = false;
				square.IsKing = false;
				
				if( PlayerIsLight == true && bLightPiece == true 
					|| PlayerIsLight == false && bLightPiece == true )
				{
					square.OccupyingName = "PLAYER";
					square.PlayerIsOnSquare = true;
				}
				if( PlayerIsLight == true && bLightPiece == false 
					|| PlayerIsLight == false && bLightPiece == false )
				{
					square.OccupyingName = "COMPUTER";
					square.PlayerIsOnSquare = false;
				}
			}

			Invalidate();
			Update();
		}


		private void RemovePieceFromSquare( string strSquareIdentifier )
		{
			DraughtsSquare square = ( DraughtsSquare )GetSquare( strSquareIdentifier );

			if( square != null )
			{
				square.IsOccupied = false;
				square.IsValid = false;
				square.OccupyingName = "EMPTY";
				square.PlayerIsOnSquare = false;
			}

			NumberOfMovesNoneTaken = 0;
		}

		private void StartPieces()
		{
			if( PlayOnLightSquares == true )
			{
				if( PlayerIsLight == true )
				{
					PlacePieceOnSquare( "AA", false );
					PlacePieceOnSquare( "CA", false );
					PlacePieceOnSquare( "EA", false );
					PlacePieceOnSquare( "GA", false );
					PlacePieceOnSquare( "BB", false );
					PlacePieceOnSquare( "DB", false );
					PlacePieceOnSquare( "FB", false );
					PlacePieceOnSquare( "HB", false );
					PlacePieceOnSquare( "AC", false );
					PlacePieceOnSquare( "CC", false );
					PlacePieceOnSquare( "EC", false );
					PlacePieceOnSquare( "GC", false );
					PlacePieceOnSquare( "BF", true );
					PlacePieceOnSquare( "DF", true );
					PlacePieceOnSquare( "FF", true );
					PlacePieceOnSquare( "HF", true );
					PlacePieceOnSquare( "AG", true );
					PlacePieceOnSquare( "CG", true );
					PlacePieceOnSquare( "EG", true );
					PlacePieceOnSquare( "GG", true );
					PlacePieceOnSquare( "BH", true );
					PlacePieceOnSquare( "DH", true );
					PlacePieceOnSquare( "FH", true );
					PlacePieceOnSquare( "HH", true );
				}
				else
				{
					PlacePieceOnSquare( "AA", true );
					PlacePieceOnSquare( "CA", true );
					PlacePieceOnSquare( "EA", true );
					PlacePieceOnSquare( "GA", true );
					PlacePieceOnSquare( "BB", true );
					PlacePieceOnSquare( "DB", true );
					PlacePieceOnSquare( "FB", true );
					PlacePieceOnSquare( "HB", true );
					PlacePieceOnSquare( "AC", true );
					PlacePieceOnSquare( "CC", true );
					PlacePieceOnSquare( "EC", true );
					PlacePieceOnSquare( "GC", true );
					PlacePieceOnSquare( "BF", false );
					PlacePieceOnSquare( "DF", false );
					PlacePieceOnSquare( "FF", false );
					PlacePieceOnSquare( "HF", false );
					PlacePieceOnSquare( "AG", false );
					PlacePieceOnSquare( "CG", false );
					PlacePieceOnSquare( "EG", false );
					PlacePieceOnSquare( "GG", false );
					PlacePieceOnSquare( "BH", false );
					PlacePieceOnSquare( "DH", false );
					PlacePieceOnSquare( "FH", false );
					PlacePieceOnSquare( "HH", false );
				}
			}
			else
			{
				if( PlayerIsLight == true )
				{
					PlacePieceOnSquare( "BA", false );
					PlacePieceOnSquare( "DA", false );
					PlacePieceOnSquare( "FA", false );
					PlacePieceOnSquare( "HA", false );
					PlacePieceOnSquare( "AB", false );
					PlacePieceOnSquare( "CB", false );
					PlacePieceOnSquare( "EB", false );
					PlacePieceOnSquare( "GB", false );
					PlacePieceOnSquare( "BC", false );
					PlacePieceOnSquare( "DC", false );
					PlacePieceOnSquare( "FC", false );
					PlacePieceOnSquare( "HC", false );
					PlacePieceOnSquare( "AF", true );
					PlacePieceOnSquare( "CF", true );
					PlacePieceOnSquare( "EF", true );
					PlacePieceOnSquare( "GF", true );
					PlacePieceOnSquare( "BG", true );
					PlacePieceOnSquare( "DG", true );
					PlacePieceOnSquare( "FG", true );
					PlacePieceOnSquare( "HG", true );
					PlacePieceOnSquare( "AH", true );
					PlacePieceOnSquare( "CH", true );
					PlacePieceOnSquare( "EH", true );
					PlacePieceOnSquare( "GH", true );
				}
				else
				{
					PlacePieceOnSquare( "BA", true );
					PlacePieceOnSquare( "DA", true );
					PlacePieceOnSquare( "FA", true );
					PlacePieceOnSquare( "HA", true );
					PlacePieceOnSquare( "AB", true );
					PlacePieceOnSquare( "CB", true );
					PlacePieceOnSquare( "EB", true );
					PlacePieceOnSquare( "GB", true );
					PlacePieceOnSquare( "BC", true );
					PlacePieceOnSquare( "DC", true );
					PlacePieceOnSquare( "FC", true );
					PlacePieceOnSquare( "HC", true );
					PlacePieceOnSquare( "AF", false );
					PlacePieceOnSquare( "CF", false );
					PlacePieceOnSquare( "EF", false );
					PlacePieceOnSquare( "GF", false );
					PlacePieceOnSquare( "BG", false );
					PlacePieceOnSquare( "DG", false );
					PlacePieceOnSquare( "FG", false );
					PlacePieceOnSquare( "HG", false );
					PlacePieceOnSquare( "AH", false );
					PlacePieceOnSquare( "CH", false );
					PlacePieceOnSquare( "EH", false );
					PlacePieceOnSquare( "GH", false );
				}
			}

			SetupSquares();


		///	Randomise the start
		///	
			Random rand = new Random( DateTime.Now.Second );

			int nTest = rand.Next( 2 );
			if( nTest >= 1 )
				ComputersMove( !PlayerIsLight );

		}

		private void HighlightSquares( DraughtsSquare currentSquare )
		{
			bool bCanTake = false;

			if( PlayerIsLight == true )
			{
				BasicSquare square = GetSquareAboveLeft( currentSquare.Identifier );

				if( square != null && square.IsOccupied == true )
				{
					if( square.OccupyingName == "COMPUTER" )
					{
						BasicSquare squareTemp = GetSquareAboveLeft( square.Identifier );
						BasicSquare squareToMoveTo = null;

						if( squareTemp != null && squareTemp.IsOccupied == false )
						{
							squareTemp.DrawHighlight = true;
							bCanTake = true;

							bool bMultiSquares = true;

							/// check to see if it is more than a single jump
							/// 
							while( bMultiSquares == true )
							{
								bMultiSquares = false;

								squareToMoveTo = squareTemp;
								BasicSquare squareNext = GetSquareAboveLeft( squareTemp.Identifier );

								if( squareNext != null && squareNext.IsOccupied == true )
								{
									if( squareNext.OccupyingName == "COMPUTER" )
									{
										squareTemp = GetSquareAboveLeft( squareNext.Identifier );

										if( squareTemp != null && squareTemp.IsOccupied == false )
										{
											squareTemp.DrawHighlight = true;
											bMultiSquares = true;
										}
									}
								}

								squareNext = GetSquareAboveRight( squareToMoveTo.Identifier );

								if( squareNext != null && squareNext.IsOccupied == true )
								{
									if( squareNext.OccupyingName == "COMPUTER" )
									{
										squareTemp = GetSquareAboveRight( squareNext.Identifier );

										if( squareTemp != null && squareTemp.IsOccupied == false )
										{
											squareTemp.DrawHighlight = true;
											bMultiSquares = true;
										}
									}
								}
							}
						}
					}
				}

				square = GetSquareAboveRight( currentSquare.Identifier );

				if( square != null && square.IsOccupied == true )
				{
					if( square.OccupyingName == "COMPUTER" )
					{
						BasicSquare squareTemp = GetSquareAboveRight( square.Identifier );
						BasicSquare squareToMoveTo = null;

						if( squareTemp != null && squareTemp.IsOccupied == false )
						{
							squareTemp.DrawHighlight = true;
							bCanTake = true;

							bool bMultiSquares = true;

							/// check to see if it is more than a single jump
							/// 
							while( bMultiSquares == true )
							{
								bMultiSquares = false;
								squareToMoveTo = squareTemp;

								BasicSquare squareNext = GetSquareAboveLeft( squareTemp.Identifier );

								if( squareNext != null && squareNext.IsOccupied == true )
								{
									if( squareNext.OccupyingName == "COMPUTER" )
									{
										squareTemp = GetSquareAboveLeft( squareNext.Identifier );

										if( squareTemp != null && squareTemp.IsOccupied == false )
										{
											squareTemp.DrawHighlight = true;
											bMultiSquares = true;
										}
									}
								}

								squareNext = GetSquareAboveRight( squareToMoveTo.Identifier );

								if( squareNext != null && squareNext.IsOccupied == true )
								{
									if( squareNext.OccupyingName == "COMPUTER" )
									{
										squareTemp = GetSquareAboveRight( squareNext.Identifier );

										if( squareTemp != null && squareTemp.IsOccupied == false )
										{
											squareTemp.DrawHighlight = true;
											bMultiSquares = true;
										}
									}
								}
							}
						}
					}
				}

				/// _TODO the highlights for a player king piece here
				/// 

				if( currentSquare.IsKing == true )
				{
					square = GetSquareBelowRight( currentSquare.Identifier );

					if( square != null && square.IsOccupied == true )
					{
						if( square.OccupyingName == "COMPUTER" )
						{
							BasicSquare squareTemp = GetSquareBelowRight( square.Identifier );
							BasicSquare squareToMoveTo = null;

							if( squareTemp != null && squareTemp.IsOccupied == false )
							{
								squareTemp.DrawHighlight = true;
								bCanTake = true;

								bool bMultiSquares = true;

								/// check to see if it is more than a single jump
								/// 
								while( bMultiSquares == true )
								{
									bMultiSquares = false;
									squareToMoveTo = squareTemp;

									BasicSquare squareNext = GetSquareBelowLeft( squareTemp.Identifier );

									if( squareNext != null && squareNext.IsOccupied == true )
									{
										if( squareNext.OccupyingName == "COMPUTER" )
										{
											squareTemp = GetSquareBelowLeft( squareNext.Identifier );

											if( squareTemp != null && squareTemp.IsOccupied == false )
											{
												squareTemp.DrawHighlight = true;
												bMultiSquares = true;
											}
										}
									}

									squareNext = GetSquareBelowRight( squareToMoveTo.Identifier );

									if( squareNext != null && squareNext.IsOccupied == true )
									{
										if( squareNext.OccupyingName == "COMPUTER" )
										{
											squareTemp = GetSquareBelowRight( squareNext.Identifier );

											if( squareTemp != null && squareTemp.IsOccupied == false )
											{
												squareTemp.DrawHighlight = true;
												bMultiSquares = true;
											}
										}
									}
								}

								/// do this in two blocks or the temp square moves backwards and forwards
								/// 

								squareTemp = GetSquareBelowRight( square.Identifier );
								bMultiSquares = true;

								while( bMultiSquares == true )
								{
									bMultiSquares = false;
									squareToMoveTo = squareTemp;

									BasicSquare squareNext = GetSquareBelowLeft( squareTemp.Identifier );

									if( squareNext != null && squareNext.IsOccupied == true )
									{
										if( squareNext.OccupyingName == "COMPUTER" )
										{
											squareTemp = GetSquareAboveLeft( squareNext.Identifier );

											if( squareTemp != null && squareTemp.IsOccupied == false )
											{
												squareTemp.DrawHighlight = true;
												bMultiSquares = true;
											}
										}
									}

									squareNext = GetSquareAboveRight( squareToMoveTo.Identifier );

									if( squareNext != null && squareNext.IsOccupied == true )
									{
										if( squareNext.OccupyingName == "COMPUTER" )
										{
											squareTemp = GetSquareAboveRight( squareNext.Identifier );

											if( squareTemp != null && squareTemp.IsOccupied == false )
											{
												squareTemp.DrawHighlight = true;
												bMultiSquares = true;
											}
										}
									}
								}
							}
						}
					}

					square = GetSquareBelowLeft( currentSquare.Identifier );

					if( square != null && square.IsOccupied == true )
					{
						if( square.OccupyingName == "COMPUTER" )
						{
							BasicSquare squareTemp = GetSquareBelowLeft( square.Identifier );
							BasicSquare squareToMoveTo = null;

							if( squareTemp != null && squareTemp.IsOccupied == false )
							{
								squareTemp.DrawHighlight = true;
								bCanTake = true;

								bool bMultiSquares = true;

								/// check to see if it is more than a single jump
								/// 
								while( bMultiSquares == true )
								{
									bMultiSquares = false;
									squareToMoveTo = squareTemp;

									BasicSquare squareNext = GetSquareBelowLeft( squareTemp.Identifier );

									if( squareNext != null && squareNext.IsOccupied == true )
									{
										if( squareNext.OccupyingName == "COMPUTER" )
										{
											squareTemp = GetSquareBelowLeft( squareNext.Identifier );

											if( squareTemp != null && squareTemp.IsOccupied == false )
											{
												squareTemp.DrawHighlight = true;
												bMultiSquares = true;
											}
										}
									}

									squareNext = GetSquareBelowRight( squareToMoveTo.Identifier );

									if( squareNext != null && squareNext.IsOccupied == true )
									{
										if( squareNext.OccupyingName == "COMPUTER" )
										{
											squareTemp = GetSquareBelowRight( squareNext.Identifier );

											if( squareTemp != null && squareTemp.IsOccupied == false )
											{
												squareTemp.DrawHighlight = true;
												bMultiSquares = true;
											}
										}
									}
								}

								squareTemp = GetSquareBelowLeft( square.Identifier );
								bMultiSquares = true;

								while( bMultiSquares == true )
								{
									bMultiSquares = false;
									squareToMoveTo = squareTemp;

									BasicSquare squareNext = GetSquareBelowLeft( squareTemp.Identifier );

									if( squareNext != null && squareNext.IsOccupied == true )
									{
										if( squareNext.OccupyingName == "COMPUTER" )
										{
											squareTemp = GetSquareAboveLeft( squareNext.Identifier );

											if( squareTemp != null && squareTemp.IsOccupied == false )
											{
												squareTemp.DrawHighlight = true;
												bMultiSquares = true;
											}
										}
									}

									squareNext = GetSquareAboveRight( squareToMoveTo.Identifier );

									if( squareNext != null && squareNext.IsOccupied == true )
									{
										if( squareNext.OccupyingName == "COMPUTER" )
										{
											squareTemp = GetSquareAboveRight( squareNext.Identifier );

											if( squareTemp != null && squareTemp.IsOccupied == false )
											{
												squareTemp.DrawHighlight = true;
												bMultiSquares = true;
											}
										}
									}
								}
							}
						}
					}
				}

				if( bCanTake == false )
				{
					square = GetSquareAboveLeft( currentSquare.Identifier );

					if( square != null && square.IsOccupied == false )
					{
						square.DrawHighlight = true;
					}

					square = GetSquareAboveRight( currentSquare.Identifier );

					if( square != null && square.IsOccupied == false )
					{
						square.DrawHighlight = true;
					}

					if( currentSquare.IsKing == true )
					{
						square = GetSquareBelowRight( currentSquare.Identifier );

						if( square != null && square.IsOccupied == false )
						{
							square.DrawHighlight = true;
						}

						square = GetSquareBelowLeft( currentSquare.Identifier );

						if( square != null && square.IsOccupied == false )
						{
							if( bCanTake == false )
								square.DrawHighlight = true;
						}
					}

				}
			}
			else
			{
				BasicSquare square = GetSquareBelowLeft( currentSquare.Identifier );

				if( square != null && square.IsOccupied == true )
				{
					if( square.OccupyingName == "COMPUTER" )
					{
						BasicSquare squareTemp = GetSquareBelowLeft( square.Identifier );
						BasicSquare squareToMoveTo = null;

						if( squareTemp != null && squareTemp.IsOccupied == false )
						{
							squareTemp.DrawHighlight = true;
							bCanTake = true;

							bool bMultiSquares = true;

							/// check to see if it is more than a single jump
							/// 
							while( bMultiSquares == true )
							{
								bMultiSquares = false;

								squareToMoveTo = squareTemp;
								BasicSquare squareNext = GetSquareBelowLeft( squareTemp.Identifier );

								if( squareNext != null && squareNext.IsOccupied == true )
								{
									if( squareNext.OccupyingName == "COMPUTER" )
									{
										squareTemp = GetSquareBelowLeft( squareNext.Identifier );

										if( squareTemp != null && squareTemp.IsOccupied == false )
										{
											squareTemp.DrawHighlight = true;
											bMultiSquares = true;
										}
									}
								}

								squareNext = GetSquareBelowRight( squareToMoveTo.Identifier );

								if( squareNext != null && squareNext.IsOccupied == true )
								{
									if( squareNext.OccupyingName == "COMPUTER" )
									{
										squareTemp = GetSquareBelowRight( squareNext.Identifier );

										if( squareTemp != null && squareTemp.IsOccupied == false )
										{
											squareTemp.DrawHighlight = true;
											bMultiSquares = true;
										}
									}
								}
							}
						}
					}
				}

				square = GetSquareBelowRight( currentSquare.Identifier );

				if( square != null && square.IsOccupied == true )
				{
					if( square.OccupyingName == "COMPUTER" )
					{
						BasicSquare squareTemp = GetSquareBelowRight( square.Identifier );
						BasicSquare squareToMoveTo = null;

						if( squareTemp != null && squareTemp.IsOccupied == false )
						{
							squareTemp.DrawHighlight = true;
							bCanTake = true;

							bool bMultiSquares = true;

							/// check to see if it is more than a single jump
							/// 
							while( bMultiSquares == true )
							{
								bMultiSquares = false;
								squareToMoveTo = squareTemp;

								BasicSquare squareNext = GetSquareBelowLeft( squareTemp.Identifier );

								if( squareNext != null && squareNext.IsOccupied == true )
								{
									if( squareNext.OccupyingName == "COMPUTER" )
									{
										squareTemp = GetSquareBelowLeft( squareNext.Identifier );

										if( squareTemp != null && squareTemp.IsOccupied == false )
										{
											squareTemp.DrawHighlight = true;
											bMultiSquares = true;
										}
									}
								}

								squareNext = GetSquareBelowRight( squareToMoveTo.Identifier );

								if( squareNext != null && squareNext.IsOccupied == true )
								{
									if( squareNext.OccupyingName == "COMPUTER" )
									{
										squareTemp = GetSquareBelowRight( squareNext.Identifier );

										if( squareTemp != null && squareTemp.IsOccupied == false )
										{
											squareTemp.DrawHighlight = true;
											bMultiSquares = true;
										}
									}
								}
							}
						}
					}
				}

				if( currentSquare.IsKing == true )
				{
					square = GetSquareAboveRight( currentSquare.Identifier );

					if( square != null && square.IsOccupied == true )
					{
						if( square.OccupyingName == "COMPUTER" )
						{
							BasicSquare squareTemp = GetSquareAboveRight( square.Identifier );
							BasicSquare squareToMoveTo = null;

							if( squareTemp != null && squareTemp.IsOccupied == false )
							{
								squareTemp.DrawHighlight = true;
								bCanTake = true;

								bool bMultiSquares = true;

								/// check to see if it is more than a single jump
								/// 
								while( bMultiSquares == true )
								{
									bMultiSquares = false;
									squareToMoveTo = squareTemp;

									BasicSquare squareNext = GetSquareAboveLeft( squareTemp.Identifier );

									if( squareNext != null && squareNext.IsOccupied == true )
									{
										if( squareNext.OccupyingName == "COMPUTER" )
										{
											squareTemp = GetSquareAboveLeft( squareNext.Identifier );

											if( squareTemp != null && squareTemp.IsOccupied == false )
											{
												squareTemp.DrawHighlight = true;
												bMultiSquares = true;
											}
										}
									}

									squareNext = GetSquareAboveRight( squareToMoveTo.Identifier );

									if( squareNext != null && squareNext.IsOccupied == true )
									{
										if( squareNext.OccupyingName == "COMPUTER" )
										{
											squareTemp = GetSquareAboveRight( squareNext.Identifier );

											if( squareTemp != null && squareTemp.IsOccupied == false )
											{
												squareTemp.DrawHighlight = true;
												bMultiSquares = true;
											}
										}
									}
								}

								squareTemp = GetSquareAboveRight( square.Identifier );
								bMultiSquares = true;

								while( bMultiSquares == true )
								{
									bMultiSquares = false;
									squareToMoveTo = squareTemp;

									BasicSquare squareNext = GetSquareBelowLeft( squareTemp.Identifier );

									if( squareNext != null && squareNext.IsOccupied == true )
									{
										if( squareNext.OccupyingName == "COMPUTER" )
										{
											squareTemp = GetSquareBelowLeft( squareNext.Identifier );

											if( squareTemp != null && squareTemp.IsOccupied == false )
											{
												squareTemp.DrawHighlight = true;
												bMultiSquares = true;
											}
										}
									}

									squareNext = GetSquareBelowRight( squareToMoveTo.Identifier );

									if( squareNext != null && squareNext.IsOccupied == true )
									{
										if( squareNext.OccupyingName == "COMPUTER" )
										{
											squareTemp = GetSquareBelowRight( squareNext.Identifier );

											if( squareTemp != null && squareTemp.IsOccupied == false )
											{
												squareTemp.DrawHighlight = true;
												bMultiSquares = true;
											}
										}
									}
								}								
							}
						}
					}

					square = GetSquareAboveLeft( currentSquare.Identifier );

					if( square != null && square.IsOccupied == true )
					{
						if( square.OccupyingName == "COMPUTER" )
						{
							BasicSquare squareTemp = GetSquareAboveLeft( square.Identifier );
							BasicSquare squareToMoveTo = null;

							if( squareTemp != null && squareTemp.IsOccupied == false )
							{
								squareTemp.DrawHighlight = true;
								bCanTake = true;

								bool bMultiSquares = true;

								/// check to see if it is more than a single jump
								/// 
								while( bMultiSquares == true )
								{
									bMultiSquares = false;
									squareToMoveTo = squareTemp;

									BasicSquare squareNext = GetSquareAboveLeft( squareTemp.Identifier );

									if( squareNext != null && squareNext.IsOccupied == true )
									{
										if( squareNext.OccupyingName == "COMPUTER" )
										{
											squareTemp = GetSquareAboveLeft( squareNext.Identifier );

											if( squareTemp != null && squareTemp.IsOccupied == false )
											{
												squareTemp.DrawHighlight = true;
												bMultiSquares = true;
											}
										}
									}

									squareNext = GetSquareAboveRight( squareToMoveTo.Identifier );

									if( squareNext != null && squareNext.IsOccupied == true )
									{
										if( squareNext.OccupyingName == "COMPUTER" )
										{
											squareTemp = GetSquareAboveRight( squareNext.Identifier );

											if( squareTemp != null && squareTemp.IsOccupied == false )
											{
												squareTemp.DrawHighlight = true;
												bMultiSquares = true;
											}
										}
									}
								}

								squareTemp = GetSquareAboveLeft( square.Identifier );
								bMultiSquares = true;

								while( bMultiSquares == true )
								{
									bMultiSquares = false;
									squareToMoveTo = squareTemp;

									BasicSquare squareNext = GetSquareBelowLeft( squareTemp.Identifier );

									if( squareNext != null && squareNext.IsOccupied == true )
									{
										if( squareNext.OccupyingName == "COMPUTER" )
										{
											squareTemp = GetSquareBelowLeft( squareNext.Identifier );

											if( squareTemp != null && squareTemp.IsOccupied == false )
											{
												squareTemp.DrawHighlight = true;
												bMultiSquares = true;
											}
										}
									}

									squareNext = GetSquareBelowRight( squareToMoveTo.Identifier );

									if( squareNext != null && squareNext.IsOccupied == true )
									{
										if( squareNext.OccupyingName == "COMPUTER" )
										{
											squareTemp = GetSquareBelowRight( squareNext.Identifier );

											if( squareTemp != null && squareTemp.IsOccupied == false )
											{
												squareTemp.DrawHighlight = true;
												bMultiSquares = true;
											}
										}
									}
								}

							}
						}
					}
			
				}

				if( bCanTake == false )
				{
					square = GetSquareBelowLeft( currentSquare.Identifier );

					if( square != null && square.IsOccupied == false )
					{
						square.DrawHighlight = true;
					}

					square = GetSquareBelowRight( currentSquare.Identifier );

					if( square != null && square.IsOccupied == false )
					{
						square.DrawHighlight = true;
					}

					if( currentSquare.IsKing == true )
					{
						square = GetSquareAboveRight( currentSquare.Identifier );

						if( square != null && square.IsOccupied == false )
						{
							square.DrawHighlight = true;
						}

						square = GetSquareAboveLeft( currentSquare.Identifier );

						if( square != null && square.IsOccupied == false )
						{
							square.DrawHighlight = true;
						}
					}

				}

			}
		}

		/// <summary>
		/// move the computer piece
		/// Note the computer takes a bool for the piece colour
		/// as this will make life easier for automatic training
		/// </summary>
		public void ComputersMove( bool light )
		{
			PlayersMove = false;

			if( NumberOfMovesNoneTaken > NumberOfMovesWithoutTake )
			{
				/// stalemate
				/// 

				MessageBox.Show( "The Number Of Moves without a piece being taken has exceeded the maximum allowed\nThe game is declared a draw" );
				game.UpdateHistoricalPatterns( false );
				CleanUp();

				NumberOfMovesNoneTaken = 0;

				return;
			}

			game.GetAvailablePieces( light );

			if( CheckForPlayerWin( light ) == true )
			{
				/// player has won
				/// 

				MessageBox.Show( "Player has won the game" );
				game.UpdateHistoricalPatterns( false );
				CleanUp();

				return;
			}

			game.MovePiece( light );

			if( game.PieceTaken == true )
				NumberOfMovesNoneTaken = 0;

			if( game.CannotMove == true )
			{
				MessageBox.Show( "Computer cannot move player wins" );
				game.UpdateHistoricalPatterns( false );
				CleanUp();

				return;
			}

			if( CheckForComputerWin( light ) == true )
			{
				/// computer has won
				/// 

				MessageBox.Show( "Computer has won the game" );
				game.UpdateHistoricalPatterns( true );
				CleanUp();

				return;
			}


			if( AutomaticTraining == false )
				PlayersMove = true;
		}

		public bool CheckForPlayerWin( bool light )
		{
			if( game.GetPiecesCount( "COMPUTER", light ) == 0 )
				return true;

			return false;
		}

		public bool CheckForComputerWin( bool light )
		{
			if( game.GetPiecesCount( "PLAYER", light ) == 0 )
				return true;

			return false;
		}

		/// <summary>
		/// The game has ended so do end game stuff here
		/// </summary>
		public void CleanUp()
		{

			foreach( DictionaryEntry dicEnt in GetHashTable )
			{
				DraughtsSquare square = ( DraughtsSquare )dicEnt.Value;

				square.PlayerIsOnSquare = false;
				square.IsKing = false;
				square.IsOccupied = false;
				square.OccupyingName = "EMPTY";
				square.IsValid = false;
			}

			game.ResetGame();

		}

	}
}

