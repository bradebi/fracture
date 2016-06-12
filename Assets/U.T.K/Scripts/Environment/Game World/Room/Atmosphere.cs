using UnityEngine;
using System.Collections;
[System.Serializable]


public class Atmosphere {
	
	//All possible gas types to choose from. Some are single elements, others are common gas compositions.	
		public enum Gas_Type
    {
		Ammonia,		 //NH3
        Argon,			 //Ar
        Carbon_Dioxide,  //CO2
		Carbon_Monoxide, //CO
		Helium,			 //He
        Hydrogen,		 //H2
		Iodine,			 //I2
		Krypton,		 //Kr
		Methane,		 //CH4
        Neon,		     //Ne
        Nitrogen,	     //N2
		Nitrogen_Dioxide,//NO2
		Nitrous_Oxide,   //N2O
		Oxygen,			 //O2
		Ozone,			 //O3
		Propane,		 //C3H8
		Water_Vapor,	 //H2O
		Xenon			 //Xe
    }
	
	public float Temperature = 273; //In Kelvin 273K is roughly equal to 0 degrees Celsius
	public float Pressure = 1; //In Atmospheres--1 Atmosphere is equal to Earth's Atmospheric Pressure
	public float Radiation = 0; //Temporary Expression of Radiation. A more advanced representation may be used in the future
	//Gas Composition Variables
	public Gas_Type[] Gas;
	public float[] Percentage;
	
	void Awake() {
		//Initializes at the atmospheric composition of Earth
		SetToEarthAtm ();
	}
	
	//Can add any of the types of gases listed in "Gas_Type"
	//Currently adds in terms of percentages, but I may change it to a percentage in terms of the current proportions
	void AddGas(Gas_Type gas_type,float percentage)
	{
		bool Detected=false;
		for(int j=0;j<Gas.Length;j++)
		{
			if(Gas[j]==gas_type)
			{
				Percentage[j]+=percentage;
				Detected=true;		
			}
		}
			if(!Detected)
			{
				Gas_Type[] GasBuffer= new Gas_Type [Gas.Length+1];
				float[] PerBuffer= new float [Percentage.Length+1];
				for(int j=0;j<Gas.Length;j++)
				{	
					GasBuffer[j]=Gas[j];
					PerBuffer[j]=Percentage[j];
				}
				GasBuffer[Gas.Length]=gas_type;
				PerBuffer[Gas.Length]=percentage;
				Gas= new Gas_Type[GasBuffer.Length];
				Percentage= new float[PerBuffer.Length];
				for(int j=0;j<Gas.Length;j++)
				{	
					Gas[j]=GasBuffer[j];
					Percentage[j]=PerBuffer[j];
				}
			}
		NormalizeGas();	
	}
	//Can remove any of the types of gases listed in "Gas_Type"
	//If more gas is removed than exists, then the gas is removed from the array entirely
	//Returns false if the input gas does not exist in the atmosphere composition
	//Currently adds in terms of percentages, but I may change it to a percentage in terms of the current proportions
	bool RemoveGas(Gas_Type gas_type,float percentage)
	{
		bool Detected=false;
		for(int j=0;j<Gas.Length;j++)
		{
			if(Gas[j]==gas_type)
			{
				Percentage[j]-=percentage;
				Detected=true;
				if(Percentage[j]<=0)
					ClearGas(Gas[j]);
			}	
			}
		if(!Detected)
			return(false);
		else
		{
			NormalizeGas();	
			return(true);
		}
	}
	//Can clear any of the types of gases listed in "Gas_Type" entirely
	void ClearGas(Gas_Type gas_type)
	{
		int Clear=0;
		bool Detected=false;
		for(int j=0;j<Gas.Length;j++)
		{
			if(Gas[j]==gas_type)
			{
				Clear=j;
				Detected=true;
			}	
		}
		if(Detected)
			{
				Gas_Type[] GasBuffer= new Gas_Type [Gas.Length-1];
				float[] PerBuffer= new float [Percentage.Length-1];
				for(int j=0;j<Gas.Length;j++)
				{	
					if(j<Clear)
					{
					GasBuffer[j]=Gas[j];
					PerBuffer[j]=Percentage[j];
					}
					else if(j>Clear)
					{
					GasBuffer[j-1]=Gas[j];
					PerBuffer[j-1]=Percentage[j];	
					}
				}

				Gas= new Gas_Type[GasBuffer.Length];
				Percentage= new float[PerBuffer.Length];
				for(int j=0;j<Gas.Length;j++)
				{	
					Gas[j]=GasBuffer[j];
					Percentage[j]=PerBuffer[j];
				}
	}
		NormalizeGas();	
	}
	//Flushes all gases in the Atmosphere
	void ClearAllGas()
	{
		Gas = new Gas_Type[0];
		Percentage = new float[0];
	}
	//Sets the Atmosphere to Earth's conditions at Sea Level
	void SetToEarthAtm()
	{
		Pressure = 1;
		Radiation = 0;
		
		Gas= new Gas_Type[4];
		Percentage= new float[4];
		Gas[0]=Gas_Type.Nitrogen;
		Percentage[0]=78.09f;
		Gas[1]=Gas_Type.Oxygen;
		Percentage[1]=20.95f;
		Gas[2]=Gas_Type.Argon;
		Percentage[2]=0.93f;
		Gas[3]=Gas_Type.Carbon_Dioxide;
		Percentage[3]=0.039f;
	}
	//Renormalizes all of the gases in the atmosphere to proper proportions out of 100%
	void NormalizeGas()
	{
		float TotalGas=0;
		for(int j=0;j<Gas.Length;j++)
		{
				TotalGas+=Percentage[j];
		}
		for(int j=0;j<Gas.Length;j++)
		{
				Percentage[j]=Percentage[j]/TotalGas;
		}
	}
	
	//Returns an index that relates to how well the mixture of gases present fuel flames and explosions 
	float GetExplosiveIndex()
	{
		return(0);
	}
	
	
}

