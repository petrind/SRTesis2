using System;
using System.Drawing;
using System.Windows.Forms;
using BoardControl;

namespace Draughts
{
	/// <summary>
	/// Summary description for DraughtsSquare.
	/// </summary>
	public class DraughtsSquare : BasicSquare
	{

		/// <summary>
		/// bitmaps used by the square
		/// </summary>
		private Bitmap bitPlayerPiece;
		private Bitmap bitPlayerKingPiece;
		private Bitmap bitComputerPiece;
		private Bitmap bitComputerKingPiece;
		private Bitmap bitTestPiece;

		private bool bPlayerIsOnSquare;
		private bool bIsKing;

		public Bitmap PlayerPiece
		{
			get
			{
				return bitPlayerPiece;
			}
			set
			{
				bitPlayerPiece = value;
			}
		}

		public Bitmap PlayerKingPiece
		{
			get
			{
				return bitPlayerKingPiece;
			}
			set
			{
				bitPlayerKingPiece = value;
			}
		}

		public Bitmap ComputerPiece
		{
			get
			{
				return bitComputerPiece;
			}
			set
			{
				bitComputerPiece = value;
			}
		}

		public Bitmap ComputerKingPiece
		{
			get
			{
				return bitComputerKingPiece;
			}
			set
			{
				bitComputerKingPiece = value;
			}
		}

		public Bitmap TestPiece
		{
			get
			{
				return bitTestPiece;
			}
			set
			{
				bitTestPiece = value;
			}
		}

		public bool PlayerIsOnSquare
		{
			get
			{
				return bPlayerIsOnSquare;
			}
			set
			{
				bPlayerIsOnSquare = value;
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

		public DraughtsSquare()
		{
			//
			// _TODO: Add constructor logic here
			//

			PlayerPiece = null;
			PlayerKingPiece = null;
			ComputerPiece = null;
			ComputerKingPiece = null;
			TestPiece = null;

			PlayerIsOnSquare = false;
			IsKing = false;
		}

		public DraughtsSquare( int squareWidth, int squareHeight, int squareHorizontalLocation, int squareVerticalLocation, string identifier ) : base( squareWidth, squareHeight, squareHorizontalLocation, squareVerticalLocation, identifier )
		{
			PlayerPiece = null;
			PlayerKingPiece = null;
			ComputerPiece = null;
			ComputerKingPiece = null;
			TestPiece = null;

			PlayerIsOnSquare = false;
			IsKing = false;
		}

		public override void DrawSquare( Graphics grfx )
		{
			if( IsValid == true )
				return;

			base.DrawSquare( grfx );

			switch( OccupyingName )
			{
				case "TEST":
				{
					if( IsOccupied == true && TestPiece != null && DrawDragDropImage == false )
					{
						if( DrawLegend == true )
							grfx.DrawImage( TestPiece, SquareHorizontalLocation + LegendWidth, SquareVerticalLocation + LegendWidth, SquareWidth, SquareHeight );
						else
							grfx.DrawImage( TestPiece, SquareHorizontalLocation, SquareVerticalLocation, SquareWidth, SquareHeight );
					}
				}break;
				case "PLAYER":
				{
					if( IsKing == false )
					{
						if( IsOccupied == true && PlayerPiece != null && DrawDragDropImage == false )
						{
							if( DrawLegend == true )
								grfx.DrawImage( PlayerPiece, SquareHorizontalLocation + LegendWidth, SquareVerticalLocation + LegendWidth, SquareWidth, SquareHeight );
							else
								grfx.DrawImage( PlayerPiece, SquareHorizontalLocation, SquareVerticalLocation, SquareWidth, SquareHeight );
						}
					}
					else
					{
						if( IsOccupied == true && PlayerKingPiece != null && DrawDragDropImage == false )
						{
							if( DrawLegend == true )
								grfx.DrawImage( PlayerKingPiece, SquareHorizontalLocation + LegendWidth, SquareVerticalLocation + LegendWidth, SquareWidth, SquareHeight );
							else
								grfx.DrawImage( PlayerKingPiece, SquareHorizontalLocation, SquareVerticalLocation, SquareWidth, SquareHeight );
						}
					}
				}break;
				case "COMPUTER":
				{
					if( IsKing == false )
					{
						if( IsOccupied == true && ComputerPiece != null && DrawDragDropImage == false )
						{
							if( DrawLegend == true )
								grfx.DrawImage( ComputerPiece, SquareHorizontalLocation + LegendWidth, SquareVerticalLocation + LegendWidth, SquareWidth, SquareHeight );
							else
								grfx.DrawImage( ComputerPiece, SquareHorizontalLocation, SquareVerticalLocation, SquareWidth, SquareHeight );
						}
					}
					else
					{
						if( IsOccupied == true && ComputerKingPiece != null && DrawDragDropImage == false )
						{
							if( DrawLegend == true )
								grfx.DrawImage( ComputerKingPiece, SquareHorizontalLocation + LegendWidth, SquareVerticalLocation + LegendWidth, SquareWidth, SquareHeight );
							else
								grfx.DrawImage( ComputerKingPiece, SquareHorizontalLocation, SquareVerticalLocation, SquareWidth, SquareHeight );
						}
					}
				}break;
			}

			IsValid = true;
		}
	}
}
