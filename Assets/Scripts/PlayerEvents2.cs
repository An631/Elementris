using UnityEngine;
using System.Collections;

public class PlayerEvents2 : MonoBehaviour {
	
	public Transform fire;
	public Transform ground;
	public Transform metal;
	public Transform water;
	public Transform wind;
	public int numberOfStartingBlocks;
	public bool checkGravityFlag;
	
	Transform[,] blocks = new Transform[12,6];	
	
	
	
	// Use this for initialization
	void Start () {
		
	int countBlocks=0;
	

	for(int rows=0;rows<=11; rows++)
	{
		
		for(int cols=0;cols<=5; cols++)
		{
			int randomBlock = UnityEngine.Random.Range(0,6);
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
		
	
		checkGravityFlag=true;
	}//start()
	
	// Update is called once per frame
	void Update () {
	
	if(Input.GetButtonDown("P2Switch")){
			turnBlocks ((int)transform.FindChild("selector").position.y,(int)transform.FindChild("selector").position.x-10,(int)transform.FindChild("selector").position.y,(int)transform.FindChild("selector").position.x-9);
		
	}
	
	if(checkGravityFlag)
	checkGravity();
	
	}
	
	
	void turnBlocks(int firstV,int firstH, int secondV,int secondH)
	{
		
		
		
		//when the selector has no blocks on either side
		if(blocks[secondV,secondH]==null && blocks[firstV,firstH]==null )
		{
			Debug.Log ("both null"+firstV+" "+firstH+" "+secondV+" "+secondH);
			
		}//if !=null ends
		//when the selector has block en the second square
		else if(blocks[firstV,firstH]==null)
		{
			Debug.Log ("first null"+firstV+" "+firstH+" "+secondV+" "+secondH);
			
			//change the displaying position of the second block
			blocks[secondV,secondH].transform.Translate(-1,0,0); 
			
			//cahnge the matrix position of the block
			blocks[secondV,secondH-1]=blocks[secondV,secondH] ;
			//cahnge the matrix positions of the blocks
			blocks[secondV,secondH] = null;
			if(blocks[secondV-1,secondH-1]==null && secondV>0)
				checkGravityFlag=true;
			
		}//if first null ends
		//when the selector has block en the first square
		else if(blocks[secondV,secondH]==null)
		{
			Debug.Log ("second null"+firstV+" "+firstH+" "+secondV+" "+secondH);
			
			//change the displaying position of the second block
			blocks[firstV,firstH].transform.Translate(1,0,0); 
			
			//cahnge the matrix position of the block
			blocks[firstV,firstH+1]=blocks[firstV,firstH] ;
			//cahnge the matrix positions of the blocks
			blocks[firstV,firstH] = null;
			
			if(blocks[firstV-1,firstH+1]==null && firstV>0)
				checkGravityFlag=true;
			
		}//if second null ends
		//when the selector has both blocks on its squares
		else
		{
			
			Debug.Log ("both exist"+firstV+" "+firstH+" "+secondV+" "+secondH);
			
			//auxiliar for saving the secoond block	to be used when its reference is lost
			Transform aux=blocks[secondV,secondH];
			
			//change the displaying positions of the blocks
			blocks[secondV,secondH].transform.Translate(-1,0,0); 
			blocks[firstV,firstH].transform.Translate(1,0,0);
			
			//cahnge the matrix positions of the blocks
			blocks[secondV,secondH] = blocks[firstV,firstH];
			blocks[firstV,firstH]=aux;
		}	
		
		
		
	
	}//turnBlocks ends
	
	
	void checkGravity()
	{
		
		for(int i = 1; i<12;i++)
		{
			for(int j = 0; j<6;j++)
			{
				Debug.Log ("gravity check: "+i+" "+j);
				
				if(blocks[i-1,j]==null && blocks[i,j]!=null)
				{
					
					
					//we move the array reference of the block 
					blocks[i-1,j]=blocks[i,j];
					blocks[i,j]=null;
					
					//we move the displaying block down
					blocks[i-1,j].transform.Translate(0,-1,0);
					
				}
			}	
		}
		checkGravityFlag=false;
	}
	
}
