using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;


/////////////////////////////////////////////////////////////////////////////////
//
//	TextParse.cs
//
// 	Author: Christopher J. Franzwa
//
//	Member of: Utilities
//
//	description:	Offers the ability to grab and parse a text file into 'snippets'
//					The text can then be searched for a variable, returning the 
//					relevant value. 
//
/////////////////////////////////////////////////////////////////////////////////

public class snippet{
	
	private string Value;
	private string Variable;
	
	public snippet(string Value,string Variable){this.Value=Value; this.Variable=Variable;}
	
	public string getValue(){return this.Value;}
	public string getVariable(){return this.Variable;}
	
	public void setValue(string Value){this.Value=Value;}
	public void setVariable(string Variable){this.Variable=Variable;}
}

public class TextParse{
	
	static List<snippet> ParsedText = new List<snippet>();
	
	private string TextFile;
	private string [] TextLines;
	private string delimiter="\n";
	public TextParse(string textLocation)
	{
		
		TextFile=File.ReadAllText(textLocation);
		
		TextLines=TextFile.Split(delimiter.ToCharArray());
		
		for(int i=0; i<TextLines.Length;i++)
		{
			string currentLine=TextLines[i];
			
			if(currentLine.ToCharArray().Length<=0)
				continue;
			
			if(currentLine.IndexOf("=")<0)
				continue;
			
			switch(currentLine.ToCharArray()[0]){
				
				case '/':
				
					//Do not parse. This line is a comment
					break;
				
				default:
					//Parse this line and split it into variable title and value
					ParsedText.Add (TextToSnippet(currentLine));
					break;
			}
		}
		
	}
	
	public string getTextFile(){return TextFile;}
	
	public string [] getTextArray(){return TextLines;}
	
	public T getValue<T>(string VariableName)
	{
		string returnValue="";
		foreach(snippet Snippet in ParsedText){
			if(VariableName.CompareTo(Snippet.getVariable())==0){
				returnValue = Snippet.getValue();
				break;
			}
		}

			return (T) Convert.ChangeType(returnValue.Trim('"'),typeof(T));		
	}
	
	public List<snippet> getAllSnippets()
	{
		return ParsedText;
	}
	
	private snippet TextToSnippet(string TextLine)
	{
		string [] textComponents = TextLine.Split("="[0]);
		
		if(textComponents.Length<2)
			return new snippet("Expecting '='","Syntax error");
		else if(textComponents.Length>2)
			return new snippet("Unexpected '='","Syntax error");
		else
			return new snippet(textComponents[1],textComponents[0].Trim('*'));
		
	}
}
