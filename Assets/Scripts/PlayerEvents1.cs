using UnityEngine;
using System.Collections;

public class PlayerEvents1 : MonoBehaviour {
	
	public Transform fire;
	public Transform ground;
	public Transform metal;
	public Transform water;
	public Transform wind;
	public Transform selector;
	public Transform newBlocksShader;
	public GUIText winLoseTxt;
	public GUIText counterNumberOfLinesClearedText;
	
	public int numberOfStartingBlocks=30;
	public int numberOfLinesToClearToWin=20;
	public int counterNumberOfLinesCleared=0;

	public float destroyBlocksFrames=0;
	public float fallingBlocksFrames=0;
	/***********controler variables***************************/
	public bool riseBlocks=true;//this bool checks if is ok to start rising the blocks
	public float riseSpeed=.1f;//this float tells us the speed at which the blocks rise each update
	public bool dying=false;//false when the player is not dying, true when the death timer has started
	public float deathTimer=0f;
	public float startOfDeathTimer=0f;
	public bool playerHasLost=false;
	public bool playerHasWon=false;
	
	public bool blockedBlock=false;
	
	
	
	public int positionHor;
	public int positionVer;
	
	
	Transform[,] blocks = new Transform[12,6];	
	Transform[] newBlocks=new Transform[6];
	public ArrayList fallingBlocks=new ArrayList();
	public ArrayList verifyDestroyable=new ArrayList();
	public ArrayList destroyableBlocks=new ArrayList();
	public ArrayList blockedBlocks=new ArrayList();
	
	// Use this for initialization
	void Start () {
		
	int countBlocks=0;
	

	for(int rows=0;rows<=11; rows++)
	{
		
		for(int cols=0;cols<=5; cols++)
		{
			int randomBlock = UnityEngine.Random.Range(1,6);
			//int randomBlock=1;
			
			if(countBlocks>=numberOfStartingBlocks)
			{
				cols=7;
				rows=12;
			}
			else
			{		
			//posiciones del player para facilitar el codigo del switch
			int posAuxX1=(int)transform.position.x+cols;
			int posAuxY1=(int)transform.position.y+rows;

			
			
			
					switch(randomBlock)
					{
						
					
						case 1:
									blocks[rows,cols] = (Transform)Instantiate(fire, new Vector3(posAuxX1,posAuxY1,15), Quaternion.identity);

									countBlocks+=1;
						break;
						case 2:
									blocks[rows,cols] = (Transform)Instantiate(ground, new Vector3(posAuxX1,posAuxY1,15), Quaternion.identity);

									  countBlocks+=1;
						break;
						case 3:
									blocks[rows,cols] = (Transform)Instantiate(metal, new Vector3(posAuxX1,posAuxY1,15), Quaternion.identity);

 									 countBlocks+=1;
						break;
						case 4:
									blocks[rows,cols] = (Transform)Instantiate(water, new Vector3(posAuxX1,posAuxY1,15), Quaternion.identity);

 									 countBlocks+=1;
						break;
						case 5:
									blocks[rows,cols] = (Transform)Instantiate(wind, new Vector3(posAuxX1,posAuxY1,15), Quaternion.identity);

 									 countBlocks+=1;
						break;
						case 6:
									blocks[rows,cols] = (Transform)Instantiate(ground, new Vector3(posAuxX1,posAuxY1,15), Quaternion.identity);

								 	countBlocks+=1;	
						break;
						default:
						break;
						
						
					}//switch
					
					
				
					
					
					
				}//else	
			}//for cols
	}//for rows
		
		
		createNewLineOfBlocks();
		
	
		
	}//start()
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	// Update is called once per frame
	void Update () {
	
	if(!playerHasLost && !playerHasWon)
		{
				
			if(dying)//when the dying variable has started counting
			{
				startOfDeathTimer = startOfDeathTimer + Time.deltaTime;//death counter to know when the player is going to die
//Debug.Log ("deathTimer: "+deathTimer+ " dying: "+dying +" start of Death Timer: "+startOfDeathTimer+" delta time "+Time.deltaTime);
	
				
				
				
				
				if(startOfDeathTimer >= 5)//when timer goes above 5 seconds the player has to die
				{
					playerDies();//function called when player dies after being 5 seconds with the top row with at least 1 block
				}
				
			}//if dying
			
			else if(dying==false)
			{
				deathTimer=0;
				
			}
			
	 
	  		
			
			if(Input.GetButtonDown("P1Rise"))//this button makes the stack rise faster 
			{
				riseSpeed=.05f;
			}
			if(Input.GetButtonUp ("P1Rise"))//when the button is released the stack stops going so fast
			{
				riseSpeed=.001f;
			}
				
			//if player swaps a block by pressing the P1Switch button
			if(Input.GetButtonDown("P1Switch")){
					//we send the position of the selector to the function turnBlocks which in turns will find the position inside the blocks array
					turnBlocks ((int)selector.transform.position.y,(int)selector.transform.position.x,(int)selector.transform.position.y,(int)selector.transform.position.x+1);
				
			}//if P1Switch ends
				
				
				
			//selector movements
			//******************************************************************************/
			if(Input.GetButtonDown("P1Left")){
			
				if(positionHor>0)
				{
				
					positionHor-=1;
					selector.transform.Translate( -1, 0,0);
				
				}
			}
			if(Input.GetButtonDown("P1Right")){
				if(positionHor<4)
				{
					positionHor+=1;
					selector.transform.Translate( 1,0,0);
				}
			}
			if(Input.GetButtonDown("P1Up")){
				if(positionVer<11)
				{
					positionVer+=1;
					selector.transform.Translate( 0, 1,0);
				}
			}
			if(Input.GetButtonDown("P1Down")){
				if(positionVer>0)
				{
					positionVer-=1;
					selector.transform.Translate( 0, -1,0);
				}
			}
			
			//******************************************************************************/	
			//selector movements ends
			
			
			
			
			verifyRiseBlocks();//we verify if we should rise the blocks or not
			
				
			if(riseBlocks)
			{
				rise();	
			}
					
			
			verifyDestroyableBlocks();
						
		if(destroyBlocksFrames>=.2)//these frames runs the destroy blocks in a more tetris attackish way
		{
				
			destroyBlocks();
			destroyBlocksFrames=0;//we reset the timer so that it waits .2 seconds to run this code again
		}
		
			
		if(fallingBlocksFrames>=.1 && destroyableBlocks.Count<=0)//this frames run the falling blocks algorithm every .1 seconds to make it more tetris attack like
		{
				
			applyGravityOnFallingBlocks();
			fallingBlocksFrames=0;//we reset the timer so that it waits .1 seconds to run this code again
		}
		
			
			
		}//player has lost
		else
		{
			if(playerHasWon)
			{
				winLoseTxt.text="You Won! :)";	
				
			}
			else if(playerHasLost)
			{
				winLoseTxt.text=("You Lost! :(");			}
			}
		
		
		fallingBlocksFrames+=Time.deltaTime;
		destroyBlocksFrames+=Time.deltaTime;
		
	}//end update
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	//function to swap blocks receives the x and y positions of the 2 blocks that we are going to swap
	void turnBlocks(int firstV,int firstH, int secondV,int secondH)
	{	
		
		foreach(int[] blocked in blockedBlocks)
		{
			if((blocked[0]==firstV&&blocked[1]==firstH)||(blocked[0]==secondV && blocked[1]==secondH)||((blocked[0]-1)==firstV&&blocked[1]==firstH)||((blocked[0]-1)==secondV && blocked[1]==secondH))
			{
				blockedBlock=true;
				break;
			}
						
		}
		
		foreach(int [] destroyable in destroyableBlocks)
		{
			if((destroyable[0]==firstV&&destroyable[1]==firstH)||(destroyable[0]==secondV&&destroyable[1]==secondH))
			{
				Debug.Log ("blocked to destroyed later:"+destroyable[0]+" "+destroyable[1]);

				blockedBlock=true;
				break;
			}
		}
		
		
		if(!blockedBlock)//if the block we try to swap is not blocked we proceed to swap it
		{
			//Debug.Log ("turning: "+firstV+" "+firstH+" "+secondV+" "+secondH);
			//when the selector has no blocks on either side
			if(blocks[secondV,secondH]==null && blocks[firstV,firstH]==null )
			{
	//Debug.Log ("both null"+firstV+" "+firstH+" "+secondV+" "+secondH);
				
			}//if !=null ends
			
			//when the selector has block in the second square and is null in the first square
			else if(blocks[firstV,firstH]==null)
			{
	//Debug.Log ("first null"+firstV+" "+firstH+" "+secondV+" "+secondH);
				
				//change the displaying position of the +second block
				
				
				blocks[secondV,secondH].transform.Translate(-1,0,0); 
				
				
				
				//change the matrix position of the block
				blocks[secondV,secondH-1]=blocks[secondV,secondH];
				//change the matrix positions of the blocks
				blocks[secondV,secondH] = null;
				
				//we need to verify gravity on the block that was over the one we just move
				if(((secondV+1)<12) && blocks[secondV+1,secondH]!=null)//we check the limit of 12 so that we don't look for a block out of index
				{
					int[] fallingBlockRight={secondV+1,secondH};
					fallingBlocks.Add(fallingBlockRight);
					fallingBlocksFrames=-.1f;
					
					//we need to block the space where this block will fall
					int [] blockedAuxDown={secondV,secondH};
					blockedBlocks.Add (blockedAuxDown);
					
					
						//we need to block the block over the block that will fall
						int [] blockedAuxUp={secondV+1,secondH};
						blockedBlocks.Add (blockedAuxUp);
					
				}
				
				//secondX-1 the one is substracted because that is now the X position of the block
				if( secondV>0 && blocks[secondV-1,secondH-1]==null )//we verify if this is a floating block
				{
					int[] arregloaux={secondV,secondH-1};
					fallingBlocks.Add(arregloaux);
					fallingBlocksFrames=-.1f;
					
					
					//we add it to the blocked blocks because it is floating now
					int [] blockedAux={secondV,secondH-1};
					blockedBlocks.Add (blockedAux);
					
					
				}
				else//in case this is not a floating block but just a grounded and lonely block we check for pairing
				{
					int[] verifyDestroyableAux={secondV,secondH-1};
					verifyDestroyable.Add(verifyDestroyableAux);
				}
				
				
				
				
			}//if first null ends
			//when the selector has block in the first square
			else if(blocks[secondV,secondH]==null)
			{
				//Debug.Log ("second null"+firstV+" "+firstH+" "+secondV+" "+secondH);
				
				//change the displaying position of the second block
				blocks[firstV,firstH].transform.Translate(1,0,0); 
				
				//change the matrix position of the block
				blocks[firstV,firstH+1]=blocks[firstV,firstH];
				
				//change the matrix positions of the blocks
				blocks[firstV,firstH] = null;
				
				//we need to verify gravity on the block that was over the one we just move if there was one block there
				if(((firstV+1)<12) && blocks[firstV+1,firstH]!=null)//we check the limit of 12 so that we don't look for a block out of index
				{
					int[] fallingBlockLeft={firstV+1,firstH};
					fallingBlocks.Add(fallingBlockLeft);
					fallingBlocksFrames=-.1f;
					
					
					//we need to block the space where this block will fall
					int [] blockedAuxDown={firstV,firstH};
					blockedBlocks.Add (blockedAuxDown);
					
					
					
						//we need to block the block over the block that will fall
						int [] blockedAuxUp={firstV+1,firstH};
						blockedBlocks.Add (blockedAuxUp);
					
					
				}
				
				if(firstV>0 && blocks[firstV-1,firstH+1]==null)//we verify if the block is floating
				{
					int[] arregloaux={firstV,firstH+1};
					fallingBlocks.Add(arregloaux);//we add the floating block to the falling queue 
					fallingBlocksFrames=-.1f;
					
					
					
					//we add it to the blocked blocks because it is floating now
					int [] blockedAux={firstV,firstH+1};
					blockedBlocks.Add (blockedAux);
					
					
				}
				else//in case this is not a floating block but just a grounded and lonely block we check for pairing
				{
					int[] verifyDestroyableAux={firstV,firstH+1};
					verifyDestroyable.Add(verifyDestroyableAux);
				}
				
			}//if second null ends
			//when the selector has both blocks on its squares
			else
			{
				
				//Debug.Log ("both exist"+firstV+" "+firstH+" "+secondV+" "+secondH);
				
				//auxiliar for saving the secoond block	to be used when its reference is lost
				Transform aux=blocks[secondV,secondH];
				
				//change the displaying positions of the blocks
				blocks[secondV,secondH].transform.Translate(-1,0,0); 
				blocks[firstV,firstH].transform.Translate(1,0,0);
				
				//change the matrix positions of the blocks
				blocks[secondV,secondH] = blocks[firstV,firstH];
				blocks[firstV,firstH]=aux;
				
				//we verify if both blocks we just moved are not over a hole, if yes we apply gravity
				if(secondV>0 && blocks[secondV-1,secondH]==null)
				{
					//we add the floating block to the falling queue 
					int[] arregloaux={secondV,secondH};
						fallingBlocks.Add(arregloaux);
					
				}
				else//because the block is not floating we add it to the verifyDestroyable ArrayList
				{
					
					int[] verifyDestroyableAux={secondV,secondH};
					verifyDestroyable.Add(verifyDestroyableAux);
					
				}
				
				
				if( firstV>0 && blocks[firstV-1,firstH]==null)
				{
					//we add the floating block to the falling queue 
						int[] arregloaux={firstV,firstH};
						fallingBlocks.Add(arregloaux);
					
				}
				else//because the block is not floating we add it to the verifyDestroyable ArrayList
				{
					
					int[] verifyDestroyableAux={firstV,firstH};
					verifyDestroyable.Add(verifyDestroyableAux);
				
				}
			}	
			
			
		}//if blocked
		blockedBlock=false;//we take out any restriction this frame could have
	}//turnBlocks ends
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	/******************************************************************************************************************/
	//HELPER FUNCTIONS START
	
	void createNewLineOfBlocks()//we use this function to create a new line of blocks
	{
		for(int cols=0;cols<=5; cols++)
		{
			int randomBlock = UnityEngine.Random.Range(1,6);
			//int randomBlock=1;
			
			
			
				
			//posiciones del player para facilitar el codigo del switch
			int posAuxX1=(int)transform.position.x+cols;
			int posAuxY1=(int)transform.position.y-1;

			
			
			//switch with random variable to create different types of blocks
					switch(randomBlock)
					{
						
					
						case 1:
									newBlocks[cols] = (Transform)Instantiate(fire, new Vector3(posAuxX1,posAuxY1,15), Quaternion.identity);
									
						break;
						case 2:
									newBlocks[cols] = (Transform)Instantiate(ground, new Vector3(posAuxX1,posAuxY1,15), Quaternion.identity);

						break;
						case 3:
									newBlocks[cols] = (Transform)Instantiate(metal, new Vector3(posAuxX1,posAuxY1,15), Quaternion.identity);

						break;
						case 4:
									newBlocks[cols] = (Transform)Instantiate(water, new Vector3(posAuxX1,posAuxY1,15), Quaternion.identity);

						break;
						case 5:
									newBlocks[cols] = (Transform)Instantiate(wind, new Vector3(posAuxX1,posAuxY1,15), Quaternion.identity);

 									 
						break;
						case 6:
									newBlocks[cols] = (Transform)Instantiate(ground, new Vector3(posAuxX1,posAuxY1,15), Quaternion.identity);

								 	
						break;
						default:
						break;
						
						
					}//switch
					
					
				
					
					
					
			
			}//for cols
	}
	
	
	
	
	
	
	
	
	
	
	

	
	
	
	void applyGravityOnFallingBlocks()
	{
		//Debug.Log ("Call up gravity on falling blocks ");
		for(int index=fallingBlocks.Count-1;index>=0;index--)
		{
			int[] fallingBlock=(int[])fallingBlocks[index];
			int row= fallingBlock[0];//vertical index (row)
			int col= fallingBlock[1];//horizontal index (column)
				
				
			//Debug.Log ("Inside for index: "+index+" row: "+row+" col: "+col+" total falling blocks "+fallingBlocks.Count);
			
			
			
			int blockUp=row;//blockUp is an auxiliar to move all the blocks over the falling block too
			
			
			do
			{
				//Debug.Log ("Inside while blockUp: "+blockUp+" col: "+col);
				//Debug.Log ("is null next block? : "+(blocks[blockUp-1,col]==null)+" blockUp "+blockUp);
				
				if(blockUp>=0 && blocks[blockUp-1,col]==null)//we check that the block under the selected block is null so that we can send it down
				{
					//Debug.Log ("Inside if blockUp-1==null: "+(blockUp-1)+" , " +col);
					//we translate the falling block in the graphics interface NOT the array itself
			 		blocks[blockUp,col].transform.Translate(0,-1f,0);
					
					
					//Now we translate the falling block inside the array
					blocks[blockUp-1,col]=blocks[blockUp,col];
					blocks[blockUp,col] = null;
					
					
					
					
				}//if blocks[,] == null
				
				
				//Debug.Log ("blockUp: " +blockUp);
				blockUp++;
			}while(blockUp>0 && blockUp<12 && blocks[blockUp,col]!=null);//we move all the blocks over the falling block, using the blockUp as a pointer to upper blocks until we don't find anymore upper blocks
			
			
			//we update the falling block integers because now it is under its previous position.	
			//Debug.Log ("updating falling block position from : "+fallingBlock[0]+" to: "+(fallingBlock[0]-1));
			fallingBlock[0]--;
			
			
			
			
			
			
			
			//we need to recheck the blockedBlocks to unlock them because they are now falling down
			foreach(int[] blocked in blockedBlocks)
			{
				
				if((blocked[0]==row&&blocked[1]==col))
				{
					//Debug.Log("unlocked "+row+ ","+col);
					blockedBlocks.Remove(blocked);
					break;
				}
							
			}
			
			
			
			if((row-1<=0)||blocks[row-2,col]!=null)//if this falling block has gotten to ground the -2 is because now the ground would be 2 blocks under its first position
				
			{
				
				//Debug.Log ("removing falling block: V: "+ fallingBlock[0]+" H: "+fallingBlock[1]+" row: "+row);
				fallingBlocks.RemoveAt (index);
				
				//we need to recheck the blockedBlocks to unlock them because they are now falling down
				foreach(int[] blocked in blockedBlocks)
				{
					//Debug.Log("unlock? "+blocked[0]+ ","+blocked[1]+" "+fallingBlock[0]+ ","+col);
					if((blocked[0]==fallingBlock[0]&&blocked[1]==col))
					{
						
						blockedBlocks.Remove(blocked);
						break;
					}
								
				}
				
				
				/********************************************/
				//add it to the checkExplodeBlocks array
				
				
				//we add the blocks that have hit ground to the verify destroyable
				int destroyUp=fallingBlock[0];//destroyUp is the pointer to add the blocks to the array
				
				while(blocks[destroyUp,col]!=null)
				{
					int[] verifyDestroyableAux={destroyUp,col};
					verifyDestroyable.Add(verifyDestroyableAux);
						Debug.Log ("Added to destroy the block: "+destroyUp+" "+col);
					destroyUp++;
					
				}
				
				/**********************************************/
			}//if row-2 != null
			
			
		}
	}
	
	
	
	
	
	void destroyBlocks()
	{
		//Debug.Log("Destroyable Blocks Queue: "+destroyableBlocks.Count);
		if(destroyableBlocks.Count>0)
		{
			int index=(destroyableBlocks.Count-1);
			int[] deathBlockPos = (int[])destroyableBlocks[index];
			int row=deathBlockPos[0];
			int col =deathBlockPos[1];
			bool blockOverIsDestroyable=false;//if the block over the current block is going to be destroyed in a future.
			
			foreach(int[] destroyableBlock in destroyableBlocks)
			{
					if((destroyableBlock[0]==(row+1)) && (destroyableBlock[1]==col))//we verify if the block is already destroyable so to not try to make it fall because it will be deleted later
					{	
						//Debug.Log("destroyable block:"+destroyableBlock[0]+" , "+destroyableBlock[1]+" over: "+(row+1)+" , "+col);
						blockOverIsDestroyable=true;
						break;
					}
					
			}
			//Debug.Log("Destroying: "+row+" "+col);
			if(blocks[row,col]!=null)//we verify there is a block in the coordinates
			{
				GameObject deathBlock=blocks[row,col].gameObject;
				Destroy(deathBlock);
				if((row+1<=11)&&(!blockOverIsDestroyable) && (blocks[row+1,col]!=null))//if the the block over is not going to be destroyed in a future and is not null then we can add it to falling blocks
				{
					//Debug.Log("added block "+(row+1)+" , "+col+" to falling blocks");
					int [] aux={row+1,col};
					fallingBlocks.Add (aux);
				}
				
					
				
			}
			blocks[row,col]=null;
			
			destroyableBlocks.RemoveAt(destroyableBlocks.Count-1);
		}//if <=0
		
	}//destroyBlocks
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	void verifyDestroyableBlocks()
	{
		ArrayList auxArray= new ArrayList();//this array will save all the blocks that we are reviewing
		
		for( int index= verifyDestroyable.Count-1;index>=0;index--)
		{
			int[] verifiableBlock=(int[])verifyDestroyable[index];
			//the verifiable block coordinates
			int row=verifiableBlock[0];
			int col=verifiableBlock[1];
			bool verifiableAdded=false;//this bool helps us to know if we have already added the principal block to the destroyable array.
			//to avoid adding it twice with the horizontal and vertical verification
			
			//Debug.Log("verifying block: "+row+" "+col);
			
			//we find out what type of block we are dealing with
			Transform block= blocks[row,col];
			string type=block.name;//the type of the block we are verifying
			
			
			
			
			
			
			
			
			
			//over search
			int overBlocks=row+1;//this is the starting pointer for blocks over
			
			if(overBlocks<=11)
			{
				Transform neighborBlock=blocks[overBlocks,col];//we get the inmediate block over the verifyable block
				if(neighborBlock!=null)
				{
					string neighborName=neighborBlock.name;
					
					while(type==neighborName)
					{
						int[] auxPosition={overBlocks,col};
						auxArray.Add(auxPosition);
						overBlocks++;
						if(overBlocks<=11)
						{
							neighborBlock=blocks[overBlocks,col];
							if(neighborBlock==null)
								break;
							neighborName=neighborBlock.name;
						}//<=11	
						else
						{
							break;
						}
					}//while over
					
				}//if neighbor!=null
			}//<=11	
				
				
				
				
				
				
				
				
			//below search
			int belowBlocks=row-1;//this is the starting pointer for blocks below
			
			if(belowBlocks>=0)
			{
				Transform neighborBlock=blocks[belowBlocks,col];//we get the inmediate block below the verifiable block
				
				if(neighborBlock!=null)
				{
					string neighborName=neighborBlock.name;
					while(type==neighborName)
					{
						int[] auxPosition={belowBlocks,col};
						auxArray.Add(auxPosition);
						belowBlocks--;
						if(belowBlocks>=0)
						{
							neighborBlock=blocks[belowBlocks,col];
							if(neighborBlock==null)
									break;
							
							neighborName=neighborBlock.name;
						}//>=0
						else
						{
							break;
						}
					}//while below
				}//if neighbor!=null
			}//>=0
			
			if(auxArray.Count>=2)//this is 2 because we are not counting the block we are verifying
			{
				//Debug.Log(auxArray.Count);
				foreach(int[] pairedBlock in auxArray)
				{
					
						destroyableBlocks.Add(pairedBlock);
						//blockedBlocks.Add(pairedBlock);//we need to block the blocks we just paired because now we cannot move them
					
				}
				
				//we add the block we are verifying
				int[] auxPosition={row,col};
				destroyableBlocks.Add(auxPosition);
				//blockedBlocks.Add(auxPosition);//we need to block the blocks we just paired because now we cannot move them
				verifiableAdded=true;
			}//if aux array =>3
			
			
			//we clear the aux array
			auxArray.Clear();
			
			
			
			
			//we verify the horizontal blocks
			//left Blocks
			int leftBlocks=col-1;//this is the starting pointer for blocks to the left
			
			if(leftBlocks>=0)
			{
				Transform neighborBlock=blocks[row,leftBlocks];//we get the inmediate block left to the verifyable block
				
				if(neighborBlock!=null)
				{
					
					string neighborName=neighborBlock.name;
					
					while(type==neighborName)
					{
						int[] auxPosition={row,leftBlocks};
						auxArray.Add(auxPosition);
						leftBlocks--;
						if(leftBlocks>=0)
						{
							neighborBlock=blocks[row,leftBlocks];
							if(neighborBlock==null)
										break;
							neighborName=neighborBlock.name;
						}//leftBlocks>=0
						else
						{
							break;
						}
					}//while left
					
				}//if neighbor!=null
			}//leftBlocks>=0
			
			//right search
			int rightBlocks=col+1;//this is the starting pointer for blocks below
			
			if(rightBlocks<=5)
			{
			
				Transform neighborBlock=blocks[row,rightBlocks];//we get the inmediate block to the right of the verifyable block
					
				if(neighborBlock!=null)
				{
					string neighborName=neighborBlock.name;
					while(type==neighborName)
					{
						int[] auxPosition={row,rightBlocks};
						auxArray.Add(auxPosition);
						rightBlocks++;
						if(rightBlocks<=5)
						{
							neighborBlock=blocks[row,rightBlocks];
							if(neighborBlock==null)
										break;
							neighborName=neighborBlock.name;
						}//<=5
						else
						{
							break;
						}
					}//while right
					
				}//if neighbor!=null
			}//<=5	
				
				
			if(auxArray.Count>=2)//this is 2 because we are not counting the block we are verifying
			{
				//Debug.Log(auxArray.Count);
				foreach(int[] pairedBlock in auxArray)
				{
					
						destroyableBlocks.Add(pairedBlock);
						//blockedBlocks.Add(pairedBlock);//we need to block the blocks we just paired because now we cannot move them
				}
				
				if(!verifiableAdded)//we add the block we are verifying if it hasn't been added
				{
					int[] auxPosition={row,col};
					destroyableBlocks.Add(auxPosition);
					//blockedBlocks.Add(auxPosition);//we need to block the blocks we just paired because now we cannot move them
				}	
			}//if aux array =>3
			
			
			//we clear the aux array
			auxArray.Clear();
			
			
			verifyDestroyable.RemoveAt(index);
			
		}//for each verifyDestroyable ends
		
	}//verifyDestroyableBlocks method ends
	
			
			
			
			
			
			
			
			
			
			
			
			
			
			
	
	
	
	void verifyRiseBlocks()	//we need to verify if player is dying or not every frame
	{
		for(int cols =0;cols<6;cols++)
		{
			if(blocks[11,cols]!=null)
				{
					dying=true;//when there is at least 1 block in the top the player starts to die
					riseBlocks=false;
					cols=7;//we escape the loop to leave the rise blocks and dying variables at their current values
				}
			else if(destroyableBlocks.Count>0 || fallingBlocks.Count>0  )
			{
				riseBlocks=false;
			}
			
			else
			{
				dying=false;//when there is no blocks in the top part the player stops dying 
				riseBlocks=true;//when there is no blocks in the top part we can start rising again
			}
		}//int cols
	}//verifyRiseBlocks
	
	
	
	void rise()
	{
		for(int i = 10; i>=0;i--)
		{
			for(int j = 0; j<6;j++)
			{
								
					if(blocks[i,j]!=null)
					{
						blocks[i,j].transform.Translate(0,riseSpeed,0);	
						
						
					}//if blocks != null
				
			}//for j
		}//for i
		
	
		
		 ///we move the new blocks-line 1 line up too
		for(int newblocks = 0; newblocks<6;newblocks++)
			{
				newBlocks[newblocks].transform.Translate(0,riseSpeed,0);	
				
			}//for newblocks
		
		selector.transform.Translate (0,riseSpeed,0);//we rise the selector along with the blocks
		newBlocksShader.transform.Translate (0,0,riseSpeed);//we rise the shader along with the new line of blocks
			
		//if the selector has gone into the following line of blocks
		if((int)selector.transform.position.y>positionVer)
		{
			//we move up one level inside the array all the blocks so that they keep their new position
			for(int i = 11; i>=0;i--)
			{
				for(int j = 0; j<6;j++)
				{
					if(blocks[i,j]!=null)//only when the array space is different than null we'll move it
					{	
						blocks[i+1,j]=blocks[i,j];
						blocks[i,j]=null;
					}
				}
			}
			
			
			
			
			
			
			//we rise the line of new blocks as well
			for(int newblocks=0;newblocks<6;newblocks++)
			{
				blocks[0,newblocks]=newBlocks[newblocks];//now the first line of the array has become the array of new blocks
				
				
				//we add all these blocks to the verifyDestroyable Array
				//Debug.Log ("Added to destroy: 0,"+newblocks);
				int[] verifyDestroyableAux={0,newblocks};//when we update the reference down in this same function we will sum 1 to the vertical position thats why we use the -1
				verifyDestroyable.Add(verifyDestroyableAux);
				
							
				newBlocks[newblocks]=null;
			}//for new blocks to first line
			
			createNewLineOfBlocks();//we create a new line of blocks
			positionVer++;//we put the cursor variable one up so that its position and variable are synced
			newBlocksShader.transform.Translate (0,0,-1);//we restart the position of the shader so that the new blocks are shaded and the others change to active
			
			
			
			
			counterNumberOfLinesCleared++;
			counterNumberOfLinesClearedText.text=(counterNumberOfLinesCleared+"");
			if(counterNumberOfLinesCleared>=numberOfLinesToClearToWin)
			{
				playerHasWon=true;
			}
			
			
			
		}//if the blocks and selector have passed to the upper row of blocks
		
		
	}//rise ends
	
	
	
	
	
	//when the deathTimer gets to 5 seconds then the player loses
	void playerDies()
	{
		playerHasLost=true;
	}
	
	
	
	
	
	
	
	
	
}//PlayerEvents1 ends
