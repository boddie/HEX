using UnityEngine;
using System.Collections;
using Assets.CitadelBuilder;

public class Spawnscript : Photon.MonoBehaviour
{
    public Transform playerPrefab;
	private PersistentData persistence;
	
	private Transform playerPos1, playerPos2, playerPos3, playerPos4;
    private Transform citadel1, citadel2, citadel3, citadel4;

    private GameObject Fog;

    void Start()
    {
		persistence = GameObject.Find("Persistence").GetComponent<PersistentData>();

		playerPos1 = GameObject.Find("Position1").GetComponent<Transform>();
		playerPos2 = GameObject.Find("Position2").GetComponent<Transform>();
		playerPos3 = GameObject.Find("Position3").GetComponent<Transform>();
		playerPos4 = GameObject.Find("Position4").GetComponent<Transform>();

        citadel1 = GameObject.Find("Citadel1").GetComponent<Transform>();
        citadel2 = GameObject.Find("Citadel2").GetComponent<Transform>();
        citadel3 = GameObject.Find("Citadel3").GetComponent<Transform>();
        citadel4 = GameObject.Find("Citadel4").GetComponent<Transform>();

        SpawnPlayer();
		SpawnCitadel();
		PhotonNetwork.automaticallySyncScene = false;
    }

    void SpawnPlayer()
    {
		if(persistence.arenaPosition == 0)
		{
			GameObject playerObj = PhotonNetwork.Instantiate(persistence.CurrentPlayer.Guild.Prefab, playerPos1.position, playerPos1.rotation, 0);
            playerObj.transform.FindChild("EnemyTracker").renderer.material.SetColor("_Color", Color.black);
            Fog = GameObject.Find("FogOfWar1");
            for(int i = 0; i < Fog.transform.childCount; i++)
            {
                Fog.transform.FindChild(i.ToString()).GetComponent<ParticleSystem>().enableEmission = false;
            }
		}
		else if(persistence.arenaPosition == 1)
		{
            GameObject playerObj = PhotonNetwork.Instantiate(persistence.CurrentPlayer.Guild.Prefab, playerPos2.position, playerPos2.rotation, 0);
            playerObj.transform.FindChild("EnemyTracker").renderer.material.SetColor("_Color", Color.black);
            Fog = GameObject.Find("FogOfWar2");
            for (int i = 0; i < Fog.transform.childCount; i++)
            {
                Fog.transform.FindChild(i.ToString()).GetComponent<ParticleSystem>().enableEmission = false;
            }
		}
		else if(persistence.arenaPosition == 2)
		{
            GameObject playerObj = PhotonNetwork.Instantiate(persistence.CurrentPlayer.Guild.Prefab, playerPos3.position, playerPos3.rotation, 0);
            playerObj.transform.FindChild("EnemyTracker").renderer.material.SetColor("_Color", Color.black);
            Fog = GameObject.Find("FogOfWar3");
            for (int i = 0; i < Fog.transform.childCount; i++)
            {
                Fog.transform.FindChild(i.ToString()).GetComponent<ParticleSystem>().enableEmission = false;
            }
		}
		else
		{
            GameObject playerObj = PhotonNetwork.Instantiate(persistence.CurrentPlayer.Guild.Prefab, playerPos4.position, playerPos4.rotation, 0);
            playerObj.transform.FindChild("EnemyTracker").renderer.material.SetColor("_Color", Color.black);
            Fog = GameObject.Find("FogOfWar4");
            for (int i = 0; i < Fog.transform.childCount; i++)
            {
                Fog.transform.FindChild(i.ToString()).GetComponent<ParticleSystem>().enableEmission = false;
            }
		}
    }
	
	void SpawnCitadel()
	{
        persistence.Blocks = new System.Collections.Generic.Dictionary<int, CitadelDamage>();
        float startx = 0, incx = 0, startz = 0, starty = 0, incz = 0, currx = 0, currz = 0;
		int count = 0;
		Quaternion r = Quaternion.identity;
		if(persistence.arenaPosition == 0)
		{
            startx = citadel1.position.x;
            startz = citadel1.position.z;
            starty = citadel1.position.y;

			incx = 4.175f;
			incz = -4.175f;
            r = citadel1.rotation;
		}
		else if(persistence.arenaPosition == 1)
		{
            startx = citadel2.position.x;
            startz = citadel2.position.z;
            starty = citadel2.position.y;

			incx = 4.175f;
			incz = 4.175f;
            r = citadel2.rotation;
		}
		else if(persistence.arenaPosition == 2)
		{
            startx = citadel3.position.x;
            startz = citadel3.position.z;
            starty = citadel3.position.y;

			incx = -4.175f;
			incz = 4.175f;
            r = citadel3.rotation;
		}
		else 
		{
            startx = citadel4.position.x;
            startz = citadel4.position.z;
            starty = citadel4.position.y;

			incx = -4.175f;
			incz = -4.175f;
            r = citadel4.rotation;
		}
		Debug.Log("Citadel Squares: " + Citadel.NUM_SQUARES);
		
		if(persistence.arenaPosition == 1 || persistence.arenaPosition == 3)
			currx = startx;
		else
			currz = startz;
		
		for(int i = 0; i < Citadel.NUM_SQUARES; i++)
		{
			if(persistence.arenaPosition == 1 || persistence.arenaPosition == 3)
				currz = startz;
			else
				currx = startx;
			
			
			for(int j = 0; j < Citadel.NUM_SQUARES; j++)
			{
				if(persistence.Citadels [persistence.CurrentCitadel] == null)
					Debug.Log("Default Citadel Null");
				CitadelBlock block = persistence.Citadels[persistence.CurrentCitadel].Blocks[i,j];
				if(block != null)
				{
					GameObject blk = PhotonNetwork.Instantiate(block.TypeName, new Vector3(currx, starty, currz), r, 0, new object[] { block.Mat.Serialize() });
					persistence.Blocks.Add(blk.GetPhotonView().viewID, new CitadelDamage(0, null, null));
                    if (blk == null)
                    {
                        Debug.Log("Created block is null: " + block.TypeName);
                    }
                    block.EditGameObject(blk);
                    switch (persistence.arenaPosition)
                    {
                        case 0:
                            blk.transform.Rotate(0, 0, 90, Space.Self);
                            break;
                        case 2:
                            blk.transform.Rotate(0, 0, -90, Space.Self);
                            break;
                        case 3:
                            blk.transform.Rotate(0, 0, 180, Space.Self);
                            break;
                    }
					if(block.TypeName == "Altar")
					{
						blk.transform.localScale = new Vector3(0.4f, 0.4f, 1.3f);
						blk.transform.rotation = Quaternion.AngleAxis(-90, Vector3.right);
					}
                    else if (block.TypeName == "Trap")
                    {
                        blk.transform.localScale = new Vector3(8.073f, 8.073f, 4.35f);
                    }
                    else
                    {
                        blk.transform.localScale = new Vector3(4.35f / 2f, 4.35f / 2f, 8.7f / 2f);
                    }
					count++;
				}
				if(persistence.arenaPosition == 1 || persistence.arenaPosition == 3)
					currz += incz;
				else
					currx += incx;
			}
			if(persistence.arenaPosition == 1 || persistence.arenaPosition == 3)
				currx += incx;
			else
				currz += incz;
		}
	}
}
					
					
