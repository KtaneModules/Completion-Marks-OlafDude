using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;
using System.Text.RegularExpressions; 

public class CompletionMarksScript: MonoBehaviour {

	//globals
    public KMBombInfo bomb;
    public KMAudio Audio;

    //logging
    static int moduleIdCounter = 1;
    int moduleId;
	private List<string> chosenItems = new List<string>(); 
	bool moduleSolved = false; 
	bool[] selected;

	//selectables
	public KMSelectable change; 
	public KMSelectable[] marks;
	public KMSelectable[] scaleBut;
	public KMSelectable d4; 

	//sprites
	public Sprite bas; 
	public Sprite[] total;
	public Sprite[] mouth;
	public Sprite[] left; 
	public Sprite[] right; 
	public Sprite[] both; 
	public Sprite[] forehead; 
	public Sprite[] side; 
	public Sprite[] foreheadBlocked; 
	public Sprite[] body; 
	public Sprite[] markSprites; 
	public Sprite note; 
	public Sprite modelSpr; 
	public Sprite[] charges;

	//sprite renderers
	public SpriteRenderer basDisp; 
	public SpriteRenderer totalDisp;
	public SpriteRenderer mouthDisp; 
	public SpriteRenderer leftDisp;
	public SpriteRenderer rightDisp;
	public SpriteRenderer foreheadDisp;
	public SpriteRenderer sideDisp;
	public SpriteRenderer topDisp;
	public SpriteRenderer bodyDisp; 
	public SpriteRenderer modelDisp; 
	public SpriteRenderer chargeDisp; 

	//text
	public TextMesh number; 
	
	//data
	private Vector3 sca = new Vector3(1f,1f,1f); 
	private object[][] bodyDat;
	private object[][] mouDat; 
	private object[][] totDat;
	private object[][] sideDat;
	private object[][] foreDat; 
	private object[][] topDat; 
	private object[][] lDat; 
	private object[][] rDat; 
	private object[][] bDat; 
	private string[] bossNames = {"The Beast","It Lives!", "Isaac","Satan","Mother","???","The Lamb","Mega Satan","Delirium","Boss Rush","Hush","Ultra Greedier"};

	//indexes
	private int bodInd; 
	private int mouInd; 
	private int totInd; 
	private int sideInd; 
	private int foreInd; 
	private int topInd; 
	private int lInd;
	private int rInd; 
	private int bInd;  
	private int viewInd = 2;
	private int[] scales = {1, -1}; 
	private Vector3 scaleInc = new Vector3(0.01f, 0.01f, 0f);
	private int count = 10; 
	private int bossInd; 
	private int pressInd = 0; 
	private int[] corOrder; 
	private int power = 0;
	private int solves = 0; 
	private int chargeInd = 0; 

	//bools
	private bool bobCond;
	private bool rushCond;
	private bool hushCond; 

	void Awake(){
		//exceldata
		moduleId = moduleIdCounter++;
		scaleBut[0].OnInteract += delegate() {ScalePress(0); return false;}; 
		scaleBut[1].OnInteract += delegate() {ScalePress(1); return false;}; 
		marks[0].OnHighlight += delegate() {MarkHover(0);};
		marks[0].OnHighlightEnded += delegate() {MarkHoverEnd(0);};
		marks[1].OnHighlight += delegate() {MarkHover(1);};
		marks[1].OnHighlightEnded += delegate() {MarkHoverEnd(1);};
		marks[2].OnHighlight += delegate() {MarkHover(2);};
		marks[2].OnHighlightEnded += delegate() {MarkHoverEnd(2);};
		marks[3].OnHighlight += delegate() {MarkHover(3);};
		marks[3].OnHighlightEnded += delegate() {MarkHoverEnd(3);};
		marks[4].OnHighlight += delegate() {MarkHover(4);};
		marks[4].OnHighlightEnded += delegate() {MarkHoverEnd(4);};
		marks[5].OnHighlight += delegate() {MarkHover(5);};
		marks[5].OnHighlightEnded += delegate() {MarkHoverEnd(5);};
		marks[6].OnHighlight += delegate() {MarkHover(6);};
		marks[6].OnHighlightEnded += delegate() {MarkHoverEnd(6);};
		marks[7].OnHighlight += delegate() {MarkHover(7);};
		marks[7].OnHighlightEnded += delegate() {MarkHoverEnd(7);};
		marks[8].OnHighlight += delegate() {MarkHover(8);};
		marks[8].OnHighlightEnded += delegate() {MarkHoverEnd(8);};
		marks[9].OnHighlight += delegate() {MarkHover(9);};
		marks[9].OnHighlightEnded += delegate() {MarkHoverEnd(9);};
		marks[10].OnHighlight += delegate() {MarkHover(10);};
		marks[10].OnHighlightEnded += delegate() {MarkHoverEnd(10);};
		marks[11].OnHighlight += delegate() {MarkHover(11);};
		marks[11].OnHighlightEnded += delegate() {MarkHoverEnd(11);};
		marks[0].OnInteract += delegate () { MarkPress(0); return false; };
		marks[1].OnInteract += delegate () { MarkPress(1); return false; };
		marks[2].OnInteract += delegate () { MarkPress(2); return false; };
		marks[3].OnInteract += delegate () { MarkPress(3); return false; };
		marks[4].OnInteract += delegate () { MarkPress(4); return false; };
		marks[5].OnInteract += delegate () { MarkPress(5); return false; };
		marks[6].OnInteract += delegate () { MarkPress(6); return false; };
		marks[7].OnInteract += delegate () { MarkPress(7); return false; };
		marks[8].OnInteract += delegate () { MarkPress(8); return false; };
		marks[9].OnInteract += delegate () { MarkPress(9); return false; };
		marks[10].OnInteract += delegate () { MarkPress(10); return false; };
		marks[11].OnInteract += delegate () { MarkPress(11); return false; };

		d4.OnInteract += delegate(){D4Press(); return false;}; 
		change.OnInteract += delegate(){ChangePress(); return false;}; 

		lDat = new object[left.Length][]; 
		rDat = new object[right.Length][]; 
		bDat = new object[both.Length][]; 
		bodyDat = new object[body.Length][];
		mouDat = new object[mouth.Length][]; 
		totDat = new object[total.Length][];
		sideDat = new object[side.Length][]; 
		foreDat = new object[forehead.Length][]; 
		topDat = new object[foreheadBlocked.Length][]; 
	
		bodyDat[0] = new object[]{"Aquarius",2, Pos(-0.029, -0.499), sca}; 
		bodyDat[1] = new object[]{"Blanket",2, new Vector3( 0f, -0.46f, 0f), sca}; 	
		bodyDat[2] = new object[]{"Box",2, new Vector3( 0f, -0.431f, 0f), sca}; 
		bodyDat[3] = new object[]{"Bucket of Lard",2, new Vector3( 0.01f, -0.45f, 0f), sca}; 
		bodyDat[4] = new object[]{"C Section",2, new Vector3( 0.01f, -0.44f, 0f), sca}; 
		bodyDat[5] = new object[]{"Cancer",2, new Vector3( 0f, -0.498f, 0f), sca}; 
		bodyDat[6] = new object[]{"Compound Fracture",2, new Vector3( -0.301f, -0.44f, 0f), sca}; 
		bodyDat[7] = new object[]{"Dirty Mind",2, new Vector3( 0.01f, -0.479f, 0f), sca}; 
		bodyDat[8] = new object[]{"Gnawed Leaf",2, Pos(-0.397, -0.557), Scale(2)}; 
		bodyDat[9] = new object[]{"Midas Touch",2, Pos(0, -0.489), sca}; 
		bodyDat[10] = new object[]{"Mom's Key",2, Pos(0.369, -0.498), sca}; 
		bodyDat[11] = new object[]{"Mom's Purse",2, Pos(-0.252, -0.537), sca};
		bodyDat[12] = new object[]{"Mr. Dolly",2, Pos(0.45, -0.5), sca}; 
		bodyDat[13] = new object[]{"Revelation",2, Pos(0, -0.636), Scale(1.4)}; 
		bodyDat[14] = new object[]{"Spirit Shackles",2, Pos(0.01, -0.519), sca}; 
		bodyDat[15] = new object[]{"Stop Watch",2, Pos(0.069, -0.432), sca}; 
		bodyDat[16] = new object[]{"Akeldama",2, Pos(0.015, -0.547), sca}; 
		bodyDat[17] = new object[]{"Consolation Prize",2, Pos(0.086, -0.433), sca};
		bodyDat[18] = new object[]{"Crack Jacks",2, Pos(-0.437, -0.471), sca}; 
		bodyDat[19] = new object[]{"Death's List",2, Pos(-0.437, -0.471), sca};  
		bodyDat[20] = new object[]{"Empty Vessel",2, Pos(-0.024, -0.415), sca}; 
		bodyDat[21] = new object[]{"Match Book",2, Pos(0.007, -0.376), sca}; 
		bodyDat[22] = new object[]{"PJs",2, Pos(0.009, -0.525), sca}; 
		bodyDat[23] = new object[]{"Shard of Glass",2, Pos(-0.035, -0.495), sca}; 
		bodyDat[24] = new object[]{"Soul Locket",2, Pos(0.003, -0.449), sca}; 
		bodyDat[25] = new object[]{"Tarot Cloth",2, Pos(0.012, -0.514), sca};  
		bodyDat[26] = new object[]{"Camo Undies",2, Pos(0.005, -0.477), sca}; 

		mouDat[0] = new object[]{"2Spooky",5,Pos(0, -0.023), sca}; 
		mouDat[1] = new object[]{"Almond Milk",5,Pos(-0.04, -0.084), sca}; 
		mouDat[2] = new object[]{"Apple",5,Pos(-0.13,-0.035), sca}; 
		mouDat[3] = new object[]{"Binky",5,Pos(0.009, -0.084), sca}; 
		mouDat[4] = new object[]{"Blood Bag",5,Pos(0.009, 0.191), sca}; 
		mouDat[5] = new object[]{"Bozo",5,Pos(0, 0.076), sca}; 
		mouDat[6] = new object[]{"Chaos",5,Pos(0.024, -0.085), sca}; 
		mouDat[7] = new object[]{"Charm of the Vampire",5,Pos(0.011, -0.072), sca}; 
		mouDat[8] = new object[]{"Common Cold",5,Pos(0,0.005), sca}; 
		mouDat[9] = new object[]{"Cricket's Head",5,Pos(0.005, -0.034), sca}; 
		mouDat[10] = new object[]{"Dead Onion",5,Pos(-0.019, -0.036), sca}; 
		mouDat[11] = new object[]{"Dead Tooth",5,Pos(0.03, -0.055), sca}; 
		mouDat[12] = new object[]{"Dog Tooth",5,Pos(0.025, -0.078), sca}; 
		mouDat[13] = new object[]{"False PHD",5,Pos(-0.021, -0.095), sca}; 
		mouDat[14] = new object[]{"Ghost Pepper",5,Pos(-0.012, -0.003), sca}; 
		mouDat[15] = new object[]{"Jesus Juice",5,Pos(0.029, -0.016), sca}; 
		mouDat[16] = new object[]{"Knockout Drops",5,Pos(0.01, -0.022), sca}; 
		mouDat[17] = new object[]{"Mars",5,Pos(0, -0.056), sca}; 
		mouDat[18] = new object[]{"Maw of the Void",5,Pos(-0.006, -0.086), sca}; 
		mouDat[19] = new object[]{"Mom's Lipstick",5,Pos(0, 0.021), sca}; 
		mouDat[20] = new object[]{"Monstro's Lung",5,Pos(0.012, -0.028), sca}; 
		mouDat[21] = new object[]{"Mutant Spider",5,Pos(0.012, 0.019), sca}; 
		mouDat[22] = new object[]{"Neptunus",5,Pos(0.01, -0.047), sca}; 
		mouDat[23] = new object[]{"PHD",5,Pos(0.008, -0.018), sca}; 
		mouDat[24] = new object[]{"Placenta",5,Pos(0.008, -0.026), sca}; 
		mouDat[25] = new object[]{"Playdough Cookie",5,Pos(0.014, 0.107), sca}; 
		mouDat[26] = new object[]{"Red Stew",5,Pos(0.01, -0.028), sca}; 
		mouDat[27] = new object[]{"Roid Rage",5,Pos(0.007, -0.073), sca}; 
		mouDat[28] = new object[]{"Rotten Tomato",5,Pos(0.007, 0.103), sca}; 
		mouDat[29] = new object[]{"Sausage",5,Pos(0.013, 0), sca}; 
		mouDat[30] = new object[]{"Serpent's Kiss",5,Pos(-0.019, -0.09), sca}; 
		mouDat[31] = new object[]{"Sinus Infection",5,Pos(0.01, -0.018), sca}; 
		mouDat[32] = new object[]{"Soy Milk",5,Pos(0.015, -0.021), sca}; 
		mouDat[33] = new object[]{"Speed Ball",5,Pos(0, -0.048), sca}; 
		mouDat[34] = new object[]{"Spirit of the Night",5,Pos(0, -0.073), sca}; 
		mouDat[35] = new object[]{"Tough Love",5,Pos(0.041, 0.038), sca}; 
		mouDat[36] = new object[]{"Ipecac",5,Pos(0.03, -0.107), sca}; 
		mouDat[37] = new object[]{"It Hurts",5,Pos(0.004, -0.009), sca}; 
		mouDat[38] = new object[]{"Pyromaniac",5,Pos(0.004, -0.106), sca};
		mouDat[39] = new object[]{"Whore of Babylon",5,Pos(0, -0.11), Scale(1.7)};  

		totDat[0] = new object[]{"2Spooky", 1, Pos(0.011, -0.021), Scale(1)}; 
		totDat[1] = new object[]{"Abaddon", 1, Pos(-0.034, 0.226), Scale(1.3)}; 
		totDat[2] = new object[]{"Anti-Gravity", 1, Pos(-0.031, 0.015), Scale(1)};  
		totDat[3] = new object[]{"Bob's Curse", 1, Pos(0.022, 0.004), Scale(1)};  
		totDat[4] = new object[]{"Dead Dove", 1, Pos(0.01, -0.047), Scale(1)}; 
		totDat[5] = new object[]{"Jupiter", 1, Pos(0.019, -0.126), Scale(1.3)}; 
		totDat[6] = new object[]{"Libra", 1, Pos(0.005, 0.018), Scale(1)};  
		totDat[7] = new object[]{"Mars", 1, Pos(0.037, 0.055), Scale(1)}; 
		totDat[8] = new object[]{"Maw of the Void", 1, Pos(0.019, 0.083), Scale(1.15)}; 
		totDat[9] = new object[]{"Neptunus", 1, Pos(0, 0.009), Scale(1)}; 
		totDat[10] = new object[]{"Spirit of the Night", 1, Pos(0, -0.08), Scale(1)}; 
		totDat[11] = new object[]{"Transcendence", 1, Pos(-0.018, 0.19), Scale(1)}; 
		totDat[12] = new object[]{"Terra", 1, Pos(0, 0.005), sca};

		sideDat[0] = new object[]{"9 Volt", 8, Pos(0.253, 0.574), sca}; 
		sideDat[1] = new object[]{"120 Volt", 8, Pos(0.49, 0.65), sca}; 
		sideDat[2] = new object[]{"Battery Pack", 8, Pos(-0.688, 0.642), Scale(1.25) }; 
		sideDat[3] = new object[]{"The Battery", 8, Pos(0, 0.011), Scale(1.35)};
		sideDat[4] = new object[]{"Black Lotus", 10, Pos(-0.543, 0.477), Scale(1.1)}; 
		sideDat[5] = new object[]{"Boom!", 8, Pos(-0.348, 0.81), sca}; 
		sideDat[6] = new object[]{"Bozo", 10, Pos(-0.348, 0.819), sca}; 
		sideDat[7] = new object[]{"Dead Bird", 10, Pos(0.389, 0.662), Scale(1.1)}; 
		sideDat[8] = new object[]{"Jumper Cables", 8, Pos(0, 0.743), sca}; 
		sideDat[9] = new object[]{"Mercurius", 0, Pos(0.028, 0.447), Scale(2)}; 
		sideDat[10] = new object[]{"Pageant Boy", 10, Pos(-0.528, 0.556), sca}; 
		sideDat[11] = new object[]{"Pentagram", 10, Pos(-0.019, 0.614), sca}; 
		sideDat[12] = new object[]{"Purity", 10, Pos(-0.349, 0.565), sca}; 
		sideDat[13] = new object[]{"Sharp Plug", 3, Pos(-0.611, 0.187), Scale(1.2)}; 
		sideDat[14] = new object[]{"Small Rock", 10, Pos(-0.329, 0.441), sca}; 
		sideDat[15] = new object[]{"Spider Baby", 10, Pos(-0.438, 0.546), sca}; 
		sideDat[16] = new object[]{"Steven", 10, Pos(-0.399, 0.672), sca};
		sideDat[17] = new object[]{"Voodoo Head", 8, Pos(-0.041, 0.17), Scale(1.5)}; 
		sideDat[18] = new object[]{"Wire Coat Hanger", 8, Pos(0.019, 0.362), Scale(1.1)}; 
		sideDat[19] = new object[]{"Brimstone", 10, Pos(0.01, 0.545), Scale(2)};
		sideDat[20] = new object[]{"Lord of the Pit", 0, Pos(0, -0.179), Scale(2.1)};
		sideDat[21] = new object[]{"Marrow", 8, Pos(0, 0.47), Scale(1.37)};
		sideDat[22] = new object[]{"Magic Scab",7, Pos(-0.35, -0.138), sca}; 

		foreDat[0] = new object[]{"Anti-Gravity",9, Pos(0, 0.393),sca};
		foreDat[1] = new object[]{"BFFS!",9, Pos(-0.03, 0.42),sca};
		foreDat[2] = new object[]{"Bomber Boy",9, Pos(0.019, 0.85),sca};
		foreDat[3] = new object[]{"Charm of the Vampire",9, Pos(0.01, 0.567),sca};
		foreDat[4] = new object[]{"Cracked Orb",9, Pos(0, 0.393),sca};
		foreDat[5] = new object[]{"Eucharist",9, Pos(0, 0.377),sca};
		foreDat[6] = new object[]{"Eye of Belial",9, Pos(0, 0.412),sca};
		foreDat[7] = new object[]{"Flat Stone",9, Pos(0.06, 0.391),sca};
		foreDat[8] = new object[]{"Godhead",9, Pos(0, 0.393),sca};
		foreDat[9] = new object[]{"The Inner Eye",9, Pos(0.01, 0.312),sca};
		foreDat[10] = new object[]{"Iron Bar",9, Pos(0.01, 0.359),sca};
		foreDat[11] = new object[]{"Little Horn",9, Pos(0.007, 0.746),sca};
		foreDat[12] = new object[]{"Lodestone",9, Pos(0, 0.525),sca};
		foreDat[13] = new object[]{"Luna",9, Pos(0, 0.393),sca};
		foreDat[14] = new object[]{"The Mark",9, Pos(0, 0.451),sca};
		foreDat[15] = new object[]{"Marked",9, Pos(0.01, 0.424),sca};
		foreDat[16] = new object[]{"More Options",9, Pos(0, 0.422),sca};
		foreDat[17] = new object[]{"Mutant Spider",9, Pos(0, 0.335),sca};
		foreDat[18] = new object[]{"My Reflection",9, Pos(0, 0.384),sca};
		foreDat[19] = new object[]{"Polyphemus",4, Pos(0, 0.102),sca};
		foreDat[20] = new object[]{"Skeleton Key",9, Pos(0, 0.384),sca};
		foreDat[21] = new object[]{"Starter Deck",9, Pos(0, 0.431),sca};
		foreDat[22] = new object[]{"Tech.5",9, Pos(-0.057, 0.441),sca};
		foreDat[23] = new object[]{"There's Options",9, Pos(0, 0.422),sca};
		foreDat[24] = new object[]{"Tiny Planet",9, Pos(0.01, 0.557),sca};
		foreDat[25] = new object[]{"Trisagion",9, Pos(0, 0.47),sca};
		foreDat[26] = new object[]{"Virgo",9, Pos(-0.019, 0.704),sca};
		foreDat[27] = new object[]{"Zodiac",9, Pos(0.005, 0.519),sca};
		foreDat[28] = new object[]{"Options?", 9, Pos(0, 0.422), sca}; 
		foreDat[29] = new object[]{"Evil Eye", 9, Pos(0, 0.34), sca}; 
		foreDat[30] = new object[]{"Whore of Babylon", 9, Pos(0, 0.46), Scale(1.7)}; 
		foreDat[31] = new object[]{"Large Zit",9, Pos(-0.126, 0.35), sca}; 
		foreDat[32] = new object[]{"Black Candle", 9, Pos(0, 0.966), Scale(1)}; 
		foreDat[33] = new object[]{"Curse of the Tower", 9, Pos(0, 0.55), Scale(1)}; 
		foreDat[34] = new object[]{"The Parasite", 9, Pos(-0.02, 0.888), Scale(1)}; 
		foreDat[35] = new object[]{"Cat-O-Nine-Tails",9, Pos(-0.042, 0.223), sca};
		foreDat[36] = new object[]{"E. Coli",9, Pos(-0.016, 0.249), sca}; 
		foreDat[37] = new object[]{"Lump of Coal",9, Pos(-0.016, 0.181), sca}; 
		foreDat[38] = new object[]{"Wooden Spoon",9, Pos(0.027, 0.259), sca};
		
		topDat[0] = new object[]{"Bloody Lust", 11, Pos(0, 0.38), Scale(1)}; 
		topDat[1] = new object[]{"Broken Modem", 11, Pos(-0.01, 0.427), Scale(1)}; 
		topDat[2] = new object[]{"Cupid's Arrow", 11, Pos(0.039, 0.752), Scale(1)}; 
		topDat[3] = new object[]{"Goat Head", 11, Pos(-0.02, 0.423), Scale(1.3)}; 
		topDat[4] = new object[]{"Magic 8 Ball", 11, Pos(-0.009, 0.487), Scale(1)}; 
		topDat[5] = new object[]{"Magneto", 11, Pos(0.01, 0.771), Scale(1)}; 
		topDat[6] = new object[]{"Mom's Wig", 11, Pos(-0.044, 0.362), Scale(1.25)}; 
		topDat[7] = new object[]{"Skatole", 11, Pos(-0.04, 0.801), Scale(1)}; 
		topDat[8] = new object[]{"Spelunker Hat", 11, Pos(0.02, 0.607), Scale(1)}; 
		topDat[9] = new object[]{"Strange Attractor", 11, Pos(0.019, 0.567), Scale(1)}; 
		topDat[10] = new object[]{"Venus", 11, Pos(-0.038, 0.035), Scale(1.3)};  
		topDat[11] = new object[]{"Super Bandage", 11, Pos(0.056, 0.556), sca}; 
		topDat[12] = new object[]{"Metal Plate", 11, Pos(-0.245, 0.521), sca};
		topDat[13] = new object[]{"Card Reader", 11, Pos(-0.038, 0.16), Scale(1.2)};
		topDat[14] = new object[]{"The Wiz", 11, Pos(-0.352, 0.742), sca};
		topDat[15] = new object[]{"Dr. Fetus", 11, Pos(-0.245, 0.856), sca}; 
		topDat[16] = new object[]{"Blood of the Martyr", 11, Pos(0.014, 0.799), sca};
		topDat[17] = new object[]{"Halo", 11, Pos(0.014, 0.799), sca}; 

		lDat[0] = new object[]{"Almond Milk",4, Pos(-0.321,-0.036), sca};
		lDat[1] = new object[]{"Bob's Curse",4, Pos(-0.334, 0.009), sca}; 
		lDat[2] = new object[]{"Cursed Eye",4, Pos(-0.24, 0.119), sca}; 
		lDat[3] = new object[]{"Dead Eye",4, Pos(-0.317, 0.042), sca}; 
		lDat[4] = new object[]{"Glass Eye",4, Pos(-0.308, 0.034), sca}; 
		lDat[5] = new object[]{"Lead Pencil",4, Pos(-0.428, 0.017), sca}; 
		lDat[6] = new object[]{"Mom's Contact",4, Pos(-0.308, 0.059), sca}; 
		lDat[7] = new object[]{"SMB Super Fan",4, Pos(-0.318, 0.127), sca}; 
		lDat[8] = new object[]{"Stye",4, Pos(-0.327, 0.033), sca}; 
		lDat[9] = new object[]{"Technology 2",6, Pos(-0.439, 0.119), Scale(1.07)};
		lDat[10] = new object[]{"Anemic",4, Pos(-0.226, 0.189), sca};

		rDat[0] = new object[]{"Bird's Eye",4, Pos(0.246, 0.127), sca}; 
		rDat[1] = new object[]{"Blood Clot",4, Pos(0.358, 0.058), sca}; 
		rDat[2] = new object[]{"Eye of Greed",4, Pos(0.333, 0.049), sca}; 
		rDat[3] = new object[]{"Guppy's Eye",4, Pos(0.338, 0.037), sca}; 
		rDat[4] = new object[]{"Lost Contact",4, Pos(0.356, 0.055), sca}; 
		rDat[5] = new object[]{"Mom's Eye",4, Pos(0.35, 0.023), sca}; 
		rDat[6] = new object[]{"Mom's Perfume",4, Pos(0.282, 0.1), sca}; 
		rDat[7] = new object[]{"Ocular Rift",6, Pos(0.376, 0.203), sca}; 
		rDat[8] = new object[]{"The Peeper",4, Pos(0.356, 0.055), sca}; 
		rDat[9] = new object[]{"Stapler",4, Pos(0.35, 0.023), sca}; 
		rDat[10] = new object[]{"Technology",6, Pos(0.41, 0.366), Scale(1.15)}; 
		rDat[11] = new object[]{"Tough Love",4, Pos(0.341, 0.026), sca}; 
		rDat[12] = new object[]{"The Wiz",4, Pos(0.392, 0.335), sca}; 
		rDat[13] = new object[]{"Dr. Fetus",6, Pos(0.332, 0.055), sca}; 
		rDat[14] = new object[]{"Chemical Peel",4, Pos(0.26, 0.194), sca}; 

		bDat[0] = new object[]{"2Spooky",4, Pos(-0.03, -0.043), sca}; 
		bDat[1] = new object[]{"Abaddon",4, Pos(0, 0.202), Scale(1.3)}; 
		bDat[2] = new object[]{"Ball of Tar",4, Pos(0, 0.124), sca}; 
		bDat[3] = new object[]{"Bozo",4, Pos(0.006, 0.075), Scale(1.1)}; 
		bDat[4] = new object[]{"Brimstone",4, Pos(0, 0.102), Scale(1.9)}; 
		bDat[5] = new object[]{"Chaos",4, Pos(0, 0.298), sca}; 
		bDat[6] = new object[]{"Common Cold",4, Pos(0, 0.147), sca}; 
		bDat[7] = new object[]{"Contract from Below",4, Pos(0.006, 0.016), sca};
		bDat[8] = new object[]{"Continuum",4, Pos(0.006, 0.159), sca};  
		bDat[9] = new object[]{"Cricket's Body",4, Pos(0, 0.008), sca}; 
		bDat[10] = new object[]{"Cricket's Head",4, Pos(0, 0.014), sca}; 
		bDat[11] = new object[]{"Dark Matter",4, Pos(0, 0.056), sca}; 
		bDat[12] = new object[]{"Eden's Blessing",4, Pos(0, 0.014), sca}; 
		bDat[13] = new object[]{"8 Inch Nails",4, Pos(0, 0.129), Scale(1.05)}; 
		bDat[14] = new object[]{"Eve's Mascara",4, Pos(0, -0.01), sca}; 
		bDat[15] = new object[]{"False PHD",4, Pos(0, 0.11), sca}; 
		bDat[16] = new object[]{"Fruit Cake",4, Pos(0, 0.009), sca};
		bDat[17] = new object[]{"Glaucoma",4, Pos(0, 0.009), sca}; 
		bDat[18] = new object[]{"God's Flesh",4, Pos(0, 0.207), sca}; 
		bDat[19] = new object[]{"Haemolacria",4, Pos(0, -0.016), sca}; 
		bDat[20] = new object[]{"Head of the Keeper",4, Pos(0, 0.033), sca}; 
		bDat[21] = new object[]{"Holy Light",4, Pos(0.007, 0.012), Scale(1.2)}; 
		bDat[22] = new object[]{"Judas' Shadow",4, Pos(0, 0.131), sca}; 
		bDat[23] = new object[]{"Jupiter",4, Pos(0, -0.009), Scale(1.3)}; 
		bDat[24] = new object[]{"Knockout Drops",4, Pos(0, 0.068), sca}; 
		bDat[25] = new object[]{"Maw of the Void",4, Pos(0, 0.222), Scale(1.15)}; 
		bDat[26] = new object[]{"Mom's Eyeshadow",4, Pos(0, 0.055), sca}; 
		bDat[27] = new object[]{"Mucormycosis",4, Pos(0, 0.132), sca}; 
		bDat[28] = new object[]{"Neptunus",4, Pos(0, 0.013), sca}; 
		bDat[29] = new object[]{"Pay to Play",4, Pos(0, 0.034), sca}; 
		bDat[30] = new object[]{"Pop!",4, Pos(0, 0.034), sca}; 
		bDat[31] = new object[]{"Proptosis",4, Pos(0, 0.055), sca}; 
		bDat[32] = new object[]{"Revelation",4, Pos(0, 0.02), Scale(1.4)}; 
		bDat[33] = new object[]{"Roid Rage",4, Pos(0, 0.083), sca}; 
		bDat[34] = new object[]{"Rotten Tomato",4, Pos(0.007, 0.131), sca}; 
		bDat[35] = new object[]{"Rubber Cement",4, Pos(0, 0.082), sca}; 
		bDat[36] = new object[]{"Sacred Heart",4, Pos(0, 0.033), sca}; 
		bDat[37] = new object[]{"Sausage",4, Pos(0, 0.138), sca}; 
		bDat[38] = new object[]{"Scorpio",4, Pos(0.014, 0.012), sca}; 
		bDat[39] = new object[]{"Speed Ball",4, Pos(0, 0.117), sca}; 
		bDat[40] = new object[]{"Spirit of the Night",4, Pos(0, 0.286), sca}; 
		bDat[41] = new object[]{"Squeezy",4, Pos(0, 0.037), sca}; 
		bDat[42] = new object[]{"Stigmata",4, Pos(0.005, 0.123), sca}; 
		bDat[43] = new object[]{"Sulfuric Acid",4, Pos(0.005, -0.008), sca}; 
		bDat[44] = new object[]{"Technology Zero",6, Pos(0.005, 0.044), Scale(1.07)}; 
		bDat[45] = new object[]{"Tooth Picks",4, Pos(-0.039, 0.149), sca}; 
		bDat[46] = new object[]{"Torn Photo",4, Pos(0, -0.014), sca}; 		
		bDat[47] = new object[]{"Trisagion",4, Pos(0, 0.019), sca}; 
		bDat[48] = new object[]{"Tropicamide",4, Pos(0.008, 0.057), sca}; 
		bDat[49] = new object[]{"Uranus",4, Pos(-0.012, 0.04), sca}; 
		bDat[50] = new object[]{"Venus",4, Pos(0.012, 0.096), Scale(1.3)}; 
		bDat[51] = new object[]{"4.5 Volt",4, Pos(0.003,0.028), Scale(1)}; 
		bDat[52] = new object[]{"Dad's Lost Coin",4, Pos(0.003, -0.02), sca}; 
		bDat[53] = new object[]{"Epiphora",4, Pos(0.003, 0.015), sca}; 
		bDat[54] = new object[]{"Ipecac",4, Pos(0.01, 0.085), sca}; 
		bDat[55] = new object[]{"Lachryphagy",4, Pos(0.004,-0.009), sca}; 
		bDat[56] = new object[]{"Pyromaniac",4, Pos(0.004,0.144), sca};
	}

	void Start () {
		for (int i = 0; i < 12; i++){
			if (i != 8){
				marks[i].GetComponent<SpriteRenderer>().sprite = null;
			}
        }
		pressInd = 0; 
		marks[8].GetComponent<SpriteRenderer>().sprite = note; 
		selected = new bool[12]; 
		chosenItems.Clear(); 
		chargeInd = 0;
		chargeDisp.sprite = charges[chargeInd]; 
		ChangePress(); 
		ChooseItems();
		FindSolution();
		if (!bobCond){
			FindInput(); 
		}
	}

	void ChooseItems(){
		basDisp.sprite = bas;
		int totCheck = UnityEngine.Random.Range(0, 3); 
		totInd = 1; 
		if (totCheck < 2){ 
			totInd = UnityEngine.Random.Range(0, totDat.Length); 
			totalDisp.sprite = total[totInd]; 
			//totalDisp.sortingLayerName = (string)totDat[totInd][1]; 
			totalDisp.gameObject.transform.localPosition = (Vector3)totDat[totInd][2]; 
			totalDisp.gameObject.transform.localScale = (Vector3)totDat[totInd][3]; 
			basDisp.sprite = null; 
			chosenItems.Add((string)(totDat[totInd][0])); 
			Debug.LogFormat("[Completion Marks #{0}] The body pickup is: {1}", moduleId, totDat[totInd][0]); 
		}
		else{
			totalDisp.sprite = null; 
		} 
		int[] excluded = {0, 4, 5, 10}; 
		int[] excluded2 = {3,4,5,6}; 
		int[] excluded3 = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14,16, 18, 19, 21, 22, 23, 25, 26}; 
		int[] tranCluded = {0, 3, 4, 5, 6, 7, 8, 9, 10, 11, 13, 14, 16, 18, 19, 21, 22, 23, 25, 26}; 
		bodInd = UnityEngine.Random.Range(0, bodyDat.Length);
		if (!excluded.Contains(totInd)){ 
			while (totInd == 11 && tranCluded.Contains(bodInd)){
				bodInd = UnityEngine.Random.Range(0, bodyDat.Length);
			} 
			while (totCheck < 2 && (excluded2.Contains(bodInd) || (totInd == 2 && bodInd == 0))){
				bodInd = UnityEngine.Random.Range(0, bodyDat.Length);
			}
		}
		else{
			while (excluded3.Contains(bodInd)){
				bodInd = UnityEngine.Random.Range(0, bodyDat.Length);
			} 
		}
		bodyDisp.sprite = body[bodInd]; 
		//bodyDisp.sortingLayerName = (string)bodyDat[bodInd][1]; 
		bodyDisp.sortingOrder = (int)bodyDat[bodInd][1]; 
		bodyDisp.gameObject.transform.localPosition = (Vector3)bodyDat[bodInd][2]; 
		bodyDisp.gameObject.transform.localScale = (Vector3)bodyDat[bodInd][3];
		chosenItems.Add((string)(bodyDat[bodInd][0]));  
		Debug.LogFormat("[Completion Marks #{0}] The body accessory pickup is: {1}", moduleId, bodyDat[bodInd][0]); 
		
		mouInd = UnityEngine.Random.Range(0, mouDat.Length); 
		while (chosenItems.Contains((string)(mouDat[mouInd][0]))){ 
			mouInd = UnityEngine.Random.Range(0, mouDat.Length); 
		}
		mouthDisp.sprite = mouth[mouInd]; 
		//mouthDisp.sortingLayerName = (string)mouDat[mouInd][1]; 
		mouthDisp.sortingOrder = (int)mouDat[mouInd][1]; 
		mouthDisp.gameObject.transform.localPosition = (Vector3)mouDat[mouInd][2]; 
		mouthDisp.gameObject.transform.localScale = (Vector3)mouDat[mouInd][3]; 
		chosenItems.Add((string)(mouDat[mouInd][0])); 
		Debug.LogFormat("[Completion Marks #{0}] The mouth pickup is: {1}", moduleId, mouDat[mouInd][0]); 

		int topCheck = UnityEngine.Random.Range(0, 3); 
		if (topCheck < 2){
			topDisp.sprite = null; 
			foreInd = UnityEngine.Random.Range(0, foreDat.Length); 
			while (chosenItems.Contains((string)(foreDat[foreInd][0]))){ 
				foreInd = UnityEngine.Random.Range(0, foreDat.Length); 
			}
			foreheadDisp.sprite = forehead[foreInd]; 
			//foreheadDisp.sortingLayerName = (string)foreDat[foreInd][1]; 
			foreheadDisp.sortingOrder = (int)foreDat[foreInd][1]; 
			foreheadDisp.gameObject.transform.localPosition = (Vector3)foreDat[foreInd][2]; 
			foreheadDisp.gameObject.transform.localScale = (Vector3)foreDat[foreInd][3]; 
			chosenItems.Add((string)(foreDat[foreInd][0])); 
			Debug.LogFormat("[Completion Marks #{0}] The forehead pickup is: {1}", moduleId, foreDat[foreInd][0]);  

			sideInd = UnityEngine.Random.Range(0, sideDat.Length);
			while (chosenItems.Contains((string)(sideDat[sideInd][0]))){ 
				sideInd = UnityEngine.Random.Range(0, sideDat.Length); 
			}
			sideDisp.sprite = side[sideInd]; 
			//sideDisp.sortingLayerName = (string)sideDat[sideInd][1]; 
			sideDisp.sortingOrder = (int)sideDat[sideInd][1]; 
			sideDisp.gameObject.transform.localPosition = (Vector3)sideDat[sideInd][2]; 
			sideDisp.gameObject.transform.localScale = (Vector3)sideDat[sideInd][3]; 
			chosenItems.Add((string)(sideDat[sideInd][0])); 
			Debug.LogFormat("[Completion Marks #{0}] The side of the head pickup is: {1}", moduleId, sideDat[sideInd][0]); 
		}
		else {
			foreheadDisp.sprite = null; 
			sideDisp.sprite = null; 
			topInd = UnityEngine.Random.Range(0, topDat.Length); 
			while (chosenItems.Contains((string)(topDat[topInd][0]))){ 
				topInd = UnityEngine.Random.Range(0, topDat.Length); 
			}
			topDisp.sprite = foreheadBlocked[topInd]; 
			//topDisp.sortingLayerName = (string)topDat[topInd][1]; 
			topDisp.sortingOrder = (int)topDat[topInd][1]; 
			topDisp.gameObject.transform.localPosition = (Vector3)topDat[topInd][2]; 
			topDisp.gameObject.transform.localScale = (Vector3)topDat[topInd][3]; 
			chosenItems.Add((string)(topDat[topInd][0])); 
			Debug.LogFormat("[Completion Marks #{0}] The head accessory pickup is: {1}", moduleId, topDat[topInd][0]);  
		}

		int eCheck = UnityEngine.Random.Range(0, lDat.Length + rDat.Length + 2*bDat.Length); 
		if (foreInd == 19){ 
			leftDisp.sprite = null; 
			rightDisp.sprite = null; 
		}
		else if (eCheck < 2*bDat.Length){
			rightDisp.sprite = null; 
			bInd = UnityEngine.Random.Range(0, bDat.Length);
			while (chosenItems.Contains((string)(bDat[bInd][0]))){
				bInd =  UnityEngine.Random.Range(0, bDat.Length);
			}
			leftDisp.sprite = both[bInd]; 
			//leftDisp.sortingLayerName = (string)bDat[bInd][1];
			leftDisp.sortingOrder = (int)bDat[bInd][1]; 
			leftDisp.gameObject.transform.localPosition = (Vector3)bDat[bInd][2]; 
			leftDisp.gameObject.transform.localScale = (Vector3)bDat[bInd][3]; 
			chosenItems.Add((string)(bDat[bInd][0])); 
			Debug.LogFormat("[Completion Marks #{0}] The both eyes pickup is: {1}", moduleId, bDat[bInd][0]);  
		}
		else{ //left eye
			lInd = UnityEngine.Random.Range(0, lDat.Length); 
			while ((totInd == 6 && lInd == 10) || chosenItems.Contains((string)(lDat[lInd][0]))){ 
				lInd = UnityEngine.Random.Range(0, lDat.Length); 
			}
			leftDisp.sprite = left[lInd]; 
			//leftDisp.sortingLayerName = (string)lDat[lInd][1]; 
			leftDisp.sortingOrder = (int)lDat[lInd][1]; 
			leftDisp.gameObject.transform.localPosition = (Vector3)lDat[lInd][2]; 
			leftDisp.gameObject.transform.localScale = (Vector3)lDat[lInd][3]; 
			chosenItems.Add((string)(lDat[lInd][0])); 
			Debug.LogFormat("[Completion Marks #{0}] The left eye pickup is: {1}", moduleId, lDat[lInd][0]); 
				
			//right eye
			rInd = UnityEngine.Random.Range(0, rDat.Length); 
			while ((totInd == 6 && rInd == 14) ||chosenItems.Contains((string)(rDat[rInd][0]))){ 
				rInd = UnityEngine.Random.Range(0, rDat.Length); 
			}
			rightDisp.sprite = right[rInd]; 
			//rightDisp.sortingLayerName = (string)rDat[rInd][1];
			rightDisp.sortingOrder = (int)rDat[rInd][1]; 
			rightDisp.gameObject.transform.localPosition = (Vector3)rDat[rInd][2]; 
			rightDisp.gameObject.transform.localScale = (Vector3)rDat[rInd][3];
			chosenItems.Add((string)(rDat[rInd][0]));
			Debug.LogFormat("[Completion Marks #{0}] The right eye pickup is: {1}", moduleId, rDat[rInd][0]); 
		}
		number.text = chosenItems.Count.ToString(); 
		Debug.LogFormat("[Completion Marks #{0}] There are {1} total pickups", moduleId, chosenItems.Count); 
	}
	void FindSolution(){
		if (bomb.IsIndicatorPresent("BOB") && chosenItems.Contains("Bob's Curse")){
			bobCond = true;
			Debug.LogFormat("[Completion Marks #{0}] Isaac, huh, more like Bob! Mark all symbols in the personal note.", moduleId); 
		}
		else if((bomb.GetIndicators().Count() > 1) && (chosenItems.Contains("Eye of Greed") || chosenItems.Contains("Head of the Keeper") || chosenItems.Contains("Pay to Play") || chosenItems.Contains("Dad's Lost Coin"))){
			bossInd = 11; 
			Debug.LogFormat("[Completion Marks #{0}] Isaac is playing Greed mode.", moduleId);
		}
		else{
			FindPower(); 
		}
	}

	void FindPower(){
		int sum = 0; 
		string[] all = new string[chosenItems.Count]; 
		for (int i = 0; i < chosenItems.Count; i++){
			all[i] = Regex.Replace(chosenItems[i], @"[^a-zA-Z0-9]", "").ToLower(); 
			sum += all[i].Length; 
			Debug.LogFormat("[Completion Marks #{0}] Pickup #{1}'s modified string label is: {2}", moduleId, i+1, all[i]); 
		} 
		int sum2 = sum%20; 
		Debug.LogFormat("[Completion Marks #{0}] There are {1} total characters after removing non-letter/non-number characters, so find character {2} in each string", moduleId, sum, sum2 +1);  
		char[] chars = new char[all.Length]; 
		int[] nums = new int[all.Length]; 
		
		for (int i = 0; i < all.Length; i++){ 
			chars[i] = all[i][sum2%(all[i].Length)]; 
			Debug.LogFormat("[Completion Marks #{0}] The character in position {1} for pickup #{2} is {3}", moduleId, sum2+1, i+1, chars[i]); 
			if (char.IsLetter(chars[i])){
				nums[i] = (char)(char.ToUpper(chars[i]) - '@');
			}
			else{
				nums[i] = (int)char.GetNumericValue(chars[i]); 
			}
			power += nums[i];  
		} 
		power = power % 26 + 1; 
		if (power < 3){bossInd = 0;}
		else if (3 <= power && power < 6){bossInd = 1;}
		else if (6 <= power && power < 9){bossInd = 2;}
		else if (9 <= power && power < 12){bossInd = 3;}
		else if (12 <= power && power < 15){bossInd = 4;}
		else if (15 <= power && power < 18){bossInd = 5;}
		else if (18 <= power && power < 21){bossInd = 6;}
		else if (21 <= power && power < 24){bossInd = 7;}
		else{bossInd = 8;}
		
		Debug.LogFormat("[Completion Marks #{0}] Isaac's power is {1}, so the final boss is {2}", moduleId, power, bossNames[bossInd]); 
	}

	void ScalePress(int ind){
		if (count + scales[ind] == 5 || count + scales[ind] == 13){
			return; 
		}
		else{
			count += scales[ind]; 
			Vector3 temp = basDisp.gameObject.transform.localScale; 
			temp += scales[ind]*scaleInc; 
			basDisp.gameObject.transform.localScale = temp;
			if (ind == 1){
				Audio.PlaySoundAtTransform("small", transform);
			}
			else{
				Audio.PlaySoundAtTransform("large", transform);
			}
		}
	}

	void ChangePress(){
		viewInd++; 
		viewInd = viewInd % 3; 
		switch (viewInd){
			case 0:
				d4.gameObject.SetActive(true); 
				modelDisp.gameObject.SetActive(true); 
				marks[8].gameObject.SetActive(false); 
				basDisp.gameObject.SetActive(false); 
				scaleBut[0].gameObject.SetActive(false); 
				scaleBut[1].gameObject.SetActive(false); 
			break;
			case 1: 
				d4.gameObject.SetActive(false);
				modelDisp.gameObject.SetActive(false); 
				marks[8].gameObject.SetActive(false); 
				basDisp.gameObject.SetActive(true);
				scaleBut[0].gameObject.SetActive(true); 
				scaleBut[1].gameObject.SetActive(true); 
			break;
			case 2:
				d4.gameObject.SetActive(false);
				modelDisp.gameObject.SetActive(false); 
				marks[8].gameObject.SetActive(true); 
				basDisp.gameObject.SetActive(false);
				scaleBut[0].gameObject.SetActive(false); 
				scaleBut[1].gameObject.SetActive(false); 
			break; 
		}
	}

	void FindInput(){
		if (chosenItems.Count == 4 && bomb.GetBatteryCount() == 4){
			rushCond = true; 
			Debug.LogFormat("[Completion Marks #{0}] Isaac can beat Boss Rush.", moduleId);
		}
		if (bossInd != 0 && bossInd != 4 && (bomb.GetSerialNumberLetters().Any(x => x == 'S' || x == 'I' || x == 'L' || x == 'E' || x == 'N' || x == 'C') || bomb.GetSerialNumberNumbers().Any(x => x == 3))){
			hushCond = true;
			Debug.LogFormat("[Completion Marks #{0}] Isaac can beat Hush.", moduleId);
		}
		List<int> entries = new List<int>(); 
		if (bossInd == 11){entries.Add(11);}
		else{
			if (rushCond){entries.Add(9);}
			if (bossInd == 0){
				entries.Add(0);
				if (hushCond){entries.Add(10);}
				}
			else{
				entries.Add(1); 
				if (hushCond){entries.Add(10);}
				switch (bossInd){
					case 2:
						entries.Add(2);
					break;
					case 3:
						entries.Add(3); 
					break;
					case 4:	
						entries.Add(4);
					break;
					case 5:
						entries.Add(2);
						entries.Add(5);
					break;
					case 6:
						entries.Add(3);
						entries.Add(6); 
					break;
					case 7:
						if (power%2==0){
							entries.Add(2); 
							entries.Add(5);
							entries.Add(7); 
						}
						else{
							entries.Add(3);
							entries.Add(6);
							entries.Add(7); 
						}
					break;
					case 8:
						if(power%2==0){
							entries.Add(2); 
							entries.Add(5);
							entries.Add(7); 
							entries.Add(8);
						}
						else{
							entries.Add(3);
							entries.Add(6);
							entries.Add(7); 
							entries.Add(8);
						}	
					break;
				}
			}
		}
		
		corOrder = entries.ToArray(); 
		string msg = ""; 
		for(int i = 0; i < corOrder.Length; i++){
			msg += " " + bossNames[corOrder[i]]; 
		}
		Debug.LogFormat("[Completion Marks #{0}] The correct press mark order is: {1}", moduleId, msg);
	}

	void MarkPress(int ind){
		if(selected[ind] || moduleSolved){
			return;
		}
		if (bobCond){
			Debug.LogFormat("[Completion Marks #{0}] Correctly pressed {1}", moduleId, bossNames[ind]);
			pressInd++; 
			marks[ind].GetComponent<SpriteRenderer>().sprite = markSprites[ind]; 
			selected[ind] = true;
			if (pressInd == bossNames.Length){
				Debug.LogFormat("[Completion Marks #{0}] Correctly marked all marks in order! Module solved.", moduleId);
				moduleSolved=true;
				Audio.PlaySoundAtTransform("success", transform);
				GetComponent<KMBombModule>().HandlePass();
			}
			if(!moduleSolved){Audio.PlaySoundAtTransform("stamp", transform);}
		}
		else if (corOrder[pressInd] == ind){
			Debug.LogFormat("[Completion Marks #{0}] Correctly pressed {1}", moduleId, bossNames[ind]);
			pressInd++; 
			marks[ind].GetComponent<SpriteRenderer>().sprite = markSprites[ind]; 
			selected[ind] = true;
			if (pressInd == corOrder.Length){
				Debug.LogFormat("[Completion Marks #{0}] Correctly marked all marks in order! Module solved.", moduleId);
				moduleSolved=true;
				Audio.PlaySoundAtTransform("success", transform);
				GetComponent<KMBombModule>().HandlePass();
			}
			if(!moduleSolved){Audio.PlaySoundAtTransform("stamp", transform);}
		}
		else{
			Debug.LogFormat("[Completion Marks #{0}] Incorrectly pressed {1}. The correct press is {2}.", moduleId, bossNames[ind], bossNames[corOrder[pressInd]]);
			Audio.PlaySoundAtTransform("pain", transform);
			GetComponent<KMBombModule>().HandleStrike();
		}
	}

	void MarkHover(int ind){
		if (moduleSolved || selected[ind]){
			
		}
		else{
			marks[ind].GetComponent<SpriteRenderer>().sprite = markSprites[ind];
		}
	}

	void MarkHoverEnd(int ind){
		if (moduleSolved || selected[ind]){
			
		}
		else{
			if (ind == 8){
				marks[ind].GetComponent<SpriteRenderer>().sprite = note; 
			}
			else{
				marks[ind].GetComponent<SpriteRenderer>().sprite = null;
			}
		}
	}
	void Update() {
        if (bomb.GetSolvedModuleIDs().Count != solves && !moduleSolved){
            solves = bomb.GetSolvedModuleIDs().Count;
			if (chargeInd != 3){
				chargeInd++; 
				chargeDisp.sprite = charges[chargeInd];  
			}
		}
	}

	void D4Press(){
		if (chargeInd != 3 || moduleSolved){
			return; 
		}
		Audio.PlaySoundAtTransform("d4", transform);
		Start(); 
	}

	Vector3 Pos(double x, double y){
		float a = (float)x;
		float b = (float)y; 
		return new Vector3(a, b, 0f); 
	}
	Vector3 Scale(double a){
		float b = (float)a;
		return new Vector3(b*sca.x, b*sca.y, sca.z); 
	}
}
