using System;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Collections;
using System.Text;


namespace Fuzzy_Logic_Library
{

	/// <summary>
	/// Fuzzy number class deals with fuzzy numbers
	/// </summary>
	public class FuzzyNumber
	{
		/// <summary>
		///  Does Fuzzy Number need an absoluteMaximum and an AbsoluteMinimum value
		///  that it cannot pass?
		/// </summary>
		private double dNumber;
		private double dMaximum;
		private double dMinimum;

		/// <summary>
		/// membership value goes between 0 and 2
		/// a value less than 1 is in the lower half of the membership and a value
		/// greater than 1 is in the higher part of the membership
		/// </summary>
		private double dMembership;

		/// <summary>
		/// arbritrary members
		/// </summary>
		private double dID;
		private string strName;

		public double Number
		{
			get
			{
				return dNumber;
			}
			set
			{
				dNumber = value;
				SetMembership();
			}
		}

		public double Maximum
		{
			get
			{
				return dMaximum;
			}
			set
			{
				dMaximum = value;
			}
		}

		/// <summary>
		/// is the value a full member of this set note it can be set by the outside
		/// this is because the code may not always treat the central point between the 
		/// maximum and minimum numbers as the location for full membership
		/// </summary>
		public double Membership
		{
			get
			{
				return dMembership;
			}
			set
			{
				dMembership = value;
			}
		}


		public double Minimum
		{
			get
			{
				return dMinimum;
			}
			set
			{
				dMinimum = value;
			}
		}

		public double ID
		{
			get
			{
				return dID;
			}
			set
			{
				dID = value;
			}
		}

		public string Name
		{
			get
			{
				return strName;
			}
			set
			{
				strName = value;
			}
		}


		public FuzzyNumber()
		{
			//
			// TODO: Add constructor logic here
			//

			Minimum = 0;
			Maximum = 0;
			Number = 0;
			dMembership = 0.0;
			ID = 0;
			Name = "";
		}

		public FuzzyNumber( FuzzyNumber fuzzy )
		{
			this.ID = fuzzy.ID;
			this.Maximum = fuzzy.Maximum;
			this.Membership = fuzzy.Membership;
			this.Minimum = fuzzy.Minimum;
			this.Name = fuzzy.Name;
			this.Number = fuzzy.Number;
		}

		public FuzzyNumber( double number )
		{
			Minimum = 0;
			Maximum = 0;
			Number = number;
			dMembership = 0.0;
			ID = 0;
			Name = "";
		}

		public FuzzyNumber( double rangeLow, double rangeHigh )
		{
			Minimum = rangeLow;
			Maximum = rangeHigh;
			Number = rangeLow + ( ( rangeHigh - rangeLow ) /2 );
			dMembership = 1.0;
			ID = 0;
			Name = "";
		}

		public FuzzyNumber( double number, double rangeLow, double rangeHigh )
		{
			Minimum = rangeLow;
			Maximum = rangeHigh;
			Number = number;
			dMembership = 0.0;
			ID = 0;
			Name = "";
			SetMembership();
		}

		public FuzzyNumber( string name, double number, double rangeLow, double rangeHigh )
		{
			Minimum = rangeLow;
			Maximum = rangeHigh;
			Number = number;
			SetMembership();
			ID = 0;
			Name = name;
		}

		public FuzzyNumber( string name, double rangeLow, double rangeHigh )
		{
			Minimum = rangeLow;
			Maximum = rangeHigh;
			Number = rangeLow + ( ( rangeHigh - rangeLow ) / 2 );
			dMembership = 1.0;
			ID = 0;
			Name = name;
		}

		public FuzzyNumber( string name, double number, double rangeLow, double rangeHigh, bool bUseMaxAsComplete )
		{
			Minimum = rangeLow;
			if( bUseMaxAsComplete == true )
				Maximum = rangeHigh + rangeHigh;
			else
				Maximum = rangeHigh;

			Number = number;
			dMembership = 0.0;
			ID = 0;
			Name = "";
			SetMembership();
		}

		/// <summary>
		/// Set the membership value for the number
		/// </summary>
		public void SetMembership()
		{
			if( Maximum == 0 || Maximum < Minimum )
				return;

			if( Number > Maximum || Number <= Minimum )
			{
				dMembership = 0.0;
				return;
			}

			double dMiddle = Minimum + ( ( Maximum - Minimum ) / 2 );
			
			if( Number == dMiddle )
			{
				dMembership = 1.0;
			} 
			else if( Number < dMiddle )
			{
				double dHalfMiddle = dMiddle - ( dMiddle / 2 );
				if( Number == dHalfMiddle )
				{
					dMembership = 0.50;
				}
				else if( Number > dHalfMiddle )
				{
					double dHalfAgain = dHalfMiddle + ( dHalfMiddle / 2 );
					if( Number == dHalfAgain )
					{
						dMembership = 0.75;
					}
					else if( Number > dHalfAgain )
					{
						dMembership = 0.87;
					}
					else
						dMembership = 0.63;

				}
				else
				{
					double dHalfAgain = dHalfMiddle - ( dHalfMiddle / 2 );
					if( Number == dHalfAgain )
					{
						dMembership = 0.25;
					}
					else if( Number > dHalfAgain )
					{
						dMembership = 0.37;
					}
					else
						dMembership = 0.12;
				}

			}
			else
			{
				double dHalfMiddle = dMiddle + ( dMiddle / 2 );
				if( Number == dHalfMiddle )
				{
					dMembership = 1.50;
				}
				else if( Number > dHalfMiddle )
				{
					double dHalfAgain = dHalfMiddle + ( ( Maximum - dHalfMiddle ) / 2 );
					if( Number == dHalfAgain )
					{
						dMembership = 1.75;
					}
					else if( Number > dHalfAgain )
					{
						dMembership = 1.87;
					}
					else
						dMembership = 1.63;

				}
				else
				{
					double dHalfAgain = dHalfMiddle - ( ( dHalfMiddle - dMiddle ) / 2 );
					if( Number == dHalfAgain )
					{
						dMembership = 1.25;
					}
					else if( Number > dHalfAgain )
					{
						dMembership = 1.37;
					}
					else
						dMembership = 1.12;
				}
			}

		}


		public override string ToString()
		{
			return base.ToString() + ", Number = " + Number.ToString() + ", Minimum = " + Minimum.ToString() + ", Maximum = " + Maximum.ToString() + ", Membership = " + dMembership.ToString();
		}

		/// <summary>
		/// Check if the set values are equal name and id not checked as 
		/// these are arbitrary
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public bool IsEqual( FuzzyNumber number )
		{
			if( number.Maximum == this.Maximum &&
				number.Membership == this.Membership &&
				number.Minimum == this.Minimum && 
				number.Number == this.Number )
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Overloaded operators Note no attempt is made to check for minimum and maximum
		/// values this is up to the code using the class 
		/// </summary>

		public static FuzzyNumber operator +( FuzzyNumber num, double number )
		{
			FuzzyNumber fuzzTemp = new FuzzyNumber( num.Number + number, num.Minimum, num.Maximum );
			fuzzTemp.SetMembership();
			return fuzzTemp;
		}

		public static FuzzyNumber operator +( FuzzyNumber num, FuzzyNumber number )
		{
			FuzzyNumber fuzzTemp = new FuzzyNumber( num.Number + number.Number, num.Minimum, num.Maximum );
			fuzzTemp.SetMembership();
			return fuzzTemp;
		}

		public static FuzzyNumber operator -( FuzzyNumber num, double number )
		{
			FuzzyNumber fuzzTemp = new FuzzyNumber( num.Number - number, num.Minimum, num.Maximum );
			fuzzTemp.SetMembership();
			return fuzzTemp;
		}

		public static FuzzyNumber operator -( FuzzyNumber num, FuzzyNumber number )
		{
			FuzzyNumber fuzzTemp = new FuzzyNumber( num.Number - number.Number, num.Minimum, num.Maximum );
			fuzzTemp.SetMembership();
			return fuzzTemp;
		}

		public static FuzzyNumber operator *( FuzzyNumber num, double number )
		{
			FuzzyNumber fuzzTemp;
			if( num.Number == 0.0 || number == 0.0 )
			{
				fuzzTemp = new FuzzyNumber( num.Number, num.Minimum, num.Maximum );
				fuzzTemp.SetMembership();
				return fuzzTemp;
			}

			fuzzTemp = new FuzzyNumber( num.Number * number, num.Minimum, num.Maximum );
			fuzzTemp.SetMembership();
			return fuzzTemp;
		}

		public static FuzzyNumber operator *( FuzzyNumber num, FuzzyNumber number )
		{
			FuzzyNumber fuzzTemp;
			if( num.Number == 0.0 || number.Number == 0.0 )
			{
				fuzzTemp = new FuzzyNumber( num.Number, num.Minimum, num.Maximum );
				fuzzTemp.SetMembership();
				return fuzzTemp;
			}

			fuzzTemp = new FuzzyNumber( num.Number * number.Number, num.Minimum, num.Maximum );
			fuzzTemp.SetMembership();
			return fuzzTemp;
		}

		public static FuzzyNumber operator /( FuzzyNumber num, double number )
		{
			FuzzyNumber fuzzTemp;
			if( num.Number == 0.0 || number == 0.0 )
			{
				fuzzTemp = new FuzzyNumber( num.Number, num.Minimum, num.Maximum );
				fuzzTemp.SetMembership();
				return fuzzTemp;
			}

			fuzzTemp = new FuzzyNumber( num.Number / number, num.Minimum, num.Maximum );
			fuzzTemp.SetMembership();
			return fuzzTemp;
		}

		public static FuzzyNumber operator /( FuzzyNumber num, FuzzyNumber number )
		{
			FuzzyNumber fuzzTemp;
			if( num.Number == 0.0 || number.Number == 0.0 )
			{
				fuzzTemp = new FuzzyNumber( num.Number, num.Minimum, num.Maximum );
				fuzzTemp.SetMembership();
				return fuzzTemp;
			}

			fuzzTemp = new FuzzyNumber( num.Number / number.Number, num.Minimum, num.Maximum );
			fuzzTemp.SetMembership();
			return fuzzTemp;
		}

		public static FuzzyNumber operator %( FuzzyNumber num, double number )
		{
			FuzzyNumber fuzzTemp;
			if( num.Number == 0.0 || number == 0.0 )
			{
				fuzzTemp = new FuzzyNumber( num.Number, num.Minimum, num.Maximum );
				fuzzTemp.SetMembership();
				return fuzzTemp;
			}

			fuzzTemp = new FuzzyNumber( num.Number % number, num.Minimum, num.Maximum );
			fuzzTemp.SetMembership();
			return fuzzTemp;
		}

		public static FuzzyNumber operator %( FuzzyNumber num, FuzzyNumber number )
		{
			FuzzyNumber fuzzTemp;
			if( num.Number == 0.0 || number.Number == 0.0 )
			{
				fuzzTemp = new FuzzyNumber( num.Number, num.Minimum, num.Maximum );
				fuzzTemp.SetMembership();
				return fuzzTemp;
			}

			fuzzTemp = new FuzzyNumber( num.Number % number.Number, num.Minimum, num.Maximum );
			fuzzTemp.SetMembership();
			return fuzzTemp;
		}

	}

	/// <summary>
	/// Fuzzy SetParameters class is an aid class to help in setting up variables 
	/// for set operations between different types of sets
	/// </summary>
	public class FuzzySetParameters
	{
		private double dSetOneMinMinimum;
		private double dSetOneMaxMinimum;
		private double dSetOneMinMembership; 
		private double dSetOneMaxMembership;
		private double dSetOneMinMaximum;
		private double dSetOneMaxMaximum;
		private double dSetOneMinNumber;
		private double dSetOneMaxNumber;
		private double dSetTwoMinMinimum;
		private double dSetTwoMaxMinimum;
		private double dSetTwoMinMembership;
		private double dSetTwoMaxMembership;
		private double dSetTwoMinMaximum;
		private double dSetTwoMaxMaximum;
		private double dSetTwoMinNumber;
		private double dSetTwoMaxNumber;

		public FuzzySetParameters()
		{
			dSetOneMinMinimum = 0;
			dSetOneMaxMinimum = 0;
			dSetOneMinMembership = 0; 
			dSetOneMaxMembership = 0;
			dSetOneMinMaximum = 0;
			dSetOneMaxMaximum = 0;
			dSetOneMinNumber = 0;
			dSetOneMaxNumber = 0;
			dSetTwoMinMinimum = 0;
			dSetTwoMaxMinimum = 0;
			dSetTwoMinMembership = 0;
			dSetTwoMaxMembership = 0;
			dSetTwoMinMaximum = 0;
			dSetTwoMaxMaximum = 0;
			dSetTwoMinNumber = 0;
			dSetTwoMaxNumber = 0;
		}


		public double SetOneMinMinimum
		{
			get
			{
				return dSetOneMinMinimum;
			}
			set
			{
				dSetOneMinMinimum = value;
			}
		}

		public double SetOneMaxMinimum
		{
			get
			{
				return dSetOneMaxMinimum;
			}
			set
			{
				dSetOneMaxMinimum = value;
			}
		}

		public double SetOneMinMembership
		{
			get
			{
				return dSetOneMinMembership;
			}
			set
			{
				dSetOneMinMembership = value;
			}
		}

		public double SetOneMaxMembership
		{
			get
			{
				return dSetOneMaxMembership;
			}
			set
			{
				dSetOneMaxMembership = value;
			}
		}

		public double SetOneMinMaximum
		{
			get
			{
				return dSetOneMinMaximum;
			}
			set
			{
				dSetOneMinMaximum = value;
			}
		}

		public double SetOneMaxMaximum
		{
			get
			{
				return dSetOneMaxMaximum;
			}
			set
			{
				dSetOneMaxMaximum = value;
			}
		}

		public double SetOneMinNumber 
		{
			get
			{
				return dSetOneMinNumber;
			}
			set
			{
				dSetOneMinNumber = value;
			}
		}

		public double SetOneMaxNumber
		{
			get
			{
				return dSetOneMaxNumber;
			}
			set
			{
				dSetOneMaxNumber = value;
			}
		}

		public double SetTwoMinMinimum
		{
			get
			{
				return dSetTwoMinMinimum;
			}
			set
			{
				dSetTwoMinMinimum = value;
			}
		}

		public double SetTwoMaxMinimum
		{
			get
			{
				return dSetTwoMaxMinimum;
			}
			set
			{
				dSetTwoMaxMinimum = value;
			}
		}

		public double SetTwoMinMembership
		{
			get
			{
				return dSetTwoMinMembership;
			}
			set
			{
				dSetTwoMinMembership = value;
			}
		}

		public double SetTwoMaxMembership
		{
			get
			{
				return dSetTwoMaxMembership;
			}
			set
			{
				dSetTwoMaxMembership = value;
			}
		}

		public double SetTwoMinMaximum
		{
			get
			{
				return dSetTwoMinMaximum;
			}
			set
			{
				dSetTwoMinMaximum = value;
			}
		}

		public double SetTwoMaxMaximum
		{
			get
			{
				return dSetTwoMaxMaximum;
			}
			set
			{
				dSetTwoMaxMaximum = value;
			}
		}

		public double SetTwoMinNumber 
		{
			get
			{
				return dSetTwoMinNumber;
			}
			set
			{
				dSetTwoMinNumber = value;
			}
		}

		public double SetTwoMaxNumber
		{
			get
			{
				return dSetTwoMaxNumber;
			}
			set
			{
				dSetTwoMaxNumber = value;
			}
		}



	}

	/// <summary>
	/// The Fuzzy Set class holds the basic Fuzzy Set stuff
	/// </summary>
	abstract public class FuzzySet
	{
		private ArrayList arrayFuzzy;

		private string strName;

		public string Name
		{
			get
			{
				return strName;
			}
			set
			{
				strName = value;
			}
		}

		/// <summary>
		/// basic constructor
		/// </summary>
		public FuzzySet()
		{
			arrayFuzzy = new ArrayList();
		}


		/// <summary>
		/// allow direct access to the ArrayList
		/// </summary>
		public ArrayList FuzzyArray
		{
			get
			{
				return arrayFuzzy;
			}
		}

		public int Count
		{
			get
			{
				return arrayFuzzy.Count;
			}
		}

		/// <summary>
		/// is there a fuzzy number that has the membership value of 1
		/// </summary>
		/// <returns></returns>
		public bool IsCompleteMembership()
		{
			for( int i=0; i<arrayFuzzy.Count; i++ )
			{
				if( ( ( FuzzyNumber )arrayFuzzy[ i ] ).Membership == 1.0 )
					return true;
			}

			return false;
		}

		/// <summary>
		/// get the value that has complete membership
		/// </summary>
		/// <returns></returns>
		public FuzzyNumber GetCompleteMembership()
		{
			for( int i=0; i<arrayFuzzy.Count; i++ )
			{
				if( ( ( FuzzyNumber )arrayFuzzy[ i ] ).Membership == 1.0 )
					return ( FuzzyNumber )arrayFuzzy[ i ];
			}

			return null;
		}

		public bool IsInSet( FuzzyNumber number )
		{
			for( int i=0; i<this.Count; i++ )
			{
				if( ( ( FuzzyNumber )this.FuzzyArray[ i ] ).IsEqual( number ) == true )
					return true;
			}

			return false;
		}

		/// <summary>
		/// Get the value of the best member in the fuzzy set
		/// Note returns lowest value ie less than one
		/// </summary>
		/// <returns></returns>
		public FuzzyNumber GetBestMembershipValue()
		{
			int nBestMember = 0;
			double dBestValue = 0.0;
			double dTempValue = 0.0;

			for( int i=0; i<arrayFuzzy.Count; i++ )
			{
				dTempValue = ( ( FuzzyNumber )arrayFuzzy[ i ] ).Membership;
				if( dTempValue > dBestValue && dTempValue < 1.0 )
				{
					dBestValue = dTempValue;
					nBestMember = i;
				}
			}

			return ( FuzzyNumber )arrayFuzzy[ nBestMember ];
		}

		/// <summary>
		/// returns the best membership value greater than 1
		/// </summary>
		/// <returns></returns>
		public FuzzyNumber GetHighestBestMembershipValue()
		{
			int nBestMember = 0;
			double dBestValue = 0.0;
			double dTempValue = 0.0;

			for( int i=0; i<arrayFuzzy.Count; i++ )
			{
				dTempValue = ( ( FuzzyNumber )arrayFuzzy[ i ] ).Membership;
				if( dTempValue < dBestValue && dTempValue > 1.0 )
				{
					dBestValue = dTempValue;
					nBestMember = i;
				}
			}

			return ( FuzzyNumber )arrayFuzzy[ nBestMember ];
		}

		/// <summary>
		/// use an indexer for the class
		/// </summary>
		public FuzzyNumber this[ int index ]
		{
			get
			{
				if( index <= arrayFuzzy.Count )
					return ( FuzzyNumber )arrayFuzzy[ index ];
				else
					return null;
			}
			set
			{
				if( index < arrayFuzzy.Count )
				{
					arrayFuzzy.RemoveAt( index );
					arrayFuzzy.Insert( index, value );
				}
				else
					arrayFuzzy.Add( value );
			}
		}


		/// Set operations
		
		/// <summary>
		/// return a union of the two passed sets
		/// </summary>
		/// <param name="setOne"></param>
		/// <param name="setTwo"></param>
		/// <returns></returns>
		public FuzzyNumberSet Union( FuzzySet fuzzySet )
		{
			FuzzyNumberSet returnSet = new FuzzyNumberSet();
			bool bFound = false;

			for( int i=0; i<this.FuzzyArray.Count; i++ )
			{
				returnSet[ returnSet.Count + 1 ] = new FuzzyNumber( ( FuzzyNumber )this.FuzzyArray[ i ] );
			}

			/// Get the ones in set one but not in set two
			for( int i=0; i<fuzzySet.Count; i++ )
			{
				bFound = false;

				for( int n=0; n<this.Count; n++ )
				{
					if( ( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Maximum == ( ( FuzzyNumber )this.FuzzyArray[ n ] ).Maximum &&
						( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Membership == ( ( FuzzyNumber )this.FuzzyArray[ n ] ).Membership &&
						( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Minimum == ( ( FuzzyNumber )this.FuzzyArray[ n ] ).Minimum &&
						( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Number == ( ( FuzzyNumber )this.FuzzyArray[ n ] ).Number )
					{
						bFound = true;
						n=fuzzySet.Count;
					}
				}

				if( bFound == false )
				{
					returnSet[ returnSet.Count + 1 ] = new FuzzyNumber( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] );					
				}
			}

			return returnSet;
		}


		/// <summary>
		/// Union override that allows the setting of the membership parameters
		/// </summary>
		/// <param name="setOne"></param>
		/// <param name="setTwo"></param>
		/// <param name="setParams"></param>
		/// <returns></returns>
		public FuzzyNumberSet Union( FuzzySet fuzzySet, FuzzySetParameters setParams )
		{
			FuzzyNumberSet returnSet = new FuzzyNumberSet();

			double dSetOneMinMinimum;
			double dSetOneMaxMinimum;
			double dSetOneMinMembership; 
			double dSetOneMaxMembership;
			double dSetOneMinMaximum;
			double dSetOneMaxMaximum;
			double dSetOneMinNumber;
			double dSetOneMaxNumber;
			double dSetTwoMinMinimum;
			double dSetTwoMaxMinimum;
			double dSetTwoMinMembership;
			double dSetTwoMaxMembership;
			double dSetTwoMinMaximum;
			double dSetTwoMaxMaximum;
			double dSetTwoMinNumber;
			double dSetTwoMaxNumber;


			/// get all the items from set one that fit the required ( passed in ) parameters.
			for( int i=0; i<this.Count; i++ )
			{
				if( setParams.SetOneMaxMembership == 0 )
					dSetOneMaxMembership = ( ( FuzzyNumber )this.FuzzyArray[ i ] ).Membership;
				else
					dSetOneMaxMembership = setParams.SetOneMaxMembership;

				if( setParams.SetOneMaxMinimum == 0 )
					dSetOneMaxMinimum = ( ( FuzzyNumber )this.FuzzyArray[ i ] ).Minimum;
				else
					dSetOneMaxMinimum = setParams.SetOneMaxMinimum;

				if( setParams.SetOneMaxNumber == 0 )
					dSetOneMaxNumber = ( ( FuzzyNumber )this.FuzzyArray[ i ] ).Number;
				else
					dSetOneMaxNumber = setParams.SetOneMaxNumber;

				if( setParams.SetOneMinMaximum == 0 )
					dSetOneMinMaximum = ( ( FuzzyNumber )this.FuzzyArray[ i ] ).Maximum;
				else
					dSetOneMinMaximum = setParams.SetOneMinMaximum;

				if( setParams.SetOneMaxMaximum == 0 )
					dSetOneMaxMaximum = ( ( FuzzyNumber )this.FuzzyArray[ i ] ).Maximum;
				else
					dSetOneMaxMaximum = setParams.SetOneMaxMaximum;

				if( setParams.SetOneMinMembership == 0 )
					dSetOneMinMembership = ( ( FuzzyNumber )this.FuzzyArray[ i ] ).Membership;
				else
					dSetOneMinMembership = setParams.SetOneMinMembership;

				if( setParams.SetOneMinMinimum == 0 )
					dSetOneMinMinimum = ( ( FuzzyNumber )this.FuzzyArray[ i ] ).Minimum;
				else
					dSetOneMinMinimum = setParams.SetOneMinMinimum;

				if( setParams.SetOneMinNumber == 0 )
					dSetOneMinNumber = ( ( FuzzyNumber )this.FuzzyArray[ i ] ).Number;
				else
					dSetOneMinNumber = setParams.SetOneMinNumber;

				if( ( ( FuzzyNumber )this.FuzzyArray[ i ] ).Membership <= dSetOneMaxMembership &&
					( ( FuzzyNumber )this.FuzzyArray[ i ] ).Maximum <= dSetOneMaxMaximum &&
					( ( FuzzyNumber )this.FuzzyArray[ i ] ).Minimum <= dSetOneMaxMinimum &&
					( ( FuzzyNumber )this.FuzzyArray[ i ] ).Number  <= dSetOneMaxNumber && 
					( ( FuzzyNumber )this.FuzzyArray[ i ] ).Membership >= dSetOneMinMembership &&
					( ( FuzzyNumber )this.FuzzyArray[ i ] ).Maximum >= dSetOneMinMaximum &&
					( ( FuzzyNumber )this.FuzzyArray[ i ] ).Minimum >= dSetOneMinMinimum && 
					( ( FuzzyNumber )this.FuzzyArray[ i ] ).Number >= dSetOneMinNumber )
				{
					returnSet[ returnSet.Count + 1 ] = new FuzzyNumber( ( FuzzyNumber )this.FuzzyArray[ i ] );
				}
			}

			/// Get the ones in set two, not in set one ( though none should be when comparing different sets )
			for( int i=0; i<fuzzySet.Count; i++ )
			{
				if( setParams.SetTwoMaxMembership == 0 )
					dSetTwoMaxMembership = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Membership;
				else
					dSetTwoMaxMembership = setParams.SetTwoMaxMembership;

				if( setParams.SetTwoMaxMinimum == 0 )
					dSetTwoMaxMinimum = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Minimum;
				else
					dSetTwoMaxMinimum = setParams.SetTwoMaxMinimum;

				if( setParams.SetTwoMaxNumber == 0 )
					dSetTwoMaxNumber = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Number;
				else
					dSetTwoMaxNumber = setParams.SetTwoMaxNumber;

				if( setParams.SetTwoMinMaximum == 0 )
					dSetTwoMinMaximum = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Maximum;
				else
					dSetTwoMinMaximum = setParams.SetTwoMinMaximum;

				if( setParams.SetTwoMaxMaximum == 0 )
					dSetTwoMaxMaximum = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Maximum;
				else
					dSetTwoMaxMaximum = setParams.SetTwoMaxMaximum;

				if( setParams.SetTwoMinMembership == 0 )
					dSetTwoMinMembership = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Membership;
				else
					dSetTwoMinMembership = setParams.SetTwoMinMembership;

				if( setParams.SetTwoMinMinimum == 0 ) 
					dSetTwoMinMinimum = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Minimum;
				else
					dSetTwoMinMinimum = setParams.SetTwoMinMinimum;

				if( setParams.SetTwoMinNumber == 0 )
					dSetTwoMinNumber = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Number;
				else
					dSetTwoMinNumber = setParams.SetTwoMinNumber;

				for( int n=0; n<fuzzySet.Count; n++ )
				{

					if( setParams.SetOneMaxMembership == 0 )
						dSetOneMaxMembership = ( ( FuzzyNumber )this.FuzzyArray[ n ] ).Membership;
					else
						dSetOneMaxMembership = setParams.SetOneMaxMembership;

					if( setParams.SetOneMaxMinimum == 0 )
						dSetOneMaxMinimum = ( ( FuzzyNumber )this.FuzzyArray[ n ] ).Minimum;
					else
						dSetOneMaxMinimum = setParams.SetOneMaxMinimum;

					if( setParams.SetOneMaxNumber == 0 )
						dSetOneMaxNumber = ( ( FuzzyNumber )this.FuzzyArray[ n ] ).Number;
					else
						dSetOneMaxNumber = setParams.SetOneMaxNumber;

					if( setParams.SetOneMinMaximum == 0 )
						dSetOneMinMaximum = ( ( FuzzyNumber )this.FuzzyArray[ n ] ).Maximum;
					else
						dSetOneMinMaximum = setParams.SetOneMinMaximum;

					if( setParams.SetOneMaxMaximum == 0 )
						dSetOneMaxMaximum = ( ( FuzzyNumber )this.FuzzyArray[ n ] ).Maximum;
					else
						dSetOneMaxMaximum = setParams.SetOneMaxMaximum;

					if( setParams.SetOneMinMembership == 0 )
						dSetOneMinMembership = ( ( FuzzyNumber )this.FuzzyArray[ n ] ).Membership;
					else
						dSetOneMinMembership = setParams.SetOneMinMembership;

					if( setParams.SetOneMinMinimum == 0 )
						dSetOneMinMinimum = ( ( FuzzyNumber )this.FuzzyArray[ n ] ).Minimum;
					else
						dSetOneMinMinimum = setParams.SetOneMinMinimum;

					if( setParams.SetOneMinNumber == 0 )
						dSetOneMinNumber = ( ( FuzzyNumber )this.FuzzyArray[ n ] ).Number;
					else
						dSetOneMinNumber = setParams.SetOneMinNumber;


					if( ( ( FuzzyNumber )this.FuzzyArray[ n ] ).Maximum >= dSetOneMinMaximum &&
						( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Maximum >= dSetTwoMinMaximum &&
						( ( FuzzyNumber )this.FuzzyArray[ n ] ).Maximum <= dSetOneMaxMaximum &&
						( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Maximum <= dSetTwoMaxMaximum &&
						( ( FuzzyNumber )this.FuzzyArray[ n ] ).Membership >= dSetOneMinMembership &&
						( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Membership >= dSetTwoMinMembership &&
						( ( FuzzyNumber )this.FuzzyArray[ n ] ).Membership <= dSetOneMaxMembership &&
						( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Membership <= dSetTwoMaxMembership &&
						( ( FuzzyNumber )this.FuzzyArray[ n ] ).Minimum >= dSetOneMinMinimum &&
						( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Minimum >= dSetTwoMinMinimum &&
						( ( FuzzyNumber )this.FuzzyArray[ n ] ).Minimum <= dSetOneMaxMinimum &&
						( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Minimum <= dSetTwoMaxMinimum &&
						( ( FuzzyNumber )this.FuzzyArray[ n ] ).Number >= dSetOneMinNumber &&
						( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Number >= dSetTwoMinNumber &&
						( ( FuzzyNumber )this.FuzzyArray[ n ] ).Number <= dSetOneMaxNumber &&
						( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Number <= dSetTwoMaxNumber )
					{
						if( returnSet.IsInSet( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ]  ) == false )
						{
							returnSet[ returnSet.Count + 1 ] = new FuzzyNumber( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] );
						}
					}
				}
			}

			return returnSet;

		}


		/// <summary>
		/// return an intersection of the two sets
		/// </summary>
		/// <param name="setOne"></param>
		/// <param name="?"></param>
		/// <returns></returns>
		public FuzzyNumberSet Intersection( FuzzySet fuzzySet )
		{
			FuzzyNumberSet returnSet = new FuzzyNumberSet();

			for( int i=0; i<this.Count; i++ )
			{
				for( int n=0; n<fuzzySet.Count; n++ )
				{
					/// can't decide if name and id should be in here or not
					/// leave them out for now as they are meant to be optional
					if( ( ( FuzzyNumber )this.FuzzyArray[ i ] ).Maximum == ( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Maximum &&
						( ( FuzzyNumber )this.FuzzyArray[ i ] ).Membership == ( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Membership &&
						( ( FuzzyNumber )this.FuzzyArray[ i ] ).Minimum == ( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Minimum && 
						( ( FuzzyNumber )this.FuzzyArray[ i ] ).Number == ( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Number )
					{
						returnSet[ returnSet.Count + 1 ] = new FuzzyNumber( ( FuzzyNumber )this.FuzzyArray[ i ] );
					}
				}
			}

			return returnSet;
		}


		/// <summary>
		/// return an intersection of the two sets and allow the passing in of the required membership values
		/// </summary>
		/// <param name="setOne"></param>
		/// <param name="setTwo"></param>
		/// <param name="setOneMinMembership"></param>
		/// <param name="setOneMaxMembership"></param>
		/// <param name="setTwoMinMembership"></param>
		/// <param name="setTwoMaxMembership"></param>
		/// <returns></returns>
		public FuzzyNumberSet Intersection( FuzzySet fuzzySet, FuzzySetParameters setParams )
		{
			FuzzyNumberSet returnSet = new FuzzyNumberSet();

			double dSetOneMinMinimum;
			double dSetOneMaxMinimum;
			double dSetOneMinMembership; 
			double dSetOneMaxMembership;
			double dSetOneMinMaximum;
			double dSetOneMaxMaximum;
			double dSetOneMinNumber;
			double dSetOneMaxNumber;
			double dSetTwoMinMinimum;
			double dSetTwoMaxMinimum;
			double dSetTwoMinMembership;
			double dSetTwoMaxMembership;
			double dSetTwoMinMaximum;
			double dSetTwoMaxMaximum;
			double dSetTwoMinNumber;
			double dSetTwoMaxNumber;

			for( int i=0; i<this.Count; i++ )
			{

				if( setParams.SetOneMaxMembership == 0 )
					dSetOneMaxMembership = ( ( FuzzyNumber )this.FuzzyArray[ i ] ).Membership;
				else
					dSetOneMaxMembership = setParams.SetOneMaxMembership;

				if( setParams.SetOneMaxMinimum == 0 )
					dSetOneMaxMinimum = ( ( FuzzyNumber )this.FuzzyArray[ i ] ).Minimum;
				else
					dSetOneMaxMinimum = setParams.SetOneMaxMinimum;

				if( setParams.SetOneMaxNumber == 0 )
					dSetOneMaxNumber = ( ( FuzzyNumber )this.FuzzyArray[ i ] ).Number;
				else
					dSetOneMaxNumber = setParams.SetOneMaxNumber;

				if( setParams.SetOneMinMaximum == 0 )
					dSetOneMinMaximum = ( ( FuzzyNumber )this.FuzzyArray[ i ] ).Maximum;
				else
					dSetOneMinMaximum = setParams.SetOneMinMaximum;

				if( setParams.SetOneMaxMaximum == 0 )
					dSetOneMaxMaximum = ( ( FuzzyNumber )this.FuzzyArray[ i ] ).Maximum;
				else
					dSetOneMaxMaximum = setParams.SetOneMaxMaximum;

				if( setParams.SetOneMinMembership == 0 )
					dSetOneMinMembership = ( ( FuzzyNumber )this.FuzzyArray[ i ] ).Membership;
				else
					dSetOneMinMembership = setParams.SetOneMinMembership;

				if( setParams.SetOneMinMinimum == 0 )
					dSetOneMinMinimum = ( ( FuzzyNumber )this.FuzzyArray[ i ] ).Minimum;
				else
					dSetOneMinMinimum = setParams.SetOneMinMinimum;

				if( setParams.SetOneMinNumber == 0 )
					dSetOneMinNumber = ( ( FuzzyNumber )this.FuzzyArray[ i ] ).Number;
				else
					dSetOneMinNumber = setParams.SetOneMinNumber;

				for( int n=0; n<fuzzySet.Count; n++ )
				{

					if( setParams.SetTwoMaxMembership == 0 )
						dSetTwoMaxMembership = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Membership;
					else
						dSetTwoMaxMembership = setParams.SetTwoMaxMembership;

					if( setParams.SetTwoMaxMinimum == 0 )
						dSetTwoMaxMinimum = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Minimum;
					else
						dSetTwoMaxMinimum = setParams.SetTwoMaxMinimum;

					if( setParams.SetTwoMaxNumber == 0 )
						dSetTwoMaxNumber = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Number;
					else
						dSetTwoMaxNumber = setParams.SetTwoMaxNumber;

					if( setParams.SetTwoMinMaximum == 0 )
						dSetTwoMinMaximum = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Maximum;
					else
						dSetTwoMinMaximum = setParams.SetTwoMinMaximum;

					if( setParams.SetTwoMaxMaximum == 0 )
						dSetTwoMaxMaximum = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Maximum;
					else
						dSetTwoMaxMaximum = setParams.SetTwoMaxMaximum;

					if( setParams.SetTwoMinMembership == 0 )
						dSetTwoMinMembership = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Membership;
					else
						dSetTwoMinMembership = setParams.SetTwoMinMembership;

					if( setParams.SetTwoMinMinimum == 0 ) 
						dSetTwoMinMinimum = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Minimum;
					else
						dSetTwoMinMinimum = setParams.SetTwoMinMinimum;

					if( setParams.SetTwoMinNumber == 0 )
						dSetTwoMinNumber = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Number;
					else
						dSetTwoMinNumber = setParams.SetTwoMinNumber;

					if( ( ( FuzzyNumber )this.FuzzyArray[ i ] ).Maximum >= dSetOneMinMaximum &&
						( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Maximum >= dSetTwoMinMaximum &&
						( ( FuzzyNumber )this.FuzzyArray[ i ] ).Maximum <= dSetOneMaxMaximum &&
						( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Maximum <= dSetTwoMaxMaximum &&
						( ( FuzzyNumber )this.FuzzyArray[ i ] ).Membership >= dSetOneMinMembership &&
						( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Membership >= dSetTwoMinMembership &&
						( ( FuzzyNumber )this.FuzzyArray[ i ] ).Membership <= dSetOneMaxMembership &&
						( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Membership <= dSetTwoMaxMembership &&
						( ( FuzzyNumber )this.FuzzyArray[ i ] ).Minimum >= dSetOneMinMinimum &&
						( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Minimum >= dSetTwoMinMinimum &&
						( ( FuzzyNumber )this.FuzzyArray[ i ] ).Minimum <= dSetOneMaxMinimum &&
						( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Minimum <= dSetTwoMaxMinimum &&
						( ( FuzzyNumber )this.FuzzyArray[ i ] ).Number >= dSetOneMinNumber &&
						( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Number >= dSetTwoMinNumber &&
						( ( FuzzyNumber )this.FuzzyArray[ i ] ).Number <= dSetOneMaxNumber &&
						( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Number <= dSetTwoMaxNumber )
					{

						if( dSetOneMinMaximum >= dSetTwoMinMaximum &&
							dSetOneMaxMaximum <= dSetTwoMaxMaximum &&
							dSetOneMinMembership >= dSetTwoMinMembership &&
							dSetOneMaxMembership <= dSetTwoMaxMembership &&
							dSetOneMinMinimum >= dSetTwoMinMinimum &&
							dSetOneMaxMinimum <= dSetTwoMaxMinimum &&
							dSetOneMinNumber >= dSetTwoMinNumber &&
							dSetOneMaxNumber <= dSetTwoMaxNumber )					
						{
							returnSet[ returnSet.Count + 1 ] = new FuzzyNumber( ( FuzzyNumber )this.FuzzyArray[ i ] );
						}
					}
				}
			}

			return returnSet;

		}

		/// <summary>
		/// return an exclusive or set
		/// </summary>
		/// <param name="setOne"></param>
		/// <param name="setTwo"></param>
		/// <returns></returns>
		public FuzzyNumberSet ExclusiveOR( FuzzySet fuzzySet )
		{
			FuzzyNumberSet returnSet = new FuzzyNumberSet();
			bool bFound = false;


			/// Get the ones in set one but not in set two
			for( int i=0; i<this.Count; i++ )
			{
				bFound = false;

				for( int n=0; n<fuzzySet.Count; n++ )
				{
					if( ( ( FuzzyNumber )this.FuzzyArray[ i ] ).Maximum == ( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Maximum &&
						( ( FuzzyNumber )this.FuzzyArray[ i ] ).Membership == ( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Membership &&
						( ( FuzzyNumber )this.FuzzyArray[ i ] ).Minimum == ( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Minimum &&
						( ( FuzzyNumber )this.FuzzyArray[ i ] ).Number == ( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Number )
					{
						bFound = true;
						n=fuzzySet.Count;
					}
				}

				if( bFound == false )
				{
					returnSet[ returnSet.Count + 1 ] = new FuzzyNumber( ( FuzzyNumber )this.FuzzyArray[ i ] );
				}
			}

			/// get the ones in set two but not in set one
			for( int i=0; i<fuzzySet.Count; i++ )
			{
				bFound = false;

				for( int n=0; n<this.Count; n++ )
				{
					if( ( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Maximum == ( ( FuzzyNumber )this.FuzzyArray[ n ] ).Maximum && 
						( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Membership == ( ( FuzzyNumber )this.FuzzyArray[ n ] ).Membership &&
						( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Minimum == ( ( FuzzyNumber )this.FuzzyArray[ n ] ).Minimum && 
						( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Number == ( ( FuzzyNumber )this.FuzzyArray[ n ] ).Number )
					{
						bFound = true;
						n=this.Count;
					}
				}

				if( bFound == false )
				{
					returnSet[ returnSet.Count + 1 ] = new FuzzyNumber( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] );
				}
			}

			return returnSet;
		}


		/// <summary>
		/// exclusive or taking the fuzzy parameter set.
		/// </summary>
		/// <param name="setOne"></param>
		/// <param name="setTwo"></param>
		/// <param name="setParams"></param>
		/// <returns></returns>
		public FuzzyNumberSet ExclusiveOR( FuzzySet fuzzySet, FuzzySetParameters setParams )
		{
			FuzzyNumberSet returnSet = new FuzzyNumberSet();
			bool bFound = false;

			double dSetOneMinMinimum;
			double dSetOneMaxMinimum;
			double dSetOneMinMembership; 
			double dSetOneMaxMembership;
			double dSetOneMinMaximum;
			double dSetOneMaxMaximum;
			double dSetOneMinNumber;
			double dSetOneMaxNumber;
			double dSetTwoMinMinimum;
			double dSetTwoMaxMinimum;
			double dSetTwoMinMembership;
			double dSetTwoMaxMembership;
			double dSetTwoMinMaximum;
			double dSetTwoMaxMaximum;
			double dSetTwoMinNumber;
			double dSetTwoMaxNumber;


			/// Get the ones in set one but not in set two
			for( int i=0; i<this.Count; i++ )
			{
				bFound = false;

				if( setParams.SetOneMaxMembership == 0 )
					dSetOneMaxMembership = ( ( FuzzyNumber )this.FuzzyArray[ i ] ).Membership;
				else
					dSetOneMaxMembership = setParams.SetOneMaxMembership;

				if( setParams.SetOneMaxMinimum == 0 )
					dSetOneMaxMinimum = ( ( FuzzyNumber )this.FuzzyArray[ i ] ).Minimum;
				else
					dSetOneMaxMinimum = setParams.SetOneMaxMinimum;

				if( setParams.SetOneMaxNumber == 0 )
					dSetOneMaxNumber = ( ( FuzzyNumber )this.FuzzyArray[ i ] ).Number;
				else
					dSetOneMaxNumber = setParams.SetOneMaxNumber;

				if( setParams.SetOneMinMaximum == 0 )
					dSetOneMinMaximum = ( ( FuzzyNumber )this.FuzzyArray[ i ] ).Maximum;
				else
					dSetOneMinMaximum = setParams.SetOneMinMaximum;

				if( setParams.SetOneMaxMaximum == 0 )
					dSetOneMaxMaximum = ( ( FuzzyNumber )this.FuzzyArray[ i ] ).Maximum;
				else
					dSetOneMaxMaximum = setParams.SetOneMaxMaximum;

				if( setParams.SetOneMinMembership == 0 )
					dSetOneMinMembership = ( ( FuzzyNumber )this.FuzzyArray[ i ] ).Membership;
				else
					dSetOneMinMembership = setParams.SetOneMinMembership;

				if( setParams.SetOneMinMinimum == 0 )
					dSetOneMinMinimum = ( ( FuzzyNumber )this.FuzzyArray[ i ] ).Minimum;
				else
					dSetOneMinMinimum = setParams.SetOneMinMinimum;

				if( setParams.SetOneMinNumber == 0 )
					dSetOneMinNumber = ( ( FuzzyNumber )this.FuzzyArray[ i ] ).Number;
				else
					dSetOneMinNumber = setParams.SetOneMinNumber;


				for( int n=0; n<fuzzySet.Count; n++ )
				{
					if( setParams.SetTwoMaxMembership == 0 )
						dSetTwoMaxMembership = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Membership;
					else
						dSetTwoMaxMembership = setParams.SetTwoMaxMembership;

					if( setParams.SetTwoMaxMinimum == 0 )
						dSetTwoMaxMinimum = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Minimum;
					else
						dSetTwoMaxMinimum = setParams.SetTwoMaxMinimum;

					if( setParams.SetTwoMaxNumber == 0 )
						dSetTwoMaxNumber = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Number;
					else
						dSetTwoMaxNumber = setParams.SetTwoMaxNumber;

					if( setParams.SetTwoMinMaximum == 0 )
						dSetTwoMinMaximum = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Maximum;
					else
						dSetTwoMinMaximum = setParams.SetTwoMinMaximum;

					if( setParams.SetTwoMaxMaximum == 0 )
						dSetTwoMaxMaximum = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Maximum;
					else
						dSetTwoMaxMaximum = setParams.SetTwoMaxMaximum;

					if( setParams.SetTwoMinMembership == 0 )
						dSetTwoMinMembership = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Membership;
					else
						dSetTwoMinMembership = setParams.SetTwoMinMembership;

					if( setParams.SetTwoMinMinimum == 0 ) 
						dSetTwoMinMinimum = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Minimum;
					else
						dSetTwoMinMinimum = setParams.SetTwoMinMinimum;

					if( setParams.SetTwoMinNumber == 0 )
						dSetTwoMinNumber = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ n ] ).Number;
					else
						dSetTwoMinNumber = setParams.SetTwoMinNumber;


					if( dSetOneMinMaximum >= dSetTwoMinMaximum &&
						dSetOneMaxMaximum <= dSetTwoMaxMaximum &&
						dSetOneMinMembership >= dSetTwoMinMembership &&
						dSetOneMaxMembership <= dSetTwoMaxMembership &&
						dSetOneMinMinimum >= dSetTwoMinMinimum &&
						dSetOneMaxMinimum <= dSetTwoMaxMinimum &&
						dSetOneMinNumber >= dSetTwoMinNumber &&
						dSetOneMaxNumber <= dSetTwoMaxNumber )						
					{
						bFound = true;
						n=fuzzySet.Count;
					}
				}

				if( bFound == false )
				{
					returnSet[ returnSet.Count + 1 ] = new FuzzyNumber( ( FuzzyNumber )this.FuzzyArray[ i ] );
				}
			}

			/// get the ones in set two but not in set one
			for( int i=0; i<fuzzySet.Count; i++ )
			{
				bFound = false;

				if( setParams.SetTwoMaxMembership == 0 )
					dSetTwoMaxMembership = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Membership;
				else
					dSetTwoMaxMembership = setParams.SetTwoMaxMembership;

				if( setParams.SetTwoMaxMinimum == 0 )
					dSetTwoMaxMinimum = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Minimum;
				else
					dSetTwoMaxMinimum = setParams.SetTwoMaxMinimum;

				if( setParams.SetTwoMaxNumber == 0 )
					dSetTwoMaxNumber = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Number;
				else
					dSetTwoMaxNumber = setParams.SetTwoMaxNumber;

				if( setParams.SetTwoMinMaximum == 0 )
					dSetTwoMinMaximum = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Maximum;
				else
					dSetTwoMinMaximum = setParams.SetTwoMinMaximum;

				if( setParams.SetTwoMaxMaximum == 0 )
					dSetTwoMaxMaximum = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Maximum;
				else
					dSetTwoMaxMaximum = setParams.SetTwoMaxMaximum;

				if( setParams.SetTwoMinMembership == 0 )
					dSetTwoMinMembership = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Membership;
				else
					dSetTwoMinMembership = setParams.SetTwoMinMembership;

				if( setParams.SetTwoMinMinimum == 0 ) 
					dSetTwoMinMinimum = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Minimum;
				else
					dSetTwoMinMinimum = setParams.SetTwoMinMinimum;

				if( setParams.SetTwoMinNumber == 0 )
					dSetTwoMinNumber = ( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] ).Number;
				else
					dSetTwoMinNumber = setParams.SetTwoMinNumber;


				for( int n=0; n<this.Count; n++ )
				{
					if( setParams.SetOneMaxMembership == 0 )
						dSetOneMaxMembership = ( ( FuzzyNumber )this.FuzzyArray[ n ] ).Membership;
					else
						dSetOneMaxMembership = setParams.SetOneMaxMembership;

					if( setParams.SetOneMaxMinimum == 0 )
						dSetOneMaxMinimum = ( ( FuzzyNumber )this.FuzzyArray[ n ] ).Minimum;
					else
						dSetOneMaxMinimum = setParams.SetOneMaxMinimum;

					if( setParams.SetOneMaxNumber == 0 )
						dSetOneMaxNumber = ( ( FuzzyNumber )this.FuzzyArray[ n ] ).Number;
					else
						dSetOneMaxNumber = setParams.SetOneMaxNumber;

					if( setParams.SetOneMinMaximum == 0 )
						dSetOneMinMaximum = ( ( FuzzyNumber )this.FuzzyArray[ n ] ).Maximum;
					else
						dSetOneMinMaximum = setParams.SetOneMinMaximum;

					if( setParams.SetOneMaxMaximum == 0 )
						dSetOneMaxMaximum = ( ( FuzzyNumber )this.FuzzyArray[ n ] ).Maximum;
					else
						dSetOneMaxMaximum = setParams.SetOneMaxMaximum;

					if( setParams.SetOneMinMembership == 0 )
						dSetOneMinMembership = ( ( FuzzyNumber )this.FuzzyArray[ n ] ).Membership;
					else
						dSetOneMinMembership = setParams.SetOneMinMembership;

					if( setParams.SetOneMinMinimum == 0 )
						dSetOneMinMinimum = ( ( FuzzyNumber )this.FuzzyArray[ n ] ).Minimum;
					else
						dSetOneMinMinimum = setParams.SetOneMinMinimum;

					if( setParams.SetOneMinNumber == 0 )
						dSetOneMinNumber = ( ( FuzzyNumber )this.FuzzyArray[ n ] ).Number;
					else
						dSetOneMinNumber = setParams.SetOneMinNumber;


					if( dSetOneMinMaximum >= dSetTwoMinMaximum &&
						dSetOneMaxMaximum <= dSetTwoMaxMaximum &&
						dSetOneMinMembership >= dSetTwoMinMembership &&
						dSetOneMaxMembership <= dSetTwoMaxMembership &&
						dSetOneMinMinimum >= dSetTwoMinMinimum &&
						dSetOneMaxMinimum <= dSetTwoMaxMinimum &&
						dSetOneMinNumber >= dSetTwoMinNumber &&
						dSetOneMaxNumber <= dSetTwoMaxNumber )						
					{
						bFound = true;
						n=this.Count;
					}
				}

				if( bFound == false )
				{
					returnSet[ returnSet.Count + 1 ] = new FuzzyNumber( ( FuzzyNumber )fuzzySet.FuzzyArray[ i ] );
				}
			}

			return returnSet;

		}


		/// get a string for logging and printing the contents of the current fuzzy set
		public string FuzzyPrintOut
		{
			get
			{
				StringBuilder strTemp = new StringBuilder();
				strTemp.Append( "\nContents Of Fuzzy Set " );
				if( this.Name != "" )
					strTemp.Append( "Name = " + this.Name + "\n" );
				else
					strTemp.Append( "\n" );

				for( int i=0; i<this.Count; i++ )
				{
					strTemp.Append( "\n" + ( ( FuzzyNumber )this.FuzzyArray[ i ] ).ToString() + "\n" );
				}

				return strTemp.ToString();
			}
		}
	}


	/// <summary>
	/// The fuzzy set class for holding and manipulating fuzzy numbers
	/// </summary>
	public class FuzzyNumberSet : FuzzySet
	{
		private double setValue;

		public double FuzzySetValue
		{
			get
			{
				return setValue;
			}
			set
			{
				setValue = value;
			}
		}
		
		/// <summary>
		/// basic constructor
		/// </summary>
		public FuzzyNumberSet() : base()
		{
		}

		/// <summary>
		///  conbstructor that creates a number of empty fuzzy numbers
		/// </summary>
		/// <param name="nNumberCount"></param>
		public FuzzyNumberSet( int nNumberCount ) : base()
		{

			for( int i=0; i<nNumberCount; i++ )
			{
				FuzzyNumber temp = new FuzzyNumber();
				FuzzyArray.Add( temp );
			}
		}

		/// <summary>
		/// constructor that takes the first fuzzy number
		/// </summary>
		/// <param name="fuzzyNum"></param>
		public FuzzyNumberSet( FuzzyNumber fuzzyNum ) : base()
		{
			FuzzyArray.Add( fuzzyNum );
		}

		/// <summary>
		/// constructor that takes the values for the first fuzzy number
		/// </summary>
		/// <param name="number"></param>
		/// <param name="minimum"></param>
		/// <param name="maximum"></param>
		public FuzzyNumberSet( double number, double minimum, double maximum ) : base()
		{
			FuzzyNumber temp = new FuzzyNumber( number, minimum, maximum );
			FuzzyArray.Add( temp );
		}

		
		/// <summary>
		///  Is the current value within the specified term ( name of the fuzzy number )
		///  
		/// </summary>
		/// <param name="strTerm">name of the fuzzy number that the value is supposed to be within</param>
		/// <returns>true or false</returns>
		public bool IsTerm( string strTerm )
		{
			for( int i=0; i<this.Count; i++ )
			{
				if( ( ( FuzzyNumber )FuzzyArray[ i ] ).Name == strTerm )
				{
					FuzzyNumber temp = ( FuzzyNumber )FuzzyArray[ i ];
					if( temp.Maximum >= this.setValue && temp.Minimum <= this.setValue )
					{
						return true;
					}
					else
						return false;
				}
			}

			return false;
		}

	}
}
