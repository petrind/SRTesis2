using System;
using Fuzzy_Logic_Library;

namespace Draughts
{
	/// <summary>
	/// Quick class to hold the validate function that doesn't belong in the Fuzzy Logic Library
	/// </summary>
	public class DraughtsDecisionSet : FuzzyDecisionSetCollection
	{
		public DraughtsDecisionSet()
		{
			//
			// TODO: Add constructor logic here
			//
		}


		public bool Validate()
		{
			for( int i=0; i<Count; i++ )
			{
				if( ValidateDecision( ( FuzzyDecisionSet )this[ i ] ) == false )
				{
					this.RemoveAt( i );
					i=0;
				}
			}

			return true;
		}

		public bool ValidateDecision( FuzzyDecisionSet decision )
		{
			if( decision.GetValidByName( "MoveBelowLeft" ) == false 
				&& decision.GetValidByName( "MoveBelowRight" ) == false
				&& decision.GetValidByName( "MoveAboveLeft" ) == false 
				&& decision.GetValidByName( "MoveAboveRight" ) == false )
				return false;

			return true;
		}
	}
}
