using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrays : MonoBehaviour
{
    private void Start()
    {

    }

    public string[] hpRule = { "" , ""};
    public string[] bleedRule = { "", "" };

    public double[] hpDom = { 0, 0 };
    public double[] bleedDom = { 0, 0 };

    public double[,] DoMem = { { 0, 0, 0, 0 },
                               { 0, 0, 0, 0}};
    public string[,] currentState = { { "", "", "", ""},
                                      { "", "", "", ""}};
    public string[,] States = { { "VeryLow", "VeryLow", "Heal" },
                                { "VeryLow", "Low"    , "Heal" },
                                { "VeryLow", "Medium" , "Heal" },
                                { "VeryLow", "High"   , "Heal" },
                                { "Low",  "VeryLow", "Shield" },
                                { "Low",  "Low"    , "Shield" },
                                { "Low",  "Medium" , "Heal" },
                                { "Low",  "High"   , "Heal" },
                                { "Medium",  "VeryLow", "Shield" },
                                { "Medium",  "Low"    , "Shield" },
                                { "Medium",  "Medium" , "Shield" },
                                { "Medium",  "High"   , "Heal" },
                                { "High",  "VeryLow", "Shield" },
                                { "High",  "Low"    , "Shield" },
                                { "High",  "Medium" , "Shield" },
                                { "High",  "High"   , "Shield" }};

    //Dom of Paladin Object(Hp, Stamina, Distance from target)
    public double[,] PaladinDom = {{ 0, 0, 0, 0},
                                   { 0, 0, 0, 0},
                                   { 0, 0, 0, 0}};

    public string[,] PaladinCurrentState = { { "", "", "", ""},
                                             { "", "", "", ""},
                                             { "", "", "", ""}};

    public string[] PaladinHpRule = { "", "" };
    public string[] PaladinStaminaRule = { "", "" };
    public string[] PaladinDistanceRule = { "", "" };

    public double[] PaladinHpDom = { 0, 0 };
    public double[] PaladinStaminaDom = { 0, 0 };
    public double[] PaladinDistanceDom = { 0, 0 };

    //Warrok object arrays
    public double[,] WarrokDom = {{ 0, 0, 0, 0},
                                   { 0, 0, 0, 0},
                                   { 0, 0, 0, 0}};

    public string[] WarrokHpRule = { "", "" };
    public string[] WarrokStaminaRule = { "", "" };
    public string[] WarrokDistanceRule = { "", "" };

    public string[,] WarrokCurrentState = { { "", "", "", ""},
                                             { "", "", "", ""},
                                             { "", "", "", ""}};

    public double[] WarrokHpDom = { 0, 0 };
    public double[] WarrokStaminaDom = { 0, 0 };
    public double[] WarrokDistanceDom = { 0, 0 };

    //Fuzzy rules for the Paladin Object decision
    public string[,] PaladinFuzzyRules = {{ "VeryLow", "VeryLow", "InRange", "Retreat" },
                                          { "VeryLow", "VeryLow", "Close", "Retreat" },
                                          { "VeryLow", "VeryLow", "Far" , "Retreat"},
                                          { "VeryLow", "VeryLow", "VeryFar" , "Retreat"},
                                          { "VeryLow",  "Low", "InRange" ,"Retreat"},
                                          { "VeryLow",  "Low", "Close" ,"Retreat"},
                                          { "VeryLow",  "Low", "Far" ,"Retreat"},
                                          { "VeryLow",  "Low", "VeryFar","Retreat" },
                                          { "VeryLow",  "Medium", "InRange","Retreat" },
                                          { "VeryLow",  "Medium", "Close" ,"Retreat"},
                                          { "VeryLow",  "Medium", "Far" ,"Retreat"},
                                          { "VeryLow",  "Medium", "VeryFar" ,"Retreat"},
                                          { "VeryLow",  "High", "InRange" ,"Retreat"},
                                          { "VeryLow",  "High", "Close","Retreat" },
                                          { "VeryLow",  "High", "Far","Retreat" },
                                          { "VeryLow",  "High", "VeryFar","Retreat" },
                                          { "Low",  "VeryLow", "InRange","Retreat" },
                                          { "Low",  "VeryLow", "Close" ,"Retreat"},
                                          { "Low",  "VeryLow", "Far" ,"Retreat"},
                                          { "Low",  "VeryLow", "VeryFar","Retreat"},
                                          { "Low",  "Low", "InRange" ,"Retreat"},
                                          { "Low",  "Low", "Close" ,"Retreat"},
                                          { "Low",  "Low", "Far" ,"Retreat"},
                                          { "Low",  "Low", "VeryFar" , "Retreat"},
                                          { "Low",  "Medium", "InRange" ,"Retreat"},
                                          { "Low",  "Medium", "Close","Retreat" },
                                          { "Low",  "Medium", "Far" , "Retreat"},
                                          { "Low",  "Medium", "VeryFar" , "Retreat"},
                                          { "Low",  "High", "InRange" ,"Retreat"},
                                          { "Low",  "High", "Close" ,"Retreat"},
                                          { "Low",  "High", "Far","Retreat"},
                                          { "Low",  "High", "VeryFar","Retreat" },
                                          { "Medium",  "VeryLow", "InRange" ,"Block"},
                                          { "Medium",  "VeryLow", "Close" ,"Block"},
                                          { "Medium",  "VeryLow", "Far" ,"Hunting"},
                                          { "Medium",  "VeryLow", "VeryFar","Searching" },
                                          { "Medium",  "Low", "InRange" ,"Attack"},
                                          { "Medium",  "Low", "Close"  ,"Block"},
                                          { "Medium",  "Low", "Far"  ,"Hunting"},
                                          { "Medium",  "Low", "VeryFar"  ,"Hunting"},
                                          { "Medium",  "Medium", "InRange","Attack" },
                                          { "Medium",  "Medium", "Close","Attack" },
                                          { "Medium",  "Medium", "Far" ,"Searching"},
                                          { "Medium",  "Medium", "VeryFar" ,"Searching"},
                                          { "Medium",  "High", "InRange" ,"Attack"},
                                          { "Medium",  "High", "Close","Hunting" },
                                          { "Medium",  "High", "Far" ,"Hunting" },
                                          { "Medium",  "High", "VeryFar","Searching" },
                                          { "High",  "VeryLow", "InRange" ,"Block"},
                                          { "High",  "VeryLow", "Close" ,"Block"},
                                          { "High",  "VeryLow", "Far" ,"Hunting"},
                                          { "High",  "VeryLow", "VeryFar","Searching" },
                                          { "High",  "Low", "InRange" ,"Attack"},
                                          { "High",  "Low", "Close" ,"Hunting"},
                                          { "High",  "Low", "Far" ,"Hunting" },
                                          { "High",  "Low", "VeryFar" ,"Searching"},
                                          { "High",  "Medium", "InRange","Attack" },
                                          { "High",  "Medium", "Close" ,"Hunting" },
                                          { "High",  "Medium", "Far","Hunting" },
                                          { "High",  "Medium", "VeryFar" ,"Searching"},
                                          { "High",  "High", "InRange" ,"Attack"},
                                          { "High",  "High", "Close","Hunting" },
                                          { "High",  "High", "Far","Searching" },
                                          { "High",  "High", "VeryFar","Searching" }};

    public string[,] WarrokFuzzyRules = {{ "VeryLow", "VeryLow", "InRange", "Heal" },
                                          { "VeryLow", "VeryLow", "Close", "Heal" },
                                          { "VeryLow", "VeryLow", "Far" , "Heal"},
                                          { "VeryLow", "VeryLow", "VeryFar" , "Heal"},
                                          { "VeryLow",  "Low", "InRange" ,"Heal"},
                                          { "VeryLow",  "Low", "Close" ,"Heal"},
                                          { "VeryLow",  "Low", "Far" ,"Heal"},
                                          { "VeryLow",  "Low", "VeryFar","Heal" },
                                          { "VeryLow",  "Medium", "InRange","Heal" },
                                          { "VeryLow",  "Medium", "Close" ,"Heal"},
                                          { "VeryLow",  "Medium", "Far" ,"Heal"},
                                          { "VeryLow",  "Medium", "VeryFar" ,"Heal"},
                                          { "VeryLow",  "High", "InRange" ,"Heal"},
                                          { "VeryLow",  "High", "Close","Heal" },
                                          { "VeryLow",  "High", "Far","Heal" },
                                          { "VeryLow",  "High", "VeryFar","Heal" },
                                          { "Low",  "VeryLow", "InRange","Heal" },
                                          { "Low",  "VeryLow", "Close" ,"Rage"},//rage
                                          { "Low",  "VeryLow", "Far" ,"Lunge"},
                                          { "Low",  "VeryLow", "VeryFar","Lunge"},
                                          { "Low",  "Low", "InRange" ,"Rage"},//rage
                                          { "Low",  "Low", "Close" ,"Rage"},//rage
                                          { "Low",  "Low", "Far" ,"Lunge"},
                                          { "Low",  "Low", "VeryFar" , "Lunge"},
                                          { "Low",  "Medium", "InRange" ,"Lunge"},
                                          { "Low",  "Medium", "Close","Rage" },//rage
                                          { "Low",  "Medium", "Far" , "Lunge"},
                                          { "Low",  "Medium", "VeryFar" , "Lunge"},
                                          { "Low",  "High", "InRange" ,"Attack"},
                                          { "Low",  "High", "Close" ,"Attack"},
                                          { "Low",  "High", "Far","Lunge"},
                                          { "Low",  "High", "VeryFar","Lunge" },
                                          { "Medium",  "VeryLow", "InRange" ,"Attack"},
                                          { "Medium",  "VeryLow", "Close" ,"Rage"},
                                          { "Medium",  "VeryLow", "Far" ,"Lunge"},
                                          { "Medium",  "VeryLow", "VeryFar","Lunge" },
                                          { "Medium",  "Low", "InRange" ,"Attack"},
                                          { "Medium",  "Low", "Close"  ,"Attack"},
                                          { "Medium",  "Low", "Far"  ,"Lunge"},
                                          { "Medium",  "Low", "VeryFar"  ,"Lunge"},
                                          { "Medium",  "Medium", "InRange","Attack" },
                                          { "Medium",  "Medium", "Close","Attack" },
                                          { "Medium",  "Medium", "Far" ,"Lunge"},
                                          { "Medium",  "Medium", "VeryFar" ,"Lunge"},
                                          { "Medium",  "High", "InRange" ,"Attack"},
                                          { "Medium",  "High", "Close","Attack" },
                                          { "Medium",  "High", "Far" ,"Lunge" },
                                          { "Medium",  "High", "VeryFar","Lunge" },
                                          { "High",  "VeryLow", "InRange" ,"Attack"},
                                          { "High",  "VeryLow", "Close" ,"Attack"},
                                          { "High",  "VeryLow", "Far" ,"Lunge"},
                                          { "High",  "VeryLow", "VeryFar","Lunge" },
                                          { "High",  "Low", "InRange" ,"Attack"},
                                          { "High",  "Low", "Close" ,"Attack"},
                                          { "High",  "Low", "Far" ,"Lunge" },
                                          { "High",  "Low", "VeryFar" ,"Lunge"},
                                          { "High",  "Medium", "InRange","Attack" },
                                          { "High",  "Medium", "Close" ,"Attack" },
                                          { "High",  "Medium", "Far","Lunge" },
                                          { "High",  "Medium", "VeryFar" ,"Lunge"},
                                          { "High",  "High", "InRange" ,"Attack"},
                                          { "High",  "High", "Close","Lunge" },
                                          { "High",  "High", "Far","Lunge" },
                                          { "High",  "High", "VeryFar","Lunge" }};




}
