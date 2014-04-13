using System;
using Fuzzy_Logic_Library;

namespace Draughts
{

	/// <summary>
	/// This is a helper class in that it serves to keep the move decisions separate 
	/// from the main body of the code. 
	/// This could technically be implemented as part of the basic code
	/// though some functions such as the can take functions will be 
	/// relying on Draughts logic so leave it here for now.
	/// </summary>
	public class DraughtsProcessMoves
	{
		public DraughtsProcessMoves()
		{
			//
			// TODO: Add constructor logic here
			//
		}


		public bool IsEnemyAboveLeft( DraughtsBoard board, string identifier, string side )
		{
			DraughtsSquare square = ( DraughtsSquare )board.GetSquareAboveLeft( identifier );

			if( square == null )
				return false;

			if( square.IsOccupied == true 
				&& square.OccupyingName != side )
				return true;

			return false;
		}

		public bool IsFriendlyAboveLeft( DraughtsBoard board, string identifier, string side )
		{
			DraughtsSquare square = ( DraughtsSquare )board.GetSquareAboveLeft( identifier );

			if( square == null )
				return false;

			if( square.IsOccupied == true
				&& square.OccupyingName == side )
				return true;
		
			return false;
		}

		public bool IsEnemyKingAboveLeft( DraughtsBoard board, string identifier, string side )
		{
			DraughtsSquare square = ( DraughtsSquare )board.GetSquareAboveLeft( identifier );

			if( square == null )
				return false;

			if( square.IsOccupied == true 
				&& square.IsKing == true
				&& square.OccupyingName != side )
				return true;

			return false;
		}

		public bool IsFriendlyKingAboveLeft( DraughtsBoard board, string identifier, string side )
		{
			DraughtsSquare square = ( DraughtsSquare )board.GetSquareAboveLeft( identifier );

			if( square == null )
				return false;

			if( square.IsOccupied == true
				&& square.IsKing == true 
				&& square.OccupyingName == side )
				return true;

			return false;
		}

		public bool CanTakeAboveLeft( DraughtsBoard board, string identifier, string side )
		{
			DraughtsSquare square = ( DraughtsSquare )board.GetSquare( identifier );

			if( square == null )
				return false;

			if( IsEnemyAboveLeft( board, identifier, side ) == true )
			{
				DraughtsSquare tempSquare = ( DraughtsSquare )board.GetSquareAboveLeft( square.Identifier );

				if( tempSquare == null )
					return false;

				return IsAboveLeftClear( board, tempSquare.Identifier );
			}

			return false;
		}

		public bool IsEnemyAboveRight( DraughtsBoard board, string identifier, string side )
		{
			DraughtsSquare square = ( DraughtsSquare )board.GetSquareAboveRight( identifier );

			if( square == null )
				return false;

			if( square.IsOccupied == true 
				&& square.OccupyingName != side )
				return true;

			return false;
		}

		public bool IsFriendlyAboveRight( DraughtsBoard board, string identifier, string side )
		{
			DraughtsSquare square = ( DraughtsSquare )board.GetSquareAboveRight( identifier );

			if( square == null )
				return false;

			if( square.IsOccupied == true 
				&& square.OccupyingName == side )
				return true;

			return false;
		}

		public bool IsEnemyKingAboveRight( DraughtsBoard board, string identifier, string side )
		{
			DraughtsSquare square = ( DraughtsSquare )board.GetSquareAboveRight( identifier );

			if( square == null )
				return false;

			if( square.IsOccupied == true
				&& square.IsKing == true
				&& square.OccupyingName != side )
				return true;

			return false;
		}

		public bool IsFriendlyKingAboveRight( DraughtsBoard board, string identifier, string side )
		{
			DraughtsSquare square = ( DraughtsSquare )board.GetSquareAboveRight( identifier );

			if( square == null )
				return false;

			if( square.IsOccupied == true
				&& square.IsKing == true
				&& square.OccupyingName == side )
				return true;

			return false;
		}

		public bool CanTakeAboveRight( DraughtsBoard board, string identifier, string side )
		{
			DraughtsSquare square = ( DraughtsSquare )board.GetSquare( identifier );

			if( square == null )
				return false;

			if( IsEnemyAboveRight( board, identifier, side ) == true )
			{
				DraughtsSquare tempSquare = ( DraughtsSquare )board.GetSquareAboveRight( square.Identifier );

				if( tempSquare == null )
					return false;

				return IsAboveRightClear( board, tempSquare.Identifier );
			}

			return false;
		}

		public bool IsEnemyBelowLeft( DraughtsBoard board, string identifier, string side )
		{
			DraughtsSquare square = ( DraughtsSquare )board.GetSquareBelowLeft( identifier );

			if( square == null )
				return false;

			if( square.IsOccupied == true 
				&& square.OccupyingName != side )
				return true;

			return false;
		}

		public bool IsFriendlyBelowLeft( DraughtsBoard board, string identifier, string side )
		{
			DraughtsSquare square = ( DraughtsSquare )board.GetSquareBelowLeft( identifier );

			if( square == null )
				return false;

			if( square.IsOccupied == true 
				&& square.OccupyingName == side )
				return true;

			return false;
		}

		public bool IsEnemyKingBelowLeft( DraughtsBoard board, string identifier, string side )
		{
			DraughtsSquare square = ( DraughtsSquare )board.GetSquareBelowLeft( identifier );

			if( square == null )
				return false;

			if( square.IsOccupied == true
				&& square.IsKing == true
				&& square.OccupyingName != side )
				return true;

			return false;
		}

		public bool IsFriendlyKingBelowLeft( DraughtsBoard board, string identifier, string side )
		{
			DraughtsSquare square = ( DraughtsSquare )board.GetSquare( identifier );

			if( square != null )
				return false;

			DraughtsSquare tempSquare = ( DraughtsSquare )board.GetSquareBelowLeft( square.Identifier );

			if( tempSquare == null )
				return false;

			if( tempSquare.IsOccupied == true
				&& tempSquare.IsKing == true
				&& tempSquare.OccupyingName == side )
				return true;
		
			return false;
		}

		public bool CanTakeBelowLeft( DraughtsBoard board, string identifier, string side )
		{
			DraughtsSquare square = ( DraughtsSquare )board.GetSquare( identifier );

			if( square == null )
				return false;

			if( IsEnemyBelowLeft( board, identifier, side ) == true )
			{
				DraughtsSquare tempSquare = ( DraughtsSquare )board.GetSquareBelowLeft( square.Identifier );

				if( tempSquare == null )
					return false;

				return IsBelowLeftClear( board, tempSquare.Identifier );
			}

			return false;		
		}

		public bool IsEnemyBelowRight( DraughtsBoard board, string identifier, string side )
		{
			DraughtsSquare square = ( DraughtsSquare )board.GetSquareBelowRight( identifier );
 
			if( square == null )
				return false;

			if( square.IsOccupied == true
				&& square.OccupyingName != side )
				return true;

			return false;
		}

		public bool IsFriendlyBelowRight( DraughtsBoard board, string identifier, string side )
		{
			DraughtsSquare square = ( DraughtsSquare )board.GetSquareBelowRight( identifier );

			if( square == null )
				return false;

			if( square.IsOccupied == true
				&& square.OccupyingName == side )
				return true;

			return false;
		}

		public bool IsEnemyKingBelowRight( DraughtsBoard board, string identifier, string side )
		{
			DraughtsSquare square = ( DraughtsSquare )board.GetSquareBelowRight( identifier );

			if( square == null )
				return false;

			if( square.IsOccupied == true 
				&& square.IsKing == true
				&& square.OccupyingName != side )
				return true;

			return false;
		}

		public bool IsFriendlyKingBelowRight( ref DraughtsBoard board, string identifier, string side )
		{
			DraughtsSquare square = ( DraughtsSquare )board.GetSquareBelowRight( identifier );

			if( square == null )
				return false;

			if( square.IsOccupied == true 
				&& square.IsKing == true 
				&& square.OccupyingName == side )
				return true;

			return false;
		}

		public bool CanTakeBelowRight( DraughtsBoard board, string identifier, string side )
		{
			DraughtsSquare square = ( DraughtsSquare )board.GetSquare( identifier );

			if( square == null )
				return false;

			if( IsEnemyBelowRight( board, identifier, side ) == true )
			{
				DraughtsSquare tempSquare = ( DraughtsSquare )board.GetSquareBelowRight( square.Identifier );

				if( tempSquare == null )
					return false;

				return IsBelowRightClear( board, tempSquare.Identifier );
			}

			return false;
		}

		public bool IsAboveLeftClear( DraughtsBoard board, string identifier )
		{
			DraughtsSquare square = ( DraughtsSquare )board.GetSquareAboveLeft( identifier );

			if( square != null && square.IsOccupied == false )
				return true;

			return false;
		}

		public bool IsAboveRightClear( DraughtsBoard board, string identifier )
		{
			DraughtsSquare square = ( DraughtsSquare )board.GetSquareAboveRight( identifier );

			if( square != null && square.IsOccupied == false )
				return true;
		
			return false;
		}

		public bool IsBelowLeftClear( DraughtsBoard board, string identifier )
		{
			DraughtsSquare square = ( DraughtsSquare )board.GetSquareBelowLeft( identifier );

			if( square != null && square.IsOccupied == false )
				return true;

			return false;
		}

		public bool IsBelowRightClear( DraughtsBoard board, string identifier )
		{
			DraughtsSquare square = ( DraughtsSquare )board.GetSquareBelowRight( identifier );

			if( square != null && square.IsOccupied == false )
				return true;

			return false;
		}

		public bool IsEnemyGenerallyAboveLeft( DraughtsBoard board, string identifier, string side )
		{
			DraughtsSquare square = ( DraughtsSquare )board.GetSquareAboveLeft( identifier );

			if( square == null )
				return false;

			do
			{
				if( IsEnemyAboveLeft( board, square.Identifier, side ) == true )
					return true;
				if( IsEnemyAboveRight( board, square.Identifier, side ) == true )
					return true;
				if( IsEnemyBelowRight( board, square.Identifier, side ) == true )
					return true;
				if( IsEnemyBelowLeft( board, square.Identifier, side ) == true )
					return true;

				square = ( DraughtsSquare )board.GetSquareAboveLeft( square.Identifier );
			}
			while( square != null );

			return false;
		}

		public bool IsEnemyGenerallyAboveRight( DraughtsBoard board, string identifier, string side )
		{
			DraughtsSquare square = ( DraughtsSquare )board.GetSquareAboveRight( identifier );

			if( square == null )
				return false;

			do
			{
				if( IsEnemyAboveLeft( board, square.Identifier, side ) == true )
					return true;
				if( IsEnemyAboveRight( board, square.Identifier, side ) == true )
					return true;
				if( IsEnemyBelowRight( board, square.Identifier, side ) == true )
					return true;
				if( IsEnemyBelowLeft( board, square.Identifier, side ) == true )
					return true;

				square = ( DraughtsSquare )board.GetSquareAboveRight( square.Identifier );
			}
			while( square != null );

			return false;
		}

		public bool IsEnemyGenerallyBelowLeft( DraughtsBoard board, string identifier, string side )
		{
			DraughtsSquare square = ( DraughtsSquare )board.GetSquareBelowLeft( identifier );

			if( square == null )
				return false;

			do
			{
				if( IsEnemyAboveLeft( board, square.Identifier, side ) == true )
					return true;
				if( IsEnemyAboveRight( board, square.Identifier, side ) == true )
					return true;
				if( IsEnemyBelowLeft( board, square.Identifier, side ) == true )
					return true;
				if( IsEnemyBelowRight( board, square.Identifier, side ) == true )
					return true;

				square = ( DraughtsSquare )board.GetSquareBelowLeft( square.Identifier );

			}
			while( square != null );
			
			return false;
		}

		public bool IsEnemyGenerallyBelowRight( DraughtsBoard board, string identifier, string side )
		{
			DraughtsSquare square = ( DraughtsSquare )board.GetSquareBelowRight( identifier );

			if( square == null )
				return false;

			do
			{
				if( IsEnemyAboveLeft( board, square.Identifier, side ) == true )
					return true;
				if( IsEnemyAboveRight( board, square.Identifier, side ) == true )
					return true;
				if( IsEnemyBelowLeft( board, square.Identifier, side ) == true )
					return true;
				if( IsEnemyBelowRight( board, square.Identifier, side ) == true )
					return true;

				square = ( DraughtsSquare )board.GetSquareBelowRight( square.Identifier );
			}
			while( square != null );

			return false;
		}


	}
}
